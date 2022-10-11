using Domain;
using System;
using Xunit;

namespace UnitTests
{
    public class TracerTester
    {
        [Fact]
        public void GetTraceResults_WithNoMethod_ReturnRightMethodsInfo()
        {
            // Arrange
            ITracer tracer = new Tracer();

            // Act

            // Assert
            Assert.Equal(0, tracer.GetTraceResult().MethodNames.Count);
            Assert.Equal(0, tracer.GetTraceResult().ThreadsIDs.Count);
            Assert.Equal(0, tracer.GetTraceResult().ClassNames.Count);
            Assert.Equal(0, tracer.GetTraceResult().InheritedMethodNames.Count);
            Assert.Equal(0, tracer.GetTraceResult().Durations.Count);
        }

        [Fact]
        public void GetTraceResults_WithEmptyMethod_ReturnZeroTime()
        {
            // Arrange
            ITracer tracer = new Tracer();

            // Act
            ClassF f = new ClassF(tracer);
            f.MethodF();
            var traceResult = tracer.GetTraceResult();

            // Assert
            Assert.Equal(1, traceResult.MethodNames.Count);
            Assert.Equal(1, traceResult.ThreadsIDs.Count);
            Assert.Equal(1, traceResult.ClassNames.Count);
            Assert.Equal(1, traceResult.InheritedMethodNames.Count);
            Assert.Equal(1, traceResult.Durations.Count);
            Assert.Equal(0, traceResult.Durations[0]);
        }

        [Fact]
        public void GetTraceResults_WithOneMethod_ReturnRightMethodsInfo()
        {
            // Arrange
            ITracer tracer = new Tracer();
            var a = new ClassA(tracer);

            // Act
            a.MethodA();

            // Assert
            Assert.Equal(2, tracer.GetTraceResult().MethodNames.Count);
            Assert.Equal(2, tracer.GetTraceResult().ThreadsIDs.Count);
            Assert.Equal(2, tracer.GetTraceResult().ClassNames.Count);
            Assert.Equal(2, tracer.GetTraceResult().InheritedMethodNames.Count);
            Assert.Equal(2, tracer.GetTraceResult().Durations.Count);
        }

        [Fact]
        public void GetTraceResults_WithTwoMethods_ReturnRightMethodsInfo()
        {

            // Arrange
            var tracer = new Tracer();
            var a = new ClassA(tracer);
            var c = new ClassC(tracer);

            // Act
            a.MethodA();
            c.MethodC();

            // Assert
            Assert.Equal(6, tracer.GetTraceResult().MethodNames.Count);
            Assert.Equal(6, tracer.GetTraceResult().ThreadsIDs.Count);
            Assert.Equal(6, tracer.GetTraceResult().ClassNames.Count);
            Assert.Equal(6, tracer.GetTraceResult().InheritedMethodNames.Count);
            Assert.Equal(6, tracer.GetTraceResult().Durations.Count);
        }

        [Fact]
        public void GetTraceResults_WithOneMethodAtThread_ReturnMethodsWithSimilarThreadId()
        {

            // Arrange
            var tracer = new Tracer();
            var a = new ClassA(tracer);

            // Act
            a.MethodA();

            // Assert
            for (int i = 1; i < tracer.GetTraceResult().ThreadsIDs.Count; i++)
            {
                Assert.Equal(tracer.GetTraceResult().ThreadsIDs[i - 1], tracer.GetTraceResult().ThreadsIDs[i]);
            }
        }

        [Fact]
        public void GetTraceResults_WithMethodInsideAnother_ReturnRightMethodsInfo()
        {

            // Arrange
            var tracer = new Tracer();
            var a = new ClassA(tracer);
            var b = new ClassB(tracer);


            // Act
            a.MethodA();
            b.MethodB();

            // Assert
            Assert.Equal(2+1, tracer.GetTraceResult().MethodNames.Count);
            Assert.Equal(2 + 1, tracer.GetTraceResult().ThreadsIDs.Count);
            Assert.Equal(2 + 1, tracer.GetTraceResult().ClassNames.Count);
            Assert.Equal(2 + 1, tracer.GetTraceResult().InheritedMethodNames.Count);
            Assert.Equal(2 + 1, tracer.GetTraceResult().Durations.Count);
        }
    }
}