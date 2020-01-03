
namespace Chezsquall.Tools.ClassObservation
{
    public sealed class CollectionChangedAttribute : ObserverAttributeBase
    {
        public CollectionChangedAttribute(string property = "") : base(property) { }
    }
}
