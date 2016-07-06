using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;

namespace Chezsquall.Tools.ClassObservation
{
    [TestClass]
    public class TestObserverBase
    {
        [TestMethod]
        public void TestPropertyChanging()
        {
            var observable = ObservableFactory.MakeObservee<DummyModele>();
            var target = new DummyObserver() { Observable = observable };
            observable.Property = string.Empty;
            target.Dispose();
            Assert.AreEqual(observable as object, target.PropertyChangingSender);
            Assert.AreEqual("Property", target.PropertyChangingEventArgs.PropertyName);
        }

        [TestMethod]
        public void TestPropertyChanged()
        {
            var observable = ObservableFactory.MakeObservee<DummyModele>();
            var target = new DummyObserver() { Observable = observable };
            observable.Property = string.Empty;
            target.Dispose();
            Assert.AreEqual(observable as object, target.PropertyChangedSender);
            Assert.AreEqual("Property", target.PropertyChangedEventArgs.PropertyName);
        }

        [TestMethod]
        public void TestOperationExecuting()
        {
            var observable = ObservableFactory.MakeObservee<DummyModele>();
            var target = new DummyObserver() { Observable = observable };
            observable.Operation();
            target.Dispose();
            Assert.AreEqual(observable as object, target.OperationExecutingSender);
            Assert.AreEqual("Operation", target.OperationExecutingEventArgs.OperationName);
        }

        [TestMethod]
        public void TestOperationExecuted()
        {
            var observable = ObservableFactory.MakeObservee<DummyModele>();
            var target = new DummyObserver() { Observable = observable };
            observable.Operation();
            target.Dispose();
            Assert.AreEqual(observable as object, target.OperationExecutedSender);
            Assert.AreEqual("Operation", target.OperationExecutedEventArgs.OperationName);
        }

        private class DummyModele : ObservableBase
        {
            public string Property { get; set; }

            public void Operation() { }
        }

        private class DummyObserver : ObserverBase<DummyModele>
        {
            public object OperationExecutingSender { get; private set; }
            public object OperationExecutedSender { get; private set; }
            public object PropertyChangingSender { get; private set; }
            public object PropertyChangedSender { get; private set; }

            public OperationExecutingEventArgs OperationExecutingEventArgs { get; private set; }
            public OperationExecutedEventArgs OperationExecutedEventArgs { get; private set; }
            public PropertyChangingEventArgs PropertyChangingEventArgs { get; private set; }
            public PropertyChangedEventArgs PropertyChangedEventArgs { get; private set; }

            [PropertyChanging]
            void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
            {
                this.PropertyChangingSender = sender;
                this.PropertyChangingEventArgs = e;
            }

            [PropertyChanged]
            void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                this.PropertyChangedSender = sender;
                this.PropertyChangedEventArgs = e;
            }

            [OperationExecuting]
            void OnOperationExecuting(object sender, OperationExecutingEventArgs e)
            {
                this.OperationExecutingSender = sender;
                this.OperationExecutingEventArgs = e;
            }

            [OperationExecuted]
            void OnOperationExecuted(object sender, OperationExecutedEventArgs e)
            {
                this.OperationExecutedSender = sender;
                this.OperationExecutedEventArgs = e;
            }

        }
    }
}
