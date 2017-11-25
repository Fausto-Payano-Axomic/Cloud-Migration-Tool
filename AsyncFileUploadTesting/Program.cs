using Cloud_Migration_Tool.Helper_Classes;
using Cloud_Migration_Tool.Models;
using OpenAsset.RestClient.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncFileUploadTesting {
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
            string username = "{USERNAME HERE}";
            string password;
            string host = "https://accountmanagers.openasset.com";
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

            StartMultiThreadedUploading(fileList);
        }

        private static void StartMultiThreadedUploading(List<FileToBeMigrated> fileList) {
            int maxConcurrency = 10;
            using (SemaphoreSlim concurrencySemaphore = new SemaphoreSlim(maxConcurrency)) {
                List<Task> tasks = new List<Task>();
                foreach (var file in fileList) {
                    concurrencySemaphore.Wait();
                    var t = Task.Factory.StartNew(() => {
                        try {
                            var fileInfo = new FileInfo(file.FilePath);
                            OpenAsset.RestClient.Library.Noun.File fileUpload = new OpenAsset.RestClient.Library.Noun.File() {
                                OriginalFilename = fileInfo.Name,
                                CategoryId = 1,
                                ProjectId = 150
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
                            Console.WriteLine($"File - {file.FilePath} Processed.");
                            concurrencySemaphore.Release();
                        }
                    });
                    tasks.Add(t);
                }

                Task.WaitAll(tasks.ToArray());
            }
        }
    }
}
