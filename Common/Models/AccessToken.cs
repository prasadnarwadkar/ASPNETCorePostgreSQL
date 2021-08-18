using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    /// <summary>
    /// Represents an access token obtained from
    /// an identity server.
    /// </summary>
    public class AccessToken
    {
        /// <summary>
        /// Actual access token.
        /// </summary>
        public string access_token { get; set; }
        public long expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
    }
}
