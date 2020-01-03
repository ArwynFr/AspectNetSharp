
namespace ArwynFr.AspectNetSharp
{
    public sealed class PropertyChangedAttribute : ObserverBaseAttribute
    {
        public PropertyChangedAttribute(string property = "") : base(property) { }
    }
}
