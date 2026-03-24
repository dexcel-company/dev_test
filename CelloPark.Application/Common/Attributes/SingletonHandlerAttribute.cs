namespace CelloPark.Application.Common.Attributes;

[AttributeUsage(AttributeTargets.Interface)]
public sealed class SingletonHandlerAttribute : Attribute
{
    public SingletonHandlerAttribute() { }
}
