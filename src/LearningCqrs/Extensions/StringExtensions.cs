using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace LearningCqrs.Extensions;

public static class StringExtensions
{
    // white space, em-dash, en-dash, underscore
    private static readonly Regex WordDelimiters = new(@"[\s—–_]", RegexOptions.Compiled);

    // characters that are not valid
    private static readonly Regex InvalidChars = new(@"[^a-z0-9\-]", RegexOptions.Compiled);

    // multiple hyphens
    private static readonly Regex MultipleHyphens = new(@"-{2,}", RegexOptions.Compiled);

    public static string ToUrlSlug(this string value)
    {
        // convert to lower case
        value = value.ToLowerInvariant();

        // remove diacritics (accents)
        value = RemoveDiacritics(value);

        // ensure all word delimiters are hyphens
        value = WordDelimiters.Replace(value, "-");

        // strip out invalid characters
        value = InvalidChars.Replace(value, "");

        // replace multiple hyphens (-) with a single hyphen
        value = MultipleHyphens.Replace(value, "-");

        // trim hyphens (-) from ends
        return value.Trim('-');
    }

    /// See: http://www.siao2.com/2007/05/14/2629747.aspx
    private static string RemoveDiacritics(this string stIn)
    {
        var stFormD = stIn.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var t in stFormD)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(t);
            if (uc != UnicodeCategory.NonSpacingMark) sb.Append(t);
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}