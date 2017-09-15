using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _150207214.Controller
{
    class DoubleDictionary<T> where T :DoubleHashCodeInterface
    {
        private Dictionary<int,Dictionary<int,List<T>>> m_DoubleDictionary;
        public DoubleDictionary()
        {
            m_DoubleDictionary = new Dictionary<int, Dictionary<int, List<T>>>();
            Count = 0;
        }
        public void Add(T temp)
        {
            Count++;
            if(!m_DoubleDictionary.ContainsKey(temp.GetHashCode()))
            {
                m_DoubleDictionary[temp.GetHashCode()] = new Dictionary<int, List<T>>();
                m_DoubleDictionary[temp.GetHashCode()][temp.GetHashCode2()] = new List<T>(){temp};
            }
            else
            {
                if(!m_DoubleDictionary[temp.GetHashCode()].ContainsKey(temp.GetHashCode2()))
                    m_DoubleDictionary[temp.GetHashCode()][temp.GetHashCode2()] = new List<T>() { temp };
                else
                    m_DoubleDictionary[temp.GetHashCode()][temp.GetHashCode2()].Add(temp);
            }
        }
        public void Remove(T temp)
        {
            Count--;
            m_DoubleDictionary[temp.GetHashCode()][temp.GetHashCode2()].Remove(temp);
        }
        public Dictionary<int, List<T>> Get(int hashCode)
        {
            return m_DoubleDictionary.ContainsKey(hashCode)?m_DoubleDictionary[hashCode]:null;
        }
        public int Count { get; set; }
    }
}
