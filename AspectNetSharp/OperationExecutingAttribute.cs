
namespace Chezsquall.Tools.ClassObservation
{
    public sealed class OperationExecutingAttribute : ObserverAttributeBase
    {
        public OperationExecutingAttribute(string operation = "") : base(operation) { }
    }
}
