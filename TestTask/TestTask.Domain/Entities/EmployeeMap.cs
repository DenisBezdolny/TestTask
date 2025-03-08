using CsvHelper.Configuration;

namespace TestTask.Domain.Entities
{
    public sealed class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Map(m => m.PayrollNumber).Name("Personnel_Records.Payroll_Number");
            Map(m => m.Forename).Name("Personnel_Records.Forenames");
            Map(m => m.Surname).Name("Personnel_Records.Surname");
            Map(m => m.DateOfBirth).Name("Personnel_Records.Date_of_Birth")
                .TypeConverterOption.Format("dd/MM/yyyy");
            Map(m => m.PhoneNumber).Name("Personnel_Records.Telephone");
            Map(m => m.MobilePhoneNumber).Name("Personnel_Records.Mobile");
            Map(m => m.StreetAddress).Name("Personnel_Records.Address");
            Map(m => m.Settlement).Name("Personnel_Records.Address_2");
            Map(m => m.Postcode).Name("Personnel_Records.Postcode");
            Map(m => m.Email).Name("Personnel_Records.EMail_Home");
            Map(m => m.StartDate).Name("Personnel_Records.Start_Date")
                .TypeConverterOption.Format("dd/MM/yyyy");
        }
    }
}
