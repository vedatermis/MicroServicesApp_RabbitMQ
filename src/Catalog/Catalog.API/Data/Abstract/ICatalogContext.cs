using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data.Abstract
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; }
    }
}