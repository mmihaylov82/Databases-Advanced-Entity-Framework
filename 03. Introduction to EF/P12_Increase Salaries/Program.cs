using System;
using System.IO;
using System.Linq;

using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

namespace P12_Increase_Salaries
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var context = new SoftUniContext())
            {
                using (StreamWriter sw = new StreamWriter("../../../../result.txt"))
                {
                    string[] departments = new string[] { "Engineering", "Tool Design", "Marketing", "Information Services" };

                    var employees = context.Employees
                         .Where(e => departments.Contains(e.Department.Name))
                         .Select(x => new Employee()
                         {
                             FirstName = x.FirstName,
                             LastName = x.LastName,
                             Salary = x.Salary
                         })
                         .OrderBy(x => x.FirstName)
                         .ThenBy(x => x.LastName)
                         .ToList();

                    foreach (var e in employees)
                    {
                        e.Salary *= 1.12m;
                        sw.WriteLine($"{e.FirstName} {e.LastName} (${e.Salary:F2})");
                    }

                    context.SaveChanges();
                }
            }
        }
    }
}
