using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyPinkTelephoneBook
{
    public partial class Form1 : Form
    {
        public static Book phbk = new Book();

        public String newEntry;

        /*private void chClosed(object sender, EventArgs e)
        {

        }*/

        private void updBase(List<Contact> con)
        {
            try
            {
                Insert.Rows.Clear();
                Insert.ColumnCount = 7;
                Insert.Columns[0].HeaderText = "Surname";
                Insert.Columns[1].HeaderText = "First Name";
                Insert.Columns[2].HeaderText = "Second Name";
                Insert.Columns[3].HeaderText = "Birthday";
                Insert.Columns[4].HeaderText = "Emails";
                Insert.Columns[5].HeaderText = "Telephones";
                Insert.Columns[6].HeaderText = "Adresses";
                Insert.RowCount = phbk.cons.Count;
                for (int i = 0; i < con.Count; i++)
                {
                    Insert.Rows[i].Cells[0].Value = con[i].Surname;
                    Insert.Rows[i].Cells[1].Value = con[i].Name;
                    Insert.Rows[i].Cells[2].Value = con[i].SecName;
                    Insert.Rows[i].Cells[3].Value = con[i].birth.ToString();
                    String tmp = "";
                    for (int j = 0; j < con[i].emails.Length; j++)
                    {
                        tmp += con[i].emails[j];
                        if (j != con[i].emails.Length - 1)
                            tmp += ";";
                    }
                    Insert.Rows[i].Cells[4].Value = tmp;
                    tmp = "";
                    for (int j = 0; j < con[i].phones.Length; j++)
                    {
                        tmp += con[i].phones[j];
                        if (j != con[i].phones.Length - 1)
                            tmp += ";";
                    }
                    Insert.Rows[i].Cells[5].Value = tmp;
                    tmp = "";
                    for (int j = 0; j < con[i].add.Length; j++)
                    {
                        tmp += con[i].add[j].City + "," + con[i].add[j].Street + "," + con[i].add[j].house;
                        if (j != con[i].emails.Length - 1)
                            tmp += ";";
                    }
                    Insert.Rows[i].Cells[6].Value = tmp;
                }
                Insert.Show();
            }
            catch (Exception err)
            {
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            phbk.fromXML("C:/Users/Hp/Desktop/1.txt");
            this.updBase(phbk.cons);
        }

        private void Show_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 ins = new Form2();
            ins.Show();
            ins.FormClosed += (obj, args) =>
            {
                this.updBase(phbk.cons);
            };
            //ins.OnButtonClicked += (obj, args) => 
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            phbk.toXML("C:/Users/Hp/Desktop/1.txt");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            updBase(phbk.searchFIO(textBox1.Text));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            updBase(phbk.searchPhone(textBox1.Text));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() != "null")
            {
                char sel = comboBox1.SelectedItem.ToString()[0];
                updBase((phbk.searchLetter(sel)));
            }
            else
                updBase(phbk.cons);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            phbk.alphabetic();
            updBase(phbk.cons);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form3 bth = new Form3();
            bth.Show();
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            updBase(phbk.cons);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            phbk.delete(Insert.SelectedRows[0].Cells[0].Value.ToString(), Insert.SelectedRows[0].Cells[1].Value.ToString(), Insert.SelectedRows[0].Cells[2].Value.ToString());
            updBase(phbk.cons);
        }
    }
}
