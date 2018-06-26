using UnityEngine;
using Mono.Xml;
using System.Security;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace NGame
{
    public class MonoXmlParse
    {
        private SecurityParser m_Sp;

        public MonoXmlParse()
        {
            m_Sp = new SecurityParser();
        }

        public SecurityElement LoadXml(string xml)
        {
            m_Sp.LoadXml(xml);
            return m_Sp.ToXml();
        }
        public SecurityElement LoadXml(System.IO.StreamReader reader)
        {
            m_Sp.LoadXml(reader);
            return m_Sp.ToXml();
        }
        

    }
    public static class ParseExt
    {
        public static int ParseInt(this SecurityElement se, string attr)
        {
            int result;
            if (!int.TryParse(se.Attribute(attr), out result))
            {
                Log.Debug("解析属性出错： " + attr);
            }
            return result;
        }
        public static float ParseFloat(this SecurityElement se, string attr)
        {
            float result;
            if (!float.TryParse(se.Attribute(attr), out result))
            {
                Log.Debug("解析属性出错： " + attr);
            }
            return result;
        }

        public static string ParseAttribute(this SecurityElement se, string attr)
        {
            return se.Attribute(attr);
        }
        public static bool HasAttribute(this SecurityElement se, string attr)
        {
            string result =  se.Attribute(attr);
            if (string.IsNullOrEmpty(result))
                return false;
            return true;
        }

        public static Vector2Int ParseVector2Int(this SecurityElement se, string attr)
        {
            Vector2Int result = Vector2Int.Zero; ;
            string args = se.Attribute(attr);
            string[] items = args.Split(',');
            if(!int.TryParse(items[0],out result.x)|| !int.TryParse(items[1], out result.y))
            {
                Log.Debug("解析属性出错： " + attr);
            }
            return result;
        }

        public static List<SecurityElement> TravelChildren(this SecurityElement se)
        {
            List<SecurityElement> securityElements = new List<SecurityElement>();
            foreach(SecurityElement child in se.Children)
            {
                securityElements.Add(child);
            }
            return securityElements;
        }
        public static SecurityElement SearchChildByTag(this SecurityElement se ,string tag)
        {
            foreach(SecurityElement child in se.Children)
            {
                if(string.Equals(child.Tag,tag))
                {
                    return child;
                }
            }
            Log.Debug(string.Format("{0}没有这个{1}Tag", se.Tag, tag));
            return null;
        }
        public static int[] ParseInts(this SecurityElement se, string attr)
        {
            string args = se.Attribute(attr);
            string[] items = args.Split(',');
            int[] result = new int[items.Length];
            for(int i =0;i<result.Length;i++)
            {
                if(!int.TryParse(items[i],out result[i]))
                {
                    Log.Debug(string.Format("{0}解析{1}属性出错", se.Tag, attr));
                }
            }
            return result;
        }
    }

}