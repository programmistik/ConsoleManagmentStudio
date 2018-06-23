
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ConsoleManagmentStudio
{
    class Program
    {
        private static void ReadSingleRow(IDataRecord record)
        {
            int count = record.FieldCount;
            for (int i = 0; i < count; i++)
            {
                var element = String.Format($"{record[i]}");

                    if (element.Length >= 10)
                {
                    Console.Write("{0, -10}|", element.Substring(0, 8) + "..");
                }

                else
                {
                    Console.Write("{0, -10}|", element);
                }


               // Console.Write(String.Format($"{record[i]}   "));
            }
            Console.WriteLine();

        }
        //---------------------------------------------------------
        static void Main(string[] args)
        {
            SqlConnection myConnection;
            string ConnStr;

          //  ConnStr = @"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = G:\SQL\DATABASES_1523373969_1525514301\LIBRARY\LIBRARYSQL.MDF; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
             ConnStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=D:\SQL\DATABASES_1523373969_1525514301\LIBRARY\LIBRARYSQL.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //string ServerName, DatabaseName, Username, Password;
            //Console.WriteLine("Connect to SQL Server");
            //Console.Write("Server: ");
            //ServerName = Console.ReadLine();
            //Console.Write("Username: ");
            //Username = Console.ReadLine();
            //Console.Write("Password: ");
            //Password = Console.ReadLine();
            //Console.WriteLine("Connect to exsisting database or create new?");
            //Console.WriteLine("Enter 1 to create");
            //int.TryParse(Console.ReadLine(), out int crt);

            //if (crt != 1)
            //{
            //    //myConnection = new SqlConnection("user id=EMBAWOOD\\DianaGojayeva;" +
            //    //                         "server=(localdb)\\MSSQLLocalDB;" +
            //    //                        "Trusted_Connection=yes;" +
            //    //                       "database=G:\\SQL\\DATABASES_1523373969_1525514301\\LIBRARY\\LIBRARYSQL.MDF " +
            //    //                      "connection timeout=30");
            myConnection = new SqlConnection(ConnStr);
               

                try
                {
                    myConnection.Open();
                    Console.WriteLine("CONNECTED!");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    Console.ReadKey();
                }

            // Query Text
            ConsoleKeyInfo cki, cki2;
            bool comment = false;
            do
            {
                Console.WriteLine("\nPress F2 for new query and F5 to execute it or ESCAPE to exit.");
                StringBuilder sb = new StringBuilder(10000);

                
                cki2 = Console.ReadKey(true);
                if (cki2.Key == ConsoleKey.F2)
                {
                    Console.WriteLine("Type query here");
                    do
                    {
                        cki = Console.ReadKey(true);
                        if (cki.Key != ConsoleKey.NumPad0 && cki.Key != ConsoleKey.Enter && cki.Key != ConsoleKey.Backspace)
                        {
                            if (cki.Key == ConsoleKey.Subtract || cki.Key == ConsoleKey.OemMinus)
                            {
                                if (sb[sb.Length - 1].ToString() == "-")
                                {
                                    comment = true;
                                    sb.Remove(sb.Length - 1, 1);
                                }
                            }
                            if (!comment)
                                sb = sb.Append(cki.KeyChar);
                            Console.Write(cki.KeyChar);
                        }
                        else if (cki.Key == ConsoleKey.Enter)
                        {
                            sb = sb.Append(" ");
                            Console.WriteLine();
                            comment = false;
                        }
                        else if (cki.Key == ConsoleKey.Backspace)
                        {
                            if (sb.Length > 0)
                            {
                                sb = sb.Remove((sb.Length - 1), 1);
                                Console.Write("\b \b");
                            }
                        }

                    } while (cki.Key != ConsoleKey.F5);


                    var str = sb.ToString();
                    if (str.Contains("SELECT") || str.Contains("select"))
                    {
                        //---------------------------
                        try
                        {
                            SqlDataReader myReader = null;
                            SqlCommand rCommand = new SqlCommand(str, myConnection);
                            myReader = rCommand.ExecuteReader();
                            Console.WriteLine();
                            var count = myReader.FieldCount;
                            for (int i = 0; i < count; i++)
                            {
                               // var Desc = String.Format(myReader.GetName(i));
                                Console.Write("{0,-10}|", myReader.GetName(i));
                            }
                            Console.WriteLine();
                            Console.WriteLine("--------------------------------------------------------------------------------");
                            while (myReader.Read())
                            {
                                ReadSingleRow(myReader);
                            }
                            myReader.Close();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }

                    }
                    else
                    {

                        SqlCommand myCommand = new SqlCommand(str, myConnection);


                        myCommand.ExecuteNonQuery();
                    }
                }
            } while (cki2.Key != ConsoleKey.Escape);
            //---------------------------------

            try
            {
                myConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }


            Console.ReadKey();

        }
    }
}
