using CommunAxiom.DotnetSdk.Helpers.Certificates;
using Microsoft.Extensions.DependencyInjection;

namespace TestHelpers
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var generator = Setup.ServiceProvider.GetService<ICertGenerator>();
            var cert = generator.Generate();
            Assert.IsNotNull(cert);
            Assert.IsNotNull(cert.Certificate);
            Assert.IsNotNull(cert.Key);

            var pem = System.Security.Cryptography.X509Certificates.X509Certificate2.CreateFromPem(cert.Certificate);

            Assert.IsNotNull(pem);
        }
    }
}