# RobinEpple.Common.Util

([back to readme](../readme.md))

This project is meant to be a collection of standalone, low dependency tools.
Currently it contains three main tools.

## Extension methods

Spread across a couple of static classes, this NuGet package offers a variety of useful extension methods. Some of them are just a proxy to call .NETs static methods in form of a null-safe extension. But I also added some custom ones with more logic. Have a look at the examples below.

```C#
// StringExtensions.
var helloTemplate = "Hello {0}!";
if(!helloTemplate.IsNullOrEmpty())
{
    var formatted = templateString.Format("World").Truncate(10);
    // Will output "Hello Worl".
}

// MemoryExtensions.
var fileBytes = File.ReadAllBytes("/path/to/file");
var memoryStream = fileBytes.GetMemoryStream();

// CollectionExtensions.
var pets = new[] { "cat", "bunny" };
var isMissingDog = pets.None(pet => pet == "dog");
var petsDict = pets.ToDictionary(pet => pet);
petsDict.GetOrAdd("dog", () => "dog", out var created);
```

## Comparison class

The static `Comparison` class offers a range of methods that compare two arbitrary sets of objects using a key definition. The full comparison `Comparison.CompareByUniqueKeyEquality(left, right, ...)` sorts all items into three buckets:

- Items in the _left_ set without a match in the _right_ set are sorted into `LeftDifference`.
- Items with a matching key in the other set are grouped together into `Intersection`.
- Items in the _right_ set without a match in the _left_ set are sorted into `RightDifference`.

Alternatively, there are functions to compute only one of these three sets, and all methods have an overload that can handle ambiguous keys. The latter ones will group all items with identical keys into lists.

This functionality is especially useful for synchronization, for example when a user alters a list in the UI and it needs to be synced back to the database.

```C#
var userInput = new List<UiModel>
{
    new UiModel(id: 1),
    new UiModel(id: 4),
    new UiModel(id: 2)
};
var existingInDb = new List<DbRecord>
{
    new DbRecord(id: 2),
    new DbRecord(id: 3),
    new DbRecord(id: 1)
}

// Compare them by integer Ids.
var result = Comparison.CompareByUniqueKeyEquality(
    left: userInput,
    right: existingInDb,
    input => input.Id,
    existing => existing.Id
);

// Then use the three categories to sync to the Db.
var transaction = _db.BeginTransaction();

// RightDifference: [3]
foreach(var obsolete in result.RightDifference)
{
    // First cleanup / "make space".
    _db.Delete(obsolete);
}

// Intersection: [(1,1), (2,2)]
foreach(var grouping in result.Intersection)
{
    // Do some proper update here.
    grouping.Right.Id = grouping.Left.Id;
    _db.Update(grouping.Right);
}

// LeftDifference: [4]
foreach(var missing in result.LeftDifference)
{
    // Finally insert missing ones in the database.
    var newRecord = new DbRecord(id: missing.Id);
    _db.Insert(newRecord);
}
_transaction.Commit();
```

## Conversion helper

I have stumbled across the problem a couple of times, that I need to do a generic type conversion. Unfortunately, `Convert.ChangeType` will fail when the source type is wrapped by `Nullable<>`.

The `NullableUnwrappingTypeConverter` handles this by checking the source value for null, and unwrapping nullable types before calling `Convert.ChangeType`.

```C#
int? myInt = 2;

// This will fail.
var hardConverted = Convert.ChangeType(myInt, typeof(double));

// This will succeed.
if (!NullableUnwrappingTypeConverter.TryConvert<double>(myInt, out var softConverted))
{
    throw new InvalidOperationException("Integer has to have a value for the conversion.");
}
```
