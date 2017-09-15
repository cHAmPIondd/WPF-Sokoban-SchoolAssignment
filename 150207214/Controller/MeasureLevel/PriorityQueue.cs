using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _150207214.Controller
{
    class PriorityQueue<T> where T : class,IComparable<T>,IEquatable<T>
    {
        List<T> m_ElementList;
        public int Count { get { return m_ElementList.Count; } }
        public PriorityQueue()
        {
            m_ElementList = new List<T>(5000);
            m_ElementList.Add(null);
        }
        /// <summary>
        /// 维护i位置的数值保持最小优先队列
        /// </summary>
        /// <param name="i"></param>
        public void MinHeapify(int i)
        {
            int l=Left(i);
            int r=Right(i);
            int minimum= 0;
            if(l<m_ElementList.Count&&m_ElementList[l].CompareTo(m_ElementList[i])<0)
            {
                minimum = l;
            }
            else
            {
                minimum = i;
            }
            if (r < m_ElementList.Count && m_ElementList[r].CompareTo(m_ElementList[minimum]) < 0)
            {
                minimum = r;
            }
            if(minimum!=i)
            {
                Exchange(i, minimum);
                MinHeapify(minimum);
            }
        }
        /// <summary>
        /// 替换i位置为更小值
        /// </summary>
        public void HeapDecreaseKey(T iTemp, T temp)
        {
            int i = GetIndex(iTemp);
            if (i == m_ElementList.Count)
                m_ElementList.Add(temp);
            else
                m_ElementList[i] = temp;
            while (i > 1 && m_ElementList[Parent(i)].CompareTo(m_ElementList[i]) > 0)
            {
                Exchange(i, Parent(i));
                i = Parent(i);
            }
        }
        public int GetIndex(T temp)
        {
            int i = 1;
            while(i<m_ElementList.Count&&!m_ElementList[i].Equals(temp))
            {
                i++;
            }
            return i;
        }
        /// <summary>
        /// 取出最小值
        /// </summary>
        public T HeapExtractMin()
        {
            T min = m_ElementList[1];
            m_ElementList[1] = m_ElementList[m_ElementList.Count - 1];
            m_ElementList.RemoveAt(m_ElementList.Count-1);
            MinHeapify(1);
            return min;
        }
        /// <summary>
        /// 插入元素
        /// </summary>
        public void MinHeapInsert(T temp)
        {
            m_ElementList.Add(temp);
            int i=m_ElementList.Count - 1;
            while (i > 1 && m_ElementList[Parent(i)].CompareTo(m_ElementList[i]) > 0)
            {
                Exchange(i, Parent(i));
                i = Parent(i);
            }
        }
        private void Exchange(int one,int two)
        {
            T temp = m_ElementList[one];
            m_ElementList[one] = m_ElementList[two];
            m_ElementList[two] = temp;
        }
        private int Left(int i)
        {
            return i << 1;
        }
        private int Right(int i)
        {
            return (i << 1) + 1;
        }
        private int Parent(int i)
        {
            return i >> 1;
        }
    }
}
