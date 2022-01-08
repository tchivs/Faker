namespace Faker.Provider.CHS
{
    public class ProviderFactory : ProviderFactoryBase
    {
        public ProviderFactory(IGenerator generator, ProviderOptions options) : base(System.Globalization.CultureInfo.GetCultureInfo("zh-CN"), generator, options)
        {

        }
        public override IPersonProvider CreatePerson()
        {
            return new PersonProvider(this.Generator, this.Options);
        }
    }
}