using System;

namespace ArwynFr.AspectNetSharp
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public abstract class ObserverAttributeBase : Attribute
    {
        private string _name;

        public ObserverAttributeBase(string name) { _name = name; }

        public string Name { get { return _name; } }
    }
}
