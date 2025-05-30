using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MazraeatiBackOffice.Dto;

namespace MazraeatiBackOffice.Configuration
{
    public class LogFile
    {
        static SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(3);

        public LogFile()
        {

        }

        public void LogCustomInfo(string methodName , string data)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Log Time : {DateTime.Now.ToString("hh:MM:ss tt")}");
            stringBuilder.AppendLine();
            stringBuilder.Append("---------------------- start Log here / Information ---------------------------------- ");
            stringBuilder.AppendLine();
            stringBuilder.Append(" method name :");
            stringBuilder.AppendLine();
            stringBuilder.Append($" {methodName}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();
            stringBuilder.Append($" data is :");
            stringBuilder.AppendLine();
            stringBuilder.Append($" {data}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();

            stringBuilder.Append("---------------------- end Log here / Information ---------------------------------- ");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();

            WriteLog(stringBuilder.ToString());

        }

        public void LogCustomError(ExceptionDto exceptionDto)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Log Time : {DateTime.Now.ToString("hh:MM:ss tt")}");
            stringBuilder.AppendLine();
            stringBuilder.Append("---------------------- start Log here / Exception ---------------------------------- ");
            stringBuilder.AppendLine();
            stringBuilder.Append(" method name :");
            stringBuilder.AppendLine();
            stringBuilder.Append($" {exceptionDto.MethodName}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();
            stringBuilder.Append($" exception => message :");
            stringBuilder.AppendLine();
            stringBuilder.Append($" {exceptionDto.exception.Message}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();
            stringBuilder.Append($" exception => stackTrace :");
            stringBuilder.AppendLine();
            stringBuilder.Append($" {exceptionDto.exception.StackTrace}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();
            stringBuilder.Append($" exception => InnerException :");
            stringBuilder.AppendLine();
            stringBuilder.Append($" {exceptionDto.exception.InnerException}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();

            stringBuilder.Append("---------------------- end Log here / Exception ---------------------------------- ");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();

            WriteLog(stringBuilder.ToString());

        }

        private async Task WriteLog(string Data)
        {
            // declaration area
            string folderPath = "C:/FarmerCrashLog/";

            await _semaphoreSlim.WaitAsync();

            // check if folder is exists
            if (Directory.Exists(folderPath))
            {
                // check if has file
                if (Directory.GetFiles(folderPath).Length > 0)
                {
                    // get last file with lastWriteTime property
                    DirectoryInfo directory = new DirectoryInfo(folderPath);

                    FileInfo _file = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();

                    // substract between two date 
                    TimeSpan duration = DateTime.Now - _file.CreationTime;

                    // create file every day
                    if ((duration.TotalHours) > 24)
                    {
                        // get number of file that have same name
                        int nSameNameLength = directory.GetFiles().Where(f => f.Name == $"log_{DateTime.Now.ToString("dd-MM-yyyy")}").Count() + 1;

                        // create new text
                        string file = Path.Combine(folderPath, $"log_{DateTime.Now.ToString("dd-MM-yyyy")}_{nSameNameLength.ToString().PadLeft(4, '0')}.txt");

                        using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            StreamWriter write = new StreamWriter(fs);
                            write.BaseStream.Seek(0, SeekOrigin.End);
                            write.WriteLine(Environment.NewLine);
                            write.WriteLine(Data);
                            write.Flush();
                            write.Close();
                            fs.Close();
                        }

                        //File.WriteAllText(file, Data);
                    }
                    else if ((_file.Length / 1024) > 5) // more than 5 mega
                    {
                        // get number of file that have same name
                        int nSameNameLength = directory.GetFiles().Where(f => f.Name == $"log_{DateTime.Now.ToString("dd-MM-yyyy")}").Count() + 1;

                        // create new text
                        string file = Path.Combine(folderPath, $"log_{DateTime.Now.ToString("dd-MM-yyyy")}_{nSameNameLength.ToString().PadLeft(4, '0')}.txt");

                        using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            StreamWriter write = new StreamWriter(fs);
                            write.BaseStream.Seek(0, SeekOrigin.End);
                            write.WriteLine(Environment.NewLine);
                            write.WriteLine(Data);
                            write.Flush();
                            write.Close();
                            fs.Close();
                        }

                        //File.WriteAllText(file, Data);
                    }
                    else
                    {
                        // write on exists file
                        string file = Path.Combine(folderPath, _file.FullName);

                        using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            StreamWriter write = new StreamWriter(fs);
                            write.BaseStream.Seek(0, SeekOrigin.End);
                            write.WriteLine(Environment.NewLine);
                            write.WriteLine(Data);
                            write.Flush();
                            write.Close();
                            fs.Close();
                        }

                        //File.AppendAllText(file, Data);
                    }

                }
                else
                {
                    // create new text
                    string file = Path.Combine(folderPath, $"log_{DateTime.Now.ToString("dd-MM-yyyy")}.txt");

                    using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        StreamWriter write = new StreamWriter(fs);
                        write.BaseStream.Seek(0, SeekOrigin.End);
                        write.WriteLine(Environment.NewLine);
                        write.WriteLine(Data);
                        write.Flush();
                        write.Close();
                        fs.Close();
                    }

                    //File.WriteAllText(file, Data);
                }

            }
            else
            {
                // create new file
                Directory.CreateDirectory(folderPath);

                // create new text
                string file = Path.Combine(folderPath, $"log_{DateTime.Now.ToString("dd-MM-yyyy")}.txt");

                using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    StreamWriter write = new StreamWriter(fs);
                    write.BaseStream.Seek(0, SeekOrigin.End);
                    write.WriteLine(Environment.NewLine);
                    write.WriteLine(Data);
                    write.Flush();
                    write.Close();
                    fs.Close();
                }

                //File.WriteAllText(file, Data);
            }


            _semaphoreSlim.Release();


        }
    }
}
