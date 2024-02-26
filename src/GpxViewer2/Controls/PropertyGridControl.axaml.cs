using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using GpxViewer2.Controls.PropertyGrid;

namespace GpxViewer2.Controls;

public partial class PropertyGridControl : UserControl
{
    public static readonly DirectProperty<PropertyGridControl, object?> SelectedObjectProperty =
        AvaloniaProperty.RegisterDirect<PropertyGridControl, object?>(
            nameof(SelectedObject),
            x => x.SelectedObject,
            (x, y) => x.SelectedObject = y);

    public static readonly StyledProperty<PropertyGridEditControlFactory?> EditControlFactoryProperty =
        AvaloniaProperty.Register<PropertyGridControl, PropertyGridEditControlFactory?>(
            nameof(EditControlFactory),
            new PropertyGridEditControlFactory());

    public static readonly StyledProperty<IPropertyContractResolver?> PropertyContractResolverProperty =
        AvaloniaProperty.Register<PropertyGridControl, IPropertyContractResolver?>(
            nameof(PropertyContractResolver));

    private PropertyGridViewModel _propertyGridVM;
    private Grid _gridMain;
    private Control? _firstValueRowEditor;
    private object? _selectedObject;

    public object? SelectedObject
    {
        get => _selectedObject;
        set
        {
            var changed= _selectedObject != value;
            _selectedObject = value;

            if (changed)
            {
                this.OnSelectedObjectChanged();
            }
        }
    }

    public PropertyGridEditControlFactory? EditControlFactory
    {
        get => this.GetValue(EditControlFactoryProperty);
        set => this.SetValue(EditControlFactoryProperty, value);
    }

    public IPropertyContractResolver? PropertyContractResolver
    {
        get => this.GetValue(PropertyContractResolverProperty);
        set
        {
            this.SetValue(PropertyContractResolverProperty, value);
            _propertyGridVM.SetPropertyContractResolver(value);
        }
    }

    public PropertyGridControl()
    {
        AvaloniaXamlLoader.Load(this);

        _gridMain = this.FindControl<Grid>("GridMain");
        
        _propertyGridVM = new PropertyGridViewModel();
        _gridMain.DataContext = _propertyGridVM;
    }

    public void FocusFirstValueRowEditor()
    {
        // TODO: FocusManager.Instance removed?
        // FocusManager.Instance?.Focus(_firstValueRowEditor, NavigationMethod.Tab);
    }

    private void OnSelectedObjectChanged()
    {
        this._propertyGridVM.SelectedObject = this.SelectedObject;
        this.UpdatePropertiesView();
    }

    private void UpdatePropertiesView()
    {
        _gridMain.Children.Clear();
        _gridMain.RowDefinitions.Clear();

        var lstProperties = new List<ConfigurablePropertyRuntime>(_propertyGridVM.PropertyMetadata);
        lstProperties.Sort((left, right) =>
            string.Compare(left.Metadata.CategoryName, right.Metadata.CategoryName, StringComparison.Ordinal));
        var allPropertiesMetadata = lstProperties.Select(x => x.Metadata);

        // Create all controls
        var actRowIndex = 0;
        var actCategory = string.Empty;
        var editControlFactory = this.EditControlFactory;
        if (editControlFactory == null) { editControlFactory = new PropertyGridEditControlFactory(); }

        foreach (var actProperty in _propertyGridVM.PropertyMetadata)
        {
            // Create category rows
            if (actProperty.Metadata.CategoryName != actCategory)
            {
                _gridMain.RowDefinitions.Add(new RowDefinition {Height = new GridLength(35)});

                actCategory = actProperty.Metadata.CategoryName;

                var txtHeader = new TextBlock
                {
                    Text = actCategory
                };

                txtHeader.SetValue(Grid.RowProperty, actRowIndex);
                txtHeader.SetValue(Grid.ColumnSpanProperty, 3);
                txtHeader.SetValue(Grid.ColumnProperty, 0);
                txtHeader.Margin = new Thickness(5d, 5d, 5d, 5d);
                txtHeader.VerticalAlignment = VerticalAlignment.Bottom;
                txtHeader.FontWeight = FontWeight.Bold;
                _gridMain.Children.Add(txtHeader);

                var rect = new Rectangle
                {
                    Height = 1d,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Margin = new Thickness(5d, 5d, 5d, 0d)
                };
                rect.Classes.Add("PropertyGridCategoryHeaderLine");

                rect.SetValue(Grid.RowProperty, actRowIndex);
                rect.SetValue(Grid.ColumnSpanProperty, 3);
                rect.SetValue(Grid.ColumnProperty, 0);
                _gridMain.Children.Add(rect);

                actRowIndex++;
            }

            // Create row header
            var ctrlTextContainer = new Border();
            var ctrlText = new TextBlock();
            ctrlText.Text = actProperty.Metadata.PropertyDisplayName;
            ctrlText.VerticalAlignment = VerticalAlignment.Center;
            SetToolTip(ctrlText, actProperty.Metadata.Description);
            ctrlTextContainer.Height = 35.0;
            ctrlTextContainer.Child = ctrlText;
            ctrlTextContainer.SetValue(Grid.RowProperty, actRowIndex);
            ctrlTextContainer.SetValue(Grid.ColumnProperty, 0);
            ctrlTextContainer.VerticalAlignment = VerticalAlignment.Top;
            _gridMain.Children.Add(ctrlTextContainer);

            // Create and configure row editor
            var ctrlValueEdit = editControlFactory.CreateControl(actProperty.Metadata, nameof(actProperty.ValueAccessor), allPropertiesMetadata);
            if (ctrlValueEdit != null)
            {
                _gridMain.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
                ctrlValueEdit.Margin = new Thickness(0d, 0d, 5d, 0d);
                ctrlValueEdit.VerticalAlignment = VerticalAlignment.Center;
                ctrlValueEdit.SetValue(Grid.RowProperty, actRowIndex);
                ctrlValueEdit.SetValue(Grid.ColumnProperty, 2);
                ctrlValueEdit.DataContext = actProperty;
                SetToolTip(ctrlValueEdit, actProperty.Metadata.Description);
                _gridMain.Children.Add(ctrlValueEdit);

                _firstValueRowEditor ??= ctrlValueEdit;
            }
            else
            {
                _gridMain.RowDefinitions.Add(new RowDefinition(1.0, GridUnitType.Pixel));
            }

            actRowIndex++;
        }

        if (_gridMain.RowDefinitions.Count > 0)
        {
            var gridSplitter = new GridSplitter();
            gridSplitter.SetValue(Grid.ColumnProperty, 1);
            gridSplitter.SetValue(Grid.RowSpanProperty, _gridMain.RowDefinitions.Count);
            gridSplitter.HorizontalAlignment = HorizontalAlignment.Stretch;
            gridSplitter.VerticalAlignment = VerticalAlignment.Stretch;
            gridSplitter.Background = Brushes.Transparent;
            _gridMain.Children.Insert(0, gridSplitter);
        }
    }

    public static void SetToolTip(AvaloniaObject targetObject, string toolTip)
    {
        if (string.IsNullOrEmpty(toolTip)) { return; }
        targetObject.SetValue(ToolTip.TipProperty, toolTip);
    }
}