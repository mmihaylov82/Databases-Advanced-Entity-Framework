using System;
using System.Linq;

using P02_DatabaseFirst.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace P08_Addresses_by_Town
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var dbContext = new SoftUniContext())
            {
                var addresses = dbContext.Addresses
                    .GroupBy(a => new
                    {
                        a.AddressId,
                        a.AddressText,
                        a.Town.Name
                    },
                        (key, group) => new
                        {
                            AddressText = key.AddressText,
                            Town = key.Name,
                            Count = group.Sum(a => a.Employees.Count)
                        })
                    .OrderByDescending(o => o.Count)
                    .ThenBy(o => o.Town)
                    .ThenBy(o => o.AddressText)
                    .Take(10)
                    .ToList();

                using (StreamWriter sw = new StreamWriter("../../../../result.txt"))
                {
                    foreach (var a in addresses)
                    {
                        sw.WriteLine($"{a.AddressText}, {a.Town} - {a.Count} employees");
                    }
                }
            }
        }
    }
}
