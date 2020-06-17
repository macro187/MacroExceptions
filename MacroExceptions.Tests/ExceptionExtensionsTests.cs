using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace
MacroExceptions.Tests
{


[TestClass]
public class
ExceptionExtensionsTests
{


[TestMethod]
public void
Format_Handles_Minimal_Exception_Details()
{
    try
    {
        throw new Exception();
    }
    catch (Exception e)
    {
        Debug.Print(ExceptionExtensions.Format(e));
    }
}


[TestMethod]
public void
Format_Handles_Lots_Of_Exception_Details()
{
    try
    {
        try
        {
            throw new Exception("Test inner exception Message");
        }
        catch (Exception inner)
        {
            var outer = new Exception("Test outer exception Message", inner);
            outer.HelpLink = "Test HelpLink";
            outer.Source = "Test Source";
            outer.Data.Add("TestDataKey1", "TestDataValue1");
            outer.Data.Add("TestDataKey2", "TestDataValue2");
            throw outer;
        }
    }
    catch (Exception e)
    {
        Debug.Print(ExceptionExtensions.Format(e));
    }
}


}
}
