using System;
using System.Linq;
using ElemarJR.FunctionalCSharp;

namespace Playground
{
    class Program
    {
        static DbContext dbContext = new DbContext();
        static void Main(string[] args)
        {
            // planejamento do fluxo.
            var promise =
                from userInput in AskEmployeeId()
                from parsedInput in ParseId(userInput)
                from user in GetEmployeeById(parsedInput)
                select user;
                
            // execução.
            var result = promise.Run();

            // resultado.
            result.Match(
                failure: exception => Console.WriteLine($"Failed to get employee - {exception}"),
                success: employee => Console.WriteLine($"{employee.Id} - {employee.Name}")
            );
        }

        public static PromiseOfTry<string> AskEmployeeId()
            => () => Console.ReadLine();

        public static PromiseOfTry<int> ParseId(string s) =>
            () => int.Parse(s);

        public static PromiseOfTry<Employee> GetEmployeeById(int id)
            => () => dbContext.Find(id);

    }

    class DbContext
    {
        public Employee Find(int id)
        {
            if (id == 0) throw new Exception();
            if (id == 1) return new Employee() { Id = 1, Name = "Elemar"};
            return null;
        }
    }

    internal class Employee
    {
        public int Id { get; set;  }
        public string Name { get; set; }
    }
}
