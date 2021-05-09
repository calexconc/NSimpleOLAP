/*
 * Created by SharpDevelop.
 * User: calex
 * Date: 23-02-2012
 * Time: 00:16
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Linq.Expressions;

namespace NSimpleOLAP.Configuration.Fluent
{
  /// <summary>
  /// Description of MeasureBuilder.
  /// </summary>
  public class MeasureBuilder
  {
    private MeasureConfig _element;

    public MeasureBuilder()
    {
      _element = new MeasureConfig();
    }

    #region public methods

    public MeasureBuilder SetName(string name)
    {
      _element.Name = name;
      return this;
    }

    public MeasureBuilder ValueField(string name)
    {
      _element.ValueFieldName = name;
      return this;
    }

    public MeasureBuilder ValueField(int index)
    {
      _element.ValueFieldIndex = index;
      return this;
    }

    public MeasureBuilder SetType(Type type)
    {
      _element.DataType = type;
      return this;
    }

    public MeasureBuilder SetMergeExpression(Expression expression)
    {
      _element.MergeFunction = expression;
      return this;
    }

    internal MeasureConfig Create()
    {
      return _element;
    }

    #region helper methods

    public static Expression DefaultMergeFunction<T>()
      where T : struct, IComparable
    {
      ParameterExpression val1Expr = Expression.Parameter(typeof(T), "oldvalue");
      ParameterExpression val2Expr = Expression.Parameter(typeof(T), "newvalue");
      BinaryExpression addExpr = BinaryExpression.Add(val1Expr, val2Expr);

      var lambdaExpr =
          Expression.Lambda<Func<T, T, T>>(
              addExpr, new ParameterExpression[] { val1Expr, val2Expr });

      return lambdaExpr;
    }

    #endregion helper methods

    #endregion public methods
  }
}