using System;

namespace ImmGate.Base.References
{
    /// <summary>
    /// Fires Event When Acquire/Release counter is zero
    /// </summary>
    public sealed class ReferenceCounter
    {
        private uint counter;
        public event EventHandler OnNoReferencesLeft;


        public void Acquire()
        {
            lock (this)
            {
                counter++;
            }
        }

        public void Release()
        {
            var fireEventRequired = false;

            lock (this)
            {
                if (counter > 0)
                {
                    counter--;
                    fireEventRequired = (counter == 0);
                }
            }
            if (fireEventRequired)
                DoNoReferencesLeft();
        }

        private void DoNoReferencesLeft()
        {
            OnNoReferencesLeft?.Invoke(this, EventArgs.Empty);
        }

        public override string ToString()
        {
            return $"{nameof(counter)}: {counter}";
        }
    }
}