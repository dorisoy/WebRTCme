using System;
using System.Threading.Tasks;

namespace WebRtcMeBindingsBlazor.Interops
{
    internal class DisposableAction : IDisposable
    {
        private readonly Action _actionOnDispose;

        public DisposableAction(Action actionOnDispose)
        {
            _actionOnDispose = actionOnDispose;
        }

        public void Dispose()
        {
            _actionOnDispose();
        }
    }
}