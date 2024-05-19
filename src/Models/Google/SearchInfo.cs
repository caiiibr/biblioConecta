using System.Text.Json.Serialization;

namespace Biblioconecta.Models.Google
{
    public partial class SearchInfo
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("textSnippet")]
        public virtual string? TextSnippet { get; set; }
    }
}
