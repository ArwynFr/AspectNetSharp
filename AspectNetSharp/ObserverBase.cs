using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;

namespace ArwynFr.AspectNetSharp
{
    [InheritedExport, PartCreationPolicy(CreationPolicy.NonShared)]
    public abstract class ObserverBase<TObservable> : IDisposable
        where TObservable : ObservableBase
    {
        private readonly ILookup<string, OperationExecutedEventHandler> _operExecutedHandlers;
        private readonly ILookup<string, OperationExecutingEventHandler> _operExecutingHandlers;
        private readonly ILookup<string, PropertyChangedEventHandler> _propChangedHandlers;
        private readonly ILookup<string, PropertyChangingEventHandler> _propChangingHandlers;
        private TObservable _observable;

        protected ObserverBase()
        {
            _operExecutingHandlers = CreateLookup<OperationExecutingEventHandler, OperationExecutingAttribute>();
            _operExecutedHandlers = CreateLookup<OperationExecutedEventHandler, OperationExecutedAttribute>();
            _propChangingHandlers = CreateLookup<PropertyChangingEventHandler, PropertyChangingAttribute>();
            _propChangedHandlers = CreateLookup<PropertyChangedEventHandler, PropertyChangedAttribute>();
        }

        protected delegate void EventHandler<in TArgs>(object sender, TArgs e)
            where TArgs : EventArgs;

        public TObservable Observable
        {
            get
            {
                return _observable;
            }
            set
            {
                if (object.Equals(_observable, value)) { return; }
                if (_observable != null) { UnWeaveEvents(); }
                _observable = value;
                if (_observable != null) { WeaveEvents(); }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [OperationExecuted(nameof(Dispose))]
        public void OnObservableDisposed_DisposeObserver(object sender, OperationExecutedEventArgs e) { Dispose(); }

        protected ILookup<string, THandler> CreateLookup<THandler, TAttribute>()
            where TAttribute : ObserverBaseAttribute
            where THandler : class
        {
            return GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(meth => meth.GetCustomAttribute<TAttribute>() != null)
                .ToLookup(
                    keySelector: meth => meth.GetCustomAttribute<TAttribute>().Name,
                    elementSelector: meth => Delegate.CreateDelegate(typeof(THandler), this, meth.Name) as THandler);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Observable = null;
            }
        }

        protected static void RaiseHandler<THandler, TArgs>(object sender, TArgs eventArgs, ILookup<string, THandler> handlers, string propertyName)
            where TArgs : EventArgs
        {
            if (handlers == null) { return; }
            if (handlers.Contains(string.Empty))
            {
                foreach (var handler in handlers[string.Empty].OfType<Delegate>())
                {
                    handler.DynamicInvoke(sender, eventArgs);
                }
            }
            if (handlers.Contains(propertyName))
            {
                foreach (var handler in handlers[propertyName].OfType<Delegate>())
                {
                    handler.DynamicInvoke(sender, eventArgs);
                }
            }
        }

        protected virtual void UnWeaveEvents()
        {
            _observable.OperationExecuted -= _observable_OperationExecuted;
            _observable.PropertyChanged -= _observable_PropertyChanged;
            _observable.PropertyChanging -= _observable_PropertyChanging;
            _observable.OperationExecuting -= _observable_OperationExecuting;
        }

        protected virtual void WeaveEvents()
        {
            _observable.OperationExecuted += _observable_OperationExecuted;
            _observable.PropertyChanged += _observable_PropertyChanged;
            _observable.PropertyChanging += _observable_PropertyChanging;
            _observable.OperationExecuting += _observable_OperationExecuting;
        }

        private void _observable_OperationExecuted(object sender, OperationExecutedEventArgs e)
        {
            RaiseHandler(sender, e, _operExecutedHandlers, e.OperationName);
        }

        private void _observable_OperationExecuting(object sender, OperationExecutingEventArgs e)
        {
            RaiseHandler(sender, e, _operExecutingHandlers, e.OperationName);
        }

        private void _observable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaiseHandler(sender, e, _propChangedHandlers, e.PropertyName);
        }

        private void _observable_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            RaiseHandler(sender, e, _propChangingHandlers, e.PropertyName);
        }
    }
}