using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace ParseQuotesToDatabase
{
    class Program
    {
        private static string _connString = string.Empty;

        static void Main(string[] args)
        {
            String appDataPath = Path.GetFullPath("..\\..\\..\\QuotesPage-Site\\App_Data\\");
            AppDomain.CurrentDomain.SetData("DataDirectory", appDataPath);

#if DEBUG
            _connString = "Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\QuotesDB.mdf;Initial Catalog=PoolStatsDB;Integrated Security=True";
#else

            _connString = privateclass._connString;
#endif

            string[] quotes = File.ReadAllLines("MoreQuotes.txt");
            List<string> quotesList = new List<string>();

            foreach (string quote in quotes)
            {
                if (quote.Length > 20)
                    quotesList.Add(quote);
            }

            InsertQuotesToDatabase(quotesList);
        }
        public static void InsertQuotesToDatabase(List<String> quotes)
        {
            foreach (var quote in quotes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection())
                    {
                        conn.ConnectionString = _connString;
                        conn.Open();

                        SqlCommand command = new SqlCommand(
                            String.Format(
                            @"INSERT INTO [Shared].[Quotes] (QuoteText)    
                              Values ('{0}');", quote)
                            , conn);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    Console.Out.Write(e.Message);
                }
            }
        }
    }
}
