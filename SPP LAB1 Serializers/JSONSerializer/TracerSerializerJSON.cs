using Serialization;
using Serialization.Info;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace JSONSerializer
{
    public class TracerSerializerJSON : ISerializer
    {
        public async Task<int> Serialize(List<ThreadData> threadsData, FileStream fs)
        {
            try
            {
                await JsonSerializer.SerializeAsync<List<ThreadData>>(fs, threadsData);
            }
            catch (Exception)
            {
                return -1;
            }

            return 1;
        }
    }
}
