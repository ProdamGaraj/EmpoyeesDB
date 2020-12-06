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
            Console.WriteLine(GetHighSalaryWorkers());
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
                    e.DepartmentId,
                    e.ManagerId,
                    e.HireDate,
                    e.AddressId
                }).ToList();

            var sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.DepartmentId} {e.ManagerId} {e.HireDate} {e.AddressId}");
            }
            return sb.ToString().TrimEnd();
        }

        static string MoveWorkers()
        {
            


            var sb = new StringBuilder();

            return sb.ToString().TrimEnd();
        }
    }
}