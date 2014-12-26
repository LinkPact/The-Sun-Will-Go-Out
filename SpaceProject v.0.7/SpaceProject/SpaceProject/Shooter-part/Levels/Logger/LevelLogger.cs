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
    }
}
