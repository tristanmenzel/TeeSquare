using System.Text.RegularExpressions;
using TeeSquare.Reflection;

namespace TeeSquare.Util
{
    public class CaseHelper
    {

        public static string ToCase(string name, NameConvention nameConvention)
        {
            if (string.IsNullOrEmpty(name)) return name;
            if (name.Contains("-"))
            {
                name = new Regex(@"\-([a-z]?)", RegexOptions.IgnoreCase)
                    .Replace(name, m => m.Groups[1].Value.ToUpper());
            }

            switch (nameConvention)
            {
                case NameConvention.CamelCase:
                    return $"{name.Substring(0, 1).ToLower()}{name.Substring(1)}";
                case NameConvention.PascalCase:
                    return $"{name.Substring(0, 1).ToUpper()}{name.Substring(1)}";
                default:
                    return name;
            }
        }
    }
}
