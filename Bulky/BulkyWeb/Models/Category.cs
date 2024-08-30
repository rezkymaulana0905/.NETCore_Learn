using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyWeb.Models
{
    public class Category
    {
        [Key]//set to primary key using data anotation, reference to first code under this
        public int Id { get; set; } //set to primary key 
        [Required] //set to required using data anotation, reference to code under this
        [MaxLength(30)]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1,100,ErrorMessage = "Display must between 1 - 100")] //adding range and error message validation
        public int DisplayOrder { get; set; }
        [NotMapped]
        [DisplayName("Upload File")]
        public string? ImagePath {get; set; }
        //public httpPostedFileBase ImageFile {  get; set; }  

    }   //model for category    
}
