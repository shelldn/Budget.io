using System.Collections.Generic;

namespace Budget.Api.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Operation> Operations { get; set; }
        public IEnumerable<Month> Months { get; set; }
    }
}