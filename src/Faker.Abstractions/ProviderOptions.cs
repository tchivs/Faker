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
}