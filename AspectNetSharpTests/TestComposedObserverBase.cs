using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace ArwynFr.AspectNetSharp.Tests
{
    [TestClass]
    public class TestComposedObserverBase
    {

        public static object OperationExecutingSender { get; private set; }
        public static object OperationExecutedSender { get; private set; }
        public static object PropertyChangingSender { get; private set; }
        public static object PropertyChangedSender { get; private set; }

        public static OperationExecutingEventArgs OperationExecutingEventArgs { get; private set; }
        public static OperationExecutedEventArgs OperationExecutedEventArgs { get; private set; }
        public static PropertyChangingEventArgs PropertyChangingEventArgs { get; private set; }
        public static PropertyChangedEventArgs PropertyChangedEventArgs { get; private set; }

        [TestCleanup]
        public void CleanUp()
        {
            OperationExecutingSender = null;
            OperationExecutedSender = null;
            PropertyChangingSender = null;
            PropertyChangedSender = null;
        }

        [TestMethod]
        public void TestPropertyChanging()
        {
            using (var observable = ObservableFactory.ComposeObservee<DummyModele>())
            {
                observable.Property = string.Empty;
                Assert.AreEqual(observable as object, PropertyChangingSender);
                Assert.AreEqual("Property", PropertyChangingEventArgs.PropertyName);
            }
        }

        [TestMethod]
        public void TestPropertyChanged()
        {
            using (var observable = ObservableFactory.ComposeObservee<DummyModele>())
            {
                observable.Property = string.Empty;
                Assert.AreEqual(observable as object, PropertyChangedSender);
                Assert.AreEqual("Property", PropertyChangedEventArgs.PropertyName);
            }
        }

        [TestMethod]
        public void TestOperationExecuting()
        {
            using (var observable = ObservableFactory.ComposeObservee<DummyModele>())
            {
                observable.Operation();
                Assert.AreEqual(observable as object, OperationExecutingSender);
                Assert.AreEqual("Operation", OperationExecutingEventArgs.OperationName);
            }            
        }

        [TestMethod]
        public void TestOperationExecuted()
        {
            using (var observable = ObservableFactory.ComposeObservee<DummyModele>())
            {
                observable.Operation();
                Assert.AreEqual(observable as object, OperationExecutedSender);
                Assert.AreEqual("Operation", OperationExecutedEventArgs.OperationName);
            }
        }

        private class DummyModele : ObservableBase
        {
            public string Property { get; set; }

            public void Operation() { }
        }

        [PartCreationPolicy(CreationPolicy.NonShared)]
        private class DummyObserver : ObserverBase<DummyModele>
        {
            [PropertyChanging]
            void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
            {
                PropertyChangingSender = sender;
                PropertyChangingEventArgs = e;
            }

            [PropertyChanged]
            void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                PropertyChangedSender = sender;
                PropertyChangedEventArgs = e;
            }

            [OperationExecuting("Operation")]
            void OnOperationExecuting(object sender, OperationExecutingEventArgs e)
            {
                OperationExecutingSender = sender;
                OperationExecutingEventArgs = e;
            }

            [OperationExecuted("Operation")]
            void OnOperationExecuted(object sender, OperationExecutedEventArgs e)
            {
                OperationExecutedSender = sender;
                OperationExecutedEventArgs = e;
            }

        }
    }
}
