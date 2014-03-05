using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Calculator;

namespace graphic
{
    public partial class Form1 : Form
    {
        Calc cl = new Calc();

        double xRl, yRl, xRu, yRu;

        double stepX, stepY;

        float wid, hei;

        float leBord, rBord, uBord, loBord;

        public Form1()
        {
            InitializeComponent();

        }

        PictureBox pic = new PictureBox();
        private int begin_x;
        private int begin_y;
        bool resize = false;


        private void Form1_Load(object sender, EventArgs e)
        {
            pic.Parent = splitContainer1.Panel1;
            pic.BackColor = Color.Transparent;
            pic.SizeMode = PictureBoxSizeMode.AutoSize;
            pic.BorderStyle = BorderStyle.FixedSingle;
            pic.Visible = false;
        }

        private void draw_axes(System.Drawing.Graphics grph, System.Drawing.Pen pen)
        {
            grph.Clear(Color.White);
            System.Drawing.Brush br;
            br  = new System.Drawing.SolidBrush(Color.Black);
            grph.DrawLine(pen, leBord, hei / 2 + uBord, wid + leBord, hei / 2 + uBord);
            grph.DrawLine(pen, leBord + wid / 2, uBord, leBord + wid / 2, hei + uBord);
            for (double i = 0; i < wid; i += wid/10) {
                grph.DrawLine(pen, (float)i, hei / 2 - 3, (float)i, hei / 2 + 3);
                grph.DrawString((xRl + i/stepX).ToString("f2"), this.Font,br, (float)i, hei / 2 + 5);
            }
            for (double i = 0; i < hei; i += hei / 10)
            {
                grph.DrawLine(pen, wid / 2 - 3, (float)i, wid / 2 + 3, (float)i);
                if (!((i - hei / 2) == 0))
                    grph.DrawString((yRu - i/stepY).ToString("f2"), this.Font, br, wid / 2 - 25, (float)i);
            }
        }

        private Pen penStyle()
        {
            System.Drawing.Pen p;
            p = new System.Drawing.Pen(System.Drawing.Color.Black);
            if (listBox1.SelectedItem != null)
            {
                switch (listBox1.SelectedItem.ToString())
                {
                    case "line":
                        p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                        break;
                    case "dash":
                        p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        break;
                    case "dot":
                        p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                        break;
                }
            }
            p.Width = (float)this.numericUpDown1.Value;
            return p;
        }

        System.Drawing.Graphics grph;
        //System.Drawing.Drawing2D.GraphicsState saveGr;

        private void makeGraph(object sender)
        {
            if (textBox1.Text != "" && sender == this.draw)
            {
                String expr = this.textBox1.Text;
                System.Drawing.Pen point;
                point = new System.Drawing.Pen(System.Drawing.Color.Black);
                grph = this.splitContainer1.Panel1.CreateGraphics();
                draw_axes(grph, point);
                point = penStyle();
                float y, xp = float.NaN, yp = float.NaN;


                for (double i = 0; i < wid; i += wid / 1000)
                {
                    y = (float)(cl.Calculator(expr, (xRl + i/stepX)));
                    if (!xp.Equals(float.NaN) && !(yp.Equals(float.NegativeInfinity)) && !(yp.Equals(float.PositiveInfinity)))
                        grph.DrawLine(point, xp, (float)(yRu - yp) * (float)stepY, (float)i, ((float)yRu  - y) * (float)stepY);
                    xp = (float)i;
                    yp = y;
                }
                point.Dispose();
                //saveGr = grph.Save();
                grph.Dispose();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            makeGraph(sender);
        }

        private void draw_Click(object sender, EventArgs e)
        {
            xRl = double.Parse(textBox2.Text);
            xRu = double.Parse(textBox3.Text);
            yRl = double.Parse(textBox4.Text);
            yRu = double.Parse(textBox5.Text);
            wid = this.splitContainer1.Panel1.Width;
            hei = this.splitContainer1.Panel1.Height;
            leBord = this.splitContainer1.Panel1.Bounds.Left;
            rBord = this.splitContainer1.Panel1.Bounds.Right;
            uBord = this.splitContainer1.Panel1.Bounds.Top;
            loBord = this.splitContainer1.Panel1.Bounds.Bottom;
            stepX = wid / (Math.Abs(xRl-xRu));
            stepY = hei / (Math.Abs(yRl-yRu));
            makeGraph(sender);
        }


        private void onCl(object sender, MouseEventArgs e)
        {
        }

        

        private void button1_Click(object sender, EventArgs e)
        {

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

                    xRl = (int)(xRl + (begin_x - leBord)/stepX);
                    xRu = (int)(xRu - ( - e.X + rBord) / stepX);
                    yRu = -(int)(yRl + (begin_y - uBord) / stepY);
                    yRl = -(int)(yRu - ( -e.Y + loBord) / stepY);
                    stepX = wid / (Math.Abs(xRl-xRu));
                    stepY = hei / (Math.Abs(yRl-yRu));
                    makeGraph(this.draw);
                
                
                }
            }
            resize = false;
        }


    }
}
