using CsvHelper;

namespace TestTask.Domain.Interfaces.BLL
{
    public interface IEmployeeImportService
    {
        Task<int> ImportEmployeeAsync(CsvReader csvReader);
    }
}
