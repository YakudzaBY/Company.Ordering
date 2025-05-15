namespace Company.Ordering.API.Models
{
    public class OrderProduct(int productId, int productAmount, string? productName = default, decimal productPrice = default)
    {
        public int ProductId => productId;

        public string? ProductName => productName;

        public int ProductAmount => productAmount;

        public decimal ProductPrice => productPrice;
    }
}
