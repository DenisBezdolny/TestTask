using TestTask.Domain.Entities;

namespace TestTask.Models
{
    public class EmployeeImportViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }
        public string Message { get; set; }
    }
}
