using Services;

namespace BackUpDLL
{
    public class CreateDatabaseResult
    {
        public string ConnectionString { get; set; }
        public ReturnedSaveFuncInfo Result { get; set; } = new ReturnedSaveFuncInfo();
        public EnCreateDataBase Status { get; set; }
    }
}
