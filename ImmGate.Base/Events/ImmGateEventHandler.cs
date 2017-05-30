namespace ImmGate.Base.Events
{

    public delegate void ImmGateEventHandler<in TSender, in TArgument>(TSender sender, TArgument e);
    public delegate void ImmGateEventHandler<in T>(object sender, T e);
    public delegate void ImmGateEventHandler(object sender);

}