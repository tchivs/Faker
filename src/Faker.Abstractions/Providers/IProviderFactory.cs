using System;
namespace Faker
{
    public interface IProviderFactory:IDisposable
    {
        
        TProvider CreateProvider<TProvider>() where TProvider : IProvider;
    }
}