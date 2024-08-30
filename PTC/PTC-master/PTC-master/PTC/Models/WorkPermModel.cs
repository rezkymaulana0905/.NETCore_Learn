using System.ComponentModel.DataAnnotations;

namespace PTC.Models;

public class WorkPermitModel
{
    public required WrkPermDesc Desc { get; set; }
    public required WrkPermWorker Worker { get; set; }
}
public class WrkPermAttndnc
{
    public int Id { get; set; }
    public required string RegNum { get; set; }
    public required int WorkerId { get; set; }
    public required DateOnly Date { get; set; }
    public required TimeOnly InTime { get; set; }
    public required TimeOnly? OutTime { get; set; }
    public required bool Flag { get; set; }
}
public class WrkPermDesc
{
    [Key]
    public required string RegNum { get; set; }
    public required string Title { get; set; }
    public required string CompName { get; set; }
    public required string Location { get; set; }
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }
}
public class ShowWrkPermAttndnc
{
    public DateOnly Date { get; set; }
    public string Name { get; set; }
    public string Company { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public TimeOnly InTime { get; set; }
    public TimeOnly? OutTime { get; set; }
}
public class WrkPermWorker
{
    public int Id { get; set; }
    public required string RegNum { get; set; }
    public required string Name { get; set; }
    public  string? Speciality { get; set; }
    public  string? Certificate { get; set; } 
    public string? NationalId { get; set; }
}
public class WorkPermit
{
    public required WrkPermDesc Description { get; set; }
    public required WrkPermWorker[] Worker { get; set; }
}
public class CompanyAttendance
{
    public required string Company { get; set; }
    public int Total { get; set; }
}

public class WrkPermScanRecord
{
    public int Id { get; set; }
    public string Username { get; set; }
    public int LoginId { get; set; }
    public int TransactionId { get; set; }
    public string Type { get; set; }
    public DateTime ScanTime { get; set; }
}