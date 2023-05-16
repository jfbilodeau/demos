namespace HrSystem;

public static class Employees
{
    public static IEnumerable<Employee> List()
    {
        return new[]
        {
            new Employee()
            {
                Id = "123",
                FirstName = "Charles",
                LastName = "Babbage",
                Salary = 65000.00,
                Department = "HR"
            },
            new Employee()
            {
                Id = "456",
                FirstName = "Ada",
                LastName = "Lovelace",
                Salary = 73000.00,
                Department = "IT"
            },
            new Employee()
            {
                Id = "789",
                FirstName = "Kathleen",
                LastName = "Booth",
                Salary = 75000.00,
                Department = "IT"
            }
        };
    }
}