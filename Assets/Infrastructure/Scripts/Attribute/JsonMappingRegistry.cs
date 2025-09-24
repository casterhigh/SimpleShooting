using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class JsonMappingRegistry
{
    public static Dictionary<string, Type> BuildMapping()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => SafeGetTypes(a))
            .Where(t => t.GetCustomAttribute<JsonMappingAttribute>() != null)
            .ToDictionary(
                t => t.GetCustomAttribute<JsonMappingAttribute>().FileName,
                t => t
            );
    }

    private static IEnumerable<Type> SafeGetTypes(Assembly asm)
    {
        try { return asm.GetTypes(); }
        catch { return Array.Empty<Type>(); }
    }
}
