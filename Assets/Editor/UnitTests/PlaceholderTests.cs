using FluentAssertions;
using NUnit.Framework;

namespace Assets.Editor.UnitTests
{
    [TestFixture]
    public class PlaceholderTests
    {
        [TestCase]
        public void PlaceHolderTestSuccess()
        {
            "Ludum Dare is cool!!!".Should().NotBeEmpty();
        }
    }
}
