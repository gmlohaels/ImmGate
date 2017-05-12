namespace ImmGate.Base.Patterns.Memento
{

    public interface IComponentSnapshot : IMementoSnapshot
    {
        void ResetToDefault();

    }


}