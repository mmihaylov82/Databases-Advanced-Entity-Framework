using System;
using System.Linq;

using P02_DatabaseFirst.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace P10_Departments_with
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var dbContext = new SoftUniContext())
            {
                var departments = dbContext.Departments
                    .Where(x => x.Employees.Count > 5)
                    .OrderBy(x => x.Employees.Count)
                    .ThenBy(x => x.Name)
                    .Select(x => new
                    {
                        DepartmentName = x.Name,
                        ManagerName = x.Manager.FirstName + " " + x.Manager.LastName,
                        Employees = x.Employees.Select(e => new
                        {
                            e.FirstName,
                            e.LastName,
                            e.JobTitle
                        })
                        .OrderBy(s => s.FirstName)
                        .ThenBy(s => s.LastName)
                    })
                    .ToArray();

                using (StreamWriter sw = new StreamWriter("../../../../result.txt"))
                {
                    foreach (var d in departments)
                    {
                        sw.WriteLine($"{d.DepartmentName} - {d.ManagerName}");

                        foreach (var e in d.Employees)
                        {
                            sw.WriteLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                        }

                        sw.WriteLine(new string('-', 10));
                    }
                }
            }
        }
    }
}
