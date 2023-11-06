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
    }

    public async Task<bool> DeleteLivroAsync(Livro value)
    {
        await Init();
        return await Connection.DeleteAsync(value) > 0;
    }

    public async Task<bool> DeletePrateleiraAsync(Prateleira value)
    {
        await Init();
        return await Connection.DeleteAsync(value) > 0;
    }

    public async Task<List<Livro>> GetLivrosAsync()
    {
        await Init();
        return await Connection.Table<Livro>().ToListAsync();
    }

    public async Task<List<Livro>> GetLivrosFavoritosAsync()
    {
        await Init();
        return await Connection.Table<Livro>().Where(e => e.Favorito == true).ToListAsync();
    }

    public async Task<Livro> GetLivroAsync(int id)
    {
        await Init();
        return await Connection.Table<Livro>().Where(e => e.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Livro>> GetLivrosAsync(int prateleiraId)
    {
        await Init();
        return await Connection.Table<Livro>().Where(e => e.PrateleiraId == prateleiraId).ToListAsync();
    }

    public async Task<Prateleira> GetPrateleiraAsync(int id)
    {
        await Init();
        return await Connection.Table<Prateleira>().Where(e => e.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Prateleira>> GetPrateleirasAsync()
    {
        await Init();
        return await Connection.Table<Prateleira>().ToListAsync();
    }

    public async Task<Usuario?> GetUsuarioAsync(string email)
    {
        await Init();
        return await Connection.Table<Usuario>().Where(e => e.Email == email).FirstOrDefaultAsync();
    }

    public async Task SaveLivroAsync(Livro value)
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
        var prateleira = await GetPrateleiraAsync(value.PrateleiraId);
        if (prateleira != null)
        {
            int quantidade = await Connection.Table<Livro>().CountAsync(e => e.PrateleiraId == value.PrateleiraId);
            prateleira.Quantidade = quantidade;
            await SavePrateleiraAsync(prateleira);
        }
    }

    public async Task SavePrateleiraAsync(Prateleira value)
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

    public async Task SaveUsuarioAsync(Usuario value)
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
}
