using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace EiEProject
{
    class Line2D
    {
        Point2D p1 = new Point2D(), p2 = new Point2D();
        Sphere2D[] b = new Sphere2D[2];
        Point2D velocity = new Point2D(0, 0);

        public Line2D(Point2D p1, Point2D p2)
        {
            this.p1 = p1;
            this.p2 = p2;
            b[0] = new Sphere2D(new Point2D(), 0.01);
            b[1] = new Sphere2D(new Point2D(), 0.01);
            b[0].Center = p1;
            b[1].Center = p2;
            b[0].Mass = float.MaxValue;
            b[1].Mass = float.MaxValue;
        }

        public Point2D P1 { get { return p1; } set { p1 = value; } }
        public Point2D P2 { get { return p2; } set { p2 = value; } }
        public Point2D Velocity { get { return velocity; } set { velocity = value; } }

        public void Draw(Graphics gr, Pen pen)
        {
            gr.DrawLine(pen, p1.PointF, p2.PointF);
        }
        public void Move()
        {
            p1.X += velocity.X;
            p2.X += velocity.X;
            p1.Y += velocity.Y;
            p2.Y += velocity.Y;
        }

        public Point2D Normal2Ball(Sphere2D ball)
        {
            Point2D v = p2 - p1;
            Point2D ballToLine = ball.Center - p1;
            Point2D normal = v.Normal;
            normal.Normalize();
            if (normal * ballToLine < 0)
                normal *= -1;
            return normal;
        }

        public bool Contains(Point2D p)
        {
            RectangleF rect = new RectangleF(Math.Min(p1.X, p2.X) - .5f, Math.Min(p1.Y, p2.Y) - .5f, Math.Abs(p1.X - p2.X) + 1f, Math.Abs(p1.Y - p2.Y) + 1f);
            return rect.Contains(p.PointF);
        }

        public Point2D LineIntersectionPoint(Line2D otherLine)
        {
            // Get A,B,C of first line - points : P1 to P2
            float A1 = P2.Y - P1.Y;
            float B1 = P1.X - P2.X;
            float C1 = A1 * P1.X + B1 * P1.Y;

            // Get A,B,C of second line - points : ps2 to pe2
            float A2 = otherLine.P2.Y - otherLine.P1.Y;
            float B2 = otherLine.P1.X - otherLine.P2.X;
            float C2 = A2 * otherLine.P1.X + B2 * otherLine.P1.Y;

            // Get delta and check if the lines are parallel
            float delta = A1 * B2 - A2 * B1;
            if (delta == 0)
                return new Point2D(float.MaxValue, float.MaxValue);
            //throw new System.Exception("Lines are parallel");

            // now return the Vector2 intersection point
            return new Point2D(
                (B2 * C1 - B1 * C2) / delta,
                (A1 * C2 - A2 * C1) / delta
            );
        }
        /// <summary>
        /// Computes the reflected segment at a point of a curve
        /// The line is the segment being reflected
        /// The normal is the normal of the reflection line and the ball
        /// </summary>
        /// <param name="normal"></param>
        /// <returns></returns>
        public Point2D Reflection(Point2D normal)
        {
            double rx, ry;
            Point2D direction = P2 - P1;
            float dot = direction * normal;
            rx = direction.X - 2 * normal.X * dot;
            ry = direction.Y - 2 * normal.Y * dot;
            return new Point2D(rx, ry);
        }

        /// <summary>
        /// return the center of the ball if it is reflected
        /// </summary>
        /// <param name="ball"></param>
        /// <returns></returns>

        public bool BallBounce(Sphere2D ball)
        {
            // determine the normal vector ball the reflection line towards the ball
            Point2D normal = Normal2Ball(ball);
            // this next line is just to be able to see the normal line
            Line2D normalLine = new Line2D(P1, P2);
            normalLine.P2 = P1 + Normal2Ball(ball) * ball.Radius;
            // we will reflect from the center of the ball so we "move" the line towards the ball by the normal a distance of radius
            Line2D aLineTemp = new Line2D(P1 + normal * ball.Radius, P2 + normal * ball.Radius);
            // where will the ball be in one step?
            Point2D ballNextStep = ball.Center + ball.Velocity;
            // calculate the path of the ball
            Line2D ballPath = new Line2D(ball.Center, ballNextStep);
            // find the point of intersection between the ball and the reflection line
            Point2D intersectionPt = aLineTemp.LineIntersectionPoint(ballPath);

            float dot = normal * ball.Velocity;

            if (b[0].IsColliding(ball))
            {
                b[0].PerformCollision(ball);
                return true;
            }
            else if (b[1].IsColliding(ball))
            {
                b[1].PerformCollision(ball);
                return true;
            }// if the intersection point is on the reflection line and the ball is moving towards the reflection line
            // if we don't travel as far as the intersection point, no reflection and moving towards the reflection line
            else if (ball.Velocity.Magnitude < (intersectionPt - ball.Center).Magnitude && normal * ball.Velocity < 0)
                return false;
            else if (aLineTemp.Contains(intersectionPt) && normal * ball.Velocity < 0)
            {
                Line2D reflectionLine = new Line2D(ball.Center + ball.Velocity, intersectionPt);
                Point2D velDirection = -1 * reflectionLine.Reflection(Normal2Ball(ball));
                velDirection.Normalize();
                ball.Velocity = velDirection * ball.Velocity.Magnitude;
                ball.Center = intersectionPt - reflectionLine.Reflection(Normal2Ball(ball)) * ball.Elasticity;
                return true;
            }
            // else there is no reflection
            else
                return false;
        }
    }
}
