namespace CustomerApi.Message.Send.Sender
{
    using CustomerApi.Message.Send.Message;

    public interface ICustomerUpdateSender
    {
        void SendUpdateCustomer(UpdateCustomerMessage customer);
    }
}
