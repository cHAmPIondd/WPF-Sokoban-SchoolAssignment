using _150207214.Controller;
using _150207214.Model;
using _150207214.View;
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
    /// MapEditorWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MapEditorWindow : Window
    {
        private static readonly string LOAD_REQUEST="load";
        private static readonly string SAVE_REQUEST = "save";
        private Window m_OwnerWindow;
        private MapXmlReader m_MapXmlReader;
        private MapView m_MapView;
        private MeasureLevelManager m_MeasureLevelManager;
        private bool m_IsMeasure;
        public MapEditorWindow(Window ownerWindow)
        {
            InitializeComponent();
            m_OwnerWindow = ownerWindow;
            this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
            this.WindowState = System.Windows.WindowState.Maximized; 
            m_MapXmlReader = new MapXmlReader();
            m_MapView = new MapView(map_canvas, m_MapXmlReader);
            m_MeasureLevelManager = new MeasureLevelManager(m_MapXmlReader);
        }
        private void AddButtonMouseDownEvent(Block block)
        {
            block.Image.MouseDown += (e, a) =>
            {
                if (m_IsMeasure)
                {
                    MessageBox.Show("The procedure is running,can't be modified");
                    return;
                }
                if (a.ChangedButton == MouseButton.Left)
                {
                    block.Type = (BlockType)((int)(block.Type + 1) % 6);
                    if ((int)block.Type == 3)
                        block.Type++;
                }
                else if(a.ChangedButton == MouseButton.Right)
                {
                    block.Type = (BlockType)((int)(block.Type + 5) % 6);
                    if ((int)block.Type == 3)
                        block.Type--;
                }
                else if(a.ChangedButton == MouseButton.Middle)
                {
                    Vector2Int v2 = block.Pos;
                    Block block2 = new Block(BlockType.BLANK, v2);
                    m_MapView.ReplaceImage(m_MapXmlReader.GetBlock(v2.Y, v2.X).Image, block2.Image);
                    m_MapXmlReader.SetBlock(v2.Y, v2.X, block2);
                    AddButtonMouseDownEvent(block2);
                    m_MapXmlReader.CurrentHeroPos = v2;
                }
            };
        }
        private void AddButton(int w, int h)
        {
            Block block = new Block(BlockType.BLANK, new Vector2Int(w,h));
            m_MapXmlReader.SetBlock(h,w,block);
            AddButtonMouseDownEvent(block);
        }
        private void load_button_Click(object sender, RoutedEventArgs e)
        {//加载地图
            if (m_IsMeasure)
            {
                MessageBox.Show("The procedure is running,can't be modified");
                return;
            }
            this.Visibility = System.Windows.Visibility.Hidden;
            Window window = new DialogWindow(LOAD_REQUEST,"Please input a level you want to load");
            window.Owner = this;
            window.Show();
        }

        private void test_button_Click(object sender, RoutedEventArgs e)
        {//测试地图
            if (m_IsMeasure)
            {
                MessageBox.Show("The procedure is running,can't be modified");
                return;
            }
            if (m_MapXmlReader.IsLoad)
            {
                m_MapView.ClearMapImage();
                //保存到xml用limit
                m_MapXmlReader.SaveMap("limit");
                //测试
                this.Visibility = System.Windows.Visibility.Hidden;
                new EditorTestManager(m_MapXmlReader, this);
            }
            else
            {
                MessageBox.Show("Can't Test Blank Map!");
            }
        }

        private void save_button_Click(object sender, RoutedEventArgs e)
        {//保存地图
            if (m_MapXmlReader.IsLoad)
            {
                this.Visibility = System.Windows.Visibility.Hidden;
                Window window = new DialogWindow(SAVE_REQUEST, "Please input a id");
                window.Owner = this;
                window.Show();
            }
            else
            {
                MessageBox.Show("Can't Save Blank Map!");
            }
        }
        private void new_map_Click(object sender, RoutedEventArgs e)
        {
            if (m_IsMeasure)
            {
                MessageBox.Show("The procedure is running,can't be modified");
                return;
            }
            //初始化地图
            m_MapView.ClearMapImage();
            m_MapXmlReader.ReadBlankMap(1, 1);
            AddButton(0, 0);
            m_MapView.AddMapImage();
            AddWHAddButton();
        }
        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void measure_button_Click(object sender, RoutedEventArgs e)
        {
            if (m_IsMeasure)
            {
                MessageBox.Show("The procedure is running,can't be modified");
                return;
            }
            if(m_MapXmlReader.IsLoad)
            {
                DateTime datetime = DateTime.Now;
                m_IsMeasure = true;
                var tempList = m_MeasureLevelManager.MeasureLevel(label);
                string tempString = "";
                if (tempList != null)
                {
                    foreach (var temp in tempList)
                    {
                        tempString += temp.MoveStep + " . ";
                    }
                    MessageBox.Show("最少步数："+tempList.Count+"\n花费时间："+(DateTime.Now-datetime)+"\n具体步骤：\n" + tempString);
                }
                m_IsMeasure = false;
            }
            else
            {
                MessageBox.Show("Can't Test Blank Map!");
            }
        }
        public void DialogCallback(string requestCode, string id)
        {//接受对话框的返回数据
            if (requestCode == LOAD_REQUEST)
            {
                if (!m_MapXmlReader.ReadMap(id))
                {
                    MessageBox.Show("Not Found This Level.");
                    return;
                }
                //加载地图
                m_MapView.AddMapImage();

                for (int i = 0; i < m_MapXmlReader.CurrentMapHeight; i++)
                {
                    for (int j = 0; j < m_MapXmlReader.CurrentMapWidth; j++)
                    {
                        Block block = m_MapXmlReader.GetBlock(i, j);
                        AddButtonMouseDownEvent(block);
                    }
                }
                AddWHAddButton();
            }
            else if (requestCode==SAVE_REQUEST)
            {//保存地图到xml用id
                m_MapXmlReader.SaveMap(id);
            }
            
        }
        public void AddWHAddButton()
        {//两个“+按钮”
            Button btn1 = new Button
            {
                Width=48,
                Height=m_MapXmlReader.CurrentMapHeight*48,
                Content="+"
            };
            btn1.SetValue(Canvas.LeftProperty, (double)48 * m_MapXmlReader.CurrentMapWidth);
            Button btn2 = new Button
            {
                Height = 48,
                Width = m_MapXmlReader.CurrentMapWidth * 48,
                Content = "+"
            };
            btn2.SetValue(Canvas.TopProperty, (double)48 * m_MapXmlReader.CurrentMapHeight);
            //宽度减少
            btn1.MouseDown += (e,a) =>
            {
                if (m_IsMeasure)
                {
                    MessageBox.Show("The procedure is running,can't be modified");
                    return;
                }
                if (m_MapXmlReader.CurrentMapWidth < 1 || a.ChangedButton != MouseButton.Right)
                {
                    return;
                }
                m_MapView.ClearMapImage();
                m_MapXmlReader.AddMapSize(-1, 0);
                m_MapView.AddMapImage();
                AddWHAddButton();
            };
            //宽度增加
            btn1.Click += (e, a) =>
            {
                if (m_IsMeasure)
                {
                    MessageBox.Show("The procedure is running,can't be modified");
                    return;
                }
                m_MapView.ClearMapImage();
                m_MapXmlReader.AddMapSize(1, 0);
                for (int i = 0; i < m_MapXmlReader.CurrentMapHeight; i++)
                    AddButton(m_MapXmlReader.CurrentMapWidth - 1,i);
                m_MapView.AddMapImage();
                AddWHAddButton();
            };
            //高度减少
            btn2.MouseDown += (e, a) =>
            {
                if (m_IsMeasure)
                {
                    MessageBox.Show("The procedure is running,can't be modified");
                    return;
                }
                if (m_MapXmlReader.CurrentMapHeight < 1 || a.ChangedButton != MouseButton.Right)
                {
                    return;
                }
                m_MapView.ClearMapImage();
                m_MapXmlReader.AddMapSize(0, -1);
                m_MapView.AddMapImage();
                AddWHAddButton();
            };
            //高度增加
            btn2.Click += (e, a) =>
            {
                if (m_IsMeasure)
                {
                    MessageBox.Show("The procedure is running,can't be modified");
                    return;
                }
                m_MapView.ClearMapImage();
                m_MapXmlReader.AddMapSize(0,1);
                for (int i = 0; i < m_MapXmlReader.CurrentMapWidth; i++)
                    AddButton(i,m_MapXmlReader.CurrentMapHeight - 1 );
                m_MapView.AddMapImage();
                AddWHAddButton();
            };
            map_canvas.Children.Add(btn1);
            map_canvas.Children.Add(btn2);
            m_MapXmlReader.CheckHeroPos();
        }
        public void ReLoad()
        {
            this.Visibility = System.Windows.Visibility.Visible;
            m_MapXmlReader.ReadMap("limit");
            for (int i = 0; i < m_MapXmlReader.CurrentMapHeight;i++)
            {
                for(int j=0;j < m_MapXmlReader.CurrentMapWidth;j++)
                {
                    Block block = m_MapXmlReader.GetBlock(i, j);
                    AddButtonMouseDownEvent(block);
                }
            }
            m_MapView.ClearMapImage();
            m_MapView.AddMapImage();
            AddWHAddButton();
        }
        private void OnClosed(object sender, EventArgs e)
        {
            m_OwnerWindow.Visibility = System.Windows.Visibility.Visible;
            Close();
        }

    }
}
