using System.ComponentModel.DataAnnotations;

namespace API.Data_Layer.Models
{
    public class Group
    {
        [Key]
        public required string Name { get; set; }
        public ICollection<Connection> Connections { get; set; } = [];
    }
}
