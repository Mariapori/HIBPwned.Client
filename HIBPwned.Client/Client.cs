
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace HIBPwned
{
    public static class Client
    {
        const string ApiUrl = "https://api.pwnedpasswords.com/range/{0}";
        
        /// <summary>
        /// Checks are this password pwned.
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Boolean are password pwned</returns>
        public static bool IsPasswordPwned(string password)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hexString = string.Empty;
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }

                hexString = sb.ToString();

                using(var httpClient = new HttpClient())
                {
                    var result = httpClient.GetStringAsync(string.Format(ApiUrl,hexString.Substring(0,5))).Result.Split(new string[]{"\r\n"},StringSplitOptions.None);
                    return result.Any(o => hexString.Substring(0,5)+o.Split(':')[0] == hexString);
                }
            }
        }
    }
}
