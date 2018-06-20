using UnityEngine;
using System.Collections.Generic;
using System.Collections;
namespace NGame
{
    [ObjectSystem]
    public class StaticGridComponentAwakeSystem : AwakeSystem<StaticGridComponent, Vector3, Vector2Int, Vector2Int>
    {
        public override void Awake(StaticGridComponent self, Vector3 entity, Vector2Int cellSize, Vector2Int rowAndCol)
        {
            self.Awake(entity, cellSize, rowAndCol);
        }
    }
    /// <summary>
    /// 静态的网格
    /// </summary>
    public class StaticGridComponent : Component
    {
        private Vector2Int cellSize;
        private Vector2Int rowAndCol;
        private Vector2Int entityPos;

        private Dictionary<Vector2Int, Vector2Int> keyPosition = new Dictionary<Vector2Int, Vector2Int>();
        private Dictionary<Vector2Int, Vector2Int> positionKey = new Dictionary<Vector2Int, Vector2Int>();
        public void Awake(Vector3 entity, Vector2Int cellSize, Vector2Int rowAndCol)
        {
            this.cellSize = cellSize;
            this.rowAndCol = rowAndCol;
            this.entityPos = new Vector2Int((int)entity.x, (int)entity.y);
            this.GeneraterGrid();
        }

        public Vector2Int CellSize
        {
            get
            {
                return cellSize;
            }
        }
        private void GeneraterGrid()
        {
            keyPosition.Clear();
            Vector2Int key = Vector2Int.Zero;
            for (int i = 0; i < rowAndCol.x; i++)
            {
                for (int j = 0; j < rowAndCol.y; j++)
                {
                    key.x = i;
                    key.y = j;
                    keyPosition[key] = new Vector2Int(entityPos.x - (rowAndCol.y / 2 - j) * cellSize.x, entityPos.y - (rowAndCol.x / 2 - i) * cellSize.y);
                    positionKey[keyPosition[key]] = key;
                    //Log.Debug("key: " + key + " position： " + keyPosition[key]);
                }
            }
        }

        public Vector2Int GetKeyInGrid(Vector2Int pos)
        {
            Vector2Int key;
            if (!positionKey.TryGetValue(pos, out key))
            {
                Log.Error("没有对应位置的key pos : " + pos);
            }
            return key;
        }

        public Vector3 GetPosInGrid(Vector2Int key,out Vector2Int rawPosition)
        {
            Vector2Int position;
            if (!keyPosition.TryGetValue(key, out position))
            {
                Log.Error("没有对应key的位置 key : " + key);
            }
            rawPosition = position;
            return new Vector3(position.x, position.y);
        }

        public Vector3 GetSpawPos(int col, int depth ,out Vector2Int rawPos)
        {
            rawPos.x = entityPos.x - (rowAndCol.y / 2 - col) * cellSize.x;
            rawPos.y = entityPos.y - (rowAndCol.x / 2 + depth) * cellSize.y;
            Vector3 pos = new Vector3(rawPos.x, rawPos.y);
            //Log.Debug("pso :" + pos);
            return pos;
        }

    }

}
