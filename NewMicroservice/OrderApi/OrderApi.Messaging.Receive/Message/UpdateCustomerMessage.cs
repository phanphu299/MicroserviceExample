
namespace OrderApi.Messaging.Receive.Message
{
    using System;

    public class UpdateCustomerMessage
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
