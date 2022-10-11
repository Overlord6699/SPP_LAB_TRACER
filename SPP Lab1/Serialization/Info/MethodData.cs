using System.Collections.Generic;

namespace Serialization.Info
{
    public struct MethodData
    {
        public string Class { get; set; }
        public string Name { get; set; }     
        public long Time { get; set; }
        public List<MethodData> Methods { get; set; }

        public MethodData(in string name, in string className, in long time, in List<MethodData> methods)
        {
            Name = name;
            Class = className;
            Time = time;
            Methods = methods;
        }

    }
}
