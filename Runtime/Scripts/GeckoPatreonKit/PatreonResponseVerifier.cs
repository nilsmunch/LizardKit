using System;
using System.Security.Cryptography;
using System.Text;

namespace LizardKit.GeckoPatreonKit
{
    public static class PatreonResponseVerifier
    {
        public static bool IsValidSignature(PatreonAuthenticator.Root response)
        {
            if (response?.payload == null || string.IsNullOrEmpty(response.signature))
                return false;

            if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() > response.payload.expires_at)
                return false;

            var stringToSign =
                $"{response.payload.tier_level}|{response.payload.user_name}|{response.payload.game}|{response.payload.client_uuid}|{response.payload.expires_at}";

            var expected = ComputeHmacSha256(stringToSign, SubscriptionSigner.FetchSecret());

            return string.Equals(expected, response.signature, StringComparison.OrdinalIgnoreCase);
        }

        private static string ComputeHmacSha256(string message, string secret)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secret);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            using var hmac = new HMACSHA256(keyBytes);
            var hash = hmac.ComputeHash(messageBytes);
            return BytesToHex(hash);
        }

        private static string BytesToHex(byte[] bytes)
        {
            var sb = new StringBuilder(bytes.Length * 2);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}