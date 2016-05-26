using OfflineCafe.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineCafe.DataAccess
{
    class MenuDA
    {
        SqlConnection con = new SqlConnection();
        
        //Add Menu Food Data 
        public void InsertFunc(Menu m)
        {
            try
            {

                string sql = "insert into Menu values ('" + m.MenuID + "','" + m.MenuName + "','" + m.MenuDesc + "','" + m.MenuCategory + "','" + m.MenuStatus + "','" + m.MenuPrice + "','" + m.DiscPrice + "')";
                con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Add multiple ala carte food to menu sets
        public void InsertMenuItem(List<MenuFood> m)
        {
            try
            {
                for (int i = 0; i < m.Count; i++)
                {
                    string sql = "insert into MenuFood values ('" + m.ElementAt(i).menuID + "','" + m.ElementAt(i).foodID + "', '" + m.ElementAt(i).quantity + "')";
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
                    con.Open();
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Search menu food by menu name
        public SqlDataAdapter SearchbyName(String name)
        {
            try
            {
                string sql = "select * from Menu where MenuName LIKE '%" + name + "%'";
                con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
                con.Open();
                var command = new SqlCommand(sql, con);
                con.Close();
                var adapter = new SqlDataAdapter(command);
                return adapter;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Search menu food by menu category
        public SqlDataAdapter SearchbyCategory(String category)
        {
            try
            {
                string sql = "select * from Menu where MenuCategory LIKE '" + category + "'";
                con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
                con.Open();
                var command = new SqlCommand(sql, con);
                con.Close();
                var adapter = new SqlDataAdapter(command);
                return adapter;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Search retrieve menufood details by menu id
        public Menu SearchUpdate(String f)
        {
            try
            {
                string sqlCommand = "Select * from Menu where MenuID ='" + f + "'";
                con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
                con.Open();

                var command = new SqlCommand(sqlCommand, con);
                Menu m = new Menu();

                using (SqlDataReader oReader = command.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        m.MenuID = oReader["MenuID"].ToString();
                        m.MenuName = oReader["MenuName"].ToString();
                        m.MenuDesc = oReader["MenuDesc"].ToString();
                        m.MenuCategory = oReader["MenuCategory"].ToString();
                        m.MenuStatus = oReader["MenuStatus"].ToString();
                        m.MenuPrice = Convert.ToDouble(oReader["MenuPrice"].ToString());
                        m.DiscPrice = Convert.ToDouble(oReader["DiscountPrice"].ToString());
                    }

                    con.Close();
                    return m;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Search multiple food record from menufood table
        public List<MenuFood> SearchMultipleRecord(String id)
        {
            try
            {

                string sqlCommand = "Select * from MenuFood where MenuID ='" + id + "'";
                con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
                con.Open();

                var command = new SqlCommand(sqlCommand, con);
                List<MenuFood> mfl = new List<MenuFood>();
              
                using (SqlDataReader oReader = command.ExecuteReader())
                {
                    while (oReader.Read())
                    {  MenuFood mf = new MenuFood();
                        mf.menuID = oReader["MenuID"].ToString();
                        mf.foodID = oReader["FoodID"].ToString();
                        mf.quantity = Convert.ToInt32(oReader["Quantity"].ToString());
                        mfl.Add(mf);
                    }

                    con.Close();
                    return mfl;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public void updateMenu(Menu m)
        {
            try
            {
                string sql = "update Menu SET MenuName='" + m.MenuName + "', MenuDesc='" + m.MenuDesc + "', MenuCategory='" + m.MenuCategory + "', MenuStatus='" + m.MenuStatus + "',MenuPrice='" + m.MenuPrice + "',DiscountPrice='" + m.DiscPrice + "' where menuID='" + m.MenuID + "'";
                con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
                con.Open();

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void updateStatus(Menu m)
        {
            try
            {
                //List<Menu> ml= new List<Menu>();
                string sql = "update Menu SET MenuStatus='" + m.MenuStatus + "' where menuID='" + m.MenuID + "'";
                con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
                con.Open();

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.ExecuteNonQuery();
                con.Close();
                //return ml;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //search the food id
        public List<MenuFood> SearchMenuFoodID(Food f)
        {
            try
            {

                string sqlCommand = "Select * from MenuFood where foodid='"+f.foodID+"'";
                con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
                con.Open();

                var command = new SqlCommand(sqlCommand, con);
                List<MenuFood> mfl = new List<MenuFood>();

                using (SqlDataReader oReader = command.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        MenuFood mf = new MenuFood();
                        mf.menuID = oReader["MenuID"].ToString();
                        mf.foodID = oReader["FoodID"].ToString();
                        //mf.quantity = Convert.ToInt32(oReader["Quantity"].ToString());
                        mfl.Add(mf);
                    }

                    con.Close();
                    return mfl;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Search all menu id
        public Menu SearchMenuID()
        {
            try
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
                con.Open();
                string sqlCommand = "Select * from Menu order by menuid asc";

                var command = new SqlCommand(sqlCommand, con);

                Menu m = new Menu();
                using (SqlDataReader oReader = command.ExecuteReader())
                {
                    while (oReader.Read())
                    {

                        m.MenuID = oReader["MenuID"].ToString();
                        // food.Add(food1);
                    }
                }
                con.Close();
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //delete multiple menu ala carte into sets
        public void DeleteFunc(MenuFood f)
        {
            try
            {

                string sql = "Delete from MenuFood where MenuID ='" + f.menuID + "'";
                con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
