using System.Linq.Expressions;
using System.Reflection;
using TeamBuild.Core.Reflection;

namespace TeamBuild.Core.UnitTests.Reflection;

[Trait("Category", "Unit")]
[Trait("Area", "Core")]
[Trait("Project", "Preamble")]
public class ReflectionExtensionsTests
{
    #region [ ExtractProperty ]

    [Theory]
    [ClassData(typeof(TryExtractPropertyTestData))]
    public void TryExtractPropertyTest(
        Expression expression,
        bool expected,
        PropertyInfo? expectedProperty
    )
    {
        var actualResult = expression.TryExtractProperty(out var actualProperty);

        expected.ShouldBe(actualResult);
        expectedProperty.ShouldBe(actualProperty);
    }

    [Theory]
    [ClassData(typeof(TryExtractPropertyTestData))]
    public void ExtractPropertyTest(
        Expression expression,
        bool expected,
        PropertyInfo? expectedProperty
    )
    {
        if (expected)
        {
            var actualProperty = expression.ExtractProperty();

            expectedProperty.ShouldBe(actualProperty);
        }
        else
        {
            Action action = () => expression.ExtractProperty();
            action.ShouldThrow<ArgumentException>();
        }
    }

    public class TryExtractPropertyTestData : TheoryData<Expression, bool, PropertyInfo?>
    {
        public TryExtractPropertyTestData()
        {
            // MemberExpression cases
            Add((string x) => x.Length, true, typeof(string).GetProperty(nameof(string.Length)));
            Add((DateTime x) => x.Year, true, typeof(DateTime).GetProperty(nameof(DateTime.Year)));

            // UnaryExpression cases
            Add((string x) => ~x.Length, true, typeof(string).GetProperty(nameof(string.Length)));
            Add((DateTime x) => ~x.Year, true, typeof(DateTime).GetProperty(nameof(DateTime.Year)));

            // Failed cases
            Add((string x) => x.Contains(""), false, null);
            Add(() => new String(Array.Empty<char>()), false, null);
        }
    }

    #endregion [ ExtractProperty ]

    #region [ ExtractMethod ]

    [Theory]
    [ClassData(typeof(TryExtractMethodTestData))]
    public void TryExtractMethodTest(
        Expression expression,
        bool expected,
        MethodInfo? expectedMethod
    )
    {
        var actualResult = expression.TryExtractMethod(out var actualMethod);

        expected.ShouldBe(actualResult);
        expectedMethod.ShouldBe(actualMethod);
    }

    [Theory]
    [ClassData(typeof(TryExtractMethodTestData))]
    public void ExtractMethodTest(Expression expression, bool expected, MethodInfo? expectedMethod)
    {
        if (expected)
        {
            var actualMethod = expression.ExtractMethod();

            expectedMethod.ShouldBe(actualMethod);
        }
        else
        {
            Action action = () => expression.ExtractMethod();
            action.ShouldThrow<ArgumentException>();
        }
    }

    public class TryExtractMethodTestData : TheoryData<Expression, bool, MethodInfo?>
    {
        public TryExtractMethodTestData()
        {
            // MemberExpression cases
            Add(
                (string x) => x.Contains(""),
                true,
                typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])
            );
            Add(
                (DateTime x) => x.AddDays(5),
                true,
                typeof(DateTime).GetMethod(nameof(DateTime.AddDays))
            );

            // UnaryExpression cases
            Add(
                (string x) => !x.Contains(""),
                true,
                typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])
            );
            Add(
                (DateTime x) => ~x.CompareTo(default(DateTime)),
                true,
                typeof(DateTime).GetMethod(nameof(DateTime.CompareTo), [typeof(DateTime)])
            );

            // Failed cases
            Add((string x) => x.Length, false, null);
            Add(() => new String(Array.Empty<char>()), false, null);
        }
    }

    #endregion [ ExtractMethod ]

    #region [ ExtractConstructor ]

    [Theory]
    [ClassData(typeof(TryExtractConstructorTestData))]
    public void TryExtractConstructorTest(
        Expression expression,
        bool expected,
        ConstructorInfo? expectedConstructor
    )
    {
        var actualResult = expression.TryExtractConstructor(out var actualConstructor);

        expected.ShouldBe(actualResult);
        expectedConstructor.ShouldBe(actualConstructor);
    }

    [Theory]
    [ClassData(typeof(TryExtractConstructorTestData))]
    public void ExtractConstructorTest(
        Expression expression,
        bool expected,
        ConstructorInfo? expectedConstructor
    )
    {
        if (expected)
        {
            var actualConstructor = expression.ExtractConstructor();

            expectedConstructor.ShouldBe(actualConstructor);
        }
        else
        {
            Action action = () => expression.ExtractConstructor();
            action.ShouldThrow<ArgumentException>();
        }
    }

    public class TryExtractConstructorTestData : TheoryData<Expression, bool, ConstructorInfo?>
    {
        public TryExtractConstructorTestData()
        {
            // MemberExpression cases
            Add(
                () => new String(Array.Empty<char>()),
                true,
                typeof(string).GetConstructor([typeof(char[])])
            );

            // Failed cases
            Add((string x) => x.Length, false, null);
            Add((string x) => x.Contains(""), false, null);
        }
    }

    #endregion [ ExtractConstructor ]

    #region [ CombineExpressions ]

    [Theory]
    [ClassData(typeof(CombineAndAlsoExpressionsTestData))]
    public void CombineAndAlsoExpressionsTest(LambdaExpression[] expressions, string expected)
    {
        var actual = expressions.Select(e => e.Body).CombineAndAlsoExpressions();

        actual.ToString().ShouldBe(expected);
    }

    public class CombineAndAlsoExpressionsTestData : TheoryData<LambdaExpression[], string>
    {
        public CombineAndAlsoExpressionsTestData()
        {
            Add([], "True");
            Add([(bool x) => x], "x");
            Add([(bool x) => x, (bool y) => y], "(x AndAlso y)");
            Add([(int x) => x > 10, (int x) => x < 20], "((x > 10) AndAlso (x < 20))");
        }
    }

    [Theory]
    [ClassData(typeof(CombineOrElseExpressionsTestData))]
    public void CombineOrElseExpressionsTest(LambdaExpression[] expressions, string expected)
    {
        var actual = expressions.Select(e => e.Body).CombineOrElseExpressions();

        actual.ToString().ShouldBe(expected);
    }

    public class CombineOrElseExpressionsTestData : TheoryData<LambdaExpression[], string>
    {
        public CombineOrElseExpressionsTestData()
        {
            Add([], "False");
            Add([(bool x) => x], "x");
            Add([(bool x) => x, (bool y) => y], "(x OrElse y)");
            Add([(int x) => x > 10, (int x) => x < 20], "((x > 10) OrElse (x < 20))");
        }
    }

    #endregion [ CombineExpressions ]
}
