using System.Diagnostics;
using System.Threading;

namespace Domain
{
    public class Tracer : ITracer
    {
        Stopwatch _watcher = new Stopwatch();

        TraceResults _traceResults = new TraceResults();

        public TraceResults GetTraceResult()
        {
            return _traceResults;
        }

        public void StartTrace()
        {
            _watcher.Start();
        }

        public void StopTrace()
        {
            _watcher.Stop();

            string methodName = (new StackTrace()).GetFrame(1).GetMethod().Name;
            string className = (new StackTrace()).GetFrame(1).GetMethod().DeclaringType.Name;
            string inheritedMethodName = (new StackTrace()).GetFrame(2).GetMethod().Name;
            int threadId = Thread.CurrentThread.ManagedThreadId;

            long time = _watcher.ElapsedMilliseconds;
            long additionalTime = 0;

            for (int i = 0; i < _traceResults.MethodNames.Count; i++)
            {
                if (_traceResults.InheritedMethodNames[i] == methodName)
                    additionalTime += _traceResults.Durations[i];
            }

            long totalTime = additionalTime + time;

            _traceResults.AddResult(methodName, className, totalTime, inheritedMethodName, threadId);

            _watcher.Reset();

            //необходимо из-за вложенности,
            //так как иначе не будут учитываться сами основные вызовы методов
            StartTrace();
        }
    }
}
