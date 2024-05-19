using System.Text.Json.Serialization;

namespace Biblioconecta.Models.Google
{
    public partial class SaleInfo
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("country")]
        public virtual string? Country { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("saleability")]
        public virtual string? Saleability { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("isEbook")]
        public virtual bool? IsEbook { get; set; }
    }
}
