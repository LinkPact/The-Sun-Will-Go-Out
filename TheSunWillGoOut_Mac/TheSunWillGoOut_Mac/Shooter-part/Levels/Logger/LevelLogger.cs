using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    class LevelLogger
    {
        public static String writeDir = Game1.SaveFilePath + "Log";
        public static String writeFile = "defaultlog.txt";
        private static String currentPath { get { return writeDir + "/" + writeFile; } }

        public static void InitializeNewLogfile()
        {
            String timeString = DateTime.Now.ToString("Md_HHmmss tt");
            writeFile = "log" + timeString + ".txt";
        }

        public static void WriteLine(String message)
        {
            System.IO.Directory.CreateDirectory(writeDir);

            using (StreamWriter file = File.AppendText(@currentPath))
            {
                file.WriteLine(message);
            }
        }
    }
}
