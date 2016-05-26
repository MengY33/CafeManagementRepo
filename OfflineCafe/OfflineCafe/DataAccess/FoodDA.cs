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
    class FoodDA
    {
        SqlConnection con = new SqlConnection();
        //Add Food Data 
        public void InsertFunc(Food f)
        {
            try {

                string sql = "insert into Food values ('" + f.foodID + "','" + f.foodName + "','" + f.foodDesc + "','" + f.foodType + "','" + f.foodPrice + "','" + f.foodStatus +"')";
                con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.ExecuteNonQuery();
                con.Close(); }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Delete Food Data
        public void DeleteFunc(Food f)
        {
            try
            {

                string sql = "Delete from Food where FoodID ='" + f.foodID + "'";
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
        //Search Food Data use by Food Name
        public SqlDataAdapter SearchFunc(Food f)
        {
            try
            {

                con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
                con.Open();
                string sqlCommand = "Select * from Food where FoodName LIKE '%" + f.foodName + "%'";
                List<Food> pList = new List<Food>();
                var command = new SqlCommand(sqlCommand, con);
                con.Close();
                var adapter = new SqlDataAdapter(command);
                return adapter;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Search Food Data use by Food Name and status is available
        public SqlDataAdapter SearchAvailableName(String f)
        {
            try
            {

                con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
                con.Open();
                string sqlCommand = "Select * from Food where foodstatus ='Available' and FoodName LIKE '%" + f + "%'";
                List<Food> pList = new List<Food>();
                var command = new SqlCommand(sqlCommand, con);
                con.Close();
                var adapter = new SqlDataAdapter(command);
                return adapter;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Search Food Data use by food type
        public SqlDataAdapter SearchTypeFunc(Food f)
        {
            try
            {

                con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
                con.Open();
                string sqlCommand = "Select * from Food where FoodType like '"+ f.foodType + "'";
                List<Food> pList = new List<Food>();
                var command = new SqlCommand(sqlCommand, con);
                con.Close();
                var adapter = new SqlDataAdapter(command);
                return adapter;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Search Food Data to Update
        public Food SearchUpdate(String f)
        {
            try
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
                con.Open();
                string sqlCommand = "Select * from Food where FoodID ='" + f + "'";
        
                var command = new SqlCommand(sqlCommand, con);
                Food food = new Food();

                using (SqlDataReader oReader = command.ExecuteReader())
                {
                    while (oReader.Read())
                    {

                        food.foodID = oReader["FoodID"].ToString();
                        food.foodName = oReader["FoodName"].ToString();
                        food.foodDesc = oReader["FoodDesc"].ToString();
                        food.foodType = oReader["FoodType"].ToString();
                        food.foodPrice = Convert.ToDouble(oReader["FoodPrice"].ToString());
                        food.foodStatus = oReader["FoodStatus"].ToString();
                       
                    }
                    
                    con.Close();
                    return food;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Search all food id
        public Food SearchFoodID()
        {
            try
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
                con.Open();
                string sqlCommand = "Select * from Food order by foodid asc";

                var command = new SqlCommand(sqlCommand, con);
                
                Food food = new Food();
                using (SqlDataReader oReader = command.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        
                        food.foodID = oReader["FoodID"].ToString();
                       // food.Add(food1);
                    }
                }
                con.Close();
                return food;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //DISPAY menu data which status is available
        public SqlDataAdapter SearchAvailable()
        {
            try
            {

                con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
                con.Open();
                string sqlCommand = "Select * from Food where foodstatus ='Available'";
                List<Food> pList = new List<Food>();
                var command = new SqlCommand(sqlCommand, con);
                con.Close();
                var adapter = new SqlDataAdapter(command);
                return adapter;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      
        //DISPAY menu data which status is available and type
        public SqlDataAdapter SearchAvailableType(Food f)
        {
            try
            {

                con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
                con.Open();
                string sqlCommand = "Select * from Food where FoodType like '" + f.foodType + "' and foodstatus = 'Available'";
                List<Food> pList = new List<Food>();
                var command = new SqlCommand(sqlCommand, con);
                con.Close();
                var adapter = new SqlDataAdapter(command);
                return adapter;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Update Food Data
        public void UpdateFunc(Food f)
        {
            try {
                string sql = "Update Food SET FoodName='" + f.foodName + "',FoodDesc='" + f.foodDesc + "',FoodType='" + f.foodType + "', FoodPrice='" + f.foodPrice + "', FoodStatus='" + f.foodStatus + "' where FoodID='" + f.foodID + "'";
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

