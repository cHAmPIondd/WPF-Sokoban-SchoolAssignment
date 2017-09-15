using _150207214.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _150207214.Controller
{
    class AStarFindPathManager
    {
        private FindPathNode[,] m_FindPathMap;
        public AStarFindPathManager(MapXmlReader mapXmlReader)
        {
            //构建FindPathMap
            m_FindPathMap = new FindPathNode[mapXmlReader.CurrentMapHeight, mapXmlReader.CurrentMapWidth];
            for (int i = 0; i < mapXmlReader.CurrentMapHeight; i++)
            {
                for (int j = 0; j < mapXmlReader.CurrentMapWidth; j++)
                {
                    BlockType type = mapXmlReader.GetBlock(i, j).MaxType;
                    if (type == BlockType.WALL)
                    {
                        m_FindPathMap[i, j] = new FindPathNode(new Vector2Int(j, i), false);
                    }
                    else
                    {
                        m_FindPathMap[i, j] = new FindPathNode(new Vector2Int(j, i), true);
                    }
                }
            }
        }
        
        public Stack<Vector2Int> FindPath(Vector2Int start,Vector2Int target,List<Vector2Int> boxList)
        {  
            //判断target是否在地图内
            if(target.X<0||target.Y<0||target.X>=m_FindPathMap.GetLength(1)||target.Y>=m_FindPathMap.GetLength(0))
            {
                return null;
            }
            //增加障碍物
            foreach (var temp in boxList)
	        {
                m_FindPathMap[temp.Y, temp.X].IsPassable = false;
	        }

            //创建priorityQueue
            List<FindPathNode> priorityQueue = new List<FindPathNode>();
            FindPathNode currentNode;
            priorityQueue.Add(m_FindPathMap[start.Y, start.X]);
            m_FindPathMap[start.Y, start.X].HadCost = 0;
            Vector2Int[] neighborPos ={new Vector2Int(1,0),
                                     new Vector2Int(-1,0),
                                     new Vector2Int(0,1),
                                     new Vector2Int(0,-1)};
            //循环找路
            while (true)
            {
                if (priorityQueue.Count == 0)
                {//找不到路
                    Reset(boxList);
                    return null;
                }
                currentNode = priorityQueue[0];
                priorityQueue.RemoveAt(0);
                if (currentNode.Pos.X == target.X && currentNode.Pos.Y== target.Y)
                {
                    Stack<Vector2Int> path = new Stack<Vector2Int>();
                    while (currentNode.Parent != null)
                    {
                        path.Push(currentNode.Pos);
                        currentNode = currentNode.Parent;
                    }
                    Reset(boxList);
                    return path;
                }
                foreach (Vector2Int v in neighborPos)
                {
                    Vector2Int v2 = v + currentNode.Pos;
                    if (v2.Y < m_FindPathMap.GetLength(0) && v2.X < m_FindPathMap.GetLength(1) && v2.Y >= 0 && v2.X >= 0)
                    {
                        if (m_FindPathMap[v2.Y, v2.X].IsPassable)
                        {
                            if (m_FindPathMap[v2.Y, v2.X].HadCost > currentNode.HadCost + 1)
                            {
                                m_FindPathMap[v2.Y, v2.X].Parent = currentNode;
                                m_FindPathMap[v2.Y, v2.X].HadCost = currentNode.HadCost + 1;
                                priorityQueue.Add(m_FindPathMap[v2.Y, v2.X]);
                                m_FindPathMap[v2.Y, v2.X].CalCost(target);
                            }
                        }
                    }
                }
                priorityQueue.Sort((x, y) => { return x.AllCost.CompareTo(y.AllCost); });
            }
        }
        /// <summary>
        /// 重置m_FindPathMap地图
        /// </summary>
        private void Reset(List<Vector2Int> boxList)
        {
            foreach (var temp in boxList)
            {
                m_FindPathMap[temp.Y, temp.X].IsPassable = true;
            }
            for(int i=0;i<m_FindPathMap.GetLength(0);i++)
            {
                for(int j=0;j<m_FindPathMap.GetLength(1);j++)
                {
                    m_FindPathMap[i, j].Parent = null;
                    m_FindPathMap[i, j].HadCost = int.MaxValue;
                }
            }
        }
    }
}
