using System;

namespace Chezsquall.Tools.ClassObservation
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
