using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace Faker.Core
{
    internal class Faker : IFaker
    {
        private readonly ReadOnlyCollection<Type> _providerTypes;
        protected CultureInfo CultureInfo { get; }
        protected IGenerator Generator { get; }
        protected FakerOptions Options { get; }
        protected IProviderFactory ProviderFactory { get; }
        public Faker(ReadOnlyCollection<Type> providerTypes, Type providerFactoryType, CultureInfo cultureInfo, IGenerator generator, FakerOptions options)
        {
            _providerTypes = providerTypes;
            CultureInfo = cultureInfo;
            Generator = generator;
            this.Options = options;
            this.ProviderFactory = CreateProviderFactory(providerFactoryType);
            Person = ProviderFactory.CreatePerson();
        }
        private IProviderFactory CreateProviderFactory(Type providerFactoryType)
        {
            var provider = providerFactoryType.CreateInstance<ProviderFactoryBase>(this.CultureInfo,this.Generator,this.Options.Provider);
            return provider;
        }
        public IPersonProvider Person { get; }

        public TProvider CreateProvider<TProvider>() where TProvider : BaseProvider
        {
            var type = typeof(TProvider);
            if (this._providerTypes.Any(x => x.FullName == type.FullName))
            {
                return ProviderFactory.CreateProvider<TProvider>();
            }
            throw new NullReferenceException($"{type.FullName} not found,please register by FakerBuilder.AddProvider({type.Name})");
        }
    }


}
