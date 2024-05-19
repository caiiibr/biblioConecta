using SQLite;

namespace Biblioconecta.Data.Models
{
    public class MetaLivro
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public int MetaId { get; set; }
        public int LivroId { get; set; }

    }
}
