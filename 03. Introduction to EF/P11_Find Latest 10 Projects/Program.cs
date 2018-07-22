using System;
using System.IO;
using System.Linq;

using P02_DatabaseFirst.Data;

namespace P11_Find_Latest_10
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var dbContext = new SoftUniContext())
            {
                using (StreamWriter sw = new StreamWriter("../../../../result.txt"))
                {
                    dbContext.Projects.
                    OrderByDescending(p => p.StartDate).
                    Take(10).
                    Select(p => new { p.Name, p.Description, p.StartDate })
                    .OrderBy(p => p.Name)
                    .ToList()
                    .ForEach(p => sw.WriteLine($"{p.Name}{Environment.NewLine}{p.Description}{Environment.NewLine}{p.StartDate}"));
                }
                
            }
        }
    }
}
