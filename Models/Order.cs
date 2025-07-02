public class Order
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }

    public ICollection<Book> Books { get; set; } = new List<Book>();
}