using System.Text.Json.Serialization;

namespace Biblioconecta.Models.Google
{
    public partial class Epub
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("isAvailable")]
        public virtual bool? IsAvailable { get; set; }
    }
}
