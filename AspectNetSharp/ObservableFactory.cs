using System.ComponentModel.Composition.Hosting;

namespace ArwynFr.AspectNetSharp
{
    public static class ObservableFactory
    {
        public static TObservee MakeObservee<TObservee>()
            where TObservee : ObservableBase, new()
        {
            return MakeObservee(new TObservee());
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
            var catalog = new ApplicationCatalog();
            var container = new CompositionContainer(catalog);
            var proxy = MakeObservee(observee);
            foreach(var export in container.GetExports<ObserverBase<TObservee>>())
            {
                export.Value.Observable = proxy;
            }
            return proxy;
        }
    }
}
