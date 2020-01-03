using System;

namespace ArwynFr.AspectNetSharp
{
    public class OperationExecutedEventArgs : EventArgs
    {
        public string OperationName { get; set; }

        public OperationExecutedEventArgs(string operationName)
        {
            OperationName = operationName;
        }
    }
}
