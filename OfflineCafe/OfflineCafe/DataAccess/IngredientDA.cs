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
    class IngredientDA
    {
        public void InsertIngredientRecord(Ingredient ing)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "INSERT INTO Ingredient VALUES ('"+ing.IngredientName+"', '"+ing.IngredientDesc+"', '"+ing.IngredientQty+"', '"+ing.Unit+"', '"+ing.StorageArea+"', '"+ing.ReOrderLevel+"', '"+ing.ReOrderQty+"', '"+ing.IngredientStatus+"')";

                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                cmd.ExecuteNonQuery();

                ing.InsertStatus = "Success";
            }
            catch(SqlException ex)
            {
                ing.InsertStatus = "Failed";
                throw ex;
            }
            con.Close();
        }

        public void UpdateIngredientRecord(Ingredient ing)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "UPDATE Ingredient SET IngredientName = '"+ing.IngredientName+"', IngredientDesc = '"+ing.IngredientDesc+"', Unit = '"+ing.Unit+"' , StorageArea = '"+ing.StorageArea+"', ReOrderLevel = '"+ing.ReOrderLevel+"', ReOrderQuantity = '"+ing.ReOrderQty+"', IngredientStatus = '"+ing.IngredientStatus+"' WHERE IngredientID = '"+ing.IngredientID+"'";

                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                cmd.ExecuteNonQuery();

                ing.UpdateStatus = "Success";
            }
            catch(SqlException ex)
            {
                ing.UpdateStatus = "Failed";
                throw ex;
            }
            con.Close();
        }

        public bool IngredientQtyCheck()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
            Ingredient i = new Ingredient();

            try
            {
                string sql = "SELECT IngredientName FROM Ingredient WHERE Quantity < ReOrderLevel AND IngredientStatus = 'Available';";
                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while(dr.Read())
                {
                    i.IngredientNameRetrieved = dr["IngredientName"].ToString();
                    return true;
                }
                return false;
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
