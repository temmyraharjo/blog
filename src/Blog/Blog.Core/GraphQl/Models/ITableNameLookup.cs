namespace Blog.Core.GraphQl.Models
{
    public interface ITableNameLookup
    {
        bool InsertKeyName(string friendlyName);
        string GetFriendlyName(string correctName);
    }
}
