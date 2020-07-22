using MongoDB.Driver;

namespace MongoDB.Thin
{
    public static class IMongoDatabaseExtensions
    {
        public static IMongoCollection<TDocument> Collection<TDocument>(this IMongoDatabase database)
        {
            return database.GetCollection<TDocument>(typeof(TDocument).Name);
        }
    }
}
