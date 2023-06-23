using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApiMongoDB01.Models;

namespace WebApiMongoDB01.Services;

public class ProdutoService
{
    private readonly IMongoCollection<Produto> _produtoCollection;

    public ProdutoService(IOptions<ProdutoDatabaseSettings> produtoService)
    {
        var mongoClient = new MongoClient(produtoService.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(produtoService.Value.DatabaseName);
       
        _produtoCollection = mongoDatabase.GetCollection<Produto>(produtoService.Value.ProdutoCollectionName);
    }

    public async Task<List<Produto>> GetAsync() =>
        await _produtoCollection.Find(x => true).ToListAsync();

    public async Task<Produto> GetAsync(string id) =>
        await _produtoCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Produto produto) =>
        await _produtoCollection.InsertOneAsync(produto);

    public async Task UpdateAsync(string id,Produto produto) =>
        await _produtoCollection.ReplaceOneAsync(x=> x.Id == id, produto);

    public async Task RemoveAsync(string id) =>
        await _produtoCollection.DeleteOneAsync(x=> x.Id == id);
}
