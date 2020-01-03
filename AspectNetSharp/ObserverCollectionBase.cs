using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace ArwynFr.AspectNetSharp
{
    public abstract class ObserverCollectionBase<TObservable> : ObserverBase<TObservable>
        where TObservable : ObservableBase
    {
        private readonly ILookup<string, CollectionChangedEventHandler> _collChangedHandlers;
        private readonly IDictionary<string, NotifyCollectionChangedEventHandler> _currentHandlers;

        protected ObserverCollectionBase()
        {
            _collChangedHandlers = CreateLookup<CollectionChangedEventHandler, CollectionChangedAttribute>();
            _currentHandlers = new Dictionary<string, NotifyCollectionChangedEventHandler>();
        }

        [PropertyChanged]
        public void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UnWeaveProperty(e.PropertyName);
        }

        [PropertyChanging]
        public void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            WeaveProperty(e.PropertyName);
        }

        protected override void UnWeaveEvents()
        {
            base.UnWeaveEvents();
            foreach (var propertyName in ObservableCollectionsPropertyNames())
            {
                UnWeaveProperty(propertyName);
            }
        }

        protected override void WeaveEvents()
        {
            base.WeaveEvents();
            foreach (var propertyName in ObservableCollectionsPropertyNames())
            {
                WeaveProperty(propertyName);
            }
        }

        private NotifyCollectionChangedEventHandler MakeHandler(string propertyName)
        {
            return (sender, e) =>
            {
                var args = new CollectionChangedEventArgs(propertyName, e);
                RaiseHandler(sender, args, _collChangedHandlers, propertyName);
            };
        }

        private IEnumerable<string> ObservableCollectionsPropertyNames()
        {
            return typeof(TObservable).GetProperties()
                .Where(prop => prop.GetValue(this.Observable) is INotifyCollectionChanged)
                .Select(prop => prop.Name);
        }

        private INotifyCollectionChanged PropertyAsObservableCollection(string propertyName)
        {
            var property = typeof(TObservable).GetProperty(propertyName);
            if (!typeof(INotifyCollectionChanged).IsAssignableFrom(property.PropertyType)) { return null; }
            return property.GetValue(Observable) as INotifyCollectionChanged;
        }

        private void UnWeaveProperty(string propertyName)
        {
            var property = PropertyAsObservableCollection(propertyName);
            if (property == null) { return; }
            var handler = _currentHandlers[propertyName];
            property.CollectionChanged -= handler;
            _currentHandlers.Remove(propertyName);
        }

        private void WeaveProperty(string propertyName)
        {
            var property = PropertyAsObservableCollection(propertyName);
            if (property == null) { return; }
            var handler = MakeHandler(propertyName);
            _currentHandlers.Add(propertyName, handler);
            property.CollectionChanged += handler;
        }
    }
}