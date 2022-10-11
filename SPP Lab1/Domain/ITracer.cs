
namespace Domain
{
    public interface ITracer
    {
        void StartTrace();
        void StopTrace();
        TraceResults GetTraceResult();
    }
}
