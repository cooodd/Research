using System;

namespace AllInOne
{
    class Proxy:MarshalByRefObject
    {
        public Action<string, bool> ConsoleWrite;
    }
}
