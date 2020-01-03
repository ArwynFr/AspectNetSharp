
namespace Chezsquall.Tools.ClassObservation
{
    public sealed class OperationExecutedAttribute : ObserverAttributeBase
    {
        public OperationExecutedAttribute(string operation = "") : base(operation) { }
    }
}
