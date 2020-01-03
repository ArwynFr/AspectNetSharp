using System;

namespace ArwynFr.AspectNetSharp
{
    public class OperationExecutedEventArgs : EventArgs
    {
        public string OperationName { get; }

        public OperationExecutedEventArgs(string operationName)
        {
            OperationName = operationName;
        }
    }
}
