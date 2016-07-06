using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Chezsquall.Tools.ClassObservation
{
    [TestClass]
    public class TestObserverCollectionBase
    {
        private object CollectionChangedSender;
        private NotifyCollectionChangedEventArgs CollectionChangedEventArgs;

        [TestMethod]
        public void TestCollectionChanged()
        {
            var newItem = "azertyuiop";
            var observable = ObservableFactory.MakeObservee<DummyObservable>();
            observable.Collection.CollectionChanged += Collection_CollectionChanged;
            var target = new DummyObserver() { Observable = observable };
            observable.Collection.Add(newItem);
            target.Dispose();
            Assert.AreSame(CollectionChangedSender, target.CollectionChangedSender);
            Assert.AreSame(CollectionChangedEventArgs, target.CollectionChangedEventArgs);
        }

        private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChangedSender = sender;
            CollectionChangedEventArgs = e;
        }

        private class DummyObservable : ObservableBase
        {
            public DummyObservable() { Collection = new ObservableCollection<string>(); }

            public ObservableCollection<string> Collection { get; set; }
        }

        private class DummyObserver : ObserverCollectionBase<DummyObservable>
        {
            public object CollectionChangedSender { get; private set; }
            public object CollectionChangedEventArgs { get; private set; }

            [CollectionChanged]
            public void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                CollectionChangedSender = sender;
                CollectionChangedEventArgs = e;
            }
        }
    }
}
