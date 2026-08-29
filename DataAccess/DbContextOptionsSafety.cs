using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DataAccess;

public static class DbContextOptionsSafety
{
    /// <summary>
    /// Prevents accidental sibling-collection joins from reaching production.
    /// Callers that intentionally load multiple collections must choose an
    /// explicit split-query or a measured single-query shape.
    /// </summary>
    public static DbContextOptionsBuilder UsePortfolioQuerySafety(
        this DbContextOptionsBuilder options)
    {
        options.ConfigureWarnings(warnings =>
            warnings.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
        return options;
    }

    public static DbContextOptionsBuilder<TContext> UsePortfolioQuerySafety<TContext>(
        this DbContextOptionsBuilder<TContext> options)
        where TContext : DbContext
    {
        options.ConfigureWarnings(warnings =>
            warnings.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
        return options;
    }
}
