namespace ImmGate.Base.Expressions
{
    public class NonNegativeIntExpression<TAssociatedObject> : IntExpression<TAssociatedObject>
    {
        public override int EndValue()
        {
            var result = base.EndValue();
            return result < 0 ? 0 : result;
        }

        public NonNegativeIntExpression(TAssociatedObject associatedObject) : base(associatedObject)
        {
        }
    }
}