using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Faker.Core
{

    public class Generator : IGenerator
    {
        private readonly GeneratorOptions _options;
        public Regex Regex { get; } = new Regex(@"\{\{\s*(\w+)(:\s*\w+?)?\s*\}\}");
        public Random Random { get; }
        public string Parse(string text)
        {
            var re = this.Regex.Replace(text, "");
            return re;
        }
        public IEnumerable<TResult> Sample<TResult>(IList<TResult> elements, int length)
        {
            for (int i = 0; i < length; i++)
            {
                var r = this.Random.Next(1, elements.Count);
                yield return elements[r];
            }
        }
        public IEnumerable<TResult> Unique<TResult>(IList<TResult> elements, int length)
        {
            var l = new SortedList<Guid, TResult>();
            foreach (var element in elements)
            {
                l.Add(Guid.NewGuid(), element);
            }
            return l.OrderBy(x => x.Key).Select(x => x.Value);
        }
        public Generator(GeneratorOptions options)
        {
            this._options = options;
            Random = _options.Seed.HasValue ? new Random(_options.Seed.Value) : new Random();
        }

        /// <summary>
        /// 对权重进行处理
        /// </summary>
        /// <param name="weights"></param>
        /// <returns>小权重的占比和大权重的索引</returns>
        public (double, int)[] GetWeightIndex(IList<double> weights)
        {
            weights.GetHashCode();
            var total = weights.Sum();//计算总和
            var average = 1f * total / weights.Count;//平均权重
            var PrepareAdRewardWeight = new (double, int)[weights.Count];
            List<(double, int)> smallAvg = new List<(double, int)>();
            List<(double, int)> bigAvg = new List<(double, int)>();
            for (int i = 0; i < weights.Count; i++)
            {
                (weights[i] > average ? bigAvg : smallAvg).Add((weights[i], i));
            }
            for (int i = 0; i < weights.Count; i++)
            {
                if (smallAvg.Count > 0)
                {
                    if (bigAvg.Count > 0)
                    {
                        PrepareAdRewardWeight[smallAvg[0].Item2] = (smallAvg[0].Item1 / average, bigAvg[0].Item2);
                        bigAvg[0] = (bigAvg[0].Item1 - average + smallAvg[0].Item1, bigAvg[0].Item2);
                        if (average - bigAvg[0].Item1 > 0.0000001f)
                        {
                            smallAvg.Add(bigAvg[0]);
                            bigAvg.RemoveAt(0);
                        }
                    }
                    else
                    {
                        PrepareAdRewardWeight[smallAvg[0].Item2] = (smallAvg[0].Item1 / average, smallAvg[0].Item2);
                    }
                    smallAvg.RemoveAt(0);
                }
                else
                {
                    PrepareAdRewardWeight[bigAvg[0].Item2] = (bigAvg[0].Item1 / average, bigAvg[0].Item2);
                    bigAvg.RemoveAt(0);
                }
            }
            return PrepareAdRewardWeight;
        }



        public int GetRandomIndexByWidths((double, int)[] width)
        {
            var randomNum = this.Random.NextDouble() * width.Length;
            int intRan = (int)Math.Floor(randomNum);
            var p = width[intRan];
            int index;
            if (p.Item1 > randomNum - intRan)
            {
                index = intRan;
            }
            else
            {
                index = p.Item2;
            }
            return index;
        }
        public int GetRandomIndex(int maxIndex)
        {
            return this.Random.Next(1, maxIndex);
        }
        public IEnumerable<int> GetRandomIndeiesByWidths(IList<double> width)
        {
            var rewardWeight = GetWeightIndex(width);
            yield return GetRandomIndexByWidths(rewardWeight);
        }
    }
}