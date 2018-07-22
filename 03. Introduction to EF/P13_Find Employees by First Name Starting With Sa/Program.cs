using System;
using System.IO;
using System.Linq;

using P02_DatabaseFirst.Data;

namespace P13_Find_Employees
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var dbContext = new SoftUniContext())
            {
                using (StreamWriter sw = new StreamWriter("../../../../result.txt"))
                {
                    dbContext.Employees
                   .Where(e => e.FirstName.Substring(0, 2) == "Sa")
                   .Select(e => new { e.FirstName, e.LastName, e.JobTitle, e.Salary })
                   .OrderBy(e => e.FirstName)
                   .ThenBy(e => e.LastName)
                   .ToList()
                   .ForEach(e => sw.WriteLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})"));
                }
               
            }
        }
    }
}
