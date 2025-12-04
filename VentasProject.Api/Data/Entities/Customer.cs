using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VentasProject.Api.Data.Entities
{
    [Table("customers")]
    public class Customer
    {
        [Key]
        [Column("customer_id")]
        public int CustomerId { get; set; }
        [Column("first_name")]
        public string FirstName { get; set; }
        [Column("last_name")]
        public string LastName { get; set; }
        [Column("email")]
        public string? Email { get; set; }
        [Column("phone")]
        public string? Phone { get; set; }
        [Column("city")]
        public string? City { get; set; }
        [Column("country")]
        public string? Country { get; set; }
    }
}
