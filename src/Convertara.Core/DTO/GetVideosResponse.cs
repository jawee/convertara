using System.Collections.Generic;
using Newtonsoft.Json;

namespace Convertara.Core.Dto
{
    public class Pagination
    {
        [JsonProperty("cursor")]
        public string Cursor {get;set;}
    }

    public class GetVideosResponse : ITwitchResponse
    {
        [JsonProperty("data")]
        public List<VideoDto> Data {get;set;}
        [JsonProperty("pagination")]
        public Pagination Pagination {get;set;}
    }
}
