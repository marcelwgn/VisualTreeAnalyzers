using System;
using VisualTreeAnalyzers.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;

namespace VisualTreeAnalyzers.Accessibility.Rules
{
    /// <summary>
    /// This rule verifies that every <see cref="PatternInterface"/> provided is actually implemented by the automation peer.
    /// </summary>
    /// <remarks>
    /// This rule ignores <see cref="ITextProvider"/>, <see cref="ITextProvider2"/> and <see cref="ITextRangeProvider"/> 
    ///  as those are not implemented by Windows Runtime peers and would lead to false negatives.
    /// </remarks>
    [AccessibilityRule]
    public sealed class PatternInterfaceImplementedRule : IRule
    {
        /// <inheritdoc/>
        public string FailureMessage => "AutomationPeer does not implement all interfaces claimed to support.";

        /// <inheritdoc/>
        public bool IsValid(FrameworkElement element, AutomationPeer peer)
        {
            if(peer != null)
            {
                for(int i=0;i<patterns.Length;i++)
                {
                    if(peer.GetPattern(patterns[i]) is AutomationPeer patternPeer)
                    {
                        if(!interfaces[i].IsAssignableFrom(patternPeer.GetType()))
                        {
                            return false;
                        }
                    }
                } 
            }
            return true;
        }

        /// <inheritdoc/>
        public void ResetState()
        {
        }

        private readonly PatternInterface[] patterns = new PatternInterface[]
        {
            PatternInterface.Annotation,
            PatternInterface.CustomNavigation,
            PatternInterface.Dock,
            PatternInterface.Drag,
            PatternInterface.DropTarget,
            PatternInterface.ExpandCollapse,
            PatternInterface.Grid,
            PatternInterface.GridItem,
            PatternInterface.Invoke,
            PatternInterface.ItemContainer,
            PatternInterface.MultipleView,
            PatternInterface.ObjectModel,
            PatternInterface.RangeValue,
            PatternInterface.Scroll,
            PatternInterface.ScrollItem,
            PatternInterface.Selection,
            PatternInterface.SelectionItem,
            PatternInterface.Spreadsheet,
            PatternInterface.SpreadsheetItem,
            PatternInterface.Styles,
            PatternInterface.SynchronizedInput,
            PatternInterface.Table,
            PatternInterface.TableItem,
            PatternInterface.TextChild,
            PatternInterface.TextEdit,
            PatternInterface.Toggle,
            PatternInterface.Transform,
            PatternInterface.Value,
            PatternInterface.VirtualizedItem,
            PatternInterface.Window
        };

        private readonly Type[] interfaces = new Type[]
        {
            typeof(IAnnotationProvider),
            typeof(ICustomNavigationProvider),
            typeof(IDockProvider),
            typeof(IDragProvider),
            typeof(IDropTargetProvider),
            typeof(IExpandCollapseProvider),
            typeof(IGridProvider),
            typeof(IGridItemProvider),
            typeof(IInvokeProvider),
            typeof(IItemContainerProvider),
            typeof(IMultipleViewProvider),
            typeof(IObjectModelProvider),
            typeof(IRangeValueProvider),
            typeof(IScrollProvider),
            typeof(IScrollItemProvider),
            typeof(ISelectionProvider),
            typeof(ISelectionItemProvider),
            typeof(ISpreadsheetProvider),
            typeof(ISpreadsheetItemProvider),
            typeof(IStylesProvider),
            typeof(ISynchronizedInputProvider),
            typeof(ITableProvider),
            typeof(ITableItemProvider),
            typeof(ITextChildProvider),
            typeof(ITextEditProvider),
            typeof(IToggleProvider),
            typeof(ITransformProvider),
            typeof(IValueProvider),
            typeof(IVirtualizedItemProvider),
            typeof(IWindowProvider)
        };
    }
}
