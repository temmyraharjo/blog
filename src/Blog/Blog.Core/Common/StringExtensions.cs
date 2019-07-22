using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Blog.Core.Common
{
    public static class StringExtensions
    {
        public static string ToMd5(this string value)
        {
            return Encoding.ASCII.GetBytes(value).ToMd5Hash();
        }

        private static string ToMd5Hash(this byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;

            using (var md5 = MD5.Create())
            {
                return string.Join("", md5.ComputeHash(bytes).Select(x => x.ToString("X2")));
            }
        }
    }
}
