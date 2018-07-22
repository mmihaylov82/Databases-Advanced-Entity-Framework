using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;
using System;
using System.IO;
using System.Linq;

namespace P04_Employees_with_Salary_Over_50_000
{
    class StartUp
    {
        static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                var employees = context.Employees
                    .OrderBy(e => e.FirstName)
                    .Where(e => e.Salary > 50000)
                    .Select(x => new { x.FirstName })
                    .ToList();

                using (StreamWriter sw = new StreamWriter("../../../result.txt"))
                {
                    foreach (var e in employees)
                    {
                        sw.WriteLine($"{e.FirstName}");
                    }
                }
            }
        }
    }
}
