using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TeamCelebrations.Data.Entities
{
    public class Friendship : BaseEntity
    {
        private Guid _employeeId1 = Guid.Empty;

        [Required]
        public Guid EmployeeId1 
        { 
            get { return _employeeId1; }
            set {
                if(_employeeId2 != Guid.Empty && _employeeId2 != value) 
                {
                    _employeeId1 = value;
                }
                else if(_employeeId2 == Guid.Empty)
                {
                    _employeeId1 = value;
                }
            } 
        }

        private Guid _employeeId2 = Guid.Empty;

        [Required]
        public Guid EmployeeId2 { 
            get { return _employeeId1; } 
            set {
                if(_employeeId1 != Guid.Empty && _employeeId1 != value) 
                {
                    _employeeId2 = value;
                }
                else if(_employeeId1 == Guid.Empty)
                {
                    _employeeId2 = value;
                }
            }
        }

        [ForeignKey(nameof(EmployeeId1))]
        public virtual Employee? Employee1 { get; set; }

        [ForeignKey(nameof(EmployeeId2))]
        public virtual Employee? Employee2 { get; set; }
    }
}