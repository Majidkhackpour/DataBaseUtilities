using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Notification;
using Services;

namespace DataBaseUtilities
{
    public class RunScript
    {
        public static async Task<ReturnedSaveFuncInfo> RunAsync(IWin32Window owner,string Script, SqlConnection cn, SqlTransaction transaction = null, short tryCount = 2)
        {
            var ret = new ReturnedSaveFuncInfo();
            try
            {
                if (string.IsNullOrEmpty(Script)) return ret;

                string[] seperator = { "go\r", "go\r\n", "\ngo\n", "\ngo\r\n", "\tgo\t", " go ", "\rgo\r" };
                Script = Script.Replace("GO", "go").Replace("Go", "go").Replace("gO", "go");
                var scripts = Script.Split(seperator, StringSplitOptions.RemoveEmptyEntries);

                var cmd = new SqlCommand("", cn)
                {
                    CommandTimeout = 60 * 1000 * 2,//2دقیقه
                    CommandType = CommandType.Text
                };
                if (transaction != null)
                    cmd.Transaction = transaction;
                if (cn.State != ConnectionState.Open) cn.Open();

                var frm = new frmSplash(scripts.ToList().Count);
                frm.Show(owner);

                var index = 1;
                foreach (var item in scripts)
                {
                    index++;
                    frm.Level = index;

                    ret.AddReturnedValue(await ExecuteAsync(item, cmd));
                }
                frm.Close();
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                ret.AddReturnedValue(ex);
            }
            if (ret.HasError && tryCount > 0)
            {
                await Task.Delay(1000);
                return await RunAsync(owner,Script, cn, transaction, --tryCount);
            }
            return ret;
        }
        private static async Task<ReturnedSaveFuncInfo> ExecuteAsync(string item, SqlCommand cmd, short tryCount = 10)
        {
            var ret = new ReturnedSaveFuncInfo();
            try
            {
                cmd.CommandText = item;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (tryCount > 0)
                {
                    await Task.Delay(500);
                    return await ExecuteAsync(item, cmd, --tryCount);
                }
                WebErrorLog.ErrorInstence.StartErrorLog(ex, "error in command " + item ?? "");
                ret.AddReturnedValue(ReturnedState.Error, $"{ex.Message}\r\nQuery:\r\n{item}");
            }

            return ret;
        }
    }
}
