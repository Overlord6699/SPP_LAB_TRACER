using System.Collections.Generic;

namespace Domain
{
    public class TraceResults
    {
        public readonly List<string> MethodNames = new List<string>();
        public readonly List<string> ClassNames = new List<string>();
        public readonly List<long> Durations = new List<long>();
        
        public readonly List<string> InheritedMethodNames = new List<string>();
        public readonly List<int> ThreadsIDs = new List<int>();

        public void AddResult(in string methodName, in string className,
            in long duration, in string inheritedMethodName,in int threadId)
        {
            MethodNames.Add(methodName);
            ClassNames.Add(className);
            Durations.Add(duration);
            InheritedMethodNames.Add(inheritedMethodName);
            ThreadsIDs.Add(threadId);
        }
    }
}