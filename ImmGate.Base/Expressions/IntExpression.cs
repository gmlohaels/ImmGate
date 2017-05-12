namespace ImmGate.Base.Expressions
{
    public class IntExpression<TAssociatedObject> : ExpressionValue<TAssociatedObject, int>
    {
        public IntExpression(TAssociatedObject associatedObject) : base(associatedObject)
        {

        }

        public void Substract(int value)
        {
            var i = EndValue() - value;
            SetRawValueTo(i);
        }

        public void Add(int value)
        {

            var i = EndValue() + value;
            SetRawValueTo(i);
        }

        public static implicit operator int(IntExpression<TAssociatedObject> x)
        {
            return x.EndValue();
        }
        public static int operator -(IntExpression<TAssociatedObject> c1, int c2)
        {
            var i = c1.EndValue() - c2;
            c1.SetRawValueTo(i);
            return i;
        }

        public static int operator *(IntExpression<TAssociatedObject> c1, int c2)
        {
            var i = c1.EndValue() * c2;
            c1.SetRawValueTo(i);
            return i;
        }



        public static int operator +(IntExpression<TAssociatedObject> c1, int c2)
        {
            var i = c1.EndValue() + c2;
            c1.SetRawValueTo(i);
            return i;
        }

        void Zu()
        {
            var t = new IntExpression<object>(null);
            var zz = t + 10;

        }
    }
}