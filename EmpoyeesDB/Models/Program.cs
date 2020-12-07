using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace EmpoyeesDB
{
    class Program
    {
        static private EmployeesContext _context = new EmployeesContext();
        static void Main(string[] args)
        {
            Console.WriteLine(ProjectAudit());
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

            var Browns = _context.Employees.Where(e => e.LastName.Equals("Brown")).Select(e => e).ToList();
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

        static string ProjectAudit()
        {
            var result =
                _context.EmployeesProjects.Join(_context.Employees,
                (ep => ep.EmployeeId),
                (e => e.EmployeeId),
                (ep, e) => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Manager = e.Manager,
                    ProjectName = ep.Project.Name,
                    StartDate = ep.Project.StartDate,
                    EndDate = ep.Project.EndDate
                }).
                Where(e => e.StartDate.Year >= 2002 && e.StartDate.Year <= 2005).
                ToList();
            string NullEndDate = "НЕ ЗАВЕРШЕН\n";
            var sb = new StringBuilder();
            int i = 0;
            int j = 0;
            foreach (var r in result)
            {
                if (i == 0||(r.FirstName != result[i - 1].FirstName && r.LastName != result[i - 1].LastName && j > 0 && j < 5))
                {
                    if (r.EndDate == null)
                    {
                        sb.Append($"{r.FirstName} {r.LastName} {r.Manager.FirstName} {r.Manager.LastName} {r.ProjectName} {r.StartDate} {NullEndDate}");
                    }
                    else
                    {
                        sb.Append($"{r.FirstName} {r.LastName} {r.Manager.FirstName} {r.Manager.LastName} {r.ProjectName} {r.StartDate} {r.EndDate}" + "\n");
                    }
                    j++;
                }
                i++;
            }
            return sb.ToString().TrimEnd();
        }

        static string ManagerProfile()
        {
            int id = Convert.ToInt32(Console.ReadLine());
            var result =
                _context.EmployeesProjects.Join(_context.Employees,
                (ep => ep.EmployeeId),
                (e => e.EmployeeId),
                (ep, e) => new
                {
                    EmployeeId = e.EmployeeId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    MiddleName = e.MiddleName,
                    ProjectName = ep.Project.Name
                }).
                Where(e => e.EmployeeId == id).
                ToList();
            var sb = new StringBuilder();
            sb.Append($"{result[0].FirstName} {result[0].LastName} {result[0].MiddleName} " + " \n");
            foreach (var r in result)
            {
                sb.Append($"{r.ProjectName}"+"\n");
            }
            return sb.ToString().TrimEnd();
        }

    }
}
