using Domain;


namespace UnitTests
{
    public class ClassA
    {
        private ClassB _b;
        private ITracer _tracer;

        public ClassA(ITracer tracer)
        {
            _tracer = tracer;
            _b = new ClassB(_tracer);
        }

        public void MethodA()
        {
            _tracer.StartTrace();

            _b.MethodB();
            System.Threading.Thread.Sleep(200);

            _tracer.StopTrace();
        }
    }
    public class ClassB
    {
        private ITracer _tracer;

        public ClassB(ITracer tracer)
        {
            _tracer = tracer;
        }

        public void MethodB()
        {
            _tracer.StartTrace();
            System.Threading.Thread.Sleep(2);
            _tracer.StopTrace();
        }
    }
    public class ClassC
    {
        private ClassD _d;
        private ITracer _tracer;
        public ClassC(ITracer tracer)
        {
            _tracer = tracer;
            _d = new ClassD(this._tracer);
        }

        public void MethodC()
        {
            _tracer.StartTrace();
            _d.MethodD();
            _d.MethodD();
            _d.MethodD();
            _tracer.StopTrace();
        }
    }
    public class ClassD
    {
        private ITracer _tracer;
        public ClassD(ITracer tracer)
        {
            _tracer = tracer;
        }

        public void MethodD()
        {
            _tracer.StartTrace();
            System.Threading.Thread.Sleep(1);
            _tracer.StopTrace();
        }
    }
    public class ClassF
    {
        private ITracer _tracer;
        public ClassF(ITracer tracer)
        {
            _tracer = tracer;
        }

        public void MethodF()
        {
            _tracer.StartTrace();
            _tracer.StopTrace();
        }

        public TraceResults getTracerResults()
        {
            return _tracer.GetTraceResult();
        }
    }
}

