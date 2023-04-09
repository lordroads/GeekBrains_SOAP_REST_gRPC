using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace PumpWcfService
{
    public class ScriptService : IScriptService
    {
        private readonly IStatisticsService _statisticsService;
        private readonly ISettingsService _settingsService;
        private readonly IPumpServiceCallback _pumpServiceCallback;

        private CompilerResults _results = null;

        public ScriptService(
            IStatisticsService statisticsService,
            ISettingsService settingsService,
            IPumpServiceCallback pumpServiceCallback)
        {
            _statisticsService = statisticsService;
            _settingsService = settingsService;
            _pumpServiceCallback = pumpServiceCallback;
        }
        public bool Compile()
        {
            try
            {
                CompilerParameters compilerParameters = new CompilerParameters();
                compilerParameters.GenerateInMemory = true;
                compilerParameters.ReferencedAssemblies.Add("System.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Core.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Data.dll");
                compilerParameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
                compilerParameters.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);

                using (FileStream fileStream = new FileStream(_settingsService.FileName, FileMode.Open))
                {
                    byte[] buffer = new byte[fileStream.Length];

                    int count = 0;
                    int sum = 0;

                    while ((count = fileStream.Read(buffer, sum, (int)fileStream.Length - sum)) > 0)
                    {
                        sum += count;
                    }

                    CSharpCodeProvider provider = new CSharpCodeProvider();
                    _results = provider.CompileAssemblyFromSource(compilerParameters, Encoding.UTF8.GetString(buffer));
                }

                if (_results.Errors != null && _results.Errors.Count != 0)
                {
                    StringBuilder compilerErrors = new StringBuilder();

                    foreach (var error in _results.Errors)
                    {
                        compilerErrors.AppendLine(error.ToString());
                    }

                    Console.WriteLine($"[COMPILE ERROR]: {compilerErrors}");

                    return false;
                }

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[COMPILE FATAL ERROR]: {ex.Message}");
                return false;
            }
        }

        public void Run(int count)
        {
            if (_results == null || (_results.Errors != null && _results.Errors.Count != 0))
            {
                if (Compile() == false)
                {
                    return;
                }
            }

            Type type = _results.CompiledAssembly.GetType("Simple.SampleScript");
            
            if (type == null) { return; }

            MethodInfo methodInfo = type.GetMethod("EntryPoint");

            if (methodInfo == null) { return; }

            Task.Run(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    if ((bool)methodInfo.Invoke(Activator.CreateInstance(type), null))
                    {
                        _statisticsService.SuccessTacts++;
                    }
                    else
                    {
                        _statisticsService.ErrorTacts++;
                    }

                    _statisticsService.AllTacts++;

                    _pumpServiceCallback.UpdateStatistics((StatisticsService)_statisticsService);
                    Thread.Sleep(300);
                }
            });
        }
    }
}