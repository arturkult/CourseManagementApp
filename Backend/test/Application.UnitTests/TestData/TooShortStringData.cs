using System.Collections;

namespace Application.UnitTests.TestData;

public class TooShortStringData: IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { string.Empty };
        yield return new object[] { " " };
        yield return new object[] { null };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}