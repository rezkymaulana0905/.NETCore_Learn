namespace PTC.Models
{
    public class PsgSpplier
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int VhcId { get; set; }
        public bool Flag { get; set; }
    }

    public class VhcSpplier
    {
        public int Id { get; set; }
        public string? VehicleId { get; set; }
        public string? VehicleType { get; set; }
        public string? Company { get; set; }
        public int PassangerCount { get; set; }
        public DateTime InTime { get; set; } = DateTime.Now;
        public bool Flag { get; set; }
        public DateTime? ConfirmOutTime { get; set; } = null;
        public required byte[] VehicleImg { get; set; }
    }

    public class  SpplierTransactionRecord
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int LoginId { get; set; }
        public int TransactionId {  get; set; }
        public string Type { get; set; }
        public DateTime ScanTime { get; set; }
    }

    public class ConfirmVhc
    {
        public required int VhcId { get; set; }
        public required int[] PsgId { get; set; }
    }

    public class Spplier
    {
        public required VhcSpplier VhcSpplier { get; set; }
        public required PsgSpplier[] PsgSppliers { get; set; }
    }
}