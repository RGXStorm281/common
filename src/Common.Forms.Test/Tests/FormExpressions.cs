using RobinEpple.Common.Forms.Expressions;

namespace RobinEpple.Common.Forms.Test.Tests;

[TestClass]
public class FormExpressions
{
	# region static values

	[TestMethod]
	public void StaticValue_ShouldReturnConfiguredValue()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsTrue(FormExpression.StaticValue(true).EvaluateOn(form));
		Assert.IsFalse(FormExpression.StaticValue(false).EvaluateOn(form));
		Assert.Equals(null, FormExpression.StaticValue<bool?>(null).EvaluateOn(form));
		Assert.Equals("testText", FormExpression.StaticValue("testText").EvaluateOn(form));
	}

	[TestMethod]
	public void Coalesce_ShouldReturnExpressionValueIfNotNull()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsTrue(FormExpression.StaticValue<bool?>(true).Coalesce(false).EvaluateOn(form));
		Assert.IsFalse(FormExpression.StaticValue<bool?>(false).Coalesce(true).EvaluateOn(form));
		Assert.AreEqual(
			"testText",
			FormExpression
				.StaticValue<string?>("testText")
				.Coalesce(FormExpression.StaticValue("fallbackText"))
				.EvaluateOn(form)
		);
	}

	[TestMethod]
	public void Coalesce_ShouldReturnFallbackValueIfNull()
	{
		var form = new FormBuilder("Test").Build();

		Assert.IsFalse(FormExpression.StaticValue<bool?>(null).Coalesce(false).EvaluateOn(form));
		Assert.IsTrue(FormExpression.StaticValue<bool?>(null).Coalesce(true).EvaluateOn(form));
		Assert.AreEqual(
			"fallbackText",
			FormExpression
				.StaticValue<string?>(null)
				.Coalesce(FormExpression.StaticValue("fallbackText"))
				.EvaluateOn(form)
		);
	}

	[TestMethod]
	public void Conditional_ShouldReturnValueDependingOnConditionResult()
	{
		var form = new FormBuilder("Test").Build();

		Assert.AreEqual(
			"trueText",
			FormExpression
				.Conditional(
					FormExpression.StaticValue(true),
					FormExpression.StaticValue("trueText"),
					FormExpression.StaticValue("falseText")
				)
				.EvaluateOn(form)
		);

		Assert.AreEqual(
			"trueText",
			FormExpression
				.Conditional(
					FormExpression.StaticValue(false),
					FormExpression.StaticValue("trueText"),
					FormExpression.StaticValue("falseText")
				)
				.EvaluateOn(form)
		);
	}

	# endregion
}
