namespace API.DTOs
{
    public class PaginatedResult<T>
    {
        public List<T> Users { get; set; }
        public int TotalCount { get; set; }
        public decimal TotalPages { get; set; }
    }
}