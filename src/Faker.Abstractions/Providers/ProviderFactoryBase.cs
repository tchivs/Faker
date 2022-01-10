using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
namespace Faker
{
    public abstract   class ProviderFactoryBase : IProviderFactory
    {
        private readonly IServiceProvider serviceProvider;
        public IServiceScope servicesScope { get; private set; }
        public TProvider CreateProvider<TProvider>() where TProvider : IProvider
        {
            servicesScope ??= this.serviceProvider.CreateScope();
            return servicesScope.ServiceProvider.GetRequiredService<TProvider>();
        }
        public void Dispose()
        {
            servicesScope.Dispose();
        }
        protected ProviderFactoryBase(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
    }
}