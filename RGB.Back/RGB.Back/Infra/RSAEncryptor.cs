using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace RGB.Back.Infra
{
	public class RSAEncryptor
	{
		private static RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
		// 加载预先生成的密钥对文件

		private RSAParameters _privateKey;
		private RSAParameters _publicKey;


		//public RSAEncryptor()
		//{
		//	_privateKey = rsa.ExportParameters(true);
		//	_publicKey = rsa.ExportParameters(false);

		//}

		public RSAEncryptor(string privateKeyFilePath, string publicKeyFilePath)
		{
			if (File.Exists(privateKeyFilePath) && File.Exists(publicKeyFilePath))
			{
				using (StreamReader srPrivate = new StreamReader(privateKeyFilePath))
				{
					var xsPrivate = new XmlSerializer(typeof(RSAParameters));
					_privateKey = (RSAParameters)xsPrivate.Deserialize(srPrivate);
				}

				using (StreamReader srPublic = new StreamReader(publicKeyFilePath))
				{
					var xsPublic = new XmlSerializer(typeof(RSAParameters));
					_publicKey = (RSAParameters)xsPublic.Deserialize(srPublic);
				}

				rsa = new RSACryptoServiceProvider();
				rsa.ImportParameters(_privateKey);

			}
			else
			{
				throw new FileNotFoundException("Private or public key file not found.");
			}
		}




		public string GetPublicKey()
		{
			var sw = new StringWriter();
			var xs = new XmlSerializer(typeof(RSAParameters));
			xs.Serialize(sw, _publicKey);
			return sw.ToString();
		}

		public string Encrypt(string plainText)
		{
			rsa = new RSACryptoServiceProvider();
			rsa.ImportParameters(_publicKey);
			var data = Encoding.Unicode.GetBytes(plainText);
			var cypher = rsa.Encrypt(data, false);
			return Convert.ToBase64String(cypher);
		}

		public string Decrypt(string cypherText)
		{
			var dataBytes = Convert.FromBase64String(cypherText);
			rsa.ImportParameters(_privateKey);
			var plainText = rsa.Decrypt(dataBytes, false);
			return Encoding.Unicode.GetString(plainText);
		}

		// 保存私钥到文件
		public void SavePrivateKeyToFile(string filePath)
		{
			using (StreamWriter sw = new StreamWriter(filePath))
			{
				var xs = new XmlSerializer(typeof(RSAParameters));
				xs.Serialize(sw, _privateKey);
			}
		}

		// 保存公钥到文件
		public void SavePublicKeyToFile(string filePath)
		{
			using (StreamWriter sw = new StreamWriter(filePath))
			{
				var xs = new XmlSerializer(typeof(RSAParameters));
				xs.Serialize(sw, _publicKey);
			}
		}

		//public static void LoadKeyPair(string publicKeyPath, string privateKeyPath, out RSAParameters publicKey, out RSAParameters privateKey)
		//{
		//	publicKey = default;
		//	privateKey = default;

		//	if (File.Exists(publicKeyPath) && File.Exists(privateKeyPath))
		//	{
		//		publicKey = ReadKeyFromFile(publicKeyPath);
		//		privateKey = ReadKeyFromFile(privateKeyPath);
		//	}
		//	else if (File.Exists(publicKeyPath))
		//	{
		//		// 如果只有公钥文件存在，则生成新的密钥对并保存到文件
		//		using (RSA rsa = RSA.Create())
		//		{
		//			publicKey = rsa.ExportParameters(false);
		//			privateKey = rsa.ExportParameters(true);

		//			WriteKeyToFile(publicKeyPath, publicKey);
		//			WriteKeyToFile(privateKeyPath, privateKey);
		//		}
		//	}
		//	else
		//	{
		//		// 如果密钥文件都不存在，则抛出异常
		//		throw new FileNotFoundException("Public key file not found.");
		//	}
		//}

		//// 从文件加载密钥参数
		//private static RSAParameters ReadKeyFromFile(string filePath)
		//{
		//	using (StreamReader reader = new StreamReader(filePath))
		//	{
		//		string keyXml = reader.ReadToEnd();
		//		RSA rsa = RSA.Create();
		//		rsa.FromXmlString(keyXml);
		//		return rsa.ExportParameters(false);
		//	}
		//}

		//// 将密钥参数写入文件
		//private static void WriteKeyToFile(string filePath, RSAParameters keyParams)
		//{
		//	using (StreamWriter writer = new StreamWriter(filePath))
		//	{
		//		string keyXml = ConvertToXmlString(keyParams);
		//		writer.Write(keyXml);
		//	}
		//}

		//// 使用公钥加密数据
		//public static byte[] Encrypt(byte[] data, RSAParameters publicKey)
		//{
		//	using (RSA rsa = RSA.Create())
		//	{
		//		rsa.ImportParameters(publicKey);
		//		return rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
		//	}
		//}

		//// 使用私钥解密数据
		//public static byte[] Decrypt(byte[] data, RSAParameters privateKey)
		//{
		//	using (RSA rsa = RSA.Create())
		//	{
		//		rsa.ImportParameters(privateKey);
		//		return rsa.Decrypt(data, RSAEncryptionPadding.OaepSHA256);
		//	}
		//}

		//// 将 RSAParameters 转换为 XML 字符串
		//private static string ConvertToXmlString(RSAParameters rsaParams)
		//{
		//	using (var rsa = RSA.Create())
		//	{
		//		rsa.ImportParameters(rsaParams);
		//		return rsa.ToXmlString(true);
		//	}
		//}
	}
}