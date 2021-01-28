using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache.Bussines;
using Services;

namespace DataBaseUtilities
{
    public class DatabaseAction
    {
        public static event EventHandler<CreateBackupArgs> OnCreateBackup;

        public static async Task<ReturnedSaveFuncInfo> BackUpLogAsync(Guid guid, EnBackUpType type, EnBackUpStatus status, string path, string desc)
        {
            var res = new ReturnedSaveFuncInfo();
            try
            {
                var fi = new FileInfo(path);
                var size = (long)0;
                try
                {
                    if (File.Exists(path))
                    {
                        size = fi.Length;
                        size /= 1000000;
                    }
                }
                catch { }


                var log = await BackUpLogBussines.GetAsync(guid) ?? new BackUpLogBussines()
                {
                    Guid = guid,
                    Modified = DateTime.Now,
                    Status = true,
                    InsertedDate = DateTime.Now
                };

                log.BackUpStatus = status;
                log.Path = path;
                log.Type = type;
                log.StatusDesc = desc;
                log.Size = (short)size;

                await log.SaveAsync();
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                res.AddReturnedValue(ex);
            }

            return res;
        }

        public static string CreateFileName(string connectionString, string directory = "")
        {
            var ret = "";
            try
            {
                var csd = new SqlConnectionStringBuilder(connectionString);
                var FileName =
                    $"{csd.InitialCatalog}__{Calendar.MiladiToShamsi(DateTime.Now).RemoveNoNumbers("-")}__{DateTime.Now.Hour}_{DateTime.Now.Minute}.NP";
                if (string.IsNullOrEmpty(directory))
                    directory = Path.Combine(Application.StartupPath, "TempBackUp");
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                ret = Path.Combine(directory, FileName);
            }
            catch (Exception ex) { WebErrorLog.ErrorInstence.StartErrorLog(ex); }
            return ret;
        }

        public static async Task<ReturnedSaveFuncInfoWithValue<string>> BackupDbAsync(string connectionString, ENSource source, EnBackUpType type, string path = "")
        {
            var line = 0;
            var res = new ReturnedSaveFuncInfoWithValue<string>();
            bool IsAutomatic = string.IsNullOrEmpty(path);
            string DatabaseName = "";
            var guid = Guid.NewGuid();
            try
            {

                OnCreateBackup?.Invoke(null, new CreateBackupArgs() { Message = "", State = CreateBackupState.Start });

                if (IsAutomatic) path = CreateFileName(connectionString);
                if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                {
                    //در نرم افزار حسابداری آدرس پوشه پشتیبان گیری اتوماتیک داده شده است
                    //باید فایل پشتیبان در این پوشه ایجاد گردد
                    path = CreateFileName(connectionString, path);
                    IsAutomatic = true;
                }

                await BackUpLogAsync(guid, type, EnBackUpStatus.Pending, path,
                    "آغاز فرآیند تهیه فایل پشتیبان");

                //تهیه اولین نسخه بکاپ توسط سرویس اس کیو ال
                var CreateSqlBackuResult = await CreateSqlServerBackupFileAsync(path, connectionString, guid, type);
                DatabaseName = CreateSqlBackuResult.value;
                res.AddReturnedValue(CreateSqlBackuResult);

                var PathForZipDirectory = await Zip.Move2Temp(path, guid, type);
                res.AddReturnedValue(PathForZipDirectory);

                try
                {
                    //please dont remove this try
                    //اگر به خطا خورد باید تابع ادامه پیدا کند و پشتیبان با پسوند .بک تهیه شود
                    res.AddReturnedValue(
                        await CompressFile.CompressFileInstance.CompressFileAsync(PathForZipDirectory.value, path, guid,
                            type));
                }
                catch (Exception ex)
                {
                    WebErrorLog.ErrorInstence.StartErrorLog(ex);
                    res.AddReturnedValue(ex);
                }
                res.value = path.ToLower().Replace(".npz2", ".np").Replace(".npz", ".np").Replace(".np", ".npz2");

                //بنا به هر دلیلی امکان فشرده کردن فایل وجود نداشته است
                //باید همان بکاپ فشرده نشده به آدرس مقصد درخواستی مشتری متقل گردد
                if (res.HasError)
                {
                    var file = Directory.GetFiles(PathForZipDirectory.value).FirstOrDefault();
                    File.Move(file, path.ToLower().Replace(".npz2", ".np").Replace(".npz", ".np"));
                }

                OnCreateBackup?.Invoke(null,
                           new CreateBackupArgs() { Message = "", State = CreateBackupState.End });

                await BackUpLogAsync(guid, type, EnBackUpStatus.Success, path.ToLower(),
                    "عملیات پشتیبان گیری با موفقیت انجام شد");
            }
            catch (OperationCanceledException ex)
            {
                res.AddReturnedValue(ex);
            }
            catch (Exception ex)
            {
                await BackUpLogAsync(guid, type, EnBackUpStatus.Error, path.ToLower().Replace(".npz2", ".np").Replace(".npz", ".np"),
                    ex.Message);
                WebErrorLog.ErrorInstence.StartErrorLog(ex, $"Error in Line {line}");
                res.AddReturnedValue(ex);
            }
            finally
            {
                Task.Run(DeleteTempsAsync);
            }
            return res;
        }

