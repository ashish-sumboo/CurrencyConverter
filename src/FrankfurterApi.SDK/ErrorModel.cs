namespace FrankfurterApi.SDK
{
    using System.Collections.Generic;

    internal sealed record ErrorModel(IEnumerable<string> ErrorCodes);
}
