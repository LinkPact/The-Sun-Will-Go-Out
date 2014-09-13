using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SpaceProject.MapCreator
{
    class WriteSave
    {
        public static void Save(String levelName, List<String> saveData)
        {
            int height = saveData.Count;

            String path = "MapCreatorLevels\\";
            path += levelName; 
            path += ".lvl";

            using (StreamWriter file = new StreamWriter(@path))
            {
                for (int n = 0; n < height; n++) 
                {
                    String line = saveData[n];

                    String saveLine = "";

                    for (int m = 0; m < line.Length; m++)
                    {
                        saveLine += line.Substring(m, 1);
                    }
                    file.WriteLine(saveLine);
                }
            }
        }
    }
}
