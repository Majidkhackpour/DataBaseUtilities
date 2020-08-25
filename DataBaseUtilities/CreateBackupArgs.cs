namespace DataBaseUtilities
{
    public enum CreateBackupState
    {
        Start, End, Error
    }
    public class CreateBackupArgs
    {

        public string Message { get; internal set; }
        public CreateBackupState State { get; set; }
    }
}
