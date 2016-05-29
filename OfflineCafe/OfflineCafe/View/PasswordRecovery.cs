using OfflineCafe.DataAccess;
using OfflineCafe.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OfflineCafe.View
{
    public partial class PasswordRecovery : Form
    {
        
        public PasswordRecovery()
        {
            InitializeComponent();
            panel1.Visible = false;
        }

        private void btnInsertMenu_Click(object sender, EventArgs e)
        {
            LoginDA lda = new LoginDA();
            List<Entity.Login> ll = lda.searchid2();
            for (int i = 0; i < ll.Count; i++)
            {           
                if (textBox2.Text != ll.ElementAt(i).loginid)
                {
                    MessageBox.Show("Staff ID does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }              
                else
                {
                    panel1.Visible = true;
                    panel2.Visible = false;
                    panel3.Visible = false;
                    textBox3.Text = ll.ElementAt(i).question;
                    if (radioButton1.Checked==true)
                    {
                        panel3.Visible = false;
                        panel2.Visible = true;
                    }else if(radioButton2.Checked==true)
                    {
                        panel3.Visible = true;
                        panel2.Visible = false;
                    }
                    textBox2.Enabled = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try {
                if (radioButton1.Checked == false && radioButton2.Checked == false)
                {
                    MessageBox.Show("Please select a Password Recovery type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                LoginDA lda = new LoginDA();
                List<Entity.Login> ll = lda.searchid2();
                List<Food> el= lda.searchid(textBox2.Text);
                
                if (radioButton1.Checked == true)
                {
                    for (int i = 0; i < ll.Count; i++)
                    {
                        if (textBox2.Text != ll.ElementAt(i).loginid || textBox1.Text != ll.ElementAt(i).answer)
                        {
                            MessageBox.Show("The Security Answer is not correct, failed to retrieve old password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("Successful Retrieved\nYour Staff ID = " + ll.ElementAt(i).loginid + "\nYour Old Password = " + ll.ElementAt(i).password, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            panel1.Visible = false;
                            textBox2.Enabled = true;
                        }
                    }
                }
                else if (radioButton2.Checked == true)
                {
                   
                    for (int i = 0; i < ll.Count; i++)
                    {
                        if (textBox4.Text != el.ElementAt(i).foodStatus)
                        {
                            MessageBox.Show("The Staff ID was not match the email address you had entered", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            String email = el.ElementAt(i).foodStatus;
                            MailMessage message = new MailMessage();
                            SmtpClient smtp;
                            message.To.Add(email);
                            message.From = new MailAddress("terrysee12345@gmail.com");
                            message.Subject = "The Coffee Bean Cafe Retrieve Your Old Password";
                            message.Body = "Hi " + el.ElementAt(i).foodName + ",\n\nYour Staff ID = " + ll.ElementAt(i).loginid+"\nYour Old Password = " +ll.ElementAt(i).password;
                            smtp = new SmtpClient("smtp.gmail.com");
                            smtp.Port = 25;
                            smtp.EnableSsl = true;
                            smtp.Credentials = new NetworkCredential("terrysee12345@gmail.com", "Ucancme770");
                            smtp.SendAsync(message, message.Subject);
                            MessageBox.Show("The old password is successful sent to your email", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            panel1.Visible = false;
                            textBox2.Enabled = true;
                        }
                    }               
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel3.Visible = false;
            textBox1.Focus();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel3.Visible = true;
            textBox4.Focus();
        }
    }
}
