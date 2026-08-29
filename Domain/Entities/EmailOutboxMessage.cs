using Domain.Enums;

namespace Domain.Entities;

public sealed class EmailOutboxMessage
{
    public Guid ID { get; set; }
    public EmailOutboxKind Kind { get; set; }
    public Guid AggregateID { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime NextAttemptAt { get; set; }
    public int AttemptCount { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public Guid? LockID { get; set; }
    public DateTime? LockedUntil { get; set; }
    public string? LastError { get; set; }
}
