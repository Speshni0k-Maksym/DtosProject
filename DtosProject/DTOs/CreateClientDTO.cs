using System.ComponentModel.DataAnnotations;

namespace DtosProject.DTOs
{
    public class CreateClientDTO
    {
        [Required(ErrorMessage = "Поле FullName є обов'язковим")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Поле Email є обов'язковим")]
        [EmailAddress(ErrorMessage = "Невірний формат Email")]
        public string Email { get; set; }

        [Range(1, 120, ErrorMessage = "Вік має бути від 1 до 120")]
        public int Age { get; set; }
    }
}