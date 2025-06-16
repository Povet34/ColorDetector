
public static class Bus<T>
{
    public delegate void Event(T args);
    public static event Event OnEvent;
    public static void Raise(T args) => OnEvent?.Invoke(args);
}

public static class EventBusSubscriberExtensions
{
    public static void SubscribeEvent<T>(this IEventBusSubscriber<T> subscriber)
    {
        Bus<T>.OnEvent += subscriber.OnEventReceived;
    }

    public static void UnsubscribeEvent<T>(this IEventBusSubscriber<T> subscriber)
    {
        Bus<T>.OnEvent -= subscriber.OnEventReceived;
    }
}