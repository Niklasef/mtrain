namespace Domain
{
    public interface ISubscriber
    {
        void Process(Message message);
    }
}