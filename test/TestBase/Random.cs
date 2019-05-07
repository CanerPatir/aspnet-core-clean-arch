using System; 
using System.Linq;

using AutoFixture;

namespace TestBase
{
    public static class Random<T>
    {
        private static readonly Fixture fixture = new Fixture();

        public static T _
        {
            get
            {
                if (typeof(T) == typeof(long))
                {
                    return (T) Convert.ChangeType(new Random().Next(), typeof(T)); // workaround to avoid autoFixture's similer long values 
                }
                return fixture.Create<Generator<T>>().First();
            }
        }
    }
}