using System.Collections.Generic;
using Budget.Api.Models;

namespace Budget.Api.Services
{
    public interface IMonthGenerator
    {
        IEnumerable<Month> GenerateYear();
    }
}