using MongoDB.Driver;

namespace Infrastructure.Persistence.Mongo
{
    public class MongoDbProvider
    {
        public static IMongoDatabase Provide(MongoSettings settings)
        {
            //var client = new MongoClient(settings.Value.ConnectionString);

            MongoClientSettings clientSettings = new MongoClientSettings();
            clientSettings.Server = new MongoServerAddress(settings.Host, 10255);
            clientSettings.UseTls = settings.UseSsl;
            if (settings.UseSsl)
            {
                clientSettings.SslSettings = new SslSettings();
                clientSettings.SslSettings.EnabledSslProtocols = settings.EnabledSslProtocols;
            }

            MongoIdentity identity = new MongoInternalIdentity(settings.Database, settings.UserName);
            MongoIdentityEvidence evidence = new PasswordEvidence(settings.Password);

            clientSettings.Credential = new MongoCredential("SCRAM-SHA-1", identity, evidence);

            MongoClient client = new MongoClient(clientSettings);

            return client.GetDatabase(settings.Database);
        }
    }
}