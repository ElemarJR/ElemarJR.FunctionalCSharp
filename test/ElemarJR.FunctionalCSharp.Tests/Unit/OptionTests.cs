using Xunit;

namespace ElemarJR.FunctionalCSharp.Tests
{
    using static Helpers;
    public class OptionTests
    {
        [Fact]
        public void OptionSupportsImplicitConversion()
        {
            Option<int> a = 5;
            Assert.True(a.IsSome);
        }

        [Fact]
        public void MatchInvokesSomeBranchWithSome()
        {
            var invokedWithRightValue = false;
            Option<int> a = 5;
            a.Match(
                some: (value) => invokedWithRightValue = value == 5,
                none: () => Unit()
            );
            Assert.True(invokedWithRightValue);
        }

        [Fact]
        public void MatchInvokesNoneBranchWithNone()
        {
            var invokedWithNone = false;
            Option<int> a = None;
            a.Match(
                some: _ => Unit(),
                none: ToFunc( () => invokedWithNone = true )
            );
            Assert.True(invokedWithNone);
        }
        
    }
}
