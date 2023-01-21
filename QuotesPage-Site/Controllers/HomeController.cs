using QuotesPage_Site.Models;
using System;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.UI;

namespace QuotesPage_Site.Controllers
{
    public class HomeController : Controller
    {
        private static string _connString = string.Empty;

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [HttpGet]
        public ViewResult Index()
        {
            var model = new QuoteModel();
            model = GetQuote();

            ModelState.Clear();

            return View(model);
        }

        public QuoteModel GetQuote()
        {

#if DEBUG
            _connString = "Data Source=JAMIE-PC\\SQLEXPRESS;AttachDbFilename=D:\\Sites\\LocalDB\\jb-local-db.mdf;Initial Catalog=JB-Local-DB;Integrated Security=True";
#else

            _connString = "Data Source=JAMIE-NAS\\SQLEXPRESS;AttachDbFilename=E:\\Sites\\LocalDB\\jb-local-db.mdf;Initial Catalog=JB-Local-DB;Integrated Security=True";
#endif

            var quote = new QuoteModel
            {
                ID = 0,
                QuoteText = "Error Reading From Database"
            };

            try
            {
                using (SqlConnection conn = new SqlConnection())
                {

                    conn.ConnectionString = _connString;
                    conn.Open();

                    SqlCommand command = new SqlCommand(
                        @"
                           SELECT TOP 1 ID, QuoteText FROM [shared].[Quotes] ORDER BY NEWID()"
                        , conn);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // while there is another record present
                        if (reader.Read())
                        {
                            quote.ID = Convert.ToInt32(reader["ID"]);
                            string rawQuote = Convert.ToString(reader["QuoteText"]);

                            int lastIndexOfHyphen = rawQuote.LastIndexOf('-');

                            if (lastIndexOfHyphen != -1)
                            {
                                string quotePart1 = rawQuote.Substring(0, lastIndexOfHyphen);
                                string quotePart2 = "<br /><font style=\"font-style: italic\">" + rawQuote.Substring(lastIndexOfHyphen, rawQuote.Length - lastIndexOfHyphen) + "</font>";

                                quote.QuoteText = quotePart1 + quotePart2;
                            }
                            else
                            {
                                quote.QuoteText = rawQuote;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
            }

            return quote;
        }
    }
}