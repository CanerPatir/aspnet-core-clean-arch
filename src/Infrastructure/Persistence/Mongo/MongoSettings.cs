using System.Security.Authentication;

namespace Infrastructure.Persistence.Mongo
{
    public class MongoSettings
    {
        //public string ConnectionString { get; set; }
        public int Port { get; set; } = 10255;
        public string Host { get; set; }
        public bool UseSsl { get; set; } = true;
        public string Database { get; set; }
        public SslProtocols EnabledSslProtocols { get; set; } = SslProtocols.Tls12;
        public string Password { get; internal set; }
        public string UserName { get; internal set; }
    }
}