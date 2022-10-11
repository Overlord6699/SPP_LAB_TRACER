using Serialization;
using Serialization.Info;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Threading;

namespace XMLSerializer
{
    public class TracerSerializerXML : ISerializer
    {
        public async Task<int> Serialize(List<ThreadData> threadsData, FileStream fs)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ThreadData>));
                 
                await Task.Run( () => xmlSerializer.Serialize(fs, threadsData));
            }
            catch (Exception ex)
            {
                string exStr = ex.Message;
                return -1;
            }
            return 1;
        }
    }
}
