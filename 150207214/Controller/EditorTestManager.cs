using _150207214.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _150207214.Controller
{
    public class EditorTestManager:ManagerInterface
    {
        private MapXmlReader m_MapXmlReader;
        private Window m_OwnerWindow;
        private GameWindow m_GameWindow;
        public EditorTestManager(MapXmlReader mapXmlReader,Window window)
        {
            m_MapXmlReader = mapXmlReader;
            m_OwnerWindow = window;
            m_MapXmlReader.ReadMap("limit");
            m_GameWindow = new GameWindow(this, m_MapXmlReader);
            m_GameWindow.Show();
        }

        public void PassLevel()
        {
            ((MapEditorWindow)m_OwnerWindow).ReLoad();
            m_GameWindow.Close();
            MessageBox.Show("This Map Can Work.");
        }

        public void ReloadLevel()
        {
            m_MapXmlReader.ReadMap("limit");
            m_GameWindow.ReLoad();
        }
        public void CloseWindow()
        {
            ((MapEditorWindow)m_OwnerWindow).ReLoad();
            m_GameWindow.Close();
        }
    }
}
