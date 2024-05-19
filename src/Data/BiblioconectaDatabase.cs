using Biblioconecta.Data.Models;
using SQLite;

namespace Biblioconecta.Data;

public class BiblioconectaDatabase
{
    SQLiteAsyncConnection Connection = null!;

    public async Task Init()
    {
        if (Connection is not null)
            return;

        Connection = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        _ = await Connection.CreateTableAsync<Usuario>();
        _ = await Connection.CreateTableAsync<Prateleira>();
        _ = await Connection.CreateTableAsync<Livro>();
        _ = await Connection.CreateTableAsync<Meta>();
        _ = await Connection.CreateTableAsync<MetaLivro>();
    }

    #region Livros
    public async Task<List<Livro>> GetLivrosAsync(int usuarioId)
    {
        await Init();
        return await Connection.Table<Livro>()
            .Where(e => e.UsuarioId == usuarioId)
            .OrderBy(e => e.Titulo)
            .ToListAsync();
    }

    public async Task<List<Livro>> GetLivrosFavoritosAsync(int usuarioId)
    {
        await Init();
        return await Connection.Table<Livro>()
            .Where(e => e.UsuarioId == usuarioId && e.Favorito == true)
            .OrderBy(e => e.Titulo)
            .ToListAsync();
    }

    public async Task<Livro> GetLivroAsync(int usuarioId, int id)
    {
        await Init();
        return await Connection.Table<Livro>()
            .Where(e => e.UsuarioId == usuarioId && e.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task CreateOrUpdateLivroAsync(Livro value)
    {
        await Init();
        int count = await Connection.Table<Livro>().CountAsync(e => e.Id == value.Id);
        if (count > 0)
        {
            _ = await Connection.UpdateAsync(value);
        }
        else
        {
            _ = await Connection.InsertAsync(value);
        }
        var prateleira = await GetPrateleiraAsync(Settings.Usuario?.Id ?? 0, value.PrateleiraId);
        if (prateleira != null)
        {
            int quantidade = await Connection.Table<Livro>().CountAsync(e => e.PrateleiraId == value.PrateleiraId);
            prateleira.Quantidade = quantidade;
            await CreateOrUpdatePrateleiraAsync(prateleira);
        }
    }

    public async Task<bool> DeleteLivroAsync(Livro value)
    {
        await Init();
        return await Connection.DeleteAsync(value) > 0;
    }
    #endregion

    #region Prateleiras
    public async Task<List<Prateleira>> GetPrateleirasAsync(int usuarioId)
    {
        await Init();
        return await Connection.Table<Prateleira>()
            .Where(e => e.UsuarioId == usuarioId)
            .OrderBy(e => e.Nome)
            .ToListAsync();
    }

    public async Task<Prateleira> GetPrateleiraAsync(int usuarioId, int prateleiraId)
    {
        await Init();
        return await Connection.Table<Prateleira>()
            .Where(e => e.UsuarioId == usuarioId && e.Id == prateleiraId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Livro>> GetLivrosByPrateleiraAsync(int usuarioId, int prateleiraId)
    {
        await Init();
        return await Connection.Table<Livro>()
            .Where(e => e.UsuarioId == usuarioId && e.PrateleiraId == prateleiraId)
            .ToListAsync();
    }

    public async Task CreateOrUpdatePrateleiraAsync(Prateleira value)
    {
        await Init();
        int count = await Connection.Table<Prateleira>().CountAsync(e => e.Id == value.Id);
        if (count > 0)
        {
            _ = await Connection.UpdateAsync(value);
        }
        else
        {
            _ = await Connection.InsertAsync(value);
        }
    }

    public async Task<bool> DeletePrateleiraAsync(Prateleira value)
    {
        await Init();
        return await Connection.DeleteAsync(value) > 0;
    }
    #endregion

    #region Usuários
    public async Task<Usuario?> GetUsuarioAsync(int id)
    {
        await Init();
        return await Connection.Table<Usuario>().Where(e => e.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Usuario?> GetUsuarioAsync(string email)
    {
        await Init();
        return await Connection.Table<Usuario>().Where(e => e.Email == email).FirstOrDefaultAsync();
    }

    public async Task CreateOrUpdateUsuarioAsync(Usuario value)
    {
        await Init();
        int count = await Connection.Table<Usuario>().CountAsync(e => e.Id == value.Id);
        if (count > 0)
        {
            _ = await Connection.UpdateAsync(value);
        }
        else
        {
            _ = await Connection.InsertAsync(value);
        }
    }

    public async Task<bool> UsuarioExists(string email)
    {
        await Init();
        int count = await Connection.Table<Usuario>().CountAsync(e => e.Email == email);
        return count > 0;
    }
    #endregion

    #region Metas
    public async Task<List<Meta>> GetMetasAsync(int usuarioId)
    {
        await Init();
        return await Connection.Table<Meta>()
            .Where(e => e.UsuarioId == usuarioId)
            .OrderBy(e => e.Nome)
            .ToListAsync();
    }

    public async Task<Meta> GetMetaAsync(int usuarioId, int id)
    {
        await Init();
        return await Connection.Table<Meta>()
            .Where(e => e.UsuarioId == usuarioId && e.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Livro>> GetLivrosByMetaAsync(int usuarioId, int metaId)
    {
        await Init();
        var metaLivros = await Connection.Table<MetaLivro>()
            .Where(e => e.MetaId == metaId)
            .ToListAsync();
        var livrosId = metaLivros
            .Select(e => e.LivroId)
            .ToList();
        return await Connection.Table<Livro>()
            .Where(e => e.UsuarioId == usuarioId && livrosId.Contains(e.Id))
            .ToListAsync();
    }

    public async Task CreateOrUpdateMetaAsync(Meta value)
    {
        await Init();
        int count = await Connection.Table<Meta>().CountAsync(e => e.Id == value.Id);
        if (count > 0)
        {
            _ = await Connection.UpdateAsync(value);
        }
        else
        {
            _ = await Connection.InsertAsync(value);
        }
    }

    public async Task<bool> DeleteMetaAsync(Meta value)
    {
        await Init();
        var metaLivros = await Connection.Table<MetaLivro>()
            .Where(e => e.MetaId == value.Id)
            .ToListAsync();
        foreach (var item in metaLivros)
        {
            await Connection.DeleteAsync(item);
        }
        return await Connection.DeleteAsync(value) > 0;
    }

    public async Task CreateMetaLivroAsync(MetaLivro value)
    {
        await Init();
        int count = await Connection.Table<MetaLivro>()
            .CountAsync(e => e.MetaId == value.MetaId && e.LivroId == value.LivroId);
        if (count < 1)
        {
            _ = await Connection.InsertAsync(value);
        }
    }

    public async Task<bool> DeleteMetaLivroAsync(MetaLivro value)
    {
        await Init();
        return await Connection.DeleteAsync(value) > 0;
    }

    public async Task DeleteMetaLivroByMetaAsync(int metaId)
    {
        await Init();
        var metaLivros = await Connection.Table<MetaLivro>()
            .Where(e => e.MetaId == metaId)
            .ToListAsync();
        foreach (var item in metaLivros)
        {
            await Connection.DeleteAsync(item);
        }
    }
    #endregion
}
