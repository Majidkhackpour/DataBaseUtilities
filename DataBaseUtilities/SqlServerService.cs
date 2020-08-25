using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using Services;

namespace DataBaseUtilities
{
   public class SqlServerService
    {
        public static (bool, List<string>) IsSqlServiceOk(string connectionstring)
        {
            if (string.IsNullOrEmpty(connectionstring))
                throw new ArgumentNullException("connectionstring");
            try
            {
                var conn = new SqlConnectionStringBuilder(connectionstring);
                if (string.IsNullOrEmpty(conn.DataSource))
                    return (false, new List<string>() { "رشته اتصال نامعتبر است" });
                var isIp = IPAddress.TryParse(conn.DataSource, out var remoteIp);
                if (isIp && conn.DataSource != "127.0.0.1")
                {
                    return !RemoteServerIsAlive(remoteIp)
                        ? (false, new List<string>() {"سیستم میزبان sql server در دسترس نیست"})
                        : CheckConnection(connectionstring);
                }
                var mc = new ManagedComputer();
                var servicecoll = mc.Services;
                var messages = new List<string>();
                var errorInStartService = false;
                var haveValisSqlService = false;
                foreach (Service serv in servicecoll)
                {
                    if (!serv.Type.Equals(ManagedServiceType.SqlServer)) continue;
                    haveValisSqlService = true;
                    var status = WindowsServiceHelper.GetServiceStatus(serv.Name);
                    if (status == ServiceState.Unknown)
                        messages.Add($"سرویس {serv.DisplayName} پیدا نشد.");
                    else if (status == ServiceState.Stopped)
                    {
                        try
                        {
                            WindowsServiceHelper.StartService(serv.Name);
                            messages.Add($"سرویس {serv.DisplayName} اجرا شد.");
                            errorInStartService = true;
                        }
                        catch (Exception ex)
                        {
                            messages.Add($" خطا در اجرای سرویس {serv.DisplayName}.");
                            messages.Add(ex.Message);
                            errorInStartService = true;
                        }
                    }
                    else
                        errorInStartService = true;
                    Console.WriteLine(serv.DisplayName);
                }
                if (!haveValisSqlService)
                    messages.Add("سرویس sql پیدا نشد");
                return (errorInStartService, messages);
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                return (false, new List<string>() { ex.Message });
            }
        }

        private static (bool, List<string>) CheckConnection(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    connection.Close();
                    return (true, new List<string>());
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToLower().Contains("A network-related or instance-specific"))
                        return (false, new List<string>() { "سرور در دسترس نیست" });
                    if (ex.Message.Contains("but then an error occurred during the login process"))
                        return (false, new List<string>() { "نام کاربری یا رمز عبور اشتباه است" });
                    return (false, new List<string>() { ex.Message });
                }
            }
        }

        private static bool RemoteServerIsAlive(IPAddress remoteIp)
        {
            try
            {
                var pingable = false;
                using (var ping = new System.Net.NetworkInformation.Ping())
                {
                    var replay = ping.Send(remoteIp);
                    pingable = replay.Status == System.Net.NetworkInformation.IPStatus.Success;
                }
                return pingable;
            }
            catch
            {
                return false;
            }

        }
    }
}
