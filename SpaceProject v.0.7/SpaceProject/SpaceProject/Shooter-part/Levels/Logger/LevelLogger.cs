using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class LevelLogger
    {
        public LevelLogger(String message)
        {
            String basepath = "Log\\testlog.txt";

            System.IO.Directory.CreateDirectory("Log");

            using (StreamWriter file = File.AppendText(@basepath))
            {
                file.WriteLine(message);
            }
        }

        public static String writeDir = "Log";
        public static String writeFile = "testlog.txt";
        private static String currentPath { get { return writeDir + "\\" + writeFile; } }

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
