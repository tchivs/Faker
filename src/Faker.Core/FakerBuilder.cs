using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace Faker.Core
{
    public interface IObjectAccessor<T>
    {
        T Value { get; set; }
    }
    public class ObjectAccessor<T> : IObjectAccessor<T>
    {
        public T Value { get; set; }
    }

    public class FakerBuilder
    {
        CultureInfo _cultureInfo = CultureInfo.CurrentCulture;
        private readonly FakerOptions _options = new FakerOptions();
        private readonly IServiceCollection services;
        private IGenerator _generator;
        private Type _type;
        #region Builder Methods
        public FakerBuilder(IServiceCollection services = null)
        {
            this.services = services ?? new ServiceCollection();
            this.services.AddTransient(typeof(IObjectAccessor<>), typeof(ObjectAccessor<>));
            //this.services.AddSingleton<IObjectAccessor<IServiceProvider>, ObjectAccessor<IServiceProvider>>();
            this.services.AddSingleton(p => p.GetRequiredService<IObjectAccessor<IServiceProvider>>().Value);
            this.services.TryAddScoped<IInternetProvider, InternetProvider>();
        }
        public FakerBuilder UseCultureInfo(CultureInfo cultureInfo)
        {
            this._cultureInfo = cultureInfo ?? throw new NullReferenceException(nameof(cultureInfo));
            return this;
        }
        public FakerBuilder Configure(Action<FakerOptions> options)
        {
            options.Invoke(this._options);
            return this;
        }
        public FakerBuilder AddProvider(Type providerType)
        {
            this.services.AddScoped(providerType);
            return this;
        }
        public FakerBuilder AddProvider<TIProvider, TProvider>()
            where TIProvider : class, IProvider
            where TProvider : BaseProvider
        {
            this.services.AddScoped(typeof(TIProvider), typeof(TProvider));
            return this;
        }
        public FakerBuilder UseFactory(Type providerFactory)
        {
            if (!typeof(IProviderFactory).IsAssignableFrom(_type))
            {
                throw new NotSupportedException($"类型{_type.FullName}没有实现{nameof(IProviderFactory)}接口");
            }
            services.TryAddScoped(typeof(IProviderFactory), _type);
            return this;
        }
        #endregion
        public IFaker Build()
        {
            void ThrowNotFind(string dll, string providerType) => throw new DllNotFoundException(
                $"缺少 Faker 语言库实现包：{dll}，可前往 nuget 下载(如果有的话，没有的话请在Github上提交相应语言的实现)；如果存在 {dll} 依然报错（原因是环境问题导致反射不到类型），请在 UseFactory 第三个参数手工传入 typeof({providerType})");
            var languageName = _cultureInfo.ThreeLetterWindowsLanguageName;
            var assemblyName = $"Faker.Provider.{languageName}";
            var typeName = $"Faker.Provider.{languageName}.ProviderFactory";
            var fullName = $"{typeName},{assemblyName}";
            _type ??= Type.GetType(fullName);
            if (_type == null) ThrowNotFind(assemblyName, typeName);

            var providerTypes = _type.Assembly.GetExportedTypes().Where(x => typeof(IProvider).IsAssignableFrom(x));
            foreach (var provider in providerTypes)
            {
                var interfaces = provider.GetInterfaces();
                services.AddScoped(interfaces.Last(), provider);
            }

            services.AddSingleton(this._cultureInfo);
            services.AddSingleton(_options);
            services.AddSingleton(p => p.GetRequiredService<FakerOptions>().Provider);
            services.AddSingleton(p => p.GetRequiredService<FakerOptions>().Generator);
            services.TryAddScoped(typeof(IProviderFactory), _type);
            services.AddScoped<IGenerator, Generator>();
            services.AddScoped<IFaker, Faker>();
            var service = services.BuildServiceProvider();
            service.GetRequiredService<IObjectAccessor<IServiceProvider>>().Value = service;
            return service.GetRequiredService<IFaker>();
            //_generator ??= new Generator(_options.Generator);
            //var f = new Faker(new ReadOnlyCollection<Type>(this.customProviders), _type, this._cultureInfo, this._generator, this._options);
            //return f;
        }
    }
}