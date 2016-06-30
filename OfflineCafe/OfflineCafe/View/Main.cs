using OfflineCafe.DataAccess;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;

namespace OfflineCafe
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection();
        List<Food> fList = new List<Food>();
        List<Food> fl = new List<Food>();
    
        public static List<Food> staticfList;
        public static List<Food> staticeList;
        public Form1()
        {
            InitializeComponent();
            //form date
            label7.Text = "Today is " + DateTime.Now.ToShortDateString();
            //pnl close

            pnlMenu.Visible = false;
            pnlEmp.Visible = false;
            pnlPwChg.Visible = false;
            pnlMain.Visible = true;

            EmployeeDataFill();
            EmpDtGrdVw.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            SupplierDataFill();
            SuppDtGdVw.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            IngredientDataFill();
            IngDtGdVw.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            IngPODataFill();
            IngPODtGrdVw.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            POSupplierDetailsDataFill();
            POSupplierDtGrdVw.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            EmpPositionCbBx.SelectedIndex = 0;
            EmpStatusCbBx.SelectedIndex = 0;
            EmpUpdateBtn.Enabled = false;
            SuppStatusCbBx.SelectedIndex = 0;
            SuppUpdateBtn.Enabled = false;
            UnitCbBx.SelectedIndex = 0;
            StorageAreaCbBx.SelectedIndex = 0;
            ExpiryDateTimePicker.Value = System.DateTime.Now;
            UnitCbBx.SelectedIndex = 0;
            ReOrderLevelCbBx.SelectedIndex = 0;
            IngStatusCbBx.SelectedIndex = 0;
            IngUpdateBtn.Enabled = false;

            IngPODtGrdVw.Enabled = false;
            IngPODtGrdVw2.Enabled = false;

            //for (int i = 0; i < staticeList.Count; i++)
            //{
            //    label34.Text = staticeList.ElementAt(i).foodName;             
            //}
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
            //  this.menuTableAdapter.Fill(this.cafeManagementDataSet2.Menu);
            // TODO: This line of code loads data into the 'cafeManagementDataSet.Food' table. You can move, or remove it, as needed.
            //   this.foodTableAdapter1.Fill(this.cafeManagementDataSet.Food);
            //display data grid view
            //  this.foodTableAdapter.Fill(this.cafeManagementDataSet1.Food);

            EmpDtGrdVw.AutoGenerateColumns = false;
            SuppDtGdVw.AutoGenerateColumns = false;
            IngDtGdVw.AutoGenerateColumns = false;
            IngPODtGrdVw.AutoGenerateColumns = false;
            //IngPODtGrdVw2.Auto
            POSupplierDtGrdVw.AutoGenerateColumns = false;

            IngredientTimer.Interval = 1000;
            IngredientTimer.Start();

            OrderDateTxtBx.Text = System.DateTime.Now.ToShortDateString();
            OrderTimeTxtBx.Text = System.DateTime.Now.ToShortTimeString();
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
                    MessageBox.Show("New Ala Carte Food record has inserted successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                    MessageBox.Show("Ala Carte Food record has updated successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                    MessageBox.Show("New Menu Sets record has inserted successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Menu Sets record has updated successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
        //menu btn
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
            pnlPwChg.Visible = false;
            pnlSupplier.Visible = false;
            pnlItem.Visible = false;
            pnlPO.Visible = false;
        }
        //employee btn
        private void button1_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < staticeList.Count; i++)
            //{               
            //    if (staticeList.ElementAt(i).foodType == "Noodle")
            //    {
            pnlEmp.Visible =true;
            pnlSupplier.Visible = false;
            pnlItem.Visible = false;
            pnlPO.Visible = false;
                //}
                //else
                //{
                   // pnlMain.Visible = true;
            //    }             
            //}
        }
        //supplier btn
        private void button2_Click(object sender, EventArgs e)
        {
            pnlMenu.Visible = false;
            pnlEmp.Visible = false;
            pnlMain.Visible = false;
            pnlItem.Visible = false;
            pnlPO.Visible = false;
            pnlSupplier.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pnlMenu.Visible = false;
            pnlEmp.Visible = false;
            pnlSupplier.Visible = false;
            pnlPO.Visible = false;
            pnlItem.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pnlMenu.Visible = false;
            pnlEmp.Visible = false;
            pnlSupplier.Visible = false;
            pnlPO.Visible = false;
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
        //profile
        private void button10_Click(object sender, EventArgs e)
        {
            txtPw1.Focus();
            pnlPwChg.Visible = true;
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
            if (r == DialogResult.Yes) { this.Close();
                OfflineCafe.View.Login login = new OfflineCafe.View.Login();
                login.Visible = true;
                this.Visible = false;

            }
            else { }
        }

        private void btnPw_Click(object sender, EventArgs e)
        {
            try {
                for (int i = 0; i < staticeList.Count; i++)
                {
                    LoginDA lda = new LoginDA();
                    List<Entity.Login> ll = lda.searchid2();

                    if (txtPw1.Text != ll.ElementAt(i).password)
                    {
                        MessageBox.Show("Your current password does not match", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtPw1.Text = "";
                        txtPw1.Focus();
                    }
                    else if (txtPw1.Text == "" || txtPw2.Text == "" || txtPw3.Text == "")
                    {
                        MessageBox.Show("Please enter the empty field ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (txtPw2.Text != txtPw3.Text)
                    {
                        MessageBox.Show("New Password and Confirm Password does not match ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtPw2.Text = "";
                        txtPw2.Focus();
                    }
                    else if (txtPw1.Text.Length <= 6 && txtPw2.Text.Length < 6 && txtPw3.Text.Length<6)
                    {
                        MessageBox.Show("Length of Current Password, New Password and Confirm Password must more than 6", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
                    }
                    else if (txtPw2.Text == staticeList.ElementAt(i).foodName || (txtPw3.Text == staticeList.ElementAt(i).foodName))
                    {
                        MessageBox.Show("New Password and Confirm Password can not enter your IC number", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtPw2.Text = "";
                        txtPw2.Focus();
                        txtPw3.Text = "";
                    }
                    else if ((!txtPw2.Text.Any(Char.IsLetter) || !txtPw2.Text.Any(Char.IsDigit)) && (!txtPw3.Text.Any(Char.IsLetter) || !txtPw3.Text.Any(Char.IsDigit)))
                    {
                        MessageBox.Show("New Password and Confirm Password must contains numbers and letters", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtPw2.Text = "";
                        txtPw2.Focus();
                        txtPw3.Text = "";
                    }
                    else
                    {                     
                        Entity.Login l = new Entity.Login();
                        l.loginid = staticeList.ElementAt(i).foodID;
                        l.password = txtPw3.Text;
                        lda.updatePassword(l);
                        MessageBox.Show("Your new password successfully to changed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPw2_Click(object sender, EventArgs e)
        {
            pnlPwChg.Visible = false;
            pnlMain.Visible = true;
        }

        private void EmpInsertBtn_Click(object sender, EventArgs e)
        {
            Employee emp = new Employee();
            EmployeeDA empDA = new EmployeeDA();

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
            String p31 = @"^\d-*$";

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
            Regex rgx31 = new Regex(p31);

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
                    if(!rgx16.Match(EmpICTxtBx.Text).Success)
                    {
                        ErrLbl.Visible = true;
                        ErrLbl.Text = "*Please make sure IC Number does not contains any alphabetics. \r\n\r\n*Please make sure IC Number does not contains any special symbols, except '-'. \r\n\r\n*Please make sure IC Number has exactly 14 characters. \r\n\r\n*Please make sure IC Number is in such format '123456-12-1234'.";
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
                                ErrLbl.Text = "*Please make sure first Address textfield does not contains any special symbols, except ','  '.'  '/'  '-'.";
                                EmpAddress1TxtBx.Focus();
                            }
                            else if (rgx20.Match(EmpAddress1TxtBx.Text).Success)
                            {
                                ErrLbl.Visible = true;
                                ErrLbl.Text = "*Invalid Address!";
                                EmpAddress1TxtBx.Focus();
                            }
                            else if (EmpAddress1TxtBx.TextLength > 170 || EmpAddress1TxtBx.TextLength < 20)
                            {
                                ErrLbl.Visible = true;
                                ErrLbl.Text = "*Please make sure first Address textfield does not exceeds more than 170 characters. \r\n\r\n*Please make sure first Address textfield has minimum 20 characters.";
                                EmpAddress1TxtBx.Focus();
                            }
                            else
                            {
                                if (!(EmpAddress2TxtBx.Text == String.Empty) && !rgx19.Match(EmpAddress2TxtBx.Text).Success)
                                {
                                    ErrLbl.Visible = true;
                                    ErrLbl.Text = "*Please make sure second Address textfield does not contains any special symbols, except ','  '.'  '/'  '-'.";
                                    EmpAddress2TxtBx.Focus();
                                }
                                else if (!(EmpAddress2TxtBx.Text == String.Empty) && rgx20.Match(EmpAddress2TxtBx.Text).Success)
                                {
                                    ErrLbl.Visible = true;
                                    ErrLbl.Text = "*Invalid Address!";
                                    EmpAddress2TxtBx.Focus();
                                }
                                else if (!(EmpAddress2TxtBx.Text == String.Empty) && EmpAddress2TxtBx.TextLength > 30)
                                {
                                    ErrLbl.Visible = true;
                                    ErrLbl.Text = "*Please make sure second Address textfield does not exceeds more than 30 characters.";
                                    EmpAddress2TxtBx.Focus();
                                }
                                else
                                {
                                    if (!rgx25.Match(EmpHomeNoTxtBx.Text).Success)
                                    {
                                        ErrLbl.Visible = true;
                                        ErrLbl.Text = "*Please make sure Home Number does not contains any alphabetics. \r\n\r\n*Please make sure Home Number does not contains any special symbols, except '-'. \r\n\r\n*Please make sure Home Number has exactly 11 characters. \r\n\r\n*Please make sure Home Number is in such format '03-12345678'.";
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
                                    else
                                    {
                                        if (!rgx29.Match(EmpHandphoneNoTxtBx.Text).Success)
                                        {
                                            ErrLbl.Visible = true;
                                            ErrLbl.Text = "*Please make sure Handphone Number does not contains any alphabetics. \r\n\r\n*Please make sure Handphone Number does not contains any special symbols, except '-'. \r\n\r\n*Please make sure Handphone Number has exactly 11 characters. \r\n\r\n*Please make sure Handphone Number is in such format '012-1234567'.";
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

                                                empDA.EmployeeInsertRecord(emp);
                                            }
                                                if (emp.InsertStatus == "Success")
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

                                                    EmpInsertBtn.Enabled = true;
                                                    ErrLbl.Visible = false;

                                                    try
                                                    {
                                                        empDA.RetreiveEmployeeID(emp);
                                                        empDA.RetrieveAdminName(emp);

                                                        //SMTP Authentication
                                                        MailMessage MailMsg = new MailMessage();
                                                        SmtpClient client = new SmtpClient();
                                                        client.Host = "smtp.gmail.com";     //Gmail's SMTP server address
                                                        client.Port = 587;
                                                        client.EnableSsl = true;
                                                        client.DeliveryMethod = SmtpDeliveryMethod.Network;     //Specifies how email message is delivered. Network means the email is sent through the network to an SMTP server.
                                                        client.Credentials = new System.Net.NetworkCredential("abbytan0415@gmail.com", "7aubenfelD");

                                                        MailMsg.From = new MailAddress("abbytan0415@gmail.com", "The Coffee Bean");
                                                        MailMsg.To.Add(emp.Email);
                                                        MailMsg.Subject = "Welcome to The Coffee Bean";
                                                        MailMsg.Body = "Dear, " + emp.EmployeeName + "\n\nYou have been hired by The Coffee Bean shop. Your Employee ID is " + emp.EmployeeIDRetrieve + "." + "\n\nPlease carefully check of your personal information as below:" + "\n\nName : " + emp.EmployeeName + "\n\nIC No. : " + emp.ICNumber + "\n\nGender : " + emp.Gender + "\n\nHome Address : " + emp.HomeAddress + "\n\nHome No. : " + emp.HomeNumber + "\n\nHandphone No. : " + emp.HandphoneNumber + "\n\nEmail : " + emp.Email + "\n\nIf your personal information has any mistakes, please contact administrator." + "\n\n\nRegards, \n" + emp.AdminNameRetrieve + "\nAdministrator";
                                                        MailMsg.BodyEncoding = System.Text.Encoding.UTF8;       //Encode body message
                                                        MailMsg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;    //Send back notification only in case of failure of delivery email message
                                                        client.Send(MailMsg);
                                                        MessageBox.Show("Employee record has inserted successfully and Email sent!");
                                                    }
                                                    catch(Exception ex)
                                                    {
                                                        MessageBox.Show(ex.ToString());
                                                    }
                                                }
                                                else
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
            catch (Exception ex)
            {
                MessageBox.Show("Employee's data grid view cannot read the database!");
                throw ex;
            }
        }
        private void EmpUpdateBtn_Click(object sender, EventArgs e)
        {
            String p1 = @"^[a-z\s/]*$";                                 //check the employee name, employee name cannot contains any digit numbers and special symbols
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
                    if (!rgx16.Match(EmpICTxtBx.Text).Success)
                    {
                        ErrLbl.Visible = true;
                        ErrLbl.Text = "*Please make sure IC Number does not contains any alphabetics. \r\n\r\n*Please make sure IC Number does not contains any special symbols, except '-'. \r\n\r\n*Please make sure IC Number has exactly 14 characters. \r\n\r\n*Please make sure IC Number is in such format '123456-12-1234'.";
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
                                ErrLbl.Text = "*Please make sure first Address textfield does not contains any special symbols, except ','  '.'  '/'  '-'.";
                                EmpAddress1TxtBx.Focus();
                            }
                            else if (rgx20.Match(EmpAddress1TxtBx.Text).Success)
                            {
                                ErrLbl.Visible = true;
                                ErrLbl.Text = "*Invalid Address!";
                                EmpAddress1TxtBx.Focus();
                            }
                            else if (EmpAddress1TxtBx.TextLength > 170 || EmpAddress1TxtBx.TextLength < 20)
                            {
                                ErrLbl.Visible = true;
                                ErrLbl.Text = "*Please make sure first Address textfield does not exceeds more than 170 characters. \r\n\r\n*Please make sure first Address textfield has minimum 20 characters.";
                                EmpAddress1TxtBx.Focus();
                            }
                            else
                            {
                                if (!(EmpAddress2TxtBx.Text == String.Empty) && !rgx19.Match(EmpAddress2TxtBx.Text).Success)
                                {
                                    ErrLbl.Visible = true;
                                    ErrLbl.Text = "*Please make sure second Address textfield does not contains any special symbols, except ','  '.'  '/'  '-'.";
                                    EmpAddress2TxtBx.Focus();
                                }
                                else if (!(EmpAddress2TxtBx.Text == String.Empty) && rgx20.Match(EmpAddress2TxtBx.Text).Success)
                                {
                                    ErrLbl.Visible = true;
                                    ErrLbl.Text = "*Invalid Address!";
                                    EmpAddress2TxtBx.Focus();
                                }
                                else if (!(EmpAddress2TxtBx.Text == String.Empty) && EmpAddress2TxtBx.TextLength > 30)
                                {
                                    ErrLbl.Visible = true;
                                    ErrLbl.Text = "*Please make sure second Address textfield does not exceeds more than 30 characters.";
                                    EmpAddress2TxtBx.Focus();
                                }
                                else
                                {
                                    if (!rgx25.Match(EmpHomeNoTxtBx.Text).Success)
                                    {
                                        ErrLbl.Visible = true;
                                        ErrLbl.Text = "*Please make sure Home Number does not contains any alphabetics. \r\n\r\n*Please make sure Home Number does not contains any special symbols, except '-'. \r\n\r\n*Please make sure Home Number has exactly 11 characters. \r\n\r\n*Please make sure Home Number is in such format '03-12345678'.";
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
                                    else
                                    {
                                        if (!rgx29.Match(EmpHandphoneNoTxtBx.Text).Success)
                                        {
                                            ErrLbl.Visible = true;
                                            ErrLbl.Text = "*Please make sure Handphone Number does not contains any alphabetics. \r\n\r\n*Please make sure Handphone Number does not contains any special symbols, except '-'. \r\n\r\n*Please make sure Handphone Number has exactly 11 characters. \r\n\r\n*Please make sure Handphone Number is in such format '012-1234567'.";
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

        private void EmpRefreshBtn_Click(object sender, EventArgs e)
        {
            EmployeeDataFill();
        }

        private void EmpDtGrdVw_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            EmpInsertBtn.Enabled = false;
            EmpUpdateBtn.Enabled = true;

            int i;
            i = EmpDtGrdVw.SelectedCells[0].RowIndex;

            EmpIDDTxtBx.Text = EmpDtGrdVw.Rows[i].Cells[0].Value.ToString();
            EmpNameTxtBx.Text = EmpDtGrdVw.Rows[i].Cells[1].Value.ToString();
            EmpICTxtBx.Text = EmpDtGrdVw.Rows[i].Cells[2].Value.ToString();

            string g = EmpDtGrdVw.Rows[i].Cells[3].Value.ToString();

            if (g.Equals("Male"))
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

            if (p.Equals("Admin"))
            {
                EmpPositionCbBx.SelectedIndex = 1;
            }
            else if (p.Equals("Cashier"))
            {
                EmpPositionCbBx.SelectedIndex = 2;
            }
            else if (p.Equals("Chef"))
            {
                EmpPositionCbBx.SelectedIndex = 3;
            }
            else if (p.Equals("Manager"))
            {
                EmpPositionCbBx.SelectedIndex = 4;
            }

            string cs = EmpDtGrdVw.Rows[i].Cells[9].Value.ToString();

            if (cs.Equals("Hired"))
            {
                EmpStatusCbBx.SelectedIndex = 1;
            }
            else if (cs.Equals("Resigned"))
            {
                EmpStatusCbBx.SelectedIndex = 2;
            }
            else if(cs.Equals("Fired"))
            {
                EmpStatusCbBx.SelectedIndex = 3;
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

            EmpInsertBtn.Enabled = true;
            EmpUpdateBtn.Enabled = false;
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
                MessageBox.Show("Can't find the Employee Name!");
                throw ex;
            }
        }

        private void EmpPostCbBx_SelectedIndexChanged(object sender, EventArgs e)
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
                MessageBox.Show("Can't find the Employee Post!");
                throw ex;
            }
        }

        private void SuppInsertBtn_Click(object sender, EventArgs e)
        {
            String p1 = @"^[a-z\s/]*$";
            String p2 = @"^\d*$";
            String p3 = @"[1-9]{1}[0-9]{2}-[0-9]{7}";                  
            String p4 = @"[0]{3}-[0-9]{7}";                            
            String p5 = @"[0-9]{3}-[0]{7}";                            
            String p6 = @"^\d{3}-\d{7}$";
            String p7 = @"^[a-z0-9_.\-]+\@[a-z]+\.(?:[a-z]{3}|com|org|net|edu|gov)|\.(?:[a-z]{2}|my)$";
            String p8 = @"^[a-z0-9\s.,/-]*$";                          
            String p9 = @"^[0-9\s.,/-]*$";
            String p10 = @"[1-9]{1}[0-9]{1}-[0-9]{8}";                  
            String p11 = @"[0]{1}[0]{1}-[0-9]{8}";                      
            String p12 = @"[0]{1}[125]{1}";                             
            String p13 = @"[0-9]{2}-[0]{8}";                           
            String p14 = @"^\d{2}-\d{8}$";                             

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
            Regex rgx11 = new Regex(p11);
            Regex rgx12 = new Regex(p12);
            Regex rgx13 = new Regex(p13);
            Regex rgx14 = new Regex(p14);

            if(SuppNameTxtBx.Text == String.Empty || SuppMaleRdBtn.Checked == false && SuppFemaleRdBtn.Checked == false || SuppHandphoneTxtBx.Text == String.Empty || SuppEmailTxtBx.Text == String.Empty || CmpyNameTxtBx.Text == String.Empty || CmpyAddress1TxtBx.Text == String.Empty && CmpyAddress2TxtBx.Text == String.Empty || CmpyNumberTxtBx.Text == String.Empty || CmpyFaxNumberTxtBx.Text == String.Empty || SuppStatusCbBx.SelectedIndex == 0)
            {
                SuppErrLbl.Visible = true;
                SuppErrLbl.Text = "*Please make sure all the fields are completed.";
            }
            else
            {
                if(!rgx1.Match(SuppNameTxtBx.Text).Success)
                {
                    SuppErrLbl.Visible = true;
                    SuppErrLbl.Text = "*Please make sure Supplier Name does not contains any numeric data. \r\n\r\n*Please make sure Supplier Name does not contains any special symbol, except '/'.";
                    SuppNameTxtBx.Focus();
                }
                else if(SuppNameTxtBx.TextLength > 200 || SuppNameTxtBx.TextLength < 10)
                {
                    SuppErrLbl.Visible = true;
                    SuppErrLbl.Text = "*Please make sure Supplier Name does not exceeds more than 200 characters. \r\n\r\n*Please make sure Supplier Name has minimum 10 characters.";
                    SuppNameTxtBx.Focus();
                }
                else
                {
                    if(rgx2.Match(SuppHandphoneTxtBx.Text).Success)
                    {
                        SuppErrLbl.Visible = true;
                        SuppErrLbl.Text = "*Handphone Number is missing dashes '-'. \r\n\r\n*Please make sure Handphone Number is in such format 'xxx-xxxxxxx'.";
                        SuppHandphoneTxtBx.Focus();
                    }
                    else if(rgx3.Match(SuppHandphoneTxtBx.Text).Success)
                    {
                        SuppErrLbl.Visible = true;
                        SuppErrLbl.Text = "*Invalid Handphone Number!  Please make sure the first digit is zero.";
                        SuppHandphoneTxtBx.Focus();
                    }
                    else if(rgx4.Match(SuppHandphoneTxtBx.Text).Success)
                    {
                        SuppErrLbl.Visible = true;
                        SuppErrLbl.Text = "*Invalid Handphone Number!";
                        SuppHandphoneTxtBx.Focus();
                    }
                    else if(rgx5.Match(SuppHandphoneTxtBx.Text).Success)
                    {
                        SuppErrLbl.Visible = true;
                        SuppErrLbl.Text = "*Invalid Handphone Number!";
                        SuppHandphoneTxtBx.Focus();
                    }
                    else if(!rgx6.Match(SuppHandphoneTxtBx.Text).Success)
                    {
                        SuppErrLbl.Visible = true;
                        SuppErrLbl.Text = "*Please make sure Handphone Number does not contains any alphabetics and any special symbols, except '-'. \r\n\r\n*Please make sure Handphone Number has exactly 11 characters. \r\n\r\n*Please make sure Handphone Number is in such format 'xxx-xxxxxxx'.";
                        SuppHandphoneTxtBx.Focus();
                    }
                    else
                    {
                        if(!rgx7.Match(SuppEmailTxtBx.Text).Success)
                        {
                            SuppErrLbl.Visible = true;
                            SuppErrLbl.Text = "*Invalid Email Address!";
                            SuppEmailTxtBx.Focus();
                        }
                        else if(SuppEmailTxtBx.TextLength > 200 || SuppEmailTxtBx.TextLength < 10)
                        {
                            SuppErrLbl.Visible = true;
                            SuppErrLbl.Text = "*Please make sure Email address does not exceeds more than 200 characters. \r\n\r\n*Please make sure Email address has minimum 10 characters.";
                            SuppEmailTxtBx.Focus();
                        }
                        else
                        {
                            if(!rgx8.Match(CmpyAddress1TxtBx.Text).Success)
                            {
                                SuppErrLbl.Visible = true;
                                SuppErrLbl.Text = "*Please make sure first Address textfield does not contains any special symbols, except ','  '.'  '/'  '-'.";
                                CmpyAddress1TxtBx.Focus();
                            }
                            else if(rgx9.Match(CmpyAddress1TxtBx.Text).Success)
                            {
                                SuppErrLbl.Visible = true;
                                SuppErrLbl.Text = "*Invalid Address!";
                                CmpyAddress1TxtBx.Focus();
                            }
                            else if(CmpyAddress1TxtBx.TextLength > 170 || CmpyAddress1TxtBx.TextLength < 20)
                            {
                                SuppErrLbl.Visible = true;
                                SuppErrLbl.Text = "*Please make sure first Address textfield does not exceeds more than 170 characters. \r\n\r\n*Please make sure first Address textfield has minimum 20 characters.";
                                CmpyAddress1TxtBx.Focus();
                            }
                            else
                            {
                                if(!(CmpyAddress2TxtBx.Text == String.Empty) && !rgx8.Match(CmpyAddress2TxtBx.Text).Success)
                                {
                                    SuppErrLbl.Visible = true;
                                    SuppErrLbl.Text = "*Please make sure second Address textfield does not contains any special symbols, except ','  '.'  '/'  '-'.";
                                    CmpyAddress2TxtBx.Focus();
                                }
                                else if(!(CmpyAddress2TxtBx.Text == String.Empty) && rgx9.Match(CmpyAddress2TxtBx.Text).Success)
                                {
                                    SuppErrLbl.Visible = true;
                                    SuppErrLbl.Text = "*Invalid Address!";
                                    CmpyAddress2TxtBx.Focus();
                                }
                                else if(!(CmpyAddress2TxtBx.Text == String.Empty) && CmpyAddress2TxtBx.TextLength > 30)
                                {
                                    SuppErrLbl.Visible = true;
                                    SuppErrLbl.Text = "*Please make sure second Address textfield does not exceeds more than 30 characters.";
                                    CmpyAddress2TxtBx.Focus();
                                }
                                else
                                {
                                    if(rgx2.Match(CmpyNumberTxtBx.Text).Success)
                                    {
                                        SuppErrLbl.Visible = true;
                                        SuppErrLbl.Text = "*Home Number is missing dashes '-'. \r\n\r\n*Please make sure Home Number is in such format 'xx-xxxxxxxx'.";
                                        CmpyNumberTxtBx.Focus();
                                    }
                                    else if(rgx10.Match(CmpyNumberTxtBx.Text).Success)
                                    {
                                        SuppErrLbl.Visible = true;
                                        SuppErrLbl.Text = "*Invalid Home Number!  Please make sure the first digit is zero.";
                                        CmpyNumberTxtBx.Focus();
                                    }
                                    else if(rgx11.Match(CmpyNumberTxtBx.Text).Success)
                                    {
                                        SuppErrLbl.Visible = true;
                                        SuppErrLbl.Text = "*Invalid Home Number!  Please make sure the second digit is not zero.";
                                        CmpyNumberTxtBx.Focus();
                                    }
                                    else if(rgx12.Match(CmpyNumberTxtBx.Text).Success)
                                    {
                                        SuppErrLbl.Visible = true;
                                        SuppErrLbl.Text = "*Invalid Home Number!  Malaysia city codes only contain 03, 04, 06, 07, 08, 09.";
                                        CmpyNumberTxtBx.Focus();
                                    }
                                    else if(rgx13.Match(CmpyNumberTxtBx.Text).Success)
                                    {
                                        SuppErrLbl.Visible = true;
                                        SuppErrLbl.Text = "*Invalid Home Number!";
                                        CmpyNumberTxtBx.Focus();
                                    }
                                    else if(!rgx14.Match(CmpyNumberTxtBx.Text).Success)
                                    {
                                        SuppErrLbl.Visible = true;
                                        SuppErrLbl.Text = "*Please make sure Home Number does not contains any alphabetics. \r\n\r\n*Please make sure Home Number does not contains any special symbols, except '-'. \r\n\r\n*Please make sure Home Number has exactly 11 characters.";
                                        CmpyNumberTxtBx.Focus();
                                    }
                                    else
                                    {
                                        if(rgx2.Match(CmpyFaxNumberTxtBx.Text).Success)
                                        {
                                            SuppErrLbl.Visible = true;
                                            SuppErrLbl.Text = "*Fax Number is missing dashes '-'. \r\n\r\n*Please make sure Fax Number is in such format 'xx-xxxxxxxx'.";
                                            CmpyFaxNumberTxtBx.Focus();
                                        }
                                        else if(rgx10.Match(CmpyFaxNumberTxtBx.Text).Success)
                                        {
                                            SuppErrLbl.Visible = true;
                                            SuppErrLbl.Text = "*Invalid Fax Number!  Please make sure the first digit is zero.";
                                            CmpyFaxNumberTxtBx.Focus();
                                        }
                                        else if(rgx11.Match(CmpyFaxNumberTxtBx.Text).Success)
                                        {
                                            SuppErrLbl.Visible = true;
                                            SuppErrLbl.Text = "*Invalid Fax Number!  Please make sure the second digit is not zero.";
                                            CmpyFaxNumberTxtBx.Focus();
                                        }
                                        else if(rgx12.Match(CmpyFaxNumberTxtBx.Text).Success)
                                        {
                                            SuppErrLbl.Visible = true;
                                            SuppErrLbl.Text = "*Invalid Fax Number!  Malaysia city codes only contain 03, 04, 06, 07, 08, 09.";
                                            CmpyFaxNumberTxtBx.Focus();
                                        }
                                        else if(rgx13.Match(CmpyFaxNumberTxtBx.Text).Success)
                                        {
                                            SuppErrLbl.Visible = true;
                                            SuppErrLbl.Text = "*Invalid Fax Number!";
                                            CmpyFaxNumberTxtBx.Focus();
                                        }
                                        else if(!rgx14.Match(CmpyFaxNumberTxtBx.Text).Success)
                                        {
                                            SuppErrLbl.Visible = true;
                                            SuppErrLbl.Text = "*Please make sure Fax Number does not contains any alphabetics. \r\n\r\n*Please make sure Fax Number does not contains any special symbols, except '-'. \r\n\r\n*Please make sure Fax Number has exactly 11 characters.";
                                            CmpyFaxNumberTxtBx.Focus();
                                        }
                                        else
                                        {
                                            Supplier supp = new Supplier();
                                            SupplierDA suppDA = new SupplierDA();

                                            supp.SupplierName = SuppNameTxtBx.Text;
                                            
                                            if(SuppMaleRdBtn.Checked == true)
                                            {
                                                supp.SupplierGender = SuppMaleRdBtn.Text;
                                            }
                                            else
                                            {
                                                supp.SupplierGender = SuppFemaleRdBtn.Text;
                                            }

                                            supp.SupplierHandphoneNum = SuppHandphoneTxtBx.Text;
                                            supp.SupplierEmail = SuppEmailTxtBx.Text;
                                            supp.CompanyName = CmpyNameTxtBx.Text;
                                            
                                            if(!(CmpyAddress1TxtBx.Text == String.Empty) && !(CmpyAddress2TxtBx.Text == String.Empty))
                                            {
                                                supp.CompanyAddress = CmpyAddress1TxtBx.Text + ", " + CmpyAddress2TxtBx.Text;
                                            }
                                            else
                                            {
                                                supp.CompanyAddress = CmpyAddress1TxtBx.Text;
                                            }

                                            supp.CompanyNum = CmpyNumberTxtBx.Text;
                                            supp.CompanyFaxNum = CmpyFaxNumberTxtBx.Text;
                                            supp.SuppStatus = SuppStatusCbBx.SelectedItem.ToString();

                                            suppDA.SupplierInsertRecord(supp);

                                            if(supp.InsertStatus == "Success")
                                            {
                                                MessageBox.Show("New supplier record has inserted successfully!");

                                                SuppNameTxtBx.Clear();
                                                SuppMaleRdBtn.Checked = false;
                                                SuppFemaleRdBtn.Checked = false;
                                                SuppHandphoneTxtBx.Clear();
                                                SuppEmailTxtBx.Clear();
                                                CmpyNameTxtBx.Clear();
                                                CmpyAddress1TxtBx.Clear();
                                                CmpyAddress2TxtBx.Clear();
                                                CmpyNumberTxtBx.Clear();
                                                CmpyFaxNumberTxtBx.Clear();
                                                SuppStatusCbBx.SelectedIndex = 0;

                                                SuppErrLbl.Visible = false;
                                            }
                                            else
                                            {
                                                MessageBox.Show("Failed to insert supplier record!");
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

        private void SupplierDataFill()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "SELECT SupplierID, SupplierName, Gender, HandphoneNumber, Email, CompanyName, CompanyAddress, CompanyNumber, CompanyFaxNumber, SuppStatus FROM Supplier";

                SqlDataAdapter da = new SqlDataAdapter(sql, con);

                DataSet ds = new DataSet();

                con.Open();

                da.Fill(ds, "Supplier");

                con.Close();

                SuppDtGdVw.DataSource = ds;
                SuppDtGdVw.DataMember = "Supplier";
            }
            catch(Exception ex)
            {
                MessageBox.Show("Supplier's data grid view cannot read the database!");
                throw ex;
            }
        }

        private void SuppRefreshBtn_Click(object sender, EventArgs e)
        {
            SupplierDataFill();
        }

        private void SuppDtGdVw_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SuppInsertBtn.Enabled = false;
            SuppUpdateBtn.Enabled = true;

            int i;
            i = SuppDtGdVw.SelectedCells[0].RowIndex;

            SuppIDTxtBx.Text = SuppDtGdVw.Rows[i].Cells[0].Value.ToString();
            SuppNameTxtBx.Text = SuppDtGdVw.Rows[i].Cells[1].Value.ToString();

            string g = SuppDtGdVw.Rows[i].Cells[2].Value.ToString();

            if(g.Equals("Male"))
            {
                SuppMaleRdBtn.Checked = true;
            }
            else
            {
                SuppFemaleRdBtn.Checked = true;
            }

            SuppHandphoneTxtBx.Text = SuppDtGdVw.Rows[i].Cells[3].Value.ToString();
            SuppEmailTxtBx.Text = SuppDtGdVw.Rows[i].Cells[4].Value.ToString();
            CmpyNameTxtBx.Text = SuppDtGdVw.Rows[i].Cells[5].Value.ToString();
            CmpyAddress1TxtBx.Text = SuppDtGdVw.Rows[i].Cells[6].Value.ToString();
            CmpyNumberTxtBx.Text = SuppDtGdVw.Rows[i].Cells[7].Value.ToString();
            CmpyFaxNumberTxtBx.Text = SuppDtGdVw.Rows[i].Cells[8].Value.ToString();

            string s = SuppDtGdVw.Rows[i].Cells[9].Value.ToString();

            if (s.Equals("Supply"))
            {
                SuppStatusCbBx.SelectedIndex = 1;
            }
            else if(s.Equals("No Supply"))
            {
                SuppStatusCbBx.SelectedIndex = 2;
            }
        }

        private void SuppUpdateBtn_Click(object sender, EventArgs e)
        {
            String p1 = @"^[a-z\s/]*$";
            String p2 = @"^\d*$";
            String p3 = @"[1-9]{1}[0-9]{2}-[0-9]{7}";
            String p4 = @"[0]{3}-[0-9]{7}";
            String p5 = @"[0-9]{3}-[0]{7}";
            String p6 = @"^\d{3}-\d{7}$";
            String p7 = @"^[a-z0-9_.\-]+\@[a-z]+\.(?:[a-z]{3}|com|org|net|edu|gov)|\.(?:[a-z]{2}|my)$";
            String p8 = @"^[a-z0-9\s.,/-]*$";
            String p9 = @"^[0-9\s.,/-]*$";
            String p10 = @"[1-9]{1}[0-9]{1}-[0-9]{8}";
            String p11 = @"[0]{1}[0]{1}-[0-9]{8}";
            String p12 = @"[0]{1}[125]{1}";
            String p13 = @"[0-9]{2}-[0]{8}";
            String p14 = @"^\d{2}-\d{8}$";

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
            Regex rgx11 = new Regex(p11);
            Regex rgx12 = new Regex(p12);
            Regex rgx13 = new Regex(p13);
            Regex rgx14 = new Regex(p14);

            if (SuppNameTxtBx.Text == String.Empty || SuppMaleRdBtn.Checked == false && SuppFemaleRdBtn.Checked == false || SuppHandphoneTxtBx.Text == String.Empty || SuppEmailTxtBx.Text == String.Empty || CmpyNameTxtBx.Text == String.Empty || CmpyAddress1TxtBx.Text == String.Empty && CmpyAddress2TxtBx.Text == String.Empty || CmpyNumberTxtBx.Text == String.Empty || CmpyFaxNumberTxtBx.Text == String.Empty || SuppStatusCbBx.SelectedIndex == 0)
            {
                SuppErrLbl.Visible = true;
                SuppErrLbl.Text = "*Please make sure all the fields are completed.";
            }
            else
            {
                if (!rgx1.Match(SuppNameTxtBx.Text).Success)
                {
                    SuppErrLbl.Visible = true;
                    SuppErrLbl.Text = "*Please make sure Supplier Name does not contains any numeric data. \r\n\r\n*Please make sure Supplier Name does not contains any special symbol, except '/'.";
                    SuppNameTxtBx.Focus();
                }
                else if (SuppNameTxtBx.TextLength > 200 || SuppNameTxtBx.TextLength < 10)
                {
                    SuppErrLbl.Visible = true;
                    SuppErrLbl.Text = "*Please make sure Supplier Name does not exceeds more than 200 characters. \r\n\r\n*Please make sure Supplier Name has minimum 10 characters.";
                    SuppNameTxtBx.Focus();
                }
                else
                {
                    if (rgx2.Match(SuppHandphoneTxtBx.Text).Success)
                    {
                        SuppErrLbl.Visible = true;
                        SuppErrLbl.Text = "*Handphone Number is missing dashes '-'. \r\n\r\n*Please make sure Handphone Number is in such format 'xxx-xxxxxxx'.";
                        SuppHandphoneTxtBx.Focus();
                    }
                    else if (rgx3.Match(SuppHandphoneTxtBx.Text).Success)
                    {
                        SuppErrLbl.Visible = true;
                        SuppErrLbl.Text = "*Invalid Handphone Number!  Please make sure the first digit is zero.";
                        SuppHandphoneTxtBx.Focus();
                    }
                    else if (rgx4.Match(SuppHandphoneTxtBx.Text).Success)
                    {
                        SuppErrLbl.Visible = true;
                        SuppErrLbl.Text = "*Invalid Handphone Number!";
                        SuppHandphoneTxtBx.Focus();
                    }
                    else if (rgx5.Match(SuppHandphoneTxtBx.Text).Success)
                    {
                        SuppErrLbl.Visible = true;
                        SuppErrLbl.Text = "*Invalid Handphone Number!";
                        SuppHandphoneTxtBx.Focus();
                    }
                    else if (!rgx6.Match(SuppHandphoneTxtBx.Text).Success)
                    {
                        SuppErrLbl.Visible = true;
                        SuppErrLbl.Text = "*Please make sure Handphone Number does not contains any alphabetics and any special symbols, except '-'. \r\n\r\n*Please make sure Handphone Number has exactly 11 characters. \r\n\r\n*Please make sure Handphone Number is in such format 'xxx-xxxxxxx'.";
                        SuppHandphoneTxtBx.Focus();
                    }
                    else
                    {
                        if (!rgx7.Match(SuppEmailTxtBx.Text).Success)
                        {
                            SuppErrLbl.Visible = true;
                            SuppErrLbl.Text = "*Invalid Email Address!";
                            SuppEmailTxtBx.Focus();
                        }
                        else if (SuppEmailTxtBx.TextLength > 200 || SuppEmailTxtBx.TextLength < 10)
                        {
                            SuppErrLbl.Visible = true;
                            SuppErrLbl.Text = "*Please make sure Email address does not exceeds more than 200 characters. \r\n\r\n*Please make sure Email address has minimum 10 characters.";
                            SuppEmailTxtBx.Focus();
                        }
                        else
                        {
                            if (!rgx8.Match(CmpyAddress1TxtBx.Text).Success)
                            {
                                SuppErrLbl.Visible = true;
                                SuppErrLbl.Text = "*Please make sure first Address textfield does not contains any special symbols, except ','  '.'  '/'  '-'.";
                                CmpyAddress1TxtBx.Focus();
                            }
                            else if (rgx9.Match(CmpyAddress1TxtBx.Text).Success)
                            {
                                SuppErrLbl.Visible = true;
                                SuppErrLbl.Text = "*Invalid Address!";
                                CmpyAddress1TxtBx.Focus();
                            }
                            else if (CmpyAddress1TxtBx.TextLength > 170 || CmpyAddress1TxtBx.TextLength < 20)
                            {
                                SuppErrLbl.Visible = true;
                                SuppErrLbl.Text = "*Please make sure first Address textfield does not exceeds more than 170 characters. \r\n\r\n*Please make sure first Address textfield has minimum 20 characters.";
                                CmpyAddress1TxtBx.Focus();
                            }
                            else
                            {
                                if (!(CmpyAddress2TxtBx.Text == String.Empty) && !rgx8.Match(CmpyAddress2TxtBx.Text).Success)
                                {
                                    SuppErrLbl.Visible = true;
                                    SuppErrLbl.Text = "*Please make sure second Address textfield does not contains any special symbols, except ','  '.'  '/'  '-'.";
                                    CmpyAddress2TxtBx.Focus();
                                }
                                else if (!(CmpyAddress2TxtBx.Text == String.Empty) && rgx9.Match(CmpyAddress2TxtBx.Text).Success)
                                {
                                    SuppErrLbl.Visible = true;
                                    SuppErrLbl.Text = "*Invalid Address!";
                                    CmpyAddress2TxtBx.Focus();
                                }
                                else if (!(CmpyAddress2TxtBx.Text == String.Empty) && CmpyAddress2TxtBx.TextLength > 30)
                                {
                                    SuppErrLbl.Visible = true;
                                    SuppErrLbl.Text = "*Please make sure second Address textfield does not exceeds more than 30 characters.";
                                    CmpyAddress2TxtBx.Focus();
                                }
                                else
                                {
                                    if (rgx2.Match(CmpyNumberTxtBx.Text).Success)
                                    {
                                        SuppErrLbl.Visible = true;
                                        SuppErrLbl.Text = "*Home Number is missing dashes '-'. \r\n\r\n*Please make sure Home Number is in such format 'xx-xxxxxxxx'.";
                                        CmpyNumberTxtBx.Focus();
                                    }
                                    else if (rgx10.Match(CmpyNumberTxtBx.Text).Success)
                                    {
                                        SuppErrLbl.Visible = true;
                                        SuppErrLbl.Text = "*Invalid Home Number!  Please make sure the first digit is zero.";
                                        CmpyNumberTxtBx.Focus();
                                    }
                                    else if (rgx11.Match(CmpyNumberTxtBx.Text).Success)
                                    {
                                        SuppErrLbl.Visible = true;
                                        SuppErrLbl.Text = "*Invalid Home Number!  Please make sure the second digit is not zero.";
                                        CmpyNumberTxtBx.Focus();
                                    }
                                    else if (rgx12.Match(CmpyNumberTxtBx.Text).Success)
                                    {
                                        SuppErrLbl.Visible = true;
                                        SuppErrLbl.Text = "*Invalid Home Number!  Malaysia city codes only contain 03, 04, 06, 07, 08, 09.";
                                        CmpyNumberTxtBx.Focus();
                                    }
                                    else if (rgx13.Match(CmpyNumberTxtBx.Text).Success)
                                    {
                                        SuppErrLbl.Visible = true;
                                        SuppErrLbl.Text = "*Invalid Home Number!";
                                        CmpyNumberTxtBx.Focus();
                                    }
                                    else if (!rgx14.Match(CmpyNumberTxtBx.Text).Success)
                                    {
                                        SuppErrLbl.Visible = true;
                                        SuppErrLbl.Text = "*Please make sure Home Number does not contains any alphabetics. \r\n\r\n*Please make sure Home Number does not contains any special symbols, except '-'. \r\n\r\n*Please make sure Home Number has exactly 11 characters.";
                                        CmpyNumberTxtBx.Focus();
                                    }
                                    else
                                    {
                                        if (rgx2.Match(CmpyFaxNumberTxtBx.Text).Success)
                                        {
                                            SuppErrLbl.Visible = true;
                                            SuppErrLbl.Text = "*Fax Number is missing dashes '-'. \r\n\r\n*Please make sure Fax Number is in such format 'xx-xxxxxxxx'.";
                                            CmpyFaxNumberTxtBx.Focus();
                                        }
                                        else if (rgx10.Match(CmpyFaxNumberTxtBx.Text).Success)
                                        {
                                            SuppErrLbl.Visible = true;
                                            SuppErrLbl.Text = "*Invalid Fax Number!  Please make sure the first digit is zero.";
                                            CmpyFaxNumberTxtBx.Focus();
                                        }
                                        else if (rgx11.Match(CmpyFaxNumberTxtBx.Text).Success)
                                        {
                                            SuppErrLbl.Visible = true;
                                            SuppErrLbl.Text = "*Invalid Fax Number!  Please make sure the second digit is not zero.";
                                            CmpyFaxNumberTxtBx.Focus();
                                        }
                                        else if (rgx12.Match(CmpyFaxNumberTxtBx.Text).Success)
                                        {
                                            SuppErrLbl.Visible = true;
                                            SuppErrLbl.Text = "*Invalid Fax Number!  Malaysia city codes only contain 03, 04, 06, 07, 08, 09.";
                                            CmpyFaxNumberTxtBx.Focus();
                                        }
                                        else if (rgx13.Match(CmpyFaxNumberTxtBx.Text).Success)
                                        {
                                            SuppErrLbl.Visible = true;
                                            SuppErrLbl.Text = "*Invalid Fax Number!";
                                            CmpyFaxNumberTxtBx.Focus();
                                        }
                                        else if (!rgx14.Match(CmpyFaxNumberTxtBx.Text).Success)
                                        {
                                            SuppErrLbl.Visible = true;
                                            SuppErrLbl.Text = "*Please make sure Fax Number does not contains any alphabetics. \r\n\r\n*Please make sure Fax Number does not contains any special symbols, except '-'. \r\n\r\n*Please make sure Fax Number has exactly 11 characters.";
                                            CmpyFaxNumberTxtBx.Focus();
                                        }
                                        else
                                        {
                                            Supplier supp = new Supplier();
                                            SupplierDA suppDA = new SupplierDA();

                                            supp.SupplierID = SuppIDTxtBx.Text;
                                            supp.SupplierName = SuppNameTxtBx.Text;

                                            if (SuppMaleRdBtn.Checked == true)
                                            {
                                                supp.SupplierGender = SuppMaleRdBtn.Text;
                                            }
                                            else
                                            {
                                                supp.SupplierGender = SuppFemaleRdBtn.Text;
                                            }

                                            supp.SupplierHandphoneNum = SuppHandphoneTxtBx.Text;
                                            supp.SupplierEmail = SuppEmailTxtBx.Text;
                                            supp.CompanyName = CmpyNameTxtBx.Text;

                                            if (!(CmpyAddress1TxtBx.Text == String.Empty) && !(CmpyAddress2TxtBx.Text == String.Empty))
                                            {
                                                supp.CompanyAddress = CmpyAddress1TxtBx.Text + ", " + CmpyAddress2TxtBx.Text;
                                            }
                                            else
                                            {
                                                supp.CompanyAddress = CmpyAddress1TxtBx.Text;
                                            }

                                            supp.CompanyNum = CmpyNumberTxtBx.Text;
                                            supp.CompanyFaxNum = CmpyFaxNumberTxtBx.Text;
                                            supp.SuppStatus = SuppStatusCbBx.SelectedItem.ToString();

                                            suppDA.SupplierUpdateRecord(supp);

                                            if (supp.UpdateStatus == "Success")
                                            {
                                                MessageBox.Show("New supplier record has updated successfully!");

                                                SuppIDTxtBx.Text = "Auto-Generated";
                                                SuppNameTxtBx.Clear();
                                                SuppMaleRdBtn.Checked = false;
                                                SuppFemaleRdBtn.Checked = false;
                                                SuppHandphoneTxtBx.Clear();
                                                SuppEmailTxtBx.Clear();
                                                CmpyNameTxtBx.Clear();
                                                CmpyAddress1TxtBx.Clear();
                                                CmpyAddress2TxtBx.Clear();
                                                CmpyNumberTxtBx.Clear();
                                                CmpyFaxNumberTxtBx.Clear();
                                                SuppStatusCbBx.SelectedIndex = 0;

                                                SuppInsertBtn.Enabled = true;
                                                SuppErrLbl.Visible = false;
                                            }
                                            else if (supp.UpdateStatus == "Failed")
                                            {
                                                MessageBox.Show("Failed to update supplier record!");
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

        private void SuppResetBtn_Click(object sender, EventArgs e)
        {
            SuppIDTxtBx.Text = "Auto-Generated";
            SuppNameTxtBx.Clear();
            SuppMaleRdBtn.Checked = false;
            SuppFemaleRdBtn.Checked = false;
            SuppHandphoneTxtBx.Clear();
            SuppEmailTxtBx.Clear();
            CmpyNameTxtBx.Clear();
            CmpyAddress1TxtBx.Clear();
            CmpyAddress2TxtBx.Clear();
            CmpyNumberTxtBx.Clear();
            CmpyFaxNumberTxtBx.Clear();
            SuppStatusCbBx.SelectedIndex = 0;

            SuppInsertBtn.Enabled = true;
            SuppUpdateBtn.Enabled = false;
            SuppErrLbl.Visible = false;
        }

        private void SuppNmTxtBx_TextChanged(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "SELECT * FROM Supplier WHERE SupplierName like '" + SuppNmTxtBx.Text + "%'";

                SqlDataAdapter da = new SqlDataAdapter(sql, con);

                DataSet ds = new DataSet();

                con.Open();

                da.Fill(ds, "Supplier");        //Supplier is the table name

                con.Close();

                SuppDtGdVw.DataSource = ds;
                SuppDtGdVw.DataMember = "Supplier";
            }
            catch(SqlException ex)
            {
                MessageBox.Show("Can't find the Supplier Name!");
                throw ex;
            }
        }

        private void ItmInsertBtn_Click(object sender, EventArgs e)
        {
            String p1 = @"^\d*$";
            Regex rgx1 = new Regex(p1);

            if(IngNameTxtBx.Text == String.Empty || IngDescTxtBx.Text == String.Empty || StorageAreaCbBx.SelectedIndex == 0 || UnitCbBx.SelectedIndex == 0 || ReOrderLevelCbBx.SelectedIndex == 0 || ReOrderQtyTxtBx.Text == String.Empty || IngStatusCbBx.SelectedIndex == 0)
            {
                IngErrLbl.Visible = true;
                IngErrLbl.Text = "*Please make sure all the fields are completed.";
            }
            else
            {
                if(IngNameTxtBx.TextLength > 200)
                {
                    IngErrLbl.Visible = true;
                    IngErrLbl.Text = "*Please make sure Ingredient Name does not more than 200 characters.";
                }
                else
                {
                    if(IngDescTxtBx.TextLength > 200)
                    {
                        IngErrLbl.Visible = true;
                        IngErrLbl.Text = "*Please make sure Ingredient Desc does not more than 200 characters.";
                    }
                    else
                    {
                            if (!rgx1.Match(ReOrderQtyTxtBx.Text).Success)
                            {
                                IngErrLbl.Visible = true;
                                IngErrLbl.Text = "*Please make sure the Re-Order Quantity is an integer value.";
                            }
                            else {
                                Ingredient ing = new Ingredient();
                                IngredientDA ingDA = new IngredientDA();

                                ing.IngredientName = IngNameTxtBx.Text;
                                ing.IngredientDesc = IngDescTxtBx.Text;
                                ing.IngredientQty = int.Parse(IngQuantityTxtBx.Text);
                                ing.Unit = UnitCbBx.SelectedItem.ToString();
                                ing.StorageArea = StorageAreaCbBx.SelectedItem.ToString();
                                //ing.ExpiryDate = ExpiryDatePicker.Value.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                                //ing.ExpiryDate = ExpiryDatePicker.Value.ToShortDateString();
                                ing.ReOrderLevel = int.Parse(ReOrderLevelCbBx.SelectedItem.ToString());
                                ing.ReOrderQty = int.Parse(ReOrderQtyTxtBx.Text);
                                ing.IngredientStatus = IngStatusCbBx.SelectedItem.ToString();

                                ingDA.InsertIngredientRecord(ing);

                                if (ing.InsertStatus == "Success")
                                {
                                    MessageBox.Show("New ingredient record has inserted successfully!");

                                    IngNameTxtBx.Clear();
                                    IngDescTxtBx.Clear();
                                    UnitCbBx.SelectedIndex = 0;
                                    StorageAreaCbBx.SelectedIndex = 0;
                                    //ExpiryDatePicker.Value = DateTime.Now;
                                    ReOrderLevelCbBx.SelectedIndex = 0;
                                    ReOrderQtyTxtBx.Clear();
                                    IngStatusCbBx.SelectedIndex = 0;

                                    IngErrLbl.Visible = false;
                                }
                                else if (ing.InsertStatus == "Failed")
                                {
                                    MessageBox.Show("Failed to insert ingredient record!");
                                }
                        }
                    }
                }
            }
        }

        private void IngredientDataFill()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "SELECT IngredientID, IngredientName, IngredientDesc, Quantity, Unit, StorageArea, ReOrderLevel, ReOrderQuantity, IngredientStatus FROM Ingredient";

                SqlDataAdapter da = new SqlDataAdapter(sql, con);

                DataSet ds = new DataSet();

                con.Open();

                da.Fill(ds, "Ingredient");

                IngDtGdVw.DataSource = ds;
                IngDtGdVw.DataMember = "Ingredient";
            }
            catch(SqlException ex)
            {
                MessageBox.Show("Ingredient's data grid view cannot read the database!");
                throw ex;
            }
            con.Close();
        }

        private void ItmResetBtn_Click(object sender, EventArgs e)
        {
            IngIDTxtBx.Text = "Auto-Generated";
            IngNameTxtBx.Clear();
            IngDescTxtBx.Clear();
            IngQuantityTxtBx.Text = "0";
            UnitCbBx.SelectedIndex = 0;
            StorageAreaCbBx.SelectedIndex = 0;
            ReOrderLevelCbBx.SelectedIndex = 0;
            ReOrderQtyTxtBx.Clear();
            IngStatusCbBx.SelectedIndex = 0;

            IngInsertBtn.Enabled = true;
            IngUpdateBtn.Enabled = false;
            IngErrLbl.Visible = false;
        }

        private void IngDtGdVw_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            IngInsertBtn.Enabled = false;
            IngUpdateBtn.Enabled = true;

            int i;
            i = IngDtGdVw.SelectedCells[0].RowIndex;

            IngIDTxtBx.Text = IngDtGdVw.Rows[i].Cells[0].Value.ToString();
            IngNameTxtBx.Text = IngDtGdVw.Rows[i].Cells[1].Value.ToString();
            IngDescTxtBx.Text = IngDtGdVw.Rows[i].Cells[2].Value.ToString();
            IngQuantityTxtBx.Text = IngDtGdVw.Rows[i].Cells[3].Value.ToString();

            string u = IngDtGdVw.Rows[i].Cells[4].Value.ToString();

            if(u.Equals("kg"))
            {
                UnitCbBx.SelectedIndex = 1;
            }
            else if(u.Equals("litre"))
            {
                UnitCbBx.SelectedIndex = 2;
            }
            else if(u.Equals("pack"))
            {
                UnitCbBx.SelectedIndex = 3;
            }
            else if(u.Equals("piece"))
            {
                UnitCbBx.SelectedIndex = 4;
            }

            string sa = IngDtGdVw.Rows[i].Cells[5].Value.ToString();

            if (sa.Equals("Dry Place"))
            {
                StorageAreaCbBx.SelectedIndex = 1;
            }
            else if (sa.Equals("Refrigeration"))
            {
                StorageAreaCbBx.SelectedIndex = 2;
            }
            else if (sa.Equals("Freeze"))
            {
                StorageAreaCbBx.SelectedIndex = 3;
            }

            string rol = IngDtGdVw.Rows[i].Cells[6].Value.ToString();

            if (rol.Equals("5"))
            {
                ReOrderLevelCbBx.SelectedIndex = 1;
            }
            else if (rol.Equals("10"))
            {
                ReOrderLevelCbBx.SelectedIndex = 2;
            }
            else if (rol.Equals("15"))
            {
                ReOrderLevelCbBx.SelectedIndex = 3;
            }
            else if (rol.Equals("20"))
            {
                ReOrderLevelCbBx.SelectedIndex = 4;
            }
            else if(rol.Equals("25"))
            {
                ReOrderLevelCbBx.SelectedIndex = 5;
            }

            ReOrderQtyTxtBx.Text = IngDtGdVw.Rows[i].Cells[7].Value.ToString();

            string s = IngDtGdVw.Rows[i].Cells[8].Value.ToString();

            if (s.Equals("Available"))
            {
                IngStatusCbBx.SelectedIndex = 1;
            }
            else if (s.Equals("Not Available"))
            {
                IngStatusCbBx.SelectedIndex = 2;
            }
        }

        private void IngRefreshBtn_Click(object sender, EventArgs e)
        {
            IngredientDataFill();
        }

        private void IngUpdateBtn_Click(object sender, EventArgs e)
        {
            if (IngNameTxtBx.Text == String.Empty || IngDescTxtBx.Text == String.Empty || StorageAreaCbBx.SelectedIndex == 0 || UnitCbBx.SelectedIndex == 0 || ReOrderLevelCbBx.SelectedIndex == 0 || ReOrderQtyTxtBx.Text == String.Empty || IngStatusCbBx.SelectedIndex == 0)
            {
                IngErrLbl.Visible = true;
                IngErrLbl.Text = "*Please make sure all the fields are completed.";
            }
            else
            {
                if (IngNameTxtBx.TextLength > 200)
                {
                    IngErrLbl.Visible = true;
                    IngErrLbl.Text = "*Please make sure Ingredient Name does not more than 200 characters.";
                }
                else
                {
                    if (IngDescTxtBx.TextLength > 200)
                    {
                        IngErrLbl.Visible = true;
                        IngErrLbl.Text = "*Please make sure Ingredient Desc does not more than 200 characters.";
                    }
                    else
                    {
                            Ingredient ing = new Ingredient();
                            IngredientDA ingDA = new IngredientDA();

                            ing.IngredientID = IngIDTxtBx.Text;
                            ing.IngredientName = IngNameTxtBx.Text;
                            ing.IngredientDesc = IngDescTxtBx.Text;
                            ing.Unit = UnitCbBx.SelectedItem.ToString();
                            ing.StorageArea = StorageAreaCbBx.SelectedItem.ToString();
                            //ing.ExpiryDate = ExpiryDatePicker.Value.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                            //ing.ExpiryDate = ExpiryDatePicker.Value.ToShortDateString();
                            ing.ReOrderLevel = int.Parse(ReOrderLevelCbBx.SelectedItem.ToString());
                            ing.ReOrderQty = int.Parse(ReOrderQtyTxtBx.Text);
                            ing.IngredientStatus = IngStatusCbBx.SelectedItem.ToString();

                            ingDA.UpdateIngredientRecord(ing);

                            if (ing.UpdateStatus == "Success")
                             {
                                MessageBox.Show("Ingredient record has updated successfully!");

                                IngIDTxtBx.Text = "Auto-Generated";
                                IngNameTxtBx.Clear();
                                IngDescTxtBx.Clear();
                                IngQuantityTxtBx.Text = "0";
                                UnitCbBx.SelectedIndex = 0;
                                StorageAreaCbBx.SelectedIndex = 0;
                                //ExpiryDatePicker.Value = System.DateTime.Now;
                                ReOrderLevelCbBx.SelectedIndex = 0;
                                ReOrderQtyTxtBx.Clear();
                                IngStatusCbBx.SelectedIndex = 0;

                                IngErrLbl.Visible = false;
                                IngInsertBtn.Enabled = true;
                             }
                             else if (ing.UpdateStatus == "Failed")
                             {
                                MessageBox.Show("Failed to update ingredient record!");
                             }
                        }
                    }
                }
            }

        private void IngredientNameTxtBx_TextChanged(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "SELECT * FROM Ingredient WHERE IngredientName like '"+IngredientNameTxtBx.Text+"%'";

                SqlDataAdapter da = new SqlDataAdapter(sql, con);

                DataSet ds = new DataSet();

                con.Open();

                da.Fill(ds, "Ingredient");

                con.Close();

                IngDtGdVw.DataSource = ds;
                IngDtGdVw.DataMember = "Ingredient";
            }
            catch(SqlException ex)
            {
                MessageBox.Show("Can't find the Ingredient Name!");
                throw ex;
            }
        }

        private void IngredientStatusCbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string ingredientStatus = IngredientStatusCbBx.SelectedItem.ToString();

                string sql = "SELECT * FROM Ingredient WHERE IngredientStatus like '"+ingredientStatus+"%'";

                SqlDataAdapter da = new SqlDataAdapter(sql, con);

                DataSet ds = new DataSet();

                con.Open();

                da.Fill(ds, "Ingredient");

                con.Close();

                IngDtGdVw.DataSource = ds;
                IngDtGdVw.DataMember = "Ingredient";
            }
            catch(SqlException ex)
            {
                throw ex;
            }
        }

        private void IngredientTimer_Tick(object sender, EventArgs e)
        {
            IngredientDA ingDA = new IngredientDA();
            //MessageBox.Show(ingDA.IngredientQtyCheck().ToString());

            if(ingDA.IngredientQtyCheck() == true)
            {
                IngredientNotifyIcn.Visible = true;
                IngredientNotifyIcn.BalloonTipTitle = "Not Enough Ingredient!";
                IngredientNotifyIcn.BalloonTipText = "Please click below The Coffee Bean icon to get more details.";
                IngredientNotifyIcn.ShowBalloonTip(100);
                IngredientTimer.Interval = 120000;
                //IngredientTimer.Stop();
            }
        }

        private void IngredientNotifyIcn_Click(object sender, EventArgs e)
        {
            pnlItem.Visible = false;
            pnlMain.Visible = false;
            pnlMenu.Visible = false;
            pnlPwChg.Visible = false;
            pnlSupplier.Visible = false;
            pnlEmp.Visible = false;
            pnlPO.Visible = true;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            pnlEmp.Visible = false;
            pnlItem.Visible = false;
            pnlMain.Visible = false;
            pnlMenu.Visible = false;
            pnlPwChg.Visible = false;
            pnlSupplier.Visible = false;
            pnlPO.Visible = true;
        }

        private void IngPODataFill()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "SELECT IngredientID, IngredientName, Quantity, Unit, ReOrderLevel, ReOrderQuantity FROM Ingredient WHERE Quantity < ReOrderLevel AND IngredientStatus = 'Available';";

                SqlDataAdapter da = new SqlDataAdapter(sql, con);

                DataSet ds = new DataSet();

                con.Open();

                da.Fill(ds, "Ingredient");
                con.Close();

                IngPODtGrdVw.DataSource = ds;
                IngPODtGrdVw.DataMember = "Ingredient";
            }
            catch(SqlException ex)
            {
                MessageBox.Show("Ingredient Purchase Order's data grid view cannot read the database!");
                throw ex;
            }
        }

        private void IngPORefreshBtn_Click(object sender, EventArgs e)
        {
            IngPODataFill();
        }

        private void POSupplierDetailsDataFill()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "SELECT SupplierID, SupplierName, CompanyName FROM Supplier WHERE SuppStatus = 'Supply';";

                SqlDataAdapter da = new SqlDataAdapter(sql, con);

                DataSet ds = new DataSet();

                con.Open();

                da.Fill(ds, "Supplier");
                con.Close();

                POSupplierDtGrdVw.DataSource = ds;
                POSupplierDtGrdVw.DataMember = "Supplier";

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Supplier Purchase Order's data grid view cannot read the database!");
                throw ex;
            }
        }

        private void POSupplierDtGrdVw_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = POSupplierDtGrdVw.SelectedCells[0].RowIndex;

            POSupplierIDTxtBx.Text = POSupplierDtGrdVw.Rows[i].Cells[0].Value.ToString();
            POSupplierNameTxtBx.Text = POSupplierDtGrdVw.Rows[i].Cells[1].Value.ToString();
            POCompanyNameTxtBx.Text = POSupplierDtGrdVw.Rows[i].Cells[2].Value.ToString();
        }

        private void POResetBtn_Click(object sender, EventArgs e)
        {
            POSupplierIDTxtBx.Clear();
            POSupplierNameTxtBx.Clear();
            POCompanyNameTxtBx.Clear();
        }

        private void POSuppRefreshBtn_Click(object sender, EventArgs e)
        {
            POSupplierDetailsDataFill();
        }

        private void IngPODtGrdVw_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            IngPORefreshBtn.Enabled = false;

            if (e.ColumnIndex == IngPOCheckBx.Index)
            {
                var row = IngPODtGrdVw.Rows[e.RowIndex];
                string ingID = row.Cells["IngredientID"].Value.ToString();
                string ingName = row.Cells["IngredientName"].Value.ToString();
                string ingQty = row.Cells["Quantity"].Value.ToString();
                string unt = row.Cells["Unit"].Value.ToString();
                string reorderlvl = row.Cells["ReOrderLevel"].Value.ToString();
                string reorderqty = row.Cells["ReOrderQuantity"].Value.ToString();

                IngPODtGrdVw2.Rows.Add(true, ingID, ingName, ingQty, unt, reorderlvl, reorderqty);
                IngPODtGrdVw.Rows.Remove(row);
            } 
        }

        private void IngPODtGrdVw2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            IngPORefreshBtn.Enabled = true;

            if(e.ColumnIndex == IngPOCheckBx2.Index)
            {
                var row = IngPODtGrdVw2.Rows[e.RowIndex];

                IngPODtGrdVw2.Rows.Remove(row);
            }
        }

        private void POInsertBtn_Click_1(object sender, EventArgs e)
        {
            PurchaseOrder po = new PurchaseOrder();
            PurchaseOrderDA poDA = new PurchaseOrderDA();

            if (POSupplierIDTxtBx.Text == String.Empty || POSupplierNameTxtBx.Text == String.Empty || POCompanyNameTxtBx.Text == String.Empty)
            {
                IngPOErrLbl.Visible = true;
                IngPOErrLbl.Text = "*Please make sure all the fields are completed.";
            }
            else
            {
                    po.POEmployeeID = POEmpIDTxtBx.Text;
                    po.POOrderDate = OrderDateTxtBx.Text;
                    po.POOrderTime = OrderTimeTxtBx.Text;
                    po.POSupplierID = POSupplierIDTxtBx.Text;

                    poDA.InsertPurchaseOrderRecord(po);

                    IngPODtGrdVw.Enabled = true;
                    IngPODtGrdVw2.Enabled = true;

                    PurchaseOrderDA p = new PurchaseOrderDA();
                    string pp = p.POIDRetrieve(po);
                    POIDTxtBx.Text = pp;
                }

            if (po.InsertStatus.Equals("Success"))
            {
                IngPOErrLbl.Visible = false;
                MessageBox.Show("New purchase order record has inserted successfully!\n\nNext, please select your ordered ingredients.");
            }
            else
            {
                MessageBox.Show("Failed to insert purchase order record!");
            }
        }

        private void POConfirmBtn_Click(object sender, EventArgs e)
        {
            PurchaseOrderDetails pod = new PurchaseOrderDetails();
            PurchaseOrderDetailsDA podDA = new PurchaseOrderDetailsDA();

            List<PurchaseOrderDetails> podList = new List<PurchaseOrderDetails>();

            if (!(ExpiryDateTimePicker.Value > System.DateTime.Today))
            {
                IngPOErrLbl.Visible = true;
                IngPOErrLbl.Text = "*Invalid date! \r\n\r\n*Please make sure the Expiry Date is greater than or equals to today date.";
            }
            else
            {
                if (IngPODtGrdVw2.RowCount == 0)
                {
                    IngPOErrLbl.Visible = true;
                    IngPOErrLbl.Text = "*Please select your ordered ingredients.";
                }
                else
                {
                    for (int i = 0; i <IngPODtGrdVw2.RowCount; i++)
                    {
                        pod.PurchaseOrderID = POIDTxtBx.Text;
                        pod.IngredientID = IngPODtGrdVw2.Rows[i].Cells["IngredientID2"].Value.ToString();
                     
                        pod.PurchaseQuantity = int.Parse(IngPODtGrdVw2.Rows[i].Cells["ReOrderQuantity2"].Value.ToString());

                        pod.ExpiryDate = ExpiryDateTimePicker.Value.ToShortDateString();

                        podList.Add(pod);
                        podDA.InsertPurchaseOrderDetails(podList);
                        podDA.IngQuantityIncrease(podList);
                    }

                    POIDTxtBx.Text = "Auto-Generated";
                    POSupplierIDTxtBx.Clear();
                    POSupplierNameTxtBx.Clear();
                    POCompanyNameTxtBx.Clear();
                    ExpiryDateTimePicker.Value = System.DateTime.Now;
                    IngPOErrLbl.Visible = false;

                    List<DataGridViewRow> toDeleteList = new List<DataGridViewRow>();

                    foreach (DataGridViewRow row in IngPODtGrdVw2.Rows)
                    {
                        bool s = Convert.ToBoolean(row.Cells[0].Value);

                        if (s == true)
                        {
                            toDeleteList.Add(row);
                        }
                    }

                    foreach (DataGridViewRow row in toDeleteList)
                    {
                        IngPODtGrdVw2.Rows.Remove(row);
                    }
                    IngPORefreshBtn.Enabled = true;
                }
            }
        }

        private void IngPONameSearchTxtBx_TextChanged(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "SELECT * FROM Ingredient WHERE IngredientName like '" + IngPONameSearchTxtBx.Text + "%' AND Quantity < ReOrderLevel AND IngredientStatus = 'Available';";

                SqlDataAdapter da = new SqlDataAdapter(sql, con);

                DataSet ds = new DataSet();

                con.Open();

                da.Fill(ds, "Ingredient");

                con.Close();

                IngPODtGrdVw.DataSource = ds;
                IngPODtGrdVw.DataMember = "Ingredient";
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Can't find the Ingredient Name!");
                throw ex;
            }
        }
    }    
}
