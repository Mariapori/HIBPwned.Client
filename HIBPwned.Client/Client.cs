using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HIBPwned
{
    public static class Client
    {
        const string ApiUrl = "https://api.pwnedpasswords.com/range/";

        static readonly HttpClient HttpClient = new HttpClient();

        /// <summary>
        /// Checks whether the given password has been compromised in a known data breach.
        /// </summary>
        /// <param name="password">The plaintext password to check.</param>
        /// <returns><c>true</c> if the password appears in the Have I Been Pwned database; otherwise <c>false</c>.</returns>
        public static bool IsPasswordPwned(string password)
        {
            return IsPasswordPwnedAsync(password).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Asynchronously checks whether the given password has been compromised in a known data breach.
        /// </summary>
        /// <param name="password">The plaintext password to check.</param>
        /// <param name="cancellationToken">A token to cancel the request.</param>
        /// <returns><c>true</c> if the password appears in the Have I Been Pwned database; otherwise <c>false</c>.</returns>
        public static async Task<bool> IsPasswordPwnedAsync(string password, CancellationToken cancellationToken = default)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));

            var hash = ComputeSha1Hex(password);
            var prefix = hash.Substring(0, 5);
            var suffix = hash.Substring(5);

            using (var response = await HttpClient.GetAsync(ApiUrl + prefix, cancellationToken).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                foreach (var line in body.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var separatorIndex = line.IndexOf(':');
                    if (separatorIndex < 0) continue;

                    if (string.Equals(line.Substring(0, separatorIndex), suffix, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        static string ComputeSha1Hex(string value)
        {
            using (var sha1 = SHA1.Create())
            {
                var bytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(value));
                var sb = new StringBuilder(bytes.Length * 2);
                foreach (var b in bytes)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
