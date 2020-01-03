
namespace ArwynFr.AspectNetSharp
{
    public sealed class OperationExecutingAttribute : ObserverBaseAttribute
    {
        public OperationExecutingAttribute(string operation = "") : base(operation) { }
    }
}
