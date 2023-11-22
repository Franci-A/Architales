using System;

using Object = UnityEngine.Object;

public class NullSingletonException : Exception
{
    public NullSingletonException() : base("Singleton Reference not found !") { }
    public NullSingletonException(string message) : base(message) { }
}

public class MissingIntefaceException<T> : Exception where T : class
{
    public MissingIntefaceException() : base($"Interface {typeof(T)} is not implemented !") { } 
    public MissingIntefaceException(string message) : base(message) { }
    public MissingIntefaceException(Object obj) : base($"{obj.name} does not implement the Interface {typeof(T)} !") { }
}