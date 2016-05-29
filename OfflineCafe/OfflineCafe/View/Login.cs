using OfflineCafe.DataAccess;
using OfflineCafe.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OfflineCafe.View
{
    public partial class Login : Form
    {
        public static List<Food> staticeList;
        public List<Food> el;
        public Login()
        {
            InitializeComponent();
        }

        private void btnInsertMenu_Click(object sender, EventArgs e)
        {
            try
            {
                LoginDA lda = new LoginDA();
                List<Entity.Login> ll = lda.searchid2();
                el = lda.searchid(textBox1.Text);
                for (int i = 0; i < ll.Count; i++)
                {
                    if (textBox1.Text == ll.ElementAt(i).loginid && textBox2.Text == ll.ElementAt(i).password)
                    {
                        if (textBox2.Text == el.ElementAt(i).foodName)
                        {
                            FirstTimeLogin.staticeList = el;
                            FirstTimeLogin ftl = new FirstTimeLogin();
                            ftl.Visible = true;
                            this.Visible = false;
                        }
                        else
                        {
                            Form1.staticeList = el;
                            Form1 main = new Form1();
                            main.Visible = true;
                            this.Visible = false;
                            
                        }
                    }
                    else if (textBox1.Text == "" ||textBox2.Text=="")
                    {
                        MessageBox.Show("Please enter the empty field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                       
                        textBox1.Focus();
                    }
                    else if (textBox1.Text != ll.ElementAt(i).loginid)
                    {
                        MessageBox.Show("Login ID does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox1.Text = "";
                        textBox1.Focus();
                    }
                    else if (textBox1.Text == ll.ElementAt(i).loginid && textBox2.Text != ll.ElementAt(i).password)
                    {
                        MessageBox.Show("Password is incorrect", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox2.Text = "";
                        textBox2.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please enter the empty fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PasswordRecovery pr = new PasswordRecovery();
            pr.Visible = true;
           
        }
    }
}
