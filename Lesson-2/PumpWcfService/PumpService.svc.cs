using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PumpWcfService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PumpService : IPumpService
    {
        private readonly IScriptService _scriptService;
        private readonly IStatisticsService _statisticsService;
        private readonly ISettingsService _settingsService;

        IPumpServiceCallback Callback
        {
            get
            {
                if (OperationContext.Current != null)
                {
                    return OperationContext.Current.GetCallbackChannel<IPumpServiceCallback>();
                }
                else
                {
                    return null;
                }
            }
        }

        public PumpService()
        {
            _statisticsService = new StatisticsService();
            _settingsService = new SettingsService();
            _scriptService = new ScriptService(
                _statisticsService,
                _settingsService,
                Callback);
        }

        public void RunScripte()
        {
            _scriptService.Run(10);
        }

        public void UpdateAndCompileScripte(string fileName)
        {
            _settingsService.FileName = fileName;
            _scriptService.Compile();
        }
    }
}
