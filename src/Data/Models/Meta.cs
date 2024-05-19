using SQLite;

namespace Biblioconecta.Data.Models
{
    public class Meta
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        [Indexed]
        public int UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime DataInicio { get; set; } = DateTime.Today;
        public DateTime DataTermino { get; set; } = DateTime.Today;
        public int QuantidadeLivros { get; set; }
        public int QuantidadeLivrosLidos { get; set; }
    }
}
