using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Services;

namespace DataBaseUtilities
{
    public class CLSServerDBs
    {
        public CLSServerDBs()
        {
        }

        public CLSServerDBs(SqlDataReader reader)
        {
            try
            {
                LoadData(reader);
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }
        }

        public CLSServerDBs(string connectionString)
        {
            try
            {
                LoadData(connectionString);
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }
        }

        public string Name { get; set; }
        public string DbId { get; set; }
        public string SId { get; set; }
        public string Mode { get; set; }
        public string Status { get; set; }
        public string Status2 { get; set; }
        public string CrDate { get; set; }
        public string Reserved { get; set; }
        public string Category { get; set; }
        public string CmptLevel { get; set; }
        public string FileName { get; set; }
        public string Version { get; set; }

        private void LoadData(string sqlConnectionString)
        {
            try
            {
                var csb = new SqlConnectionStringBuilder(sqlConnectionString);
                var serverSqlConnectionString =
                    sqlConnectionString.Replace(";Initial Catalog=" + csb.InitialCatalog + ";", ";Initial Catalog=;");
                var cn = new SqlConnection(serverSqlConnectionString);
                var cmd = new SqlCommand("Select * from sysdatabases where name='" + csb.InitialCatalog + "'", cn);
                cn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    LoadData(reader);
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }
        }

        private void LoadData(SqlDataReader reader)
        {
            try
            {
                Name = GetValue(reader, "name");
                DbId = GetValue(reader, "dbid");
                SId = GetValue(reader, "sid");
                Mode = GetValue(reader, "mode");
                Status = GetValue(reader, "status");
                Status2 = GetValue(reader, "status2");
                CrDate = GetValue(reader, "crdate");
                Reserved = GetValue(reader, "reserved");
                Category = GetValue(reader, "category");
                CmptLevel = GetValue(reader, "cmptlevel");
                FileName = GetValue(reader, "filename");
                Version = GetValue(reader, "version");
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }
        }

        private string GetValue(SqlDataReader dr, string name)
        {
            try
            {
                var col = dr.GetOrdinal(name);
                if (col < 0) return "";
                var tmp = dr.GetValue(col);
                return tmp == null ? "" : tmp.ToString();
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                return "";
            }
        }

        public static List<CLSServerDBs> GetAll(string serverCn)
        {
            var ret = new List<CLSServerDBs>();
            try
            {
                var cn = new SqlConnection();
                var csb = new SqlConnectionStringBuilder(serverCn) { InitialCatalog = "master" };
                cn.ConnectionString = csb.ConnectionString;
                var cmd = new SqlCommand("Select * from sysdatabases", cn);
                cn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        ret.Add(new CLSServerDBs(reader));
                    }
                    catch (Exception ex)
                    {
                        WebErrorLog.ErrorInstence.StartErrorLog(ex);
                    }
                }

                cn.Close();
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }
            return ret;
        }
    }
}
