using System;
using System.Collections.Generic;

namespace Serialization.Info
{
    public struct ThreadData
    {
        public int ThreadID { get; set; }
        public long Time { get; set; }       
        public List<MethodData> Methods { get; set; }

        public ThreadData(in long time, in int id, in List<MethodData> methods)
        {
            ThreadID = id;
            Time = time;
            Methods = methods;
        }
    }
}
