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

            try {
                string sql = "INSERT INTO Employee VALUES ('"+emp.EmployeeName+"', '"+emp.ICNumber+"', '"+emp.Gender+"', '"+emp.HomeAddress+"', '"+emp.HomeNumber+"', '"+emp.HandphoneNumber+"', '"+emp.Email+"', '"+emp.Position+"')";

                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                emp.Status = "Success";
            }
            catch(SqlException ex)
            {
                emp.Status = "Failed";
                throw ex;      
            }
        }
    }
}
