using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras
{
    public class Database
    {
        private readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Produto>().Wait();
        }

        public Task<List<Produto>> GetAll()
        {
            return _database.Table<Produto>().ToListAsync();
        }

        public Task<List<Produto>> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return GetAll();
            return _database.Table<Produto>()
                .Where(p => p.Descricao.Contains(query))
                .ToListAsync();
        }

        public Task<int> Insert(Produto produto)
        {
            return _database.InsertAsync(produto);
        }

        public Task<int> Update(Produto produto)
        {
            return _database.UpdateAsync(produto);
        }

        public Task<int> Delete(Produto produto)
        {
            return _database.DeleteAsync(produto);
        }

        public async Task<int> Delete(int id)
        {
            var produto = await _database.Table<Produto>().Where(p => p.Id == id).FirstOrDefaultAsync();
            if (produto != null)
                return await _database.DeleteAsync(produto);
            return 0;
        }
    }
}