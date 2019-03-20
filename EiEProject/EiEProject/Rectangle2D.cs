using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace EiEProject
{
    class Rectangle2D
    {
        #region variables
        Point2D center = new Point2D();
        float width = 0;
        float height = 0;
        Pen pen = Pens.Black;
        Brush brush = Brushes.Black;
        Point2D velocity = new Point2D();
        float moveDistance = 0;

        int srcRectInt;

        int playerInsideBarrier;
        #endregion

        #region constructors
        public Rectangle2D(Point2D center, float width, float height)
        {
            this.center = center;
            this.width = width;
            this.height = height;
            playerInsideBarrier = 0;
        }
        #endregion

        #region properties
        public Point2D Center { get { return center; } set { center = value; } }
        public float Width { get { return width; } set { width = value; } }
        public float Height { get { return height; } set { height = value; } }
        public Pen Pen { get { return pen; } set { pen = value; } }
        public Brush Brush { get { return brush; } set { brush = value; } }

        public float Top { get { return center.Y - height / 2; } }
        public float Bottom { get { return center.Y + height / 2; } }
        public float Right { get { return center.X + width / 2; } }
        public float Left { get { return center.X - width / 2; } }
        public Point2D Velocity { get { return velocity; } set { velocity = value; } }
        public int SrcRectInt { get { return srcRectInt; } set { srcRectInt = value; } }

        public int PlayerInsideBarrier { get { return playerInsideBarrier; } set { playerInsideBarrier = value; } }

        #endregion


        #region methods
        /// <summary>
        /// Determines if the rectangle contains a point
        /// </summary>
        /// <param name="pt">The point being checked</param>
        /// <returns>True if the pt is within the rectangle else false</returns>
        public bool Contains(Point2D pt)
        {
            return (pt.X >= Left && pt.Y >= Top && pt.X <= Right && pt.Y <= Bottom);
        }
        public void Draw(Graphics gr)
        {
            gr.DrawRectangle(pen, Left, Top, width, height);
        }
        public void Fill(Graphics gr)
        {
            gr.FillRectangle(brush, Left, Top, width, height);
        }
        public void ImageFill(Graphics gr, Bitmap graphicsBits)
        {
            Rectangle destRect = new Rectangle((int)Left, (int)Top, (int)Width, (int)height);
            gr.DrawImage(graphicsBits, destRect, new Rectangle(SrcRectInt * (int)Width, 0, (int)Width, (int)Height), GraphicsUnit.Pixel);
        }
        public void movingBarrier()
        {
            float scalarX = (moveDistance / 2) / velocity.X;
            if (scalarX == 0)
            {
                center.X += velocity.X;
                scalarX--;
            }

        }
        public int playerHitBarrier(Sphere2D player)
        {
            if (player.Center.X >= center.X - width / 2 && player.Center.X <= center.X + width / 2 &&
                player.Center.Y <= center.Y + height / 2 && player.Center.Y >= center.Y - height)
            {
                return 1;
            }
            return 0;
        }
        #endregion

    }
}
