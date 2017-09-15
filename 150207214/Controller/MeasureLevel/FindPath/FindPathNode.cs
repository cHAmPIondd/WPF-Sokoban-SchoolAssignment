using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _150207214.Controller
{
    class FindPathNode
    {
        public Vector2Int Pos { get; set; }
        public int HadCost { get; set; }
        public int AllCost { get; set; }
        public FindPathNode Parent { get; set; }
        public bool IsPassable { get; set; }
        public FindPathNode(Vector2Int v2, bool isPassable)
        {
            Pos = v2;
            HadCost = int.MaxValue;
            IsPassable = isPassable;
        }
        public void CalCost(Vector2Int target)
        {
            AllCost=HadCost+Math.Abs(Pos.X - target.X) + Math.Abs(Pos.Y - target.Y);
        }
    }
}
