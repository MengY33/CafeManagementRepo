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
    class EmployeeDA
    {
        public void EmployeeInsertRecord(Employee emp)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "INSERT INTO Employee VALUES ('" + emp.EmployeeName + "', '" + emp.ICNumber + "', '" + emp.Gender + "', '" + emp.HomeAddress + "', '" + emp.HomeNumber + "', '" + emp.HandphoneNumber + "', '" + emp.Email + "', '" + emp.Position + "', '" + emp.CurrentStatus + "')";

                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                cmd.ExecuteNonQuery();

                emp.InsertStatus = "Success";
            }
            catch (SqlException ex)
            {
                emp.InsertStatus = "Failed";
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public void EmployeeUpdateRecord(Employee em)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "UPDATE Employee SET EmployeeName = '" + em.EmployeeName + "', ICNumber = '" + em.ICNumber + "', Gender = '" + em.Gender + "', HomeAddress = '" + em.HomeAddress + "', HomeNumber = '" + em.HomeNumber + "', HandphoneNumber = '" + em.HandphoneNumber + "', Email = '" + em.Email + "', Position = '" + em.Position + "', EmpStatus = '" + em.CurrentStatus + "' WHERE EmployeeID = '" + em.EmployeeID + "'";

                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                em.UpdateStatus = "Success";
            }
            catch (SqlException ex)
            {
                em.UpdateStatus = "Failed";
                throw ex;
            }
        }

        public void RetreiveEmployeeID(Employee e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "SELECT EmployeeID FROM Employee WHERE ICNumber = '"+e.ICNumber+"'";
                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    e.EmployeeIDRetrieve = dr["EmployeeID"].ToString();
                } 
            }
            catch(SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public void RetrieveAdminName(Employee ee)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "SELECT EmployeeName FROM Employee WHERE Position = 'Admin';";
                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while(dr.Read())
                {
                    ee.AdminNameRetrieve = dr["EmployeeName"].ToString();
                }
            }
            catch(SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
    }
}
