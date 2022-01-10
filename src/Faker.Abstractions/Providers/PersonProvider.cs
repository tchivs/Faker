using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Faker
{
    public abstract class BasePersonProvider : BaseProvider<string>, IPersonProvider
    {
        protected virtual string Formats => $"{LastName()}{FirstName()}";
        protected Func<string> romanizedFormats;
        protected virtual string RomanizedFormats => romanizedFormats();
        IList<string> _firstNames;
        protected IList<string> FirstNames => _firstNames ??= FirstNamesMale.Concat(FirstNamesFeMale).ToArray();
        protected abstract SortedList<string, double> LastNames { get; }
        protected abstract string[] FirstNamesMale { get; }
        protected abstract string[] FirstNamesFeMale { get; }
        protected abstract string[] FirstRomanizedNames { get; }
        protected abstract string[] LastRomanizedNames { get; }
        public override void Dispose()
        {
            _firstNames = null;
        }

        public virtual string Name()
        {
            return Formats;
        }

        public virtual string FirstName()
        {
            return this.ChoicesOneSample(FirstNames);
        }

        public virtual string LastName()
        {
            return this.ChoicesOneFunc(LastNames);
        }
        public virtual string NameMale()
        {
            return $"{LastName()}{this.ChoicesOneSample(FirstNamesMale)}";
        }
        public string NameFemale()
        {
            return $"{LastName()}{this.ChoicesOneSample(FirstNamesFeMale)}";
        }
        public string RomanizedName()
        {
            return this.RomanizedFormats;
        }

        public string FirstRomanizedName()
        {
            return this.ChoicesOneSample(FirstRomanizedNames);
        }
        public string LastRomanizedName()
        {
            return this.ChoicesOneSample(LastRomanizedNames);
        }


        protected BasePersonProvider(CultureInfo cultureInfo, IGenerator generator, ProviderOptions options) : base(cultureInfo, generator, options)
        {
            this.romanizedFormats = () =>
             this.Options.Person.RomanizedWithSpace
                 ? $"{FirstRomanizedName()} {LastRomanizedName()}"
                 : $"{FirstRomanizedName()}{LastRomanizedName()}";
        }
    }
}
