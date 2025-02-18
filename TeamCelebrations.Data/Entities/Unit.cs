using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TeamCelebrations.Data.Entities
{
    public class Unit : BaseEntity
    {
        public string? Name { get; set; }
    }
}