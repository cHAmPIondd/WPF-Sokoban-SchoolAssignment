using _150207214.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _150207214.Controller
{
    class GameManager:ManagerInterface
    {
        private Window m_OwnerWindow;
        private MapXmlReader m_MapXmlReader;
        private GameWindow m_GameWindow;
        public GameManager(Window ownerWindow)
        {
            m_OwnerWindow = ownerWindow;
            m_MapXmlReader = new MapXmlReader();
            m_MapXmlReader.ReadMap(1+"");
            m_GameWindow = new GameWindow(this, m_MapXmlReader);
            m_GameWindow.Show();
        }
        public void PassLevel()
        {
            LoadLevel(int.Parse(m_MapXmlReader.CurrentLevel) + 1);
        }
        public void ReloadLevel()
        {
            LoadLevel(int.Parse(m_MapXmlReader.CurrentLevel));
        }
        public void CloseWindow()
        {
            m_OwnerWindow.Visibility = System.Windows.Visibility.Visible;
        }
        private void LoadLevel(int level)
        {
            if (!m_MapXmlReader.ReadMap(level + ""))
            {
                MessageBox.Show("Congratulations,you win!");
                m_GameWindow.Close();
                Window window = new MainWindow();
                window.Show();
            }
            else
            {
                MessageBox.Show("Level---" + level);
                m_GameWindow.ReLoad();
            }
        }
    }
}
