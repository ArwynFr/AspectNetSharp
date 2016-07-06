
namespace Chezsquall.Tools.ClassObservation
{
    public sealed class PropertyChangingAttribute : ObserverAttributeBase
    {
        public PropertyChangingAttribute(string property = "") : base(property) { }
    }
}
