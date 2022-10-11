
using System.Collections.Generic;
using System.IO;

namespace ConsoleExample
{
    public static class Configuration
    {

#region OUTPUT_FILE_NAME
        private const string OUTPUT_NAME = "Result.";
        private static string OUTPUT_DIRECTORY = Directory.GetCurrentDirectory() + @"..\\..\\..\\..\\..\\..\\Files";
        #endregion

        private const string SERIALIZATION_METHOD = "Serialize";


        public static List<string> GetContext(in string name)
        {
            if (name != "YAML" && name != "XML" && name != "JSON")
                return null;


            List<string> result = new List<string>(10);

            /*switch (name)
            {
                case "JSON":
                    result.Add(OUTPUT_JSON);
                        break;              
                case "XML":
                    result.Add(OUTPUT_XML);
                        break;
                case "YAML":
                    result.Add(OUTPUT_YAML);
                        break;
            }*/

            result.Add(SERIALIZATION_METHOD);
            result.Add(OUTPUT_DIRECTORY);
            result.Add(OUTPUT_NAME);

            return result;
        }
    }
}
