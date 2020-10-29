using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Services;
using SevenZip;

namespace DataBaseUtilities
{
    public class Zip
    {
        private const string _NPPWS = "@NoViN 09158980915_+";
        private string _Dir = "";
        private string _ZipName = "";
        //private bool _UI = false;

        private bool _Finished = false;
        private List<string> _Files = null;
        private bool _ExtractFinished = false;

        public Zip(string Dir, string ZipName, bool UI)
        {
            _Dir = Dir;
            _ZipName = ZipName;
        }

        public Zip()
        {
        }

        public Zip(List<string> Files, string ZipName, bool UI)
        {
            _ZipName = ZipName;
            _Files = Files;
        }

        public static string TempDirName()
        {
            int line = 0;
            try
            {
                line = 1;
                string tempDIR = Path.Combine(System.Windows.Forms.Application.StartupPath, "Temp");
                line = 2;
                if (!Directory.Exists(tempDIR))
                    Directory.CreateDirectory(tempDIR);
                line = 3;
                tempDIR = Path.Combine(tempDIR, Guid.NewGuid().ToString());
                line = 4;
                Directory.CreateDirectory(tempDIR);
                line = 5;
                return tempDIR;
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex, $"Line:{line}");
                return "";
            }
        }

        /// <summary>
        /// ورودی آدرس فایل خروجی آدرس پوشه ای که باید فشرده سازی شود
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static ReturnedSaveFuncInfoWithValue<string> Move2Temp(string FilePath)
        {
            var ret = new ReturnedSaveFuncInfoWithValue<string>();
            try
            {
                string tempDIR = TempDirName();
                FileInfo tempInfor = new FileInfo(FilePath);
                if (!File.Exists(FilePath))
                {
                    ret.AddReturnedValue(ReturnedState.Error, $"path not exists : {FilePath}");
                    return ret;
                }
                var retFilePath = Path.Combine(tempDIR, tempInfor.Name);
                File.Move(FilePath, retFilePath);
                ret.value = tempDIR;
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                ret.AddReturnedValue(ex);
            }
            return ret;
        }

        public bool Start()
        {
            SevenZipCompressor cmp = null;
            string archFileName = "";
            int line = 0;
            try
            {
                line = 1;
                //if (!System.IO.File.Exists(Application.StartupPath + "\\7z.dll"))
                //    System.IO.File.WriteAllBytes(Application.StartupPath + "\\7z.dll", BackUpDLL.Properties.Resources._7z);
                //line = 2;
                //SevenZipCompressor.SetLibraryPath(Application.StartupPath + "\\7z.dll");
                line = 3;
                cmp = new SevenZipCompressor();
                line = 4;
                line = 5;
                line = 6;
                cmp.CompressionFinished += new EventHandler<EventArgs>(cmp_CompressionFinished);
                line = 7;
                line = 10;
                cmp.VolumeSize = 0;
                line = 11;
                string directory = _Dir;
                line = 12;
                archFileName = _ZipName;
                line = 13;
                if (_Files == null)
                {
                    line = 14;
                    cmp.BeginCompressDirectory(directory, archFileName, _NPPWS);
                    line = 15;
                }
                else
                {
                    line = 16;
                    cmp.BeginCompressFilesEncrypted(archFileName, _NPPWS, _Files.ToArray());
                    line = 17;
                }
                line = 18;
                while (!_Finished)
                {
                    line = 19;
                    System.Threading.Thread.Sleep(100);

                    line = 20;
                    System.Windows.Forms.Application.DoEvents();
                    line = 21;
                }
                return true;
            }
            catch (ThreadAbortException)
            {
                cmp = null;
                if (System.IO.File.Exists(archFileName))
                    try
                    {
                        System.IO.File.Delete(archFileName);
                    }
                    catch (Exception)
                    {
                    }
                return false;
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex, $"Line:{line}");
                return false;
            }
        }

        void cmp_CompressionFinished(object sender, EventArgs e)
        {
            //l_CompressProgress.Text = "Finished";
            //pb_CompressWork.Style = ProgressBarStyle.Blocks;
            //pb_CompressProgress.Value = 0;
            _Finished = true;
            //System.Windows.Forms.MessageBox.Show(this,"Finished");
        }

        void cmp_Compressing(object sender, ProgressEventArgs e) { }


        public string ExtractTempDIR(string Pathf)
        {
            int line = 0;
            try
            {
                line = 1;
                //if (!File.Exists(Application.StartupPath + "\\7z.dll"))
                //    File.WriteAllBytes(Application.StartupPath + "\\7z.dll", Properties.Resources._7z);
                line = 2;
                SevenZipCompressor.SetLibraryPath(System.Windows.Forms.Application.StartupPath + "\\7z.dll");
                line = 3;
                string fileName = Pathf;
                line = 4;
                string directory = TempDirName();
                line = 5;
                string NewFileName = TempDirName() + "\\bk1.7z";
                line = 6;
                File.Copy(fileName, NewFileName);
                line = 7;
                var extr = new SevenZipExtractor(NewFileName, _NPPWS);
                line = 8;
                //ProgressBar1.Maximum = (int)extr.FilesCount;
                line = 9;
                line = 10;
                line = 11;
                line = 12;
                extr.BeginExtractArchive(directory);
                line = 13;
                while (!_ExtractFinished)
                {
                    line = 14;
                    Thread.Sleep(100);
                    line = 15;
                    System.Windows.Forms.Application.DoEvents();
                    line = 16;
                }
                line = 17;
                foreach (var item in Directory.GetFiles(directory))
                {
                    line = 18;
                    if (item.ToUpper().EndsWith(".BAK") || item.ToUpper().EndsWith(".NP"))
                        return item;
                    if (!item.EndsWith(".NPZ")) continue;
                    File.Move(item, Path.ChangeExtension(item, ".Bak"));
                    var pathBak = item.Replace(".NPZ", ".Bak");
                    return pathBak;
                }
                line = 19;
                return "";
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex, $"Error in Line {line}");
                return "";
            }
        }
    }
}
