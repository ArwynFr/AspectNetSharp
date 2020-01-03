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
        protected TObservable _observable;
        protected bool _disposed = false;

        private ILookup<string, PropertyChangingEventHandler> _propChangingHandlers;
        private ILookup<string, PropertyChangedEventHandler> _propChangedHandlers;
        private ILookup<string, OperationExecutingEventHandler> _operExecutingHandlers;
        private ILookup<string, OperationExecutedEventHandler> _operExecutedHandlers;

        protected ObserverBase()
        {
            _operExecutingHandlers = CreateLookup<OperationExecutingEventHandler, OperationExecutingAttribute>();
            _operExecutedHandlers = CreateLookup<OperationExecutedEventHandler, OperationExecutedAttribute>();
            _propChangingHandlers = CreateLookup<PropertyChangingEventHandler, PropertyChangingAttribute>();
            _propChangedHandlers = CreateLookup<PropertyChangedEventHandler, PropertyChangedAttribute>();
        }

        public virtual TObservable Observable
        {
            set
            {
                if (value == null) throw new ArgumentNullException();
                if (_observable != null) throw new InvalidOperationException();

                _observable = value;

                _observable.OperationExecuted += _observable_OperationExecuted;
                _observable.PropertyChanged += _observable_PropertyChanged;
                _observable.PropertyChanging += _observable_PropertyChanging;
                _observable.OperationExecuting += _observable_OperationExecuting;
            }
        }

        public bool IsDisposed { get { return _disposed; } }

        public virtual void Dispose()
        {
            if (_disposed) return;
            _observable.OperationExecuting -= _observable_OperationExecuting;
            _observable.PropertyChanging -= _observable_PropertyChanging;
            _observable.PropertyChanged -= _observable_PropertyChanged;
            _observable.OperationExecuted -= _observable_OperationExecuted;
            _disposed = true;
        }

        [OperationExecuted("Dispose")]
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

        protected delegate void EventHandler<TArgs>(object sender, TArgs e)
            where TArgs : EventArgs;

        protected void RaiseHandler<THandler, TArgs>(object sender, TArgs e, ILookup<string, THandler> handlers, string propertyName)
            where TArgs : EventArgs
        {
            if (handlers == null) return;
            if (handlers.Contains(string.Empty))
                foreach (var handler in handlers[string.Empty].OfType<Delegate>()) handler.DynamicInvoke(sender, e);
            if (handlers.Contains(propertyName))
                foreach (var handler in handlers[propertyName].OfType<Delegate>()) handler.DynamicInvoke(sender, e);
        }

        private void _observable_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            RaiseHandler(sender, e, _propChangingHandlers, e.PropertyName);
        }

        private void _observable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaiseHandler(sender, e, _propChangedHandlers, e.PropertyName);
        }

        private void _observable_OperationExecuting(object sender, OperationExecutingEventArgs e)
        {
            RaiseHandler(sender, e, _operExecutingHandlers, e.OperationName);
        }

        private void _observable_OperationExecuted(object sender, OperationExecutedEventArgs e)
        {
            RaiseHandler(sender, e, _operExecutedHandlers, e.OperationName);
        }
    }
}
