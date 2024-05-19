using System.Text.Json.Serialization;

namespace Biblioconecta.Models.Google
{
    public partial class IndustryIdentifier
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("type")]
        public virtual string? Type { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("identifier")]
        public virtual string? Identifier { get; set; }
    }
}
