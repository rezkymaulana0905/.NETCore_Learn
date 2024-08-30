using System.ComponentModel.DataAnnotations;
namespace PTC.Models;

//DB MODEL

public class GuestReg
{
    [Key] required public string Id { get; set; }
    public required string Name { get; set; }
    public  string? Email { get; set; }
    public  required string CountryCode { get; set; }
    public  required int CategoryId { get; set; }
    public required string Company { get; set; }
    public required string CompanyType { get; set; }
    public required string DeptSect { get; set; }
    public string? NationalId { get; set; }
    public int Total { get; set; }
    public required string Requirement { get; set; }
    public required string ReqId { get; set; }
    public required string MetId { get; set; }
    public byte[]? ImageData { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
public class GuestAttndnc
{

    public int Id { get; set; }
    public string TransactionId { get; set; }
    public DateOnly Date { get; set; }
    public int Total { get; set; }
    public TimeOnly TimeIn { get; set; }
    public TimeOnly? TimeOut { get; set; }
    public bool Flag { get; set; }
}
public class GuestCategory
{ 
  public int Id { get; set; }
  public string CategoryName { get; set; }
}

public class GuestScanRecord
{
    public int Id { get; set; }
    public string Username { get; set; }
    public int LoginId { get; set; }
    public int TransactionId { get; set; }
    public string Type {  get; set; }
    public DateTime ScanTime { get; set; }
}

public class GuestIdTemp
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}