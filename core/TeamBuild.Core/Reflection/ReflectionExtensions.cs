using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace TeamBuild.Core.Reflection;

public static class ReflectionExtensions
{
    #region [ ExtractProperty ]

    public static bool TryExtractProperty(
        this Expression expression,
        [NotNullWhen(true)] out PropertyInfo? property
    )
    {
        ArgumentNullException.ThrowIfNull(expression);

        switch (expression)
        {
            case LambdaExpression expr:
                return expr.Body.TryExtractProperty(out property);

            case MemberExpression { Member: PropertyInfo propertyInfo }:
                property = propertyInfo;
                return true;

            case UnaryExpression
            {
                Operand: MemberExpression { Member: PropertyInfo propertyInfo }
            }:
                property = propertyInfo;
                return true;

            default:
                property = null;
                return false;
        }
    }

    public static PropertyInfo ExtractProperty(this Expression expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        if (expression.TryExtractProperty(out var property))
            return property;

        throw new ArgumentException("Expression does not represent a property.");
    }

    #endregion [ ExtractProperty ]

    #region [ ExtractMethod ]

    public static bool TryExtractMethod(
        this Expression expression,
        [NotNullWhen(true)] out MethodInfo? method
    )
    {
        ArgumentNullException.ThrowIfNull(expression);

        switch (expression)
        {
            case LambdaExpression expr:
                return expr.Body.TryExtractMethod(out method);

            case MethodCallExpression { Method: var methodInfo }:
                method = methodInfo;
                return true;

            case UnaryExpression { Operand: MethodCallExpression { Method: var methodInfo } }:
                method = methodInfo;
                return true;

            default:
                method = null;
                return false;
        }
    }

    public static MethodInfo ExtractMethod(this Expression expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        if (expression.TryExtractMethod(out var method))
            return method;

        throw new ArgumentException("Expression does not represent a method.");
    }

    #endregion [ ExtractMethod ]

    #region [ ExtractConstructor ]

    public static bool TryExtractConstructor(
        this Expression expression,
        [NotNullWhen(true)] out ConstructorInfo? constructor
    )
    {
        ArgumentNullException.ThrowIfNull(expression);

        switch (expression)
        {
            case LambdaExpression expr:
                return expr.Body.TryExtractConstructor(out constructor);

            case NewExpression { Constructor: { } constructorInfo }:
                constructor = constructorInfo;
                return true;

            default:
                constructor = null;
                return false;
        }
    }

    public static ConstructorInfo ExtractConstructor(this Expression expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        if (expression.TryExtractConstructor(out var constructor))
            return constructor;

        throw new ArgumentException("Expression does not represent a constructor.");
    }

    #endregion [ ExtractConstructor ]

    #region [ CombineExpressions ]

    public static Expression CombineLeftExpressions(
        this IEnumerable<Expression> source,
        Func<Expression, Expression, Expression> combineTwo,
        Func<Expression> getDefaultExpression
    )
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(combineTwo);
        ArgumentNullException.ThrowIfNull(getDefaultExpression);

        var combined = default(Expression?);

        foreach (var expression in source)
        {
            ArgumentNullException.ThrowIfNull(expression);

            combined = combined is null ? expression : combineTwo(combined, expression);
        }

        return combined ?? getDefaultExpression();
    }

    public static Expression CombineAndAlsoExpressions(this IEnumerable<Expression> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return source.CombineLeftExpressions(
            Expression.AndAlso,
            () => Expression.Constant(true, typeof(bool))
        );
    }

    public static Expression CombineOrElseExpressions(this IEnumerable<Expression> source)
    {
        ArgumentNullException.ThrowIfNull(source);
        return source.CombineLeftExpressions(
            Expression.OrElse,
            () => Expression.Constant(false, typeof(bool))
        );
    }

    #endregion [ CombineExpressions ]
}
