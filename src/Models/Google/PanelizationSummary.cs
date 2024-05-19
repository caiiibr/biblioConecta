using System.Text.Json.Serialization;

namespace Biblioconecta.Models.Google
{
    public partial class PanelizationSummary
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("containsEpubBubbles")]
        public virtual bool? ContainsEpubBubbles { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("containsImageBubbles")]
        public virtual bool? ContainsImageBubbles { get; set; }
    }
}
