using System.Collections.Generic;
using Newtonsoft.Json;

namespace Convertara.Core.Dto
{
    public class GetAccessTokenResponse : ITwitchResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("scope")]
        public List<string> Scope { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
}
