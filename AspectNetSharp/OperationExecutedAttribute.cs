
namespace ArwynFr.AspectNetSharp
{
    public sealed class OperationExecutedAttribute : ObserverAttributeBase
    {
        public OperationExecutedAttribute(string operation = "") : base(operation) { }
    }
}
