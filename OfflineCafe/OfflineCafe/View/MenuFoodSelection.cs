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
    public partial class MenuFoodSelection : Form
    {
        List<Food> fList = new List<Food>();
        public static List<Food> staticfList;
        public MenuFoodSelection()
        {
            InitializeComponent();
            //DISPAY menu data which status is available
            FoodDA fDA = new FoodDA();
            var adapter = fDA.SearchAvailable();

            var ds = this.cafeManagementDataSet.Food;
            ds.Clear();
            adapter.Fill(ds);

            //retrieve data
            dataGridView3.Refresh();
            fList = staticfList;
            for (int i=0; i < staticfList.Count; i++)
            {
              
                dataGridView3.Rows.Add();
                dataGridView3.Rows[i].Cells[0].Value = staticfList.ElementAt(i).foodID;
                dataGridView3.Rows[i].Cells[1].Value = staticfList.ElementAt(i).foodName;
                dataGridView3.Rows[i].Cells[2].Value = staticfList.ElementAt(i).foodType;
                dataGridView3.Rows[i].Cells[3].Value = staticfList.ElementAt(i).foodPrice;
                dataGridView3.Rows[i].Cells[4].Value = staticfList.ElementAt(i).qty;
                double quantity = Convert.ToDouble(dataGridView3.Rows[i].Cells[4].Value);
                double price2 = Convert.ToDouble(dataGridView3.Rows[i].Cells[3].Value);
                double totalP = quantity * price2;
                dataGridView3.Rows[i].Cells[5].Value = String.Format("{0:0.00}", totalP.ToString("0.00"));
            }
            


        }

        private void MenuFoodSelection_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'cafeManagementDataSet.Food' table. You can move, or remove it, as needed.
           // this.foodTableAdapter.Fill(this.cafeManagementDataSet.Food);

        }
        //DISPAY menu data which status is available and type
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == 0)
            {
                FoodDA fDA = new FoodDA();
                var adapter = fDA.SearchAvailable();

                var ds = this.cafeManagementDataSet.Food;
                ds.Clear();
                adapter.Fill(ds);
            }
            else
            {
                Food f = new Food();
                FoodDA fDA = new FoodDA();
                f.foodType = comboBox3.SelectedItem.ToString();
                var adapter = fDA.SearchAvailableType(f);


                var ds = this.cafeManagementDataSet.Food;
                ds.Clear();
                adapter.Fill(ds);
            }
        }
       //To add the menu food to dgv3 from dgv2
        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Boolean flag = true;
            string index = dataGridView2.CurrentRow.Index.ToString();
            String id;
            String name;
            String type;
            double price;

            id = dataGridView2.Rows[Convert.ToInt32(index)].Cells[0].Value.ToString();
            name = dataGridView2.Rows[Convert.ToInt32(index)].Cells[1].Value.ToString();
            type = dataGridView2.Rows[Convert.ToInt32(index)].Cells[2].Value.ToString();
            price = Convert.ToDouble(dataGridView2.Rows[Convert.ToInt32(index)].Cells[3].Value.ToString());
            Food f = new Food();

            f.foodID = id;
            f.foodName = name;
            f.foodType = type;
            f.foodPrice = price;
            f.qty = 1;
            int qty = 0;
            for (int i = 0; i < fList.Count; i++)
            {
                if (fList.ElementAt(i).foodID == id)
                {

                    qty = i;
                    flag = false;
                }
            }
            if (flag == true)
            {
                if (dataGridView3.RowCount <= 4)
                {
                    fList.Add(f);
                    dataGridView3.Refresh();
                    dataGridView3.Rows.Add();
                    for (int i = 0; i < fList.Count; i++)
                    {
                        dataGridView3.Rows[i].Cells["fID"].Value = fList.ElementAt(i).foodID;
                        dataGridView3.Rows[i].Cells["fName"].Value = fList.ElementAt(i).foodName;
                        dataGridView3.Rows[i].Cells["fType"].Value = fList.ElementAt(i).foodType;
                        dataGridView3.Rows[i].Cells["fPrice"].Value = String.Format("{0:0.00}", (fList.ElementAt(i).foodPrice).ToString("0.00"));
                        dataGridView3.Rows[i].Cells["qty"].Value = fList.ElementAt(i).qty;
                        double quantity = Convert.ToDouble(dataGridView3.Rows[i].Cells["qty"].Value);
                        double price2 = Convert.ToDouble(dataGridView3.Rows[i].Cells["fPrice"].Value);
                        //textBox1.Text = String.Format("{0:0.00}", price2.ToString("0.00"));
                        double totalP = quantity * price2;
                        dataGridView3.Rows[i].Cells["totalPrice"].Value = String.Format("{0:0.00}", totalP.ToString("0.00"));
                    }
                }
                else
                {
                    MessageBox.Show("It is over maximum Ala Carte Food to Menu Sets", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                if (fList.ElementAt(qty).qty > 5)
                {
                    MessageBox.Show("It is over quantity of Ala Carte Food to Menu Sets", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    int qty2 = Convert.ToInt32(dataGridView3.Rows[qty].Cells["qty"].Value);
                    //int qty3 = 1;
                    //int qty4 = qty3+qty2;
                    qty2 += 1;
                    //MessageBox.Show("This Food ID = " + id + " is already Selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    fList.ElementAt(qty).qty = qty2;
                    dataGridView3.Rows[qty].Cells["qty"].Value = qty2;
                }
                double quantity2 = Convert.ToDouble(dataGridView3.Rows[qty].Cells["qty"].Value);
                double price3 = Convert.ToDouble(dataGridView3.Rows[qty].Cells["fPrice"].Value);
                double totalP2 = quantity2 * price3;
                dataGridView3.Rows[qty].Cells["totalPrice"].Value = String.Format("{0:0.00}", totalP2.ToString("0.00"));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();            
        }

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView3.Rows.Remove(dataGridView3.CurrentRow);
            int i = dataGridView3.CurrentRow.Index;
            fList.RemoveAt(i);
        }

        private void button1_Click(object sender, EventArgs e)
        {

           Form1.staticfList = fList;
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Food f = new Food();
            FoodDA fDA = new FoodDA();
            f.foodName = textBox1.Text;
            var adapter = fDA.SearchAvailableName(f.foodName);

            var ds = this.cafeManagementDataSet.Food;
            ds.Clear();
            adapter.Fill(ds);
        }
    }
    
}
