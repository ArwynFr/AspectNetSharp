using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ArwynFr.AspectNetSharp
{
    internal static class PropertyChangeExtension
    {
        internal static bool SetPropertyValue<T>(this PropertyChangedEventHandler postHandler, PropertyChangingEventHandler preHandler, Func<T> getter, Action<T> setter, T newValue, [CallerMemberName] string propertyName = "")
        {
            T oldValue = getter();
            if (Equals(oldValue, newValue)) { return false; }
            if (!Equals(preHandler.Target, postHandler.Target)) { throw new ArgumentException("preHandler and postHandler have different targets"); }
            try
            {
                preHandler.Invoke(preHandler.Target, new PropertyChangingEventArgs(propertyName));
                setter(newValue);
                postHandler.Invoke(postHandler.Target, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            catch
            {
                setter(oldValue);
                return false;
            }
        }

        internal static bool ExecuteOperation(this OperationExecutedEventHandler postHandler, OperationExecutingEventHandler preHandler, Action<object[]> operation, [CallerMemberName] string operationName = "")
        {
            try
            {
                var values = preHandler.InvokeMulticast(preHandler.Target, new OperationExecutingEventArgs(operationName));
                operation(values);
                postHandler.Invoke(postHandler.Target, new OperationExecutedEventArgs(operationName));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static object[] InvokeMulticast(this MulticastDelegate deleg, params object[] parameters)
        {
            return deleg.GetInvocationList().Select(handler => handler.DynamicInvoke(parameters)).ToArray();
        }
    }
}
