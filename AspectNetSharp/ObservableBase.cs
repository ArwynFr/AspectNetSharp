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

        protected void RaisePropertyChanging(string propertyName) => PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));

        protected void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected void RaiseOperationExecuting(string operationName) => OperationExecuting?.Invoke(this, new OperationExecutingEventArgs(operationName));

        protected void RaiseOperationExecuted(string operationName) => OperationExecuted?.Invoke(this, new OperationExecutedEventArgs(operationName));
    }
}
