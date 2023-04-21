using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementMVCAPP.Models
{
    public class AddUpdateEmployeeModel
    {
        [Display(Name = "Emp_ID")]

        public long Emp_ID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(150)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]

        public string Email { get; set; }

        [Required(ErrorMessage = "DateOfBirth is required.")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(50)]
        public string Department { get; set; }

    }
   
}
