namespace MauzoHub.Infrastructure.Databases
{
    public class MauzoHubDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string UsersCollectionName { get; set; } = null!;
        public string RefreshTokensCollectionName { get; set; } = null!;
        public string BusinessCategoriesCollectionName { get; set; } = null!;
    }
}
