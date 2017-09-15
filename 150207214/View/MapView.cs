using _150207214.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace _150207214.View
{
    class MapView
    {
        private Canvas m_Canvas;
        private MapXmlReader m_MapXmlReader;
        public MapView(Canvas canvas,MapXmlReader mapXmlReader)
        {
            m_Canvas = canvas;
            m_MapXmlReader=mapXmlReader;
        }
        public void AddMapImage()
        {
            m_Canvas.Children.Clear();
            for (int i = 0; i < m_MapXmlReader.CurrentMapHeight; i++)
            {
                for (int j = 0; j < m_MapXmlReader.CurrentMapWidth; j++)
                {
                    //生成地图
                    m_Canvas.Children.Add(m_MapXmlReader.GetBlock(i, j).Image);
                }
            } 
            m_Canvas.Children.Add(m_MapXmlReader.HeroImage);
        }
        public void ClearMapImage()
        {
            m_Canvas.Children.Clear();
        }
        public void AddImage(Image image)
        {
            m_Canvas.Children.Add(image);
        }
        public void RemoveImage(Image image)
        {
            m_Canvas.Children.Remove(image);
        }
        public void ReplaceImage(Image oldImage,Image newImage)
        {
            m_Canvas.Children.Insert(m_Canvas.Children.IndexOf(oldImage),newImage);
            RemoveImage(oldImage);
        }
    }
}
