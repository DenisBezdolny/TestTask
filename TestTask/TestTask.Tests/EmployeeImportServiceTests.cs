using System.Globalization;
using Moq;
using TestTask.BLL.Services;
using TestTask.Domain.Entities;
using TestTask.Domain.Interfaces.Fabrics;
using TestTask.Domain.Interfaces.Repositories;
using Xunit;
using Microsoft.Extensions.Logging;

namespace TestTask.Tests
{
    public class EmployeeImportServiceTests
    {
        [Fact]
        public async Task ImportEmployeeAsync_ShouldReturnSuccessCount()
        {
            // Arrange
            var employeeFactoryMock = new Mock<IEmployeeFactory>();
            var repositoryMock = new Mock<IRepository<Employee>>();
            var loggerMock = new Mock<ILogger<EmployeeImportService>>();

            // Setup the employee factory to return a new Employee instance when called.
            employeeFactoryMock
                .Setup(f => f.CreateEmployee(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>()))
                .Returns((string payroll, string forename, string surname, DateTime dob,
                          string phone, string mobile, string address, string address2,
                          string postcode, string email, DateTime startDate) =>
                          new Employee
                          {
                              PayrollNumber = payroll,
                              Forename = forename,
                              Surname = surname,
                              DateOfBirth = dob,
                              PhoneNumber = phone,
                              MobilePhoneNumber = mobile,
                              StreetAddress = address,
                              Settlement = address2,
                              Postcode = postcode,
                              Email = email,
                              StartDate = startDate
                          });

            // Setup the repository to simply complete when CreateAsync is called.
            repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<Employee>()))
                .Returns(Task.CompletedTask);

            // Create a sample CSV string with one employee record.
            string csvData =
@"Personnel_Records.Payroll_Number,Personnel_Records.Forenames,Personnel_Records.Surname,Personnel_Records.Date_of_Birth,Personnel_Records.Telephone,Personnel_Records.Mobile,Personnel_Records.Address,Personnel_Records.Address_2,Personnel_Records.Postcode,Personnel_Records.EMail_Home,Personnel_Records.Start_Date
COOP08,John,William,26/01/1955,12345678,987654231,12 Foreman road,London,GU12 6JW,nomadic20@hotmail.co.uk,18/04/2013";

            // Create an instance of the EmployeeImportService with mocks.
            var service = new EmployeeImportService(
                employeeFactoryMock.Object,
                repositoryMock.Object,
                loggerMock.Object);

            // Act: Import the CSV data.
            int result = await service.ImportEmployeeAsync(csvData);

            // Assert: Expect one successful record import.
            Assert.Equal(1, result);

            // Verify that CreateAsync was called exactly once.
            repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Employee>()), Times.Once);
        }
    }
}
