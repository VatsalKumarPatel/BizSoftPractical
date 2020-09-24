using Practical.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practical.Models
{
    public class EmployeeListModel
    {
        public int EmployeeId { get; set; }

        public string Department { get; set; }

        public string EmployeeName { get; set; }

        public DateTime BirthDate { get; set; }

        public Gender Sex { get; set; }

        public bool IsActive { get; set; }

        [NotMapped]
        public int Age
        {
            get
            {
                int age = 0;
                age = DateTime.Now.Year - BirthDate.Year;
                if (DateTime.Now.DayOfYear < BirthDate.DayOfYear)
                    age = age - 1;

                return age;
            }
        }
    }
}
