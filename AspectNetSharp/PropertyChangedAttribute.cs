
namespace ArwynFr.AspectNetSharp
{
    public sealed class PropertyChangedAttribute : ObserverBaseAttribute
    {
        public PropertyChangedAttribute() : this(string.Empty) { }

        public PropertyChangedAttribute(string property) : base(property) { }
    }
}
