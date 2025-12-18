namespace RobinEpple.Common.SourceGenerators.Test;

using RobinEpple.Common.SourceGenerators.Abstractions;

[StaticFactory(typeof(IStaticFactoryMarker))]
[StaticFactory(typeof(IStaticFactoryGenericMarker<>))]
public static partial class StaticFactory { }
