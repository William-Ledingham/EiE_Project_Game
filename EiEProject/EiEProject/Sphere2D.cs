using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace EiEProject
{
    class Sphere2D
    {
        //variables
        Point2D center;
        float radius;
        Brush brush = Brushes.Blue; //the brush to fill the sphere
        Pen pen = Pens.Black; //the pen to outline the shpere
        Point2D velocity = new Point2D();
        float friction = 1f;//slows ball
        float mass = 10; // some value to give the ball momentum
        float elasticity = 1f; // a value between 0 and 1
        float gravity = 1.7f;

        #region Class constructors
        /// <summary>
        /// constructor an object wiht variables initialized to 0
        /// </summary>
        public Sphere2D() { center = new Point2D(); radius = 0; }

        /// <summary>
        /// constructor for a sphere2D
        /// </summary>
        /// <param name="center">the center of the sphere</param>
        /// <param name="r">the radius of the sphere</param>
        public Sphere2D(Point2D center, double r)
        {
            this.center = center;
            radius = (float)r;
        }
        #endregion






        #region Class properties
        /// <summary>
        /// Get/Set the center of the sphere
        /// </summary>
        public Point2D Center { get { return center; } set { center = value; } }
        /// <summary>
        /// Get/Set the radius of the sphere
        /// </summary>
        public float Radius { get { return radius; } set { radius = value; } }
        /// <summary>
        /// Get/Set the pen to outline the sphere
        /// </summary>
        public Pen Pen { get { return pen; } set { pen = value; } }
        /// <summary>
        /// Get/Set the brush to fill the sphere
        /// </summary>
        public Brush Brush { get { return brush; } set { brush = value; } }
        /// <summary>
        /// Get/Set the sphere's velocity
        /// </summary>
        public Point2D Velocity { get { return velocity; } set { velocity = value; } }
        /// <summary>
        /// Get/Set the mass of the sphere
        /// </summary>
        public float Mass { get { return mass; } set { mass = value; } }
        /// <summary>
        /// Get/Set the elasticity of the sphere
        /// </summary>
        public float Elasticity { get { return elasticity; } set { elasticity = value; } }
        /// <summary>
        /// Get/Set the friction of the sphere
        /// </summary>
        public float Friction { get { return friction; } set { friction = value; } }

        #endregion







        #region Class methods
        public void Move()
        {
            center += velocity;
            velocity.Y += gravity;
            velocity *= friction;
            if (velocity.Magnitude < 0.05)
            {
                velocity = new Point2D(0, 0);
            }
        }
        /// <summary>
        /// Bounces the sphere off the edges of the rectangle
        /// </summary>
        /// <param name="rect">The rectangle containing the sphere</param>
        public void Bounce(Rectangle2D rect)
        {
            if (center.X - radius <= rect.Left)
            {
                //float difference = rect.left - (center.x - radius);
                //center.x += (2 * difference);
                center.X = (center.X * -1) + (2 * rect.Left) + (2 * radius);
                velocity.X = velocity.X * -1;
            }
            if (center.Y - radius <= rect.Top)
            {
                //float difference = rect.top - (center.y -radius);
                //center.y += (2*difference);
                center.Y = (center.Y * -1) + (2 * rect.Top) + (2 * radius);
                velocity.Y = velocity.Y * -1;
            }
            if (center.Y + radius >= rect.Bottom)
            {
                //float difference = (center.y+radius) - rect.bottom;
                //center.y -= (2 * difference);
                center.Y = (center.Y * -1) - (2 * radius) + (2 * rect.Bottom);
                velocity.Y = velocity.Y * -1;
            }
            if (center.X + radius >= rect.Right)
            {
                //float difference = (center.x + radius) - rect.right;
                //center.x -= (2 * difference);
                center.X = (center.X * -1) - (2 * radius) + (2 * rect.Right);
                velocity.X = velocity.X * -1;

            }

        }

        /// <summary>
        /// Fill and outline the sphere on the graphics display
        /// </summary>
        /// <param name="gr"></param>
        public void Draw(Graphics gr)
        {
            gr.FillEllipse(brush, center.X - radius, center.Y - radius, 2 * radius, 2 * radius);
            gr.DrawEllipse(pen, center.X - radius, center.Y - radius, 2 * radius, 2 * radius);

        }






        // Ball to Ball collision code
        public bool IsColliding(Sphere2D otherBall)
        {
            // measure the distance between the centers of the balls
            Point2D difference = this.center - otherBall.center;
            float distance = difference.Magnitude;
            // if distance is less than sum of radii, we collide
            if (distance < this.radius + otherBall.Radius)
                return true;
            else
                return false;
        }


        public void PerformCollision(Sphere2D otherBall)
        {
            Point2D difference = this.center - otherBall.center;
            float distance = difference.Magnitude;
            if (distance == 0) return; // what were you thinking?


            // minimum translation distance mtd will be used to seperate the balls
            Point2D mtd = difference * (this.radius + otherBall.Radius - distance) / distance * 1.1f;


            // get the inverse of masses
            float myMassInverse = 1 / mass;
            float otherMassInverse = 1 / otherBall.Mass;


            // push the balls apart the minimum translation distance based on their relative mass
            Point2D center = mtd * (myMassInverse / (myMassInverse + otherMassInverse));
            this.center.X += center.X;
            this.center.Y += center.Y;
            Point2D centerotherBall = mtd * (otherMassInverse / (myMassInverse + otherMassInverse));
            otherBall.center.X -= centerotherBall.X;
            otherBall.center.Y -= centerotherBall.Y;


            // now normalize the mtd to get a unit vector in the mtd direction
            mtd /= mtd.Magnitude; ;


            // impact speed
            Point2D v = this.velocity - otherBall.Velocity;
            float v_dot_mtd = v * mtd;
            if (float.IsNaN(v_dot_mtd)) return; // vn was Not A Number


            // if the balls are intersecting but already moving apart - do nothing
            if (v_dot_mtd > 0) return;


            // work out the collision effect
            float i = -(1.0f + elasticity) * v_dot_mtd / (myMassInverse + otherMassInverse);


            Point2D impulse = mtd * i;


            // change the ball's velocities
            this.velocity += impulse * myMassInverse;
            otherBall.Velocity -= impulse * otherMassInverse;
        }







        #endregion


    }
}
