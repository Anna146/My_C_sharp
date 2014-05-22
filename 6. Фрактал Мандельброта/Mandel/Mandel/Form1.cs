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
            this.comboBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.comboBox1_MouseWheel);
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
                                grph.DrawEllipse(point, (float)((realCoord - lBord) * koefX), (float)((imagCoord - tBord) * koefY), 5, 5);
                                break;
                            case 2:
                                grph.DrawEllipse(pointR, (float)((realCoord - lBord) * koefX), (float)((imagCoord - tBord) * koefY), 5, 5);
                                break;
                            case 3:
                                grph.DrawEllipse(pointB, (float)((realCoord - lBord) * koefX), (float)((imagCoord - tBord) * koefY), 5, 5);
                                break;
                        }
                    }
                }
            }
        }

        int thCount = 100;

        private void pens()
        {
            switch (this.comboBox4.Text)
            {
                case "Green":
                    point = new System.Drawing.Pen(System.Drawing.Color.Lime);
                    break;
                case "Yellow":
                    point = new System.Drawing.Pen(System.Drawing.Color.Gold);
                    break;
                case "Red":
                    point = new System.Drawing.Pen(System.Drawing.Color.Red);
                    break;
                case "Blue":
                    point = new System.Drawing.Pen(System.Drawing.Color.Blue);
                    break;
                case "Black":
                    point = new System.Drawing.Pen(System.Drawing.Color.Black);
                    break;
                default:
                    point = new System.Drawing.Pen(System.Drawing.Color.Black);
                    break;
            }
        
            
            switch (this.comboBox2.Text)
            {
                case "Green":
                    pointR = new System.Drawing.Pen(System.Drawing.Color.Lime);
                    break;
                case "Yellow":
                    pointR = new System.Drawing.Pen(System.Drawing.Color.Gold);
                    break;
                case "Red":
                    pointR = new System.Drawing.Pen(System.Drawing.Color.Red);
                    break;
                case "Blue":
                    pointR = new System.Drawing.Pen(System.Drawing.Color.Blue);
                    break;
                case "Black":
                    pointR = new System.Drawing.Pen(System.Drawing.Color.Black);
                    break;
                default:
                    pointR = new System.Drawing.Pen(System.Drawing.Color.Red);
                    break;
            }

            switch (this.comboBox3.Text)
            {
                case "Green":
                    pointB = new System.Drawing.Pen(System.Drawing.Color.Lime);
                    break;
                case "Yellow":
                    pointB = new System.Drawing.Pen(System.Drawing.Color.Gold);
                    break;
                case "Red":
                    pointB = new System.Drawing.Pen(System.Drawing.Color.Red);
                    break;
                case "Blue":
                    pointB = new System.Drawing.Pen(System.Drawing.Color.Blue);
                    break;
                case "Black":
                    pointB = new System.Drawing.Pen(System.Drawing.Color.Black);
                    break;
                default:
                    pointB = new System.Drawing.Pen(System.Drawing.Color.Gold);
                    break;
            }
            grph = this.panel1.CreateGraphics();


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

        double inc = 1;

        bool roll = false;
        double c1 = -0.6;
        double c2 = 1.77;
        double c3 = -1.2;
        double c4 = 1.2;


        private void comboBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("works!!");
            pic.Width = 0;
            pic.Height = 0;
            inc = e.Delta < 0 ? (1.25) : (0.75);
            pic.Visible = false;
            if (c1 * inc > -0.6 && c2 * inc < 1.77 && c3 * inc > -1.6 && c4 * inc < 1.6)
            {
                roll = true;
                //Rectangle rec = new Rectangle(begin_x, begin_y, e.X - begin_x, e.Y - begin_y);
                grph.Clear(Color.White);
                picture(c1 * inc, c2 * inc, c3 * inc, c4 * inc);
                c1 = c1 * inc;
                c2 = c2 * inc;
                c3 = c3 * inc;
                c4 = c4 * inc;
                inc = 1;
            }
            else
            {
                roll = true;
                inc = 1;
                c1 = -0.6;
                c2 = 1.77;
                c3 = -1.2;
                c4 = 1.2;
                grph.Clear(Color.White);
                picture(c1 / inc, c2 / inc, c3 / inc, c4 / inc);
            }
            resize = false;
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
                    c1 = begin_x / koefX + lBord;
                    c2 = e.X / koefX + lBord;
                    c3 = begin_y / koefY + tBord;
                    c4 = e.Y / koefY + tBord;
                    inc = 1;
                    grph.Clear(Color.White);
                    picture(begin_x / koefX + lBord, e.X / koefX + lBord, begin_y / koefY + tBord, e.Y / koefY + tBord);
                }
            }
            resize = false;
            this.Activate();
        }

        void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            grph.Clear(Color.White);
            picture(-0.6, 1.77, -1.2, 1.2);
            roll = false;
            inc = 1;
            c1 = -0.6;
            c2 = 1.77;
            c3 = -1.2;
            c4 = 1.2;
            this.comboBox1.Focus();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            grph.Clear(Color.White);
            picture(-0.6, 1.77, -1.2, 1.2);
        }


        private void comboBox2_TextChanged_1(object sender, EventArgs e)
        {
            grph.Clear(Color.White);
            picture(-0.6, 1.77, -1.2, 1.2);
        }

        private void comboBox3_TextChanged_1(object sender, EventArgs e)
        {
            grph.Clear(Color.White);
            picture(-0.6, 1.77, -1.2, 1.2);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            String text = textBox1.Text;
            int cnt;
            if (int.TryParse(text, out cnt) == false)
            {
                MessageBox.Show("Invalid number of threads!");
                textBox1.Text = thCount.ToString();
            }
            if (cnt >= 1 && cnt <= 1000)
            {
                thCount = cnt;
                grph.Clear(Color.White);
                picture(-0.6, 1.77, -1.2, 1.2);
            }
            else
            {
                MessageBox.Show("Invalid number of threads!");
                textBox1.Text = thCount.ToString();
            }
            this.comboBox1.Focus();
        }
    }
}
