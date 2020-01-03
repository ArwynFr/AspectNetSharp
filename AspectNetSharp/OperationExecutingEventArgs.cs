using System;

namespace ArwynFr.AspectNetSharp
{
    public class OperationExecutingEventArgs : EventArgs
    {
        public string OperationName { get; set; }

        public OperationExecutingEventArgs(string operationName)
        {
            OperationName = operationName;
        }
    }    
}
