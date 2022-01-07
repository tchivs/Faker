using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace Faker.Core
{
    public static class FakerBuilderExtensions
    {
        public static FakerBuilder UseCultureInfo(this FakerBuilder builder, string name)
        {
            builder.UseCultureInfo(CultureInfo.GetCultureInfo(name));
            return builder;
        }

        public static FakerBuilder AddProvider<TProvider>(this FakerBuilder builder) where TProvider : IProvider
        {
            builder.AddProvider(typeof(TProvider));
            return builder;
        }

        public static FakerBuilder UseFactory<TProviderFactory>(this FakerBuilder builder)
            where TProviderFactory : IProviderFactory
        {
            builder.UseFactory(typeof(TProviderFactory));
            return builder;
        }
    }
}
