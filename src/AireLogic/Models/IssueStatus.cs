using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AireLogic.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum IssueStatus
    {
        Open,
        Closed
    }
}