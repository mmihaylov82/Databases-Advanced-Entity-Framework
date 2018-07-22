using System;
using System.Linq;

using P02_DatabaseFirst.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace P14_Delete_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var dbContext = new SoftUniContext())
            {
                using (StreamWriter sw = new StreamWriter("../../../../result.txt"))
                {
                    var project = dbContext.Projects.First(p => p.ProjectId == 2);

                    dbContext.EmployeesProjects.ToList().ForEach(ep => dbContext.EmployeesProjects.Remove(ep));
                    dbContext.Projects.Remove(project);

                    dbContext.SaveChanges();

                    dbContext.Projects.Take(10).Select(p => p.Name).ToList().ForEach(p => sw.WriteLine(p));
                }
            }
        }
    }
}
