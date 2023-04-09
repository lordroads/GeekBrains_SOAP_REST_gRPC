using PumpWcfClient.PumpServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PumpWcfClient
{
    public class CallbackHandler : IPumpServiceCallback
    {
        public void UpdateStatistics(StatisticsService service)
        {
            Console.Clear();
            Console.WriteLine("ОБновление по статистике выполнения скрипта");
            Console.WriteLine($"Всего\tтактов: {service.AllTacts}");
            Console.WriteLine($"Успешных\tтактов: {service.SuccessTacts}");
            Console.WriteLine($"Ошибочных\tтактов: {service.ErrorTacts}");
        }
    }
}
