namespace CelloPark.Application.Common.Attributes;

[AttributeUsage(AttributeTargets.Interface)]
public sealed class ScopedHandlerAttribute : Attribute
{
    public ScopedHandlerAttribute() { }
}
