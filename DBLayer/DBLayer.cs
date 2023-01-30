using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Xml.Linq;

namespace DBLayer
{

        public class DBLayer
        {
            public DataTable GetBoligAndOwnersByPhone(string tlf)
            {
                var connectionString = ConfigurationManager.ConnectionStrings["ConnHus"].ConnectionString;
                DataTable dt = new DataTable();
                SqlParameter param;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    //den samme sql queryen her som dere allerede har testet i sql manager - her med et parameter, som er telefonnummeret
                    SqlCommand cmd = new SqlCommand("SELECT Hus.HusID, Hus.Adresse, Eier.Telefon, Eier.Fodselsnummer, Eier.Fornavn FROM Eienskap INNER JOIN Eier ON Eier.Fodselsnummer = Eienskap.Fodselsnummer INNER JOIN Hus ON Hus.HusID = Eienskap.HusID Where Telefon = @tlf", conn);
                    cmd.CommandType = CommandType.Text;

                    //params here
                    param = new SqlParameter("@tlf", SqlDbType.NVarChar);
                    param.Value = tlf; //variabel som blir sendt inn til metoden. Kommer fra bruker som tastet dette inn i et tekstfelt.
                    cmd.Parameters.Add(param);

                    SqlDataReader reader = cmd.ExecuteReader();

                    dt.Load(reader);

                    reader.Close();
                    conn.Close();
                }
                return dt; //hele datasettet returneres. skal da kobles til feks en gridview
            }

            /// <summary>
            /// Returnerer alt fra tabellen boliger. Ikke hensiktsmessig om det er mange boliger.
            /// </summary>
            /// <returns></returns>
            public DataTable GetAllBolig()
            {
                var connectionString = ConfigurationManager.ConnectionStrings["ConnHus"].ConnectionString;
                DataTable dt = new DataTable();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Hus", conn);
                    cmd.CommandType = CommandType.Text;

                    SqlDataReader reader = cmd.ExecuteReader();

                    dt.Load(reader);

                    reader.Close();
                    conn.Close();
                }
                return dt;
            }
            
        }
}
