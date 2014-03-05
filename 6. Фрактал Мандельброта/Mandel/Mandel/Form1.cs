using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;

namespace Mandel
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        
            System.Drawing.Pen point, pointR, pointB;
            System.Drawing.Graphics grph;
            double koefX, koefY;
            double lBord = -0.6;
            double rBord = 1.77;
            double tBord = -1.2;
            double bBord = 1.2;


        void onePoint(object mass)
        {
            double L = ((double[])mass)[0];
            double R = ((double[])mass)[1];
            double B = ((double[])mass)[2];
            double T = ((double[])mass)[3];
            double stepX = (rBord - lBord) * 0.03 / 2.33;
            double stepY = (bBord - tBord) * 0.05 / 2.4;
            double realCoord, imagCoord;
            double realTemp, imagTemp, realTemp2, arg;
            int iterations;
            for (imagCoord = T; imagCoord >= B; imagCoord -= stepY)
            {
                for (realCoord = L; realCoord <= R; realCoord += stepX)
                {
                    iterations = 0;
                    realTemp = realCoord;
                    imagTemp = imagCoord;
                    arg = (realCoord * realCoord) + (imagCoord * imagCoord);
                    while ((arg < 4) && (iterations < 40))
                    {
                        realTemp2 = (realTemp * realTemp) - (imagTemp * imagTemp) - realCoord;
                        imagTemp = (2 * realTemp * imagTemp) - imagCoord;
                        realTemp = realTemp2;
                        arg = (realTemp * realTemp) + (imagTemp * imagTemp);
                        iterations += 1;
                    }
                    lock (grph)
                    {
                        switch (iterations % 4)
                        {
                            case 0:
                                grph.DrawEllipse(point, (float)((realCoord - lBord) * koefX), (float)((imagCoord - tBord) * koefY), 1, 1);
                                break;
                            case 1:
                                grph.DrawEllipse(point, (float)((realCoord - lBord) * koefX), (float)((imagCoord - tBord) * koefY), 2, 2);
                                break;
                            case 2:
                                grph.DrawEllipse(pointR, (float)((realCoord - lBord) * koefX), (float)((imagCoord - tBord) * koefY), 2, 2);
                                break;
                            case 3:
                                grph.DrawEllipse(pointB, (float)((realCoord - lBord) * koefX), (float)((imagCoord - tBord) * koefY), 2, 2);
                                break;
                        }
                    }
                }
            }
            //MessageBox.Show("I did points " + R + "-" + L+ " and " + T + "-" + B);
        }

        int thCount = 1;

        private void pens()
        {

            grph = this.panel1.CreateGraphics();

            point = new System.Drawing.Pen(System.Drawing.Color.Black);
            pointR = new System.Drawing.Pen(System.Drawing.Color.Red);
            pointB = new System.Drawing.Pen(System.Drawing.Color.Blue);

        }

        PictureBox pic = new PictureBox();
        private int begin_x;
        private int begin_y;
        bool resize = false;


        private void picture(double l, double r, double t, double b)
        {
            lBord = l;
            rBord = r;
            tBord = t;
            bBord = b;
            koefX = 1 / (rBord - lBord) * panel1.Width;
            koefY = 1 / (bBord - tBord) * panel1.Height;
            pens();
            if (t<0)
                grph.DrawLine(point, 0, (float)(-t * koefY), panel1.Width, (float)(-t * koefY));
            if (l<0)
                grph.DrawLine(point, (float)(-l * koefY), 0, (float)(-l * koefY), panel1.Height);

            double portY = (b-t) / thCount;


            double[] mass = new double[4];

            //-----------------
            /*
            mass[0] = l;
            mass[1] = r;
            mass[2] = t;
            mass[3] = b;
            onePoint(mass);
            
            */
            //-----------------------
            Thread[] pool = new Thread[thCount];
            for (int i = 0; i < thCount; i++)
            {
                mass[0] = l;
                mass[1] = r;
                mass[2] = i * portY + t;
                mass[3] = (i + 1) * portY + t;
                pool[i] = new Thread(onePoint);
                pool[i].Start(mass);
                //MessageBox.Show("thread "+ i+ " started");
                //Thread t = new Thread(onePoint);
                //t.Start(mass);
                //t.Join();
            }
            for (int i = 0; i < thCount; i++)
            {
                pool[i].Join();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pic.Parent = this.panel1;
            pic.BackColor = Color.Transparent;
            pic.SizeMode = PictureBoxSizeMode.AutoSize;
            pic.BorderStyle = BorderStyle.FixedSingle;
            pic.Visible = false;
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            picture(-0.6, 1.77, -1.2, 1.2);
        }

        private void onMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                pic.Width = e.X - begin_x;
                pic.Height = e.Y - begin_y;
                //grph.Restore(saveGr);
            }

        }

        private void onDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                begin_x = e.X;
                begin_y = e.Y;
                pic.Left = e.X;
                pic.Top = e.Y;
                pic.Width = 0;
                pic.Height = 0;
                pic.Visible = true;
                resize = true;
            }
        }



        private void onUp(object sender, MouseEventArgs e)
        {
            pic.Width = 0;
            pic.Height = 0;
            pic.Visible = false;
            if (resize == true)
            {
                if ((e.X > begin_x + 10) && (e.Y > begin_y + 10)) //Чтобы совсем уж мелочь не вырезал - и по случайным нажатиям не срабатывал! (можно убрать +10)
                {
                    Rectangle rec = new Rectangle(begin_x, begin_y, e.X - begin_x, e.Y - begin_y);
                    //this.splitContainer1.Panel1.BackgroundImage = Copy(this.splitContainer1.Panel1.BackgroundImage, rec);
                    grph.Clear(Color.White);
                    //picture(-0.6, 1.77, -1.2, 1.2);
                    picture(begin_x / koefX + lBord, e.X / koefX + lBord, begin_y / koefY + tBord, e.Y / koefY + tBord);
                }
            }
            resize = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            grph.Clear(Color.White);
            picture(-0.6, 1.77, -1.2, 1.2);
        }

    }
}
