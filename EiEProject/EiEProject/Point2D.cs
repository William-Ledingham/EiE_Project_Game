using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace EiEProject
{
    class Point2D
    {
        //variables for the class

        float x = 0;
        float y = 0;


        #region Class constructors
        /// <summary>
        /// A basic constructor with x,y initialized to 0,0
        /// </summary>
        public Point2D() { }

        /// <summary>
        /// Point2D constructor
        /// </summary>
        /// <param name="x">The initial x-value</param>
        /// <param name="y">The initial y-value</param>
        public Point2D(double x, double y)
        {
            //'this' means the public variables
            this.x = (float)x;
            this.y = (float)y;
        }

        /// <summary>
        /// Point2D constructor
        /// </summary>
        /// <param name="pt">the initial x,y values passed as PointF</param>
        public Point2D(PointF pt)
        {
            x = pt.X;
            y = pt.Y;
        }

        /// <summary>
        /// Point2D constructor
        /// </summary>
        /// <param name="pt">the initial x,y values passed as Point</param>
        public Point2D(Point pt)
        {
            x = pt.X;
            y = pt.Y;
        }
        #endregion

        #region Class properties
        /// <summary>
        /// Gets and Sets the x-value of the point
        /// </summary>
        public float X { get { return x; } set { x = value; } }

        /// <summary>
        /// Gets and Sets the x-value of the point
        /// </summary>
        public float Y { get { return y; } set { y = value; } }

        /// <summary>
        /// Get and Set the x,y pair using a PointF value
        /// </summary>
        public PointF PointF { get { return new PointF(x, y); } set { x = value.X; y = value.Y; } }

        /// <summary>
        /// The magnitude of the Point2D (when used a vector
        /// </summary>
        public float Magnitude { get { return (float)Math.Sqrt(x * x + y * y); } }

        public Point2D Normal { get { return new Point2D(-Y, X); } }
        #endregion



        #region Class Operators

        /// <summary>
        /// Add tow points together and return the result as a Point2D
        /// </summary>
        /// <param name="p1">The first point</param>
        /// <param name="p2">the second point</param>
        /// <returns>the resultant addition</returns>
        public static Point2D operator +(Point2D p1, Point2D p2)
        {
            return new Point2D(p1.x + p2.x, p1.y + p2.y);
        }
        /// <summary>
        /// Suvstraction of two Point2D variables
        /// </summary>
        /// <param name="p1">The first point</param>
        /// <param name="p2">the second point</param>
        /// <returns>the resultant subtraction</returns>
        public static Point2D operator -(Point2D p1, Point2D p2)
        {
            return new Point2D(p1.x - p2.x, p1.y - p2.y);
        }
        /// <summary>
        /// multiply a point by a scalar value
        /// </summary>
        /// <param name="p1">The point</param>
        /// <param name="scalar">The scalar</param>
        /// <returns>//the resultant multiplication</returns>
        public static Point2D operator *(Point2D p1, float scalar)
        {
            return new Point2D(p1.x * scalar, p1.y * scalar);
        }
        /// <summary>
        /// multiply a point by a scalar value
        /// </summary>
        /// <param name="scalar">The scalar</param>
        /// <param name="p1">The point</param>
        /// <returns>//the resultant multiplication</returns>
        public static Point2D operator *(float scalar, Point2D p1)
        {
            return new Point2D(p1.x * scalar, p1.y * scalar);
        }


        /// <summary>
        /// Divide a point by a scalar value
        /// </summary>
        /// <param name="p1">The point</param>
        /// <param name="scalar">The scalar</param>
        /// <returns>//the resultant division</returns>
        public static Point2D operator /(Point2D p1, float scalar)
        {
            return new Point2D(p1.x / scalar, p1.y / scalar);
        }

        /// <summary>
        /// The Dot product of two points
        /// </summary>
        /// <param name="p1">//The first point</param>
        /// <param name="p2">the second point</param>
        /// <returns></returns>
        public static float operator *(Point2D p1, Point2D p2)
        {
            return p1.x * p2.x + p1.y * p2.y;
        }
        #endregion


        #region Class methods
        public void Draw(Graphics gr)
        {
            gr.FillEllipse(Brushes.White, x - 1, y - 1, 3, 3);
        }


        public void Normalize()
        {
            float len = this.Magnitude;
            if (len == 0) return;
            x /= len;
            y /= len;
        }
        #endregion
    }
}
