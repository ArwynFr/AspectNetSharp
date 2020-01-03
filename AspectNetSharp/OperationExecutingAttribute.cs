
namespace ArwynFr.AspectNetSharp
{
    public sealed class OperationExecutingAttribute : ObserverAttributeBase
    {
        public OperationExecutingAttribute(string operation = "") : base(operation) { }
    }
}
