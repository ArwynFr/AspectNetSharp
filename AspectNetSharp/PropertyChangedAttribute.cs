
namespace ArwynFr.AspectNetSharp
{
    public sealed class PropertyChangedAttribute : ObserverAttributeBase
    {
        public PropertyChangedAttribute(string property = "") : base(property) { }
    }
}
