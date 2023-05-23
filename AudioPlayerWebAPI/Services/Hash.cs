using System.Security.Cryptography;
using System.Text;

namespace AudioPlayerWebAPI.Services
{
    public static class Hash
    {
        public static string GetSha1Hash(string text)
        {
            using var hash = SHA1.Create();
            return string.Concat(
                hash.ComputeHash(Encoding.UTF8.GetBytes(text))
                    .Select(x => x.ToString("X2")));
        }
    }
}
