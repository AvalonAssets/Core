namespace AvalonAssets.Core.Event
{
    /// <summary>
    ///     Marker interface.
    /// </summary>
    /// <remarks>
    ///     Do not use this. Use <see cref="IEvent{T}" /> instead.
    /// </remarks>
    public interface IEvent
    {
        /// <summary>
        ///     Publisher of the event.
        /// </summary>
        object Publisher { get; }
    }

    /// <summary>
    ///     <see cref="IEvent{T}" /> is the basic structure of a event.
    ///     Any additional data is passed by <see cref="Data" />.
    /// </summary>
    /// <typeparam name="T">Data object.</typeparam>
    public interface IEvent<out T> : IEvent
    {
        T Data { get; }
    }
}