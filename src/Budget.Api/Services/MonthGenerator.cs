using System.Collections.Generic;
using Budget.Api.Models;

namespace Budget.Api.Services
{
    public class MonthGenerator : IMonthGenerator
    {
        public IEnumerable<Month> GenerateYear()
        {
            for (var i = 0; i < 12; i++)
            {
                yield return new Month
                {
                    Id = i
                };
            }
        }
    }
}