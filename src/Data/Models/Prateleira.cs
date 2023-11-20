using SQLite;

namespace Biblioconecta.Data.Models;

public class Prateleira
{
    [PrimaryKey]
    [AutoIncrement]
    public int Id { get; set; }
    [Indexed]
    public int UsuarioId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    [Ignore]
    public string Iniciais
    {
        get
        {
            if (string.IsNullOrEmpty(Nome))
            {
                return string.Empty;
            }
            string[] nomes = Nome.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string inciais = nomes[0].First().ToString();
            if (nomes.Length > 1)
            {
                inciais += nomes[1].First().ToString();
            }
            return inciais.ToUpper();
        }
    }
}
