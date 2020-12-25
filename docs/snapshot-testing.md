# Snapshot testing

This document explains how to use create snapshots of the visual tree and how to export visual tree snapshots in human readable formats.

### What is snapshot testing?

Snapshot testing is a way of verifying that control and rendering output did not suddenly change. Snapshot tests compare the current rendering output of a control/visual tree against an already saved rendering output that has been saved earlier as correct rendering output. The [Snapshot namespace](xref:VisualTreeAnalyzers.Snapshot) contains classes that help in generating snapshots of a visual tree and also include helpers to export the snapshot in human readable formats.

### Creating a raw snapshot

Before taking a snapshot, you will first need to determine what properties should be included inside the snapshot. The [Snapshot namespace](xref:VisualTreeAnalyzers.Snapshot) also includes the [StandardOptions](xref:VisualTreeAnalyzers.Snapshot.StandardOptions) which provides a list of common properties to use for snapshot testing. The explicit list of properties can be seen in the [source code](https://github.com/chingucoding/VisualTreeAnalyzers/tree/main/src/VisualTreeAnalyzers/Snapshot/StandardOptions.cs).

To take a snapshot of an element and its visual tree with the standard options, you need to create a new [ElementSnapshotCreator](xref:VisualTreeAnalyzers.Snapshot.ElementSnapshotCreator) object. Then you can just call `CreateSnapshot`to generate the snapshot object for the visual tree:

```c#
var element = ...; // The element you want a snapshot of

// Create a new snapshot creator object
var snapshotCreator = new ElementSnapshotCreator(StandardOptions.StandardPropertyNames, element);

// Get snapshot
var elementSnapshot = snapshotCreator.CreateSnapshot();
```

To use an ElementSnapshotCreator on multiple elements, you can omit the second parameter and call `CreateSnapshot` with the element as parameter:

```c#
// Create a new snapshot creator object
var snapshotCreator = new ElementSnapshotCreator(StandardOptions.StandardPropertyNames);

// Get snapshots
var firstElementSnapshot = snapshotCreator.CreateSnapshot(firstElement);
var secondElementSnapshot = snapshotCreator.CreateSnapshot(secondElement);
```

If you wish to use a different set of properties to include in the visual tree snapshot, you can also pass your own list of properties to the snapshot creator.
Properties are being accessed using reflection and the passed names, so spelling of properties must equal the property on the controls.
Below is an example of how to use a custom set of properties with the snapshot creator:

```c#
// List of custom properties
var properties = new string[]
{
    "Background",
    "Foreground",
    "FontSize",
    "FontFamily",
    "Text"
};

var element = ...; // The element you want a snapshot of

// Create a new snapshot creator object
var snapshotCreator = new ElementSnapshotCreator(properties, name);

// Get snapshot
var elementSnapshot = snapshotCreator.CreateSnapshot();
```
