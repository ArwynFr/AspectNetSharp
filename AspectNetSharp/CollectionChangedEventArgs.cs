using System;
using System.Collections.Specialized;

namespace ArwynFr.AspectNetSharp
{
    public class CollectionChangedEventArgs : EventArgs
    {
        public CollectionChangedEventArgs(string propertyName, NotifyCollectionChangedEventArgs inner)
        {
            PropertyName = propertyName;
            Inner = inner;
        }

        public string PropertyName { get; }
        public NotifyCollectionChangedEventArgs Inner { get; }
    }
}
