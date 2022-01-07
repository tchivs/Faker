using System;
using System.Collections.Generic;

namespace Faker
{
    public interface IGenerator
    {
        Random Random { get; }
     //   void AddProvider(IProvider provider);
        /// <summary>
        /// 根据表达式生成随机字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        string Parse(string text);

        IEnumerable<TResult> Sample<TResult>(IReadOnlyCollection<TResult> elements, int length);
    }
}