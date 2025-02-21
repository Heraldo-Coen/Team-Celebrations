using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamCelebrations.Data.Requests
{
    public class EmployeeSignUpRequest : SignUpRequest
    {
        public string? PhoneNumber { get; set; }

        public Guid PhoneCodeId { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime HireDate { get; set; }

        public Guid UnitId { get; set; }
    }
}