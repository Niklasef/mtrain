namespace Server
{
    public interface IQuery<TResult>
    {
        TResult Execute();
    }
}
