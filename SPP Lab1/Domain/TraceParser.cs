using Serialization.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class TraceParser
    {
        public List<ThreadData> GetThreadsHierarchy(TraceResults traceResults)
        {
            List<ThreadData> result = new List<ThreadData>();
            var threadsIds = GetThreadsIDs(traceResults);

            foreach (var id in threadsIds)
            {
                TraceResults threadTraceResults = GetThreadTraceResults(id, traceResults);
                List<string> parentMethods = GetParentMethods(threadTraceResults);
                List<MethodData> methods = new List<MethodData>();
                foreach (var parentMethod in parentMethods)
                {
                    methods.AddRange(GetMethodsHierarchy(threadTraceResults, parentMethod));
                }
                result.Add(new ThreadData(CalculateTotalTime(methods), id, methods));
            }
            return result;
        }

        private long CalculateTotalTime(in List<MethodData> methods)
        {
            long result = 0;
            foreach (var method in methods)
            {
                result += method.Time;
            }
            return result;
        }

        private List<MethodData> GetMethodsHierarchy(TraceResults traceResults, string parentMethod)
        {
            List<MethodData> methodsData = new List<MethodData>();

            for (int i = 0; i < traceResults.InheritedMethodNames.Count; i++)
            {
                if (traceResults.InheritedMethodNames[i].Equals(parentMethod))
                    methodsData.Add(new MethodData(traceResults.MethodNames[i], traceResults.ClassNames[i],
                        traceResults.Durations[i], GetMethodsHierarchy(traceResults, traceResults.MethodNames[i])));
            }
            return methodsData;
        }

        private List<string> GetParentMethods(TraceResults results)
        {
            List<string> result = new List<string>();
            foreach (var method in results.InheritedMethodNames)
            {
                if (!results.MethodNames.Contains(method) && !result.Contains(method))
                    result.Add(method);
            }
            return result;
        }

        private HashSet<int> GetThreadsIDs(TraceResults traceResult)
        {
            var result = new HashSet<int>(traceResult.ThreadsIDs);
            return result;
        }

        private TraceResults GetThreadTraceResults(int threadId, TraceResults traceResult)
        {
            var result = new TraceResults();

            for (int i = 0; i < traceResult.MethodNames.Count; i++)
            {
                if (traceResult.ThreadsIDs[i] == threadId)
                    result.AddResult(traceResult.MethodNames[i], traceResult.ClassNames[i],
                        traceResult.Durations[i], traceResult.InheritedMethodNames[i], threadId);
            }
            return result;
        }
    }
}
