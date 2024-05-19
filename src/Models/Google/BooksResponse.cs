using System.Text.Json.Serialization;

namespace Biblioconecta.Models.Google
{
    public partial class BooksResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("kind")]
        public virtual string? Kind { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("totalItems")]
        public virtual long? TotalItems { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("items")]
        public virtual List<Book> Items { get; set; } = new();
    }
}
