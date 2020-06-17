using System;
using System.Linq;
using System.Text;
using MacroSystem;

namespace MacroExceptions
{

    public static class ExceptionExtensions
    {

        /// <summary>
        /// Format exception details for humans to read
        /// </summary>
        ///
        public static string Format(Exception ex)
        {
            if (ex == null) return "";
            var sb = new StringBuilder();
            sb.AppendLine(ex.Message);
            sb.AppendLine("Type: " + ex.GetType().FullName);
            if (ex.Data != null)
            {
                foreach (var key in ex.Data.Keys)
                {
                    sb.AppendLine(StringExtensions.FormatInvariant(
                        "Data.{0}: {1}",
                        key.ToString(),
                        ex.Data[key].ToString()));
                }
            }
            if (!string.IsNullOrWhiteSpace(ex.Source))
            {
                sb.AppendLine("Source: " + ex.Source);
            }
            if (!string.IsNullOrWhiteSpace(ex.HelpLink))
            {
                sb.AppendLine("HelpLink: " + ex.HelpLink);
            }
            if (!string.IsNullOrWhiteSpace(ex.StackTrace))
            {
                sb.AppendLine("StackTrace:");
                sb.AppendLine(StringExtensions.Indent(FormatStackTrace(ex.StackTrace)));
            }
            if (ex.InnerException != null)
            {
                sb.AppendLine("InnerException:");
                sb.AppendLine(StringExtensions.Indent(Format(ex.InnerException)));
            }
            return sb.ToString();
        }


        static string FormatStackTrace(string stackTrace)
        {
            return string.Join(
                Environment.NewLine,
                StringExtensions.SplitLines(stackTrace)
                    .Select(line => line.Trim())
                    .SelectMany(line => {
                        var i = line.IndexOf(" in ", StringComparison.Ordinal);
                        if (i <= 0) return new[] {line};
                        var inPart = line.Substring(i + 1);
                        var atPart = line.Substring(0, i);
                        return new[] {atPart, StringExtensions.Indent(inPart)};
                        }));
        }

    }
}
