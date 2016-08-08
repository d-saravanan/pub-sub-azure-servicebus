using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.ServiceBus.Wrapper.CacheStore
{
    public interface ILogger
    {
        void Log(string message);
        void Log(Exception exception);
    }
}
