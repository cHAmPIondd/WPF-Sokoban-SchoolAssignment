using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace _150207214.Model
{
    public class Block
    {
        private BlockType m_Type;
        public BlockType Type
        {
            get { return m_Type; }
            set
            {
                m_Type = value;
                IntPtr bitmap = IntPtr.Zero;
                switch ((int)Type)
                {
                    case 0: bitmap = Resource.block_0.GetHbitmap(); break;
                    case 1: bitmap = Resource.block_1.GetHbitmap(); break;
                    case 2:
                    case 3: bitmap = Resource.block_2.GetHbitmap(); break;
                    case 4: bitmap = Resource.block_4.GetHbitmap(); break;
                    case 5: bitmap = Resource.block_5.GetHbitmap(); break;
                }
                Image.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                              bitmap,
                              IntPtr.Zero,
                              Int32Rect.Empty,
                              BitmapSizeOptions.FromEmptyOptions());
            }
        }
        public BlockType MaxType
        {
            get
            {
                BlockType t=BlockType.BLANK;
                foreach(var type in Enum.GetValues(typeof(BlockType)))
                {
                    if ((int)Type < (int)type)
                        break;
                    t = (BlockType)type;
                }
                return t;
            }
        }
        public Vector2Int Pos { get; private set; }
        private Image m_Image;
        public Image Image
        {
            get
            {
                if (m_Image == null)
                {
                    m_Image = new Image();
                    m_Image.SetValue(Canvas.LeftProperty, (double)48 * Pos.X);
                    m_Image.SetValue(Canvas.TopProperty, (double)48 * Pos.Y);
                }
                return m_Image;
            }
        }
        /// <summary>
        /// pos.x是宽度，y是高度
        /// </summary>
        public Block(BlockType type, Vector2Int pos)
        {
            Pos = pos;
            Type = type;
        }
    }
}
