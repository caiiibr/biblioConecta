using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioconecta.Models;

public class PrateleiraModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public bool Selecionado {  get; set; } = false;
    public bool NaoSelecionado => !Selecionado;
}
