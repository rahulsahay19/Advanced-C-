using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace ExpressionTrees
{
    class Program
    {
        static void Main(string[] args)
        {
            var fooVar = new Foo();
        
            var list = new List<string>().AsQueryable().Where(s => s.Length > 0);
            var list1 = new List<string>().Where(s => s.Length > 0);
            var empId = 27;
            Expression<Func<Foo, string>> expr = c => c.ReturnSomething(empId, "Hello Expression");
            Expression<Func<Foo, bool>> expr1 = c => c.FooProp;

            var exprFunc = expr.Compile();
            var expr1Func = expr1.Compile();
            var result1 = exprFunc(fooVar);
            var result2 = expr1Func(fooVar);

            Console.WriteLine($"Compiled Expressions:- {exprFunc}, {expr1Func}");
            Console.WriteLine($"Results:- {result1}, {result2}");
            Console.ReadLine();
        }

        private static void ExpressionParser(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Lambda)
            {
                var lambdaExpr = (LambdaExpression) expression;
                ExpressionParser(lambdaExpr.Body);
            }
            else if (expression.NodeType == ExpressionType.Call)
            {
                var methodExpr = (MethodCallExpression) expression;
                Console.WriteLine($"Method Name: {methodExpr.Method.Name}, Return Type: {methodExpr.Method.ReturnType}");
                for (int i = 0; i < methodExpr.Arguments.Count; i++)
                {
                    // Parsing Arguments. Basically parsing constants
                    ExpressionParser(methodExpr.Arguments[i]);
                }
            } 
            else if (expression.NodeType == ExpressionType.MemberAccess)
            {
                var memberExpr = (MemberExpression) expression;
                Console.WriteLine($"Member Name: {memberExpr.Member.Name}, Member Type: {memberExpr.Member.MemberType}");
            }
            else if (expression.NodeType == ExpressionType.Constant)
            {
                var constantExpression = (ConstantExpression) expression;
                Console.WriteLine($"Constant expression value:- {constantExpression.Value}");
            }
        }

        private static void Bar(Func<Foo, string> func)
        {
            Console.WriteLine($"Called foo from bar:{func}");
            Console.ReadLine();
        }
    }
}
