using System;
using System.ComponentModel;

namespace ArwynFr.AspectNetSharp
{
    public abstract class ObservableBase :
            ContextBoundObject,
            INotifyPropertyChanging,
            INotifyPropertyChanged,
            IDisposable
    {
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;
        public event OperationExecutingEventHandler OperationExecuting;
        public event OperationExecutedEventHandler OperationExecuted;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) { }

        private void RaisePropertyChanging(string propertyName)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RaiseOperationExecuting(string operationName)
        {
            OperationExecuting?.Invoke(this, new OperationExecutingEventArgs(operationName));
        }

        private void RaiseOperationExecuted(string operationName)
        {
            OperationExecuted?.Invoke(this, new OperationExecutedEventArgs(operationName));
        }
    }
}
