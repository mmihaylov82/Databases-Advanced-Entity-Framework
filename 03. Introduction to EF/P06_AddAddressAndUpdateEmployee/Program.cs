using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;
using System;
using System.IO;
using System.Linq;

namespace P06_AddAddressAndUpdateEmployee
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                Address address = new Address()
                {
                    AddressText = "Vitoshka 15",
                    TownId = 4
                };
                
                var nakov = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");

                nakov.Address = address;

                context.SaveChanges();

                var addresses = context.Employees
                    .OrderByDescending(x => x.AddressId)
                    .Select(x => x.Address.AddressText)
                    .Take(10)
                    .ToList();

                using (StreamWriter sw = new StreamWriter("../../../../result.txt"))
                {
                    foreach (var a in addresses)
                    {
                        sw.WriteLine($"{a}");
                    }
                }
            }
        }
    }
}
