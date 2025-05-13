using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Descriptions { get; set; }

        public int Price { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
