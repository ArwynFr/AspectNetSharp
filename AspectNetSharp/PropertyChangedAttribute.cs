
namespace Chezsquall.Tools.ClassObservation
{
    public sealed class PropertyChangedAttribute : ObserverAttributeBase
    {
        public PropertyChangedAttribute(string property = "") : base(property) { }
    }
}
