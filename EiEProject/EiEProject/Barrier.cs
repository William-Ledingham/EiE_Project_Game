using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace EiEProject
{
    class Barrier
    {
        float  XPosition;
        Rectangle2D obstacleA = new Rectangle2D(new Point2D(), 0, 0);
        Rectangle2D obstacleB = new Rectangle2D(new Point2D(), 0, 0);
        Point2D velocity = new Point2D(-10, 0);
        int playerInsideBarrier = 0;

        float formX = 1400;
        float formY = 750;

        public Barrier(float X, float width)
        {
            this.XPosition = X;
            this.obstacleA.Width = width;
            this.obstacleA.Center.X = X;
            this.obstacleA.Center.Y = 0;
            this.obstacleB.Width = width;
            this.obstacleB.Center.X = X;
            this.obstacleB.Center.Y = formY;
            this.obstacleA.Velocity = velocity;
            this.obstacleB.Velocity = velocity;
            gapGenerator();
            
        }

        public Point2D Velocity { get { return velocity; } set { velocity = value; obstacleA.Velocity = value;} }
        public float CenterX { get { return XPosition; }
            set
            {
                XPosition = value;
                obstacleA.Center.X = value;
                obstacleB.Center.X = value;
            }
        }

        public int PlayerInsideBarrier { get { return playerInsideBarrier; } set { playerInsideBarrier = value; } }

        public void gapGenerator()
        {
            Random rand = new Random();
            float gap = rand.Next(200, 400);

            Random r = new Random();
            int offset = r.Next(0, (int)(formY - gap));

            obstacleA.Height = offset * 2;
            obstacleB.Height = (formY - (gap + offset)) * 2;



        }

        public void Move()
        {
            XPosition += velocity.X;
            obstacleA.movingBarrier();
            obstacleB.movingBarrier();

        }
        public void Draw(Graphics gr)
        {
            obstacleA.Draw(gr);
            obstacleA.Fill(gr);
            obstacleB.Draw(gr);
            obstacleB.Fill(gr);

        }
        public int playerHitBarrier(Sphere2D player)
        {
            
            if ((player.Center.X + player.Radius >= obstacleA.Center.X - obstacleA.Width / 2 && player.Center.X - player.Radius <= obstacleA.Center.X + obstacleA.Width / 2 &&
                player.Center.Y - player.Radius <= obstacleA.Center.Y + obstacleA.Height / 2) || (
                player.Center.X + player.Radius >= obstacleB.Center.X - obstacleB.Width / 2 && player.Center.X - player.Radius <= obstacleB.Center.X + obstacleB.Width / 2 &&
                player.Center.Y + player.Radius >= obstacleB.Center.Y - obstacleB.Height/2))
            {
                return 1;
            }
            return 0;
        }
    }
}
