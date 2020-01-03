using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace ArwynFr.AspectNetSharp
{
    internal class ObservableEventAspect<T> : RealProxy
        where T : ObservableBase
    {
        private readonly T _target;

        internal ObservableEventAspect(T target)
            : base(typeof(T))
        {
            _target = target;
        }

        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = msg as IMethodCallMessage;

            if (!methodCall.MethodBase.IsSpecialName)
                return InvokeNormalMethod(methodCall);

            if (methodCall.MethodName.StartsWith("set_") &&
                typeof(T).GetProperty(methodCall.MethodName.Substring(4)) != null)
                return InvokePropertySetter(methodCall);

            return InvokeSpecialMethod(methodCall);
        }

        private void RaiseEvent(string eventName, string propertyName)
        {
            var method = typeof(ObservableBase).GetMethod(string.Format("Raise{0}", eventName), BindingFlags.NonPublic | BindingFlags.Instance);
            if (method != null) method.Invoke(_target, new object[] { propertyName });
        }

        private IMessage InvokePropertySetter(IMethodCallMessage msg)
        {
            var propertyName = msg.MethodName.Substring(4);
            var oldValue = typeof(T).GetProperty(propertyName).GetValue(_target);
            try
            {
                RaiseEvent("PropertyChanging", propertyName);
                try { msg.MethodBase.Invoke(_target, msg.InArgs); }
                catch (TargetInvocationException e) { return new ReturnMessage(e.InnerException, msg); }
                RaiseEvent("PropertyChanged", propertyName);
            }
            catch
            {
                typeof(T).GetProperty(propertyName).SetValue(_target, oldValue);
            }
            return new ReturnMessage(null, null, 0, msg.LogicalCallContext, msg);
        }

        private IMessage InvokeNormalMethod(IMethodCallMessage msg)
        {
            try
            {
                RaiseEvent("OperationExecuting", msg.MethodName);
                var result = msg.MethodBase.Invoke(_target, msg.InArgs);
                RaiseEvent("OperationExecuted", msg.MethodName);
                return new ReturnMessage(result, null, 0, msg.LogicalCallContext, msg);
            }
            catch (TargetInvocationException e)
            {
                return new ReturnMessage(e.InnerException, msg);
            }
        }

        private IMessage InvokeSpecialMethod(IMethodCallMessage msg)
        {
            try
            {
                var result = msg.MethodBase.Invoke(_target, msg.InArgs);
                return new ReturnMessage(result, null, 0, msg.LogicalCallContext, msg);
            }
            catch (TargetInvocationException e)
            {
                return new ReturnMessage(e.InnerException, msg);
            }
        }
    }
}
