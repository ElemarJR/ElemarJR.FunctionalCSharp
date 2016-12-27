using System;
using Xunit;

namespace ElemarJR.FunctionalCSharp.Tests.TrySamples
{
    class Employee { }

    class DbContext
    {
        public Employee Find(string id)
        {
            if (id == "e") throw new Exception();
            if (id == "1") return new Employee();
            return null;
        } 
    }

    class FakeEmployeeRepository
    {
        public Try<Exception, Employee> GetById(string id)
            => Try.Run(() => new DbContext().Find(id))
                .Bind(e => e ?? (Try<Exception, Employee>) new Exception());
    }

    public class DealingWithRepositories
    {
        [Fact]
        public void WhenDbContextThrowsAnException()
        {
            var result = new FakeEmployeeRepository().GetById("e");
            Assert.True(result.IsFailure);
        }

        [Fact]
        public void WhenDbContextReturnsEntityInstance()
        {
            var result = new FakeEmployeeRepository().GetById("1");
            Assert.True(result.IsSucess);
        }

        [Fact]
        public void WhenDbContextReturnsNullResultsException()
        {
            var result = new FakeEmployeeRepository().GetById("2");
            Assert.True(result.IsFailure);
        }
    }
}
