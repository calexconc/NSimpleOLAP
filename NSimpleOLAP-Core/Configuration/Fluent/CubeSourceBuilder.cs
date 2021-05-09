namespace NSimpleOLAP.Configuration.Fluent
{
  /// <summary>
  /// Description of CubeSourceBuilder.
  /// </summary>
  public class CubeSourceBuilder
  {
    private CubeSourceConfig _element;

    public CubeSourceBuilder()
    {
      _element = new CubeSourceConfig();
    }

    #region public methods

    public CubeSourceBuilder SetSource(string source)
    {
      _element.Name = source;
      return this;
    }

    public CubeSourceBuilder AddMapping(string fieldname, string dimensionname)
    {
      _element.Fields.Add(new SourceMappingsElement() { Field = fieldname, Dimension = dimensionname });
      return this;
    }

    public CubeSourceBuilder AddMapping(string fieldname, params string[] dimensionLabels)
    {
      _element.Fields.Add(new SourceMappingsElement() { Field = fieldname, Labels = dimensionLabels });
      return this;
    }

    internal CubeSourceConfig Create()
    {
      return _element;
    }

    #endregion public methods
  }
}