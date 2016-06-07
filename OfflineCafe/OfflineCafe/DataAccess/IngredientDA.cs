﻿using OfflineCafe.Entity;
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
                string sql = "INSERT INTO Ingredient VALUES ('"+ing.IngredientName+"', '"+ing.IngredientDesc+"', '"+ing.IngredientQty+"', '"+ing.StorageArea+"', '"+ing.ExpiryDate+"', '"+ing.ReOrderLevel+"', '"+ing.ReOrderQty+"', '"+ing.IngredientStatus+"')";

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
                string sql = "UPDATE Ingredient SET IngredientName = '"+ing.IngredientName+"', IngredientDesc = '"+ing.IngredientDesc+"', StorageArea = '"+ing.StorageArea+"', ExpiryDate = '"+ing.ExpiryDate+"', ReOrderLevel = '"+ing.ReOrderLevel+"', ReOrderQuantity = '"+ing.ReOrderQty+"', IngredientStatus = '"+ing.IngredientStatus+"' WHERE IngredientID = '"+ing.IngredientID+"'";

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
    }
}