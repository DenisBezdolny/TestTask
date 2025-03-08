using CsvHelper.Configuration;
using TestTask.BLL.DTOs;

namespace TestTask.BLL.Mappings
{
    public sealed class EmployeeImportDtoMap : ClassMap<EmployeeImportDto>
    {
        public EmployeeImportDtoMap()
        {
            Map(m => m.PayrollNumber).Name("Personnel_Records.Payroll_Number");
            Map(m => m.Forenames).Name("Personnel_Records.Forenames");
            Map(m => m.Surname).Name("Personnel_Records.Surname");
            Map(m => m.DateOfBirth).Name("Personnel_Records.Date_of_Birth")
                .TypeConverterOption.Format("dd/MM/yyyy");
            Map(m => m.Telephone).Name("Personnel_Records.Telephone");
            Map(m => m.Mobile).Name("Personnel_Records.Mobile");
            Map(m => m.Address).Name("Personnel_Records.Address");
            Map(m => m.Address2).Name("Personnel_Records.Address_2");
            Map(m => m.Postcode).Name("Personnel_Records.Postcode");
            Map(m => m.EMail_Home).Name("Personnel_Records.EMail_Home");
            Map(m => m.Start_Date).Name("Personnel_Records.Start_Date")
                .TypeConverterOption.Format("dd/MM/yyyy");
        }
    }
}
