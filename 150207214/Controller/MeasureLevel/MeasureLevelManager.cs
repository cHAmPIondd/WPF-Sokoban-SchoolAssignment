using _150207214.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
namespace _150207214.Controller
{
    class MeasureLevelManager
    {
        private MapXmlReader m_MapXmlReader;
        public MeasureLevelManager(MapXmlReader mapXmlReader)
        {
            m_MapXmlReader = mapXmlReader;
        }
        public Stack<MapState> MeasureLevel(Label label)
        {
            //创建寻路管理器
            AStarFindPathManager aStarFindPathManager = new AStarFindPathManager(m_MapXmlReader);
            MapState.s_AStarFindPathManager = aStarFindPathManager;
            //创建targetList
            List<Vector2Int> targetList=new List<Vector2Int>();
            for (int i = 0; i < m_MapXmlReader.CurrentMapHeight; i++)
            {
                for (int j = 0; j < m_MapXmlReader.CurrentMapWidth; j++)
                {
                    if ((m_MapXmlReader.GetBlock(i, j).Type & BlockType.TARGET)!=0)
                    {
                        targetList.Add(new Vector2Int(j,i));
                    }
                }
            }
            //创建首个状态
            MapState currentState = new MapState(m_MapXmlReader.CurrentHeroPos);
            int boxNum = 0;
            for(int i=0;i<m_MapXmlReader.CurrentMapHeight;i++)
	        {
		        for(int j=0;j<m_MapXmlReader.CurrentMapWidth;j++)
                {
                    if((m_MapXmlReader.GetBlock(i,j).Type&BlockType.BOX)!=0)
                    {
                        currentState.AddBox(new Vector2Int(j,i));
                        boxNum++;
                    }
                }
            }
            if(boxNum!=targetList.Count)
            {//箱子数和目标数不同
                MessageBox.Show("箱子数和目标数不同"+"\n箱子:"+boxNum+"   目标:"+targetList.Count);
                return null;
            }
            DoubleDictionary<MapState> doubleDictionary = new DoubleDictionary<MapState>();
            PriorityQueue<MapState> priorityQueue = new PriorityQueue<MapState>();
            priorityQueue.MinHeapInsert(currentState);
            doubleDictionary.Add(currentState);
            Vector2Int[] neighborPos ={new Vector2Int(1,0),
                                     new Vector2Int(-1,0),
                                     new Vector2Int(0,1),
                                     new Vector2Int(0,-1)};
            DateTime datetime = DateTime.Now;
            while(true)
            {
                DoEvents();
                label.Content = "AllState:" + doubleDictionary.Count + "\n" + "CurrentState:" + priorityQueue.Count + "\nTime:" + (DateTime.Now - datetime);
                
                if (priorityQueue.Count == 1)
                {//找不到解法
                    MessageBox.Show("找不到解法");
                    return null;
                }
                currentState = priorityQueue.HeapExtractMin();
                
                //判断是否胜利
                if (currentState.IsVictory(targetList))
                {
                    Stack<MapState> path = new Stack<MapState>();
                    while (currentState.Parent != null)
                    {
                        path.Push(currentState);
                        currentState = currentState.Parent;
                    }
                    return path;
                }

                for (int i = 0; i < currentState.BoxList.Count;i++ )
                {
                    foreach (Vector2Int v in neighborPos)
                    {
                        if (aStarFindPathManager.FindPath(currentState.HeroPos, currentState.BoxList[i] - v, currentState.BoxList) != null)
                        {
                            Vector2Int v2 = currentState.BoxList[i]+v;
                            if (IsBlank(v2, currentState.BoxList))
                            {
                                //创建新的MapState
                                MapState newState = new MapState(currentState.BoxList[i]);
                                newState.AddBox(v2);
                                for (int j = 0; j < currentState.BoxList.Count;j++)
                                {
                                    if(j!=i)
                                    {
                                        newState.AddBox(currentState.BoxList[j]);
                                    }
                                }
                                newState.HadCost = currentState.HadCost+1;
                                newState.Parent = currentState;
                                newState.MoveStep = currentState.BoxList[i]+"->"+v2;
                                //判断是否有箱子在角落且不在目标点
                                bool isCornerAndNotTarget = false;
                                foreach(var temp in newState.BoxList)
                                {
                                    if(IsCorner(temp))
                                    {
                                        if(!targetList.Contains(temp))
                                        {
                                            isCornerAndNotTarget = true;
                                            break;
                                        }
                                    }
                                }
                                if (isCornerAndNotTarget)
                                    continue;
                                //判断是否已经有相同的状态
                                bool hadContains = false;
                                var dictionary = doubleDictionary.Get(newState.GetHashCode());
                                List<MapState> tempList = null;
                                if (dictionary != null)
                                {
                                    if (dictionary.ContainsKey(newState.GetHashCode2()))
                                        tempList = dictionary[newState.GetHashCode2()];
                                }
                                if (tempList != null)
                                {
                                    foreach (MapState temp in tempList)
                                    {
                                        if (temp.Equals(newState))
                                        {//有相同
                                            hadContains = true;
                                            if (temp.HadCost > newState.HadCost)
                                            {
                                                newState.CalCost(targetList);
                                                doubleDictionary.Remove(temp);
                                                doubleDictionary.Add(newState);
                                                priorityQueue.HeapDecreaseKey(temp, newState);
                                            }
                                            break;
                                        }
                                    }
                                }
                                if(!hadContains)
                                {//没相同
                                    newState.CalCost(targetList);
                                    priorityQueue.MinHeapInsert(newState);
                                    doubleDictionary.Add(newState);
                                }
                            }
                        }
                    }
                }
            }
        }
        private bool IsBlank(Vector2Int v2,List<Vector2Int> boxList)
        {
            if (v2.X < 0 || v2.Y < 0 || v2.X >= m_MapXmlReader.CurrentMapWidth || v2.Y >= m_MapXmlReader.CurrentMapHeight)
            {
                return false;
            }
            foreach(var temp in boxList)
            {
                if (temp == v2)
                    return false;
            }
            if (m_MapXmlReader.GetBlock(v2.Y, v2.X).Type == BlockType.WALL)
                return false;
            return true;
        }
        private bool IsCorner(Vector2Int v2)
        {
            Vector2Int[] neighborPos ={new Vector2Int(1,0),
                                     new Vector2Int(0,1),
                                     new Vector2Int(-1,0),
                                     new Vector2Int(0,-1)};
            for (int i = 0; i < neighborPos.Length-1;i++ )
            {
                Vector2Int v = v2 + neighborPos[i];
                if (v.X < 0 || v.Y < 0 || v.X >= m_MapXmlReader.CurrentMapWidth || v.Y >= m_MapXmlReader.CurrentMapHeight
                ||m_MapXmlReader.GetBlock(v.Y,v.X).Type==BlockType.WALL)
                {
                    Vector2Int v3=v2+neighborPos[(i+1)%4];
                    if (v3.X < 0 || v3.Y < 0 || v3.X >= m_MapXmlReader.CurrentMapWidth || v3.Y >= m_MapXmlReader.CurrentMapHeight)
                    {
                        return true;
                    }
                    if(m_MapXmlReader.GetBlock(v3.Y,v3.X).Type==BlockType.WALL)
                        return true;
                    v3 = v2 + neighborPos[(i + 3) % 4];
                    if (v3.X < 0 || v3.Y < 0 || v3.X >= m_MapXmlReader.CurrentMapWidth || v3.Y >= m_MapXmlReader.CurrentMapHeight)
                    {
                        return true;
                    }
                    if (m_MapXmlReader.GetBlock(v3.Y, v3.X).Type == BlockType.WALL)
                        return true;
                }
            }
            return false;
        }
        public void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(delegate(object f)
                {
                    ((DispatcherFrame)f).Continue = false;

                    return null;
                }
                    ), frame);
            Dispatcher.PushFrame(frame);
        }
    }
}
