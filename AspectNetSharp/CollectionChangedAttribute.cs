
namespace ArwynFr.AspectNetSharp
{
    public sealed class CollectionChangedAttribute : ObserverAttributeBase
    {
        public CollectionChangedAttribute(string property = "") : base(property) { }
    }
}
