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
    class AnnouncementDA
    {
        public void InsertAnnouncementRecord(Announcement Ann)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "INSERT INTO Announcement VALUES ('"+Ann.Title+"', '"+Ann.Content+"', '"+Ann.PromoEndDate+"')";

                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                cmd.ExecuteNonQuery();
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

        public string AnnounceIDRetrieve(Announcement A)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
            string p = "";

            try
            {
                string sql = "SELECT AnnouncementID FROM Announcement WHERE Title = '"+A.Title+"' AND AnnouncementContent = '"+A.Content+"' AND PromoEndDate = '"+A.PromoEndDate+"'";
                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    p = dr["AnnouncementID"].ToString();
                }
                return p;
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

        public void UpdateAnnouncementRecord(Announcement Ann)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "UPDATE Announcement SET Title = '"+Ann.Title+"', AnnouncementContent = '"+Ann.Content+"', PromoEndDate = '"+Ann.PromoEndDate+"' WHERE AnnouncementID = '"+Ann.AnnouncementID+"'";

                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                cmd.ExecuteNonQuery();

                Ann.UpdateStatus = "Success";
            }
            catch(SqlException ex)
            {
                Ann.UpdateStatus = "Failed";
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public bool ExpiredAnnouncementCheck()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "Select PromoEndDate FROM Announcement WHERE CONVERT(date, PromoEndDate, 105) < CONVERT(date, GETDATE(), 105)";
                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    return true;
                }
                return false;
            }
            catch(SqlException ex)
            {
                throw ex;
            }
        }

        public void DeleteExpiredAnnouncement(Announcement Ann)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "DELETE FROM Announcement WHERE AnnouncementID = '"+Ann.AnnouncementID+"'";
                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                cmd.ExecuteNonQuery();

                Ann.DeleteStatus = "Success";
            }
            catch(SqlException ex)
            {
                Ann.DeleteStatus = "Failed";
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public List <string> RetrieveMemberEmails()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            List<string> EmailList = new List<string>();

            try
            {
                string sql = "SELECT Email FROM Member WHERE Status = 'Available';";
                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while(dr.Read())
                {
                    EmailList.Add(Convert.ToString(dr["Email"]));

                }
                return EmailList;
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
