using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllInOne
{
    static class DomainManager
    {
        static AppDomain domain;

        static DomainManager()
        {
            domain = AppDomain.CreateDomain("Samples");
            domain.SetupInformation.ShadowCopyFiles = "True";
        }

        public static AppDomain SampleDomain
        {
            get { return domain; }
        }
    }
}
