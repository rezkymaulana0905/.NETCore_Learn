using System.ComponentModel.DataAnnotations;

namespace PTC.Models;

public class DdedmEmployee
{
    public required string EmpId { get; set; }

    public bool? Active { get; set; }

    public required string Name { get; set; }

    public string? Email { get; set; }

    public string? PrivateEmail { get; set; }

    public string? CurrentPhone { get; set; }

    public string? MobilePhone { get; set; }

    public string? GradeId { get; set; }

    public required string SectionStru { get; set; }

    public required string Department { get; set; }
}

public class EmployeeClocking
{
   [Key] public required string EmpId { get; set; }
    public DateOnly ClockingDate { get; set; }
    public DateTime ClockingTime { get;  set; }
    public string InOut { get;  set; }
    public string ShiftGroupId { get; set; }
    public string ShiftId { get; set; }
    public string TimeReasonId { get; set; }
    public bool Processed { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
}

public class TrxTmsEmpPcDtKpFlt
{
    public required int SeqNo { get; set; }

    public required bool Flag { get; set; }

    public required string EmpId { get; set; }

    public required string Name { get; set; }

    public string? Department { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly? TimeOut { get; set; }

    public TimeOnly? TimeReturn { get; set; }

    public string TimereasonId { get; set; } = null!;

    public string? Reason { get; set; }

    public string? CreateBy { get; set; }
}

public class TrxTmsEmpPcDtKpFltAct
{
    [Key]
    public int Id { get; set; }
    public int? SeqNo { get; set; }

    public bool Flag { get; set; }

    public required string EmpId { get; set; }

    public required string Name { get; set; }

    public string? Department { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly? TimeOut { get; set; }

    public TimeOnly? TimeReturn { get; set; }

    public string? TimereasonId { get; set; }

    public string? Reason { get; set; }
    public string? CreateBy { get; set; }
}
public class EmpScanRecord
{
    public int Id { get; set; }
    public string Username { get; set; }
    public int LoginId { get; set; }
    public int TransactionId { get; set; }
    public string Type { get; set; }
    public DateTime ScanTime { get; set; }
}


