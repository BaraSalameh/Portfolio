namespace Application.Common.Persistence;

public interface IPersistenceExceptionClassifier
{
    PersistenceExceptionKind Classify(Exception exception);
}

public enum PersistenceExceptionKind
{
    None,
    ConcurrencyConflict,
    DataConflict
}
