namespace CustomerApi.Message.Send.Message
{
    using System;
    public class UpdateCustomerMessage
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
