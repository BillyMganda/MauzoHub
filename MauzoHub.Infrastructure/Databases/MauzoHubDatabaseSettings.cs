namespace MauzoHub.Infrastructure.Databases
{
    public class MauzoHubDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string UsersCollectionName { get; set; } = null!;
        public string RefreshTokensCollectionName { get; set; } = null!;
        public string BusinessCategoriesCollectionName { get; set; } = null!;
        public string BusinessCollectionName { get; set; } = null!;
        public string ProductsCollectionName { get; set; } = null!;
        public string ServicesCollectionName { get; set; } = null!;
        public string AppointmentsCollectionName { get; set; } = null!;
    }
}
