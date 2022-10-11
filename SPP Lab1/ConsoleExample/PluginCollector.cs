using Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleExample
{
    public class PluginCollector
    {
        public delegate void AddPlugin(ISerializer plugin);
        public event AddPlugin OnPluginAdded;

        private string _pluginPath;

        private List<ISerializer> _plugins = new List<ISerializer>();

        public void LoadPlugins()
        {
            _plugins.Clear();

            var exePath =  Directory.GetCurrentDirectory();
            _pluginPath = System.IO.Path.Combine(
                exePath,
                "..\\..\\..\\..\\Plugins");

            DirectoryInfo pluginDirectory = new DirectoryInfo(_pluginPath);

            if (!pluginDirectory.Exists)
                pluginDirectory.Create();
 
            var pluginFiles = Directory.GetFiles(_pluginPath, "*.dll");
            foreach (var file in pluginFiles)
            {
                Assembly asm = Assembly.LoadFrom(file);
                var types = asm.GetTypes().
                                Where(t => t.GetInterfaces().
                                Where(i => i.FullName == typeof(ISerializer).FullName).Any());

                foreach (var type in types)
                {
                    var plugin = asm.CreateInstance(type.FullName) as ISerializer;
                    _plugins.Add(plugin);

                    var realType = asm.CreateInstance(type.FullName);

                    OnPluginAdded?.Invoke((ISerializer) realType);
                }
            }
        }
    }
}
