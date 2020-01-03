using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace ArwynFr.AspectNetSharp
{
    public abstract class ObserverCollectionBase<TObservable> : ObserverBase<TObservable>
        where TObservable : ObservableBase
    {
        private readonly ILookup<string, NotifyCollectionChangedEventHandler> _collChangedHandlers;
        private readonly Dictionary<INotifyCollectionChanged, string> _collectionNames = new Dictionary<INotifyCollectionChanged, string>();

        public ObserverCollectionBase()
        {
            _collChangedHandlers = CreateLookup<NotifyCollectionChangedEventHandler, CollectionChangedAttribute>();            
        }

        private IDictionary<string, INotifyCollectionChanged> ObservableCollections
        {
            get
            {
                return typeof(TObservable).GetProperties()
                    .Where(prop => prop.GetValue(_observable) is INotifyCollectionChanged)
                    .ToDictionary(
                        prop => prop.Name,
                        prop => prop.GetValue(_observable) as INotifyCollectionChanged);

            }
        }

        public override TObservable Observable
        {
            set
            {
                base.Observable = value;
                foreach (var collection in ObservableCollections)
                {
                    _collectionNames[collection.Value] = collection.Key;
                    collection.Value.CollectionChanged += OnCollectionChanged;
                }
            }
        }

        public override void Dispose()
        {
            if (_disposed) return;
            foreach (var collection in ObservableCollections)
            {
                collection.Value.CollectionChanged -= OnCollectionChanged;
                _collectionNames.Remove(collection.Value);
            }
            base.Dispose();
        }

        [PropertyChanging]
        public void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            var property = typeof(TObservable).GetProperty(e.PropertyName);
            if (!typeof(INotifyCollectionChanged).IsAssignableFrom(property.PropertyType)) return;
            var value = property.GetValue(sender) as INotifyCollectionChanged;
            if (value == null) return;
            value.CollectionChanged -= OnCollectionChanged;
            _collectionNames.Remove(value);
        }

        [PropertyChanged]
        public void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var property = typeof(TObservable).GetProperty(e.PropertyName);
            if (!typeof(INotifyCollectionChanged).IsAssignableFrom(property.PropertyType)) return;
            var value = property.GetValue(sender) as INotifyCollectionChanged;
            if (value == null) return;
            _collectionNames[value] = e.PropertyName;
            value.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var propertyName = _collectionNames[sender as INotifyCollectionChanged];
            RaiseHandler(sender, e, _collChangedHandlers, propertyName);
        }
    }
}
