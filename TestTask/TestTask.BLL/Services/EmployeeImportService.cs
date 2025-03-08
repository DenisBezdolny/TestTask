using CsvHelper;
using TestTask.Domain.Interfaces.Fabrics; // Consider renaming to IEmployeeFactory if appropriate
using TestTask.Domain.Interfaces.Repositories;
using TestTask.Domain.Interfaces.BLL;
using TestTask.Domain.Entities;

namespace TestTask.BLL.Services
{
    public class EmployeeImportService : IEmployeeImportService
    {
        private readonly IEmployeeFactory _employeeFactory;
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeeImportService(IEmployeeFactory employeeFacrory, IRepository<Employee> employeeRepository)
        {
            _employeeFactory = employeeFacrory;
            _employeeRepository = employeeRepository;
        }

        public async Task<int> ImportEmployeeAsync(CsvReader csvReader)
        {
            int successCount = 0;

            // Register the mapping if you're not using auto-mapping via attributes:
            csvReader.Context.RegisterClassMap<EmployeeMap>();

            while (csvReader.Read())
            {
                try
                {
                    var employee = _employeeFactory.CreateEmployee(
                        csvReader.GetField("Personnel_Records.Payroll_Number"),
                        csvReader.GetField("Personnel_Records.Forenames"),
                        csvReader.GetField("Personnel_Records.Surname"),
                        DateTime.ParseExact(csvReader.GetField("Personnel_Records.Date_of_Birth"), "dd/MM/yyyy", null),
                        csvReader.GetField("Personnel_Records.Telephone"),
                        csvReader.GetField("Personnel_Records.Mobile"),
                        csvReader.GetField("Personnel_Records.Address"),
                        csvReader.GetField("Personnel_Records.Address_2"),
                        csvReader.GetField("Personnel_Records.Postcode"),
                        csvReader.GetField("Personnel_Records.EMail_Home"),
                        DateTime.ParseExact(csvReader.GetField("Personnel_Records.Start_Date"), "dd/MM/yyyy", null)
                    );

                    await _employeeRepository.CreateAsync(employee);
                    successCount++;
                }
                catch (Exception ex)
                {
                    // TODO: Log the exception for debugging purposes.
                    // Example: _logger.LogError(ex, "Error importing employee record.");
                }
            }

            return successCount;
        }
    }
}
