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
    class PurchaseOrderDA
    {
        public void InsertPurchaseOrderRecord(PurchaseOrder po)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;

            try
            {
                string sql = "INSERT INTO PurchaseOrder VALUES ('"+po.POEmployeeID+"', '"+po.POSupplierID+"', '"+po.POOrderDate+"', '"+po.POOrderTime+"')";
                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                cmd.ExecuteNonQuery();

                po.InsertStatus = "Success";
            }
            catch (SqlException ex)
            {
                po.InsertStatus = "Failed";
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public void POIDRetrieve(PurchaseOrder pOrder)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Cafe"].ConnectionString;
            PurchaseOrder po = new PurchaseOrder();

            try
            {
                string sql = "SELECT PurchaseOrderID FROM PurchaseOrder WHERE EmployeeID = '"+pOrder.POEmployeeID+"' AND SupplierID = '"+pOrder.POSupplierID+"' AND OrderDate = '"+pOrder.POOrderDate+"' AND OrderTime = '"+pOrder.POOrderTime+"'";
                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    po.POID = dr["PurchaseOrderID"].ToString();
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
