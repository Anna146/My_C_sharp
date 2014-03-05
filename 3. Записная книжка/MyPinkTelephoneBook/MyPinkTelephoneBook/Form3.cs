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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            List<Contact> tmp = Form1.phbk.birthDay();
            try
            {
                Insert.Rows.Clear();
                Insert.ColumnCount = 4;
                Insert.Columns[0].HeaderText = "Surname";
                Insert.Columns[1].HeaderText = "First Name";
                Insert.Columns[2].HeaderText = "Second Name";
                Insert.Columns[3].HeaderText = "Birthday";
                Insert.RowCount = tmp.Count;
                for (int i = 0; i < tmp.Count; i++)
                {
                    Insert.Rows[i].Cells[0].Value = tmp[i].Surname;
                    Insert.Rows[i].Cells[1].Value = tmp[i].Name;
                    Insert.Rows[i].Cells[2].Value = tmp[i].SecName;
                    Insert.Rows[i].Cells[3].Value = tmp[i].birth.ToString();
                }
                Insert.Show();
            }
            catch (Exception err)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
