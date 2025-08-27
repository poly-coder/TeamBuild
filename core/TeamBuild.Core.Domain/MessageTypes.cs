namespace TeamBuild.Core.Domain;

public interface IDomainMessage;

public interface IDomainRequest : IDomainMessage;

public interface IDomainResponse : IDomainMessage;

public interface IDomainFact : IDomainMessage;

public interface IDomainEvent : IDomainFact;

public interface IDomainCommand : IDomainRequest;

public interface IDomainQuery : IDomainRequest;

public interface IDomainCommandSuccess : IDomainResponse;

public interface IDomainQuerySuccess : IDomainResponse;
