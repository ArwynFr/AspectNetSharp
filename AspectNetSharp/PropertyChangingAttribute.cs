
namespace ArwynFr.AspectNetSharp
{
    public sealed class PropertyChangingAttribute : ObserverAttributeBase
    {
        public PropertyChangingAttribute(string property = "") : base(property) { }
    }
}
