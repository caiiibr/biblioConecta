using System.Text.Json.Serialization;

namespace Biblioconecta.Models.Google
{
    public partial class Book
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("kind")]
        public virtual string? Kind { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("id")]
        public virtual string? Id { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("etag")]
        public virtual string? Etag { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("selfLink")]
        public virtual string? SelfLink { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("volumeInfo")]
        public virtual VolumeInfo VolumeInfo { get; set; } = new();

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("saleInfo")]
        public virtual SaleInfo SaleInfo { get; set; } = new();

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("accessInfo")]
        public virtual AccessInfo AccessInfo { get; set; } = new();

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("searchInfo")]
        public virtual SearchInfo SearchInfo { get; set; } = new();
    }
}
