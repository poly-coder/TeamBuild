//using System.Net;

//namespace TeamBuild.Core.Domain;

////public interface IDomainError
////{
////    string? Type { get; }

////    string? Title { get; }

////    HttpStatusCode? Status { get; }

////    string? Detail { get; }

////    string? Instance { get; }

////    IReadOnlyDictionary<string, object?>? Extensions { get; }

////    IReadOnlyDictionary<string, IReadOnlyCollection<string>>? Errors { get; }
////}

//public record DomainError(
//    string? Type = null,
//    string? Title = null,
//    HttpStatusCode? Status = null,
//    string? Detail = null,
//    string? Instance = null,
//    IReadOnlyDictionary<string, object?>? Extensions = null,
//    IReadOnlyDictionary<string, IReadOnlyCollection<string>>? Errors = null
//) // : IDomainError
//{
//    public const string EntityTypeExtensionKey = "entityType";
//    public const string EntityIdExtensionKey = "entityId";

//    #region [ BadRequest ]

//    public static DomainError BadRequest(
//        IReadOnlyDictionary<string, IReadOnlyCollection<string>>? errors = null
//    )
//    {
//        return new(
//            Type: "https://tools.ietf.org/html/rfc7231#section-6.5.1",
//            Title: "Bad Request",
//            Detail: "The request is invalid",
//            Status: HttpStatusCode.BadRequest,
//            Errors: errors
//        );
//    }

//    public static DomainError BadRequest(string property, string errorMessage)
//    {
//        ArgumentException.ThrowIfNullOrWhiteSpace(property);
//        ArgumentException.ThrowIfNullOrWhiteSpace(errorMessage);

//        return BadRequest(CreateErrors(property, errorMessage));
//    }

//    public static DomainError BadRequest(IEnumerable<(string property, string errorMessage)> errors)
//    {
//        ArgumentNullException.ThrowIfNull(errors);

//        return BadRequest(CreateErrors(errors));
//    }

//    #endregion [ BadRequest ]

//    #region [ NotFound ]

//    public static DomainError NotFound(string entityType, string entityId)
//    {
//        ArgumentException.ThrowIfNullOrWhiteSpace(entityType);
//        ArgumentException.ThrowIfNullOrWhiteSpace(entityId);

//        return new(
//            Type: "https://tools.ietf.org/html/rfc7231#section-6.5.4",
//            Title: "Not Found",
//            Detail: $"NotFound: {entityType} with ID '{entityId}' was not found",
//            Status: HttpStatusCode.NotFound,
//            Extensions: new Dictionary<string, object?>()
//            {
//                [EntityTypeExtensionKey] = entityType,
//                [EntityIdExtensionKey] = entityId,
//            }
//        );
//    }

//    #endregion [ NotFound ]

//    #region [ Conflict ]

//    public static DomainError Conflict(
//        string entityType,
//        string entityId,
//        IReadOnlyDictionary<string, IReadOnlyCollection<string>>? errors = null
//    )
//    {
//        ArgumentException.ThrowIfNullOrWhiteSpace(entityType);
//        ArgumentException.ThrowIfNullOrWhiteSpace(entityId);

//        return new(
//            Type: "https://tools.ietf.org/html/rfc7231#section-6.5.8",
//            Title: "Conflict",
//            Detail: $"Conflict: {entityType} with ID '{entityId}' has conflicts",
//            Status: HttpStatusCode.Conflict,
//            Extensions: new Dictionary<string, object?>()
//            {
//                [EntityTypeExtensionKey] = entityType,
//                [EntityIdExtensionKey] = entityId,
//            },
//            Errors: errors
//        );
//    }

//    public static DomainError Conflict(
//        string entityType,
//        string entityId,
//        string property,
//        string errorMessage
//    )
//    {
//        ArgumentException.ThrowIfNullOrWhiteSpace(entityType);
//        ArgumentException.ThrowIfNullOrWhiteSpace(entityId);
//        ArgumentException.ThrowIfNullOrWhiteSpace(property);
//        ArgumentException.ThrowIfNullOrWhiteSpace(errorMessage);

//        return Conflict(entityType, entityId, CreateErrors(property, errorMessage));
//    }

//    public static DomainError Conflict(
//        string entityType,
//        string entityId,
//        IEnumerable<(string property, string errorMessage)> errors
//    )
//    {
//        ArgumentException.ThrowIfNullOrWhiteSpace(entityType);
//        ArgumentException.ThrowIfNullOrWhiteSpace(entityId);
//        ArgumentNullException.ThrowIfNull(errors);

//        return Conflict(entityType, entityId, CreateErrors(errors));
//    }

//    #endregion [ Conflict ]

//    #region [ InternalError ]

//    public static DomainError InternalError()
//    {
//        return new(
//            Type: "https://tools.ietf.org/html/rfc7231#section-6.6.1",
//            Title: "Internal Error",
//            Detail: "An unexpected error occurred.",
//            Status: HttpStatusCode.InternalServerError
//        );
//    }

//    #endregion [ InternalError ]

//    #region [ NotImplemented ]

//    public static DomainError NotImplemented()
//    {
//        return new(
//            Type: "https://tools.ietf.org/html/rfc7231#section-6.6.2",
//            Title: "Not Implemented",
//            Detail: "The requested functionality is not implemented yet.",
//            Status: HttpStatusCode.NotImplemented
//        );
//    }

//    #endregion [ NotImplemented ]

//    #region [ ServiceUnavailable ]

//    public static DomainError ServiceUnavailable()
//    {
//        return new(
//            Type: "https://tools.ietf.org/html/rfc7231#section-6.6.4",
//            Title: "Service Unavailable",
//            Detail: "The service is temporarily unavailable.",
//            Status: HttpStatusCode.ServiceUnavailable
//        );
//    }

//    #endregion [ ServiceUnavailable ]

//    private static IReadOnlyDictionary<string, IReadOnlyCollection<string>> CreateErrors(
//        string property,
//        string errorMessage
//    )
//    {
//        return new Dictionary<string, IReadOnlyCollection<string>>
//        {
//            [property] = [errorMessage],
//        }.AsReadOnly();
//    }

//    private static IReadOnlyDictionary<string, IReadOnlyCollection<string>> CreateErrors(
//        IEnumerable<(string property, string errorMessage)> errors
//    )
//    {
//        return errors
//            .GroupBy(p => p.property)
//            .ToDictionary(
//                g => g.Key,
//                IReadOnlyCollection<string> (g) =>
//                    g.Select(p => p.errorMessage).ToList().AsReadOnly()
//            )
//            .AsReadOnly();
//    }
//}
