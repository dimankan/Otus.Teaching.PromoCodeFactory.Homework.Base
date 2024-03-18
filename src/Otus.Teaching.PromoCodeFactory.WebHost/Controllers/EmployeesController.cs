using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController
        : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Role> _roleRepository;

        public EmployeesController(IRepository<Employee> employeeRepository, IRepository<Role> roleRepository)
        {
            _employeeRepository = employeeRepository;
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x => 
                new EmployeeShortResponse()
                    {
                        Id = x.Id,
                        Email = x.Email,
                        FullName = x.FullName,
                    }).ToList();

            return employeesModelList;
        }
        
        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();
            
            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }

        /// <summary>
        /// Создать данные сотрудника
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateEmployeeRequest request)
        {
            if(request.RoleNames.Count == 0)
                return BadRequest("Роль не указана");

            var allRoles = await _roleRepository.GetAllAsync();
            var employeeRoles = allRoles.Where(x=>request.RoleNames.Contains(x.Name)).ToList();

            if (employeeRoles.Count == 0)
                return BadRequest($"Нет доступных ролей. Выберите из списка: {string.Join(", ",allRoles.Select(x=>x.Name).Distinct())}") ;

            var employee = Employee.Create(request.FirstName, request.LastName, request.Email, employeeRoles);
            if (employee.IsFailure)
            {
                return BadRequest(employee.Error);
            }

            var saveEmployee = await _employeeRepository.AddAsync(employee.Value);

            return Ok(saveEmployee);
        }

        /// <summary>
        /// Обновить данные сотрудника
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{employeeId:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid employeeId, UpdateEmployeeRequest request)
        {
            if (request.RoleNames.Count == 0)
                return BadRequest("Роль не указана");

            var allRoles = await _roleRepository.GetAllAsync();
            var employeeRoles = allRoles.Where(x => request.RoleNames.Contains(x.Name)).ToList();

            if (employeeRoles.Count == 0)
                return BadRequest($"Нет доступных ролей. Выберите из списка: {string.Join(", ", allRoles.Select(x => x.Name).Distinct())}");

            var employee = Employee.Create(request.FirstName, request.LastName, request.Email, employeeRoles);
            if (employee.IsFailure)
            {
                return BadRequest(employee.Error);
            }

            var updateEmployee = await _employeeRepository.UpdateAsync(employeeId, employee.Value);

            return Ok(updateEmployee);
        }

        /// <summary>
        /// Удалить сотрудника по Id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpDelete("{employeeId:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid employeeId)
        {
            string resultDelete = await _employeeRepository.DeleteAsync(employeeId);

            return Ok(resultDelete) ;
        }
    }
}