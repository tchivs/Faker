using System;
using System.Collections.Generic;

namespace Faker
{
    public interface IGenerator
    {
        Random Random { get; }
        /// <summary>
        /// 根据表达式生成随机字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        string Parse(string text);
        IEnumerable<TResult> Sample<TResult>(IList<TResult> elements, int length);
        IEnumerable<TResult> Unique<TResult>(IList<TResult> elements, int length);
     
        /// <summary>
        /// 处理权重
        /// </summary>
        /// <param name="weights"></param>
        /// <returns></returns>
        (double, int)[] GetWeightIndex(IList<double> weights);
        /// <summary>
        /// 通过权重获取随机索引
        /// </summary>
        /// <param name="widths"></param>
        /// <returns></returns>
        IEnumerable<int> GetRandomIndeiesByWidths(IList<double> widths);
        /// <summary>
        /// 通过解析的权重占比与索引中获取一个随机索引值
        /// </summary>
        /// <param name="widths"></param>
        /// <returns></returns>
        int GetRandomIndexByWidths((double, int)[] widths);
        /// <summary>
        /// 获取随机索引
        /// </summary>
        /// <param name="max">最大索引值</param>
        /// <returns></returns>
        int GetRandomIndex(int max);
       
    }
}