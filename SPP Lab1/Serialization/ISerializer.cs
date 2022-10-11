using Serialization.Info;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Serialization
{
    public interface ISerializer
    {
        Task<int> Serialize(List<ThreadData> threadInfo, FileStream destination);
    }
}
