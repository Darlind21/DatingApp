using System.ComponentModel.DataAnnotations;

namespace API.Data_Layer.Models
{
    public class Connection
    {
        [Key]
        public required string ConnectionId { get; set; }
        public required string Username { get; set; }
    }
}
