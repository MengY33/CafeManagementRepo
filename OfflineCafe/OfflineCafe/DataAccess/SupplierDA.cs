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
    class SupplierDA
    {
        public void SupplierInsertRecord(Supplier supp)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "INSERT INTO Supplier VALUES ('"+supp.SupplierName+"', '"+supp.SupplierGender+"', '"+supp.SupplierHandphoneNum+"', '"+supp.SupplierEmail+"', '"+supp.CompanyName+"', '"+supp.CompanyAddress+"', '"+supp.CompanyNum+"', '"+supp.CompanyFaxNum+"', '"+supp.SuppStatus+"')";

                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                cmd.ExecuteNonQuery();

                supp.InsertStatus = "Success";
            }
            catch(Exception ex)
            {
                supp.InsertStatus = "Failed";
                throw ex;
            }
            con.Close();
        }

        public void SupplierUpdateRecord(Supplier sup)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "UPDATE Supplier SET SupplierName = '"+sup.SupplierName+"', Gender = '"+sup.SupplierGender+"', HandphoneNumber = '"+sup.SupplierHandphoneNum+"', Email = '"+sup.SupplierEmail+"', CompanyName = '"+sup.CompanyName+"', CompanyAddress = '"+sup.CompanyAddress+"', CompanyNumber = '"+sup.CompanyNum+"', CompanyFaxNumber = '"+sup.CompanyFaxNum+"', SuppStatus = '"+sup.SuppStatus+"' WHERE SupplierID = '"+sup.SupplierID+"'";

                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                sup.UpdateStatus = "Success";
            }
            catch(Exception ex)
            {
                sup.UpdateStatus = "Failed";
                throw ex;
            }
        }
    }
}
