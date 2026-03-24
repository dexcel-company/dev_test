using System.Text.RegularExpressions;

namespace CelloPark.Domain.Common.Regexes;

public static partial class ValidationRegexes
{
    public static readonly Regex EmailRegex = GetEmailRegex();
    public static readonly Regex PhoneNumberRegex = GetPhoneNumberRegex();
    public static readonly Regex ShadowIdRegex = GetShadowIdRegex();

    [GeneratedRegex("^\\S+@\\S+\\.\\S+$")]
    private static partial Regex GetEmailRegex();

    [GeneratedRegex("\"^\\\\+?[1-9][0-9]{7,14}$\"")]
    private static partial Regex GetPhoneNumberRegex();

    [GeneratedRegex("^[0-9]*$")]
    private static partial Regex GetShadowIdRegex();
}
