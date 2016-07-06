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
    class ExpiredIngredientDetailsDA
    {
        public void InsertExpiredIngredientdetailsRecord(ExpiredIngredientDetails eid)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "INSERT INTO ExpiredIngredientDetails VALUES('"+eid.ExpiredPurchaseOrderID+"', '"+eid.ExpiredIngredientID+"', '"+eid.ExpiredIngredientName+"', '"+eid.ExpiredPurchaseQuantity+"', '"+eid.ExpiredDate+"', '"+eid.ExpiredAmount+"')";

                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                cmd.ExecuteNonQuery();

                eid.InsertStatus = "Success";
            }
            catch(SqlException ex)
            {
                eid.InsertStatus = "Failed";
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
    }
}
