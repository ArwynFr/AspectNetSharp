
namespace ArwynFr.AspectNetSharp
{
    public sealed class OperationExecutedAttribute : ObserverBaseAttribute
    {
        public OperationExecutedAttribute() : this(string.Empty) { }

        public OperationExecutedAttribute(string operation) : base(operation) { }
    }
}
