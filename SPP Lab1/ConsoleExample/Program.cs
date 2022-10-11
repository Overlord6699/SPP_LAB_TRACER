using Domain;
using Serialization;
using Serialization.Info;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleExample
{
    class Program
    {
        static List<ThreadData> threadsInfo;

        static void Main(string[] args)
        {
            /* Thread thread1 = new Thread(new ThreadStart(e.MethodE));
             thread1.Start();
             thread1.Join();*/

            /* Thread thread2 = new Thread(new ThreadStart(c.MethodC));
             thread2.Start();
             thread2.Join();*/
            var tracer = new Tracer();

            // Calling methods using the same tracer

            var e = new E(tracer);
            e.MethodE();

            var a = new A(tracer);
            a.MethodA();

            var c = new C(tracer);
            c.MethodC();

            Thread thread = new Thread(new ThreadStart(e.MethodE));
            thread.Start();
            thread.Join();


            var aResults = c.getTracerResults();
            threadsInfo = new TraceParser().GetThreadsHierarchy(aResults);
            //threadsInfo = ProcessThreads(aResults);
            //threadsInfo = ProcessThreads(aResults);
            PrintInfo(threadsInfo);


            /*    Task task = new Task(c.MethodC);
                task.Start(TaskScheduler.Default);*/


            PluginCollector pluginCollector = new PluginCollector();
            pluginCollector.OnPluginAdded += ProcessNewPlugin;
            pluginCollector.LoadPlugins();
        }

        static void PrintInfo(in List<ThreadData> threadsData)
        {
            Console.WriteLine("Threads:\n");
            foreach (var threadResult in threadsData)
            {
                Console.WriteLine($"\tthreadID: {threadResult.ThreadID}");
                Console.WriteLine($"\ttime: {threadResult.Time}");
                Console.WriteLine($"\tmethods:\n");

                OutputResult(threadResult.Methods, "\t\t");
            }
        }

        static string GetContextFromName(in string typeName)
        {
            var defaultPrefix = "TracerSerializer";
            var length = defaultPrefix.Length;

            var name = typeName.ToString();
            var context = name.Remove(0, name.IndexOf(".") + 1).Remove(0, length);

            return context;
        }

        static void ProcessNewPlugin(ISerializer serializer)
        {
            var contextName = GetContextFromName(serializer.GetType().ToString());

            TestSerialization(serializer, contextName, threadsInfo);
        }

        static void TestSerialization(ISerializer serializer, in string name, in List<ThreadData> threadsData)
        {
            var context = Configuration.GetContext(name);

            if (context == null)
                return;


            var type = serializer.GetType();
            MethodInfo method = type.GetMethod(context[0]);
            Console.WriteLine($"Saving result to {name} file...");

            var directory = context[1];
            if (!Directory.Exists(directory))
            {
                Console.WriteLine($"{directory} NOT FOUND!\n");
                return;
            }

            var path = directory + "\\" + context[2] + name.ToLower();

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                fs.SetLength(0);
                var taskResult = serializer.Serialize(threadsData, fs);

                if (((Task<int>)taskResult).Result != 1)
                    Console.WriteLine($"Failed {name} serialization!\n");
                else
                    Console.WriteLine($"Successfull {name} serialization!\n");
            }
        }



        private static void OutputResult(List<MethodData> methods, string indent)
        {
            foreach (var method in methods)
            {
                Console.WriteLine($"{indent}class: {method.Class}");
                Console.WriteLine($"{indent}name: {method.Name}");
                Console.WriteLine($"{indent}time: {method.Time}");

                //обработка встроенных методов
                if (method.Methods.Count == 0)
                    Console.WriteLine($"{indent}methods: []\n");
                else
                {
                    Console.WriteLine($"{indent}methods:\n");
                    OutputResult(method.Methods, $"\t{indent}");
                }
            }
        }
        /*
        private static void AddTraceResultsInfo(in TraceResults from, ref TraceResults to, in int id)
        {
            to.threadsId.Add(from.threadsId[id]);
            to.classNames.Add(from.classNames[id]);
            to.inheritedMethodNames.Add(from.inheritedMethodNames[id]);
            to.methodNames.Add(from.methodNames[id]);
            to.durations.Add(from.durations[id]);
        }

        private static List<ThreadData> ProcessThreads(TraceResults traceResults)
        {
            int threadsNumber = traceResults.threadsId.Max();
            var threadsData = new List<ThreadData>(threadsNumber);



            for (int i = 0; i <= threadsNumber; i++)
            {
                //id в потоках идут не по порядку
                if (traceResults.threadsId.Contains(i))
                {
                    List<int> threadsID = new List<int>();


                    for (int j = 0; j < traceResults.methodNames.Count; j++)
                    {
                        if (traceResults.threadsId[j] == i)
                        {
                            threadsID.Add(j);
                        }
                    }

                    var currentTraceResults = new TraceResults();

                    for (int k = 0; k < threadsID.Count; k++)
                    {
                        AddTraceResultsInfo(traceResults, ref currentTraceResults, k);
                    }

                    for (int m = 0; m < threadsID.Count; m++)
                    {
                        threadsID[m] = m;
                    }

                    var methods = ProcessMethodsHierarchy(ref threadsID,
                        currentTraceResults);

                    var time = CalculateThreadTime(methods);

                    //так как обработка начинается с дочерних методов, то нужно перевернуть список
                    methods.Reverse();

                    threadsData.Add(new ThreadData(time, i, methods));
                }
            }

            return threadsData;
        }

        private static long CalculateThreadTime(in List<MethodData> methodsData)
        {
            long totalTime = 0;
            foreach (var method in methodsData)
            {
                totalTime += method.Time;
            }

            return totalTime;
        }

        private static void AddMethodInfo(ref List<MethodData> methodsData, 
            in TraceResults traceResults, in int id)
        {
            methodsData.Add(
                CreateNewMethodData(
                    traceResults,
                    new List<MethodData>(),
                    id)
            );
        }

        private static MethodData CreateNewMethodData(TraceResults traceResults, 
            List<MethodData> methodsData, in int id)
        {
            return new MethodData(
              traceResults.methodNames[id],
              traceResults.classNames[id],
              traceResults.durations[id],
              methodsData
          );
        }


    private static List<MethodData> ProcessMethodsHierarchy(ref List<int> threadsID,
            in TraceResults traceResults)
        {
            var methodsData = new List<MethodData>();

            int id = threadsID.LastOrDefault();

            if (id == 0 || threadsID.Count == 1 ||
               (traceResults.methodNames.Count != id + 1 &&
               traceResults.inheritedMethodNames[id] != traceResults.methodNames[id + 1]))
            {
                AddMethodInfo(ref methodsData, traceResults, id);
                return methodsData;
            }

            if (traceResults.inheritedMethodNames[id - 1] == traceResults.inheritedMethodNames[id])
            {
                AddMethodInfo(ref methodsData, traceResults, id);

                threadsID.RemoveAt(id);

                if (traceResults.inheritedMethodNames[threadsID.Last()] == traceResults.methodNames[id + methodsData.Count])
                {
                    threadsID.RemoveAt(id - methodsData.Count);

                    AddMethodInfo(ref methodsData, traceResults, id - methodsData.Count);
                }

                return methodsData;
            }

            threadsID.RemoveAt(id);

            var methodData = CreateNewMethodData(
                traceResults,
                ProcessMethodsHierarchy(ref threadsID, traceResults), id);

            methodsData.Add(methodData);

            while (threadsID.Count != 0)
            {
                if (threadsID.Count == 1)
                {
                    int lastID = threadsID.Last();
                    threadsID.RemoveAt(lastID);

                    AddMethodInfo(ref methodsData, traceResults, lastID);

                    break;
                }

                int tempId = threadsID.Last();
                threadsID.RemoveAt(tempId);


                methodsData.Add(
                    CreateNewMethodData(
                        traceResults,
                        ProcessMethodsHierarchy(ref threadsID, traceResults),
                        tempId)
                );
            }

            return methodsData;
        }*/
    }
}

  
