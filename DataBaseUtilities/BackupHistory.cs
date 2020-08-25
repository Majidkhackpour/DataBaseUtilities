using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services;

namespace BackUpDLL
{
    public class BackupHistory
    {
        public Guid Guid { get; set; }
        public string DatabaseName { get; set; }
        public string Path { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public bool IsAutomatic { get; set; } = false;
        public bool IsSuccess { get; set; }
        public ENSource Source { get; set; }

        public static async Task<List<BackupHistory>> GetAllAsync()
        {
            var ret = new List<BackupHistory>();
            try
            {

            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }
            return ret;
        }

        public static async Task<ReturnedSaveFuncInfo> SaveAsync(BackupHistory item)
        {
            var ret = new ReturnedSaveFuncInfo();
            try
            {

            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                ret.AddReturnedValue(ex);
            }
            return ret;
        }

        public static async Task<ReturnedSaveFuncInfo> DeleteOldBackupsAsync()
        {
            var ret = new ReturnedSaveFuncInfo();
            try
            {

            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                ret.AddReturnedValue(ex);
            }
            return ret;
        }
    }
}