        private static async Task DeleteTempsAsync()
        {
            try
            {
                await Task.Delay(60 * 1000);
                string tempDIR = Path.Combine(Application.StartupPath, "Temp");
                if (Directory.Exists(tempDIR))
                    Directory.Delete(tempDIR, true);
            }
            catch (OperationCanceledException) { }
            catch (ThreadAbortException) { }
            catch (Exception ex) { WebErrorLog.ErrorInstence.StartErrorLog(ex); }
        }

        private static async Task<ReturnedSaveFuncInfoWithValue<string>> CreateSqlServerBackupFileAsync(string path, string connectionString, Guid guid, EnBackUpType type)
        {
            var ret = new ReturnedSaveFuncInfoWithValue<string>();
            string commandText = "";
            try
            {
                var inf = new FileInfo(path);
                if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(inf.Name) || !Directory.Exists(inf.DirectoryName))
                {
                    var msg = $"آدرس {path} موجود یا معتبر نیست.";
                    await BackUpLogAsync(guid, type, EnBackUpStatus.Error, path, msg);
                    ret.AddReturnedValue(ReturnedState.Error, msg);
                    return ret;
                }
                var cn = new SqlConnection(connectionString);
                ret.value = new SqlConnectionStringBuilder(connectionString).InitialCatalog;
                var cmd = new SqlCommand($"Backup database [{ret.value }] to disk='{path}'", cn) { CommandType = CommandType.Text };
                commandText = cmd.CommandText;
                //تنها جای مجاز برای افزایش تایم اوت هست
                cmd.CommandTimeout = 5 * 60 * 1000;//5 minutes

                await cn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                cn.Close();
                await BackUpLogAsync(guid, type, EnBackUpStatus.Success, path, "پشتیبان اولیه با پسوند .bak تهیه شد");
                commandText = "";
            }
            catch (SqlException ex)
            {
                var msg = $"خطا در تهیه نسخه پشتیبان اطلاعات \r\nحساب کاربری SQL دسترسی به مسیر نصب برنامه ندارد\r\n{ex.Message}//{ex?.InnerException?.Message ?? ""}";
                await BackUpLogAsync(guid, type, EnBackUpStatus.Error, path, msg);
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                ret.AddReturnedValue(ReturnedState.Error, msg);
            }
            catch (Exception ex)
            {
                await BackUpLogAsync(guid, type, EnBackUpStatus.Error, path, ex.Message);
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                ret.AddReturnedValue(ex);
            }
            return ret;
        }

