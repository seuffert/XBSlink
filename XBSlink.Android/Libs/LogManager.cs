using System;
using System.Collections.Generic;
using System.Text;

namespace XBSlink.Android.Managers
{
    public class LogManager
    {
      
        LogActivity actualActivity;

        public LogManager()
        {

            

            //log_tmp = new List<string>();
        }

        public void SetLogActitivty(LogActivity logAct)
        {
            actualActivity = logAct;
        }

        public void WriteLine(string message)
        {
            if (actualActivity != null)
                actualActivity.AppendLog(message);
        }

        public void CheckWriteLine(string message)
        {
            if (actualActivity != null)
            {
                if ( EnvironmentEx._log_cache.Count > 0)
                {
                    lock (EnvironmentEx._log_cache)
                    {
                        foreach (var item in EnvironmentEx._log_cache)
                            WriteLine(item);
                    }
                  
                }
            }
            else
                WriteLine(message);
        }


    }
}
