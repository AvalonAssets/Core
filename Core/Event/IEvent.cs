namespace AvalonAssets.Core.Event
{
    public interface IEvent
    {
        object Publisher { get; }
    }

    public interface IEvent<out T> : IEvent
    {
        T Data { get; }
    }
}