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
        IInternetProvider _internet;
        public IInternetProvider Internet => _internet ??= CreateProvider<IInternetProvider>();

        public TProvider CreateProvider<TProvider>() where TProvider : IProvider
        {
            return ProviderFactory.CreateProvider<TProvider>();
        }
        public void Dispose()
        {
            if (_person!=null)
            {
                _person.Dispose();
                _person = null;
            }
            if (_internet != null)
            {
                _internet.Dispose();
                _internet = null;
            }
            ProviderFactory.Dispose();
        }
    }


}
