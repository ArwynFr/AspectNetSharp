using System;

namespace Chezsquall.Tools.ClassObservation
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
