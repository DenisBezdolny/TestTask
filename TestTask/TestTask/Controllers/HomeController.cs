using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestTask.Domain.Entities;
using TestTask.Domain.Interfaces.BLL;
using TestTask.Domain.Interfaces.Repositories;
using TestTask.Models;

namespace TestTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeImportService _employeeImportService;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IEmployeeImportService employeeImportService,
            IRepository<Employee> employeeRepository,
            ILogger<HomeController> logger)
        {
            _employeeImportService = employeeImportService;
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        // GET: Home/Index
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeRepository.GetAllAsync();
            var sortedEmployees = employees.OrderBy(e => e.Surname).ToList();
            // Create the view model
            var viewModel = new EmployeeImportViewModel
            {
                Employees = sortedEmployees,
                Message = TempData["Message"] as string
            };

            return View(viewModel);
        }

        // POST: Home/ImportFile
        [HttpPost]
        public async Task<IActionResult> ImportFile()
        {
            var file = Request.Form.Files["csvFile"];
            if (file == null || file.Length == 0)
            {
                TempData["Message"] = "Please select a CSV file.";
                return RedirectToAction("Index");
            }

            // Open and read file
            using var stream = file.OpenReadStream();
            using var reader = new System.IO.StreamReader(stream);
            string csvData = await reader.ReadToEndAsync();

            int count = await _employeeImportService.ImportEmployeeAsync(csvData);
            TempData["Message"] = $"{count} employee record(s) imported successfully.";

            return RedirectToAction("Index");
        }

        // POST: Home/EditEmployee
        [HttpPost]
        public async Task<IActionResult> EditEmployee(Employee employee)
        {
            try
            {
                await _employeeRepository.UpdateAsync(employee);
                return Json(new { success = true });
            }
            catch (DbUpdateException dbEx)
            {
                string errorDetail = "Error saving changes.";
                if (dbEx.InnerException is SqlException sqlEx)
                {
                    // Take the first error in the collection.
                    var firstError = sqlEx.Errors[0];
                    // Use a simple approach to extract the column name from the error message.
                    string message = firstError.Message;
                    string columnName = "";
                    int startIndex = message.IndexOf("column '");
                    if (startIndex != -1)
                    {
                        startIndex += "column '".Length;
                        int endIndex = message.IndexOf("'", startIndex);
                        if (endIndex != -1)
                        {
                            columnName = message.Substring(startIndex, endIndex - startIndex);
                        }
                    }

                    errorDetail = $"Error saving changes.The column '{columnName}' is missing or invalid.";
                    _logger.LogError(dbEx, "Detailed SQL error: Error Number: {ErrorNumber}, Extracted Column: {ColumnName}, Message: {ErrorMessage}",
                        firstError.Number, columnName, firstError.Message);
                }
                else
                {
                    _logger.LogError(dbEx, "Error saving employee record.");
                }
                Response.StatusCode = 500;
                return Json(new { success = false, message = errorDetail });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while saving changes.");
                Response.StatusCode = 500;
                return Json(new { success = false, message = "An unexpected error occurred while saving changes." });
            }
        }
    }
}
