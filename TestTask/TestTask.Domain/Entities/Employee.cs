namespace TestTask.Domain.Entities
{
    public class Employee
    {
        public Guid Id { get; private set; }

        public string PayrollNumber { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string StreetAddress { get; set; }
        public string Settlement { get; set; }
        public string Postcode { get; set; }
        public string Email { get; set; }
        public DateTime StartDate { get; set; }
    }
}
