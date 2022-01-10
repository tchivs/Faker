using System;
using System.Globalization;

namespace Faker
{
    public interface IFaker : IDisposable
    {

        IPersonProvider Person { get; }
        IInternetProvider Internet { get; }
        /// <summary>
        /// 创建一个自定义的Provider
        /// </summary>
        /// <typeparam name="TProvider"></typeparam>
        /// <returns></returns>
        TProvider CreateProvider<TProvider>() where TProvider : IProvider;
    }
}