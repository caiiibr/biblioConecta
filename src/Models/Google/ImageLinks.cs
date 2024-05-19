using System.Text.Json.Serialization;

namespace Biblioconecta.Models.Google
{
    public partial class ImageLinks
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("smallThumbnail")]
        public virtual string? SmallThumbnail { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("thumbnail")]
        public virtual string? Thumbnail { get; set; }
    }
}
