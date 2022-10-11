using Serialization;
using Serialization.Info;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using ISerializer = Serialization.ISerializer;

namespace YAMLSerializer
{
    public class TracerSerializerYAML : ISerializer
    {
        public async Task<int> Serialize(List<ThreadData> threadsData, FileStream fs)
        {
            try
            {
                var serializer = new SerializerBuilder().WithNamingConvention(
                    CamelCaseNamingConvention.Instance).Build();
                var message = serializer.Serialize(threadsData);
                
                byte[] buffer = new UTF8Encoding().GetBytes(message);
                await fs.WriteAsync(buffer, 0, buffer.Length);
            }
            catch (Exception)
            {
                return -1;
            }
            return 1;
        }
    }
}
