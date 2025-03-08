using TestTask.Domain.Entities;
using TestTask.Domain.Interfaces.Fabrics;

namespace TestTask.BLL.Factories
{
    public class EmployeeFactory : IEmployeeFactory
    {
        public Employee CreateEmployee(
            string payrollNumber,
            string forename,
            string surname,
            DateTime dateOfBirth,
            string phoneNumber,
            string mobilePhoneNumber,
            string streetAddress,
            string settlement,
            string postcode,
            string email,
            DateTime startDate)
        {
            
            return new Employee
            {
                PayrollNumber = payrollNumber,
                Forename = forename,
                Surname = surname,
                DateOfBirth = dateOfBirth,
                PhoneNumber = phoneNumber,
                MobilePhoneNumber = mobilePhoneNumber,
                StreetAddress = streetAddress,
                Settlement = settlement,
                Postcode = postcode,
                Email = email,
                StartDate = startDate
            };
        }
    }
}
