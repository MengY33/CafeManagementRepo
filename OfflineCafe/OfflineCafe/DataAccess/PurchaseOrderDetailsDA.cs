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
            int i = 0;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                for(; i < pod.Count;)
                {
                    
                    string sql = "INSERT INTO PurchaseOrderDetails VALUES ('"+pod.ElementAt(i).PurchaseOrderID+"', '"+pod.ElementAt(i).IngredientID+"', '"+pod.ElementAt(i).PurchaseQuantity+"', '"+pod.ElementAt(i).ExpiryDate+"')";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    i++;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                
            }
            catch(SqlException ex)
            {
                throw ex;
            }
           
        }

        public void IngQuantityIncrease(List<PurchaseOrderDetails> podList)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
            string sql = "";

            try
            {
                for(int i = 0; i <podList.Count; i++)
                {
                    sql = "UPDATE Ingredient SET Quantity = Quantity + '"+podList.ElementAt(i).PurchaseQuantity+"' WHERE IngredientID = '"+podList.ElementAt(i).IngredientID+"'";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                
            }
            catch(SqlException ex)
            {
                throw ex;
            }
 
        }
    }
}
