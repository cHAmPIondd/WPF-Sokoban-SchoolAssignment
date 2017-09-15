using _150207214.Controller;
using _150207214.View;
using _150207214.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _150207214
{
    /// <summary>
    /// GameWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GameWindow : Window
    {
        private ManagerInterface m_Manager;
        private MapXmlReader m_MapXmlReader;
        private MapView m_MapView;
        private List<Block> m_TargetList=new List<Block>();
        public GameWindow(ManagerInterface manager,MapXmlReader mapXmlReader)
        {
            InitializeComponent();
            m_Manager = manager;
            m_MapXmlReader = mapXmlReader;
            m_MapView = new MapView(gameview, m_MapXmlReader);
            ReLoad();
            this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
            this.WindowState = System.Windows.WindowState.Maximized;
            ReLoadTargetList();
           
        }
        public void ReLoad()
        {
            m_MapView.AddMapImage();
            ReLoadTargetList();
        }
        private void ReLoadTargetList()
        {
            m_TargetList.Clear();
            for (int i = 0; i < m_MapXmlReader.CurrentMapHeight; i++)
            {
                for (int j = 0; j < m_MapXmlReader.CurrentMapWidth; j++)
                {
                    Block block = m_MapXmlReader.GetBlock(i, j);
                    if ((block.Type & BlockType.TARGET) != 0)
                    {
                        m_TargetList.Add(block);
                    }
                }
            }
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            int x = 0, y = 0;
            switch(e.Key)
            {
                case Key.W:
                case Key.Up: y = -1; break;
                case Key.S:
                case Key.Down: y = 1; break;
                case Key.A:
                case Key.Left: x = -1; break;
                case Key.D:
                case Key.Right: x = 1; break;
            }
            Block block = m_MapXmlReader.GetBlock( m_MapXmlReader.CurrentHeroPos.Y + y,m_MapXmlReader.CurrentHeroPos.X + x);
            if (block == null)
                return;
            switch(block.MaxType)
            {
                case BlockType.BOX:
                    {
                        Block block2 = m_MapXmlReader.GetBlock( block.Pos.Y + y,block.Pos.X + x);
                        if (block2 == null)
                            return;
                        switch(block2.Type)
                        {
                            case BlockType.BLANK:
                            case BlockType.TARGET:
                                {
                                    block2.Type += (int)block.MaxType; 
                                    block.Type -= block.MaxType;
                                    m_MapXmlReader.CurrentHeroPos += new Vector2Int(x, y);
                                    CheckIsVictory();
                                    break;
                                }
                            case BlockType.WALL:
                            case BlockType.BOX: break;
                        }
                        break;
                    }
                case BlockType.BLANK:
                case BlockType.TARGET: m_MapXmlReader.CurrentHeroPos += new Vector2Int(x, y); break;
                case BlockType.WALL: break;
            }
        }
        private void CheckIsVictory()
        {
            bool isWin = true;
            foreach(var temp in m_TargetList)
            {
                if(temp.Type==BlockType.TARGET)
                {
                    isWin = false;
                    break;
                }
            }
            if (isWin)
            {
                m_MapView.ClearMapImage();
                m_Manager.PassLevel();
            }
        }
        private void backto_button_Click(object sender, RoutedEventArgs e)
        {
            m_MapView.ClearMapImage();
            m_Manager.ReloadLevel();
        }
        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            Close(); 
            Application.Current.Shutdown();
        }
        private void OnClosed(object sender, EventArgs e)
        {
            m_MapView.ClearMapImage();
            m_Manager.CloseWindow();
        }

    }
}
