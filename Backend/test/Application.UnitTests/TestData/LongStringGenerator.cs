using System.CodeDom.Compiler;

namespace Application.UnitTests.TestData;

public static class LongStringGenerator
{
    public static string Generate(int length)
    {
        return new string(Enumerable.Repeat('a', length).ToArray());
    }
}