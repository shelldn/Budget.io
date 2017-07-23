namespace Budget.Data.Operations
{
    public interface IPlanPatcher
    {
        void Patch(string id, string accountId, int @value);        
    }
}
