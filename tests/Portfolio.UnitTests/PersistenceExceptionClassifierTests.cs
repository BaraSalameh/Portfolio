using Application.Common.Persistence;
using DataAccess.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Portfolio.UnitTests;

public sealed class PersistenceExceptionClassifierTests
{
    private readonly PersistenceExceptionClassifier _classifier = new();

    [Fact]
    public void OptimisticConcurrencyIsClassifiedAsConcurrencyConflict() =>
        Assert.Equal(
            PersistenceExceptionKind.ConcurrencyConflict,
            _classifier.Classify(new DbUpdateConcurrencyException()));

    [Fact]
    public void EntityFrameworkUpdateFailureWithoutConstraintEvidenceIsOperationalFailure() =>
        Assert.Equal(
            PersistenceExceptionKind.None,
            _classifier.Classify(new DbUpdateException()));

    [Fact]
    public void PostgreSqlIntegrityConstraintIsClassifiedAsDataConflict() =>
        Assert.Equal(
            PersistenceExceptionKind.DataConflict,
            _classifier.Classify(new PostgresException(
                "constraint violation",
                "ERROR",
                "ERROR",
                PostgresErrorCodes.UniqueViolation)));

    [Fact]
    public void WrappedPostgreSqlIntegrityConstraintIsClassifiedAsDataConflict() =>
        Assert.Equal(
            PersistenceExceptionKind.DataConflict,
            _classifier.Classify(new DbUpdateException(
                "update failed",
                PostgreSql(PostgresErrorCodes.ForeignKeyViolation))));

    [Theory]
    [InlineData(PostgresErrorCodes.SerializationFailure)]
    [InlineData(PostgresErrorCodes.DeadlockDetected)]
    public void PostgreSqlTransactionCollisionIsClassifiedAsConcurrencyConflict(string sqlState) =>
        Assert.Equal(
            PersistenceExceptionKind.ConcurrencyConflict,
            _classifier.Classify(new DbUpdateException("update failed", PostgreSql(sqlState))));

    [Fact]
    public void WrappedConnectionFailureIsNotMisclassifiedAsClientConflict() =>
        Assert.Equal(
            PersistenceExceptionKind.None,
            _classifier.Classify(new DbUpdateException(
                "update failed",
                new InvalidOperationException("database unavailable"))));

    [Fact]
    public void UnrelatedFailureIsNotMisclassified() =>
        Assert.Equal(
            PersistenceExceptionKind.None,
            _classifier.Classify(new InvalidOperationException()));

    private static PostgresException PostgreSql(string sqlState) => new(
        "database error",
        "ERROR",
        "ERROR",
        sqlState);
}
