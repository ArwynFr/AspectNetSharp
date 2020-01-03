
namespace ArwynFr.AspectNetSharp
{
    public sealed class OperationExecutingAttribute : ObserverBaseAttribute
    {
        public OperationExecutingAttribute() : this(string.Empty) { }

        public OperationExecutingAttribute(string operation) : base(operation) { }
    }
}
