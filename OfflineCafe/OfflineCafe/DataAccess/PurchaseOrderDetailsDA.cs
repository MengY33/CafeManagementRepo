using OfflineCafe.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineCafe.DataAccess
{
    class PurchaseOrderDetailsDA
    {
        int i = 0;
        int p, a, b;

        public void InsertPurchaseOrderDetails(List<PurchaseOrderDetails> pod)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            SqlCommand cmd = new SqlCommand("INSERT INTO PurchaseOrderDetails (PurchaseOrderID, IngredientID, PurchaseQuantity, ExpiryDate, ExpiredStatus) VALUES (@PurchaseOrderID, @IngredientID, @PurchaseQuantity, @ExpiryDate, @ExpiredStatus)", con);

            try
            {
                con.Open();
                cmd.Parameters.Add("@PurchaseOrderID", SqlDbType.VarChar);
                cmd.Parameters.Add("@IngredientID", SqlDbType.VarChar);
                cmd.Parameters.Add("@PurchaseQuantity", SqlDbType.Int);
                cmd.Parameters.Add("@ExpiryDate", SqlDbType.VarChar);
                cmd.Parameters.Add("@ExpiredStatus", SqlDbType.VarChar);
                

                foreach (PurchaseOrderDetails pod2 in pod)
                {
                    i = p;

                    cmd.Parameters["@PurchaseOrderID"].Value = pod2.PurchaseOrderID;
                    cmd.Parameters["@IngredientID"].Value = pod2.IngredientID;
                    cmd.Parameters["@PurchaseQuantity"].Value = pod2.PurchaseQuantity;
                    cmd.Parameters["@ExpiryDate"].Value = pod2.ExpiryDate;
                    cmd.Parameters["@ExpiredStatus"].Value = "Uncheck";
                    
                    cmd.ExecuteNonQuery();
                    i++;

                    p = i;

                    if(i >= pod.Count)
                    
                        break;
                               
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

        public void IngQuantityIncrease(List<PurchaseOrderDetails> podList)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
            

            try
            {
                con.Open();

                foreach(PurchaseOrderDetails podList2 in podList)
                {
                    a = b;

                    SqlCommand cmd = new SqlCommand("UPDATE Ingredient SET Quantity = Quantity + '"+podList2.PurchaseQuantity+"'  WHERE IngredientID = '"+podList2.IngredientID+"'", con);

                    cmd.ExecuteNonQuery();

                    a++;
                    b = a;

                    if (a >= podList.Count)

                        break;
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

        public void ExpiredStatusUpdate(ExpiredIngredientDetails eid)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "UPDATE PurchaseOrderDetails SET ExpiredStatus = 'Checked' WHERE PurchaseOrderID = '"+eid.ExpiredPurchaseOrderID+"'";

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
    }
}
