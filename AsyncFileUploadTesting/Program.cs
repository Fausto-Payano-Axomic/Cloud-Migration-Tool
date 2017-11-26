using Cloud_Migration_Tool.Helper_Classes;
using Cloud_Migration_Tool.Models;
using OpenAsset.RestClient.Library;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;

namespace ThreadedFileUploadTesting {
    class Program {

        private static Connection conn;

        [STAThread]
        static void Main(string[] args) {
            TestingAsyncFileUpload();
        }

        private static void TestingAsyncFileUpload() {


            var fileOpenDialog = new OpenFileDialog();
            Parser reader = new Parser();
            fileOpenDialog.ShowDialog();
            List<FileToBeMigrated> fileList = new List<FileToBeMigrated>(reader.Parse(fileOpenDialog.FileName));
            foreach (var file in fileList) {
                Console.WriteLine(file.FilePath);
            }


            #region Logging in
            string username = "{USERNAME}";
            string password;
            string host = "https://demo-fpa.openasset.com";
            var tryingToLogIn = true;

            do {
                Console.WriteLine($"Connecting to {host}. Username is: {username} - please type the password now ");
                password = Console.ReadLine();
                try {
                    conn = Connection.GetConnection(host, username, password);
                    var result = conn.ValidateCredentials(0);
                    if (conn.ValidateCredentials(0)) {
                        Console.WriteLine("Successfully logged in");
                        tryingToLogIn = false;
                    }
                    else {
                        Console.WriteLine("Was unable to log in -> Please try again.");
                    }



                }
                catch (Exception failedLogin) {
                    Console.WriteLine($"Failed to login: \n {failedLogin}");
                }
            } while (tryingToLogIn);

            #endregion

            //StartMultiThreadedUploading(fileList.OrderBy(f => new FileInfo(f.FilePath).Length).ToList());
            StartMultiThreadedUploading(fileList);
            Console.ReadLine();
        }

        private static void StartMultiThreadedUploading(List<FileToBeMigrated> fileList) {
            int maxConcurrency = 10;
            Stopwatch mainTimer = new Stopwatch();
            mainTimer.Start();
            using (SemaphoreSlim concurrencySemaphore = new SemaphoreSlim(maxConcurrency)) {

                List<Task> tasks = new List<Task>();
                foreach (var file in fileList) {
                    
                    Stopwatch timer = new Stopwatch();

                    concurrencySemaphore.Wait();
                    var t = Task.Factory.StartNew(() => {
                        try {
                            timer.Start();
                            var fileInfo = new FileInfo(file.FilePath);
                            OpenAsset.RestClient.Library.Noun.File fileUpload = new OpenAsset.RestClient.Library.Noun.File() {
                                OriginalFilename = fileInfo.Name,
                                CategoryId = 1,
                                ProjectId = 10
                            };
                            conn.SendObject(fileUpload, file.FilePath, true);
                        }
                        catch(RESTAPIException ex) {
                            Console.WriteLine((ex as OpenAsset.RestClient.Library.RESTAPIException).ErrorObj);
                        }
                        catch(Exception oddEx) {
                            Console.WriteLine(oddEx);
                        }
                        finally {
                            timer.Stop();
         
                            Console.WriteLine($"File - {file.FilePath} Processed. It took {timer.Elapsed}");
                            concurrencySemaphore.Release();
                        }
                    });
                    tasks.Add(t);
                }

                Task.WaitAll(tasks.ToArray());
            }
            mainTimer.Stop();
            Console.WriteLine(mainTimer.Elapsed);

        }
    }
}
