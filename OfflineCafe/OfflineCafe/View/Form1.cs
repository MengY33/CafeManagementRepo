﻿using OfflineCafe.DataAccess;
using OfflineCafe.Entity;
using OfflineCafe.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace OfflineCafe
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection();
        List<Food> fList = new List<Food>();
        List<Food> fl = new List<Food>();
        public static List<Food> staticfList;
        public Form1()
        {
            InitializeComponent();
            //form date
            label7.Text = "Today is " + DateTime.Now.ToShortDateString();
            //pnl close
            pnlMenu.Visible = false;
            pnlEmp.Visible = false;
            pnlMain.Visible = true;

            EmployeeDataFill();
            EmpDtGrdVw.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        //Form minimize
        private void minimizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        //Form exit close
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Are you confirm Exit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes) { this.Close(); }
            else { }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'cafeManagementDataSet2.Menu' table. You can move, or remove it, as needed.
            //this.menuTableAdapter.Fill(this.cafeManagementDataSet2.Menu);
            // TODO: This line of code loads data into the 'cafeManagementDataSet.Food' table. You can move, or remove it, as needed.
            //   this.foodTableAdapter1.Fill(this.cafeManagementDataSet.Food);
            //display data grid view
            //this.foodTableAdapter.Fill(this.cafeManagementDataSet1.Food);

            EmpDtGrdVw.AutoGenerateColumns = false;
        }
        //insert menu data
        private void btnInsertMenu_Click(object sender, EventArgs e)
        {
            try {
                if (Convert.ToDouble(txtPrice.Text) < 5)
                {
                    MessageBox.Show("Invalid Food Price(RM)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //insert menu data
                    Food f = new Food();
                    FoodDA fDA = new FoodDA();
                    f.foodID = txtID.Text;
                    f.foodName = txtName.Text;
                    f.foodDesc = txtDesc.Text;
                    f.foodType = cboType.SelectedItem.ToString();
                    f.foodStatus = comboBox2.SelectedItem.ToString();
                    f.foodPrice = Convert.ToDouble(txtPrice.Text);

                    fDA.InsertFunc(f);
                    this.foodTableAdapter.Fill(this.cafeManagementDataSet1.Food);
                    this.menuTableAdapter.Fill(this.cafeManagementDataSet2.Menu);
                    retrievefoodid();

                    newFoodID();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Duplicate Food ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please enter empty fields for Food details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //Menu GridView data
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Food f = new Food();
            //FoodDA fDA = new FoodDA();
            //string index = dataGridView1.CurrentRow.Index.ToString();
            //string id1 = dataGridView1.Rows[Convert.ToInt32(index)].Cells[0].Value.ToString();
            //f.foodID = id1;
            //DialogResult r = MessageBox.Show("Are you confirm Delete? \nFood ID = "+f.foodID+ "", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (r == DialogResult.Yes) {
            //display menu data into gridview
            string index = dataGridView1.CurrentRow.Index.ToString();
            string id1 = dataGridView1.Rows[Convert.ToInt32(index)].Cells[0].Value.ToString();
            Food f = new Food();
            FoodDA fDA = new FoodDA();
            f = fDA.SearchUpdate(id1);
            txtID.Text = f.foodID;
            txtName.Text = f.foodName;
            txtDesc.Text = f.foodDesc;
            comboBox2.SelectedItem = f.foodStatus;
            cboType.SelectedItem = f.foodType;
            txtPrice.Text = String.Format("{0:0.00}", f.foodPrice.ToString("0.00"));
            btnUpdateMenu.Enabled = true;
        }
        //Menu search from food Name using keystroke
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Food f = new Food();
            FoodDA fDA = new FoodDA();
            f.foodName = textBox1.Text;
            var adapter = fDA.SearchFunc(f);

            var ds = this.cafeManagementDataSet1.Food;
            ds.Clear();
            adapter.Fill(ds);
        }
        //Menu search from food type using combobox
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                this.foodTableAdapter.Fill(this.cafeManagementDataSet1.Food);
            }
            else
            {
                Food f = new Food();
                FoodDA fDA = new FoodDA();
                f.foodType = comboBox1.SelectedItem.ToString();
                var adapter = fDA.SearchTypeFunc(f);

                var ds = this.cafeManagementDataSet1.Food;
                ds.Clear();
                adapter.Fill(ds);
            }
        }
        //update food data
        private void btnUpdateMenu_Click(object sender, EventArgs e)
        {
            try {
                if (Convert.ToDouble(txtPrice.Text) < 5)
                {
                    MessageBox.Show("Invalid Food Price(RM)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Food f = new Food();
                    FoodDA fDA = new FoodDA();
                    f.foodID = txtID.Text;
                    f.foodName = txtName.Text;
                    f.foodDesc = txtDesc.Text;
                    f.foodType = cboType.SelectedItem.ToString();
                    f.foodStatus = comboBox2.SelectedItem.ToString();
                    f.foodPrice = Convert.ToDouble(txtPrice.Text);

                    fDA.UpdateFunc(f);
                    this.foodTableAdapter.Fill(this.cafeManagementDataSet1.Food);
                    List<MenuFood> mlf = new List<MenuFood>();
                    MenuDA mda = new MenuDA();
                    mlf = mda.SearchMenuFoodID(f);
                    for (int i = 0; i < mlf.Count; i++)
                    {
                        if (f.foodStatus == "Non-Available")
                        {

                            Entity.Menu m = new Entity.Menu();
                            m.MenuID = mlf.ElementAt(i).menuID;
                            m.MenuStatus = "Non-Available";
                            mda.updateStatus(m);
                        }
                    }
                    this.menuTableAdapter.Fill(this.cafeManagementDataSet2.Menu);
                    newFoodID();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please enter empty field for Food Details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //reset menu details
        public void newMenuID()
        {
            timer1.Stop();
            txtMenuName.Text = "";
            txtMenuDesc.Text = "";
            cboMenu.SelectedIndex = -1;
            cboMenuStatus.SelectedIndex = 0;
            txtMenuPrice.Text = "";
            txtMenuDiscP.Text = "";
            radioButton1.Checked = true;
            comboBox3.SelectedIndex = -1;
            txtMenuF.Text = "";
            btnUpdMenuFood.Enabled = false;
            retrievemenuid();

        }
        //reset food details
        public void newFoodID()
        {
            this.foodTableAdapter.Fill(this.cafeManagementDataSet1.Food);
            txtName.Text = "";
            txtDesc.Text = "";
            cboType.SelectedIndex = -1;
            comboBox2.SelectedIndex = 0;
            txtPrice.Text = "";
            btnUpdateMenu.Enabled = false;
            retrievefoodid();
        }
        
        //display menu food selection
        private void button12_Click(object sender, EventArgs e)
        {
            timer1.Start();
            MenuFoodSelection.staticfList = fl;
            MenuFoodSelection mfs = new MenuFoodSelection();
            mfs.Visible = true;
            textBox1.Text = "";
        }
   
        //insert menu food data
        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMenuF.Text == "")
                {
                    MessageBox.Show("Please enter the menu selection field", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //insert menu food data
                    Entity.Menu m = new Entity.Menu();
                    MenuDA mda = new MenuDA();
                    m.MenuID = txtMenuID.Text;
                    m.MenuName = txtMenuName.Text;
                    m.MenuDesc = txtMenuDesc.Text;
                    m.MenuCategory = cboMenu.SelectedItem.ToString();
                    m.MenuStatus = cboMenuStatus.SelectedItem.ToString();
                    m.MenuPrice = Convert.ToDouble(txtMenuPrice.Text);
                    m.DiscPrice = Convert.ToDouble(txtMenuDiscP.Text);
                    mda.InsertFunc(m);
                    this.menuTableAdapter.Fill(this.cafeManagementDataSet2.Menu);
                    //insert ala carte menu into sets
                    List<MenuFood> mfl = new List<MenuFood>();

                    for (int i = 0; i < staticfList.Count; i++)
                    {
                        MenuFood mf = new MenuFood();

                        mf.menuID = txtMenuID.Text;
                        mf.foodID = staticfList.ElementAt(i).foodID;
                        mf.quantity = staticfList.ElementAt(i).qty;
                        mfl.Add(mf);
                    }
                    mda.InsertMenuItem(mfl);
                    retrievemenuid();
                    newMenuID();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Duplicate Menu ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please enter empty fields for Menu Sets details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       //yes for menu set disc
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox3.Enabled = true;
        }
        //no for menu set disc
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            comboBox3.Enabled = false;
            comboBox3.SelectedIndex =-1;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //refresh the menu price and discount price   
            double totalPrice = 0.00;
            double grandTotal = 0.00;
            double discTotal = 0.00;
            txtMenuF.Text = "";
            if (staticfList != null)
            {              
                //DialogResult r = MessageBox.Show("Please Click the 'Select' button to select Ala Carte into Menu sets.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                for (int i = 0; i < staticfList.Count; i++)
                {
                    txtMenuF.Text += staticfList.ElementAt(i).foodName + ", ";
                    double foodPrice = staticfList.ElementAt(i).foodPrice;
                    int quantity = staticfList.ElementAt(i).qty;
                    totalPrice += (foodPrice * quantity);
                    grandTotal = totalPrice - 2.00;
                }
                txtMenuPrice.Text = String.Format("{0:0.00}", grandTotal.ToString("0.00"));
                if (comboBox3.SelectedIndex == -1 || radioButton2.Checked == true)
                {

                    txtMenuDiscP.Text = String.Format("{0:0.00}", grandTotal.ToString("0.00"));
                }
                else if (comboBox3.SelectedIndex == 0)
                {
                    grandTotal = (totalPrice - 2.00) * 0.95;
                    txtMenuDiscP.Text = String.Format("{0:0.00}", grandTotal.ToString("0.00"));
                }
                else if (comboBox3.SelectedIndex == 1)
                {
                    grandTotal = (totalPrice - 2.00) * 0.90;
                    txtMenuDiscP.Text = String.Format("{0:0.00}", grandTotal.ToString("0.00"));
                }

            }

        }
        //SEARCH menu details by name
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Entity.Menu m = new Entity.Menu();
            MenuDA mda = new MenuDA();
            m.MenuName = textBox2.Text;
            var adapter = mda.SearchbyName(m.MenuName);
            var ds = this.cafeManagementDataSet2.Menu;
            ds.Clear();
            adapter.Fill(ds);
        }
        //Search menu details by category
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.SelectedIndex == 0)
            {
                this.menuTableAdapter.Fill(this.cafeManagementDataSet2.Menu);
            }
            else
            {
                Entity.Menu f = new Entity.Menu();
                MenuDA mda = new MenuDA();

                var adapter = mda.SearchbyCategory(comboBox4.SelectedItem.ToString());

                var ds = this.cafeManagementDataSet2.Menu;
                ds.Clear();
                adapter.Fill(ds);
            }
        }
        //search retrieve menu food set by menu id
        private void dataGridView4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            timer1.Stop();
            
            String index = dataGridView4.CurrentRow.Index.ToString();
            String id = dataGridView4.Rows[Convert.ToInt32(index)].Cells[0].Value.ToString();
            Entity.Menu m = new Entity.Menu();
            MenuDA mda = new MenuDA();
            m = mda.SearchUpdate(id);
            txtMenuID.Text = m.MenuID;
            txtMenuName.Text = m.MenuName;
            txtMenuDesc.Text = m.MenuDesc;
            cboMenu.SelectedItem = m.MenuCategory;
            cboMenuStatus.SelectedItem = m.MenuStatus;
            txtMenuPrice.Text = String.Format("{0:0.00}", m.MenuPrice.ToString("0.00"));
            txtMenuDiscP.Text = String.Format("{0:0.00}", m.DiscPrice.ToString("0.00"));
            double value = (m.DiscPrice / m.MenuPrice) * 100;
            if (m.MenuPrice == m.DiscPrice)
            {
                radioButton2.Checked = true;
                comboBox3.SelectedIndex = -1;
            }
            else if((Double)Math.Round(value) == 94 || (Double)Math.Round(value) == 95 || (Double)Math.Round(value) == 96)
            {
                radioButton1.Checked = true;
                comboBox3.SelectedIndex = 0;
            }
            else if ((Double)Math.Round(value) == 89 || (Double)Math.Round(value) == 90 || (Double)Math.Round(value) == 91)
            {
                radioButton1.Checked = true;
                comboBox3.SelectedIndex = 1;
            }
            //retrieve multiple menu food items by menu id
          
            FoodDA fda = new FoodDA();
            List<MenuFood> mfl = new List<MenuFood>();
            mfl = mda.SearchMultipleRecord(id);
            String menufood="";
            fl = new List<Food>();
            for(int i=0; i<mfl.Count; i++)
            {
                fl.Add(fda.SearchUpdate(mfl.ElementAt(i).foodID));
                fl.ElementAt(i).qty = mfl.ElementAt(i).quantity;
                menufood += fl.ElementAt(i).foodName + ", ";
            }
            txtMenuF.Text = menufood;
            btnUpdMenuFood.Enabled = true;
            
        }
        //update menu sets and ala carte into sets
        private void btnUpdMenuFood_Click(object sender, EventArgs e)
        {
            try {
                //Update Menu Set details         
                Entity.Menu m = new Entity.Menu();
                MenuDA mda = new MenuDA();
                m.MenuID = txtMenuID.Text;
                m.MenuName = txtMenuName.Text;
                m.MenuDesc = txtMenuDesc.Text;
                m.MenuCategory = cboMenu.SelectedItem.ToString();
                m.MenuStatus = cboMenuStatus.SelectedItem.ToString();
                m.MenuPrice = Convert.ToDouble(txtMenuPrice.Text);
                m.DiscPrice = Convert.ToDouble(txtMenuDiscP.Text);

                mda.updateMenu(m);
                this.menuTableAdapter.Fill(this.cafeManagementDataSet2.Menu);
                //update multiple menu food into sets
                if (staticfList != null)
                {
                    timer1.Start();
                    List<MenuFood> mfl = new List<MenuFood>();
                    MenuFood mf1 = new MenuFood();
                    mf1.menuID = txtMenuID.Text;
                    mda.DeleteFunc(mf1);

                    for (int i = 0; i < staticfList.Count; i++)
                    {
                        MenuFood mf = new MenuFood();

                        mf.menuID = txtMenuID.Text;
                        mf.foodID = staticfList.ElementAt(i).foodID;
                        mf.quantity = staticfList.ElementAt(i).qty;
                        mfl.Add(mf);
                    }
                    mda.InsertMenuItem(mfl);
                }
                retrievemenuid();
                newMenuID();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please enter empty field for Menu Sets Details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        //discount rate text select change
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //display discount price
            if (txtMenuPrice.Text != "")
            {
                double grandTotal = 0.00;
                double total = Convert.ToDouble(txtMenuPrice.Text);
                if (comboBox3.SelectedIndex == -1 || radioButton2.Checked == true)
                {

                    txtMenuDiscP.Text = String.Format("{0:0.00}", total.ToString("0.00"));
                }
                else if (comboBox3.SelectedIndex == 0)
                {
                    grandTotal = total * 0.95;
                    txtMenuDiscP.Text = String.Format("{0:0.00}", grandTotal.ToString("0.00"));
                }
                else if (comboBox3.SelectedIndex == 1)
                {
                    grandTotal = total * 0.90;
                    txtMenuDiscP.Text = String.Format("{0:0.00}", grandTotal.ToString("0.00"));
                }
            }
        }       
        //retrieve menuid
        public void retrievemenuid()
        {
            Entity.Menu m= new Entity.Menu();
            MenuDA mda = new MenuDA();
            m = mda.SearchMenuID();
            int oldid = Convert.ToInt32(m.MenuID.Substring(1, 6));
            int newid = oldid + 1;
            String nid = "M" + newid;
            txtMenuID.Text = nid;
        }
        //retrieve foodid
        public void retrievefoodid()
        {
            List<Food> fl = new List<Food>();
            Food f = new Food();
            FoodDA fda = new FoodDA();
            f = fda.SearchFoodID();
            int oldid = Convert.ToInt32(f.foodID.Substring(1, 6));
            int newid = oldid + 1;
            String nid = "F" + newid;
            txtID.Text = nid;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            pnlMenu.Visible = true;
            //Menu button Update
            btnUpdateMenu.Enabled = false;
            btnUpdMenuFood.Enabled = false;
            comboBox2.SelectedIndex = 0;
            cboMenuStatus.SelectedIndex = 0;
            retrievefoodid();
            retrievemenuid();
            pnlEmp.Visible = false;
            pnlMain.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pnlMenu.Visible = false;
            pnlEmp.Visible = true;
            EmpPositionCbBx.SelectedIndex = 0;
            EmpStatusCbBx.SelectedIndex = 0;
            pnlMain.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pnlMenu.Visible = false;
            pnlEmp.Visible = false;
            pnlMain.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pnlMenu.Visible = false;
            pnlEmp.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pnlMenu.Visible = false;
            pnlEmp.Visible = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            pnlMenu.Visible = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pnlMenu.Visible = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            pnlMenu.Visible = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            pnlMenu.Visible = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            pnlMenu.Visible = false;
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            for(int i=0;i<txtPrice.Text.Length;i++)
            {
                char a = txtPrice.Text.ElementAt(i);
                if (Char.IsNumber(a) == false && a != '.')
                {
                    MessageBox.Show("Please enter correct currency format", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrice.Clear();
                    txtPrice.Focus();
                    break;
                }              
            }          
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Are you confirm Exit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes) { this.Close(); }
            else { }
        }

        private void EmpInsertBtn_Click(object sender, EventArgs e)
        {
            String p1 = @"^[a-z\s/]*$";
            String p2 = @"^\d*$";
            String p3 = @"[0-9]{2}[23456789]{1}\d{3}-\d{2}-\d{4}";      //check the month from the IC Number, make sure the first digit of the month is not starts from 2,3,4,5,6,7,8,9
            String p4 = @"[0-9]{2}[1]{1}[3456789]{1}\d{2}-\d{2}-\d{4}"; //check the month from the IC Number, make sure the second digit of the month is not starts from 3,4,5,6,7,8,9
            String p5 = @"[0-9]{4}[456789]{1}[0-9]{1}-\d{2}-\d{4}";     //check the day from the IC Number, make sure the first digit of the day is not starts from 4,5,6,7,8,9
            String p6 = @"[0-9]{4}[3]{1}[23456789]{1}-\d{2}-\d{4}";     //check the day from the IC Number, make sure the second digit of the day is not starts from 2,3,4,5,6,7,8,9
            String p7 = @"\d{6}-[0]{1}[0]{1}-\d{4}";                    //check the birthplace code from the IC Number, it does not contains 00                  
            String p8 = @"\d{6}-[1]{1}[789]{1}-\d{4}";                  //check the birthplace code from the IC Number, it does not contains 17,18,19
            String p9 = @"\d{6}-[2]{1}[0]{1}-\d{4}";                    //check the birthplace code from the IC Number, it does not contains 20
            String p10 = @"\d{6}-[6789]{1}[0123456789]{1}-\d{4}";       //check the birthplace code from the IC Number, it does not contains 60-69, 70-79, 80-89, 90-99
            //String p11 = @"[0]{1}[0-9]{5}-[0-9]{2}-[0-9]{4}";           //check the year from the IC Number, make sure the first digit is not starts from 0
            String p12 = @"[0-9]{2}[0]{4}-[0-9]{2}-[0-9]{4}";           //check the IC Number, it can't be like 920000-12-1234
            String p13 = @"[0-9]{3}[0]{3}-[0-9]{2}-[0-9]{4}";           //check the IC Number, it can't be like 921000-12-1234
            String p14 = @"[0-9]{4}[0]{2}-[0-9]{2}-[0-9]{4}";           //check the IC Number, it can't be like 920400-12-1234
            String p15 = @"\d{6}-\d{2}-[0]{4}";                         //check the IC Number, it can't be like 921221-12-0000
            String p16 = @"^\d{6}-\d{2}-\d{4}$";                        //check the IC Number, it should not contains any alphabetics and special symbols
            String p17 = @"\d{6}-\d{2}-\d{3}[13579]{1}";                //Gender Validation for Male
            String p18 = @"\d{6}-\d{2}-\d{3}[02468]{1}";                //Gender Validation for Female
            String p19 = @"^[a-z0-9\s.,/-]*$";                          //Address Validation, if the address not matched with this format, it may causes error
            String p20 = @"^[0-9\s.,/-]*$";                             //Address Validation, the address can't be like 85784,577873.
            String p21 = @"[1-9]{1}[0-9]{1}-[0-9]{8}";                  //Home No. Validation, the first digit must be zero
            String p22 = @"[0]{1}[0]{1}-[0-9]{8}";                      //Home No. Validation, the second digit cannot be zero
            String p23 = @"[0]{1}[125]{1}";                             //Home No. Validation, make sure the city code does not contains 01,02,05
            String p24 = @"[0-9]{2}-[0]{8}";                            //Home No. Validation, it can't be like 03-00000000
            String p25 = @"^\d{2}-\d{8}$";                              //Home No. Validation, it can't contains any alphabetics and any special symbols and also has exactly 11 charatcers
            String p26 = @"[1-9]{1}[0-9]{2}-[0-9]{7}";                  //Handphone No. Validation, the first digit must be zero
            String p27 = @"[0]{3}-[0-9]{7}";                            //Handphone No. Validation, it can't be like 000-1234567
            String p28 = @"[0-9]{3}-[0]{7}";                            //Handphone No. Validation, it can't be like 012-0000000
            String p29 = @"^\d{3}-\d{7}$";                                //Handphone No. Validation, it can't contains any alphabetics and any special symbols and also has exactly 11 characters
            String p30 = @"^[a-z0-9_.\-]+\@[a-z]+\.(?:[a-z]{3}|com|org|net|edu|gov)|\.(?:[a-z]{2}|my)$";    //Email Validation

            Regex rgx1 = new Regex(p1, RegexOptions.IgnoreCase);
            Regex rgx2 = new Regex(p2);
            Regex rgx3 = new Regex(p3);
            Regex rgx4 = new Regex(p4);
            Regex rgx5 = new Regex(p5);
            Regex rgx6 = new Regex(p6);
            Regex rgx7 = new Regex(p7);
            Regex rgx8 = new Regex(p8);
            Regex rgx9 = new Regex(p9);
            Regex rgx10 = new Regex(p10);
            //Regex rgx11 = new Regex(p11);
            Regex rgx12 = new Regex(p12);
            Regex rgx13 = new Regex(p13);
            Regex rgx14 = new Regex(p14);
            Regex rgx15 = new Regex(p15);
            Regex rgx16 = new Regex(p16);
            Regex rgx17 = new Regex(p17);
            Regex rgx18 = new Regex(p18);
            Regex rgx19 = new Regex(p19);
            Regex rgx20 = new Regex(p20);
            Regex rgx21 = new Regex(p21);
            Regex rgx22 = new Regex(p22);
            Regex rgx23 = new Regex(p23);
            Regex rgx24 = new Regex(p24);
            Regex rgx25 = new Regex(p25);
            Regex rgx26 = new Regex(p26);
            Regex rgx27 = new Regex(p27);
            Regex rgx28 = new Regex(p28);
            Regex rgx29 = new Regex(p29);
            Regex rgx30 = new Regex(p30);


            if (EmpNameTxtBx.Text == String.Empty || EmpICTxtBx.Text == String.Empty || EmpMaleRdBtn.Checked == false && EmpFemaleRdBtn.Checked == false || EmpAddress1TxtBx.Text == String.Empty && EmpAddress2TxtBx.Text == String.Empty || EmpHomeNoTxtBx.Text == String.Empty || EmpHandphoneNoTxtBx.Text == String.Empty || EmpEmailTxtBx.Text == String.Empty || EmpPositionCbBx.SelectedIndex == 0 || EmpStatusCbBx.SelectedIndex == 0)
            {
                ErrLbl.Visible = true;
                ErrLbl.Text = "*Please make sure you are completed all the fields.";
            }
            else
            {
                if (!rgx1.Match(EmpNameTxtBx.Text).Success)
                {
                    ErrLbl.Visible = true;
                    ErrLbl.Text = "*Please make sure Employee Name does not contains any numeric data. \r\n\r\n*Please make sure Employee Name does not contains any special symbol, except '/'.";
                    EmpNameTxtBx.Focus();
                }
                else if (EmpNameTxtBx.TextLength > 200 || EmpNameTxtBx.TextLength < 10)
                {
                    ErrLbl.Visible = true;
                    ErrLbl.Text = "*Please make sure Employee Name does not exceeds more than 200 characters. \r\n\r\n*Please make sure Employee Name has minimum 10 characters.";
                    EmpNameTxtBx.Focus();
                }
                else
                {
                    if(rgx2.Match(EmpICTxtBx.Text).Success)
                    {
                        ErrLbl.Visible = true;
                        ErrLbl.Text = "*IC Number is missing dashes '-'. \r\n\r\nPlease make sure IC Number is in such format 'xxxxxx-xx-xxxx'.";
                        EmpICTxtBx.Focus();
                    }
                    else if(EmpICTxtBx.TextLength > 14 || EmpICTxtBx.TextLength < 14)
                    {
                        ErrLbl.Visible = true;
                        ErrLbl.Text = "*Please make sure IC Number has exactly 14 characters.";
                        EmpICTxtBx.Focus();
                    }
                    else if(EmpICTxtBx.Text.Equals("000000-00-0000") || rgx12.Match(EmpICTxtBx.Text).Success || rgx13.Match(EmpICTxtBx.Text).Success || rgx14.Match(EmpICTxtBx.Text).Success || rgx15.Match(EmpICTxtBx.Text).Success)
                    {
                        ErrLbl.Visible = true;
                        ErrLbl.Text = "*Invalid IC Number!";
                        EmpICTxtBx.Focus();
                    }
                    else if(rgx3.Match(EmpICTxtBx.Text).Success || rgx4.Match(EmpICTxtBx.Text).Success)
                    {
                        ErrLbl.Visible = true;
                        ErrLbl.Text = "*Invalid IC Number!  Please make sure the month number is entered correctly.";
                        EmpICTxtBx.Focus();
                    }
                    else if(rgx5.Match(EmpICTxtBx.Text).Success || rgx6.Match(EmpICTxtBx.Text).Success)
                    {
                        ErrLbl.Visible = true;
                        ErrLbl.Text = "*Invalid IC Number!  Please make sure the day number is entered correctly.";
                        EmpICTxtBx.Focus();
                    }
                    else if(rgx7.Match(EmpICTxtBx.Text).Success || rgx8.Match(EmpICTxtBx.Text).Success || rgx9.Match(EmpICTxtBx.Text).Success || rgx10.Match(EmpICTxtBx.Text).Success)
                    {
                        ErrLbl.Visible = true;
                        ErrLbl.Text = "*Invalid IC Number!  Please make sure the birthplace code is entered correctly.";
                        EmpICTxtBx.Focus();
                    }
                    else if(!rgx16.Match(EmpICTxtBx.Text).Success)
                    {
                        ErrLbl.Visible = true;
                        ErrLbl.Text = "*Please make sure the IC Number does not contains any alphabetics.\r\n\r\n*Please make sure the IC Number does not contains any special symbols, except '-'.";
                        EmpICTxtBx.Focus();
                    }
                    else
                    {
                        if(rgx17.Match(EmpICTxtBx.Text).Success && EmpFemaleRdBtn.Checked == true || rgx18.Match(EmpICTxtBx.Text).Success && EmpMaleRdBtn.Checked == true)
                        {
                            ErrLbl.Visible = true;
                            ErrLbl.Text = "*Invalid Gender selected or IC Number may typed wrongly!\r\n\r\n*Please check your Gender and IC Number again.";
                        }
                        else
                        {
                            if(!rgx19.Match(EmpAddress1TxtBx.Text).Success)
                            {
                                ErrLbl.Visible = true;
                                ErrLbl.Text = "*Please make sure Address does not contains any special symbols, except ','  '.'  '/'  '-'.";
                                EmpAddress1TxtBx.Focus();
                            }
                            else if(rgx20.Match(EmpAddress1TxtBx.Text).Success)
                            {
                                ErrLbl.Visible = true;
                                ErrLbl.Text = "*Invalid Address!";
                                EmpAddress1TxtBx.Focus();
                            }
                            else if(EmpAddress1TxtBx.TextLength > 150 || EmpAddress1TxtBx.TextLength < 20)
                            {
                                ErrLbl.Visible = true;
                                ErrLbl.Text = "*Please make sure Address does not exceeds more than 150 characters. \r\n\r\n*Please make sure Address has minimum 20 characters.";
                                EmpAddress1TxtBx.Focus();
                            }
                            else
                            {
                                if(!(EmpAddress2TxtBx.Text == String.Empty) && !rgx19.Match(EmpAddress2TxtBx.Text).Success)
                                {
                                    ErrLbl.Visible = true;
                                    ErrLbl.Text = "*Please make sure Address does not contains any special symbols, except ','  '.'  '/'  '-'.";
                                    EmpAddress2TxtBx.Focus();
                                }
                                else if(!(EmpAddress2TxtBx.Text == String.Empty) && rgx20.Match(EmpAddress2TxtBx.Text).Success)
                                {
                                    ErrLbl.Visible = true;
                                    ErrLbl.Text = "*Invalid Address!";
                                    EmpAddress2TxtBx.Focus();
                                }
                                else if(!(EmpAddress2TxtBx.Text == String.Empty) && EmpAddress2TxtBx.TextLength > 50)
                                {
                                    ErrLbl.Visible = true;
                                    ErrLbl.Text = "*Please make sure Address does not exceeds more than 50 characters.";
                                    EmpAddress2TxtBx.Focus();
                                }
                                else
                                {
                                    if(rgx2.Match(EmpHomeNoTxtBx.Text).Success)
                                    {
                                        ErrLbl.Visible = true;
                                        ErrLbl.Text = "*Home Number is missing dashes '-'. \r\n\r\n*Please make sure Home Number is in such format 'xx-xxxxxxxx'.";
                                        EmpHomeNoTxtBx.Focus();
                                    }
                                    else if(rgx21.Match(EmpHomeNoTxtBx.Text).Success)
                                    {
                                        ErrLbl.Visible = true;
                                        ErrLbl.Text = "*Invalid Home Number!  Please make sure the first digit is zero.";
                                        EmpHomeNoTxtBx.Focus();
                                    }
                                    else if(rgx22.Match(EmpHomeNoTxtBx.Text).Success)
                                    {
                                        ErrLbl.Visible = true;
                                        ErrLbl.Text = "*Invalid Home Number!  Please make sure the second digit is not zero.";
                                        EmpHomeNoTxtBx.Focus();
                                    }
                                    else if(rgx23.Match(EmpHomeNoTxtBx.Text).Success)
                                    {
                                        ErrLbl.Visible = true;
                                        ErrLbl.Text = "*Invalid Home Number!  Malaysia city codes only contain 03, 04, 06, 07, 08, 09.";
                                        EmpHomeNoTxtBx.Focus();
                                    }
                                    else if(rgx24.Match(EmpHomeNoTxtBx.Text).Success)
                                    {
                                        ErrLbl.Visible = true;
                                        ErrLbl.Text = "*Invalid Home Number!";
                                        EmpHomeNoTxtBx.Focus();
                                    }
                                    else if(!rgx25.Match(EmpHomeNoTxtBx.Text).Success)
                                    {
                                        ErrLbl.Visible = true;
                                        ErrLbl.Text = "*Please make sure Home Number does not contains any alphabetics. \r\n\r\n*Please make sure Home Number does not contains any special symbols, except '-'. \r\n\r\n*Please make sure Home Number has exactly 11 characters.";
                                        EmpHomeNoTxtBx.Focus();
                                    }
                                    else
                                    {
                                        if(rgx2.Match(EmpHandphoneNoTxtBx.Text).Success)
                                        {
                                            ErrLbl.Visible = true;
                                            ErrLbl.Text = "*Handphone Number is missing dashes '-'. \r\n\r\n*Please make sure Handphone Number is in such format 'xxx-xxxxxxx'.";
                                            EmpHandphoneNoTxtBx.Focus();
                                        }
                                        else if(rgx26.Match(EmpHandphoneNoTxtBx.Text).Success)
                                        {
                                            ErrLbl.Visible = true;
                                            ErrLbl.Text = "*Invalid Handphone Number!  Please make sure the first digit is zero.";
                                            EmpHandphoneNoTxtBx.Focus();
                                        }
                                        else if(rgx27.Match(EmpHandphoneNoTxtBx.Text).Success)
                                        {
                                            ErrLbl.Visible = true;
                                            ErrLbl.Text = "*Invalid Handphone Number!";
                                            EmpHandphoneNoTxtBx.Focus();
                                        }
                                        else if(rgx28.Match(EmpHandphoneNoTxtBx.Text).Success)
                                        {
                                            ErrLbl.Visible = true;
                                            ErrLbl.Text = "*Invalid Handphone Number!";
                                            EmpHandphoneNoTxtBx.Focus();
                                        }
                                        else if(!rgx29.Match(EmpHandphoneNoTxtBx.Text).Success)
                                        {
                                            ErrLbl.Visible = true;
                                            ErrLbl.Text = "*Please make sure Handphone Number does not contains any alphabetics. \r\n\r\n*Please make sure Handphone Number does not contains any special symbols, except '-'. \r\n\r\n*Please make sure Handphone Number has exactly 11 characters.";
                                            EmpHandphoneNoTxtBx.Focus();
                                        }
                                        else
                                        {
                                            if(!rgx30.Match(EmpEmailTxtBx.Text).Success)
                                            {
                                                ErrLbl.Visible = true;
                                                ErrLbl.Text = "*Invalid Email address!";
                                                EmpEmailTxtBx.Focus();
                                            }
                                            else if(EmpEmailTxtBx.TextLength > 200 || EmpEmailTxtBx.TextLength < 10)
                                            {
                                                ErrLbl.Visible = true;
                                                ErrLbl.Text = "*Please make sure Email address does not exceeds more than 200 characters. \r\n\r\n*Please make sure Email address has minimum 10 characters.";
                                                EmpEmailTxtBx.Focus();
                                            }
                                            else
                                            {
                                                Employee emp = new Employee();
                                                EmployeeDA empDA = new EmployeeDA();

                                                emp.EmployeeName = EmpNameTxtBx.Text;
                                                emp.ICNumber = EmpICTxtBx.Text;

                                                if(EmpMaleRdBtn.Checked == true)
                                                {
                                                    emp.Gender = EmpMaleRdBtn.Text;
                                                }
                                                else
                                                {
                                                    emp.Gender = EmpFemaleRdBtn.Text;
                                                }

                                                if(!(EmpAddress1TxtBx.Text == String.Empty) && !(EmpAddress2TxtBx.Text == String.Empty))
                                                {
                                                    emp.HomeAddress = EmpAddress1TxtBx.Text + ", " + EmpAddress2TxtBx.Text;
                                                }
                                                else
                                                {
                                                    emp.HomeAddress = EmpAddress1TxtBx.Text;
                                                }
                                                emp.HomeNumber = EmpHomeNoTxtBx.Text;
                                                emp.HandphoneNumber = EmpHandphoneNoTxtBx.Text;
                                                emp.Email = EmpEmailTxtBx.Text;
                                                emp.Position = EmpPositionCbBx.SelectedItem.ToString();
                                                emp.CurrentStatus = EmpStatusCbBx.SelectedItem.ToString();

                                                empDA.EmployeeInsertRecord(emp);

                                                if (emp.InsertStatus == "Success")
                                                {
                                                    MessageBox.Show("New employee record has inserted successfully!");

                                                    EmpNameTxtBx.Clear();
                                                    EmpICTxtBx.Clear();
                                                    EmpMaleRdBtn.Checked = false;
                                                    EmpFemaleRdBtn.Checked = false;
                                                    EmpAddress1TxtBx.Clear();
                                                    EmpAddress2TxtBx.Clear();
                                                    EmpHomeNoTxtBx.Clear();
                                                    EmpHandphoneNoTxtBx.Clear();
                                                    EmpEmailTxtBx.Clear();
                                                    EmpPositionCbBx.SelectedIndex = 0;
                                                    EmpStatusCbBx.SelectedIndex = 0;

                                                    ErrLbl.Visible = false;
                                                }
                                                else if(emp.InsertStatus == "Failed")
                                                {
                                                    MessageBox.Show("Failed to insert employee record!");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void EmployeeDataFill()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "SELECT EmployeeID, EmployeeName, ICNumber, Gender, HomeAddress, HomeNumber, HandphoneNumber, Email, Position, EmpStatus FROM Employee";

                SqlDataAdapter da = new SqlDataAdapter(sql, con);

                DataSet ds = new DataSet();

                con.Open();

                da.Fill(ds, "Employee");

                con.Close();

                EmpDtGrdVw.DataSource = ds;
                EmpDtGrdVw.DataMember = "Employee";
            }
            catch(Exception ex)
            {
                MessageBox.Show("Employee's data grid view cannot read the database!");
            }
        }

        private void EmpRefreshBtn_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "SELECT EmployeeID, EmployeeName, ICNumber, Gender, HomeAddress, HomeNumber, HandphoneNumber, Email, Position, EmpStatus FROM Employee";

                SqlDataAdapter da = new SqlDataAdapter(sql, con);

                DataSet ds = new DataSet();

                con.Open();

                da.Fill(ds, "Employee");

                con.Close();

                EmpDtGrdVw.DataSource = ds;
                EmpDtGrdVw.DataMember = "Employee";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Employee's data grid view cannot read the database!");
            }
        }

        private void EmpDtGrdVw_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            EmpInsertBtn.Enabled = false;

            int i;
            i = EmpDtGrdVw.SelectedCells[0].RowIndex;

            EmpIDDTxtBx.Text = EmpDtGrdVw.Rows[i].Cells[0].Value.ToString();
            EmpNameTxtBx.Text = EmpDtGrdVw.Rows[i].Cells[1].Value.ToString();
            EmpICTxtBx.Text = EmpDtGrdVw.Rows[i].Cells[2].Value.ToString();

            string g = EmpDtGrdVw.Rows[i].Cells[3].Value.ToString();

            if(g.Equals("Male"))
            {
                EmpMaleRdBtn.Checked = true;
            }
            else
            {
                EmpFemaleRdBtn.Checked = true;
            }
            EmpAddress1TxtBx.Text = EmpDtGrdVw.Rows[i].Cells[4].Value.ToString();
            EmpHomeNoTxtBx.Text = EmpDtGrdVw.Rows[i].Cells[5].Value.ToString();
            EmpHandphoneNoTxtBx.Text = EmpDtGrdVw.Rows[i].Cells[6].Value.ToString();
            EmpEmailTxtBx.Text = EmpDtGrdVw.Rows[i].Cells[7].Value.ToString();

            string p = EmpDtGrdVw.Rows[i].Cells[8].Value.ToString();

            if(p.Equals("Admin"))
            {
                EmpPositionCbBx.SelectedIndex = 1;
            }
            else if(p.Equals("Cashier"))
            {
                EmpPositionCbBx.SelectedIndex = 2;
            }
            else if(p.Equals("Chef"))
            {
                EmpPositionCbBx.SelectedIndex = 3;
            }
            else if(p.Equals("Manager"))
            {
                EmpPositionCbBx.SelectedIndex = 4;
            }

            string cs = EmpDtGrdVw.Rows[i].Cells[9].Value.ToString();

            if(cs.Equals("Hired"))
            {
                EmpStatusCbBx.SelectedIndex = 1;
            }
            else if(cs.Equals("Resigned"))
            {
                EmpStatusCbBx.SelectedIndex = 2;
            }
        }

        private void EmpUpdateBtn_Click(object sender, EventArgs e)
        {
            String p1 = @"^[a-z\s/]*$";
            String p2 = @"^\d*$";
            String p3 = @"[0-9]{2}[23456789]{1}\d{3}-\d{2}-\d{4}";      //check the month from the IC Number, make sure the first digit of the month is not starts from 2,3,4,5,6,7,8,9
            String p4 = @"[0-9]{2}[1]{1}[3456789]{1}\d{2}-\d{2}-\d{4}"; //check the month from the IC Number, make sure the second digit of the month is not starts from 3,4,5,6,7,8,9
            String p5 = @"[0-9]{4}[456789]{1}[0-9]{1}-\d{2}-\d{4}";     //check the day from the IC Number, make sure the first digit of the day is not starts from 4,5,6,7,8,9
            String p6 = @"[0-9]{4}[3]{1}[23456789]{1}-\d{2}-\d{4}";     //check the day from the IC Number, make sure the second digit of the day is not starts from 2,3,4,5,6,7,8,9
            String p7 = @"\d{6}-[0]{1}[0]{1}-\d{4}";                    //check the birthplace code from the IC Number, it does not contains 00                  
            String p8 = @"\d{6}-[1]{1}[789]{1}-\d{4}";                  //check the birthplace code from the IC Number, it does not contains 17,18,19
            String p9 = @"\d{6}-[2]{1}[0]{1}-\d{4}";                    //check the birthplace code from the IC Number, it does not contains 20
            String p10 = @"\d{6}-[6789]{1}[0123456789]{1}-\d{4}";       //check the birthplace code from the IC Number, it does not contains 60-69, 70-79, 80-89, 90-99
            //String p11 = @"[0]{1}[0-9]{5}-[0-9]{2}-[0-9]{4}";           //check the year from the IC Number, make sure the first digit is not starts from 0
            String p12 = @"[0-9]{2}[0]{4}-[0-9]{2}-[0-9]{4}";           //check the IC Number, it can't be like 920000-12-1234
            String p13 = @"[0-9]{3}[0]{3}-[0-9]{2}-[0-9]{4}";           //check the IC Number, it can't be like 921000-12-1234
            String p14 = @"[0-9]{4}[0]{2}-[0-9]{2}-[0-9]{4}";           //check the IC Number, it can't be like 920400-12-1234
            String p15 = @"\d{6}-\d{2}-[0]{4}";                         //check the IC Number, it can't be like 921221-12-0000
            String p16 = @"^\d{6}-\d{2}-\d{4}$";                        //check the IC Number, it should not contains any alphabetics and special symbols
            String p17 = @"\d{6}-\d{2}-\d{3}[13579]{1}";                //Gender Validation for Male
            String p18 = @"\d{6}-\d{2}-\d{3}[02468]{1}";                //Gender Validation for Female
            String p19 = @"^[a-z0-9\s.,/-]*$";                          //Address Validation, if the address not matched with this format, it may causes error
            String p20 = @"^[0-9\s.,/-]*$";                             //Address Validation, the address can't be like 85784,577873.
            String p21 = @"[1-9]{1}[0-9]{1}-[0-9]{8}";                  //Home No. Validation, the first digit must be zero
            String p22 = @"[0]{1}[0]{1}-[0-9]{8}";                      //Home No. Validation, the second digit cannot be zero
            String p23 = @"[0]{1}[125]{1}";                             //Home No. Validation, make sure the city code does not contains 01,02,05
            String p24 = @"[0-9]{2}-[0]{8}";                            //Home No. Validation, it can't be like 03-00000000
            String p25 = @"^\d{2}-\d{8}$";                              //Home No. Validation, it can't contains any alphabetics and any special symbols and also has exactly 11 charatcers
            String p26 = @"[1-9]{1}[0-9]{2}-[0-9]{7}";                  //Handphone No. Validation, the first digit must be zero
            String p27 = @"[0]{3}-[0-9]{7}";                            //Handphone No. Validation, it can't be like 000-1234567
            String p28 = @"[0-9]{3}-[0]{7}";                            //Handphone No. Validation, it can't be like 012-0000000
            String p29 = @"^\d{3}-\d{7}$";                                //Handphone No. Validation, it can't contains any alphabetics and any special symbols and also has exactly 11 characters
            String p30 = @"^[a-z0-9_.\-]+\@[a-z]+\.(?:[a-z]{3}|com|org|net|edu|gov)|\.(?:[a-z]{2}|my)$";    //Email Validation

            Regex rgx1 = new Regex(p1, RegexOptions.IgnoreCase);
            Regex rgx2 = new Regex(p2);
            Regex rgx3 = new Regex(p3);
            Regex rgx4 = new Regex(p4);
            Regex rgx5 = new Regex(p5);
            Regex rgx6 = new Regex(p6);
            Regex rgx7 = new Regex(p7);
            Regex rgx8 = new Regex(p8);
            Regex rgx9 = new Regex(p9);
            Regex rgx10 = new Regex(p10);
            //Regex rgx11 = new Regex(p11);
            Regex rgx12 = new Regex(p12);
            Regex rgx13 = new Regex(p13);
            Regex rgx14 = new Regex(p14);
            Regex rgx15 = new Regex(p15);
            Regex rgx16 = new Regex(p16);
            Regex rgx17 = new Regex(p17);
            Regex rgx18 = new Regex(p18);
            Regex rgx19 = new Regex(p19);
            Regex rgx20 = new Regex(p20);
            Regex rgx21 = new Regex(p21);
            Regex rgx22 = new Regex(p22);
            Regex rgx23 = new Regex(p23);
            Regex rgx24 = new Regex(p24);
            Regex rgx25 = new Regex(p25);
            Regex rgx26 = new Regex(p26);
            Regex rgx27 = new Regex(p27);
            Regex rgx28 = new Regex(p28);
            Regex rgx29 = new Regex(p29);
            Regex rgx30 = new Regex(p30);


            if (EmpNameTxtBx.Text == String.Empty || EmpICTxtBx.Text == String.Empty || EmpMaleRdBtn.Checked == false && EmpFemaleRdBtn.Checked == false || EmpAddress1TxtBx.Text == String.Empty && EmpAddress2TxtBx.Text == String.Empty || EmpHomeNoTxtBx.Text == String.Empty || EmpHandphoneNoTxtBx.Text == String.Empty || EmpEmailTxtBx.Text == String.Empty || EmpPositionCbBx.SelectedIndex == 0 || EmpStatusCbBx.SelectedIndex == 0)
            {
                ErrLbl.Visible = true;
                ErrLbl.Text = "*Please make sure you are completed all the fields.";
            }
            else
            {
                if (!rgx1.Match(EmpNameTxtBx.Text).Success)
                {
                    ErrLbl.Visible = true;
                    ErrLbl.Text = "*Please make sure Employee Name does not contains any numeric data. \r\n\r\n*Please make sure Employee Name does not contains any special symbol, except '/'.";
                    EmpNameTxtBx.Focus();
                }
                else if (EmpNameTxtBx.TextLength > 200 || EmpNameTxtBx.TextLength < 10)
                {
                    ErrLbl.Visible = true;
                    ErrLbl.Text = "*Please make sure Employee Name does not exceeds more than 200 characters. \r\n\r\n*Please make sure Employee Name has minimum 10 characters.";
                    EmpNameTxtBx.Focus();
                }
                else
                {
                    if (rgx2.Match(EmpICTxtBx.Text).Success)
                    {
                        ErrLbl.Visible = true;
                        ErrLbl.Text = "*IC Number is missing dashes '-'. \r\n\r\nPlease make sure IC Number is in such format 'xxxxxx-xx-xxxx'.";
                        EmpICTxtBx.Focus();
                    }
                    else if (EmpICTxtBx.TextLength > 14 || EmpICTxtBx.TextLength < 14)
                    {
                        ErrLbl.Visible = true;
                        ErrLbl.Text = "*Please make sure IC Number has exactly 14 characters.";
                        EmpICTxtBx.Focus();
                    }
                    else if (EmpICTxtBx.Text.Equals("000000-00-0000") || rgx12.Match(EmpICTxtBx.Text).Success || rgx13.Match(EmpICTxtBx.Text).Success || rgx14.Match(EmpICTxtBx.Text).Success || rgx15.Match(EmpICTxtBx.Text).Success)
                    {
                        ErrLbl.Visible = true;
                        ErrLbl.Text = "*Invalid IC Number!";
                        EmpICTxtBx.Focus();
                    }
                    else if (rgx3.Match(EmpICTxtBx.Text).Success || rgx4.Match(EmpICTxtBx.Text).Success)
                    {
                        ErrLbl.Visible = true;
                        ErrLbl.Text = "*Invalid IC Number!  Please make sure the month number is entered correctly.";
                        EmpICTxtBx.Focus();
                    }
                    else if (rgx5.Match(EmpICTxtBx.Text).Success || rgx6.Match(EmpICTxtBx.Text).Success)
                    {
                        ErrLbl.Visible = true;
                        ErrLbl.Text = "*Invalid IC Number!  Please make sure the day number is entered correctly.";
                        EmpICTxtBx.Focus();
                    }
                    else if (rgx7.Match(EmpICTxtBx.Text).Success || rgx8.Match(EmpICTxtBx.Text).Success || rgx9.Match(EmpICTxtBx.Text).Success || rgx10.Match(EmpICTxtBx.Text).Success)
                    {
                        ErrLbl.Visible = true;
                        ErrLbl.Text = "*Invalid IC Number!  Please make sure the birthplace code is entered correctly.";
                        EmpICTxtBx.Focus();
                    }
                    else if (!rgx16.Match(EmpICTxtBx.Text).Success)
                    {
                        ErrLbl.Visible = true;
                        ErrLbl.Text = "*Please make sure the IC Number does not contains any alphabetics.\r\n\r\n*Please make sure the IC Number does not contains any special symbols, except '-'.";
                        EmpICTxtBx.Focus();
                    }
                    else
                    {
                        if (rgx17.Match(EmpICTxtBx.Text).Success && EmpFemaleRdBtn.Checked == true || rgx18.Match(EmpICTxtBx.Text).Success && EmpMaleRdBtn.Checked == true)
                        {
                            ErrLbl.Visible = true;
                            ErrLbl.Text = "*Invalid Gender selected or IC Number may typed wrongly!\r\n\r\n*Please check your Gender and IC Number again.";
                        }
                        else
                        {
                            if (!rgx19.Match(EmpAddress1TxtBx.Text).Success)
                            {
                                ErrLbl.Visible = true;
                                ErrLbl.Text = "*Please make sure Address does not contains any special symbols, except ','  '.'  '/'  '-'.";
                                EmpAddress1TxtBx.Focus();
                            }
                            else if (rgx20.Match(EmpAddress1TxtBx.Text).Success)
                            {
                                ErrLbl.Visible = true;
                                ErrLbl.Text = "*Invalid Address!";
                                EmpAddress1TxtBx.Focus();
                            }
                            else if (EmpAddress1TxtBx.TextLength > 150 || EmpAddress1TxtBx.TextLength < 20)
                            {
                                ErrLbl.Visible = true;
                                ErrLbl.Text = "*Please make sure Address does not exceeds more than 150 characters. \r\n\r\n*Please make sure Address has minimum 20 characters.";
                                EmpAddress1TxtBx.Focus();
                            }
                            else
                            {
                                if (!(EmpAddress2TxtBx.Text == String.Empty) && !rgx19.Match(EmpAddress2TxtBx.Text).Success)
                                {
                                    ErrLbl.Visible = true;
                                    ErrLbl.Text = "*Please make sure Address does not contains any special symbols, except ','  '.'  '/'  '-'.";
                                    EmpAddress2TxtBx.Focus();
                                }
                                else if (!(EmpAddress2TxtBx.Text == String.Empty) && rgx20.Match(EmpAddress2TxtBx.Text).Success)
                                {
                                    ErrLbl.Visible = true;
                                    ErrLbl.Text = "*Invalid Address!";
                                    EmpAddress2TxtBx.Focus();
                                }
                                else if (!(EmpAddress2TxtBx.Text == String.Empty) && EmpAddress2TxtBx.TextLength > 50)
                                {
                                    ErrLbl.Visible = true;
                                    ErrLbl.Text = "*Please make sure Address does not exceeds more than 50 characters.";
                                    EmpAddress2TxtBx.Focus();
                                }
                                else
                                {
                                    if (rgx2.Match(EmpHomeNoTxtBx.Text).Success)
                                    {
                                        ErrLbl.Visible = true;
                                        ErrLbl.Text = "*Home Number is missing dashes '-'. \r\n\r\n*Please make sure Home Number is in such format 'xx-xxxxxxxx'.";
                                        EmpHomeNoTxtBx.Focus();
                                    }
                                    else if (rgx21.Match(EmpHomeNoTxtBx.Text).Success)
                                    {
                                        ErrLbl.Visible = true;
                                        ErrLbl.Text = "*Invalid Home Number!  Please make sure the first digit is zero.";
                                        EmpHomeNoTxtBx.Focus();
                                    }
                                    else if (rgx22.Match(EmpHomeNoTxtBx.Text).Success)
                                    {
                                        ErrLbl.Visible = true;
                                        ErrLbl.Text = "*Invalid Home Number!  Please make sure the second digit is not zero.";
                                        EmpHomeNoTxtBx.Focus();
                                    }
                                    else if (rgx23.Match(EmpHomeNoTxtBx.Text).Success)
                                    {
                                        ErrLbl.Visible = true;
                                        ErrLbl.Text = "*Invalid Home Number!  Malaysia city codes only contain 03, 04, 06, 07, 08, 09.";
                                        EmpHomeNoTxtBx.Focus();
                                    }
                                    else if (rgx24.Match(EmpHomeNoTxtBx.Text).Success)
                                    {
                                        ErrLbl.Visible = true;
                                        ErrLbl.Text = "*Invalid Home Number!";
                                        EmpHomeNoTxtBx.Focus();
                                    }
                                    else if (!rgx25.Match(EmpHomeNoTxtBx.Text).Success)
                                    {
                                        ErrLbl.Visible = true;
                                        ErrLbl.Text = "*Please make sure Home Number does not contains any alphabetics. \r\n\r\n*Please make sure Home Number does not contains any special symbols, except '-'. \r\n\r\n*Please make sure Home Number has exactly 11 characters.";
                                        EmpHomeNoTxtBx.Focus();
                                    }
                                    else
                                    {
                                        if (rgx2.Match(EmpHandphoneNoTxtBx.Text).Success)
                                        {
                                            ErrLbl.Visible = true;
                                            ErrLbl.Text = "*Handphone Number is missing dashes '-'. \r\n\r\n*Please make sure Handphone Number is in such format 'xxx-xxxxxxx'.";
                                            EmpHandphoneNoTxtBx.Focus();
                                        }
                                        else if (rgx26.Match(EmpHandphoneNoTxtBx.Text).Success)
                                        {
                                            ErrLbl.Visible = true;
                                            ErrLbl.Text = "*Invalid Handphone Number!  Please make sure the first digit is zero.";
                                            EmpHandphoneNoTxtBx.Focus();
                                        }
                                        else if (rgx27.Match(EmpHandphoneNoTxtBx.Text).Success)
                                        {
                                            ErrLbl.Visible = true;
                                            ErrLbl.Text = "*Invalid Handphone Number!";
                                            EmpHandphoneNoTxtBx.Focus();
                                        }
                                        else if (rgx28.Match(EmpHandphoneNoTxtBx.Text).Success)
                                        {
                                            ErrLbl.Visible = true;
                                            ErrLbl.Text = "*Invalid Handphone Number!";
                                            EmpHandphoneNoTxtBx.Focus();
                                        }
                                        else if (!rgx29.Match(EmpHandphoneNoTxtBx.Text).Success)
                                        {
                                            ErrLbl.Visible = true;
                                            ErrLbl.Text = "*Please make sure Handphone Number does not contains any alphabetics. \r\n\r\n*Please make sure Handphone Number does not contains any special symbols, except '-'. \r\n\r\n*Please make sure Handphone Number has exactly 11 characters.";
                                            EmpHandphoneNoTxtBx.Focus();
                                        }
                                        else
                                        {
                                            if (!rgx30.Match(EmpEmailTxtBx.Text).Success)
                                            {
                                                ErrLbl.Visible = true;
                                                ErrLbl.Text = "*Invalid Email address!";
                                                EmpEmailTxtBx.Focus();
                                            }
                                            else if (EmpEmailTxtBx.TextLength > 200 || EmpEmailTxtBx.TextLength < 10)
                                            {
                                                ErrLbl.Visible = true;
                                                ErrLbl.Text = "*Please make sure Email address does not exceeds more than 200 characters. \r\n\r\n*Please make sure Email address has minimum 10 characters.";
                                                EmpEmailTxtBx.Focus();
                                            }
                                            else
                                            {
                                                Employee emp = new Employee();
                                                EmployeeDA empDA = new EmployeeDA();

                                                emp.EmployeeID = EmpIDDTxtBx.Text;
                                                emp.EmployeeName = EmpNameTxtBx.Text;
                                                emp.ICNumber = EmpICTxtBx.Text;

                                                if (EmpMaleRdBtn.Checked == true)
                                                {
                                                    emp.Gender = EmpMaleRdBtn.Text;
                                                }
                                                else
                                                {
                                                    emp.Gender = EmpFemaleRdBtn.Text;
                                                }

                                                if (!(EmpAddress1TxtBx.Text == String.Empty) && !(EmpAddress2TxtBx.Text == String.Empty))
                                                {
                                                    emp.HomeAddress = EmpAddress1TxtBx.Text + ", " + EmpAddress2TxtBx.Text;
                                                }
                                                else
                                                {
                                                    emp.HomeAddress = EmpAddress1TxtBx.Text;
                                                }
                                                emp.HomeNumber = EmpHomeNoTxtBx.Text;
                                                emp.HandphoneNumber = EmpHandphoneNoTxtBx.Text;
                                                emp.Email = EmpEmailTxtBx.Text;
                                                emp.Position = EmpPositionCbBx.SelectedItem.ToString();
                                                emp.CurrentStatus = EmpStatusCbBx.SelectedItem.ToString();

                                                empDA.EmployeeUpdateRecord(emp);

                                                if (emp.UpdateStatus == "Success")
                                                {
                                                    MessageBox.Show("Employee record has updated successfully!");

                                                    EmpIDDTxtBx.Text = "Auto-Generated";
                                                    EmpNameTxtBx.Clear();
                                                    EmpICTxtBx.Clear();
                                                    EmpMaleRdBtn.Checked = false;
                                                    EmpFemaleRdBtn.Checked = false;
                                                    EmpAddress1TxtBx.Clear();
                                                    EmpAddress2TxtBx.Clear();
                                                    EmpHomeNoTxtBx.Clear();
                                                    EmpHandphoneNoTxtBx.Clear();
                                                    EmpEmailTxtBx.Clear();
                                                    EmpPositionCbBx.SelectedIndex = 0;
                                                    EmpStatusCbBx.SelectedIndex = 0;

                                                    //EmpIdCbBx.SelectedIndex = -1;
                                                    EmpInsertBtn.Enabled = true;
                                                    ErrLbl.Visible = false;
                                                }
                                                else if (emp.UpdateStatus == "Failed")
                                                {
                                                    MessageBox.Show("Failed to update employee record!");
                                                }   
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void EmpResetBtn_Click(object sender, EventArgs e)
        {
            EmpIDDTxtBx.Text = "Auto-Generated";
            EmpNameTxtBx.Clear();
            EmpICTxtBx.Clear();
            EmpMaleRdBtn.Checked = false;
            EmpFemaleRdBtn.Checked = false;
            EmpAddress1TxtBx.Clear();
            EmpAddress2TxtBx.Clear();
            EmpHomeNoTxtBx.Clear();
            EmpHandphoneNoTxtBx.Clear();
            EmpEmailTxtBx.Clear();
            EmpPositionCbBx.SelectedIndex = 0;
            EmpStatusCbBx.SelectedIndex = 0;

            //EmpIdCbBx.SelectedIndex = -1;
            EmpInsertBtn.Enabled = true;
            ErrLbl.Visible = false;
        }

        private void EmpNmTxtBx_TextChanged(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "SELECT * FROM Employee WHERE EmployeeName like '" + EmpNmTxtBx.Text + "%'";

                SqlDataAdapter da = new SqlDataAdapter(sql, con);

                DataSet ds = new DataSet();

                con.Open();

                da.Fill(ds, "Employee");        //Employee is the table name

                con.Close();

                EmpDtGrdVw.DataSource = ds;
                EmpDtGrdVw.DataMember = "Employee";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Employee's data grid view cannot read the database!");
            }
        }

        private void EmpIdCbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string idd = EmpPostCbBx.SelectedItem.ToString();

                string sql = "SELECT * FROM Employee WHERE Position like '" + idd + "%'";

                SqlDataAdapter da = new SqlDataAdapter(sql, con);

                DataSet ds = new DataSet();

                con.Open();

                da.Fill(ds, "Employee");        //Employee is the table name

                con.Close();

                EmpDtGrdVw.DataSource = ds;
                EmpDtGrdVw.DataMember = "Employee";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Employee's data grid view cannot read the database!");
            }
        } 
    }
    }
