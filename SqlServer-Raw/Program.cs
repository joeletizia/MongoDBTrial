using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data;
using System.Diagnostics;

namespace SqlServer_Raw
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection conn = new SqlConnection("Server=localhost;Database=trial;Integrated Security=true;");

            conn.Open();

            Stopwatch watch = new Stopwatch();

            watch.Start();
            for (int i = 0; i < 1000000; i++)
            {
                CredentialSet cred = new CredentialSet
                    {
                        LastUpdate = DateTime.Now,
                        Title = string.Format("Joe's cred {0}", i),
                        Notes = string.Format("Joe's note {0}", i)
                    };

                WriteCredential_StoredProcedure(cred, conn);
            }
            watch.Stop();
            conn.Close();

            Console.WriteLine(string.Format("Elapsed time: {0}", watch.Elapsed));
            Console.Read();
        }

        public class CredentialSet
        {
            public string Title { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string WebSite { get; set; }
            public string Notes { get; set; }
            public int Owner { get; set; }
            public DateTime LastUpdate { get; set; }
        }

        public static bool WriteCredential(CredentialSet cred, SqlConnection conn)
        {
            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.InsertCommand = new SqlCommand("insert into Credentials1 (Notes, Title) values (@note, @title)",conn);
                da.InsertCommand.Parameters.Add(new SqlParameter("@note", SqlDbType.VarChar));
                da.InsertCommand.Parameters.Add(new SqlParameter("@title", SqlDbType.VarChar));
                da.InsertCommand.Parameters["@note"].Value = cred.Notes;
                da.InsertCommand.Parameters["@title"].Value = cred.Title;

                da.InsertCommand.ExecuteNonQuery();
            }

            return true;
        }

        public static bool WriteCredential_StoredProcedure(CredentialSet cred, SqlConnection conn)
        {
            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.InsertCommand = new SqlCommand("Insert_Credential", conn);
                da.InsertCommand.CommandType = CommandType.StoredProcedure;
                da.InsertCommand.Parameters.Add(new SqlParameter("@notes", SqlDbType.VarChar));
                da.InsertCommand.Parameters.Add(new SqlParameter("@title", SqlDbType.VarChar));
                da.InsertCommand.Parameters["@notes"].Value = cred.Notes;
                da.InsertCommand.Parameters["@title"].Value = cred.Title;

                da.InsertCommand.ExecuteNonQuery();
            }

            return true;
        }
    }
}
