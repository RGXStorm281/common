namespace RobinEpple.Common.Forms.Wrappers;

using System.Collections.Generic;
using Microsoft.CodeAnalysis;

public static class AttributeCollector
{
	/// <summary>
	/// Collects all attributes across
	/// - This method
	/// - All overridden base methods
	/// - All implemented interface methods
	/// </summary>
	/// <param name="method">The method to collect attributes for.</param>
	/// <returns>The list of attributes.</returns>
	public static IReadOnlyList<AttributeData> CollectAllMethodAttributes(IMethodSymbol method)
	{
		var attributes = new List<AttributeData>();
		var visitedMethods = new HashSet<IMethodSymbol>(SymbolEqualityComparer.Default);

		void Collect(IMethodSymbol method)
		{
			// Recursion end and guard double visit.
			if (method == null || !visitedMethods.Add(method))
			{
				return;
			}

			// Add this method's attributes
			attributes.AddRange(method.GetAttributes());

			// Walk override chain
			if (method.OverriddenMethod != null)
			{
				Collect(method.OverriddenMethod);
			}

			// Walk interface implementations
			CollectInterfaceMethods(method);
		}

		void CollectInterfaceMethods(IMethodSymbol method)
		{
			var containingType = method.ContainingType;
			if (containingType == null)
			{
				return;
			}

			// Handle both implicit & explicit implementations
			foreach (var implementedInterface in containingType.AllInterfaces)
			{
				foreach (var interfaceMethod in implementedInterface.GetMembers().OfType<IMethodSymbol>())
				{
					var interfaceImplementation =
						containingType.FindImplementationForInterfaceMember(interfaceMethod) as IMethodSymbol;
					if (SymbolEqualityComparer.Default.Equals(interfaceImplementation, method))
					{
						Collect(interfaceMethod);
					}
				}
			}
		}

		Collect(method);
		return attributes;
	}

	/// <summary>
	/// Collects all attributes for a specific parameter index across:
	/// - This method
	/// - All overridden base methods
	/// - All implemented interface methods
	/// </summary>
	/// <param name="method">The method to collect attributes from.</param>
	/// <param name="parameterIndex">The index of the parameter the attributes are collected for.</param>
	/// <returns>The list of attributes.</returns>
	public static IReadOnlyList<AttributeData> CollectAllParameterAttributes(IMethodSymbol method, int parameterIndex)
	{
		var results = new List<AttributeData>();
		var visited = new HashSet<IMethodSymbol>(SymbolEqualityComparer.Default);

		void Collect(IMethodSymbol method)
		{
			// Recursion end and guard double visit.
			if (method == null || !visited.Add(method))
			{
				return;
			}

			if (parameterIndex >= 0 && parameterIndex < method.Parameters.Length)
			{
				results.AddRange(method.Parameters[parameterIndex].GetAttributes());
			}

			// Walk override chain
			if (method.OverriddenMethod != null)
			{
				Collect(method.OverriddenMethod);
			}

			// Walk interface implementations
			CollectInterfaceMethods(method);
		}

		void CollectInterfaceMethods(IMethodSymbol method)
		{
			var containingType = method.ContainingType;
			if (containingType == null)
			{
				return;
			}

			foreach (var implementedInterface in containingType.AllInterfaces)
			{
				foreach (var interfaceMethod in implementedInterface.GetMembers().OfType<IMethodSymbol>())
				{
					var interfaceImplementation =
						containingType.FindImplementationForInterfaceMember(interfaceMethod) as IMethodSymbol;
					if (SymbolEqualityComparer.Default.Equals(interfaceImplementation, method))
					{
						Collect(interfaceMethod);
					}
				}
			}
		}

		Collect(method);
		return results;
	}

	/// <summary>
	/// Collects parameter attributes for all parameters across:
	/// - This method
	/// - All overridden base methods
	/// - All implemented interface methods
	/// </summary>
	/// <returns>
	/// A dictionary where:
	///   Key   = parameter index
	///   Value = all AttributeData found for that parameter across the hierarchy
	/// </returns>
	public static IReadOnlyDictionary<int, IReadOnlyList<AttributeData>> CollectAllParameterAttributes(
		IMethodSymbol method
	)
	{
		var results = new Dictionary<int, List<AttributeData>>();
		var visited = new HashSet<IMethodSymbol>(SymbolEqualityComparer.Default);

		void Collect(IMethodSymbol method)
		{
			if (method == null || !visited.Add(method))
			{
				return;
			}

			// Collect this method's parameter attributes
			for (int parameterIndex = 0; parameterIndex < method.Parameters.Length; parameterIndex++)
			{
				if (!results.TryGetValue(parameterIndex, out var parameterAttributeList))
				{
					parameterAttributeList = new List<AttributeData>();
					results[parameterIndex] = parameterAttributeList;
				}

				parameterAttributeList.AddRange(method.Parameters[parameterIndex].GetAttributes());
			}

			// Walk override chain
			if (method.OverriddenMethod != null)
			{
				Collect(method.OverriddenMethod);
			}

			// Walk interface implementations
			CollectInterfaceMethods(method);
		}

		void CollectInterfaceMethods(IMethodSymbol method)
		{
			var containingType = method.ContainingType;
			if (containingType == null)
			{
				return;
			}

			foreach (var implementedInterface in containingType.AllInterfaces)
			{
				foreach (var interfaceMethod in implementedInterface.GetMembers().OfType<IMethodSymbol>())
				{
					var interfaceImplementation =
						containingType.FindImplementationForInterfaceMember(interfaceMethod) as IMethodSymbol;
					if (SymbolEqualityComparer.Default.Equals(interfaceImplementation, method))
					{
						Collect(interfaceMethod);
					}
				}
			}
		}

		Collect(method);

		// Freeze into read-only shape
		return results.ToDictionary(kvp => kvp.Key, kvp => (IReadOnlyList<AttributeData>)kvp.Value);
	}
}
