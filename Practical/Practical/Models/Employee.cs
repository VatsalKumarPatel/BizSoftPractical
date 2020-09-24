using Practical.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practical.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [ForeignKey("Department")]
        [Range(1, int.MaxValue, ErrorMessage = "Please Select Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        [Required(ErrorMessage  = "Please enter Employee Name")]
        public string EmployeeName { get; set; }

        public string Address { get; set; }

        [Display(Name = "Birth Date")]
        [Required(ErrorMessage = "Please enter Birth Date")]
        public DateTime BirthDate { get; set; }

        public Gender Sex { get; set; }

        public bool IsActive { get; set; }
    }
}
