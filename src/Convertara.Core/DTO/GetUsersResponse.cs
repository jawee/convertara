
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Convertara.Core.DTO
{

    public class GetUsersResponse : ITwitchResponse
    {
        [JsonProperty("data")]
        public List<UserDTO> Data {get;set;}
    }
}
