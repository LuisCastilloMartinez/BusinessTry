using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Business_Try
{
    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=LUIS\\SQLSERVER;Initial Catalog=EmployeesDB;Integrated Security=True;";
        private string DepartmentUser = "Accounting and Finance";
        private EmployeeRepository employeeRepository;
        private List<Employee> employees;

        public Form1()
        {
            InitializeComponent();
            employeeRepository = new EmployeeRepository(connectionString);
        }
        private bool CheckDatabaseConnection()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                MessageBox.Show("Couldn't connect to the database. Please make sure SQL Server is running.");
                return false;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
            LoadData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count > 0)
            {
                int employeeID = Convert.ToInt32(dgvEmployees.SelectedRows[0].Cells["ID"].Value);
                employeeRepository.DeleteEmployee(employeeID);
                LoadData();
            }
            else
            {
                MessageBox.Show("Select an Employee to Delete");
            }
        }

        private void LoadData()
        {
            try
            {
                employees = employeeRepository.GetAllEmployees();
                DisplayEmployees();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Failed to connect to the database: " + ex.Message);
            }
        }

        private void DisplayEmployees()
        {
            dgvEmployees.DataSource = employees;

            // Configura visibilidad de salario
            dgvEmployees.Columns["Salary"].Visible = (DepartmentUser == "Accounting and Finance");
        }

        private void SaveData()
        {
            if (!ValidateInput())
                return;

            var employee = new Employee
            {
                Name = txtName.Text,
                Department = txtDepartment.Text,
                Job = txtJob.Text,
                Salary = decimal.Parse(txtSalary.Text),
                InHour = TimeSpan.Parse(txtIn.Text),
                OutHour = TimeSpan.Parse(txtOut.Text)
            };

            employeeRepository.SaveEmployee(employee);
            MessageBox.Show("Data Saved Correctly.");
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtDepartment.Text) ||
                string.IsNullOrEmpty(txtJob.Text) || string.IsNullOrEmpty(txtSalary.Text) ||
                string.IsNullOrEmpty(txtIn.Text) || string.IsNullOrEmpty(txtOut.Text))
            {
                MessageBox.Show("Please, fill in all the blanks");
                return false;
            }
            if (!decimal.TryParse(txtSalary.Text, out _))
            {
                MessageBox.Show("Entered Salary must be a valid amount.");
                return false;
            }

            if (!TimeSpan.TryParse(txtIn.Text, out _) || !TimeSpan.TryParse(txtOut.Text, out _))
            {
                MessageBox.Show("The In or Out time format is not valid.");
                return false;
            }
            return true;
        }
        private void close_Click(object sender, EventArgs e) { Application.Exit(); }
    }
}
