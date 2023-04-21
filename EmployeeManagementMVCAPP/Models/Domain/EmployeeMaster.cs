using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementMVCAPP.Models.Domain
{
    public class EmployeeMaster
    {
        [Key]
        public long Emp_ID { get; set; }
        public string Name { get; set; }
        public string  Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Department { get; set; }

    }
}
