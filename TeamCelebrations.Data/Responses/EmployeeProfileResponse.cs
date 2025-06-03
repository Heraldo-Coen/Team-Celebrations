using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamCelebrations.Data.Entities;

namespace TeamCelebrations.Data.Responses
{
    public class EmployeeProfileResponse
    {
        public Guid Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public bool IsConnected { get; set; }

        public DateTime LastConnectionDate { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; } = DateTime.MinValue;
        
        public PhoneCodeResponse? PhoneCode { get; set; }
        
        public  UnitResponse? Unit { get; set; }

        public EmployeeProfileResponse()
        {
        }

        public EmployeeProfileResponse(Employee employee)
        {
            Id = employee.Id;
            FirstName = employee.FirstName;
            LastName = employee.LastName;
            Email = employee.Email;
            IsConnected = employee.IsConnected;
            LastConnectionDate = employee.LastConnectionDate;
            PhoneNumber = employee.PhoneNumber;
            BirthDate = employee.BirthDate;
            PhoneCode = new PhoneCodeResponse
            {
                Id = employee.PhoneCode!.Id,
                Code = employee.PhoneCode.Code,
                Length = employee.PhoneCode.Length,
                CountryName = employee.PhoneCode.CountryName,
                CountryCode = employee.PhoneCode.CountryCode
            };
            Unit = new UnitResponse(employee.Unit!);
        }
    }
}