using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace _150207214.Model
{
    public class MapXmlReader
    {
        public static readonly string PATH = @"..\..\mapXml.xml";
        private Block[,] m_CurrentMap;
        public bool IsLoad { get; private set; }
        public string CurrentLevel { get; private set; }
        public int CurrentMapWidth { get { return m_CurrentMap.GetLength(1); } }
        public int CurrentMapHeight { get { return m_CurrentMap.GetLength(0); } }
        private Image m_HeroImage;
        public Image HeroImage
        {
            get
            {
                if (m_HeroImage == null)
                {
                    m_HeroImage = new Image();
                    m_HeroImage.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                  Resource.hero.GetHbitmap(),
                                  IntPtr.Zero,
                                  Int32Rect.Empty,
                                  BitmapSizeOptions.FromEmptyOptions());
                }
                return m_HeroImage;
            }
        }

        private Vector2Int m_CurrentHeroPos;
        public Vector2Int CurrentHeroPos
        {
            get { return m_CurrentHeroPos; }
            set
            {
                m_CurrentHeroPos = value;
                HeroImage.SetValue(Canvas.LeftProperty, (double)48 * CurrentHeroPos.X);
                HeroImage.SetValue(Canvas.TopProperty, (double)48 * CurrentHeroPos.Y);
            }
        }
        public void ReadBlankMap(int w,int h)
        {
            m_CurrentMap = new Block[h, w];
            IsLoad = true;
        }
        public bool ReadMap(string level)
        {
            XElement xe = XElement.Load(PATH);
            IEnumerable<XElement> elements = from ele in xe.Elements("map")
                                             where (string)ele.Attribute("Id") == level
                                             select ele;
            if (elements.Count() < 1)
            {
                return false;
            }
            else
            {
                XElement first = elements.First();
                CurrentLevel = level;
                int width = int.Parse(first.Element("width").Value);
                int height = int.Parse(first.Element("height").Value);
                m_CurrentMap = new Block[height, width];
                CurrentHeroPos = new Vector2Int(
                    int.Parse(first.Element("hero").Attribute("x").Value),
                    int.Parse(first.Element("hero").Attribute("y").Value)); ;
                for (int i = 0; i < CurrentMapHeight; i++)
                {
                    char[] row = first.Element("row" + i).Value.ToCharArray();
                    for (int j = 0; j < CurrentMapWidth; j++)
                    {
                        m_CurrentMap[i, j] = new Block((BlockType)row[j] - '0', new Vector2Int(j, i));
                    }
                }
                IsLoad = true;
                return true;
            }
        }
        public void AddMapSize(int width,int height)
        {
            Block[,] oldMap = m_CurrentMap;
            Block[,] newMap=new Block[CurrentMapHeight+height,CurrentMapWidth+width];
            for (int i = 0; i < CurrentMapHeight&&i<CurrentMapHeight+height; i++)
            {
                for (int j = 0; j < CurrentMapWidth&&j<CurrentMapWidth+width; j++)
                {
                    newMap[i, j] = oldMap[i, j];
                }
            }
            m_CurrentMap = newMap;
        }
        public Block GetBlock(int h, int w)
        {
            if (m_CurrentMap == null)
                throw new Exception("Not Read Map");
            if (h >= CurrentMapHeight || h < 0 || w >= CurrentMapWidth || w < 0)
                return null;
            return m_CurrentMap[h, w];
        }
        public void SetBlock(int h, int w,Block block)
        {
            if (m_CurrentMap == null)
                throw new Exception("Not Read Map");
            if (h >= CurrentMapHeight || h < 0 || w >= CurrentMapWidth || w < 0)
                throw new Exception("Out Of Index");
            m_CurrentMap[h, w]=block;
        }
        public void SaveMap(string id)
        {
            XElement xe = XElement.Load(PATH);
            IEnumerable<XElement> elements = from ele in xe.Elements("map")
                                             where (string)ele.Attribute("Id") == id
                                             select ele;
            elements.Remove();

            XElement saveXe=new XElement("map");
            saveXe.SetAttributeValue("Id",id);
            
            XElement heroXe = new XElement("hero");
            heroXe.SetAttributeValue("x",CurrentHeroPos.X);
            heroXe.SetAttributeValue("y", CurrentHeroPos.Y); 
            saveXe.Add(heroXe);
            saveXe.SetElementValue("width", CurrentMapWidth);
            saveXe.SetElementValue("height", CurrentMapHeight);

            for (int i = 0; i < CurrentMapHeight; i++)
            {
                XElement rowXe = new XElement("row" + i);
                string content="";
                for (int j = 0; j < CurrentMapWidth; j++)
                {
                    content+=(int)m_CurrentMap[i,j].Type;
                }
                rowXe.SetValue(content);
                saveXe.Add(rowXe);
            }
            xe.Add(saveXe);
            xe.Save(PATH);
        }
        public void CheckHeroPos()
        {
            if(m_CurrentHeroPos.Y>CurrentMapHeight)
            {
                CurrentHeroPos = new Vector2Int(CurrentHeroPos.X, CurrentMapHeight);
            }
            if(m_CurrentHeroPos.X>CurrentMapWidth)
            {
                CurrentHeroPos = new Vector2Int(CurrentMapWidth, CurrentHeroPos.Y);
            }
        }
    }
}
