using PumpWcfClient.PumpServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PumpWcfClient
{
    
    internal class Program
    {
        public static PumpServiceClient client;
        static void Main(string[] args)
        {
            try
            {
                InstanceContext instanceContext = new InstanceContext(new CallbackHandler());
                
                client = new PumpServiceClient(instanceContext);

                client.UpdateAndCompileScripte("E:\\WorkTable\\Simple.script");

                client.RunScripte();

                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[FATAL ERROR]: \n" +  ex.Message  );
                Console.ReadKey(true);
            }
            finally
            {
                client.Close();
            }

        }
    }
}
