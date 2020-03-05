using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
#if NET452
using FlagFtp;
#elif NETCOREAPP2_2
using CoreFtp;
using Microsoft.AspNetCore.Http;
#endif

namespace Headstone.Framework.Common.Extensions
{
    public static class StreamExtentions
    {
        public static void UploadToFtp(this Stream file, string uploadFolderPath, string uploadPath, string userName, string password, bool? usePassive = false)
        {
            Upload(file, uploadFolderPath, uploadPath, userName, password, usePassive);
        }

#if NET452
        public static List<string> UploadToFtp(this HttpFileCollectionBase file, string uploadFolderPath, string uploadPath, string userName, string password, bool? usePassive = false)
        {
            try
            {
                var result = new List<string>();
                for (int i = 0; i < file.Count; i++)
                {
                    if (file[i] != null)
                    {
                        var item = file[i];
                        string fileName = item.FileName;
                        result.Add(fileName);
                        Upload(file[i].InputStream, uploadFolderPath, uploadPath + "/" + fileName, userName, password, usePassive);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public static Byte[] ToByteArray(this HttpPostedFileBase value)
        {
            if (value == null)
                return null;
            var array = new Byte[value.ContentLength];
            value.InputStream.Position = 0;
            value.InputStream.Read(array, 0, value.ContentLength);
            return array;
        }
#endif

        public static void Upload(Stream file, string directoryUrl, string fileUrl, string userName, string password, bool? usePassive = false)
        {
#region Folder

            //replace last slash
            fileUrl = fileUrl.EndsWith("/") ? fileUrl.Substring(0, fileUrl.Length - 1) : fileUrl;
            directoryUrl = directoryUrl.EndsWith("/") ? directoryUrl.Substring(0, directoryUrl.Length - 1) : directoryUrl;

#if NET452
            // Set the ftp credentials
            var credential = new NetworkCredential(userName, password);

            // Create a ftp client
            var client = new FtpClient(credential);
#elif NETCOREAPP2_2
            // Set the ftp credentials
            var config = new FtpClientConfiguration
            {
                Username = userName,
                Password = password
            };
            // Create a ftp client
            var client = new FtpClient(config);
#endif

#if NET452
            // Check for folder existence
            if (!client.DirectoryExists(new Uri(directoryUrl)))
            {
#endif
            try
            {
                    // Create the ftp request object
                    FtpWebRequest ftpReq = (FtpWebRequest)FtpWebRequest.Create(new Uri(directoryUrl));
                    ftpReq.Credentials = new NetworkCredential(userName, password);
                    ftpReq.Method = WebRequestMethods.Ftp.MakeDirectory;

                    // Create the ftp response object
                    FtpWebResponse ftpRes = (FtpWebResponse)ftpReq.GetResponse();
                }
                catch (Exception)
                {
                }
#if NET452
            }
#endif

            #endregion

            #region File

            // Create FtpWebRequest object from the Uri provided
            System.Net.FtpWebRequest ftpWebRequest = (System.Net.FtpWebRequest)System.Net.FtpWebRequest.Create(new Uri(fileUrl));

            // Provide the WebPermission Credintials
            ftpWebRequest.Credentials = new System.Net.NetworkCredential(userName, password);
            ftpWebRequest.ImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.None;

            // set timeout for 20 seconds
            ftpWebRequest.Timeout = 20000;

            // set transfer mode
            ftpWebRequest.UsePassive = true;

            // Specify the command to be executed.
            ftpWebRequest.Method = System.Net.WebRequestMethods.Ftp.UploadFile;

            // Specify the data transfer type.
            ftpWebRequest.UseBinary = true;

            // Notify the server about the size of the uploaded file
            ftpWebRequest.ContentLength = file.Length;

            // The buffer size is set to 2kb
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];

            try
            {
                // Stream to which the file to be upload is written
                System.IO.Stream stream = ftpWebRequest.GetRequestStream();

                // Read from the file stream 2kb at a time
                int contentLen = file.Read(buff, 0, buffLength);

                // Till Stream content ends
                while (contentLen != 0)
                {
                    // Write Content from the file stream to the FTP Upload Stream
                    stream.Write(buff, 0, contentLen);
                    contentLen = file.Read(buff, 0, buffLength);
                }

                // Close the file stream and the Request Stream
                stream.Flush();
                stream.Close();
                stream.Dispose();
                file.Close();
                file.Dispose();

                FtpWebResponse ftpRes = (FtpWebResponse)ftpWebRequest.GetResponse();
            }
            catch (Exception)
            {
            }


#endregion
        }

    }
}
