namespace FCG.Application._Common.Handlers;
public interface IEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IEvent
{
}
