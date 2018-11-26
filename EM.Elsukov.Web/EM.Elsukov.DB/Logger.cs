using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace EM.Elsukov.DB
{
    public static class Logger
    {
        private static ILog log = LogManager.GetLogger("LOGGER");


        public static ILog Log
        {
            get { return log; }
        }

        public static void InitLogger()
        {
            System.IO.FileInfo f = new System.IO.FileInfo("App.config");
            XmlConfigurator.Configure(f);
        }
    }
}
