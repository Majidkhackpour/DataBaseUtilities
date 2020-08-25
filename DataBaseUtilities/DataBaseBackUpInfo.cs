using System;
using System.Data;
using System.Data.SqlClient;
using Services;

namespace BackUpDLL
{
    public class DataBaseBackUpInfo
    {
        public DataBaseBackUpInfo()
        {

        }
        public DataBaseBackUpInfo(string backUpAddress, string connectionString)
        {
            LoadData(backUpAddress, connectionString);
        }
        public void LoadData(SqlDataReader reader)
        {
            BackUpName = reader[0].ToString();
            BackUpDescription = reader[1].ToString();
            BackUpType = reader[2].ToString();
            ExpirationDate = reader[3].ToString();
            Compressed = reader[4].ToString();
            Position = reader[5].ToString();
            DeviceType = reader[6].ToString();
            UserName = reader[7].ToString();
            ServerName = reader[8].ToString();
            DatabaseName = reader[9].ToString();
            DatabaseVersion = reader[10].ToString();
            DatabaseCreationDate = reader[11].ToString();
            FristLsn = reader[12].ToString();
            LastLsn = reader[13].ToString();
            CheckPointLsn = reader[14].ToString();
            DatabaseBackUpLsn = reader[15].ToString();
            BackUpstartdate = reader[16].ToString();
            SortOrder = reader[17].ToString();
            CodePage = reader[18].ToString();
            UnicodeLocaleId = reader[19].ToString();
            UniCodeComparisonStyle = reader[20].ToString();
            CompatibilityLevel = reader[21].ToString();
            SoftwareVendoId = reader[22].ToString();
            SoftwareVersionMajor = reader[23].ToString();
            MachineName = reader[24].ToString();
            Flags = reader[25].ToString();
            BindingId = reader[26].ToString();
            RecoveryForkId = reader[27].ToString();
            Collation = reader[28].ToString();
            FamilyGuid = reader[29].ToString();
            HasBulkLoggeddata = reader[30].ToString();
            IsSnapshot = reader[31].ToString();
            IsReadOnly = reader[32].ToString();
            IsSingleUser = reader[33].ToString();
            HasBackUpChecksums = reader[34].ToString();
            IsDamaged = reader[35].ToString();
            BeginsLogChain = reader[36].ToString();
            HasIncompleteMetaData = reader[37].ToString();
            IsForcedOffline = reader[38].ToString();
            IsCopyOnly = reader[39].ToString();
            FirstRecoveryForkId = reader[40].ToString();
            ForkPointLsn = reader[41].ToString();
            RecoveryModel = reader[42].ToString();
            DifferentialBaseLsn = reader[43].ToString();
            DifferentialBaseGuid = reader[44].ToString();
            BackupTypeDescription = reader[45].ToString();
            BackupSetGuid = reader[46].ToString();

        }
        public string BackUpName { get; set; } = "";
        public string BackUpDescription { get; set; } = "";
        public string BackUpType { get; set; } = "";
        public string ExpirationDate { get; set; } = "";
        public string Compressed { get; set; } = "";
        public string Position { get; set; } = "";
        public string DeviceType { get; set; } = "";
        public string UserName { get; set; } = "";
        public string ServerName { get; set; } = "";
        public string DatabaseName { get; set; } = "";
        public string DatabaseVersion { get; set; } = "";
        public string DatabaseCreationDate { get; set; } = "";
        public string FristLsn { get; set; } = "";
        public string LastLsn { get; set; } = "";
        public string CheckPointLsn { get; set; } = "";
        public string DatabaseBackUpLsn { get; set; } = "";
        public string BackUpstartdate { get; set; } = "";
        public string SortOrder { get; set; } = "";
        public string CodePage { get; set; } = "";
        public string UnicodeLocaleId { get; set; } = "";
        public string UniCodeComparisonStyle { get; set; } = "";
        public string CompatibilityLevel { get; set; } = "";
        public string SoftwareVendoId { get; set; } = "";
        public string SoftwareVersionMajor { get; set; } = "";
        public string MachineName { get; set; } = "";
        public string Flags { get; set; } = "";
        public string BindingId { get; set; } = "";
        public string RecoveryForkId { get; set; } = "";
        public string Collation { get; set; } = "";
        public string FamilyGuid { get; set; } = "";
        public string HasBulkLoggeddata { get; set; } = "";
        public string IsSnapshot { get; set; } = "";
        public string IsReadOnly { get; set; } = "";
        public string IsSingleUser { get; set; } = "";
        public string HasBackUpChecksums { get; set; } = "";
        public string IsDamaged { get; set; } = "";
        public string BeginsLogChain { get; set; } = "";
        public string HasIncompleteMetaData { get; set; } = "";
        public string IsForcedOffline { get; set; } = "";
        public string IsCopyOnly { get; set; } = "";
        public string FirstRecoveryForkId { get; set; } = "";
        public string ForkPointLsn { get; set; } = "";
        public string RecoveryModel { get; set; } = "";
        public string DifferentialBaseLsn { get; set; } = "";
        public string DifferentialBaseGuid { get; set; } = "";
        public string BackupTypeDescription { get; set; } = "";
        public string BackupSetGuid { get; set; } = "";
        public string LogicalName { get; set; } = "";
        private void LoadData(string backUpAddress, string connectionString)
        {
            try
            {
                var cn = new SqlConnection(connectionString);

                var command = "RESTORE HEADERONLY FROM DISK =N'" + backUpAddress + "'";
                var cmd = new SqlCommand(command, cn) { CommandType = CommandType.Text };
                cn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    LoadData(reader);
                cmd.CommandText = "RESTORE FILELISTONLY FROM DISK =N'" + backUpAddress + "'";
                reader.Close();
                reader = cmd.ExecuteReader();
                reader.Read();
                LogicalName = reader[0].ToString();

                cn.Close();
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }
        }
    }
}
