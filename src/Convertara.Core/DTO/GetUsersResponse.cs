
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Convertara.Core.Dto
{

    public class GetUsersResponse : ITwitchResponse
    {
        [JsonProperty("data")]
        public List<UserDto> Data {get;set;}
    }
}
