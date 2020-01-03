using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;

namespace ArwynFr.AspectNetSharp
{
    public static class ObservableFactory
    {
        public static CompositionContainer Container = new CompositionContainer(new ApplicationCatalog());

        public static TObservee MakeObservee<TObservee>()
            where TObservee : ObservableBase, new()
        {
            return MakeObservee<TObservee>(new TObservee());
        }

        public static TObservee MakeObservee<TObservee>(TObservee observee)
            where TObservee : ObservableBase
        {
            return new ObservableEventAspect<TObservee>(observee).GetTransparentProxy() as TObservee;
        }

        public static TObservee ComposeObservee<TObservee>()
            where TObservee : ObservableBase, new()
        {
            return ComposeObservee(new TObservee());
        }

        public static TObservee ComposeObservee<TObservee>(TObservee observee)
            where TObservee : ObservableBase
        {
            var value = MakeObservee(observee);
            Container.GetExports<ObserverBase<TObservee>>().Select(lazy => lazy.Value.Observable = value).ToArray();
            return value;
        }
    }
}
