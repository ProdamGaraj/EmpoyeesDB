using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EmpoyeesDB
{
    class Program
    {
        static public EmployeesContext _context { get; } = new EmployeesContext();
        static void Main(string[] args)
        {
            Console.WriteLine();//Type any of task names right here.
        }
        static string GetEmployeesInformation()
        {


            var employees = (
                from e in _context.Employees
                orderby e.LastName
                where e.Salary > 48000
                select e
                ).ToList();
            var sb = new StringBuilder();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle}");
            }

            return sb.ToString().TrimEnd();
        }

        static string GetHighSalaryWorkers()
        {
            var employees = (
                from e in _context.Employees
                orderby e.LastName
                where e.Salary > 48000
                select e
            ).ToList();

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

            var Browns = (
                from employee in _context.Employees
                where employee.LastName == "Brown"
                select employee
            ).ToList();

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
                from Employees in _context.Employees
                join EmployeesProjects in _context.EmployeesProjects on Employees.EmployeeId equals EmployeesProjects.EmployeeId
                join Projects in _context.Projects on EmployeesProjects.ProjectId equals Projects.ProjectId
                where EmployeesProjects.Project.StartDate.Year >= 2002 && EmployeesProjects.Project.StartDate.Year <= 2005
                select new { Employees, Projects };


            string NullEndDate = "НЕ ЗАВЕРШЕН\n";
            var sb = new StringBuilder();
            int i = 0;
            int j = 0;
            foreach (var r in result)
            {
                if (i == 0 || (r.Employees.FirstName != result.AsEnumerable().ElementAt(i - 1).Employees.FirstName && r.Employees.LastName != result.AsEnumerable().ElementAt(i - 1).Employees.LastName && j > 0 && j < 5))
                {
                    if (r.Projects.EndDate != null)
                    {
                        sb.Append($"{r.Employees.FirstName} {r.Employees.LastName} {r?.Employees.Manager.FirstName} {r?.Employees.Manager.LastName} {r.Projects.Name} {r.Projects.StartDate} {NullEndDate}");
                    }
                    else
                    {
                        sb.Append($"{r.Employees.FirstName} {r.Employees.LastName} {r?.Employees.Manager.FirstName} {r?.Employees.Manager.LastName} {r.Projects.Name} {r.Projects.StartDate} {r.Projects.EndDate}" + "\n");
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
                from Employees in _context.Employees
                join EmployeesProjects in _context.EmployeesProjects on Employees.EmployeeId equals EmployeesProjects.EmployeeId
                join Projects in _context.Projects on EmployeesProjects.ProjectId equals Projects.ProjectId
                where (Employees.EmployeeId == id)
                select new { Employees, Projects };

            var sb = new StringBuilder();
            sb.Append($"{result.AsEnumerable().ElementAt(0).Employees.FirstName} {result.AsEnumerable().ElementAt(0).Employees.LastName} {result.AsEnumerable().ElementAt(0).Employees.MiddleName} " + " \n");
            foreach (var r in result)
            {
                sb.Append($"{r.Projects.Name}" + "\n");
            }
            return sb.ToString().TrimEnd();
        }

        static string LitleDepartaments()
        {
            var result =
                from Departments in _context.Departments
                where Departments.Employees.Count < 5
                select new { Departments };

            var sb = new StringBuilder();

            foreach (var r in result)
            {
                sb.Append($"{r.Departments.Name}" + "\n");
            }

            return sb.ToString().TrimEnd();
        }

        static string SalaryIncrease()
        {
            Console.WriteLine("Vvedite otdel v kotorom nado povisit zarplatu.");
            int DepartamentId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Vvedite % povisheniya zarplat?");
            int IncreasePercent = Convert.ToInt32(Console.ReadLine());
            var result =
                from Employees in _context.Employees
                where Employees.DepartmentId == DepartamentId
                select new { Employees };

            var sb = new StringBuilder();

            foreach (var r in result)
            {
                sb.Append($"{r.Employees.FirstName} {r.Employees.LastName} {r.Employees.Salary} " + "\n");
                r.Employees.Salary *= (decimal)((IncreasePercent + 100) / 100f);
            }
            sb.Append("New salaries \n");
            _context.SaveChanges();
            foreach (var r in result)
            {
                sb.Append($"{r.Employees.FirstName} {r.Employees.LastName} {r.Employees.Salary} " + "\n");
            }
            return sb.ToString().TrimEnd();
        }

        static string PanicMethod()
        {
            string DepartmentName = Console.ReadLine();
            var department = _context.Departments.First(d => d.Name == DepartmentName);
            Employees TempEmpl = new Employees
            {
                FirstName = "Temp",
                LastName = "Temp",
                JobTitle = "Temp",
                DepartmentId = 2,
                HireDate = new DateTime(2008, 5, 1),
                Salary = 0
            };
            Departments TempDep = new Departments { Name = "Temp", Manager = TempEmpl };
            _context.Departments.Add(TempDep);
            _context.SaveChanges();
            TempEmpl.Department = TempDep;
            _context.SaveChanges();
            if (department == null)
            {
                Console.WriteLine("It`s nothing to search in here.");
                return null;
            }
            var employees =
                from Employees in _context.Employees
                where Employees.Department.Name == DepartmentName
                select new { Employees };
            foreach (var e in employees)
            {
                e.Employees.Department = _context.Departments.First(d => d == TempDep);
            }
            department.Manager = null;
            _context.SaveChanges();
            _context.Departments.Remove(department);
            _context.SaveChanges();
            return "Keep panic away it`s gone.";
        }
        static string Town404()
        {
            string TownName = Console.ReadLine();

            var TownToDelete =
                from Towns in _context.Towns
                where Towns.Name == TownName
                select Towns;
            Addresses Adr = new Addresses { AddressText = "Temp" };
            var AddressesToDelete =
                from Adresses in _context.Addresses
                where Adresses.Town == TownToDelete
                select Adresses;
            var Employees = 
                from e in _context.Employees
                where e.Address.Town == TownToDelete
                select  e;
            foreach (var e in Employees)
            {
                e.Address = Adr;
            }
            _context.SaveChanges();
            foreach (var a in AddressesToDelete)
            {
                _context.Addresses.Remove(a);
            }
            _context.SaveChanges();
            foreach (var t in TownToDelete)
            {
                _context.Towns.Remove(t);
            }
            _context.SaveChanges();
            return TownName + " deleted";
        }
    }
}
