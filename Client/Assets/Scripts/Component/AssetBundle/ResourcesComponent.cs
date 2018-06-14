using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NGame
{
    public class ABInfo : Component
    {
        private int refCount;

        private string m_Name;
        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public int RefCount
        {
            get
            {
                return this.refCount;
            }
            set
            {
                //Log.Debug($"{this.Name} refcount: {value}");
                this.refCount = value;
            }
        }

        private AssetBundle m_AssetBundle;
        public AssetBundle assetBundle
        {
            get
            {
                return m_AssetBundle;
            }
        }

        public ABInfo(string name, AssetBundle ab)
        {
            this.m_Name = name;
            this.m_AssetBundle = ab;
            this.RefCount = 1;
            //Log.Debug($"load assetbundle: {this.Name}");
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            //Log.Debug($"destroy assetbundle: {this.Name}");

            if(this.assetBundle)
            {
                this.assetBundle.Unload(true);
            }
        }
    }

    public class ResourcesComponent : Component
    {
        public static AssetBundleManifest assetBundleManifestObject { get; set; }

        private readonly Dictionary<string, UnityEngine.Object> resourceCache = new Dictionary<string, UnityEngine.Object>();

        private readonly Dictionary<string, ABInfo> bundles = new Dictionary<string, ABInfo>();

        // lru缓存队列
        private readonly QueueDictionary<string, ABInfo> cacheDictionary = new QueueDictionary<string, ABInfo>();

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            foreach (var abInfo in this.bundles)
            {
                if(abInfo.Value != null && abInfo.Value.assetBundle)
                {
                    abInfo.Value.assetBundle.Unload(true);
                }
            }

            this.bundles.Clear();
            this.cacheDictionary.Clear();
            this.resourceCache.Clear();
        }

        public UnityEngine.Object GetAsset(string bundleName, string prefab)
        {
            string path = string.Format("{0}/{1}",bundleName,prefab).ToLower();

            UnityEngine.Object resource = null;
            if (!this.resourceCache.TryGetValue(path, out resource))
            {
                throw new Exception("not found asset: "+path);
            }

            if (resource == null)
            {
                throw new Exception("asset type error, path: "+path);
            }
            return resource;
        }

        public void UnloadBundle(string assetBundleName)
        {
            assetBundleName = assetBundleName.ToLower();

            string[] dependencies = ResourcesHelper.GetSortedDependencies(assetBundleName);

            //Log.Debug($"-----------dep unload {assetBundleName} dep: {dependencies.ToList().ListToString()}");
            foreach (string dependency in dependencies)
            {
                this.UnloadOneBundle(dependency);
            }
        }

        private void UnloadOneBundle(string assetBundleName)
        {
            assetBundleName = assetBundleName.ToLower();

            //Log.Debug($"unload bundle {assetBundleName}");
            ABInfo abInfo;
            if (!this.bundles.TryGetValue(assetBundleName, out abInfo))
            {
                throw new Exception("not found assetBundle: "+assetBundleName);
            }

            --abInfo.RefCount;
            if (abInfo.RefCount > 0)
            {
                return;
            }


            this.bundles.Remove(assetBundleName);

            // 缓存10个包
            this.cacheDictionary.Enqueue(assetBundleName, abInfo);
            if (this.cacheDictionary.Count > 10)
            {
                abInfo = this.cacheDictionary[this.cacheDictionary.FirstKey];
                this.cacheDictionary.Dequeue();
                abInfo.Dispose();
            }
            //Log.Debug($"cache count: {this.cacheDictionary.Count}");
        }

        /// <summary>
        /// 同步加载assetbundle
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <returns></returns>
        public void LoadBundle(string assetBundleName)
        {
            assetBundleName = assetBundleName.ToLower();
            string[] dependencies = ResourcesHelper.GetSortedDependencies(assetBundleName);
            Log.Debug(string.Format("-----------dep load {0} dep: {1}", assetBundleName,dependencies.ToList().ListToString()));
            foreach (string dependency in dependencies)
            {
                if (string.IsNullOrEmpty(dependency))
                {
                    continue;
                }
                this.LoadOneBundle(dependency);
            }
        }

        public void LoadOneBundle(string assetBundleName)
        {
            ABInfo abInfo;
            if (this.bundles.TryGetValue(assetBundleName, out abInfo))
            {
                ++abInfo.RefCount;
                return;
            }


            if (this.cacheDictionary.ContainsKey(assetBundleName))
            {
                abInfo = this.cacheDictionary[assetBundleName];
                ++abInfo.RefCount;
                this.bundles[assetBundleName] = abInfo;
                this.cacheDictionary.Remove(assetBundleName);
                return;
            }


            string[] realPath = null;
#if UNITY_EDITOR
            realPath = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
            foreach (string s in realPath)
            {
                string assetName = Path.GetFileNameWithoutExtension(s);
                string path = string.Format("{0}/{1}",assetBundleName,assetName).ToLower();
                UnityEngine.Object resource = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(s);
                this.resourceCache[path] = resource;
            }

            this.bundles[assetBundleName] = new ABInfo(assetBundleName, null);
#endif
            return;
        }



        public string DebugString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (ABInfo abInfo in this.bundles.Values)
            {
                sb.Append(string.Format("{0}:{1}\n",abInfo.Name,abInfo.RefCount));
            }
            return sb.ToString();
        }
    }
}