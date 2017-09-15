using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _150207214
{
    public struct Vector2Int
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public Vector2Int(int x, int y)
            : this()  
        {
            X = x;
            Y = y;
        }
        public static  Vector2Int operator+(Vector2Int one,Vector2Int two)
        {
            return new Vector2Int(one.X+two.X,one.Y+two.Y);
        }
        public static bool operator >(Vector2Int one, Vector2Int two)
        {
            return one.X*100 + one.Y> two.X*100 + two.Y;
        }
        public static bool operator <(Vector2Int one, Vector2Int two)
        {
            return one.X * 100 + one.Y < two.X * 100 + two.Y;
        }
        public static Vector2Int operator -(Vector2Int one, Vector2Int two)
        {
            return new Vector2Int(one.X - two.X, one.Y - two.Y);
        }
        public static bool operator ==(Vector2Int one, Vector2Int two)
        {
            return (one.X==two.X&&one.Y==two.Y)?true:false;
        }
        public static bool operator !=(Vector2Int one, Vector2Int two)
        {
            return (one.X != two.X || one.Y != two.Y) ? true : false;
        }
        public static bool Equals(Vector2Int one, Vector2Int two)
        {
            return (one.X == two.X && one.Y == two.Y) ? true : false;
        }
        public override string ToString()
        {
            return "("+X + "," + Y+")";
        }
    }
}
