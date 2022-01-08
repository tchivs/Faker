﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Faker
{
    public class ProviderOptions
    {
        public bool UseWeighting { get; set; }
        public PersonOption Person { get; set; } = new PersonOption();
        public bool Unique { get;   set; }
    }

    public class PersonOption
    {
        /// <summary>
        /// 名称中间是否需要空格
        /// </summary>
        public bool RomanizedWithSpace { get; set; } = true;
    }
    public class GeneratorOptions
    {
        /// <summary>
        /// 随机种子
        /// </summary>
        public int? Seed { get; set; }
    }

    public static class TypeExtensions
    {
        public static TInstance CreateInstance<TInstance>(this Type type, int? argsCount, params object[] arguments)
        {
            NewExpression newExpression = null;

            if (argsCount == null)
            {
                var ctors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);//获取构造函数
                foreach (var constructorInfo in ctors)
                {
                    var ps = constructorInfo.GetParameters().Select(x => x.ParameterType).ToArray();//获取参数
                    var skip = arguments.Length - ps.Length;
                    newExpression = Expression.New(constructorInfo, arguments.Skip(skip).Take(ps.Length).Select(Expression.Constant));
                }
            }
            else if (arguments.Length == argsCount)
            {
                var constructorInfo = type.GetConstructors().First(x => x.GetParameters().Length == argsCount);
                newExpression = Expression.New(constructorInfo, arguments.Select(Expression.Constant));
            }
            if (newExpression == null)
            {
                throw new NullReferenceException($"{type.FullName} Ctor is not Create");
            }
            var lambda = Expression.Lambda<Func<TInstance>>(newExpression);
            var instance = lambda.Compile().Invoke();
            return instance;
        }
        public static TInstance CreateInstance<TInstance>(this Type type, params object[] arguments)
        {
            return CreateInstance<TInstance>(type, null, arguments);
        }
    }
}