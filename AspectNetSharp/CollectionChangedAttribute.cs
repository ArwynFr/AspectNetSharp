﻿
namespace ArwynFr.AspectNetSharp
{
    public sealed class CollectionChangedAttribute : ObserverBaseAttribute
    {
        public CollectionChangedAttribute(string property = "") : base(property) { }
    }
}
