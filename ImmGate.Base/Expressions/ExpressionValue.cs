using System;
using System.Collections.Generic;
using System.Linq;

namespace ImmGate.Base.Expressions
{
    public class ExpressionValue<TAssociatedObject, TValue>
    {
        private readonly TAssociatedObject associatedObject;
        public bool HasExpressions()
        {
            return exprList.Count > 0;
        }

        private TValue value;

        public ExpressionValue()
        {
            associatedObject = default(TAssociatedObject);
            value = default(TValue);

        }

        public ExpressionValue(TAssociatedObject associatedObject)
        {
            this.associatedObject = associatedObject;

            value = default(TValue);
        }


        private readonly List<Func<TAssociatedObject, TValue, TValue>> exprList = new List<Func<TAssociatedObject, TValue, TValue>>();



        private readonly Func<TAssociatedObject, TValue, TValue> defaultLastExpression = (o, value1) => value1;
        private Func<TAssociatedObject, TValue, TValue> lastExpression = (o, value1) => value1;

        public void SetLastExpression(Func<TAssociatedObject, TValue, TValue> expression)
        {
            lastExpression = expression;

        }

        public void ResetLastExpressionToDefault()
        {
            lastExpression = defaultLastExpression;

        }


        public void AddExpr(Func<TAssociatedObject, TValue, TValue> expr)
        {
            if (!exprList.Contains(expr))
                exprList.Add(expr);
        }

        public void RemoveExpr(Func<TAssociatedObject, TValue, TValue> expr)
        {
            exprList.Remove(expr);
        }

        TValue Calculate(TAssociatedObject obj)
        {

            if (!HasExpressions())
                return lastExpression(obj, value);

            var result = exprList.Aggregate(value, (current, expr) => expr(obj, current));
            return lastExpression(obj, result);

        }


        public TValue EndValueFrom(TAssociatedObject assocObject)
        {
            return Calculate(assocObject);
        }

        public virtual TValue EndValue()
        {
            return Calculate(associatedObject);
        }

 
        public void SetRawValueTo(TValue i)
        {
            value = i;
        }


        public void ResetToDefault()
        {
            Clear();
            SetRawValueTo(default(TValue));

        }


        public void Clear()
        {
            exprList.Clear();
        }
    }


}