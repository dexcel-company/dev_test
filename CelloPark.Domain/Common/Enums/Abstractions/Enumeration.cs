using System.Reflection;

namespace CelloPark.Domain.Common.Enums.Abstractions;

public abstract class Enumeration<TElement> :
    IEquatable<Enumeration<TElement>> where TElement : Enumeration<TElement>
{
    public static readonly IEnumerable<TElement> Elements;

    static Enumeration()
    {
        Elements = FindElements();
        _enumerationDictionary = FindElements().ToDictionary(element => element.Key);
    }

    protected Enumeration(byte key, string value)
    {
        Key = key;
        Value = value;
    }

    public byte Key { get; protected set; }
    public string Value { get; protected set; } = string.Empty;
    protected virtual Type EqualityContract => typeof(Enumeration<TElement>);

    private static readonly Dictionary<byte, TElement> _enumerationDictionary;

    public static TElement? FromKey(byte key)
    {
        return _enumerationDictionary.TryGetValue(key, out TElement? enumeration)
            ? enumeration
            : null;
    }

    public static TElement? FromKey(string? key)
    {
        bool isParsed = byte.TryParse(key, out byte parsedKey);

        return !isParsed ? null : FromKey(parsedKey);
    }

    public static TElement? FromValue(string value)
    {
        return _enumerationDictionary.Values.SingleOrDefault(x => x.Value == value);
    }

    public override bool Equals(object? obj)
    {
        return obj is Enumeration<TElement> other && Equals(other);
    }

    public virtual bool Equals(Enumeration<TElement>? other)
    {
        return (object)this == other || (other is not null
            && EqualityContract == other.EqualityContract
            && EqualityComparer<int>.Default.Equals(Key, other.Key)
            && EqualityComparer<string>.Default.Equals(Value, other.Value));
    }

    public override int GetHashCode()
    {
        return EqualityComparer<Type>.Default.GetHashCode(EqualityContract)
            * -1521134295
            + EqualityComparer<int>.Default.GetHashCode(Key)
            * -1521134295
            + EqualityComparer<string>.Default.GetHashCode(Value);
    }

    public override string ToString()
    {
        return Value;
    }

    public static bool operator !=(Enumeration<TElement>? left, Enumeration<TElement>? right)
    {
        return !(left == right);
    }

    public static bool operator ==(Enumeration<TElement>? left, Enumeration<TElement>? right)
    {
        return (object?)left == right || (left is not null && left.Equals(right));
    }

    private static IEnumerable<TElement> FindElements()
    {
        Type enumerationType = typeof(TElement);

        return enumerationType
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(field => enumerationType.IsAssignableFrom(field.FieldType))
            .Select(field => (TElement)field.GetValue(default)!);
    }
}
