# Snapshot testing

This document explains how to use create snapshots of the visual tree and how to export visual tree snapshots in human readable formats.

## What is snapshot testing?

Snapshot testing is a way of verifying that control and rendering output did not suddenly change. Snapshot tests compare the current rendering output of a control/visual tree against an already saved rendering output that has been saved earlier as correct rendering output. The [Snapshot namespace](xref:VisualTreeAnalyzers.Snapshot) contains classes that help in generating snapshots of a visual tree and also include helpers to export the snapshot in human readable formats. Note that currently, only UWP with OS - XAML is supported.

## Creating a raw snapshot

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

## Exporting to other formats
Snapshots can also be exported into others formats such as XML or YML using the exporters included in the [exporter namespace](xref:VisualTreeAnalyzers.Snapshot.Exporter).
Below are getting started guides for the different exporters.

### Exporting to XML
You can export snapshots to XML using the [XmlExporter](xref:VisualTreeAnalyzers.Snapshot.Exporter.XmlExporter).
The XmlExporter supports exporting both as [XmlDocument](https://docs.microsoft.com/uwp/api/windows.data.xml.dom.xmldocument) and as formatted string.
In both exporting formats, namespaces can be omitted, null valued properties and empty values can be excluded.

Below are examples on how to use the `XmExporter`:


```c#
var element = new TextBlock()
{
    Text="Demo text",
    Foreground = new SolidColorBrush(Colors.DarkGreen)
};
var snapshot = new ElementSnapshotCreator(StandardOptions.StandardPropertyNames, element).CreateSnapshot();
var xmlExporter = new XmlExporter();

var xmlDocument = xmlExporter.CreateXMLDocument(snapshot, true, true); // Creates a new XmlDocument representation including null values and namespaces

var stringWithNullValuesAndNamespaces = xmlExporter.CreateFormattedXMLString(snapshot, true, true);
// stringWithNullValuesAndNamespaces: <Windows.UI.Xaml.Controls.TextBlock Name="" Visibility="Visible" Margin="0,0,0,0" Padding="0,0,0,0" Background="[null]" BorderBrush="[null]" BorderThickness="[null]" Foreground="#FF006400" />

var stringWithNonNullValuesAndTypename = xmlExporter.CreateFormattedXMLString(snapshot, false, false);
// stringWithNonNullValuesAndTypename: <TextBlock Name="" Visibility="Visible" Margin="0,0,0,0" Padding="0,0,0,0" Foreground="#FF006400" />
```

Depending on your use case, it can be beneficial to exclude null values or the namespaces. As long as there are no type name collisions and there is no doubt what control names are referring to, namespaces can be excluded from the snapshot. The formatted string also indents child elements, e.g. the following XAML will generate the output below:

```xml
<StackPanel BorderBrush="DarkGreen" BorderThickness="1">
    <Button x:Name="MyButton1" Background="LightGray">
        <TextBlock Text="Click me" Foreground="Black"/>
    </Button>
    <TextBlock x:Name="ExampleTextBlock" Text="Some text" Foreground="DarkGreen"/>
</StackPanel>
```

Formatted string output with namespaces and null values:
```xml
<Windows.UI.Xaml.Controls.StackPanel Name="" Visibility="Visible" Margin="0,0,0,0" Padding="0,0,0,0" Background="[null]" BorderBrush="#FF006400" BorderThickness="1,1,1,1" Foreground="[null]">
    <Windows.UI.Xaml.Controls.Button Name="MyButton1" Visibility="Visible" Margin="0,0,0,0" Padding="8,4,8,5" Background="#FFD3D3D3" BorderBrush="#00FFFFFF" BorderThickness="2,2,2,2" Foreground="#FFFFFFFF">
    <Windows.UI.Xaml.Controls.ContentPresenter Name="ContentPresenter" Visibility="Visible" Margin="0,0,0,0" Padding="8,4,8,5" Background="#FFD3D3D3" BorderBrush="#00FFFFFF" BorderThickness="2,2,2,2" Foreground="#FFFFFFFF">
        <Windows.UI.Xaml.Controls.TextBlock Name="" Visibility="Visible" Margin="0,0,0,0" Padding="0,0,0,0" Background="[null]" BorderBrush="[null]" BorderThickness="[null]" Foreground="#FF000000" />
    </Windows.UI.Xaml.Controls.ContentPresenter>
    </Windows.UI.Xaml.Controls.Button>
    <Windows.UI.Xaml.Controls.TextBlock Name="ExampleTextBlock" Visibility="Visible" Margin="0,0,0,0" Padding="0,0,0,0" Background="[null]" BorderBrush="[null]" BorderThickness="[null]" Foreground="#FF006400" />
</Windows.UI.Xaml.Controls.StackPanel>
```

Formatted string output without namespaces and excluding null values and empty strings:
```xml
<StackPanel Visibility="Visible" Margin="0,0,0,0" Padding="0,0,0,0" BorderBrush="#FF006400" BorderThickness="1,1,1,1">
    <Button Name="MyButton1" Visibility="Visible" Margin="0,0,0,0" Padding="8,4,8,5" Background="#FFD3D3D3" BorderBrush="#00FFFFFF" BorderThickness="2,2,2,2" Foreground="#FFFFFFFF">
    <ContentPresenter Name="ContentPresenter" Visibility="Visible" Margin="0,0,0,0" Padding="8,4,8,5" Background="#FFD3D3D3" BorderBrush="#00FFFFFF" BorderThickness="2,2,2,2" Foreground="#FFFFFFFF">
        <TextBlock Visibility="Visible" Margin="0,0,0,0" Padding="0,0,0,0" Foreground="#FF000000" />
    </ContentPresenter>
    </Button>
    <TextBlock Name="ExampleTextBlock" Visibility="Visible" Margin="0,0,0,0" Padding="0,0,0,0" Foreground="#FF006400" />
</StackPanel>
```

The XmlExporter and other exporters allow the usage of [IObjectToStringConverter](xref:VisualTreeAnalyzers.Snapshot.Exporter.IObjectToStringConverter) objects to provide better string representation of property values.
Unless specified otherwise, the [StandardObjectToStringConverter](xref:VisualTreeAnalyzers.Snapshot.Exporter.StandardObjectToStringConverter) will be used to provide better representations.

### Exporting to JSON
You can also export snapshots to JSON using the [JsonExporter](xref:VisualTreeAnalyzers.Snapshot.Exporter.JsonExporter).
The JsonExporter supports exporting both as [JsonDocument](https://docs.microsoft.com/uwp/api/windows.data.json.jsonobject) and as formatted string.
In both exporting formats, namespaces can be omitted, null valued properties and empty values can be excluded.

Below are examples on how to use the JsonExporter:


```c#
var element = new TextBlock()
{
    Text = "Demo text",
    Foreground = new SolidColorBrush(Colors.DarkGreen)
};
var snapshot = new ElementSnapshotCreator(StandardOptions.StandardPropertyNames, element).CreateSnapshot();
var jsonExporter = new JsonExporter();

var jsonObject = jsonExporter.CreateJsonObject(snapshot, true, true); // Creates a new JsonObject representation including null values and namespaces

var stringWithNullValuesAndNamespaces = jsonExporter.CreateFormattedJSONString(snapshot, true, true);
// stringWithNullValuesAndNamespaces:
// {
//     "type": "Windows.UI.Xaml.Controls.TextBlock",
//     "Name": "",
//     "Visibility": "Visible",
//     "Margin": "0,0,0,0",
//     "Padding": "0,0,0,0",
//     "Background": "[null]",
//     "BorderBrush": "[null]",
//     "BorderThickness": "[null]",
//     "Foreground": "#FF006400",
//     "children": []
// }

var stringWithNonNullValuesAndTypename = jsonExporter.CreateFormattedJSONString(snapshot, false, false);
// stringWithNonNullValuesAndTypename:
// {
//     "type": "TextBlock",
//     "Visibility": "Visible",
//     "Margin": "0,0,0,0",
//     "Padding": "0,0,0,0",
//     "Foreground": "#FF006400",
//     "children": []
// }
```

Depending on your use case, it can be beneficial to exclude null values or the namespaces. As long as there are no type name collisions and there is no doubt what control names are referring to, namespaces can be excluded from the snapshot. The formatted string also indents child elements, e.g. the following XAML will generate the output below:

```xml
<StackPanel BorderBrush="DarkGreen" BorderThickness="1">
    <Button x:Name="MyButton1" Background="LightGray">
        <TextBlock Text="Click me" Foreground="Black"/>
    </Button>
    <TextBlock x:Name="ExampleTextBlock" Text="Some text" Foreground="DarkGreen"/>
</StackPanel>
```

Formatted string output with namespaces and null values:
```json
{
    "type": "Windows.UI.Xaml.Controls.StackPanel",
    "Name": "",
    "Visibility": "Visible",
    "Margin": "0,0,0,0",
    "Padding": "0,0,0,0",
    "Background": "[null]",
    "BorderBrush": "#FF006400",
    "BorderThickness": "1,1,1,1",
    "Foreground": "[null]",
    "children": [
        {
            "type": "Windows.UI.Xaml.Controls.Button",
            "Name": "MyButton1",
            "Visibility": "Visible",
            "Margin": "0,0,0,0",
            "Padding": "8,5,8,6",
            "Background": "#FFD3D3D3",
            "BorderBrush": "#00FFFFFF",
            "BorderThickness": "1,1,1,1",
            "Foreground": "#FFFFFFFF",
            "children": [
                {
                    "type": "Windows.UI.Xaml.Controls.ContentPresenter",
                    "Name": "ContentPresenter",
                    "Visibility": "Visible",
                    "Margin": "0,0,0,0",
                    "Padding": "8,5,8,6",
                    "Background": "#FFD3D3D3",
                    "BorderBrush": "#00FFFFFF",
                    "BorderThickness": "1,1,1,1",
                    "Foreground": "#FFFFFFFF",
                    "children": [
                        {
                            "type": "Windows.UI.Xaml.Controls.TextBlock",
                            "Name": "",
                            "Visibility": "Visible",
                            "Margin": "0,0,0,0",
                            "Padding": "0,0,0,0",
                            "Background": "[null]",
                            "BorderBrush": "[null]",
                            "BorderThickness": "[null]",
                            "Foreground": "#FF000000",
                            "children": []
                        }
                    ]
                }
            ]
        },
        {
            "type": "Windows.UI.Xaml.Controls.TextBlock",
            "Name": "ExampleTextBlock",
            "Visibility": "Visible",
            "Margin": "0,0,0,0",
            "Padding": "0,0,0,0",
            "Background": "[null]",
            "BorderBrush": "[null]",
            "BorderThickness": "[null]",
            "Foreground": "#FF006400",
            "children": []
        }
    ]
}
```

Formatted string output without namespaces and excluding null values and empty strings:
```json
{
    "type": "StackPanel",
    "Visibility": "Visible",
    "Margin": "0,0,0,0",
    "Padding": "0,0,0,0",
    "BorderBrush": "#FF006400",
    "BorderThickness": "1,1,1,1",
    "children": [
        {
            "type": "Button",
            "Name": "MyButton1",
            "Visibility": "Visible",
            "Margin": "0,0,0,0",
            "Padding": "8,5,8,6",
            "Background": "#FFD3D3D3",
            "BorderBrush": "#00FFFFFF",
            "BorderThickness": "1,1,1,1",
            "Foreground": "#FFFFFFFF",
            "children": [
                {
                    "type": "ContentPresenter",
                    "Name": "ContentPresenter",
                    "Visibility": "Visible",
                    "Margin": "0,0,0,0",
                    "Padding": "8,5,8,6",
                    "Background": "#FFD3D3D3",
                    "BorderBrush": "#00FFFFFF",
                    "BorderThickness": "1,1,1,1",
                    "Foreground": "#FFFFFFFF",
                    "children": [
                        {
                            "type": "TextBlock",
                            "Visibility": "Visible",
                            "Margin": "0,0,0,0",
                            "Padding": "0,0,0,0",
                            "Foreground": "#FF000000",
                            "children": []
                        }
                    ]
                }
            ]
        },
        {
            "type": "TextBlock",
            "Name": "ExampleTextBlock",
            "Visibility": "Visible",
            "Margin": "0,0,0,0",
            "Padding": "0,0,0,0",
            "Foreground": "#FF006400",
            "children": []
        }
    ]
}
```

Like the XmlExporter, the JsonExporter allows the usage of [IObjectToStringConverter](xref:VisualTreeAnalyzers.Snapshot.Exporter.IObjectToStringConverter) objects to provide better string representation of property values.
Unless specified otherwise, the [StandardObjectToStringConverter](xref:VisualTreeAnalyzers.Snapshot.Exporter.StandardObjectToStringConverter) will be used to provide better representations.