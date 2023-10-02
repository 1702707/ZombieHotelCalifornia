namespace Controller.Components.Events
{
    public interface IListener<in T>
    {
        void OnEnable();
        void OnDisable();
        void OnEventTriggered(T contactPoint);
    }
    
    public interface IListener
    {
        void OnEnable();
        void OnDisable();
        void OnEventTriggered();
    }
}