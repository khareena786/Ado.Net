using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace onlinventorymgtsys
{
    internal class datalayer
    {
        //connecting database using connectionstring
        //static string connectionstring = "Data source=ICS-LT-F6L96V3\\SQLEXPRESS;" + "Initial Catalog=onlinventory; Integrated Security=True;";

        public static void InsertCategory(int proid, string proname, int price, int proqty)
        {
            //inserting product into the database
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Khareenadbconn"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "usp_insertprod";
                    cmd.CommandType = CommandType.StoredProcedure;//using storedprocedure for insertion
                    //parameters for the storedprocedure
                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@proid";
                    param1.SqlDbType = SqlDbType.Int;
                    param1.Value = proid;

                    cmd.Parameters.Add(param1);
                    //parameter2
                    SqlParameter param2 = new SqlParameter();
                    param2.ParameterName = "@proname";
                    param2.SqlDbType = SqlDbType.VarChar;
                    param2.Value = proname;

                    cmd.Parameters.Add(param2);
                    //parameter 3
                    SqlParameter param3 = new SqlParameter();
                    param3.ParameterName = "@price";
                    param3.SqlDbType = SqlDbType.Money;
                    param3.Value = price;

                    cmd.Parameters.Add(param3);
                    //parameter 4
                    SqlParameter param4 = new SqlParameter();
                    param4.ParameterName = "@proqty";
                    param4.SqlDbType = SqlDbType.Int;
                    param4.Value = proqty;

                    cmd.Parameters.Add(param4);

                    con.Open();
                    Console.WriteLine(con.State);
                    int i = cmd.ExecuteNonQuery();//this cmd executes the stored procedure
                    if (i > 0)
                    {
                        Console.WriteLine(proname + "data inserted succesfully");
                    }
                    con.Close();
                    Console.WriteLine(con.State);

                }
                 
            }
                catch (Exception ex)
                {
                Console.WriteLine(ex.Message);
                }
            }
            public static void Retrieveprodinfo()//retrieving the values from the database
            {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Khareenadbconn"].ConnectionString))
                {               
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandText = "selectallproducts";//displays the table values
                    command.CommandType = CommandType.StoredProcedure;
                    //open the connection and execute the reader
                    con.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("{0} {1} {2} {3}",
                                    reader[0], reader[1], reader[2], reader[3]);

                           }
                        }
                        else
                        {

                            Console.WriteLine("No records found");
                        }

                        reader.Close();
                    }
                }
            }

            catch (Exception ex)

            {
                Console.WriteLine(ex);
            }
            }
            //updating the product information
                public static void updateproinfo(int proid, string newproname,int newprice, int newproqty)
                {
                    try

                    {

                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Khareenadbconn"].ConnectionString))

                        {

                            SqlCommand command = new SqlCommand();
                            command.Connection = con;
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "usp_updateprod";//stored procedure for updating values
                            //para1
                            SqlParameter parmeter = new SqlParameter();
                            parmeter.ParameterName = "@proid";
                            parmeter.SqlDbType = SqlDbType.Int;
                            parmeter.Direction = ParameterDirection.Input;
                            parmeter.Value = proid;
                            command.Parameters.Add(parmeter);
                            //para2
                            SqlParameter parmeter2 = new SqlParameter();
                            parmeter2.ParameterName = "@newprice";
                            parmeter2.SqlDbType = SqlDbType.Money;
                            parmeter2.Direction = ParameterDirection.Input;
                            parmeter2.Value = newprice;
                            command.Parameters.Add(parmeter2);
                            //para3
                            SqlParameter parmeter3 = new SqlParameter();
                            parmeter3.ParameterName = "@newproname";
                            parmeter3.SqlDbType = SqlDbType.VarChar;
                            parmeter3.Direction = ParameterDirection.Input;
                            parmeter3.Value = newproname;
                            command.Parameters.Add(parmeter3);
                           //para4
                            SqlParameter parmeter4 = new SqlParameter();
                            parmeter4.ParameterName = "@newproqty";
                            parmeter4.SqlDbType = SqlDbType.Int;
                            parmeter4.Direction = ParameterDirection.Input;
                            parmeter4.Value = newproqty ;
                            command.Parameters.Add(parmeter4);
                            con.Open();
                            int i = command.ExecuteNonQuery();//here this cmd used for insertion and updating
                            if (i > 0)
                            {

                                Console.WriteLine("updated successfully");//after execution it will display output

                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message);

                    }

                }
             //deleting a product
              public static void deleteprod(int proid)

              {
               try
              {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Khareenadbconn"].ConnectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "deleteproduct";//storedprocedure for deleting a product

                    SqlParameter parmeter = new SqlParameter();
                    parmeter.ParameterName = "@proid";//by using prodid the row will be deleted
                    parmeter.SqlDbType = SqlDbType.Int;
                    parmeter.Direction = ParameterDirection.Input;
                    parmeter.Value = proid;
                    command.Parameters.Add(parmeter);

                    con.Open();

                    int i = command.ExecuteNonQuery();

                    if (i > 0)

                    {

                        Console.WriteLine("deleted successfully");

                    }
                }
               }

                catch (Exception ex)

               {

                Console.WriteLine(ex.Message);

              }

              }
               //calculating the sold items amount
             public static int calculateamt(int qtysold, int price)
            {
            return qtysold * price;
            }
           //inserting values into the sales 
           public static void sales(int salesid, int proid, int price, int qtysold)
           {
            double amount = calculateamt(qtysold, price);
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Khareenadbconn"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "usp_sales";//stored procedure for the sales 
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@salesid";
                    param1.SqlDbType = SqlDbType.Int;
                    param1.Value = salesid;

                    cmd.Parameters.Add(param1);

                    SqlParameter param2 = new SqlParameter();
                    param2.ParameterName = "@proid";//here prodid will be the foreign key
                    param2.SqlDbType = SqlDbType.Int;
                    param2.Value = proid;

                    cmd.Parameters.Add(param2);

                    SqlParameter param3 = new SqlParameter();
                    param3.ParameterName = "@amount";
                    param3.SqlDbType = SqlDbType.Money;
                    param3.Value = amount;

                    cmd.Parameters.Add(param3);

                    SqlParameter param4 = new SqlParameter();
                    param4.ParameterName = "@qtysold";
                    param4.SqlDbType = SqlDbType.Int;
                    param4.Value = qtysold;

                    cmd.Parameters.Add(param4);

                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        Console.WriteLine(salesid + "data inserted succesfully");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           }
            public static void Retrievesales()//retrieve the sales values 
            {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Khareenadbconn"].ConnectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandText = "selectsales";
                    command.CommandType = CommandType.StoredProcedure;
                    //open the connection and execute the reader
                    con.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("{0} {1} {2} {3}",
                                    reader[0], reader[1], reader[2], reader[3]);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No records found");
                        }
                        reader.Close();
                    }
                }
            }
             catch (Exception ex)

            {
                Console.WriteLine(ex);
            }
        }
    }
}
