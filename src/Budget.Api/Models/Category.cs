namespace Budget.Api.Models
{
    public class Category
    {
        public string Id { get; set; }
        public int BudgetId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
}