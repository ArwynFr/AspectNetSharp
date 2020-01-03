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
        
        public void Dispose() { }

        private void RaisePropertyChanging(string propertyName)
        {
            if (PropertyChanging != null) PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RaiseOperationExecuting(string operationName)
        {
            if (OperationExecuting != null) OperationExecuting(this, new OperationExecutingEventArgs(operationName));
        }

        private void RaiseOperationExecuted(string operationName)
        {
            if (OperationExecuted != null) OperationExecuted(this, new OperationExecutedEventArgs(operationName));
        }
    }
}
