using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citymobil
{
    using SQLite.Net;
    using System.Diagnostics; 

    public class DebugTraceListener : ITraceListener 
    { 
        public void Receive(string message)
        { 
            Debug.WriteLine(message); 
        } 
    } 

}
