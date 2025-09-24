using System;

[AttributeUsage(AttributeTargets.Class)]
public sealed class JsonMappingAttribute : Attribute
{
    public string FileName { get; }
    public JsonMappingAttribute(string fileName) => FileName = fileName;
}
