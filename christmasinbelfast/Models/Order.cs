namespace christmasinbelfast.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string DeliveryAddress { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
