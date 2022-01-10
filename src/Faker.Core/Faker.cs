using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
namespace Faker.Core
{
    internal class Faker : IFaker
    {
        protected IProviderFactory ProviderFactory { get; }
        public Faker(IProviderFactory factory)
        {
            this.ProviderFactory = factory;
        }
        IPersonProvider _person;
        public IPersonProvider Person { get => _person ??= CreateProvider<IPersonProvider>(); }
        public TProvider CreateProvider<TProvider>() where TProvider : IProvider
        {
            return ProviderFactory.CreateProvider<TProvider>();
        }
        public void Dispose()
        {
            Console.WriteLine($"{nameof(Dispose)} Faker");
            _person.Dispose();
            _person = null;
            ProviderFactory.Dispose();
        }
    }


}
