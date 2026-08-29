using Application.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DataAccess.Services;

public sealed class PersistenceExceptionClassifier : IPersistenceExceptionClassifier
{
    public PersistenceExceptionKind Classify(Exception exception)
    {
        if (exception is DbUpdateConcurrencyException)
        {
            return PersistenceExceptionKind.ConcurrencyConflict;
        }

        var postgres = FindPostgresException(exception);
        if (postgres is null)
        {
            return PersistenceExceptionKind.None;
        }

        if (postgres.SqlState is PostgresErrorCodes.SerializationFailure or PostgresErrorCodes.DeadlockDetected)
        {
            return PersistenceExceptionKind.ConcurrencyConflict;
        }

        if (postgres.SqlState.StartsWith("23", StringComparison.Ordinal))
        {
            return PersistenceExceptionKind.DataConflict;
        }

        return PersistenceExceptionKind.None;
    }

    private static PostgresException? FindPostgresException(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException!)
        {
            if (current is PostgresException postgres)
            {
                return postgres;
            }
        }

        return null;
    }
}
