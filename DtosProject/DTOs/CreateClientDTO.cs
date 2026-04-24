using System.ComponentModel.DataAnnotations;

namespace DtosProject.DTOs
{
    public class CreateClientDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }
}