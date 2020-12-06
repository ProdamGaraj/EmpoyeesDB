using System;
using System.Linq;
using System.Text;

namespace EmpoyeesDB
{
    class Program
    {
        static private EmployeesContext _context = new EmployeesContext();
        static void Main(string[] args)
        {
            Console.WriteLine(MoveWorkers());
        }
        static string GetEmployeesInformation()
        {
            var employees = _context.Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle
                })
                .ToList();
            var sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle}");
            }

            return sb.ToString().TrimEnd();
        }

        static string GetHighSalaryWorkers()
        {
            var employees = _context.Employees.
                OrderBy(e => e.LastName).
                Where(e => e.Salary >= 48000).
                Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.HireDate,
                    e.Salary
                }).ToList();

            var sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.HireDate} {e.Salary}");
            }
            return sb.ToString().TrimEnd();
        }

        static string MoveWorkers()
        {
            var sb = new StringBuilder();
            _context.Towns.Add(new Towns() { Name = "Peteria" });
            _context.SaveChanges();

            var Browns = _context.Employees.Where(e => e.LastName.Equals("Brown")).Select(e=>e).ToList();
            _context.Addresses.Add(new Addresses()
            {
                AddressText = "There is nothing to read.",
                TownId = _context.Towns.AsEnumerable().LastOrDefault().TownId
            });
            _context.SaveChanges();
            foreach (var e in Browns)
            {
                e.AddressId = _context.Addresses.AsEnumerable().LastOrDefault().AddressId;
                _context.SaveChanges();
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.JobTitle} {e.HireDate} {e.Salary} {e.AddressId}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
