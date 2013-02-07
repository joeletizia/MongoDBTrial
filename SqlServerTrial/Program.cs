using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SqlServerTrial
{
    class Program
    {
        static void Main(string[] args)
        {
            using (TrialModelContainer db = new TrialModelContainer())
            {
                var all_creds = db.Credentials1.ToList();

                all_creds.ForEach(db.Credentials1.DeleteObject);
                db.SaveChanges();

                Stopwatch watch = new Stopwatch();

                watch.Start();

                for (int i = 0; i < 1000000; i++)
                {
                    Credential cred = new Credential
                        {
                            LastUpdated = DateTime.Now,
                            Notes = String.Format("Joe's notes {0}", i),
                            Title = string.Format("Joe's creds {0}", i)
                        };
                    db.AddToCredentials1(cred);
                    db.SaveChanges();
                }

                watch.Stop();

                Console.WriteLine(String.Format("Elapsed time: {0}", watch.Elapsed));
            }

            Console.Read();
        }
    }
}
