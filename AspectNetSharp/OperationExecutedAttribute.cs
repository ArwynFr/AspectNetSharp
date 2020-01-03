
namespace ArwynFr.AspectNetSharp
{
    public sealed class OperationExecutedAttribute : ObserverBaseAttribute
    {
        public OperationExecutedAttribute(string operation = "") : base(operation) { }
    }
}
