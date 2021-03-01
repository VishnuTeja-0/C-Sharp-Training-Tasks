
namespace BankManagement.Models
{
    public class Transaction
    {
        public TransactionTypes Type { get; set; }
        public string Id { get; set; }
        public string SenderBankId { get; set; }
        public string SenderId { get; set; }
        public string ReceiverBankId { get; set; }
        public string ReceiverId { get; set; }
        public double Amount { get; set; }
        public string Time { get; set; }
    }
}
