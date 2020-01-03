
namespace ArwynFr.AspectNetSharp
{
    public sealed class CollectionChangedAttribute : ObserverBaseAttribute
    {
        public CollectionChangedAttribute() : this(string.Empty) { }

        public CollectionChangedAttribute(string property) : base(property) { }
    }
}
