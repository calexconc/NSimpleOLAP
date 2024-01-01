using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSimpleOLAP.Common;
using NSimpleOLAP.CubeExpressions.Interfaces;
using NSimpleOLAP.Interfaces;

namespace NSimpleOLAP.CubeExpressions
{
  public abstract class ExpressionPartDefinition<T>
    where T : struct, IComparable
  {
    protected OperationType operation;

    protected T measure;

    protected List<KeyValuePair<T, T>[]> targetTuples;

    public abstract Func<IExpressionContext<T, ICell<T>>, object> Create();
  }
}
