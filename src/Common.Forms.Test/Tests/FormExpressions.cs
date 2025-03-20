namespace RobinEpple.Common.Forms.Test.Tests;

using RobinEpple.Common.Forms.Expressions;
using static RobinEpple.Common.Forms.Expressions.FormExpression;

[TestClass]
public class FormExpressions
{
	# region static values

	[TestMethod]
	public void StaticValue_ShouldReturnConfiguredValue()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsTrue(StaticValue(true).EvaluateOn(form));
		Assert.IsFalse(StaticValue(false).EvaluateOn(form));
		Assert.Equals(null, StaticValue<bool?>(null).EvaluateOn(form));
		Assert.Equals("testText", StaticValue("testText").EvaluateOn(form));
	}

	[TestMethod]
	public void Coalesce_ShouldReturnExpressionValueIfNotNull()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsTrue(StaticValue<bool?>(true).Coalesce(false).EvaluateOn(form));
		Assert.IsFalse(StaticValue<bool?>(false).Coalesce(true).EvaluateOn(form));
		Assert.AreEqual(
			"testText",
			StaticValue<string?>("testText").Coalesce(StaticValue("fallbackText")).EvaluateOn(form)
		);
	}

	[TestMethod]
	public void Coalesce_ShouldReturnFallbackValueIfNull()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsFalse(StaticValue<bool?>(null).Coalesce(false).EvaluateOn(form));
		Assert.IsTrue(StaticValue<bool?>(null).Coalesce(true).EvaluateOn(form));
		Assert.AreEqual(
			"fallbackText",
			StaticValue<string?>(null).Coalesce(StaticValue("fallbackText")).EvaluateOn(form)
		);
	}

	[TestMethod]
	public void Conditional_ShouldReturnValueDependingOnConditionResult()
	{
		var form = new FormBuilder("Test").Build();

		Assert.AreEqual(
			"trueText",
			StaticValue(true).Conditional(StaticValue("trueText"), StaticValue("falseText")).EvaluateOn(form)
		);

		Assert.AreEqual(
			"trueText",
			StaticValue(false).Conditional(StaticValue("trueText"), StaticValue("falseText")).EvaluateOn(form)
		);
	}

	# endregion

	# region logical operators

	[TestMethod]
	public void And_ShouldBeTrueIfBothValuesTrue()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsTrue(StaticValue(true).And(StaticValue(true)).EvaluateOn(form));
	}

	[TestMethod]
	public void And_ShouldBeFalseIfAnyValueNotTrue()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsFalse(StaticValue(true).And(StaticValue(false)).EvaluateOn(form));
		Assert.IsFalse(StaticValue(false).And(StaticValue(true)).EvaluateOn(form));
		Assert.IsFalse(StaticValue(false).And(StaticValue(false)).EvaluateOn(form));
	}

	[TestMethod]
	public void All_ShouldBeTrueIfAllValuesTrue()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsTrue(
			new IFormExpression<bool>[] { StaticValue(true), StaticValue(true), StaticValue(true) }
				.All()
				.EvaluateOn(form)
		);
	}

	[TestMethod]
	public void All_ShouldBeFalseIfAnyValueNotTrue()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsFalse(
			new IFormExpression<bool>[] { StaticValue(false), StaticValue(true), StaticValue(true) }
				.All()
				.EvaluateOn(form)
		);

		Assert.IsFalse(
			new IFormExpression<bool>[] { StaticValue(true), StaticValue(false), StaticValue(true) }
				.All()
				.EvaluateOn(form)
		);

		Assert.IsFalse(
			new IFormExpression<bool>[] { StaticValue(true), StaticValue(true), StaticValue(false) }
				.All()
				.EvaluateOn(form)
		);

		Assert.IsFalse(
			new IFormExpression<bool>[] { StaticValue(false), StaticValue(false), StaticValue(false) }
				.All()
				.EvaluateOn(form)
		);
	}

	[TestMethod]
	public void Or_ShouldBeTrueIfAnyValueTrue()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsTrue(StaticValue(true).Or(StaticValue(true)).EvaluateOn(form));
		Assert.IsTrue(StaticValue(true).Or(StaticValue(false)).EvaluateOn(form));
		Assert.IsTrue(StaticValue(false).Or(StaticValue(true)).EvaluateOn(form));
	}

	[TestMethod]
	public void Or_ShouldBeFalseIfBothValueNotTrue()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsFalse(StaticValue(false).Or(StaticValue(false)).EvaluateOn(form));
	}

	[TestMethod]
	public void Any_ShouldBeTrueIfAnyValueTrue()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsTrue(
			new IFormExpression<bool>[] { StaticValue(true), StaticValue(true), StaticValue(true) }
				.Any()
				.EvaluateOn(form)
		);

		Assert.IsTrue(
			new IFormExpression<bool>[] { StaticValue(true), StaticValue(false), StaticValue(false) }
				.Any()
				.EvaluateOn(form)
		);

		Assert.IsTrue(
			new IFormExpression<bool>[] { StaticValue(false), StaticValue(true), StaticValue(false) }
				.Any()
				.EvaluateOn(form)
		);

		Assert.IsTrue(
			new IFormExpression<bool>[] { StaticValue(false), StaticValue(false), StaticValue(true) }
				.Any()
				.EvaluateOn(form)
		);
	}

	[TestMethod]
	public void Any_ShouldBeFalseIfAllValuesFalse()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsFalse(
			new IFormExpression<bool>[] { StaticValue(false), StaticValue(false), StaticValue(false) }
				.Any()
				.EvaluateOn(form)
		);
	}

	# endregion
}
