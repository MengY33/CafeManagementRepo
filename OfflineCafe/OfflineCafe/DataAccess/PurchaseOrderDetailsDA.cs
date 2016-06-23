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
    class PurchaseOrderDetailsDA
    {
        public void InsertPurchaseOrderDetails(List<PurchaseOrderDetails> pod)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                for(int i = 0; i < pod.Count; i++)
                {
                    string sql = "INSERT INTO PurchaseOrderDetails VALUES ('"+pod.ElementAt(i).PurchaseOrderID+"', '"+pod.ElementAt(i).IngredientID+"', '"+pod.ElementAt(i).PurchaseQuantity+"')";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    Console.WriteLine(pod.ElementAt(i).PurchaseOrderID);

                    con.Open();
                    cmd.ExecuteNonQuery();
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
