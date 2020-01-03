using System;

namespace ArwynFr.AspectNetSharp
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public abstract class ObserverBaseAttribute : Attribute
    {
        protected ObserverBaseAttribute(string name) { Name = name; }

        public string Name { get; }
    }
}
