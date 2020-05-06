using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using System;
using System.IO;
using System.Security.Cryptography;

namespace SBTP.Data
{
    class RsaHelper
    {
        /// <summary>
        ///  XML格式公钥转PEM格式公钥
        /// </summary>
        /// <param name="xml">XML格式的公钥</param>
        /// <param name="saveFile">保存文件的物理路径</param>
        public static string Xml2PemPublic(string xml, string saveFile)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xml);
            var p = rsa.ExportParameters(false);
            RsaKeyParameters key = new RsaKeyParameters(false, new BigInteger(1, p.Modulus), new BigInteger(1, p.Exponent));
            using (var sw = new StreamWriter(saveFile))
            {
                var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(sw);
                pemWriter.WriteObject(key);
            }

            SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(key);
            byte[] serializedPublicBytes = publicKeyInfo.ToAsn1Object().GetDerEncoded();
            string publicKey = Convert.ToBase64String(serializedPublicBytes);
            return Format(publicKey, 1);
        }

        /// <summary>
        ///  XML格式私钥转PEM
        /// </summary>
        /// <param name="xml">XML格式私钥</param>
        /// <param name="saveFile">保存文件的物理路径</param>
        public static string Xml2PemPrivate(string xml, string saveFile)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xml);
            var p = rsa.ExportParameters(true);
            var key = new RsaPrivateCrtKeyParameters(
                new BigInteger(1, p.Modulus), new BigInteger(1, p.Exponent), new BigInteger(1, p.D),
                new BigInteger(1, p.P), new BigInteger(1, p.Q), new BigInteger(1, p.DP), new BigInteger(1, p.DQ),
                new BigInteger(1, p.InverseQ));
            using (var sw = new StreamWriter(saveFile))
            {
                var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(sw);
                pemWriter.WriteObject(key);
            }

            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(key);
            byte[] serializedPrivateBytes = privateKeyInfo.ToAsn1Object().GetEncoded();
            string privateKey = Convert.ToBase64String(serializedPrivateBytes);
            return Format(privateKey, 2);
        }

        /// <summary>
        ///  格式化公钥/私钥
        /// </summary>
        /// <param name="key">生成的公钥/私钥</param>
        /// <param name="type">1:公钥 2:私钥</param>
        /// <returns>PEM格式的公钥/私钥</returns>
        public static string Format(string key, int type)
        {
            string result = string.Empty;

            int length = key.Length / 64;
            for (int i = 0; i < length; i++)
            {
                int start = i * 64;
                result = result + key.Substring(start, 64) + "\r\n";
            }

            result = result + key.Substring(length * 64);
            if (type == 1)
            {
                result = result.Insert(0, "-----BEGIN PUBLIC KEY-----\r\n");
                result += "\r\n-----END PUBLIC KEY-----";
            }
            if (type == 2)
            {
                result = result.Insert(0, "-----BEGIN PRIVATE KEY-----\r\n");
                result += "\r\n-----END PRIVATE KEY-----";
            }

            return result;
        }
    }
}
