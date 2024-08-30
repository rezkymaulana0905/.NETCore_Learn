using System.ComponentModel.DataAnnotations;

namespace PTC.Models;

public class CountryModel
{
    [Key] public string Code { get; set; }
    [Required] public string Name { get; set; }
}
