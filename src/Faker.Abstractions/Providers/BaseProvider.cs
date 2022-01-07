using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

// ReSharper disable once CheckNamespace
namespace Faker
{
    public interface IProvider : IDisposable
    {
    }
    public interface IProvider<out TResult> : IProvider
    {
        TResult this[string name] { get; }
    }

    public abstract class BaseProvider : IProvider
    {
        protected CultureInfo CultureInfo { get; }
        protected IGenerator Generator { get; }

        protected ProviderOptions Options { get; }

        protected BaseProvider(CultureInfo cultureInfo, IGenerator generator, ProviderOptions options)
        {
            CultureInfo = cultureInfo;
            Generator = generator;
            this.Options = options;
        }
        public abstract void Dispose();
    }
    public abstract class BaseProvider<TResult> : BaseProvider, IProvider<TResult>
    {


        protected TResult RandomElement(IReadOnlyCollection<TResult> elements)
        {
            return this.RandomElements(elements, 1).First();
        }
        protected IEnumerable<TResult> RandomElements(IReadOnlyCollection<TResult> elements, int? length = null, bool? useWeighting = false, bool unique = false)
        {
            useWeighting ??= this.Options.UseWeighting;
            var len = length ??= this.Generator.Random.Next(1, elements.Count);
            Func<IReadOnlyCollection<TResult>, int, IEnumerable<TResult>> fn = null;
            if (unique)
            {
                fn = ChoicesDistributionUnique;
            }
            else
            {
                fn = ChoicesDistribution;
            }
            if (unique && length > elements.Count)
            {
                throw new Exception("Sample length cannot be longer than the number of unique elements to pick from.");
            }
            return fn.Invoke(elements, len);
        }

        protected virtual IEnumerable<TResult> ChoicesDistributionUnique(IReadOnlyCollection<TResult> elements,
            int length)
        {
            return this.Generator.Sample(elements, length);
        }


        protected virtual IEnumerable<TResult> ChoicesDistribution(IReadOnlyCollection<TResult> elements, int length)
        {
            return this.Generator.Sample(elements, length);
        }

        public TResult this[string name] => GetOrCreateIndexFunction(name).Invoke();
        readonly Dictionary<string, Func<TResult>> _lambdaCache = new Dictionary<string, Func<TResult>>();
        protected Func<TResult> GetOrCreateIndexFunction(string methodName)
        {
            if (!_lambdaCache.TryGetValue(methodName ?? throw new NullReferenceException(nameof(methodName)), out var lambda))
            {
                var method = this.GetType().GetMethod(methodName);
                if (method == null)
                {
                    throw new MissingMethodException(nameof(methodName));
                }

                var x = Expression.Constant(this);
                var call = Expression.Call(x, method);
                var expression = Expression.Lambda<Func<TResult>>(call);

                lambda = expression.Compile();
                _lambdaCache.Add(methodName, lambda);
            }
            return lambda;

        }

        protected BaseProvider(CultureInfo cultureInfo, IGenerator generator, ProviderOptions options) : base(cultureInfo, generator, options)
        {
        }
    }
}