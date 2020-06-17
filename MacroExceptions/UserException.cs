using System;
using System.Collections.Generic;
using MacroGuards;


namespace
MacroExceptions
{


/// <summary>
/// A user-facing error
/// </summary>
///
/// <remarks>
/// <para>
/// This exception signals the occurrence of a user-facing error, i.e. one that was the user's fault and/or one that
/// they have the ability to do something about.
/// </para>
/// <para>
/// <see cref="Exception.Message"/>, if present, is suitable for presentation to the end user.
/// </para>
/// <para>
/// <see cref="Exception.InnerException"/>, if it is itself a <see cref="UserException"/> or if
/// <see cref="IsInnerExceptionUserFacing"/> is <c>true</c>, is presentable to the user as additional detail about the
/// problem.  Otherwise it should not be considered safe to display to the user.
/// </para>
/// </remarks>
///
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Microsoft.Usage",
    "CA2237:MarkISerializableTypesWithSerializable",
    Justification = "Don't care about serialization")]
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Microsoft.Design",
    "CA1032:ImplementStandardExceptionConstructors",
    Justification = "Don't care about serialization")]
public class
UserException
    : Exception
{


/// <summary>
/// Initialise a new <see cref="UserException"/> with the specified user-facing message
/// </summary>
///
/// <exception cref="ArgumentNullException">
/// <paramref name="userFacingMessage"/> is <c>null</c>
/// </exception>
///
public
UserException(string userFacingMessage)
    : this(
        Guard.NotNull(userFacingMessage, nameof(userFacingMessage)),
        null,
        false)
{
}


/// <summary>
/// Initialise a new <see cref="UserException"/> with the specified user-facing message and possibly-user-facing inner
/// exception
/// </summary>
///
/// <param name="innerException">
/// A <see cref="UserException"/> containing additional user-facing details about the underlying problem, or an
/// exception of some other type containing non-user-facing diagnostic information about the underlying problem.
/// </param>
///
/// <exception cref="ArgumentNullException">
/// <paramref name="userFacingMessage"/> is <c>null</c>
/// - OR -
/// <paramref name="innerException"/> is <c>null</c>
/// </exception>
///
public
UserException(string userFacingMessage, Exception innerException)
    : this(
        Guard.NotNull(userFacingMessage, nameof(userFacingMessage)),
        Guard.NotNull(innerException, nameof(innerException)),
        false)
{
}


/// <summary>
/// Initialise a new <see cref="UserException"/> promoting the specified inner exception as user-facing
/// </summary>
///
/// <remarks>
/// The promotion does not logically extend to the inner exception's inner exception.
/// </remarks>
///
/// <exception cref="ArgumentNullException">
/// <paramref name="userFacingInnerException"/> is <c>null</c>
/// </exception>
///
public
UserException(Exception userFacingInnerException)
    : this(
        Guard.NotNull(userFacingInnerException, nameof(userFacingInnerException)).Message,
        Guard.NotNull(userFacingInnerException, nameof(userFacingInnerException)),
        true)
{
}


UserException(string message, Exception innerException, bool isInnerExceptionUserFacing)
    : base(message, innerException)
{
    IsInnerExceptionUserFacing = isInnerExceptionUserFacing;
}


/// <summary>
/// Can <see cref="Exception.InnerException"/> be considered user-facing, even if it's not a
/// <see cref="UserException"/>?
/// </summary>
///
public bool
IsInnerExceptionUserFacing
{
    get;
    private set;
}


/// <summary>
/// Chain of exceptions suitable for display to the user
/// </summary>
///
public IEnumerable<Exception>
UserFacingExceptionChain
{
    get
    {
        if (!string.IsNullOrWhiteSpace(Message)) yield return this;
        if (InnerException == null) yield break;
        var innerUserException = InnerException as UserException;
        if (innerUserException != null)
        {
            foreach (var e in innerUserException.UserFacingExceptionChain) yield return e;
        }
        else if (IsInnerExceptionUserFacing)
        {
            yield return InnerException;
        }
    }
}


}
}
