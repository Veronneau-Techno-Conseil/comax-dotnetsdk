using CommunAxiom.DotnetSdk.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHelpers
{
    [TestClass()]
    public static class Setup
    {
        public static IServiceProvider ServiceProvider { get; set; }

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            var sc = new ServiceCollection();
            sc.SetupHelpers();
            ServiceProvider = sc.BuildServiceProvider();
        }
    }
}
