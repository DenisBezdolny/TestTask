using System.Globalization;
using CsvHelper;
using TestTask.BLL.Mappings;
using TestTask.BLL.DTOs;
using TestTask.Domain.Entities;
using TestTask.Domain.Interfaces.BLL;
using TestTask.Domain.Interfaces.Fabrics;
using TestTask.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

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

            // Define acceptable date formats.
            string[] dateFormats = new[] { "d/M/yyyy", "dd/MM/yyyy" };

            foreach (var dto in dtos)
            {
                try
                {
                    // Attempt to parse Date of Birth
                    if (!DateTime.TryParseExact(dto.DateOfBirth, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob))
                    {
                        _logger.LogError("Invalid date format for DateOfBirth: {DateOfBirth}", dto.DateOfBirth);
                        continue; // Skip this record and move to the next one.
                    }

                    // Attempt to parse Start Date
                    if (!DateTime.TryParseExact(dto.Start_Date, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate))
                    {
                        _logger.LogError("Invalid date format for StartDate: {StartDate}", dto.Start_Date);
                        continue; // Skip this record.
                    }

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
                catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
                {
                    // If the inner exception is a PostgresException, it might have details about the column.
                    if (dbEx.InnerException is Npgsql.PostgresException pgEx)
                    {
                        _logger.LogError(dbEx, "Error saving employee record. The column '{ColumnName}' is null. Detail: {Detail}",
                            pgEx.ColumnName, pgEx.Detail);
                    }
                    else
                    {
                        _logger.LogError(dbEx, "Error saving employee record.");
                    }
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
