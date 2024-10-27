using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Try
{
    internal class EmployeeRepository
    {
        private string connectionString;

        public EmployeeRepository(string connectionString) { this.connectionString = connectionString; }

        public void SaveEmployee(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Employees (Name, Department, Job, Salary, inHour, outHour)" +
                               "VALUES (@Name, @Department, @Job, @Salary, @InHour, @OutHour)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", employee.Name);
                    command.Parameters.AddWithValue("@Department", employee.Department);
                    command.Parameters.AddWithValue("@Job", employee.Job);
                    command.Parameters.AddWithValue("@Salary", Encrypt(employee.Salary.ToString())); // Encrypt salary
                    command.Parameters.AddWithValue("@InHour", employee.InHour);
                    command.Parameters.AddWithValue("@OutHour", employee.OutHour);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Employees";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var employee = new Employee
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Department = reader.GetString(2),
                                Job = reader.GetString(3),
                                Salary = decimal.Parse(Decrypt(reader.GetString(4))), // Decrypt salary
                                InHour = reader.GetTimeSpan(5),
                                OutHour = reader.GetTimeSpan(6)
                            };
                            employees.Add(employee);
                        }
                    }
                }
            }

            return employees;
        }

        public void DeleteEmployee(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Employees WHERE ID = @ID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private string Encrypt(string input) { return input; }
        private string Decrypt(string input) { return input; }
    }
}
