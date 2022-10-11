using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleExample
{
     public class A
     {
         private B _b;
         private ITracer _tracer;
         public A(ITracer tracer)
         {
             _tracer = tracer;
             _b = new B(_tracer);
         }

         public void MethodA()
         {
             _tracer.StartTrace();
             _b.MethodB();
             _b.MethodB();
             Thread.Sleep(100);
             _tracer.StopTrace();
         }

         public TraceResults getTracerResults()
         {
             return _tracer.GetTraceResult();
         }
     }
     public class B
     {
         private ITracer _tracer;
         public B(ITracer tracer)
         {
             _tracer = tracer;
         }

         public void MethodB()
         {
             _tracer.StartTrace();
             Thread.Sleep(1);
             _tracer.StopTrace();
         }
     }
     public class C
     {
         private D _d;
         private ITracer _tracer;
         public C(ITracer tracer)
         {
             _tracer = tracer;
             _d = new D(_tracer);
         }

         public void MethodC()
         {
             _tracer.StartTrace();
             _d.MethodD();
             _d.MethodD();
             Thread.Sleep(100);
             _tracer.StopTrace();
         }

         public TraceResults getTracerResults()
         {
             return _tracer.GetTraceResult();
         }
     }
     public class D
     {
         private ITracer _tracer;
         public D(ITracer tracer)
         {
             _tracer = tracer;
         }

         public void MethodD()
         {
             _tracer.StartTrace();
             Thread.Sleep(50);
             _tracer.StopTrace();
         }

         public TraceResults getTracerResults()
         {
             return _tracer.GetTraceResult();
         }
     }
     public class E
     {
         private ITracer _tracer;
         private F _f;

         public E(ITracer tracer)
         {
             _tracer = tracer;
             _f = new F(tracer);
         }

         public void MethodE()
         {
             _tracer.StartTrace();
             _f.MethodF();               
             _tracer.StopTrace();
         }

         public TraceResults getTracerResults()
         {
             return _tracer.GetTraceResult();
         }
     }

     public class F
     {
         private ITracer _tracer;
         public F(ITracer tracer)
         {
             _tracer = tracer;
         }

         public void MethodF()
         {
             _tracer.StartTrace();
             int a = 1 + 1;
             _tracer.StopTrace();
         }

         public TraceResults getTracerResults()
         {
             return _tracer.GetTraceResult();
         }
     }
}
