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
    class EmployeeAnnouncementDA
    {
        public void InsertEmployeeAnnouncementRecord(EmployeeAnnouncement EmpAnn)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "INSERT INTO EmployeeAnnouncement VALUES ('"+EmpAnn.AnnouncementID+"', '"+EmpAnn.AnnounceEmployeeID+"', '"+EmpAnn.CreatedDate+"')";

                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                cmd.ExecuteNonQuery();

                EmpAnn.InsertStatus = "Success";
            }
            catch(SqlException ex)
            {
                EmpAnn.InsertStatus = "Failed";
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public void DeleteExpiredEmployeeAnnouncement(EmployeeAnnouncement EmpAnn)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "DELETE FROM EmployeeAnnouncement WHERE AnnouncementID = '"+EmpAnn.AnnouncementID+"'";
                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                cmd.ExecuteNonQuery();

                EmpAnn.DeleteStatus = "Success";
            }
            catch(SqlException ex)
            {
                EmpAnn.DeleteStatus = "Failed";
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
    }
}
