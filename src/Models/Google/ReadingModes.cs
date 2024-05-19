using System.Text.Json.Serialization;

namespace Biblioconecta.Models.Google
{
    public partial class ReadingModes
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("text")]
        public virtual bool? Text { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("image")]
        public virtual bool? Image { get; set; }
    }
}
