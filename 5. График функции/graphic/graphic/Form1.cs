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
        System.Drawing.Graphics grph;
        
        float wid, hei;

        float leBord, rBord, uBord, loBord;

        public Form1()
        {
            InitializeComponent();

        }

        PictureBox pic = new PictureBox();
        //Rectangle rec = new Rectangle();
        private int begin_x;
        private int begin_y;
        bool resize = false;
        


        private void makeCanvas() 
        {
            Pen point = new System.Drawing.Pen(System.Drawing.Color.Black);
            grph = this.splitContainer1.Panel1.CreateGraphics();
            draw_axes(grph, point);
            point.Dispose();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            pic.Parent = splitContainer1.Panel1;
            pic.BackColor = Color.Transparent;
            pic.BackgroundImage = null;
            pic.SizeMode = PictureBoxSizeMode.AutoSize;
            pic.BorderStyle = BorderStyle.FixedSingle;
            pic.Visible = false;
            counts();
            makeCanvas();
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
                    grph.DrawString((yRu - i/stepY).ToString("f2"), this.Font, br, wid / 2 - 35, (float)i);
            }
            br.Dispose();
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

        string func = "";

        //System.Drawing.Drawing2D.GraphicsState saveGr;

        private void makeGraph(object sender)
        {
            if (func != "" && sender == this.draw)
            {
                String expr = func;
                System.Drawing.Pen point;
                point = new System.Drawing.Pen(System.Drawing.Color.Black);
                //grph = this.splitContainer1.Panel1.CreateGraphics();
                makeCanvas();
                //draw_axes(grph, point);
                point = penStyle();
                float y, xp = float.NaN, yp = float.NaN;
                float eps = 0;//hei/1000000;

                for (double i = 0; i < wid; i += wid / 5000)
                {
                    y = (float)(cl.Calculator(expr, (xRl + i/stepX)));
                    if (!xp.Equals(float.NaN) && !(((y < (float)yRl + eps) || (yp > (float)yRu - eps)) || ((yp < (float)yRl + eps) || (y > (float)yRu - eps))))
                    {
                        grph.DrawLine(point, xp, (float)(yRu - yp) * (float)stepY, (float)i, ((float)yRu - y) * (float)stepY);
                    }
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

        private void counts()
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
            stepX = wid / (Math.Abs(xRl - xRu));
            stepY = hei / (Math.Abs(yRl - yRu));
        }

        private void draw_Click(object sender, EventArgs e)
        {
            func = textBox1.Text;
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
            stepX = wid / (Math.Abs(xRl - xRu));
            stepY = hei / (Math.Abs(yRl - yRu));

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
                System.Drawing.Pen recPen;
                recPen = new System.Drawing.Pen(System.Drawing.Color.Black);
                pic.Width = e.X - begin_x;
                pic.Height = e.Y - begin_y;
                //makeGraph(this.draw);
                //if ((e.X - begin_x > 0) && (e.Y - begin_y > 0))
                    //grph.DrawRectangle(recPen, begin_x, begin_y, e.X - begin_x, e.Y - begin_y);
                //grph.Restore(saveGr);
                //saveGr = grph.Save();
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
                //saveGr = grph.Save();
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

                    xRl = (int)(xRl + (float)begin_x / (float)stepX);
                    xRu = -(int)(xRu + (- e.X) / stepX);
                    yRu = (int)(yRu - begin_y / stepY);
                    yRl = -(int)(yRl + e.Y / stepY);
                    stepX = wid / (Math.Abs(xRl-xRu));
                    stepY = hei / (Math.Abs(yRl-yRu));
                    makeGraph(this.draw);
                
                
                }
            }
            resize = false;
        }



        
    }
}
