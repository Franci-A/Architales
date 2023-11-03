using System;

public class NullSingletonException : Exception
{
    public NullSingletonException() : base("Singleton Reference not found !") { }
    public NullSingletonException(string message) : base(message) { }
}