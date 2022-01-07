using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Faker.Core
{

    public class Generator : IGenerator
    {
        private readonly GeneratorOptions _options;
        // public List<IProvider> Providers { get; set; } = new List<IProvider>();
        public Regex Regex { get; } = new Regex(@"\{\{\s*(\w+)(:\s*\w+?)?\s*\}\}");
        public Random Random { get; }

        public string Parse(string text)
        {
            var re = this.Regex.Replace(text, "");
            return re;
        }
        public IEnumerable<TResult> Sample<TResult>(IReadOnlyCollection<TResult> elements, int length)
        {
            for (int i = 0; i < length; i++)
            {
                var r = this.Random.Next(1, elements.Count);
                yield return elements.Skip(r).First();
            }
        }

        public Generator(GeneratorOptions options)
        {
            this._options = options;
            Random = _options.Seed.HasValue ? new Random(_options.Seed.Value) : new Random();
        }
    }
}