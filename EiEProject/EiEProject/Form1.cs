using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ANT_Managed_Library;
using System.Threading;

namespace EiEProject
{
    public partial class Form1 : Form
    {

        static ANT slaveChannel;
        static ANT masterChannel;

        int playerLifes = 3;
        int score = 0;

        Sphere2D player;

        Rectangle2D ground;
        Line2D groundLine;
        Line2D topLine;

        //Rectangle2D Obstacle1A;
        //Rectangle2D Obstacle1B;
        Barrier Obstacle1;
        Barrier Obstacle2;


        public Form1()
        {
            InitializeComponent();
            this.ClientSize = new Size(1200, 600);

            masterChannel = new ANT();
            masterChannel.UcChannelType = 0;
            ThreadStart antRefMaster = new ThreadStart(masterChannel.startANT);
            Thread antThreadMaster = new Thread(antRefMaster);
            antThreadMaster.Start();


            //ground = new Rectangle2D(new Point2D(600, 425), 1100, 50);
            topLine = new Line2D(new Point2D(0, 0), new Point2D(1200, 0));
            groundLine = new Line2D(new Point2D(0, 600), new Point2D(1200, 600));
            /*
            Obstacle1B = new Rectangle2D(new Point2D(1150, 400-25), 20, 50);
            Obstacle1B.Velocity = new Point2D(-10, 0);
            */
            Obstacle1 = new Barrier(1210, 20);
            Obstacle2 = new Barrier(1210 + 610, 20);
            player = new Sphere2D(new Point2D(200, 350), 25);

            updatePlayerLifes(playerLifes);

        }

        private void restartGame()
        {
            Obstacle1.CenterX = 1210;
            Obstacle2.CenterX = 1210 + 610;
            Obstacle1.Velocity.X = -10;
            Obstacle2.Velocity.X = -10;
            score = 0;
            playerLifes = 3;
            byte l = Convert.ToByte(3);
            masterChannel.TxBuffer[7] = l;
            player.Center.Y = 550;
            lblGameOver.Text = "";
            timer2.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            ThreadStart antRef = new ThreadStart(ANTStart);
            Thread antThread = new Thread(antRef);
            antThread.Start();
            */
            /*
            slaveChannel = new ANT();
            slaveChannel.UcChannelType = 1;
            ThreadStart antRefSlave = new ThreadStart(slaveChannel.startANT);
            Thread antThreadSlave = new Thread(antRefSlave);
            antThreadSlave.Start();
            */

            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }


        private void timer2_Tick(object sender, EventArgs e)
        {
            
            if (masterChannel.Button0Pressed == 1)
            {
                label1.Text = "button 0 pressed";
                SendKeys.Send(" ");
                //button0Pressed = 0;
            }
            if(masterChannel.Button1Pressed == 1)
            {
                label1.Text = "button 1 pressed";
                SendKeys.Send("{UP}");
                ///button1Pressed = 0;
            }
            else
                label1.Text = " ";
                
            player.Move();

            groundLine.BallBounce(player);
            topLine.BallBounce(player);
            if(Obstacle1.playerHitBarrier(player) == 1 && Obstacle1.PlayerInsideBarrier == 0)
            {
                Obstacle1.PlayerInsideBarrier = 1;
                updatePlayerLifes(--playerLifes);
            }
            else if(Obstacle1.playerHitBarrier(player) == 0 && Obstacle1.PlayerInsideBarrier == 1)
            {
                Obstacle1.PlayerInsideBarrier = 0;
            }
            if (Obstacle2.playerHitBarrier(player) == 1 && Obstacle2.PlayerInsideBarrier == 0)
            {
                Obstacle2.PlayerInsideBarrier = 1;
                updatePlayerLifes(--playerLifes);
            }
            else if (Obstacle2.playerHitBarrier(player) == 0 && Obstacle2.PlayerInsideBarrier == 1)
            {
                Obstacle2.PlayerInsideBarrier = 0;
            }


            Obstacle1.Move();
            Obstacle2.Move();
            checkObstacle();
            this.Invalidate();
        }

        private void Form1_Paint_1(object sender, PaintEventArgs e)
        {
            Obstacle1.Draw(e.Graphics);
            Obstacle2.Draw(e.Graphics);
            player.Draw(e.Graphics);
        }

        private void createObstacle()
        {

        }

        private void updatePlayerLifes(int lifes)
        {
            if(lifes == 0)
            {
                lblGameOver.Text = "GAME OVER";
                timer2.Enabled = false;
            }
            byte l = Convert.ToByte(lifes);
            masterChannel.TxBuffer[7] = l;

        }

        private void playerJump()
        {
            player.Velocity = new Point2D(0, -10);
        }

        private void checkObstacle()
        {
            if(Obstacle1.CenterX <= -10)
            {
                score++;
                if (Obstacle1.Velocity.X >= -15)
                {
                    Obstacle1.Velocity.X = --Obstacle1.Velocity.X;
                    Obstacle2.Velocity.X = --Obstacle2.Velocity.X;
                }
                byte s = Convert.ToByte(score);
                masterChannel.TxBuffer[6] = s;
                Obstacle1.CenterX = 1210;
                Obstacle1.gapGenerator();
            }
            if(Obstacle2.CenterX <= -10)
            {
                score += 1;
                if (Obstacle2.Velocity.X >= -15)
                {
                    Obstacle2.Velocity.X = --Obstacle2.Velocity.X;
                    Obstacle1.Velocity.X = --Obstacle1.Velocity.X;
                }
                byte s = Convert.ToByte(score);
                masterChannel.TxBuffer[6] = s;
                Obstacle2.CenterX = 1210;
                Obstacle2.gapGenerator();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space)
            {
                playerJump();
            }
            if(e.KeyCode == Keys.Space && playerLifes == 0)
            {
                restartGame();
            }
        }
    }
}
