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
    class LoginDA
    {
        SqlConnection con = new SqlConnection();
        //Insert login when employee is created
        public void createLogin(Login l)
        {
            try
            {

                string sql = "insert into EmployeeLogin values ('" + l.loginid + "','" + l.password+ "','" + l.question + "','" + l.answer + "')";
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
        //search login id get employee details
        public List<Food> searchid(string id)
        {
            try
            {
                string sqlCommand = "Select * from Food where FoodID='" + id+ "'";
                con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
                con.Open();

                var command = new SqlCommand(sqlCommand, con);
                List<Food> el = new List<Food>();

                using (SqlDataReader oReader = command.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        Food e = new Food();
                        e.foodID = oReader["FoodID"].ToString();
                        e.foodName = oReader["FoodName"].ToString();
                        e.foodType = oReader["FoodType"].ToString();
                        e.foodStatus = oReader["FoodStatus"].ToString();
                        //mf.quantity = Convert.ToInt32(oReader["Quantity"].ToString());
                        el.Add(e);
                    }

                    con.Close();
                    return el;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //search id list
        public List<Login> searchid2()
        {
            try
            {
                string sqlCommand = "Select * from EmployeeLogin";
                con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
                con.Open();

                var command = new SqlCommand(sqlCommand, con);
                List<Login> ll = new List<Login>();

                using (SqlDataReader oReader = command.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        Login l = new Login();
                        l.loginid = oReader["EmpLoginID"].ToString();
                        l.password = oReader["LoginPassword"].ToString();
                        l.question = oReader["SecurityQuestion"].ToString();
                        l.answer = oReader["SecurityAnswer"].ToString();

                        ll.Add(l);
                    }

                    con.Close();
                    return ll;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //update the security question
        public void updateSecurity(Login l)
        {
            try
            {
                string sql = "Update EmployeeLogin SET LoginPassword='" +l.password+"',SecurityQuestion='" + l.question+ "',SecurityAnswer='" + l.answer + "' where EmpLoginID='" + l.loginid + "'";
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
        //update the password
        public void updatePassword(Login l)
        {
            try
            {
                string sql = "Update EmployeeLogin SET LoginPassword='" + l.password + "' where EmpLoginID='" + l.loginid + "'";
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
