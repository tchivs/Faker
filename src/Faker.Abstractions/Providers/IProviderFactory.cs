namespace Faker
{
    public interface IProviderFactory
    {
        IPersonProvider CreatePerson();
        TProvider CreateProvider<TProvider>() where TProvider : BaseProvider;
    }
}