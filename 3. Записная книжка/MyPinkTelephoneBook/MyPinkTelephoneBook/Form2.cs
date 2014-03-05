using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace MyPinkTelephoneBook
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            Insert.ColumnCount = 7;
            Insert.Columns[0].HeaderText = "Surname";
            Insert.Columns[1].HeaderText = "First Name";
            Insert.Columns[2].HeaderText = "Second Name";
            Insert.Columns[3].HeaderText = "Birthday";
            Insert.Columns[4].HeaderText = "Emails";
            Insert.Columns[5].HeaderText = "Telephones";
            Insert.Columns[6].HeaderText = "Adresses";
        }

        public bool validate(String[] strs, String ptr)
        {
            Regex rx = new Regex(ptr);

            foreach (String s in strs)
            {
                if (!rx.IsMatch(s))
                    return false;
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Insert.Rows[0].Cells[0].Value == null || Insert.Rows[0].Cells[1].Value == null || Insert.Rows[0].Cells[2].Value == null || Insert.Rows[0].Cells[3].Value == null || Insert.Rows[0].Cells[4].Value == null || Insert.Rows[0].Cells[5].Value == null || Insert.Rows[0].Cells[6].Value == null)
                    throw new Exception("Please fill all the fields");
                String[] val1;
                val1 = Insert.Rows[0].Cells[4].Value.ToString().Split(';');
                if (!validate(val1, "^[_A-Za-z0-9-]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9-]+)*(\\.[A-Za-z]{2,})$"))
                {
                    MessageBox.Show("Your emails are incorrect. Try again", "Error",
                                     MessageBoxButtons.OK);
                }
                else
                {
                    val1 = Insert.Rows[0].Cells[5].Value.ToString().Split(';');
                    if (!validate(val1, "^\\(([0-9]){3}\\)([0-9]){7}$"))
                    {
                        MessageBox.Show("Your telephones are incorrect. Try again", "Error",
                                         MessageBoxButtons.OK);
                    }
                    else
                    { 
                        String tmp = Insert.Rows[0].Cells[0].Value.ToString() + " " + Insert.Rows[0].Cells[1].Value.ToString() + " " + Insert.Rows[0].Cells[2].Value.ToString() + " " + Insert.Rows[0].Cells[3].Value.ToString() + " " + Insert.Rows[0].Cells[4].Value.ToString() + " " + Insert.Rows[0].Cells[5].Value.ToString() + " " + Insert.Rows[0].Cells[6].Value.ToString();
                        Form1.phbk.addContactString(tmp);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                                         MessageBoxButtons.OK);
            }
        }

        public event EventHandler<MyEventArgs> OnButtonClicked;

        private void Insert_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        

    }

    public class MyEventArgs: EventArgs
    { 
        public string Value {get; set;}
    }
}
