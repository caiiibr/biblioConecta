using System.Text.Json.Serialization;

namespace Biblioconecta.Models.Google
{
    public partial class AccessInfo
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("country")]
        public virtual string? Country { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("viewability")]
        public virtual string? Viewability { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("embeddable")]
        public virtual bool? Embeddable { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("publicDomain")]
        public virtual bool? PublicDomain { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("textToSpeechPermission")]
        public virtual string? TextToSpeechPermission { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("epub")]
        public virtual Epub? Epub { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("pdf")]
        public virtual Epub? Pdf { get; set; } = new();

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("webReaderLink")]
        public virtual string? WebReaderLink { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("accessViewStatus")]
        public virtual string? AccessViewStatus { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("quoteSharingAllowed")]
        public virtual bool? QuoteSharingAllowed { get; set; }
    }
}
