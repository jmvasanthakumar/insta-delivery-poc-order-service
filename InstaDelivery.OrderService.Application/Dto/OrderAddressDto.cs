namespace InstaDelivery.OrderService.Application.Dto
{
    public class OrderAddressDto
    {
        public required string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Landmark { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string PostalCode { get; set; }
    }
}
