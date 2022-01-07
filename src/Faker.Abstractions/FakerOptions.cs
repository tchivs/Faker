using System.Globalization;

namespace Faker
{
    public class FakerOptions
    {
       
        public GeneratorOptions Generator { get; set; } = new GeneratorOptions();
        public ProviderOptions Provider { get; set; } = new ProviderOptions();

    }
}