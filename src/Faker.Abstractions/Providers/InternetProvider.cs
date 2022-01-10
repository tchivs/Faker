using System.Globalization;
using System.Linq;
namespace Faker
{
    public class InternetProvider : BaseProvider<string>, IInternetProvider
    {
        protected virtual string[] http_methods
        {
            get => new[]
{
        "GET",
        "HEAD",
        "POST",
        "PUT",
        "DELETE",
        "CONNECT",
        "OPTIONS",
        "TRACE",
        "PATCH",
        };
        }
        protected virtual string[] uri_extensions { get; } = new string[]
        {
        ".html",
        ".html",
        ".html",
        ".htm",
        ".htm",
        ".php",
        ".php",
        ".jsp",
        ".asp",
        };
        protected virtual string[] tlds { get; } = new string[]
        {
              "com",
        "com",
        "com",
        "com",
        "com",
        "com",
        "biz",
        "info",
        "net",
        "org",
        };
        protected virtual string[] free_email_domains { get; } = new string[]
        {
        "gmail.com", "yahoo.com", "hotmail.com"
        };
        public InternetProvider(CultureInfo cultureInfo, IGenerator generator, ProviderOptions options) : base(cultureInfo, generator, options)
        {
        }
        public InternetOption Option { get => this.ProviderOptions.Internet; }
        public virtual string Email()
        {
            throw new System.NotImplementedException();
        }

        public virtual string HostName()
        {
            throw new System.NotImplementedException();
        }

        public string HttpMethod()
        {
            return this.ChoicesOneSample(http_methods);
        }

        public string MacAddress()
        {
            int[] ints = new int[6];
            for (int i = 0; i < ints.Length; i++)
            {
                ints[i] = Generator.Random.Next(0, 0xff);
            }
            return string.Join(this.Option.MacAddress.Separator, ints.Select(x => x.ToString(this.Option.MacAddress.Format)));
        }
        public virtual int PortNumber(PortNumberModel? model = null)
        {
            return model switch
            {
                PortNumberModel.IsSystem => this.Generator.Random.Next(0, 1023),
                PortNumberModel.IsUser => this.Generator.Random.Next(1024, 49151),
                PortNumberModel.IsDynamic => this.Generator.Random.Next(49152, 65535),
                _ => this.Generator.Random.Next(0, 65535)
            };
        }

        public virtual string UserName()
        {
            throw new System.NotImplementedException();
        }

        public string UriExtension()
        {
            return this.ChoicesOneSample(uri_extensions);
        }
    }
}