using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;


namespace Otus.Teaching.PromoCodeFactory.Core.Domain.Administration
{
    public class Employee
        : BaseEntity
    {
        public Employee()
        {
        }

        private Employee(string firstName, string lastName, string email, List<Role> roles)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Roles = roles;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Email { get; set; }

        public List<Role> Roles { get; set; }

        public int AppliedPromocodesCount { get; set; }

        public static Result<Employee> Create(string firstName, string lastName, string email, List<Role> roles)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                return Result.Failure<Employee>("Имя не может быть пустым");
            }
            if (string.IsNullOrEmpty(lastName))
            {
                return Result.Failure<Employee>("Фамилия не может быть пустой");
            }
            if (string.IsNullOrEmpty(email))
            {
                return Result.Failure<Employee>("Почта не может быть пустой");
            }
            if(roles.Count == 0)
            {
                return Result.Failure<Employee>("Роли не определены");
            }

            var employee = new Employee(firstName, lastName, email, roles);
            return employee;
        }

    }
}