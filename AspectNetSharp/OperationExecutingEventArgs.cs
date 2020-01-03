using System;

namespace ArwynFr.AspectNetSharp
{
    public class OperationExecutingEventArgs : EventArgs
    {
        public string OperationName { get; }

        public OperationExecutingEventArgs(string operationName)
        {
            OperationName = operationName;
        }
    }    
}
