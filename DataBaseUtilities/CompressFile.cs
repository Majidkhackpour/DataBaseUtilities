using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Services;
using SharpCompress.Archives;
using SharpCompress.Archives.GZip;
using SharpCompress.Common;

namespace DataBaseUtilities
{
    public class CompressFile
    {
        public async Task<ReturnedSaveFuncInfo> CompressFileAsync(string PathForZipDirectory, string zipFilePath, CancellationToken token)
        {
            var ret = new ReturnedSaveFuncInfo();
            int line = 0;
            try
            {
                line = 1;
                zipFilePath = zipFilePath.ToLower();
                line = 2;
                var inf = new FileInfo(zipFilePath);
                line = 3;
                zipFilePath = inf.FullName.Replace(inf.Extension, ".npz2");
                using (var archive = ArchiveFactory.Create(ArchiveType.GZip))
                {
                    line = 48;
                    archive.AddAllFromDirectory(PathForZipDirectory);
                    line = 51;
                    token.ThrowIfCancellationRequested();
                    archive.SaveTo(zipFilePath, CompressionType.GZip);
                }

                line = 52;
                token.ThrowIfCancellationRequested();
                line = 53;
                Directory.Delete(PathForZipDirectory, true);
            }
            catch (OperationCanceledException ex) { ret.AddReturnedValue(ex); }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex, $"Line:{line}");
                ret.AddReturnedValue(ex);
            }
            return ret;
        }

        public async Task<ReturnedSaveFuncInfo> ExtractTempDIR(string archiveName)
        {
            var ret = new ReturnedSaveFuncInfo();
            try
            {
                var pathtemp = Zip.TempDirName();
                var fileInfo = new FileInfo(archiveName);
                pathtemp = Path.Combine(pathtemp, fileInfo.Name);
                pathtemp = Path.ChangeExtension(pathtemp, ".bak");
                using (var archive = GZipArchive.Open(archiveName))
                {
                    foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                    {
                        entry.WriteToFile(pathtemp, new ExtractionOptions()
                        {
                            ExtractFullPath = true,
                            Overwrite = true
                        });
                    }
                }
                ret.value = pathtemp;
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                ret.AddReturnedValue(ex);
            }
            return ret;
        }

        private class NestedCompressFile
        {
            internal static readonly CompressFile instance = new CompressFile();
            static NestedCompressFile()
            {
            }
        }

        public static CompressFile CompressFileInstance => NestedCompressFile.instance;
    }
}
