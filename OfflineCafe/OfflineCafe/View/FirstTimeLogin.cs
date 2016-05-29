using OfflineCafe.DataAccess;
using OfflineCafe.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OfflineCafe.View
{
    public partial class FirstTimeLogin : Form
    {
        public static List<Food> staticeList;

        public FirstTimeLogin()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = -1;
        }

        private void btnInsertMenu_Click(object sender, EventArgs e)
        {
            try
            {
                Entity.Login l = new Entity.Login();
                LoginDA lda = new LoginDA();

                for (int i = 0; i < staticeList.Count; i++)
                {

                    if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || comboBox1.SelectedIndex == -1)
                    {
                        MessageBox.Show("Please enter the empty field ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox1.Focus();
                    }
                    else if (textBox2.Text != textBox3.Text)
                    {
                        MessageBox.Show("New Password and Confirm Password does not match ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox3.Text = "";
                        textBox3.Focus();
                    }
                    else if (textBox2.Text.Length <= 6 && textBox3.Text.Length < 6)
                    {
                        MessageBox.Show("Length of New Password and Confirm Password must more than 6", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox3.Text = "";
                        textBox2.Focus();
                        textBox2.Text = "";
                    }
                    else if (textBox2.Text == staticeList.ElementAt(i).foodName || (textBox3.Text == staticeList.ElementAt(i).foodName))
                    {
                        MessageBox.Show("New Password and Confirm Password can not enter your IC number", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBox3.Text = "";
                        textBox2.Focus();
                        textBox2.Text = "";
                    }
                    else if((!textBox2.Text.Any(Char.IsLetter) || !textBox2.Text.Any(Char.IsDigit)) &&(!textBox3.Text.Any(Char.IsLetter) || !textBox3.Text.Any(Char.IsDigit))) {
                        MessageBox.Show("New Password and Confirm Password must contains numbers and letters", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBox3.Text = "";
                        textBox2.Focus();
                        textBox2.Text = "";
                    }
                    else
                    {
                        l.loginid = staticeList.ElementAt(i).foodID;
                        l.password = Convert.ToString(textBox3.Text);
                        l.question = comboBox1.SelectedItem.ToString();
                        l.answer = textBox1.Text;
                        lda.updateSecurity(l);
                        MessageBox.Show("Your security question and new password successfully created.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Close();
                        OfflineCafe.View.Login login = new OfflineCafe.View.Login();
                        login.Visible = true;
                        this.Visible = false;
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
