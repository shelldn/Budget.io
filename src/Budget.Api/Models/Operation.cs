namespace Budget.Api.Models
{
    public class Operation
    {
        public string Id { get; set; }
        public int BudgetId { get; set; }
        public string CategoryId { get; set; }
        public int MonthId { get; set; }
        public decimal Plan { get; set; }
        public decimal Fact { get; set; }
    }
}