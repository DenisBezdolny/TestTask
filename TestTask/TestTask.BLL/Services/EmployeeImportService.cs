using System.Globalization;
using CsvHelper;
using TestTask.BLL.Mappings;
using TestTask.Domain.Entities;
using TestTask.Domain.Interfaces.BLL;
using TestTask.Domain.Interfaces.Fabrics;
using TestTask.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using TestTask.BLL.DTOs;

namespace TestTask.BLL.Services
{
    public class EmployeeImportService : IEmployeeImportService
    {
        private readonly IEmployeeFactory _employeeFactory;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly ILogger<EmployeeImportService> _logger;

        public EmployeeImportService(
            IEmployeeFactory employeeFactory,
            IRepository<Employee> employeeRepository,
            ILogger<EmployeeImportService> logger)
        {
            _employeeFactory = employeeFactory;
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public async Task<int> ImportEmployeeAsync(string csvData)
        {
            int successCount = 0;

            // Create a CsvReader using our helper method.
            using var csv = CreateCsvReader(csvData);

            // Get records as EmployeeImportDto objects using the mapping configuration.
            var dtos = csv.GetRecords<EmployeeImportDto>();

            foreach (var dto in dtos)
            {
                try
                {
                    // Parse date fields from string to DateTime.
                    var dob = DateTime.ParseExact(dto.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var startDate = DateTime.ParseExact(dto.Start_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    // Use the factory to create an Employee domain entity from the DTO.
                    var employee = _employeeFactory.CreateEmployee(
                        dto.PayrollNumber,
                        dto.Forenames,
                        dto.Surname,
                        dob,
                        dto.Telephone,
                        dto.Mobile,
                        dto.Address,
                        dto.Address2,
                        dto.Postcode,
                        dto.EMail_Home,
                        startDate
                    );

                    await _employeeRepository.CreateAsync(employee);
                    successCount++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error importing employee record.");
                }
            }

            return successCount;
        }

        private CsvReader CreateCsvReader(string csvData)
        {
            var reader = new StringReader(csvData);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<EmployeeImportDtoMap>();
            return csv;
        }
    }
}
