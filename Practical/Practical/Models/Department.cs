using System.ComponentModel;

namespace Practical.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }
}
