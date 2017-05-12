namespace ImmGate.Base.Patterns.Memento
{
    /// <summary>
    /// Represent Memento Pattern
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMementoSetter<in T> where T : IMementoSnapshot
    {
        void SetMemento(T memento);
    }
}