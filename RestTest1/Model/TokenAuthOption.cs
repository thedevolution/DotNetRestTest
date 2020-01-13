using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;

namespace RestTest1.Model
{
	public class TokenAuthOption
    {
		public static string Audience { get; } = "MyAudience";
		public static string Issuer { get; } = "MyIssuer";
		public static RsaSecurityKey Key { get; } = new RsaSecurityKey(GenerateKey());
		public static SigningCredentials SigningCredentials { get; } = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);

		public static TimeSpan ExpiresSpan { get; } = TimeSpan.FromHours(7);
		public static string TokenType { get; } = "Bearer";

		public static RSAParameters GenerateKey()
		{
			using (var key = new RSACryptoServiceProvider(2048))
			{
				return key.ExportParameters(true);
			}
		}
	}
}
