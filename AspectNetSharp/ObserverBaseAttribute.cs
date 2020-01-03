using System;

namespace ArwynFr.AspectNetSharp
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public abstract class ObserverBaseAttribute : Attribute
    {
        private string _name;

        public ObserverBaseAttribute(string name) { _name = name; }

        public string Name { get { return _name; } }
    }
}
