using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Models
{
    public class CreateEmployeeRequest
    {
        public CreateEmployeeRequest()
        {
            
        }
        public CreateEmployeeRequest(string firstName, string lastName, string email, List<string> roleNames)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            RoleNames = roleNames;
        }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public List<string> RoleNames { get; set; }
    }
}
