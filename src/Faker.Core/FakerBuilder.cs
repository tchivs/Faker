using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Faker.Core
{
    public class FakerBuilder
    {
        CultureInfo _cultureInfo = CultureInfo.CurrentCulture;
        private readonly FakerOptions _options = new FakerOptions();
        private IGenerator _generator;
        private Type _type;
        private List<Type> customProviders = new List<Type>();
        #region Builder Methods
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
            customProviders.Add(providerType);
            return this;
        }

        public FakerBuilder UseFactory(Type providerFactory)
        {
            this._type = providerFactory;
            if (!typeof(ProviderFactoryBase).IsAssignableFrom(_type))
            {
                throw new NotSupportedException($"类型{_type.FullName}没有实现{nameof(IProviderFactory)}接口");
            }

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
            _generator ??= new Generator(_options.Generator);
            var f = new Faker(new ReadOnlyCollection<Type>(this.customProviders),_type, this._cultureInfo, this._generator, this._options);
            return f;
        }
    }
}