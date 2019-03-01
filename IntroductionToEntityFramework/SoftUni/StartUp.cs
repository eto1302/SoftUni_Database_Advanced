using System;
using System.Linq;
using System.Text;
using SoftUni.Data;
using SoftUni.Models;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();
            Employee[] employees = context.Employees.ToArray();
            employees = employees.OrderBy(e => e.EmployeeId).ToArray();
            foreach (var employee in employees)
            {
                result.AppendLine(
                    $"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary.ToString("F2")}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();
            Employee[] employees = context.Employees.ToArray();
            employees = employees.Where(e => e.Salary > 50000).OrderBy(e => e.FirstName).ToArray();
            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.FirstName} - {employee.Salary.ToString("F2")}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();
            Employee[] employees = context.Employees.ToArray();
            employees = employees.Where(e => e.Department.Name == "Research and Development").OrderBy(e => e.Salary).ThenByDescending(e => e.FirstName).ToArray();
            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.Department.Name} - ${employee.Salary.ToString("F2")}");
            }

            return result.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();
            context.Addresses.Add(new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            });
            context.SaveChanges();
            context.Employees.FirstOrDefault(e => e.LastName == "Nakov").Address =
                context.Addresses.FirstOrDefault(a => a.AddressText == "Vitoshka 15");
            context.SaveChanges();
            Employee[] employees = context.Employees.ToArray();
            employees = employees.OrderByDescending(e => e.AddressId).Take(10).ToArray();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.Address.AddressText}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();


            var employees = context.Employees.
                Where(e => e.EmployeesProjects.Any(p => p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003))
                .Select(x => new
                {
                    EmployeeFullName = x.FirstName + " " + x.LastName,
                    ManagerFullName = x.Manager.FirstName + " " + x.Manager.LastName,
                    Projects = x.EmployeesProjects.Select(p => new
                    {
                        ProjectName = p.Project.Name,
                        StartDate = p.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt"),
                        EndDate = p.Project.EndDate
                    })
                })
                .Take(10).ToArray();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.EmployeeFullName} - Manager: {employee.ManagerFullName}");

                foreach (var project in employee.Projects)
                {
                    string endDateString = !project.EndDate.HasValue
                        ? "not finished"
                        : Convert.ToDateTime(project.EndDate).ToString("M/d/yyyy h:mm:ss tt");

                    result.AppendLine($"--{project.ProjectName} - {project.StartDate} - {endDateString}");

                }
            }

            return result.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();


            var addresses = context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .Select(a => new
                {
                    AddressText = a.AddressText,
                    TownName = a.Town.Name,
                    EmployeeCount = a.Employees.Count
                })
                .ToArray();

            foreach (var address in addresses)
            {
                result.AppendLine($"{address.AddressText}, {address.TownName} - {address.EmployeeCount} employees");
            }


            return result.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();


            var employee = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(x => new
                {
                    EmployeeFullName = x.FirstName + " " + x.LastName,
                    JobTitle = x.JobTitle,
                    Projects = x.EmployeesProjects.Select(p => new
                    {
                        ProjectName = p.Project.Name
                    })
                })
                .FirstOrDefault();

            result.AppendLine($"{employee.EmployeeFullName} - {employee.JobTitle}");
            foreach (var project in employee.Projects.OrderBy(p => p.ProjectName))
            {
                result.AppendLine($"{project.ProjectName}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();
            
            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    ManagerName = $"{d.Manager.FirstName} {d.Manager.LastName}",
                    Employees = d.Employees
                })
                .ToArray();

            foreach (var department in departments)
            {
                Console.WriteLine($"{department.DepartmentName} - {department.ManagerName}");
                foreach (var employee in department.Employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName))
                {
                    Console.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }

            return result.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(p => p.Name)
                .Select(p => new
                {
                    Name = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate
                })
                .ToArray();

            foreach (var project in projects)
            {
                result.AppendLine($"{project.Name}");
                result.AppendLine($"{project.Description}");
                result.AppendLine($"{project.StartDate}");
            }

            return result.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            foreach (var employee in context.Employees.Where(e => e.Department.Name == "Engineering"
                                                                  || e.Department.Name == "Marketing"
                                                                  || e.Department.Name == "Tool Design"
                                                                  || e.Department.Name == "Information Services").OrderBy(e => e.FirstName).ThenBy(e => e.LastName))
            {
                employee.Salary *= 1.12m;
                
                result.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary.ToString("F2")})");
            }
            context.SaveChanges();
            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();

            var employees = context.Employees.Where(e => e.FirstName.StartsWith("Sa"))
                .OrderBy(e => e.FirstName).ThenBy(e => e.LastName)
                .Select(e => new
                {
                    Name = $"{e.FirstName} {e.LastName}",
                    JobTitle = e.JobTitle,
                    Salary = e.Salary.ToString("F2")
                }).ToArray();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.Name} - {employee.JobTitle} - (${employee.Salary})");
            }

            return result.ToString().TrimEnd();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder result = new StringBuilder();
            context.EmployeesProjects.RemoveRange(context.EmployeesProjects.Where(p => p.ProjectId == 2));
            context.Projects.RemoveRange(context.Projects.Where(p => p.ProjectId == 2));
            foreach (var project in context.Projects.Take(10))
            {
                result.AppendLine(project.Name);
            }

            return result.ToString().TrimEnd();
        }

        public static string RemoveTown(SoftUniContext context)
        {
            foreach (var employee in context.Employees.Where(e => e.Address.Town.Name == "Seattle"))
            {
                employee.AddressId = null;
            }

            int addressesCount = context.Addresses.Where(a => a.Town.Name == "Seattle").ToArray().Length;
            context.Addresses.RemoveRange(context.Addresses.Where(a => a.Town.Name == "Seattle"));
            context.Towns.Remove(context.Towns.First(t => t.Name == "Seattle"));
            return $"{addressesCount} addresses in Seattle were deleted";
        }
    }
}
