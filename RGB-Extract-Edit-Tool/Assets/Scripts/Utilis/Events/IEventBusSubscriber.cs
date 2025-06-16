public interface IEventBusSubscriber<T>
{
    void OnEventReceived(T args);
}