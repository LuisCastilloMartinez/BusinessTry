using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Try
{
    internal class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Job { get; set; }
        public decimal Salary { get; set; }
        public TimeSpan InHour { get; set; }
        public TimeSpan OutHour { get; set; }
    }
}
