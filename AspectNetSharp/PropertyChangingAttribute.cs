
namespace ArwynFr.AspectNetSharp
{
    public sealed class PropertyChangingAttribute : ObserverBaseAttribute
    {
        public PropertyChangingAttribute(): this(string.Empty) { }

        public PropertyChangingAttribute(string property) : base(property) { }
    }
}
