using TestTask.Domain.Entities;

namespace TestTask.Domain.Interfaces.Fabrics
{
    public interface IEmployeeFactory
    {
        Employee CreateEmployee(
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
            DateTime startDate);
    }
}
