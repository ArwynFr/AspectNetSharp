
namespace ArwynFr.AspectNetSharp
{
    public sealed class PropertyChangingAttribute : ObserverBaseAttribute
    {
        public PropertyChangingAttribute(string property = "") : base(property) { }
    }
}
