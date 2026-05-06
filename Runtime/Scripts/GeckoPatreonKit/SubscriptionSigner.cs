using System.Security.Cryptography;
using System.Text;

namespace GeckoPatreonKit
{
    public static class SubscriptionSigner
    {
        public static string CreateSignature(string playerId, long timestamp, string nonce)
        {
            var payload = $"{playerId}|{timestamp}|{nonce}";
            var keyBytes = Encoding.UTF8.GetBytes(PatreonAuthenticator.Instance.package.safetyKey);
            var payloadBytes = Encoding.UTF8.GetBytes(payload);

            using var hmac = new HMACSHA256(keyBytes);
            var hash = hmac.ComputeHash(payloadBytes);
            return BytesToHex(hash);
        }

        private static string BytesToHex(byte[] bytes)
        {
            var sb = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }

            return sb.ToString();
        }

        public static string FetchSecret() => PatreonAuthenticator.Instance.package.safetyKey;
    }
}