        public static async Task<ReturnedSaveFuncInfo> ReStoreDbAsync(string connectionString, string pathf, bool autoBackup, ENSource Source)
        {
            var ret = new ReturnedSaveFuncInfo();
            var cn = new SqlConnection();
            try
            {
                GC.Collect();
                SqlConnection.ClearAllPools();

                if (autoBackup)
                {

                    var dir = Application.StartupPath + "\\AradBackUp";
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    var file = Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly()?.Location)?.Replace(".exe", "__");
                    var d = Calendar.MiladiToShamsi(DateTime.Now).Replace("/", "_");
                    d += "__" + DateTime.Now.Hour + " - " + DateTime.Now.Minute;
                    file += d;

                    var filepath = dir + "\\" + file + ".Bak";
                    ret.AddReturnedValue(await BackupDbAsync(connectionString, Source, EnBackUpType.Manual, filepath));
                }

                if (pathf.EndsWith(".NPZ") || pathf.EndsWith(".npz"))
                {
                    var zp = new Zip();
                    pathf = zp.ExtractTempDIR(pathf);
                }
                else if (pathf.EndsWith(".NPZ2") || pathf.EndsWith(".npz2"))
                {
                    try
                    {
                        var zp = new CompressFile();
                        var res = await zp.ExtractTempDIR(pathf);
                        ret.AddReturnedValue(res);
                        if (res.HasError) return ret;
                        pathf = res.value;
                    }
                    catch
                    {
                        var zp = new Zip();
                        pathf = zp.ExtractTempDIR(pathf);
                    }
                }

                var csb = new SqlConnectionStringBuilder(connectionString);
                var serverConnectionString =
                    connectionString.Replace("Initial Catalog=" + csb.InitialCatalog, "Initial Catalog=");
                cn = new SqlConnection(serverConnectionString) { ConnectionString = serverConnectionString };
                var cmd = new SqlCommand("RESTORE DATABASE [dbName] FROM  DISK = N'bkPath' WITH  FILE = 1,  MOVE N'novin' TO N'dbMDFPath',  MOVE N'novin_log' TO N'dbLDFPath',   NOUNLOAD,  REPLACE,  STATS = 5", cn);
                var dbInfo = new CLSServerDBs(connectionString);
                var backUpInfo = new DataBaseBackUpInfo(pathf, cn.ConnectionString);

                cmd.CommandText = cmd.CommandText.Replace("dbName", csb.InitialCatalog);
                cmd.CommandText = cmd.CommandText.Replace("bkPath", pathf);
                cmd.CommandText = cmd.CommandText.Replace("novin", backUpInfo.LogicalName);
                cmd.CommandText = cmd.CommandText.Replace("novin_log", backUpInfo.LogicalName + "_log.ldf");
                cmd.CommandText = cmd.CommandText.Replace("dbMDFPath", dbInfo.FileName);
                cmd.CommandText =
                    cmd.CommandText.Replace("dbLDFPath", dbInfo.FileName.ToLower().Replace(".mdf", "_log.Ldf"));

                //تایم اوت بالا بردن مجاز است
                cmd.CommandTimeout = 5 * 60 * 1000;//5 minutes

                cn.Open();
                var temp = cmd.CommandText;
                cmd.CommandText = "ALTER DATABASE [" + csb.InitialCatalog + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                cmd.ExecuteNonQuery();

                cmd.CommandText = temp;
                cmd.ExecuteNonQuery();

                // cmd.CommandText = NovinPardaz.Properties.Resources.DatabaseSettings;
                cmd.CommandText = cmd.CommandText.Replace("dbName", csb.InitialCatalog);
                cmd.ExecuteNonQuery();

                cmd.CommandText = "ALTER DATABASE [" + csb.InitialCatalog + "] SET Multi_USER";
                cmd.ExecuteNonQuery();
                cn.Close();

                SqlConnection.ClearAllPools();
                Application.DoEvents();
            }
            catch (SqlException ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                try { cn.Close(); } catch { }
                ret.AddReturnedValue(ReturnedState.Error, $"حساب کاربری SQL دسترسی به مسیر نصب برنامه ندارد\r\n{ex.Message }//{ex?.InnerException?.Message ?? ""}");
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                try { cn.Close(); } catch { }
                ret.AddReturnedValue(ex);
            }
            Task.Run(() => SetMultiUser(connectionString));
            return ret;
        }

        public static void SetMultiUser(string connectionString)
        {
            try
            {
                SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder(connectionString);
                string ServerConnectionString = connectionString.Replace("Initial Catalog=" + csb.InitialCatalog, "Initial Catalog=");
                SqlConnection cn = new SqlConnection(ServerConnectionString);
                SqlCommand cmd = new SqlCommand("ALTER DATABASE [" + csb.InitialCatalog + "] SET Multi_USER", cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }
        }

        public static CreateDatabaseResult CreateDataBase(string dbName)
            => CreateDataBase(dbName, Settings.ServerConnectionsString);

        public static CreateDatabaseResult CreateDataBase(string dbName, string serverConnectionString)
        {
            var ret = new CreateDatabaseResult();
            if (string.IsNullOrEmpty(dbName))
            {
                ret.Status = EnCreateDataBase.DatabaseNameEmpty;
                ret.Result.AddReturnedValue(ReturnedState.Error, $"نام بانک اطلاعاتی خالی است.");
                return ret;
            }

            if (string.IsNullOrEmpty(serverConnectionString))
            {
                ret.Status = EnCreateDataBase.ServerConnectionStringError;
                ret.Result.AddReturnedValue(ReturnedState.Error, $"رشته اتصال به سرور خالی است.");
                return ret;
            }

            try
            {
                var cbs = new SqlConnectionStringBuilder(serverConnectionString) { InitialCatalog = "master" };

                if (IsExistDatabase(dbName, cbs.ConnectionString))
                {
                    ret.Result.AddReturnedValue(ReturnedState.Error, $"بانک اطلاعاتی با نام {dbName} \r\nدر سرویس {serverConnectionString}\r\n در حال حاضر وجود دارد.");
                    ret.Status = EnCreateDataBase.DatabaseExists;
                    return ret;
                }

                var cn = new SqlConnection(cbs.ConnectionString);
                var cmd = new SqlCommand("", cn) { CommandType = CommandType.Text };
                var dbPath = Application.StartupPath + "\\DataBase";
                if (Settings.SetDistanationFolder(cn.ConnectionString))
                {
                    cmd.CommandText = "CREATE DATABASE [DBName] ON  PRIMARY ( NAME = N'DBName'" +
                                  ",FILENAME = N'DBPath\\DBName.mdf', SIZE = 10240KB , FILEGROWTH = 10240KB )" +
                                  "LOG ON ( NAME = N'DBName_log',FILENAME = N'DBPath\\DBName_log.ldf'" +
                                  ",SIZE=10240KB,FILEGROWTH=10%)";
                    cmd.CommandText = cmd.CommandText.Replace("DBName", dbName);
                    cmd.CommandText = cmd.CommandText.Replace("DBPath", dbPath);
                }
                else
                {
                    cmd.CommandText = "CREATE DATABASE [DBName]";
                    cmd.CommandText = cmd.CommandText.Replace("DBName", dbName);
                }

                var constr = cn.ConnectionString.Replace("master", dbName);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                ret.ConnectionString = constr;
                ret.Status = EnCreateDataBase.Success;
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("because it already exists".ToLower()))
                    ret.Status = EnCreateDataBase.DatabaseExists;
                else
                    WebErrorLog.ErrorInstence.StartErrorLog(ex);
                ret.Result.AddReturnedValue(ex);
            }
            return ret;
        }

        public static bool IsExistDatabase(string dbName, string connectionString)
        {
            try
            {
                var cn = new SqlConnection(connectionString);
                var cmd = new SqlCommand("SELECT case when exists (select 1 from sys.Databases where Name = @DbName) then 1 else 0 end as DbExists", cn);
                cmd.Parameters.AddWithValue("@DbName", dbName);
                cn.Open();
                var exist = int.Parse(cmd.ExecuteScalar().ToString());
                cn.Close();
                return exist != 0;
            }
            catch (Exception e)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(e);
                return false;
            }
        }
    }
}
