﻿using SQLite;

namespace Biblioconecta.Data.Models;

public class Livro
{
    [PrimaryKey]
    [AutoIncrement]
    public int Id { get; set; }
    [Indexed]
    public int UsuarioId { get; set; }
    [Indexed]
    public int PrateleiraId { get; set; }
    [Indexed]
    public string Titulo { get; set; } = string.Empty;
    public string Subtitulo { get; set; } = string.Empty;
    public string ImagemUrl { get; set; } = string.Empty;
    [Indexed]
    public string Autor { get; set; } = string.Empty;
    public string Editora { get; set; } = string.Empty;
    public string Edicao { get; set; } = string.Empty;
    public int AnoPublicacao { get; set; }
    public string Idioma { get; set; } = string.Empty;
    public int Paginas { get; set; }
    public string ISBN { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public bool Favorito { get; set; } = false;
    public bool Lido { get; set; } = false;
    [Ignore]
    public bool PossuiImageUrl => !string.IsNullOrWhiteSpace(ImagemUrl);
    [Ignore]
    public bool NaoPossuiImageUrl => string.IsNullOrWhiteSpace(ImagemUrl);

}
