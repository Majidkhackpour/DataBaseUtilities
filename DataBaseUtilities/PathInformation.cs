namespace DataBaseUtilities
{
    public class PathInformation
    {
        private string _path;
        public string Path
        {
            get
            {
                try
                {
                    if (!System.IO.Directory.Exists(_path))
                        System.IO.Directory.CreateDirectory(_path);
                    return _path;
                }
                catch
                {
                    return "";
                }
            }
            set => _path = value;
        }
        public System.IO.DriveInfo DriveInfo { get; set; }
    }
}
