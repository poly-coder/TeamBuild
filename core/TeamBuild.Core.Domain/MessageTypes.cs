namespace TeamBuild.Core.Domain;

public interface IDomainMessage;

public interface IDomainRequest : IDomainMessage;

public interface IDomainFact : IDomainMessage;

public interface IDomainEvent : IDomainFact;

public interface IIntegrationEvent : IDomainFact;

public interface IDomainCommand : IDomainRequest;

public interface IDomainQuery : IDomainRequest;
