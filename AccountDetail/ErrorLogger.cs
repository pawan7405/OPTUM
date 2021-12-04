using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using System.Web.Security;
using System.Net.Mail;
using System.IO;

namespace Prism.Utility
{
    public static class ErrorLogger
    {
        
        public static void ErrorLog(string pagename, string fuctionname, string errorMessage)
        {
            string rootPath = HttpContext.Current.Server.MapPath("~" + "\\ErrorLog");

            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string directoryName = year + month;
            string fileName = year + month + day;
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            if (Directory.Exists(rootPath + Path.DirectorySeparatorChar + directoryName))
            {
                WriteToTxtFile(rootPath + Path.DirectorySeparatorChar + directoryName + Path.DirectorySeparatorChar + fileName + "-Errorlogs.txt", pagename, fuctionname, errorMessage);
            }
            else
            {
                Directory.CreateDirectory(rootPath + Path.DirectorySeparatorChar + directoryName);
                WriteToTxtFile(rootPath + Path.DirectorySeparatorChar + directoryName + Path.DirectorySeparatorChar + fileName + "-Errorlogs.txt", pagename, fuctionname, errorMessage);
            }
        }
        
        private static void WriteToTxtFile(string filePath, string pagename, string fuctionname, string errorMessage)
        {
            StreamWriter objWriter = File.AppendText(filePath);
            objWriter.WriteLine(DateTime.Now + "~" + pagename + "~" + fuctionname + "~" + errorMessage);
            objWriter.Flush();
            objWriter.Close();
        }
    }
}
