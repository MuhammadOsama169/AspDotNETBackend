
namespace DotNetBackend.Extensions;

public class PaginatedList<T>
{
    /// <summary>Total number of items across all pages.</summary>
    public int Count { get; set; }

    /// <summary>Current page index (1‐based).</summary>
    public int PageIndex { get; set; }

    /// <summary>The items for this page.</summary>
    public List<T> Data { get; set; } = new();
}