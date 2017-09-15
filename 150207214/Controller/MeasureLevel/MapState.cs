using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _150207214.Controller
{
    class MapState : IEquatable<MapState>, DoubleHashCodeInterface,IComparable<MapState>
    {
        public static AStarFindPathManager s_AStarFindPathManager;
        public MapState Parent { get; set; }
        public int HadCost { get; set; }
        public int AllCost { get; set; }
        public List<Vector2Int> BoxList { get; set; }
        public Vector2Int HeroPos { get; set; }
        public string MoveStep { get; set; }
        public MapState(Vector2Int heroPos)
        {
            HeroPos = heroPos;
            BoxList = new List<Vector2Int>();
        }
        public void AddBox(Vector2Int v2)
        {
            BoxList.Add(v2);
        }
        public void CalCost(List<Vector2Int> target)
        {
            if (target.Count != BoxList.Count)
                throw new Exception("Error");


            int leastCost = int.MaxValue;
            Random ran=new Random();
            for (int k = 0; k < BoxList.Count; k++)
            {
                //随机交换
                int one = ran.Next(0, BoxList.Count);
                int two = ran.Next(0, BoxList.Count);
                Vector2Int temp = BoxList[one];
                BoxList[one] = BoxList[two];
                BoxList[two] = temp;

                int curLeastCost = 0;
                List<Vector2Int> tempArray = new List<Vector2Int>();
                for (int i = 0; i < target.Count; i++)
                {
                    tempArray.Add(target[i]);
                }
                for (int j = 0; j < BoxList.Count; j++)
                {
                    int best = int.MaxValue;
                    int num = 0;

                    for (int i = 0; i < tempArray.Count; i++)
                    {
                        //int cur = Math.Abs(tempArray[i].X - BoxList[j].X) + Math.Abs(tempArray[i].Y - BoxList[j].Y);
                        int cur = s_AStarFindPathManager.FindPath(BoxList[j], tempArray[i], new List<Vector2Int>()).Count;
                        if (cur < best)
                        {
                            best = cur;
                            num = i;
                        }
                    }
                    tempArray.RemoveAt(num);
                    curLeastCost += best;
                }
                if (leastCost > curLeastCost)
                    leastCost = curLeastCost;
            }
            
            AllCost = HadCost + leastCost;
        }
        public bool IsVictory(List<Vector2Int> targetList)
        {
            foreach (var temp in targetList)
            {
                bool has = false;
                foreach (var temp2 in BoxList)
                {
                    if (temp == temp2)
                    {
                        has = true;
                        break;
                    }
                }
                if (!has)
                {
                    return false;
                }
            }
            return true;
        }
        public bool Equals(MapState other)
        {
            
            if (BoxList.Count != other.BoxList.Count)
            {
                return false;
            }
            foreach (var item in BoxList)
            {
                if (!other.BoxList.Contains(item))
                    return false;
            }
            foreach (var item in other.BoxList)
            {
                if (!BoxList.Contains(item))
                    return false;
            }
            if (s_AStarFindPathManager.FindPath(this.HeroPos, other.HeroPos, BoxList) == null)
            {
                return false;
            }
            return true;
        }
        public int GetHashCode()
        {
            int hashcode=0;
            foreach(var box in BoxList)
            {
                hashcode += box.X *box.Y+box.X*7+box.Y*23;
            }
            return hashcode;
        }
        public int GetHashCode2()
        {
            int hashcode = 0;
            foreach (var box in BoxList)
            {
                hashcode += box.X * box.Y + box.X * 23 + box.Y * 7;
            }
            return hashcode;
        }

        public int CompareTo(MapState other)
        {
            if(AllCost>other.AllCost)
            {
                return 1;
            }
            else if(AllCost<other.AllCost)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
