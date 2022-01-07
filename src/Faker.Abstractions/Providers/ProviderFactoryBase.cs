using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace Faker
{
    public abstract class ProviderFactoryBase : IProviderFactory
    {
        protected readonly CultureInfo CultureInfo;
        protected readonly IGenerator Generator;
        protected readonly ProviderOptions Options;
        public abstract IPersonProvider CreatePerson();
        public TProvider CreateProvider<TProvider>() where TProvider : BaseProvider
        {
            var type = typeof(TProvider);
            return type.CreateInstance<TProvider>(CultureInfo, Generator, Options);
        }
        protected ProviderFactoryBase(CultureInfo cultureInfo, IGenerator generator, ProviderOptions options)
        {
            CultureInfo = cultureInfo;
            Generator = generator;
            Options = options;
        }
    }
}