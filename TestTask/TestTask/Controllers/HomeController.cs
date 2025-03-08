using Microsoft.AspNetCore.Mvc;
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

        public HomeController(IEmployeeImportService employeeImportService, IRepository<Employee> employeeRepository)
        {
            _employeeImportService = employeeImportService;
            _employeeRepository = employeeRepository;
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
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
            {
                string errorDetail = "Error saving changes.";
                if (dbEx.InnerException is Npgsql.PostgresException pgEx)
                {
                    // Extract the column name and details from the Postgres exception
                    errorDetail = $"Error: The field '{pgEx.ColumnName}' is missing or invalid."; 
                }
                // Return a JSON error response 
                Response.StatusCode = 500;
                return Json(new { success = false, message = errorDetail });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { success = false, message = "An unexpected error occurred while saving changes." });
            }
        }
    }
}
