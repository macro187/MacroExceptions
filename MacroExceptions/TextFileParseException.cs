using System;
using System.Globalization;
using MacroGuards;


namespace
MacroExceptions
{


/// <summary>
/// Unexpected content encountered while parsing a text file
/// </summary>
///
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Microsoft.Design",
    "CA1032:ImplementStandardExceptionConstructors",
    Justification = "Don't care about serialization")]
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Microsoft.Usage",
    "CA2237:MarkISerializableTypesWithSerializable",
    Justification = "Don't care about serialization")]
public class
TextFileParseException
    : Exception
{


public
TextFileParseException(string description, int lineNumber, string line)
    : this(description, null, lineNumber, line, null)
{
}


public
TextFileParseException(string description, string path, int lineNumber, string line)
    : this(description, path, lineNumber, line, null)
{
}


public
TextFileParseException(string description, int lineNumber, string line, Exception innerException)
    : this(description, null, lineNumber, line, innerException)
{
}


public
TextFileParseException(string description, string path, int lineNumber, string line, Exception innerException)
    : base("", innerException)
{
    Guard.Required(description, nameof(description));
    if (lineNumber < 1) throw new ArgumentOutOfRangeException("lineNumber");

    Description = description;
    Path = path;
    LineNumber = lineNumber;
    Line = line ?? "";
}


/// <summary>
/// Description of the unexpected content
/// </summary>
///
public string
Description
{
    get;
    private set;
}


/// <summary>
/// Path or filename of the file being parsed
/// </summary>
///
public string
Path
{
    get { return _path; }
    set { _path = value ?? ""; }
}

string _path;


/// <summary>
/// 1-based line number the parse error was on
/// </summary>
///
public int
LineNumber
{
    get;
    private set;
}


/// <summary>
/// Contents of the line the parse error was on
/// </summary>
///
public string
Line
{
    get;
    private set;
}


/// <inheritdoc/>
public override string
Message
{
    get
    {
        return string.Format(
            CultureInfo.InvariantCulture,
            "Error{1} on line {2}{0}" +
            "  {3}{0}" +
            "  {4}",
            Environment.NewLine,
            Path.Length > 0 ? " in " + Path : "",
            LineNumber,
            Description,
            Line);
    }
}


}
}
