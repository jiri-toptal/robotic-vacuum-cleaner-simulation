using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MyQ.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Facing
    {
        [EnumMember(Value = "N")]
        North = 0,

        [EnumMember(Value = "E")]
        East,

        [EnumMember(Value = "S")]
        South,

        [EnumMember(Value = "W")]
        West
    }
}
