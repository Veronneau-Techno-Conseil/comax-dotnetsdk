using CertificateManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunAxiom.DotnetSdk.Helpers.Certificates
{
    public class CertGenerator : ICertGenerator
    {
        private readonly CreateCertificatesRsa _createCertificatesRsa;
        private readonly ImportExportCertificate _importExportCertificate;
        public CertGenerator(CreateCertificatesRsa createCertificatesRsa, ImportExportCertificate importExportCertificate)
        {
            _createCertificatesRsa = createCertificatesRsa;
            _importExportCertificate = importExportCertificate;
        }
        public CertData Generate(int keysize = 2048)
        {
            // Create development certificate for localhost
            var devCertificate = _createCertificatesRsa
                .CreateDevelopmentCertificate("localhost", 10, keysize);

            devCertificate.FriendlyName = "localhost self";

            // private key
            var exportRsaPrivateKeyPem = _importExportCertificate.PemExportRsaPrivateKey(devCertificate);

            // public key certificate as pem
            var exportPublicKeyCertificatePem = _importExportCertificate.PemExportPublicKeyCertificate(devCertificate);

            return new CertData { Certificate = exportPublicKeyCertificatePem, Key = exportRsaPrivateKeyPem };
        }
    }

    public interface ICertGenerator
    {
        CertData Generate(int keysize = 2048);
    }

    public class CertData
    {
        public string Key { get; set; }
        public string Certificate { get; set; }
    }
}
