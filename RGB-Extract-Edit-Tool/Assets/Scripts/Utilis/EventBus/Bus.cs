
public static class Bus<T>
{
    public delegate void Event(T args);
    public static event Event OnEvent;
    public static void Raise(T args) => OnEvent?.Invoke(args);
}