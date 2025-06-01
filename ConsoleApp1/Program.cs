namespace ConsoleApp1
{
    internal class Program
    {
        public enum SalaryType
        {
            Monthly,
            Performance,
            Bonus
        }

        public class Employee
        {
            public int EmployeeID { get; set; }
            public string? EmployeeFirstName { get; set; }
            public string? EmployeeLastName { get; set; }
            public int Age { get; set; }
        }

        public class Salary
        {
            public int EmployeeID { get; set; }
            public int Amount { get; set; }
            public SalaryType Type { get; set; }
        }
        IList<Employee> employeeList;
        IList<Salary> salaryList;

        public Program()
        {
            employeeList = new List<Employee>() {
                new Employee(){ EmployeeID = 1, EmployeeFirstName = "Rajiv", EmployeeLastName = "Desai", Age = 49},
                new Employee(){ EmployeeID = 2, EmployeeFirstName = "Karan", EmployeeLastName = "Patel", Age = 32},
                new Employee(){ EmployeeID = 3, EmployeeFirstName = "Sujit", EmployeeLastName = "Dixit", Age = 28},
                new Employee(){ EmployeeID = 4, EmployeeFirstName = "Mahendra", EmployeeLastName = "Suri", Age = 26},
                new Employee(){ EmployeeID = 5, EmployeeFirstName = "Divya", EmployeeLastName = "Das", Age = 20},
                new Employee(){ EmployeeID = 6, EmployeeFirstName = "Ridhi", EmployeeLastName = "Shah", Age = 60},
                new Employee(){ EmployeeID = 7, EmployeeFirstName = "Dimple", EmployeeLastName = "Bhatt", Age = 53}
            };

            salaryList = new List<Salary>() {
                new Salary(){ EmployeeID = 1, Amount = 1000, Type = SalaryType.Monthly},
                new Salary(){ EmployeeID = 1, Amount = 500, Type = SalaryType.Performance},
                new Salary(){ EmployeeID = 1, Amount = 100, Type = SalaryType.Bonus},
                new Salary(){ EmployeeID = 2, Amount = 3000, Type = SalaryType.Monthly},
                new Salary(){ EmployeeID = 2, Amount = 1000, Type = SalaryType.Bonus},
                new Salary(){ EmployeeID = 3, Amount = 1500, Type = SalaryType.Monthly},
                new Salary(){ EmployeeID = 4, Amount = 2100, Type = SalaryType.Monthly},
                new Salary(){ EmployeeID = 5, Amount = 2800, Type = SalaryType.Monthly},
                new Salary(){ EmployeeID = 5, Amount = 600, Type = SalaryType.Performance},
                new Salary(){ EmployeeID = 5, Amount = 500, Type = SalaryType.Bonus},
                new Salary(){ EmployeeID = 6, Amount = 3000, Type = SalaryType.Monthly},
                new Salary(){ EmployeeID = 6, Amount = 400, Type = SalaryType.Performance},
                new Salary(){ EmployeeID = 7, Amount = 4700, Type = SalaryType.Monthly}
            };
        }
            static void Main(string[] args)
        {

            Program program = new Program();

            program.Task1();

            program.Task2();

            program.Task3();
        }

        public void Task1()
        {
            // Step 1: Join employees and their corresponding salaries
            var joinedEmpSal = employeeList.Join(
                salaryList,
                emp => emp.EmployeeID,                // Key from employee list
                sal => sal.EmployeeID,                // Key from salary list
                (emp, sal) => new { emp, sal }        // Combine into a new anonymous object
            );

            // Step 2: Group the joined data by Employee ID and full name
            var grouped = joinedEmpSal.GroupBy(x => new
            {
                x.emp.EmployeeID,
                x.emp.EmployeeFirstName,
                x.emp.EmployeeLastName
            });

            // Step 3: Project grouped data into new anonymous object with name and total salary
            var project = grouped
                .Select(x => new
                {
                    Name = x.Key.EmployeeFirstName + " " + x.Key.EmployeeLastName, // Full name
                    TotalSalary = x.Sum(x => x.sal.Amount)                         // Sum of all salary types
                })
                .OrderBy(x => x.TotalSalary); // Step 4: Order result by total salary (ascending)

            // Step 5: Print results
            Console.WriteLine();
            Console.WriteLine($"Salary of all the employees with their corresponding names");
            foreach (var item in project)
            {
                Console.WriteLine($"Employee: {item.Name}, TotalSalary: {item.TotalSalary}");
            }
        }


        public void Task2()
        {
            // Step 1: Find the age of the second oldest employee
            var secondOldestAge = employeeList
                .Select(x => x.Age)                // Select only the ages
                .OrderByDescending(x => x)         // Sort in descending order (oldest first)
                .Skip(1)                           // Skip the first (oldest)
                .FirstOrDefault();                 // Take the next one (second oldest)

            // Step 2: Filter employees who have the second oldest age
            var emp = employeeList.Where(x => x.Age == secondOldestAge);

            // Step 3: Join the filtered employee(s) with their salary records
            var joinedEmpSal = emp.Join(
                salaryList,
                emp => emp.EmployeeID,             // Employee key
                sal => sal.EmployeeID,             // Salary key
                (emp, sal) => new { emp, sal }     // Result: new anonymous object with employee and salary
            );

            // Step 4: Filter only monthly salary records and project required fields
            var monthlySalaryDetails = joinedEmpSal
                .Where(x => x.sal.Type == SalaryType.Monthly) // Filter only monthly salary entries
                .Select(x => new
                {
                    Name = x.emp.EmployeeFirstName + " " + x.emp.EmployeeLastName,
                    MonthlySalary = x.sal.Amount,
                    x.emp.Age
                });

            // Step 5: Print employee details
            Console.WriteLine(); // Print an empty line for spacing
            Console.WriteLine($"Employee details of 2nd oldest");
            foreach (var item in monthlySalaryDetails)
            {
                Console.WriteLine($"Name: {item.Name}, MonthlySalary: {item.MonthlySalary}, Age: {item.Age}");
            }
        }


        public void Task3()
        {
            // Step 1: Filter employees whose age is greater than 30
            var emp = employeeList.Where(x => x.Age > 30);

            // Step 2: Join filtered employees with their salary records on EmployeeID
            var joinedEmpSal = emp.Join(
                salaryList,
                emp => emp.EmployeeID,      // Key from employee
                sal => sal.EmployeeID,      // Key from salary
                (emp, sal) => new { emp, sal }  // Result selector: creates a new object containing both
            );

            // Step 3: Group the joined data by the salary type (e.g., Monthly, Performance, Bonus)
            var group = joinedEmpSal.GroupBy(x => new { x.sal.Type });

            // Step 4: Project each group into an object with the salary type and its average amount
            var project = group.Select(x => new
            {
                x.Key.Type,                         // The salary type (from grouping key)
                AverageAmount = x.Average(x => x.sal.Amount)  // Average amount for that type
            });

            // Step 5: Print the results to the console
            Console.WriteLine(); // Print an empty line for spacing
            Console.WriteLine($"Means of Monthly, Performance, Bonus salary of employees age greater than 30");
            foreach (var item in project)
            {
                Console.WriteLine($"Type: {item.Type}, Average Amount: {item.AverageAmount}");
            }
        }
    }
}
