namespace App.Models
{
    public class ElectricityBill : BaseEntity
    {
        public decimal Amount { get; set; } 
        public PaymentStatus Status { get; set; }
        public string VerificationRef { get; set; }
    }
}
