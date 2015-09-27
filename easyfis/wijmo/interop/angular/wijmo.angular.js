var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
/*
*
* Wijmo Library 5.20151.48
* http://wijmo.com/
*
* Copyright(c) GrapeCity, Inc.  All rights reserved.
*
* Licensed under the Wijmo Commercial License.
* sales@wijmo.com
* http://wijmo.com/products/wijmo-5/license/
*
*/
var wijmo;
(function (wijmo) {
    // prevent double loading
    if (wijmo && wijmo.interop) {
        return;
    }

    (function (interop) {
        /*
        Represents a shared metadata (control properties/events descriptions) storage used by interop services like
        Angular directives and Knockout custom bindings.
        Control metadata is retrieved using the getMetaData method by passing the control's metaDataId (see the
        method description for details).
        Descriptor objects are created using the CreateProp, CreateEvent and CreateComplexProp static methods.
        The specific interop service should create a class derived from ControlMetaFactory and override these methods to
        create descriptors of the platform specific types (see the wijmo.angular.MetaFactory class as an example).
        To initialize platform specific properties of the descriptors an interop services can use the findProp, findEvent and
        findComplexProp methods to find a necessary descriptor object by name.
        */
        var ControlMetaFactory = (function () {
            function ControlMetaFactory() {
            }
            // Creates a property descriptor object. A specific interop service should override this method in the derived
            // metadata factory class to create platrorm specific descriptor object.
            ControlMetaFactory.CreateProp = function (propertyName, propertyType, bindingMode, enumType, isNativeControlProperty, priority) {
                return new PropDescBase(propertyName, propertyType, bindingMode, enumType, isNativeControlProperty, priority);
            };

            // Creates an event descriptor object. A specific interop service should override this method in the derived
            // metadata factory class to create platrorm specific descriptor object.
            ControlMetaFactory.CreateEvent = function (eventName, isPropChanged) {
                return new EventDescBase(eventName, isPropChanged);
            };

            // Creates a complex property descriptor object. A specific interop service should override this method in the derived
            // metadata factory class to create platrorm specific descriptor object.
            ControlMetaFactory.CreateComplexProp = function (propertyName, isArray, ownsObject) {
                return new ComplexPropDescBase(propertyName, isArray, ownsObject);
            };

            // Finds a property descriptor by the property name in the specified array.
            ControlMetaFactory.findProp = function (propName, props) {
                return this.findInArr(props, 'propertyName', propName);
            };

            // Finds an event descriptor by the event name in the specified array.
            ControlMetaFactory.findEvent = function (eventName, events) {
                return this.findInArr(events, 'eventName', eventName);
            };

            // Finds a complex property descriptor by the property name in the specified array.
            ControlMetaFactory.findComplexProp = function (propName, props) {
                return this.findInArr(props, 'propertyName', propName);
            };

            /*
            Returns metadata for the control by its metadata ID.In the most cases the control type (constructor function)
            is used as metadata ID. In cases where this is not applicable an arbitrary object can be used as an ID, e.g.
            'MenuItem' string is used as the ID for Menu Item.
            
            The sets of descriptors returned for the specific metadata ID take into account the controls inheritance chain
            and include metadata defined for the control's base classes.
            In case of a control that has no a base class metadata you create its metadata object with a constructor:
            return new MetaDataBase(... descriptor arrays ...);
            
            If the control has the base control metadata then you create its metadata object by a recursive call to
            the getMetaData method with the base control's metadata ID passed, and add the controls own metadata to
            the returned object using the 'add' method. E.g. for the ComboBox derived from the DropDown this looks like:
            return this.getMetaData(wijmo.input.DropDown).add(... descriptor arrays ...);
            
            The specific platforms provide the following implementations of the metadata ID support:
            Angular
            =======
            The WjDirective._getMetaDataId method returns a metadata ID. By default it returns a value of the
            WjDirective._controlConstructor property. Because of this approach it's reasonable to override the
            _controlConstructor property even in the abstract classes like WjDropDown, in this case it's not necessary
            to override the _getMetaDataId method itself.
            ----------------
            WARNING: if you overridden the _getMetaDataId method, don't forget to override it in the derived classes!
            ----------------
            You usually need to override the _getMetaDataId method only for classes like WjMenuItem and WjCollectionViewNavigator
            for which the _controlConstructor as an ID approach doesn't work.
            
            Knockout
            ========
            TBD
            */
            ControlMetaFactory.getMetaData = function (metaDataId) {
                switch (metaDataId) {
                    case wijmo.input && wijmo.input.DropDown:
                        return new MetaDataBase([
                            this.CreateProp('isDroppedDown', PropertyType.Boolean, BindingMode.TwoWay),
                            this.CreateProp('showDropDownButton', PropertyType.Boolean),
                            this.CreateProp('placeholder', PropertyType.String),
                            this.CreateProp('text', PropertyType.String, BindingMode.TwoWay, null, true, 1000)
                        ], [
                            this.CreateEvent('isDroppedDownChanged', true),
                            this.CreateEvent('textChanged', true)
                        ]);

                    case wijmo.input && wijmo.input.ComboBox:
                        return this.getMetaData(wijmo.input.DropDown).add([
                            this.CreateProp('displayMemberPath', PropertyType.String),
                            this.CreateProp('selectedValuePath', PropertyType.String),
                            this.CreateProp('isContentHtml', PropertyType.Boolean),
                            this.CreateProp('isEditable', PropertyType.Boolean),
                            this.CreateProp('required', PropertyType.Boolean),
                            this.CreateProp('maxDropDownHeight', PropertyType.Number),
                            this.CreateProp('maxDropDownWidth', PropertyType.Number),
                            this.CreateProp('itemFormatter', PropertyType.Function),
                            this.CreateProp('itemsSource', PropertyType.Any, BindingMode.OneWay, null, true, 900),
                            this.CreateProp('selectedIndex', PropertyType.Number, BindingMode.TwoWay, null, true, 1000),
                            this.CreateProp('selectedItem', PropertyType.Any, BindingMode.TwoWay, null, true, 1000),
                            this.CreateProp('selectedValue', PropertyType.Any, BindingMode.TwoWay, null, true, 1000)
                        ], [
                            this.CreateEvent('selectedIndexChanged', true)
                        ]).addOptions({ ngModelProperty: 'selectedValue' });

                    case wijmo.input && wijmo.input.AutoComplete:
                        return this.getMetaData(wijmo.input.ComboBox).add([
                            this.CreateProp('delay', PropertyType.Number),
                            this.CreateProp('maxItems', PropertyType.Number),
                            this.CreateProp('minLength', PropertyType.Number),
                            this.CreateProp('cssMatch', PropertyType.String),
                            this.CreateProp('itemsSourceFunction', PropertyType.Function)
                        ]);

                    case wijmo.input && wijmo.input.Calendar:
                        return new MetaDataBase([
                            this.CreateProp('itemFormatter', PropertyType.Function),
                            this.CreateProp('monthView', PropertyType.Boolean),
                            this.CreateProp('showHeader', PropertyType.Boolean),
                            this.CreateProp('max', PropertyType.Date),
                            this.CreateProp('min', PropertyType.Date),
                            this.CreateProp('value', PropertyType.Date, BindingMode.TwoWay),
                            this.CreateProp('displayMonth', PropertyType.Date, BindingMode.TwoWay),
                            this.CreateProp('firstDayOfWeek', PropertyType.Number)
                        ], [
                            this.CreateEvent('valueChanged', true),
                            this.CreateEvent('displayMonthChanged', true)
                        ]).addOptions({ ngModelProperty: 'value' });

                    case wijmo.input && wijmo.input.ColorPicker:
                        return new MetaDataBase([
                            this.CreateProp('showAlphaChannel', PropertyType.Boolean),
                            this.CreateProp('showColorString', PropertyType.Boolean),
                            this.CreateProp('palette', PropertyType.Any),
                            this.CreateProp('value', PropertyType.String, BindingMode.TwoWay)
                        ], [
                            this.CreateEvent('valueChanged', true)
                        ]).addOptions({ ngModelProperty: 'value' });

                    case wijmo.input && wijmo.input.ListBox:
                        return new MetaDataBase([
                            this.CreateProp('isContentHtml', PropertyType.Boolean),
                            this.CreateProp('maxHeight', PropertyType.Number),
                            this.CreateProp('selectedValuePath', PropertyType.String),
                            this.CreateProp('itemFormatter', PropertyType.Function),
                            this.CreateProp('displayMemberPath', PropertyType.String),
                            this.CreateProp('checkedMemberPath', PropertyType.String),
                            this.CreateProp('itemsSource', PropertyType.Any),
                            this.CreateProp('selectedIndex', PropertyType.Number, BindingMode.TwoWay),
                            this.CreateProp('selectedItem', PropertyType.Any, BindingMode.TwoWay),
                            this.CreateProp('selectedValue', PropertyType.Any, BindingMode.TwoWay)
                        ], [
                            this.CreateEvent('itemsChanged', true),
                            this.CreateEvent('itemChecked', false),
                            this.CreateEvent('selectedIndexChanged', true)
                        ]).addOptions({ ngModelProperty: 'selectedValue' });

                    case wijmo.input && wijmo.input.Menu:
                        return this.getMetaData(wijmo.input.ComboBox).add([
                            this.CreateProp('header', PropertyType.String),
                            this.CreateProp('commandParameterPath', PropertyType.String),
                            this.CreateProp('commandPath', PropertyType.String),
                            this.CreateProp('isButton', PropertyType.Boolean),
                            this.CreateProp('value', PropertyType.Any, BindingMode.TwoWay, null, false, 1000)
                        ], [
                            this.CreateEvent('itemClicked')
                        ]);

                    case 'MenuItem':
                        return new MetaDataBase([
                            this.CreateProp('value', PropertyType.Any, BindingMode.OneWay),
                            this.CreateProp('cmd', PropertyType.Any, BindingMode.OneWay),
                            this.CreateProp('cmdParam', PropertyType.Any, BindingMode.OneWay)
                        ]);

                    case wijmo.input && wijmo.input.InputDate:
                        return this.getMetaData(wijmo.input.DropDown).add([
                            this.CreateProp('required', PropertyType.Boolean),
                            this.CreateProp('format', PropertyType.String),
                            this.CreateProp('mask', PropertyType.String),
                            this.CreateProp('max', PropertyType.Date),
                            this.CreateProp('min', PropertyType.Date),
                            this.CreateProp('value', PropertyType.Date, BindingMode.TwoWay, null, true, 1000)
                        ], [
                            this.CreateEvent('valueChanged', true)
                        ]).addOptions({ ngModelProperty: 'value' });

                    case wijmo.input && wijmo.input.InputNumber:
                        return new MetaDataBase([
                            this.CreateProp('showSpinner', PropertyType.Boolean),
                            this.CreateProp('max', PropertyType.Number),
                            this.CreateProp('min', PropertyType.Number),
                            this.CreateProp('step', PropertyType.Number),
                            this.CreateProp('required', PropertyType.Boolean),
                            this.CreateProp('placeholder', PropertyType.String),
                            this.CreateProp('inputType', PropertyType.String),
                            this.CreateProp('value', PropertyType.Number, BindingMode.TwoWay),
                            this.CreateProp('text', PropertyType.String, BindingMode.TwoWay),
                            this.CreateProp('format', PropertyType.String)
                        ], [
                            this.CreateEvent('valueChanged', true),
                            this.CreateEvent('textChanged', true)
                        ]).addOptions({ ngModelProperty: 'value' });

                    case wijmo.input && wijmo.input.InputMask:
                        return new MetaDataBase([
                            this.CreateProp('mask', PropertyType.String),
                            this.CreateProp('promptChar', PropertyType.String),
                            this.CreateProp('placeholder', PropertyType.String),
                            this.CreateProp('value', PropertyType.String, BindingMode.TwoWay)
                        ], [
                            this.CreateEvent('valueChanged', true)
                        ]).addOptions({ ngModelProperty: 'value' });

                    case wijmo.input && wijmo.input.InputTime:
                        return this.getMetaData(wijmo.input.ComboBox).add([
                            this.CreateProp('max', PropertyType.Date),
                            this.CreateProp('min', PropertyType.Date),
                            this.CreateProp('step', PropertyType.Number),
                            this.CreateProp('format', PropertyType.String),
                            this.CreateProp('mask', PropertyType.String),
                            this.CreateProp('value', PropertyType.Date, BindingMode.TwoWay, null, true, 1000)
                        ], [
                            this.CreateEvent('valueChanged', true)
                        ]).addOptions({ ngModelProperty: 'value' });

                    case wijmo.input && wijmo.input.InputColor:
                        return this.getMetaData(wijmo.input.DropDown).add([
                            this.CreateProp('required', PropertyType.Boolean),
                            this.CreateProp('showAlphaChannel', PropertyType.Boolean),
                            this.CreateProp('value', PropertyType.String, BindingMode.TwoWay)
                        ], [
                            this.CreateEvent('valueChanged', true)
                        ]).addOptions({ ngModelProperty: 'value' });

                    case 'CollectionViewNavigator':
                        return new MetaDataBase([
                            this.CreateProp('cv', PropertyType.Any)
                        ]);

                    case 'CollectionViewPager':
                        return new MetaDataBase([
                            this.CreateProp('cv', PropertyType.Any)
                        ]);

                    case wijmo.grid && wijmo.grid.FlexGrid:
                        return new MetaDataBase([
                            this.CreateProp('allowAddNew', PropertyType.Boolean),
                            this.CreateProp('allowDelete', PropertyType.Boolean),
                            this.CreateProp('allowDragging', PropertyType.Enum, BindingMode.OneWay, wijmo.grid.AllowDragging),
                            this.CreateProp('allowMerging', PropertyType.Enum, BindingMode.OneWay, wijmo.grid.AllowMerging),
                            this.CreateProp('allowResizing', PropertyType.Enum, BindingMode.OneWay, wijmo.grid.AllowResizing),
                            this.CreateProp('autoSizeMode', PropertyType.Enum, BindingMode.OneWay, wijmo.grid.AutoSizeMode),
                            this.CreateProp('allowSorting', PropertyType.Boolean),
                            this.CreateProp('autoGenerateColumns', PropertyType.Boolean),
                            this.CreateProp('childItemsPath', PropertyType.Any),
                            this.CreateProp('groupHeaderFormat', PropertyType.String),
                            this.CreateProp('headersVisibility', PropertyType.Enum, BindingMode.OneWay, wijmo.grid.HeadersVisibility),
                            this.CreateProp('itemFormatter', PropertyType.Function),
                            this.CreateProp('isReadOnly', PropertyType.Boolean),
                            this.CreateProp('mergeManager', PropertyType.Any),
                            this.CreateProp('selectionMode', PropertyType.Enum, BindingMode.OneWay, wijmo.grid.SelectionMode),
                            this.CreateProp('showGroups', PropertyType.Boolean),
                            this.CreateProp('showSort', PropertyType.Boolean),
                            this.CreateProp('treeIndent', PropertyType.Number),
                            this.CreateProp('itemsSource', PropertyType.Any),
                            this.CreateProp('autoClipboard', PropertyType.Boolean),
                            this.CreateProp('frozenRows', PropertyType.Number),
                            this.CreateProp('frozenColumns', PropertyType.Number),
                            this.CreateProp('deferResizing', PropertyType.Boolean),
                            this.CreateProp('sortRowIndex', PropertyType.Number)
                        ], [
                            this.CreateEvent('beginningEdit'),
                            this.CreateEvent('cellEditEnded'),
                            this.CreateEvent('cellEditEnding'),
                            this.CreateEvent('prepareCellForEdit'),
                            this.CreateEvent('formatItem'),
                            this.CreateEvent('resizingColumn'),
                            this.CreateEvent('resizedColumn'),
                            this.CreateEvent('autoSizingColumn'),
                            this.CreateEvent('autoSizedColumn'),
                            this.CreateEvent('draggingColumn'),
                            this.CreateEvent('draggedColumn'),
                            this.CreateEvent('sortingColumn'),
                            this.CreateEvent('sortedColumn'),
                            this.CreateEvent('resizingRow'),
                            this.CreateEvent('resizedRow'),
                            this.CreateEvent('autoSizingRow'),
                            this.CreateEvent('autoSizedRow'),
                            this.CreateEvent('draggingRow'),
                            this.CreateEvent('draggedRow'),
                            this.CreateEvent('deletingRow'),
                            this.CreateEvent('loadingRows'),
                            this.CreateEvent('loadedRows'),
                            this.CreateEvent('rowEditEnded'),
                            this.CreateEvent('rowEditEnding'),
                            this.CreateEvent('rowAdded'),
                            this.CreateEvent('groupCollapsedChanged'),
                            this.CreateEvent('groupCollapsedChanging'),
                            this.CreateEvent('itemsSourceChanged', true),
                            this.CreateEvent('selectionChanging'),
                            this.CreateEvent('selectionChanged', true),
                            this.CreateEvent('scrollPositionChanged', false),
                            this.CreateEvent('pasting'),
                            this.CreateEvent('pasted'),
                            this.CreateEvent('copying'),
                            this.CreateEvent('copied')
                        ]);

                    case wijmo.grid && wijmo.grid.Column:
                        return new MetaDataBase([
                            this.CreateProp('name', PropertyType.String),
                            this.CreateProp('dataMap', PropertyType.Any),
                            this.CreateProp('dataType', PropertyType.Enum, BindingMode.OneWay, wijmo.DataType),
                            this.CreateProp('binding', PropertyType.String),
                            this.CreateProp('sortMemberPath', PropertyType.String),
                            this.CreateProp('format', PropertyType.String),
                            this.CreateProp('header', PropertyType.String),
                            this.CreateProp('width', PropertyType.Number),
                            this.CreateProp('minWidth', PropertyType.Number),
                            this.CreateProp('maxWidth', PropertyType.Number),
                            this.CreateProp('align', PropertyType.String),
                            this.CreateProp('allowDragging', PropertyType.Boolean),
                            this.CreateProp('allowSorting', PropertyType.Boolean),
                            this.CreateProp('allowResizing', PropertyType.Boolean),
                            this.CreateProp('allowMerging', PropertyType.Boolean),
                            this.CreateProp('aggregate', PropertyType.Enum, BindingMode.OneWay, wijmo.Aggregate),
                            this.CreateProp('isReadOnly', PropertyType.Boolean),
                            this.CreateProp('cssClass', PropertyType.String),
                            this.CreateProp('isContentHtml', PropertyType.Boolean),
                            this.CreateProp('isSelected', PropertyType.Boolean, BindingMode.TwoWay),
                            this.CreateProp('visible', PropertyType.Boolean),
                            this.CreateProp('wordWrap', PropertyType.Boolean),
                            this.CreateProp('mask', PropertyType.String),
                            this.CreateProp('inputType', PropertyType.String),
                            this.CreateProp('required', PropertyType.Boolean),
                            this.CreateProp('showDropDown', PropertyType.Boolean)
                        ], [], [], 'columns', true);

                    case 'FlexGridCellTemplate':
                        return new MetaDataBase([
                            this.CreateProp('cellType', PropertyType.String, BindingMode.OneWay, null, false),
                            this.CreateProp('cellOverflow', PropertyType.String, BindingMode.OneWay)
                        ], [], [], undefined, undefined, undefined, 'owner');

                    case wijmo.grid && wijmo.grid.filter && wijmo.grid.filter.FlexGridFilter:
                        return new MetaDataBase([
                            this.CreateProp('showFilterIcons', PropertyType.Boolean),
                            this.CreateProp('filterColumns', PropertyType.Any)
                        ], [
                            this.CreateEvent('filterApplied')
                        ], [], undefined, undefined, undefined, '');

                    case wijmo.grid && wijmo.grid.grouppanel && wijmo.grid.grouppanel.GroupPanel:
                        return new MetaDataBase([
                            this.CreateProp('hideGroupedColumns', PropertyType.Boolean),
                            this.CreateProp('maxGroups', PropertyType.Number),
                            this.CreateProp('placeholder', PropertyType.String),
                            this.CreateProp('grid', PropertyType.Any)
                        ]);

                    case wijmo.chart && wijmo.chart.FlexChartBase:
                        return new MetaDataBase([
                            this.CreateProp('binding', PropertyType.String),
                            this.CreateProp('footer', PropertyType.String),
                            this.CreateProp('header', PropertyType.String),
                            this.CreateProp('selectionMode', PropertyType.Enum, BindingMode.OneWay, wijmo.chart.SelectionMode),
                            this.CreateProp('palette', PropertyType.Any),
                            this.CreateProp('plotMargin', PropertyType.Any),
                            this.CreateProp('footerStyle', PropertyType.Any),
                            this.CreateProp('headerStyle', PropertyType.Any),
                            this.CreateProp('tooltipContent', PropertyType.String, BindingMode.OneWay, null, false),
                            this.CreateProp('itemsSource', PropertyType.Any)
                        ], [
                            this.CreateEvent('rendering'),
                            this.CreateEvent('rendered')
                        ]);

                    case wijmo.chart && wijmo.chart.FlexChart:
                        return this.getMetaData(wijmo.chart.FlexChartBase).add([
                            this.CreateProp('bindingX', PropertyType.String),
                            this.CreateProp('chartType', PropertyType.Enum, BindingMode.OneWay, wijmo.chart.ChartType),
                            this.CreateProp('interpolateNulls', PropertyType.Boolean),
                            this.CreateProp('legendToggle', PropertyType.Boolean),
                            this.CreateProp('rotated', PropertyType.Boolean),
                            this.CreateProp('stacking', PropertyType.Enum, BindingMode.OneWay, wijmo.chart.Stacking),
                            this.CreateProp('symbolSize', PropertyType.Number),
                            this.CreateProp('options', PropertyType.Any),
                            this.CreateProp('selection', PropertyType.Any, BindingMode.TwoWay),
                            this.CreateProp('itemFormatter', PropertyType.Function),
                            this.CreateProp('labelContent', PropertyType.String, BindingMode.OneWay, null, false)
                        ], [
                            this.CreateEvent('seriesVisibilityChanged'),
                            this.CreateEvent('selectionChanged', true)
                        ], [
                            this.CreateComplexProp('axisX', false, false),
                            this.CreateComplexProp('axisY', false, false),
                            this.CreateComplexProp('axes', true)
                        ]);

                    case wijmo.chart && wijmo.chart.FlexPie:
                        return this.getMetaData(wijmo.chart.FlexChartBase).add([
                            this.CreateProp('bindingName', PropertyType.String),
                            this.CreateProp('innerRadius', PropertyType.Number),
                            this.CreateProp('isAnimated', PropertyType.Boolean),
                            this.CreateProp('offset', PropertyType.Number),
                            this.CreateProp('reversed', PropertyType.Boolean),
                            this.CreateProp('startAngle', PropertyType.Number),
                            this.CreateProp('selectedItemPosition', PropertyType.Enum, BindingMode.OneWay, wijmo.chart.Position),
                            this.CreateProp('selectedItemOffset', PropertyType.Number),
                            this.CreateProp('itemFormatter', PropertyType.Function),
                            this.CreateProp('labelContent', PropertyType.String, BindingMode.OneWay, null, false)
                        ]);

                    case wijmo.chart && wijmo.chart.Axis:
                        return new MetaDataBase([
                            this.CreateProp('axisLine', PropertyType.Boolean),
                            this.CreateProp('format', PropertyType.String),
                            this.CreateProp('labels', PropertyType.Boolean),
                            this.CreateProp('majorGrid', PropertyType.Boolean),
                            this.CreateProp('majorTickMarks', PropertyType.Enum, BindingMode.OneWay, wijmo.chart.TickMark),
                            this.CreateProp('majorUnit', PropertyType.Number),
                            this.CreateProp('max', PropertyType.Number),
                            this.CreateProp('min', PropertyType.Number),
                            this.CreateProp('position', PropertyType.Enum, BindingMode.OneWay, wijmo.chart.Position),
                            this.CreateProp('reversed', PropertyType.Boolean),
                            this.CreateProp('title', PropertyType.String),
                            this.CreateProp('labelAngle', PropertyType.Number),
                            this.CreateProp('minorGrid', PropertyType.Boolean),
                            this.CreateProp('minorTickMarks', PropertyType.Enum, BindingMode.OneWay, wijmo.chart.TickMark),
                            this.CreateProp('origin', PropertyType.Number),
                            this.CreateProp('logBase', PropertyType.Number)
                        ], [], [], 'axes', true);

                    case wijmo.chart && wijmo.chart.Legend:
                        return new MetaDataBase([
                            this.CreateProp('position', PropertyType.Enum, BindingMode.OneWay, wijmo.chart.Position)
                        ], [], [], 'legend', false, false);

                    case wijmo.chart && wijmo.chart.DataLabelBase:
                        return new MetaDataBase([
                            this.CreateProp('content', PropertyType.Any, BindingMode.OneWay),
                            this.CreateProp('border', PropertyType.Boolean)
                        ], [], [], 'dataLabel', false, false);

                    case wijmo.chart && wijmo.chart.DataLabel:
                        return this.getMetaData(wijmo.chart.DataLabelBase).add([
                            this.CreateProp('position', PropertyType.Enum, BindingMode.OneWay, wijmo.chart.LabelPosition)
                        ]);

                    case wijmo.chart && wijmo.chart.PieDataLabel:
                        return this.getMetaData(wijmo.chart.DataLabelBase).add([
                            this.CreateProp('position', PropertyType.Enum, BindingMode.OneWay, wijmo.chart.PieLabelPosition)
                        ]);

                    case wijmo.chart && wijmo.chart.Series:
                        return new MetaDataBase([
                            this.CreateProp('binding', PropertyType.String),
                            this.CreateProp('bindingX', PropertyType.String),
                            this.CreateProp('chartType', PropertyType.Enum, BindingMode.OneWay, wijmo.chart.ChartType),
                            this.CreateProp('cssClass', PropertyType.String),
                            this.CreateProp('name', PropertyType.String),
                            this.CreateProp('style', PropertyType.Any),
                            this.CreateProp('symbolMarker', PropertyType.Enum, BindingMode.OneWay, wijmo.chart.Marker),
                            this.CreateProp('symbolSize', PropertyType.Number),
                            this.CreateProp('symbolStyle', PropertyType.Any),
                            this.CreateProp('visibility', PropertyType.Enum, BindingMode.TwoWay, wijmo.chart.SeriesVisibility),
                            this.CreateProp('itemsSource', PropertyType.Any)
                        ], [], [
                            this.CreateComplexProp('axisX', false, true),
                            this.CreateComplexProp('axisY', false, true)
                        ], 'series', true);

                    case wijmo.chart && wijmo.chart.LineMarker:
                        return new MetaDataBase([
                            this.CreateProp('isVisible', PropertyType.Boolean),
                            this.CreateProp('seriesIndex', PropertyType.Number),
                            this.CreateProp('horizontalPosition', PropertyType.Number),
                            this.CreateProp('content', PropertyType.Function),
                            this.CreateProp('verticalPosition', PropertyType.Number),
                            this.CreateProp('alignment', PropertyType.Enum, BindingMode.OneWay, wijmo.chart.LineMarkerAlignment),
                            this.CreateProp('lines', PropertyType.Enum, BindingMode.OneWay, wijmo.chart.LineMarkerLines),
                            this.CreateProp('interaction', PropertyType.Enum, BindingMode.OneWay, wijmo.chart.LineMarkerInteraction),
                            this.CreateProp('dragLines', PropertyType.Boolean),
                            this.CreateProp('dragThreshold', PropertyType.Number),
                            this.CreateProp('dragContent', PropertyType.Boolean)
                        ], [
                            this.CreateEvent('positionChanged')
                        ], [], undefined, undefined, undefined, '');

                    case wijmo.chart && wijmo.chart.interaction && wijmo.chart.interaction.RangeSelector:
                        return new MetaDataBase([
                            this.CreateProp('isVisible', PropertyType.Boolean),
                            this.CreateProp('min', PropertyType.Number),
                            this.CreateProp('max', PropertyType.Number),
                            this.CreateProp('orientation', PropertyType.Enum, BindingMode.OneWay, wijmo.chart.interaction.Orientation)
                        ], [
                            this.CreateEvent('rangeChanged')
                        ], [], undefined, undefined, undefined, '');

                    case wijmo.gauge && wijmo.gauge.Gauge:
                        return new MetaDataBase([
                            this.CreateProp('value', PropertyType.Number, BindingMode.TwoWay),
                            this.CreateProp('min', PropertyType.Number),
                            this.CreateProp('max', PropertyType.Number),
                            this.CreateProp('origin', PropertyType.Number),
                            this.CreateProp('isReadOnly', PropertyType.Boolean),
                            this.CreateProp('step', PropertyType.Number),
                            this.CreateProp('format', PropertyType.String),
                            this.CreateProp('thickness', PropertyType.Number),
                            this.CreateProp('hasShadow', PropertyType.Boolean),
                            this.CreateProp('isAnimated', PropertyType.Boolean),
                            this.CreateProp('showText', PropertyType.Enum, BindingMode.OneWay, wijmo.gauge.ShowText),
                            this.CreateProp('showRanges', PropertyType.Boolean)
                        ], [
                            this.CreateEvent('valueChanged', true)
                        ], [
                            this.CreateComplexProp('ranges', true),
                            this.CreateComplexProp('pointer', false, false),
                            this.CreateComplexProp('face', false, false)
                        ]).addOptions({ ngModelProperty: 'value' });

                    case wijmo.gauge && wijmo.gauge.LinearGauge:
                        return this.getMetaData(wijmo.gauge.Gauge).add([
                            this.CreateProp('direction', PropertyType.Enum, BindingMode.OneWay, wijmo.gauge.GaugeDirection)
                        ]);

                    case wijmo.gauge && wijmo.gauge.BulletGraph:
                        return this.getMetaData(wijmo.gauge.LinearGauge).add([
                            this.CreateProp('target', PropertyType.Number),
                            this.CreateProp('good', PropertyType.Number),
                            this.CreateProp('bad', PropertyType.Number)
                        ]);

                    case wijmo.gauge && wijmo.gauge.RadialGauge:
                        return this.getMetaData(wijmo.gauge.Gauge).add([
                            this.CreateProp('autoScale', PropertyType.Boolean),
                            this.CreateProp('startAngle', PropertyType.Number),
                            this.CreateProp('sweepAngle', PropertyType.Number)
                        ]);

                    case wijmo.gauge && wijmo.gauge.Range:
                        return new MetaDataBase([
                            this.CreateProp('color', PropertyType.String),
                            this.CreateProp('min', PropertyType.Number),
                            this.CreateProp('max', PropertyType.Number),
                            this.CreateProp('name', PropertyType.String),
                            this.CreateProp('thickness', PropertyType.Number)
                        ], [], [], 'ranges', true);
                }

                return new MetaDataBase([]);
            };

            // For the specified class reference returns its name as a string, e.g.
            // getClassName(wijmo.input.ComboBox) returns 'ComboBox'.
            ControlMetaFactory.getClassName = function (classRef) {
                return (classRef.toString().match(/function (.+?)\(/) || [, ''])[1];
            };

            // Returns a camel case representation of the dash delimited name.
            ControlMetaFactory.toCamelCase = function (s) {
                return s.toLowerCase().replace(/-(.)/g, function (match, group1) {
                    return group1.toUpperCase();
                });
            };

            ControlMetaFactory.findInArr = function (arr, propName, value) {
                for (var i in arr) {
                    if (arr[i][propName] === value) {
                        return arr[i];
                    }
                }
                return null;
            };
            return ControlMetaFactory;
        })();
        interop.ControlMetaFactory = ControlMetaFactory;

        // Describes a scope property: name, type, binding mode.
        // Also defines enum type and custom watcher function extender
        var PropDescBase = (function () {
            // Initializes a new instance of a PropDesc
            function PropDescBase(propertyName, propertyType, bindingMode, enumType, isNativeControlProperty, priority) {
                if (typeof bindingMode === "undefined") { bindingMode = BindingMode.OneWay; }
                if (typeof isNativeControlProperty === "undefined") { isNativeControlProperty = true; }
                if (typeof priority === "undefined") { priority = 0; }
                this._priority = 0;
                this._propertyName = propertyName;
                this._propertyType = propertyType;
                this._bindingMode = bindingMode;
                this._enumType = enumType;
                this._isNativeControlProperty = isNativeControlProperty;
                this._priority = priority;
            }
            Object.defineProperty(PropDescBase.prototype, "propertyName", {
                // Gets the property name
                get: function () {
                    return this._propertyName;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(PropDescBase.prototype, "propertyType", {
                // Gets the property type (number, string, boolean, enum, or any)
                get: function () {
                    return this._propertyType;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(PropDescBase.prototype, "enumType", {
                // Gets the property enum type
                get: function () {
                    return this._enumType;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(PropDescBase.prototype, "bindingMode", {
                // Gets the property binding mode
                get: function () {
                    return this._bindingMode;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(PropDescBase.prototype, "isNativeControlProperty", {
                // Gets whether the property belongs to the control is just to the directive
                get: function () {
                    return this._isNativeControlProperty;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(PropDescBase.prototype, "priority", {
                // Gets an initialization priority. Properties with higher priority are assigned to directive's underlying control
                // property later than properties with lower priority. Properties with the same priority are assigned in the order of
                // their index in the _props collection.
                get: function () {
                    return this._priority;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(PropDescBase.prototype, "shouldUpdateSource", {
                // Indicates whether a bound 'controller' property should be updated on this property change (i.e. two-way binding).
                get: function () {
                    return this.bindingMode === BindingMode.TwoWay && this.propertyType != PropertyType.EventHandler;
                },
                enumerable: true,
                configurable: true
            });

            PropDescBase.prototype.initialize = function (options) {
                wijmo.copy(this, options);
            };
            return PropDescBase;
        })();
        interop.PropDescBase = PropDescBase;

        // Property types as used in the PropDesc class.
        (function (PropertyType) {
            PropertyType[PropertyType["Boolean"] = 0] = "Boolean";
            PropertyType[PropertyType["Number"] = 1] = "Number";
            PropertyType[PropertyType["Date"] = 2] = "Date";
            PropertyType[PropertyType["String"] = 3] = "String";
            PropertyType[PropertyType["Enum"] = 4] = "Enum";
            PropertyType[PropertyType["Function"] = 5] = "Function";
            PropertyType[PropertyType["EventHandler"] = 6] = "EventHandler";
            PropertyType[PropertyType["Any"] = 7] = "Any";
        })(interop.PropertyType || (interop.PropertyType = {}));
        var PropertyType = interop.PropertyType;

        // Gets a value indicating whether the specified type is simple (true) or complex (false).
        function isSimpleType(type) {
            return type <= PropertyType.Enum;
        }
        interop.isSimpleType = isSimpleType;

        (function (BindingMode) {
            BindingMode[BindingMode["OneWay"] = 0] = "OneWay";
            BindingMode[BindingMode["TwoWay"] = 1] = "TwoWay";
        })(interop.BindingMode || (interop.BindingMode = {}));
        var BindingMode = interop.BindingMode;

        // Describes a scope event
        var EventDescBase = (function () {
            // Initializes a new instance of an EventDesc
            function EventDescBase(eventName, isPropChanged) {
                this._eventName = eventName;
                this._isPropChanged = isPropChanged;
            }
            Object.defineProperty(EventDescBase.prototype, "eventName", {
                // Gets the event name
                get: function () {
                    return this._eventName;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(EventDescBase.prototype, "isPropChanged", {
                // Gets whether this event is a property change notification
                get: function () {
                    return this._isPropChanged === true;
                },
                enumerable: true,
                configurable: true
            });
            return EventDescBase;
        })();
        interop.EventDescBase = EventDescBase;

        // Describe property info for nested directives.
        var ComplexPropDescBase = (function () {
            function ComplexPropDescBase(propertyName, isArray, ownsObject) {
                if (typeof ownsObject === "undefined") { ownsObject = false; }
                this.isArray = false;
                this._ownsObject = false;
                this.propertyName = propertyName;
                this.isArray = isArray;
                this._ownsObject = ownsObject;
            }
            Object.defineProperty(ComplexPropDescBase.prototype, "ownsObject", {
                get: function () {
                    return this.isArray || this._ownsObject;
                },
                enumerable: true,
                configurable: true
            });
            return ComplexPropDescBase;
        })();
        interop.ComplexPropDescBase = ComplexPropDescBase;

        // Stores a control metadata as arrays of property, event and complex property descriptors.
        var MetaDataBase = (function () {
            function MetaDataBase(props, events, complexProps, parentProperty, isParentPropertyArray, ownsObject, parentReferenceProperty, ngModelProperty) {
                this._props = [];
                this._events = [];
                this._complexProps = [];
                this.props = props;
                this.events = events;
                this.complexProps = complexProps;
                this.parentProperty = parentProperty;
                this.isParentPropertyArray = isParentPropertyArray;
                this.ownsObject = ownsObject;
                this.parentReferenceProperty = parentReferenceProperty;
                this.ngModelProperty = ngModelProperty;
            }
            Object.defineProperty(MetaDataBase.prototype, "props", {
                get: function () {
                    return this._props;
                },
                set: function (value) {
                    this._props = value || [];
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(MetaDataBase.prototype, "events", {
                get: function () {
                    return this._events;
                },
                set: function (value) {
                    this._events = value || [];
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(MetaDataBase.prototype, "complexProps", {
                get: function () {
                    return this._complexProps;
                },
                set: function (value) {
                    this._complexProps = value || [];
                },
                enumerable: true,
                configurable: true
            });

            // Adds the specified arrays to the end of corresponding arrays of this object, and overwrite the simple properties
            // if specified. Returns 'this'.
            MetaDataBase.prototype.add = function (props, events, complexProps, parentProperty, isParentPropertyArray, ownsObject, parentReferenceProperty, ngModelProperty) {
                return this.addOptions({
                    props: props,
                    events: events,
                    complexProps: complexProps,
                    parentProperty: parentProperty,
                    isParentPropertyArray: isParentPropertyArray,
                    ownsObject: ownsObject,
                    parentReferenceProperty: parentReferenceProperty,
                    ngModelProperty: ngModelProperty
                });
                //this._props = this._props.concat(props || []);
                //this._events = this._events.concat(events || []);
                //this._complexProps = this._complexProps.concat(complexProps || []);
                //if (parentProperty !== undefined) {
                //    this.parentProperty = parentProperty;
                //}
                //if (isParentPropertyArray !== undefined) {
                //    this.isParentPropertyArray = isParentPropertyArray;
                //}
                //if (ownsObject !== undefined) {
                //    this.ownsObject = ownsObject;
                //}
                //if (parentReferenceProperty !== undefined) {
                //    this.parentReferenceProperty = parentReferenceProperty;
                //}
                //if (ngModelProperty !== undefined) {
                //    this.ngModelProperty = ngModelProperty;
                //}
                //return this;
            };

            MetaDataBase.prototype.addOptions = function (options) {
                for (var prop in options) {
                    var thisValue = this[prop], optionsValue = options[prop];
                    if (thisValue instanceof Array) {
                        this[prop] = thisValue.concat(optionsValue || []);
                    } else if (optionsValue !== undefined) {
                        this[prop] = optionsValue;
                    }
                }
                return this;
            };

            // Prepares a raw defined metadata for a usage, for exmple sorts the props array on priority.
            MetaDataBase.prototype.prepare = function () {
                // stable sort of props on priority
                var baseArr = [].concat(this._props);
                this._props.sort(function (a, b) {
                    var ret = a.priority - b.priority;
                    if (!ret) {
                        ret = baseArr.indexOf(a) - baseArr.indexOf(b);
                    }
                    return ret;
                });
            };
            return MetaDataBase;
        })();
        interop.MetaDataBase = MetaDataBase;
    })(wijmo.interop || (wijmo.interop = {}));
    var interop = wijmo.interop;
})(wijmo || (wijmo = {}));
var wijmo;
(function (wijmo) {
    (function (angular) {
        var MetaFactory = (function (_super) {
            __extends(MetaFactory, _super);
            function MetaFactory() {
                _super.apply(this, arguments);
            }
            // Override to return wijmo.angular.PropDesc
            MetaFactory.CreateProp = function (propertyName, propertyType, bindingMode, enumType, isNativeControlProperty, priority) {
                return new PropDesc(propertyName, propertyType, bindingMode, enumType, isNativeControlProperty, priority);
            };

            // Override to return wijmo.angular.EventDesc
            MetaFactory.CreateEvent = function (eventName, isPropChanged) {
                return new EventDesc(eventName, isPropChanged);
            };

            // Override to return wijmo.angular.ComplexPropDesc
            MetaFactory.CreateComplexProp = function (propertyName, isArray, ownsObject) {
                return new ComplexPropDesc(propertyName, isArray, ownsObject);
            };

            // Typecast override.
            MetaFactory.findProp = function (propName, props) {
                return wijmo.interop.ControlMetaFactory.findProp(propName, props);
            };

            // Typecast override.
            MetaFactory.findEvent = function (eventName, events) {
                return wijmo.interop.ControlMetaFactory.findEvent(eventName, events);
            };

            // Typecast override.
            MetaFactory.findComplexProp = function (propName, props) {
                return wijmo.interop.ControlMetaFactory.findComplexProp(propName, props);
            };
            return MetaFactory;
        })(wijmo.interop.ControlMetaFactory);
        angular.MetaFactory = MetaFactory;

        // Describes a scope property: name, type, binding mode.
        // Also defines enum type and custom watcher function extender
        var PropDesc = (function (_super) {
            __extends(PropDesc, _super);
            // Initializes a new instance of a PropDesc
            function PropDesc(propertyName, propertyType, bindingMode, enumType, isNativeControlProperty, priority) {
                _super.call(this, propertyName, propertyType, bindingMode, enumType, isNativeControlProperty, priority);

                this._scopeBindingMode = this.propertyType === wijmo.interop.PropertyType.EventHandler ? '&' : (this.bindingMode == wijmo.interop.BindingMode.OneWay && wijmo.interop.isSimpleType(this.propertyType) ? '@' : '=');
            }
            Object.defineProperty(PropDesc.prototype, "scopeBindingMode", {
                // Gets or sets the property binding mode ('@' - by val, '=' - by ref, '&' - expression)
                get: function () {
                    return this._scopeBindingMode;
                },
                set: function (value) {
                    this._scopeBindingMode = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(PropDesc.prototype, "customHandler", {
                // Gets related property custom watcher function extender
                get: function () {
                    return this._customHandler;
                },
                set: function (value) {
                    this._customHandler = value;
                },
                enumerable: true,
                configurable: true
            });
            return PropDesc;
        })(wijmo.interop.PropDescBase);
        angular.PropDesc = PropDesc;

        // Describes a scope event
        var EventDesc = (function (_super) {
            __extends(EventDesc, _super);
            function EventDesc() {
                _super.apply(this, arguments);
            }
            return EventDesc;
        })(wijmo.interop.EventDescBase);
        angular.EventDesc = EventDesc;

        // Describes property info for nested directives.
        var ComplexPropDesc = (function (_super) {
            __extends(ComplexPropDesc, _super);
            function ComplexPropDesc() {
                _super.apply(this, arguments);
            }
            return ComplexPropDesc;
        })(wijmo.interop.ComplexPropDescBase);
        angular.ComplexPropDesc = ComplexPropDesc;
    })(wijmo.angular || (wijmo.angular = {}));
    var angular = wijmo.angular;
})(wijmo || (wijmo = {}));

var wijmo;
(function (wijmo) {
    //
    // AngularJS base directive class
    //
    (function (angular) {
        // Base class for AngularJS directives (abstract class).
        var WjDirective = (function () {
            // Initializes a new instance of the DirectiveBase class
            function WjDirective() {
                // Tells directive to replace or not original tag with the template
                this.replace = true;
                // Defines the way directive can be used in HTML
                this.restrict = 'E';
                // Defines directive's template
                this.template = '<div />';
                // Tells directive to move content into template element marked with
                // 'ng-transclude' attribute
                this.transclude = false;
                //#endregion Settings
                // Directive property map
                // Holds PropDesc[] array with Wijmo control's properties available in directive's scope
                this._props = [];
                // Directive events map
                // Holds EventDesc[] array with Wijmo control's events available in directive's scope
                this._events = [];
                // Property descriptions used by nested directives.
                this._complexProps = [];
                var self = this;
                this._dirId = (++WjDirective._dirIdCounter) + '';
                this.link = this._postLinkFn();
                this.controller = [
                    '$scope', '$parse', '$element', function ($scope, $parse, $element) {
                        // 'this' points to the controller instance here
                        self._$parse = $parse;
                        this[WjDirective._cntrlScopeProp] = $scope;
                        $scope[WjDirective._scopeChildrenProp] = [];
                        self._controllerImpl(this, $scope, $element);
                    }];

                this._initDirective();
            }
            Object.defineProperty(WjDirective.prototype, "_controlConstructor", {
                // Gets the constructor for the related Wijmo control.
                // Abstract member, must be overridden in inherited class
                get: function () {
                    throw 'Abstract method call';
                },
                enumerable: true,
                configurable: true
            });

            // Gets the metadata ID, see the wijmo.interop.getMetaData method description for details.
            WjDirective.prototype._getMetaDataId = function () {
                return this._controlConstructor;
            };

            // Gets directive metadata.
            WjDirective.prototype._getMetaData = function () {
                return angular.MetaFactory.getMetaData(this._getMetaDataId());
            };

            // Initializes DDO properties
            WjDirective.prototype._initDirective = function () {
                this._initSharedMeta();
                this._prepareProps();
                this._initEvents();
                this._initScopeEvents();
                this._initScopeDescription();
            };

            // Initialize _props, _events and _complexProps with the shared metadata from wijmo.interop.ControlMetaFactory.
            WjDirective.prototype._initSharedMeta = function () {
                var meta = this._getMetaData();
                this._props = meta.props;
                this._events = meta.events;
                this._complexProps = meta.complexProps;
                this._property = meta.parentProperty;
                this._isPropertyArray = meta.isParentPropertyArray;
                this._ownObject = meta.ownsObject;
                this._parentReferenceProperty = meta.parentReferenceProperty;
                this._ngModelProperty = meta.ngModelProperty;
            };

            // Initializes control's property map. Abstract member, must be overridden in inherited class
            WjDirective.prototype._initProps = function () {
            };

            // Initializes control's event map. Abstract member, must be overridden in inherited class
            WjDirective.prototype._initEvents = function () {
            };

            // Creates and returns WjLink instance pertain to the directive.
            WjDirective.prototype._createLink = function () {
                return new WjLink();
            };

            // Implements a controller body, override it to implement a custom controller logic.
            // controller - a pointer to controller object.
            // scope - controller (and corresponding WjLink) scope.
            //
            // The DDO.controller property is occupied by our wrapper that creates a controller with the _cntrlScope property assigned
            // to the controller's scope. The wrapper then calls this method that is intended to implement a custom controller logic.
            WjDirective.prototype._controllerImpl = function (controller, scope, tElement) {
            };

            // Initializes control owned by the directive
            WjDirective.prototype._initControl = function (element) {
                try  {
                    var controlConstructor = this._controlConstructor;
                    var control = new controlConstructor(element);
                    return control;
                } catch (e) {
                    // Do nothing. Return 'undefined' explicitly
                    return undefined;
                }
            };

            // Indicates whether this directictive can operate as a child directictive.
            WjDirective.prototype._isChild = function () {
                return this._isParentInitializer() || this._isParentReferencer();
            };

            // Indicates whether this directictive operates as a child directictive that initializes a property of its parent.
            WjDirective.prototype._isParentInitializer = function () {
                return this._property != undefined;
            };

            // Indicates whether this directictive operates as a child directictive that references a parent in its property or
            // a constructor.
            WjDirective.prototype._isParentReferencer = function () {
                return this._parentReferenceProperty != undefined;
            };

            // For the specified scope/control property name returns its corresponding directive tag attribute name.
            WjDirective.prototype._scopeToAttrName = function (scopeName) {
                var alias = this.scope[scopeName];
                if (alias) {
                    var bindMarkLen = 1, aliasLen = alias.length;
                    if (aliasLen < 2) {
                        return scopeName;
                    }
                    if (alias.charAt(1) === '?') {
                        bindMarkLen = 2;
                    }
                    if (aliasLen === bindMarkLen) {
                        return scopeName;
                    }
                    return alias.substr(bindMarkLen);
                }
                return scopeName;
            };

            WjDirective.prototype._getComplexPropDesc = function (propName) {
                return angular.MetaFactory.findComplexProp(propName, this._complexProps);
            };

            // Extends control's property map with events
            // Do not confuse with _initEvents(), which is abstract.
            WjDirective.prototype._initScopeEvents = function () {
                for (var i in this._events) {
                    var event = this._events[i];
                    this._props.push(new angular.PropDesc(event.eventName, wijmo.interop.PropertyType.EventHandler));
                }
            };

            // Creates isolated scope based on directive property map
            WjDirective.prototype._initScopeDescription = function () {
                var props = this._props, scope = {}, byRefMark = WjDirective._optionalAttr ? '=?' : '=';

                // fill result object with control properties
                if (props != null) {
                    var prop;
                    for (var i = 0; i < props.length; i++) {
                        prop = props[i];
                        scope[prop.propertyName] = prop.scopeBindingMode;

                        //1.1.1
                        if (WjDirective._optionalAttr && prop.scopeBindingMode == '=')
                            scope[prop.propertyName] = '=?';
                    }
                }

                // add property for control
                scope['control'] = byRefMark;
                scope[WjDirective._initPropAttr] = byRefMark;
                scope[WjDirective._initEventAttr] = '&';
                scope[WjDirective._parPropAttr] = '@';
                scope[WjDirective._wjModelPropAttr] = '@';

                // save result
                this.scope = scope;
            };

            // Returns the directive's 'link' function.
            // This is a virtual method, can be overridden by derived classes.
            // @param beforeLinkDelegate Delegate to run before the link function
            // @param afterLinkDelegate Delegate to run after the link function
            // @return Directive's link function
            WjDirective.prototype._postLinkFn = function () {
                var self = this;

                // Final directive link function
                var linkFunction = function (scope, tElement, tAttrs, controller) {
                    var link = self._createLink();
                    link.directive = self;
                    link.scope = scope;
                    link.tElement = tElement;
                    link.tAttrs = tAttrs;

                    if (wijmo.isArray(controller)) {
                        var parEl = tElement.parent();

                        // If working Angular version supports the isolateScope function then we use it, because in this case
                        // the scope function returns a non-isolated scope; otherwise we use scope that returns an isolated scope
                        // in this case.
                        var scopeFunc = parEl.isolateScope || parEl.scope;
                        var parScope = scopeFunc.call(parEl);
                        for (var i in controller) {
                            var curCntrl = controller[i];
                            if (curCntrl != undefined) {
                                //if (!link.controller) {
                                if (curCntrl[WjDirective._cntrlScopeProp] === scope) {
                                    //require parent controller by name
                                    curCntrl = tElement.parent().controller(self._stripRequire(i));
                                }
                                if (curCntrl && curCntrl[WjDirective._cntrlScopeProp] === parScope) {
                                    link.controller = curCntrl;
                                    break;
                                    //continue;
                                }
                                //}
                            }
                        }
                    } else {
                        link.controller = controller;
                    }

                    link.ngModel = tElement.controller('ngModel');

                    link._link();
                };
                return linkFunction;
            };

            // Gathers PropertyDesc(s) and sorts them (using stable sort) in a priority order.
            WjDirective.prototype._prepareProps = function () {
                // gather property descriptors
                this._initProps();

                // stable sort on priority
                var baseArr = [].concat(this._props);
                this._props.sort(function (a, b) {
                    var ret = a.priority - b.priority;
                    if (!ret) {
                        ret = baseArr.indexOf(a) - baseArr.indexOf(b);
                    }
                    return ret;
                });
            };

            // For the 'require' property represented by an array, returns its value at the specified index stripped of a leading specifier.
            WjDirective.prototype._stripRequire = function (index) {
                if (!this._stripReq) {
                    this._stripReq = [];
                    this._stripReq.length = this['require'].length;
                }
                if (!this._stripReq[index]) {
                    var patt = /^[^A-Za-z]*(.*)/;
                    var res = patt.exec(this['require'][index]);
                    this._stripReq[index] = res ? res[1] : '';
                }
                return this._stripReq[index];
            };

            // Gets a directive unique ID
            WjDirective.prototype._getId = function () {
                return this._dirId;
            };

            // Determines whether the specified version is not older than the current Angular version.
            WjDirective._versionOk = function (minVer) {
                var angVer = window['angular'].version;
                var angVerParts = [angVer.major, angVer.minor, angVer.dot];
                var verParts = minVer.split(".");
                if (verParts.length !== angVerParts.length)
                    throw 'Unrecognizable version number.';
                for (var i = 0; i < verParts.length; i++) {
                    if (angVerParts[i] < verParts[i]) {
                        return false;
                    } else if (angVerParts[i] > verParts[i]) {
                        return true;
                    }
                }

                return true;
            };
            WjDirective._parPropAttr = 'wjProperty';

            WjDirective._wjModelPropAttr = 'wjModelProperty';

            WjDirective._initPropAttr = 'isInitialized';

            WjDirective._initEventAttr = 'initialized';

            WjDirective._cntrlScopeProp = '_cntrlScope';

            WjDirective._cntrlLinkProp = '$_thisLink';

            WjDirective._scopeChildrenProp = '$_childLinks';

            WjDirective._dirIdAttr = 'wj-directive-id';

            WjDirective._optionalAttr = WjDirective._versionOk("1.1.4");

            WjDirective._dynaTemplates = WjDirective._optionalAttr;

            WjDirective._angStripPrefixes = ['data', 'x'];
            WjDirective._dirIdCounter = 0;
            return WjDirective;
        })();
        angular.WjDirective = WjDirective;

        var WjLink = (function () {
            function WjLink() {
                // Hash containing <property name> - true pairs for scope properties that can't be assigned.
                this._nonAssignable = {};
                // Hash containing <property name> - PropDesc pairs for all properties that have defined tag attributes.
                this._definedProps = {};
                // Hash containing <event name> - EventDesc pairs for all events that have defined tag attributes.
                this._definedEvents = {};
                // Hash containing <property name> - any pairs containing previous scope values for the $watch function.
                this._oldValues = {};
                /* private */ this._isInitialized = false;
                this._scopeSuspend = 0;
                this._suspendedEvents = [];
                this._areChlildrenReady = false;
                // #region 'initialized' stuff
                this._isAppliedToParent = false;
            }
            WjLink.prototype._link = function () {
                var dir = this.directive;
                this.tElement[0].setAttribute(WjDirective._dirIdAttr, dir._getId());
                this.directiveTemplateElement = dir.replace ? this.tElement : window['angular'].element(this.tElement.children()[0]);
                this._initNonAssignable();
                if (this._isChild()) {
                    //Defines initial _parentPropDesc, which can be overridden later in the _parentReady method.
                    this._parentPropDesc = new angular.ComplexPropDesc(dir._property, dir._isPropertyArray, dir._ownObject);

                    // Register this link as a child in the parent link's scope and postpone initialization
                    this.controller[WjDirective._cntrlScopeProp][WjDirective._scopeChildrenProp].push(this);

                    var parentScope = this.controller[WjDirective._cntrlScopeProp], parentLink = parentScope[WjDirective._cntrlLinkProp];
                    if (parentLink && parentLink._areChlildrenReady) {
                        this._parentReady(parentLink);
                    }
                } else {
                    this._createInstance();
                    this._notifyReady();
                    this._prepareControl();
                }
            };

            // This method can be overridden to implement custom application of child directives. Child directives are already
            // initialized at this moment.
            WjLink.prototype._onChildrenReady = function () {
            };

            WjLink.prototype._createInstance = function () {
                this.control = this._initControl();
                this._safeApply(this.scope, 'control', this.control);
            };

            // This method is called by the parent link for the child link to notify that parent link's control is created.
            WjLink.prototype._parentReady = function (parentLink) {
                if (!this._isChild())
                    return;
                var self = this;

                // In case where parent property name is defined via attribute by a user, in early Angular versions (e.g. 1.1.1)
                // the scope is not initialized with attribute values defined on directive tag. To manage this we watch this attribute
                // and init the link when its value appears.
                if (this._isAttrDefined(WjDirective._parPropAttr) && !this.scope[WjDirective._parPropAttr]) {
                    this.scope.$watch(WjDirective._parPropAttr, function () {
                        self._parentReady(parentLink);
                    });
                    return;
                }
                var parProp = this._getParentProp();

                //Override _parentPropDesc if it's defined for the servicing property in the parent link's directive.
                var parPropDescOverride = parentLink.directive._getComplexPropDesc(parProp);
                if (parPropDescOverride) {
                    this._parentPropDesc = parPropDescOverride;
                } else {
                    this._parentPropDesc.propertyName = parProp;
                }
                this.parent = parentLink;
                if (this._useParentObj()) {
                    this.control = parentLink.control[parProp];
                    this._safeApply(this.scope, 'control', this.control);
                } else {
                    this._createInstance();
                }
                this._notifyReady();
                this._prepareControl();
                this._initParent();
                this.directiveTemplateElement[0].style.display = 'none';
                this._appliedToParent();

                this.scope.$on('$destroy', function (event) {
                    var args = [];
                    for (var _i = 0; _i < (arguments.length - 1); _i++) {
                        args[_i] = arguments[_i + 1];
                    }
                    self._destroy();
                });
            };

            // Assigns/adds this directive's object to the parent object.
            WjLink.prototype._initParent = function () {
                if (this._useParentObj())
                    return;
                var dir = this.directive, propName = this._getParentProp(), parCtrl = this.parent.control, ctrl = this.control;
                if (this._isParentInitializer()) {
                    if (this._isParentArray()) {
                        // insert child at correct index, which is the same as an index of the directive element amid sibling directives
                        // of the same type
                        var parArr = parCtrl[propName], linkIdx = this._getIndex();
                        if (linkIdx < 0 || linkIdx >= parArr.length) {
                            linkIdx = parArr.length;
                        }
                        parArr.splice(linkIdx, 0, ctrl);
                        var self = this;
                        this._siblingInsertedEH = this._siblingInserted.bind(this);
                        this.tElement[0].addEventListener('DOMNodeInserted', this._siblingInsertedEH);
                    } else {
                        parCtrl[propName] = ctrl;
                    }
                }
                if (this._isParentReferencer() && !this._parentInCtor()) {
                    ctrl[this._getParentReferenceProperty()] = parCtrl;
                }
            };

            // Performes directive removal (currently called for child directives only).
            WjLink.prototype._destroy = function () {
                if (this._siblingInsertedEH) {
                    this.tElement[0].removeEventListener('DOMNodeInserted', this._siblingInsertedEH);
                }
                if (this._isParentArray()) {
                    var parControl = this.parent.control, parProp = this._getParentProp(), control = this.control;

                    if (parControl && parProp && control) {
                        var parArr = parControl[parProp];
                        if (parArr) {
                            var idx = parArr.indexOf(control);
                            if (idx >= 0) {
                                parArr.splice(idx, 1);
                            }
                        }
                    }
                }
            };

            WjLink.prototype._siblingInserted = function (e) {
                if (e.target === this.tElement[0]) {
                    var lIdx = this._getIndex(), parArr = this.parent.control[this._getParentProp()], ctrl = this.control, arrIdx = parArr.indexOf(ctrl);
                    if (lIdx >= 0 && arrIdx >= 0 && lIdx !== arrIdx) {
                        parArr.splice(arrIdx, 1);
                        lIdx = Math.min(lIdx, parArr.length);
                        parArr.splice(lIdx, 0, ctrl);
                    }
                }
            };

            // Notify child links after this directive was attached to its control.
            WjLink.prototype._notifyReady = function () {
                // Notify child links
                //
                this.scope[WjDirective._cntrlLinkProp] = this;

                //
                var childLinks = [].concat(this.scope[WjDirective._scopeChildrenProp]);
                for (var i = 0; i < childLinks.length; i++) {
                    childLinks[i]._parentReady(this);
                }

                // Clear children list to free references for GC.
                //childLinks.length = 0; //cleared one by one by the _childInitialized method
                this._areChlildrenReady = true;

                this._onChildrenReady();
            };

            // Creates a control instance owned by the directive.
            WjLink.prototype._initControl = function () {
                return this.directive._initControl(this._parentInCtor() ? this.parent.control : this.directiveTemplateElement[0]);
            };

            // Defines scope's default values, registers properties watchers and event handlers
            WjLink.prototype._prepareControl = function () {
                this._addEventHandlers();
                this._addWatchers();
            };

            // Sets control's default values to scope properties
            WjLink.prototype._setupScopeWithControlProperties = function () {
                var prop, name, scopeValue, controlValue, control = this.control, scope = this.scope, props = this.directive._props;

                for (var i = 0; i < props.length; i++) {
                    prop = props[i];
                    if (prop.scopeBindingMode === '=' && prop.isNativeControlProperty) {
                        name = prop.propertyName;
                        scopeValue = scope[name];
                        controlValue = control[name];

                        var isFunction = prop.propertyType == wijmo.interop.PropertyType.Function;
                        var isEventHandler = prop.propertyType == wijmo.interop.PropertyType.EventHandler;

                        if (this._canApply(scope, prop.propertyName) && controlValue != scopeValue && !isFunction && !isEventHandler) {
                            scope[prop.propertyName] = controlValue;
                        }
                    }
                }

                if (!scope['$root'].$$phase) {
                    scope.$apply();
                }
            };

            WjLink.prototype._initNonAssignable = function () {
                var parse = this.directive._$parse, scopeDef = this.directive.scope, binding;
                for (var name in scopeDef) {
                    if (scopeDef[name].charAt(0) === '=') {
                        binding = this.tAttrs[this.directive._scopeToAttrName(name)];
                        if (binding === undefined || parse(binding).assign == undefined) {
                            this._nonAssignable[name] = true;
                        }
                    }
                }
            };

            WjLink.prototype._suspendScope = function () {
                this._scopeSuspend++;
            };

            WjLink.prototype._resumeScope = function () {
                if (this._scopeSuspend > 0) {
                    if (--this._scopeSuspend === 0 && this._suspendedEvents.length > 0) {
                        this._updateScope();
                    }
                }
            };

            WjLink.prototype._isScopeSuspended = function () {
                return this._scopeSuspend > 0;
            };

            WjLink.prototype._isAttrDefined = function (name) {
                return this.tAttrs.hasOwnProperty(this.directive._scopeToAttrName(name));
            };

            // Called by child link when its fully initialized
            WjLink.prototype._childInitialized = function (child) {
                var childLinks = this.scope[WjDirective._scopeChildrenProp], idx = childLinks.indexOf(child);
                if (idx >= 0) {
                    childLinks.splice(idx, 1);
                    this._checkRaiseInitialized();
                }
            };

            // Called after first watch on this links has worked out.
            WjLink.prototype._initialized = function () {
                this._checkRaiseInitialized();
            };

            // For the child link, called after this link has applied (added to array, assigned) its object to the parent.
            WjLink.prototype._appliedToParent = function () {
                this._isAppliedToParent = true;
                this._checkRaiseInitialized();
            };

            WjLink.prototype._checkRaiseInitialized = function () {
                if (this.scope[WjDirective._scopeChildrenProp].length === 0 && this._isInitialized && (!this._isChild() || this._isAppliedToParent)) {
                    // set the scope isInitialized property to true
                    this._safeApply(this.scope, WjDirective._initPropAttr, true);

                    // raise the initialized event
                    var handler = this.scope[WjDirective._initEventAttr], self = this;
                    if (handler) {
                        // delay the event to allow the 'isInitialized' property value be propagated to a controlles scope before
                        // the event is raised
                        setTimeout(function () {
                            handler({ s: self.control, e: undefined });
                        }, 0);
                    }

                    //notify parent
                    if (this._isChild() && this.parent) {
                        this.parent._childInitialized(this);
                    }
                }
            };

            //#endregion 'initialized' stuff
            // Adds watchers for scope properties to update control values
            WjLink.prototype._addWatchers = function () {
                var self = this, props = this.directive._props, scope = this.scope;
                if (!props) {
                    return;
                }

                if (this.ngModel) {
                    var ngModel = this.ngModel;

                    // Patch: in Angular 1.3+ these classes are initially set but then removed by Angular,
                    // probably because directive's replace=true ???
                    if (ngModel.$pristine) {
                        wijmo.addClass(this.tElement[0], 'ng-pristine');
                    }
                    if (ngModel.$valid) {
                        wijmo.addClass(this.tElement[0], 'ng-valid');
                    }
                    if (ngModel.$untouched) {
                        wijmo.addClass(this.tElement[0], 'ng-untouched');
                    }

                    // end patch
                    ngModel.$render = this._ngModelRender.bind(this);
                    this._updateNgModelPropDesc();
                    if (this._isAttrDefined(WjDirective._wjModelPropAttr)) {
                        scope.$watch(WjDirective._wjModelPropAttr, function () {
                            self._updateNgModelPropDesc();
                            self._ngModelRender();
                        });
                    }
                }

                var i, name, prop;
                for (i = 0; i < props.length; i++) {
                    prop = props[i];
                    name = prop.propertyName;
                    if (prop.propertyType !== wijmo.interop.PropertyType.EventHandler && this._isAttrDefined(name)) {
                        this._definedProps[name] = prop;
                    }
                }
                var control = this.control;
                scope.$watch(function (scope) {
                    try  {
                        var assignValues = {};
                        for (var name in self._definedProps) {
                            if (scope[name] !== self._oldValues[name]) {
                                assignValues[name] = scope[name];
                            }
                        }

                        for (var name in assignValues) {
                            var newVal = assignValues[name], oldVal = self._oldValues[name];
                            if (newVal !== oldVal) {
                                self._oldValues[name] = newVal;
                                if (self._isInitialized || newVal !== undefined) {
                                    // get value from scope
                                    var prop = self._definedProps[name], value = self._nullOrValue(self._castValueToType(newVal, prop));

                                    // check that the control value is out-of-date
                                    var oldCtrlVal = control[name];
                                    if (oldCtrlVal != value) {
                                        // apply value to control if it's a native property
                                        // (as opposed to directive-only property)
                                        if (prop.isNativeControlProperty) {
                                            control[name] = value;
                                        }

                                        // invoke custom handler (if any) to handle the change
                                        if (prop.customHandler != null) {
                                            prop.customHandler(scope, control, value, oldCtrlVal, self);
                                        }
                                    }
                                }
                            }
                        }
                    } finally {
                        if (!self._isInitialized) {
                            self._isInitialized = true;
                            self._setupScopeWithControlProperties();
                            self._initialized();
                        }
                    }
                });
            };

            // Adds handlers for control events
            WjLink.prototype._addEventHandlers = function () {
                var i, event, evList = this.directive._events;
                for (i = 0; i < evList.length; i++) {
                    event = evList[i];
                    this._addEventHandler(event); // avoiding 'i' closure
                }
            };
            WjLink.prototype._addEventHandler = function (eventDesc) {
                var self = this, eventName = eventDesc.eventName, controlEvent = this.control[eventName];

                // check that the event name is valid
                if (controlEvent == null) {
                    throw 'Event "' + eventName + '" not found in ' + self.constructor.name;
                }

                var isDefined = this._isAttrDefined(eventName);
                if (isDefined) {
                    this._definedEvents[eventName] = eventDesc;
                } else if (!eventDesc.isPropChanged) {
                    // don't subscribe if event is neither subscribed nor "isPropChanged" event.
                    return;
                }

                var scope = this.scope, props = this.directive._props, control = this.control;

                // add the event handler
                controlEvent.addHandler(function (s, e) {
                    var eventInfo = { eventDesc: eventDesc, s: s, e: e };
                    if (self._isScopeSuspended()) {
                        self._suspendedEvents.push(eventInfo);
                    } else {
                        self._updateScope(eventInfo);
                    }
                }, control);
            };

            // Updates scope values with control values for two-way bindings.
            WjLink.prototype._updateScope = function (eventInfo) {
                if (typeof eventInfo === "undefined") { eventInfo = null; }
                // apply changes to scope
                var update = eventInfo ? eventInfo.eventDesc.isPropChanged : this._suspendedEvents.some(function (value) {
                    return value.eventDesc.isPropChanged;
                }), self = this;

                //var hasChanges = false;
                if (update) {
                    var props = this.directive._props;
                    for (var i = 0; i < props.length; i++) {
                        var p = props[i];
                        if (p.scopeBindingMode == '=' && p.isNativeControlProperty) {
                            var name = p.propertyName, value = this.control[name];
                            if (this._shouldApply(this.scope, name, value)) {
                                this.scope[name] = value;

                                //
                                this.directive._$parse(this.tAttrs[this.directive._scopeToAttrName(name)]).assign(this.scope.$parent, value);
                                //
                                //hasChanges = true;
                            }
                            if (this._ngModelPropDesc && this._ngModelPropDesc.propertyName == name && this.ngModel.$viewValue !== value) {
                                this.ngModel.$setViewValue(value);
                                if (!this._isInitialized) {
                                    this.ngModel.$setPristine();
                                }
                            }
                        }
                    }
                }

                var raiseEvents = function () {
                    var suspEvArr = eventInfo ? [eventInfo] : this._suspendedEvents;

                    for (var i = 0; i < suspEvArr.length; i++) {
                        var suspInfo = suspEvArr[i], eventName = suspInfo.eventDesc.eventName, scopeHandler = this.scope[eventName];
                        if (self._definedEvents[eventName] && scopeHandler) {
                            scopeHandler({ s: suspInfo.s, e: suspInfo.e });
                        }
                    }
                    if (!eventInfo) {
                        this._suspendedEvents.length = 0;
                    }
                }.bind(this);

                if (update) {
                    if (!this.scope['$root'].$$phase) {
                        this.scope.$apply();
                        //raiseEvents();
                    } else {
                        // We may be in a call to directive's scope $watch finalizing the digest, so there is a chance that
                        // there will be no more digests and changes made here to directive scope will not propagate to controller
                        // scope. To manage with this we initiate one more digest cycle by adding a dummy watch to the scope.
                        // We don't use setTimeout($apply(), 0) for this purpose to guarantee that all changes will be applied
                        // in this digest where we are now.
                        var dispose = this.scope.$watch('value', function () {
                            // dispose the watch right away
                            dispose();
                            //raiseEvents();
                        });
                    }
                }
                raiseEvents();
            };

            // ngModel.$render function implementation
            WjLink.prototype._ngModelRender = function () {
                var viewValue = this.ngModel.$viewValue, propDesc = this._ngModelPropDesc;
                if (!propDesc || viewValue === undefined && !this._isInitialized) {
                    return;
                }
                var value = this._nullOrValue(this._castValueToType(viewValue, propDesc));
                if (viewValue !== this.control[propDesc.propertyName]) {
                    this.control[propDesc.propertyName] = viewValue;
                }
            };

            // Casts value to the property type
            WjLink.prototype._castValueToType = function (value, prop) {
                if (value == undefined) {
                    //return undefined;
                    return value;
                }

                var type = prop.propertyType;
                switch (type) {
                    case wijmo.interop.PropertyType.Number:
                        if (typeof value == 'string') {
                            if (value.indexOf('*') >= 0) {
                                return value;
                            }
                            if (value.trim() === '') {
                                return null;
                            }
                        }
                        return +value;
                    case wijmo.interop.PropertyType.Boolean:
                        if (value === 'true') {
                            return true;
                        }
                        if (value === 'false') {
                            return false;
                        }
                        return !!value;
                    case wijmo.interop.PropertyType.String:
                        return value + '';
                    case wijmo.interop.PropertyType.Date:
                        return this._parseDate(value);
                    case wijmo.interop.PropertyType.Enum:
                        if (typeof value === 'number') {
                            return value;
                        }
                        return prop.enumType[value];
                    default:
                        return value;
                }
            };

            // Parsing DateTime values from string
            WjLink.prototype._parseDate = function (value) {
                if (value && wijmo.isString(value)) {
                    // For by-val attributes Angular converts a Date object to a
                    // string wrapped in quotation marks, so we strip them.
                    value = value.replace(/["']/g, '');

                    // parse date/time using RFC 3339 pattern
                    var dt = wijmo.changeType(value, wijmo.DataType.Date, 'r');
                    if (wijmo.isDate(dt)) {
                        return dt;
                    }
                }
                return value;
            };

            //Determines whether this is a child link.
            //NOTE: functionality is *not* based on _parentPropDesc
            WjLink.prototype._isChild = function () {
                return this.directive._isChild();
            };

            // Indicates whether this directictive operates as a child directictive that initializes a property of its parent.
            WjLink.prototype._isParentInitializer = function () {
                return this.directive._isParentInitializer();
            };

            // Indicates whether this directictive operates as a child directictive that references a parent in its property or
            // a constructor.
            WjLink.prototype._isParentReferencer = function () {
                return this.directive._isParentReferencer();
            };

            //For the child directives returns parent's property name that it services. Property name defined via
            //the wjProperty attribute of directive tag has priority over the directive._property definition.
            //NOTE: functionality is *not* based on _parentPropDesc
            WjLink.prototype._getParentProp = function () {
                return this._isParentInitializer() ? this.scope[WjDirective._parPropAttr] || this.directive._property : undefined;
            };

            // For a child directive, the name of the property of the directive's underlying object that receives the reference
            // to the parent, or an empty string that indicates that the reference to the parent should be passed as the
            // underlying object's constructor parameter.
            WjLink.prototype._getParentReferenceProperty = function () {
                return this.directive._parentReferenceProperty;
            };

            // Determines whether the child link uses an object created by the parent property, instead of creating it by
            // itself, and thus object's initialization should be delayed until parent link's control is created.
            //IMPORTANT: functionality is *based* on _parentPropDesc
            WjLink.prototype._useParentObj = function () {
                return !this._isParentReferencer() && this._isParentInitializer() && !this._parentPropDesc.isArray && !this._parentPropDesc.ownsObject;
            };

            // For the child link, determines whether the servicing parent property is an array.
            //IMPORTANT: functionality is *based* on _parentPropDesc
            WjLink.prototype._isParentArray = function () {
                return this._isParentInitializer() && this._parentPropDesc.isArray;
            };

            // For the child referencer directive, indicates whether the parent should be passed as a parameter the object
            // constructor.
            WjLink.prototype._parentInCtor = function () {
                return this._isParentReferencer() && this._getParentReferenceProperty() == '';
            };

            WjLink.prototype._getNgModelProperty = function () {
                return this.scope[WjDirective._wjModelPropAttr] || this.directive._ngModelProperty;
            };

            WjLink.prototype._updateNgModelPropDesc = function () {
                var ngModelProp = this._getNgModelProperty();
                this._ngModelPropDesc = wijmo.isNullOrWhiteSpace(ngModelProp) ? null : angular.MetaFactory.findProp(ngModelProp, this.directive._props);
            };

            // apply value to scope and notify
            WjLink.prototype._safeApply = function (scope, name, value) {
                // check that value and scope are defined, and that value changed
                if (this._shouldApply(scope, name, value)) {
                    // apply new value to scope and notify
                    scope[name] = value;
                    if (!scope.$root.$$phase) {
                        scope.$apply();
                    }
                }
            };

            // Detrmines whether value should be assigned to scope[name], depending on optional attribute support in current Angular version.
            WjLink.prototype._shouldApply = function (scope, name, value) {
                return this._canApply(scope, name) && value != scope[name];
            };

            // Detrmines whether scope[name] can be safely updated without getting an exception.
            WjLink.prototype._canApply = function (scope, name) {
                return !this._nonAssignable[name];
            };

            // Returns null for undefined or null value; otherwise, the original value.
            WjLink.prototype._nullOrValue = function (value) {
                return value != undefined ? value : null;
            };

            // Gets an index of this link among another links pertain to the same directive type.
            WjLink.prototype._getIndex = function () {
                var siblings = this.tElement[0].parentElement.childNodes, thisEl = this.tElement[0], idx = -1, dirId = this.directive._getId();
                for (var i = 0; i < siblings.length; i++) {
                    var curEl = siblings[i];
                    if (curEl.nodeType == 1 && curEl.getAttribute(WjDirective._dirIdAttr) == dirId) {
                        ++idx;
                        if (curEl === thisEl) {
                            return idx;
                        }
                    }
                }

                return -1;
            };
            return WjLink;
        })();
        angular.WjLink = WjLink;
    })(wijmo.angular || (wijmo.angular = {}));
    var angular = wijmo.angular;
})(wijmo || (wijmo = {}));

var wijmo;
(function (wijmo) {
    //
    // AngularJS directives for wijmo module
    //
    (function (angular) {
        //#region "Container directives registration"
        var wijmoContainers = window['angular'].module('wj.container', []);

        wijmoContainers.directive('wjTooltip', [function () {
                return new WjTooltip();
            }]);

        //#endregion "Container directives definitions"
        //#region "Container directives classes"
        /**
        * AngularJS directive for the @see:Tooltip class.
        *
        * Use the <b>wj-tooltip</b> directive to add tooltips to elements on the page.
        * The wj-tooltip directive supports HTML content, smart positioning, and touch.
        *
        * The wj-tooltip directive is specified as a parameter added to the
        * element that the tooltip applies to. The parameter value is the tooltip
        * text or the id of an element that contains the text. For example:
        *
        * <pre>&lt;p wj-tooltip="#fineprint" &gt;
        *     Regular paragraph content...&lt;/p&gt;
        * ...
        * &lt;div id="fineprint" style="display:none"&gt;
        *   &lt;h3&gt;Important Note&lt;/h3&gt;
        *   &lt;p&gt;
        *     Data for the current quarter is estimated
        *     by pro-rating etc.&lt;/p&gt;
        * &lt;/div&gt;</pre>
        */
        var WjTooltip = (function (_super) {
            __extends(WjTooltip, _super);
            // Initializes a new instance of WjTooltip
            function WjTooltip() {
                _super.call(this);
                this.restrict = 'A';
                this.template = '';
            }
            Object.defineProperty(WjTooltip.prototype, "_controlConstructor", {
                // Returns Wijmo Tooltip control constructor
                get: function () {
                    return wijmo.Tooltip;
                },
                enumerable: true,
                configurable: true
            });

            WjTooltip.prototype._createLink = function () {
                return new WjTooltipLink();
            };
            return WjTooltip;
        })(angular.WjDirective);

        var WjTooltipLink = (function (_super) {
            __extends(WjTooltipLink, _super);
            function WjTooltipLink() {
                _super.apply(this, arguments);
            }
            //override
            WjTooltipLink.prototype._link = function () {
                _super.prototype._link.call(this);

                var tt = this.control;
                tt.setTooltip(this.tElement[0], this.tAttrs['wjTooltip']);
            };
            return WjTooltipLink;
        })(angular.WjLink);
    })(wijmo.angular || (wijmo.angular = {}));
    var angular = wijmo.angular;
})(wijmo || (wijmo = {}));

var wijmo;
(function (wijmo) {
    //
    // AngularJS directives for wijmo.input module
    //
    (function (angular) {
        //#region "Input directives registration"
        var wijmoInput = window['angular'].module('wj.input', []);

        // register only if module is loaded
        if (wijmo.input) {
            wijmoInput.directive('wjAutoComplete', [function () {
                    return new WjAutoComplete();
                }]);

            wijmoInput.directive('wjCalendar', [function () {
                    return new WjCalendar();
                }]);

            wijmoInput.directive('wjColorPicker', [function () {
                    return new WjColorPicker();
                }]);

            wijmoInput.directive('wjComboBox', [function () {
                    return new WjComboBox();
                }]);

            wijmoInput.directive('wjInputDate', [function () {
                    return new WjInputDate();
                }]);

            wijmoInput.directive('wjInputNumber', [function () {
                    return new WjInputNumber();
                }]);

            wijmoInput.directive('wjInputMask', [function () {
                    return new WjInputMask();
                }]);

            wijmoInput.directive('wjInputTime', [function () {
                    return new WjInputTime();
                }]);

            wijmoInput.directive('wjInputColor', [function () {
                    return new WjInputColor();
                }]);

            wijmoInput.directive('wjListBox', [function () {
                    return new WjListBox();
                }]);

            wijmoInput.directive('wjMenu', [function () {
                    return new WjMenu();
                }]);

            wijmoInput.directive('wjMenuItem', [function () {
                    return new WjMenuItem();
                }]);

            wijmoInput.directive('wjMenuSeparator', [function () {
                    return new WjMenuSeparator();
                }]);

            wijmoInput.directive('wjContextMenu', [function () {
                    return new WjContextMenu();
                }]);

            wijmoInput.directive('wjCollectionViewNavigator', [function () {
                    return new WjCollectionViewNavigator();
                }]);

            wijmoInput.directive('wjCollectionViewPager', [function () {
                    return new WjCollectionViewPager();
                }]);
        }

        //#endregion "Input directives definitions"
        //#region "Input directives classes"
        // DropDown control directive
        // Provides base setup for all directives related to controls derived from DropDown
        // Abstract class, not for use in markup
        var WjDropDown = (function (_super) {
            __extends(WjDropDown, _super);
            function WjDropDown() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(WjDropDown.prototype, "_controlConstructor", {
                get: function () {
                    return wijmo.input.DropDown;
                },
                enumerable: true,
                configurable: true
            });
            return WjDropDown;
        })(angular.WjDirective);

        /**
        * AngularJS directive for the @see:ComboBox control.
        *
        * Use the <b>wj-combo-box</b> directive to add <b>ComboBox</b> controls to your AngularJS applications.
        * Note that directive and parameter names must be formatted as lower-case with dashes
        * instead of camel-case. For example:
        *
        * <pre>&lt;p&gt;Here is a ComboBox control:&lt;/p&gt;
        * &lt;wj-combo-box
        *   text="theCountry"
        *   items-source="countries"
        *   is-editable="false"
        *   placeholder="country"&gt;
        * &lt;/wj-combo-box&gt;</pre>
        *
        * The example below creates a <b>ComboBox</b> control and binds it to a 'countries' array
        * exposed by the controller. The <b>ComboBox</b> searches for the country as the user
        * types. The <b>isEditable</b> property is set to false, so the user is forced to
        * select one of the items in the list.
        *
        * @fiddle:37GHw
        *
        * The <b>wj-combo-box</b> directive supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>ng-model</dt>      <dd><code>@</code> Binds the control's <b>selectedValue</b> property using the ng-model Angular
        *                          directive. Binding the property using the ng-model directive provides standard benefits
        *                          like validation, adding the control's state to the form instance, and so on. To redefine
        *                          properties on a control that is bound by the ng-model directive, use the wj-model-property
        *                          attribute.</dd>
        *   <dt>wj-model-property</dt>  <dd><code>@</code> Specifies a control property that is bound to a scope using the
        *                               <b>ng-model</b> directive.</dd>
        *   <dt>control</dt>              <dd><code>=</code> A reference to the @see:ComboBox
        *                                 control created by this directive.</dd>
        *   <dt>display-member-path</dt>    <dd><code>@</code> The name of the property to use as
        *                                 the visual representation of the items.</dd>
        *   <dt>is-content-html</dt>        <dd><code>@</code> A value indicating whether the drop-down
        *                                 list displays the items as plain text or as HTML.</dd>
        *   <dt>is-dropped-down</dt>        <dd><code>@</code> A value indicating whether the drop down
        *                                 list is currently visible.</dd>
        *   <dt>is-editable</dt>           <dd><code>@</code> A value indicating whether the user can
        *                                 enter values not present on the list.</dd>
        *   <dt>initialized</dt>          <dd><code>&</code> This event occurs after the binding has finished
        *                                 initializing the control with attribute values.</dd>
        *   <dt>is-initialized</dt>       <dd><code>=</code> A value indicating whether the binding has finished
        *                                 initializing the control with attribute values. </dd>
        *   <dt>item-formatter</dt>        <dd><code>=</code> A function used to customize the values
        *                                 shown in the drop-down list.</dd>
        *   <dt>items-source</dt>          <dd><code>=</code> An array or @see:ICollectionView that
        *                                 contains items to show in the list.</dd>
        *   <dt>max-drop-down-height</dt>    <dd><code>@</code> The maximum height of the drop-down
        *                                 list.</dd>
        *   <dt>max-drop-down-width</dt>     <dd><code>@</code> The maximum width of the drop-down
        *                                 list.</dd>
        *   <dt>placeholder</dt>          <dd><code>@</code> A string shown as a hint when the
        *                                 control is empty.</dd>
        *   <dt>required</dt>             <dd><code>@</code> A value indicating whether to prevent
        *                                 null values.</dd>
        *   <dt>show-drop-down-button</dt>   <dd><code>@</code> A value indicating whether the control
        *                                 displays a drop-down button.</dd>
        *   <dt>selected-index</dt>        <dd><code>=</code> The index of the currently selected
        *                                 item in the drop-down list.</dd>
        *   <dt>selected-item</dt>         <dd><code>=</code> The currently selected item in the
        *                                 drop-down list.</dd>
        *   <dt>selected-value</dt>        <dd><code>=</code> The value of the selected item, obtained
        *                                 using the <b>selected-value-[ath</b>.</dd>
        *   <dt>selected-value-path</dt>    <dd><code>@</code> The name of the property used to get the
        *                                 <b>selected-value</b> from the <b>selected-item</b>.</dd>
        *   <dt>text</dt>                 <dd><code>=</code> The text to show in the control.</dd>
        *   <dt>is-dropped-down-changed</dt> <dd><code>&</code> The @see:isDroppedDownChanged event
        *                                 handler.</dd>
        *   <dt>selected-index-changed</dt> <dd><code>&</code> The @see:selectedIndexChanged event
        *                                 handler.</dd>
        *   <dt>text-changed</dt>          <dd><code>&</code> The @see:textChanged event handler.</dd>
        * </dl>
        */
        var WjComboBox = (function (_super) {
            __extends(WjComboBox, _super);
            function WjComboBox() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(WjComboBox.prototype, "_controlConstructor", {
                // Gets the Combobox control constructor
                get: function () {
                    return wijmo.input.ComboBox;
                },
                enumerable: true,
                configurable: true
            });
            return WjComboBox;
        })(WjDropDown);

        /**
        * AngularJS directive for the @see:AutoComplete control.
        *
        * Use the <b>wj-auto-complete</b> directive to add <b>AutoComplete</b> controls to your
        * AngularJS applications. Note that directive and parameter names must be
        * formatted as lower-case with dashes instead of camel-case. For example:
        *
        * <pre>&lt;p&gt;Here is an AutoComplete control:&lt;/p&gt;
        * &lt;wj-auto-complete
        *   text="theCountry"
        *   items-source="countries"
        *   is-editable="false"
        *   placeholder="country"&gt;
        * &lt;/wj-auto-complete&gt;</pre>
        *
        * The example below creates an <b>AutoComplete</b> control and binds it to a 'countries' array
        * exposed by the controller. The <b>AutoComplete</b> searches for the country as the user
        * types, and narrows down the list of countries that match the current input.
        *
        * @fiddle:37GHw
        *
        * The <b>wj-auto-complete</b> directive extends @see:WjComboBox with the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>css-match</dt>            <dd><code>@</code> The name of the css class used to highlight
        *                                 parts of the content that match the search terms.</dd>
        *   <dt>delay</dt>                <dd><code>@</code> The amount of delay in milliseconds between
        *                                 when a keystroke occurs and when the search is performed.</dd>
        *   <dt>items-source-function</dt><dd><code>=</code> A function that provides the items
        *                                 dynamically as the user types.</dd>
        *   <dt>max-items</dt>            <dd><code>@</code> The maximum number of items to display
        *                                 in the dropdown.</dd>
        *   <dt>min-length</dt>           <dd><code>@</code> The minimum input length to require before
        *                                 triggering autocomplete suggestions.</dd>
        * </dl>
        */
        var WjAutoComplete = (function (_super) {
            __extends(WjAutoComplete, _super);
            function WjAutoComplete() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(WjAutoComplete.prototype, "_controlConstructor", {
                // Gets AutoComplete control constructor
                get: function () {
                    return wijmo.input.AutoComplete;
                },
                enumerable: true,
                configurable: true
            });
            return WjAutoComplete;
        })(WjComboBox);

        /**
        * AngularJS directive for the @see:Calendar control.
        *
        * Use the <b>wj-calendar</b> directive to add <b>Calendar</b> controls to your
        * AngularJS applications.
        * Note that directive and parameter names must be formatted as lower-case with dashes
        * instead of camel-case. For example:
        *
        * <pre>&lt;p&gt;Here is a Calendar control:&lt;/p&gt;
        * &lt;wj-calendar
        *   value="theDate"&gt;
        * &lt;/wj-calendar&gt;</pre>
        *
        * @fiddle:46PhD
        *
        * This example creates a <b>Calendar</b> control and binds it to a 'date' variable
        * exposed by the controller. The range of dates that may be selected is limited
        * by the <b>min</b> and <b>max</b> properties.
        *
        * The <b>wj-calendar</b> directive supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>ng-model</dt>      <dd><code>@</code> Binds the control's <b>value</b> property using the ng-model Angular
        *                          directive. Binding the property using the ng-model directive provides standard benefits
        *                          like validation, adding the control's state to the form instance, and so on. To redefine
        *                          properties on a control that is bound by the ng-model directive, use the wj-model-property
        *                          attribute.</dd>
        *   <dt>wj-model-property</dt>  <dd><code>@</code> Specifies a control property that is bound to a scope using the
        *                               <b>ng-model</b> directive.</dd>
        *   <dt>control</dt>        <dd><code>=</code> A reference to the @see:Calendar control
        *                           created by this directive.</dd>
        *   <dt>display-month</dt>  <dd><code>=</code> The month being displayed in the calendar.</dd>
        *   <dt>first-day-of-week</dt> <dd><code>@</code> The first day of the week.</dd>
        *   <dt>initialized</dt>          <dd><code>&</code> This event occurs after the binding has finished
        *                                 initializing the control with attribute values.</dd>
        *   <dt>is-initialized</dt>       <dd><code>=</code> A value indicating whether the binding has finished
        *                                 initializing the control with attribute values. </dd>
        *   <dt>item-formatter</dt> <dd><code>=</code> The function used to customize the dates
        *                           shown in the calendar.</dd>
        *   <dt>max</dt>            <dd><code>@</code> The latest valid date (string in the
        *                           format "yyyy-MM-dd").</dd>
        *   <dt>min</dt>            <dd><code>@</code> The earliest valid date (string in the
        *                           format "yyyy-MM-dd").</dd>
        *   <dt>month-view</dt>     <dd><code>@</code> A value indicating whether the control displays
        *                           a month or the entire year.</dd>
        *   <dt>show-header</dt>    <dd><code>@</code> A value indicating whether the control displays
        *                           the header area.</dd>
        *   <dt>value</dt>          <dd><code>=</code> The date being edited.</dd>
        *   <dt>value-changed</dt>  <dd><code>&</code> The @see:valueChanged event handler.</dd>
        * </dl>
        *
        * If provided, the <b>min</b> and <b>max</b> attributes are strings in the format
        * "yyyy-MM-dd." Technically, you can use any full date as defined in the W3C
        * <a href="http://tools.ietf.org/html/rfc3339" target="_blank">[RFC 3339]</a>,
        * which is also the format used with regular HTML5 input elements.
        */
        var WjCalendar = (function (_super) {
            __extends(WjCalendar, _super);
            function WjCalendar() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(WjCalendar.prototype, "_controlConstructor", {
                // Gets the Calendar control constructor
                get: function () {
                    return wijmo.input.Calendar;
                },
                enumerable: true,
                configurable: true
            });
            return WjCalendar;
        })(angular.WjDirective);

        /**
        * AngularJS directive for the @see:ColorPicker control.
        *
        * Use the <b>wj-color-picker</b> directive to add <b>ColorPicker</b> controls to your
        * AngularJS applications. Note that directive and parameter names must be
        * formatted as lower-case with dashes instead of camel-case. For example:
        *
        * <pre>&lt;p&gt;Here is a ColorPicker control:&lt;/p&gt;
        * &lt;wj-color-picker
        *   value="theColor"
        *   show-alpha-channel="false"&gt;
        * &lt;/wj-color-picker&gt;</pre>
        *
        * The <b>wj-color-picker</b> directive supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>ng-model</dt>      <dd><code>@</code> Binds the control's <b>value</b> property using the ng-model Angular
        *                          directive. Binding the property using the ng-model directive provides standard benefits
        *                          like validation, adding the control's state to the form instance, and so on. To redefine
        *                          properties on a control that is bound by the ng-model directive, use the wj-model-property
        *                          attribute.</dd>
        *   <dt>wj-model-property</dt>  <dd><code>@</code> Specifies a control property that is bound to a scope using the
        *                               <b>ng-model</b> directive.</dd>
        *   <dt>control</dt>           <dd><code>=</code> A reference to the @see:ColorPicker
        *                              control created by this directive.</dd>
        *   <dt>initialized</dt>          <dd><code>&</code> This event occurs after the binding has finished
        *                                 initializing the control with attribute values.</dd>
        *   <dt>is-initialized</dt>       <dd><code>=</code> A value indicating whether the binding has finished
        *                                 initializing the control with attribute values. </dd>
        *   <dt>show-alpha-channel</dt><dd><code>@</code> A value indicating whether the control
        *                              displays the alpha channel (transparency) editor.</dd>
        *   <dt>show-color-string</dt> <dd><code>@</code> A value indicating whether the control
        *                              displays a string representation of the color being edited.</dd>
        *   <dt>palette</dt>           <dd><code>=</code> An array with ten color values to use
        *                              as the palette.</dd>
        *   <dt>value</dt>             <dd><code>=</code> The color being edited.</dd>
        *   <dt>value-changed</dt>     <dd><code>&</code> The @see:valueChanged event handler.</dd>
        * </dl>
        */
        var WjColorPicker = (function (_super) {
            __extends(WjColorPicker, _super);
            function WjColorPicker() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(WjColorPicker.prototype, "_controlConstructor", {
                // Gets the ColorPicker control constructor
                get: function () {
                    return wijmo.input.ColorPicker;
                },
                enumerable: true,
                configurable: true
            });
            return WjColorPicker;
        })(angular.WjDirective);

        /**
        * AngularJS directive for the @see:ListBox control.
        *
        * Use the <b>wj-list-box</b> directive to add @see:ListBox controls to your
        * AngularJS applications.
        * Note that directive and parameter names must be formatted as lower-case with dashes
        * instead of camel-case. For example:
        *
        * <pre>b&gt;Here is a ListBox control:&lt;/p&gt;
        * &lt;wj-list-box
        *   selected-item="theCountry"
        *   items-source="countries"
        *   placeholder="country"&gt;
        * &lt;/wj-list-box&gt;</pre>
        *
        * The example below creates a <b>ListBox</b> control and binds it to a 'countries' array
        * exposed by the controller. The value selected is bound to the 'theCountry'
        * controller property using the <b>selected-item</b> attribute.
        *
        * @fiddle:37GHw
        *
        * The <b>wj-list-box</b> directive supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>ng-model</dt>      <dd><code>@</code> Binds the control's <b>selectedValue</b> property using the ng-model Angular
        *                          directive. Binding the property using the ng-model directive provides standard benefits
        *                          like validation, adding the control's state to the form instance, and so on. To redefine
        *                          properties on a control that is bound by the ng-model directive, use the wj-model-property
        *                          attribute.</dd>
        *   <dt>wj-model-property</dt>  <dd><code>@</code> Specifies a control property that is bound to a scope using the
        *                               <b>ng-model</b> directive.</dd>
        *   <dt>control</dt>              <dd><code>=</code> A reference to the @see:ListBox
        *                                 control created by this directive.</dd>
        *   <dt>display-member-path</dt>  <dd><code>@</code> The property to use as the visual
        *                                 representation of the items.</dd>
        *   <dt>is-content-html</dt>      <dd><code>@</code> A value indicating whether items
        *                                 contain plain text or HTML.</dd>
        *   <dt>initialized</dt>          <dd><code>&</code> This event occurs after the binding has finished
        *                                 initializing the control with attribute values.</dd>
        *   <dt>is-initialized</dt>       <dd><code>=</code> A value indicating whether the binding has finished
        *                                 initializing the control with attribute values. </dd>
        *   <dt>item-formatter</dt>       <dd><code>=</code> A function used to customize the
        *                                 values to show in the list.</dd>
        *   <dt>items-source</dt>         <dd><code>=</code> An array or @see:ICollectionView
        *                                 that contains the list items.</dd>
        *   <dt>max-height</dt>           <dd><code>@</code> The maximum height of the list.</dd>
        *   <dt>selected-index</dt>       <dd><code>=</code> The index of the currently selected
        *                                 item.</dd>
        *   <dt>selected-item</dt>        <dd><code>=</code> The item that is currently selected.</dd>
        *   <dt>selected-value</dt>       <dd><code>=</code> The value of the <b>selected-item</b>
        *                                 obtained using the <b>selected-value-path</b>.</dd>
        *   <dt>selected-value-path</dt>  <dd><code>@</code> The property used to get the
        *                                 <b>selected-value</b> from the <b>selected-item</b>.</dd>
        *   <dt>items-changed</dt>        <dd><code>&</code> The @see:itemsChanged event handler.</dd>
        *   <dt>selected-index-changed</dt> <dd><code>&</code> The s@see:electedIndexChanged event handler.</dd>
        * </dl>
        */
        var WjListBox = (function (_super) {
            __extends(WjListBox, _super);
            function WjListBox() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(WjListBox.prototype, "_controlConstructor", {
                // Gets the ListBox control constructor
                get: function () {
                    return wijmo.input.ListBox;
                },
                enumerable: true,
                configurable: true
            });
            return WjListBox;
        })(angular.WjDirective);

        /**
        * AngularJS directive for the @see:Menu control.
        *
        * Use the <b>wj-menu</b> directive to add drop-down menus to your AngularJS applications.
        * Note that directive and parameter names must be formatted as lower-case with dashes
        * instead of camel-case. For example:
        *
        * <pre>&lt;p&gt;Here is a Menu control used as a value picker:&lt;/p&gt;
        * &lt;wj-menu header="Tax" value="tax"&gt;
        *   &lt;wj-menu-item value="0"&gt;Exempt&lt;/wj-menu-item&gt;
        *   &lt;wj-menu-item value=".05"&gt;5%&lt;/wj-menu-item&gt;
        *   &lt;wj-menu-item value=".1"&gt;10%&lt;/wj-menu-item&gt;
        *   &lt;wj-menu-item value=".15"&gt;15%&lt;/wj-menu-item&gt;
        * &lt;/wj-menu&gt;</pre>
        *
        * @fiddle:Wc5Mq
        *
        * This example creates three <b>Menu</b> controls. The first is used as a value picker,
        * the second uses a list of commands with parameters, and the third is a group of
        * three menus handled by an <b>itemClicked</b> function in the controller.
        *
        * The <b>wj-menu</b> directive extends @see:WjComboBox with the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>command-path</dt>          <dd><code>@</code> The command to be executed when the item is clicked.</dd>
        *   <dt>command-parameter-path</dt><dd><code>@</code> The name of the property that contains command parameters.</dd>
        *   <dt>header</dt>                <dd><code>@</code> The text shown on the control.</dd>
        *   <dt>is-button</dt>             <dd><code>@</code> Whether the menu should react to clicks on its header area.</dd>
        *   <dt>value</dt>                 <dd><code>@</code> The value of the selected <b>wj-menu-item</b> value property. </dd>
        *   <dt>item-clicked</dt>          <dd><code>&</code> The @see:itemClicked event handler.</dd>
        * </dl>
        *
        * The <b>wj-menu</b> directive may contain the following child directives:
        * @see:WjMenuItem and @see:WjMenuSeparator.
        */
        var WjMenu = (function (_super) {
            __extends(WjMenu, _super);
            // Initializes a new instance of a WjMenu
            function WjMenu() {
                _super.call(this);

                this.template = '<div ng-transclude />';
                this.transclude = true;
            }
            Object.defineProperty(WjMenu.prototype, "_controlConstructor", {
                // Gets the Menu control constructor
                get: function () {
                    return wijmo.input.Menu;
                },
                enumerable: true,
                configurable: true
            });

            WjMenu.prototype._createLink = function () {
                return new WjMenuLink();
            };

            // WjMenu property map
            WjMenu.prototype._initProps = function () {
                _super.prototype._initProps.call(this);
                var self = this;
                var valueDesc = angular.MetaFactory.findProp('value', this._props);
                valueDesc.customHandler = function (scope, control, value, oldValue, link) {
                    self.updateControlValue(scope, control, link);
                };
            };

            WjMenu.prototype._controllerImpl = function (controller, scope) {
                // stores collection of menu items defined as nested tags
                var items = scope.items = [];

                // adds menu items defined as nested tags into scope collection
                controller.addItem = function (scope, element) {
                    items.push({ scope: scope, header: element[0].innerHTML });
                };
            };

            WjMenu.prototype.updateControlValue = function (scope, control, link) {
                if (scope.value != null) {
                    control.selectedValue = scope.value;
                    link.directive.updateHeader(scope, control, link);
                }
            };

            // update header to show the currently selected value
            WjMenu.prototype.updateHeader = function (scope, control, link) {
                control.header = scope.header;
                if (typeof (scope.value) != 'undefined' && control.selectedItem && control.displayMemberPath) {
                    var currentValue = control.selectedItem[control.displayMemberPath];
                    if (currentValue != null) {
                        control.header += ': <b>' + currentValue + '</b>';
                    }
                }
            };
            return WjMenu;
        })(WjComboBox);

        var WjMenuLink = (function (_super) {
            __extends(WjMenuLink, _super);
            function WjMenuLink() {
                _super.apply(this, arguments);
            }
            //override
            WjMenuLink.prototype._link = function () {
                _super.prototype._link.call(this);

                var self = this, control = this.control, scope = this.scope, directive = this.directive;

                // populate menu
                if (scope.items.length) {
                    var itemsource = [];
                    for (var i = 0; i < scope.items.length; i++) {
                        var itemScope = scope.items[i].scope;
                        itemsource.push({
                            header: itemScope.header ? itemScope.header : scope.items[i].header,
                            cmd: itemScope.cmd,
                            cmdParam: itemScope.cmdParam,
                            value: itemScope.value
                        });
                    }
                    control.displayMemberPath = 'header';
                    control.commandPath = 'cmd';
                    control.commandParameterPath = 'cmdParam';
                    control.selectedValuePath = 'value';
                    control.itemsSource = itemsource;
                    control.selectedIndex = 0;
                }

                // update 'value' and header when an item is clicked
                control.itemClicked.addHandler(function () {
                    self._safeApply(scope, 'value', control.selectedValue);
                    directive.updateHeader(scope, control, self);
                });

                // and update the header now
                directive.updateHeader(scope, control, self);
            };
            return WjMenuLink;
        })(angular.WjLink);
        angular.WjMenuLink = WjMenuLink;

        /**
        * AngularJS directive for menu items.
        *
        * The <b>wj-menu-item</b> directive must be contained in a @see:WjMenu directive.
        * It supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>cmd</dt>       <dd><code>=</code> The function to execute in the controller
        *                      when the item is clicked.</dd>
        *   <dt>cmd-param</dt>  <dd><code>=</code> The parameter passed to the <b>cmd</b> function
        *                      when the item is clicked.</dd>
        *   <dt>value</dt>     <dd><code>=</code> The value to select when the item is clicked
        *                      (use either this or <b>cmd</b>).</dd>
        * </dl>
        *
        * The content displayed by the item is defined by the HTML contained in <b>wj-menu-item</b> directive.
        */
        var WjMenuItem = (function (_super) {
            __extends(WjMenuItem, _super);
            // Initializes a new instance of a WjMenuItem
            function WjMenuItem() {
                _super.call(this);

                this.template = '<span ng-transclude/>';
                this.require = '^wjMenu';
                this.transclude = true;
                this.replace = true;
            }
            WjMenuItem.prototype._getMetaDataId = function () {
                return 'MenuItem';
            };

            // Gets the WjMenuItem directive's link function. Overrides parent method
            WjMenuItem.prototype._postLinkFn = function () {
                var self = this;
                return function (scope, tElement, tAttrs, menu) {
                    menu.addItem(scope, tElement);
                    tElement[0].style.display = 'none';
                };
            };
            return WjMenuItem;
        })(angular.WjDirective);

        /**
        * AngularJS directive for menu separators.
        *
        * The <b>wj-menu-item-separator</b> directive must be contained in a @see:WjMenu directive.
        * It adds a non-selectable separator to the menu, and has no attributes.
        */
        var WjMenuSeparator = (function (_super) {
            __extends(WjMenuSeparator, _super);
            // Initializes a new instance of a WjMenuSeparator
            function WjMenuSeparator() {
                _super.call(this);
                this.template = '<span />';
                this.require = '^wjMenu';
            }
            WjMenuSeparator.prototype._getMetaDataId = function () {
                return 'MenuSeparator';
            };

            // Gets the WjMenuSeparator's link function. Overrides parent member
            WjMenuSeparator.prototype._postLinkFn = function () {
                return function (scope, tElement, tAttrs, menu) {
                    scope.header = '<div style="width:100%;height:1px;background-color:lightgray;opacity:.2"/>';
                    menu.addItem(scope, tElement);
                    tElement[0].style.display = 'none';
                };
            };
            return WjMenuSeparator;
        })(angular.WjDirective);

        /**
        * AngularJS directive for context menus.
        *
        * Use the <b>wj-context-menu</b> directive to add context menus to elements
        * on the page. The wj-context-menu directive is based on the <b>wj-menu</b>
        * directive; it displays a popup menu when the user performs a context menu
        * request on an element (usually a right-click).
        *
        * The wj-context-menu directive is specified as a parameter added to the
        * element that the context menu applies to. The parameter value is a
        * selector for the element that contains the menu. For example:
        *
        * <pre>&lt;!-- paragraph with a context menu --&gt;
        *&lt;p wj-context-menu="#idMenu" &gt;
        *  This paragraph has a context menu.&lt;/p&gt;
        *
        *&lt;!-- define the context menu (hidden and with an id) --&gt;
        *&lt;wj-menu id="idMenu" ng-show="false"&gt;
        *  &lt;wj-menu-item cmd="cmdOpen" cmd-param ="1"&gt;Open...&lt;/wj-menu-item&gt;
        *  &lt;wj-menu-item cmd="cmdSave" cmd-param="2"&gt;Save &lt;/wj-menu-item&gt;
        *  &lt;wj-menu-item cmd="cmdSave" cmd-param="3"&gt;Save As...&lt;/wj-menu-item&gt;
        *  &lt;wj-menu-item cmd="cmdNew" cmd-param ="4"&gt;New...&lt;/wj-menu-item&gt;
        *  &lt;wj-menu-separator&gt;&lt;/wj-menu-separator&gt;
        *  &lt;wj-menu-item cmd="cmdExit" cmd-param="5"&gt;Exit&lt;/wj-menu-item&gt;
        *&lt;/wj-menu &gt;</pre>
        */
        var WjContextMenu = (function (_super) {
            __extends(WjContextMenu, _super);
            // Initializes a new instance of a WjContextMenu
            function WjContextMenu() {
                _super.call(this);
                this.template = undefined;

                //this.require = '^wjMenu';
                this.restrict = 'A';
                this.scope = false;
            }
            WjContextMenu.prototype._getMetaDataId = function () {
                return 'WjContextMenu';
            };

            // Gets the WjContextMenu's link function. Overrides parent member
            WjContextMenu.prototype._postLinkFn = function () {
                return function (scope, tElement, tAttrs) {
                    // get context menu and drop-down list
                    var host = wijmo.getElement(tAttrs['wjContextMenu']);

                    // show the drop-down list in response to the contextmenu command
                    tElement[0].addEventListener('contextmenu', function (e) {
                        var menu = wijmo.Control.getControl(host), dropDown = menu.dropDown;
                        if (menu && dropDown) {
                            e.preventDefault();
                            menu.selectedIndex = -1;
                            wijmo.showPopup(dropDown, e);
                            dropDown.focus();
                        }
                    });
                };
            };
            return WjContextMenu;
        })(angular.WjDirective);

        /**
        * AngularJS directive for the @see:InputDate control.
        *
        * Use the <b>wj-input-date</b> directive to add @see:InputDate controls to your
        * AngularJS applications.
        * Note that directive and parameter names must be formatted as lower-case with dashes
        * instead of camel-case. For example:
        *
        * <pre>&lt;p&gt;Here is an InputDate control:&lt;/p&gt;
        * &lt;wj-input-date
        *   value="theDate"
        *   format="M/d/yyyy"&gt;
        * &lt;/wj-input-date&gt;</pre>
        *
        * The example below shows a <b>Date</b> value (that includes date and time information)
        * using an @see:InputDate and an an @see:InputTime control. Notice how both controls
        * are bound to the same controller variable, and each edits the appropriate information
        * (either date or time). The example also shows a @see:Calendar control that can be
        * used to select the date with a single click.
        *
        * @fiddle:46PhD
        *
        * The <b>wj-input-date</b> directive supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>ng-model</dt>      <dd><code>@</code> Binds the control's <b>value</b> property using the ng-model Angular
        *                          directive. Binding the property using the ng-model directive provides standard benefits
        *                          like validation, adding the control's state to the form instance, and so on. To redefine
        *                          properties on a control that is bound by the ng-model directive, use the wj-model-property
        *                          attribute.</dd>
        *   <dt>wj-model-property</dt>  <dd><code>@</code> Specifies a control property that is bound to a scope using the
        *                               <b>ng-model</b> directive.</dd>
        *   <dt>control</dt>             <dd><code>=</code> A reference to the @see:InputDate
        *                                control created by this directive.</dd>
        *   <dt>format</dt>              <dd><code>@</code> The format used to display the date
        *                                being edited (see @see:Globalize).</dd>
        *   <dt>mask</dt>                <dd><code>@</code> The mask used to validate the input as
        *                                the user types (see @see:wijmo.input.InputMask).</dd>
        *   <dt>is-dropped-down</dt>     <dd><code>@</code> A value indicating whether the drop-down
        *                                is currently visible.</dd>
        *   <dt>initialized</dt>          <dd><code>&</code> This event occurs after the binding has finished
        *                                 initializing the control with attribute values.</dd>
        *   <dt>is-initialized</dt>       <dd><code>=</code> A value indicating whether the binding has finished
        *                                 initializing the control with attribute values. </dd>
        *   <dt>max</dt>                 <dd><code>@</code> The latest valid date (a string in the
        *                                format "yyyy-MM-dd").</dd>
        *   <dt>min</dt>                 <dd><code>@</code> The earliest valid date (a string in the
        *                                format "yyyy-MM-dd").</dd>
        *   <dt>place-holder</dt>        <dd><code>@</code> The string to show as a hint when the
        *                                control is empty.</dd>
        *   <dt>required</dt>            <dd><code>@</code> A value indicating whether to prevent
        *                                null values.</dd>
        *   <dt>show-drop-down-button</dt><dd><code>@</code> A value indicating whether the control
        *                                displays a drop-down button.</dd>
        *   <dt>text</dt>                <dd><code>=</code> The text to show in the control.</dd>
        *   <dt>value</dt>               <dd><code>=</code> The date being edited.</dd>
        *   <dt>is-dropped-down-changed</dt> <dd><code>&</code> The @see:isDroppedDownChanged event
        *                                handler.</dd>
        *   <dt>text-changed</dt>         <dd><code>&</code> The @see:textChanged event handler.</dd>
        *   <dt>value-changed</dt>        <dd><code>&</code> The @see:valueChanged event handler.</dd>
        * </dl>
        *
        * If provided, the <b>min</b> and <b>max</b> attributes are strings in the format
        * "yyyy-MM-dd". Technically, you can use any full date as defined in the W3C
        * <a href="http://tools.ietf.org/html/rfc3339" target="_blank">[RFC 3339]<a>, which is also
        * the format used with regular HTML5 input elements.
        */
        var WjInputDate = (function (_super) {
            __extends(WjInputDate, _super);
            function WjInputDate() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(WjInputDate.prototype, "_controlConstructor", {
                // Gets the InputDate control constructor
                get: function () {
                    return wijmo.input.InputDate;
                },
                enumerable: true,
                configurable: true
            });
            return WjInputDate;
        })(WjDropDown);

        /**
        * AngularJS directive for the @see:InputNumber control.
        *
        * Use the <b>wj-input-number</b> directive to add <b>InputNumber</b> controls to your
        * AngularJS applications.
        * Note that directive and parameter names must be formatted as lower-case with dashes
        * instead of camel-case. For example:
        *
        * <pre>&lt;p&gt;Here is an InputNumber control:&lt;/p&gt;
        * &lt;wj-input-number
        *   value="theNumber"
        *   min="0"
        *   max="10"
        *   format="n0"
        *   placeholder="number between zero and ten"&gt;
        * &lt;/wj-input-number&gt;</pre>
        *
        * The example below creates several <b>InputNumber</b> controls and shows the effect
        * of using different formats, ranges, and step values.
        *
        * @fiddle:u7HpD
        *
        * The <b>wj-input-number</b> directive supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>ng-model</dt>      <dd><code>@</code> Binds the control's <b>value</b> property using the ng-model Angular
        *                          directive. Binding the property using the ng-model directive provides standard benefits
        *                          like validation, adding the control's state to the form instance, and so on. To redefine
        *                          properties on a control that is bound by the ng-model directive, use the wj-model-property
        *                          attribute.</dd>
        *   <dt>wj-model-property</dt>  <dd><code>@</code> Specifies a control property that is bound to a scope using the
        *                               <b>ng-model</b> directive.</dd>
        *   <dt>control</dt>       <dd><code>=</code> A reference to the @see:InputNumber
        *                          control created by this directive.</dd>
        *   <dt>format</dt>        <dd><code>@</code> The format used to display the number
        *                          (see @see:Globalize).</dd>
        *   <dt>input-type</dt>    <dd><code>@</code> The "type" attribute of the HTML
        *                          input element hosted by the control.</dd>
        *   <dt>initialized</dt>          <dd><code>&</code> This event occurs after the binding has finished
        *                                 initializing the control with attribute values.</dd>
        *   <dt>is-initialized</dt>       <dd><code>=</code> A value indicating whether the binding has finished
        *                                 initializing the control with attribute values. </dd>
        *   <dt>max</dt>           <dd><code>@</code> The largest valid number.</dd>
        *   <dt>min</dt>           <dd><code>@</code> The smallest valid number.</dd>
        *   <dt>place-holder</dt>  <dd><code>@</code> The string to show as a hint when the
        *                          control is empty.</dd>
        *   <dt>required</dt>      <dd><code>@</code> A value indicating whether to prevent null
        *                          values.</dd>
        *   <dt>show-spinner</dt>  <dd><code>@</code> A value indicating whether to display spinner
        *                          buttons to change the value by <b>step</b> units.</dd>
        *   <dt>step</dt>          <dd><code>@</code> The amount to add or subtract to the value
        *                          when the user clicks the spinner buttons.</dd>
        *   <dt>text</dt>          <dd><code>=</code> The text to show in the control.</dd>
        *   <dt>value</dt>         <dd><code>=</code> The number being edited.</dd>
        *   <dt>text-changed</dt>  <dd><code>&</code> The @see:textChanged event handler.</dd>
        *   <dt>value-changed</dt> <dd><code>&</code> The @see:valueChanged event handler.</dd>
        * </dl>
        */
        var WjInputNumber = (function (_super) {
            __extends(WjInputNumber, _super);
            function WjInputNumber() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(WjInputNumber.prototype, "_controlConstructor", {
                // Gets the InputNumber control constructor
                get: function () {
                    return wijmo.input.InputNumber;
                },
                enumerable: true,
                configurable: true
            });
            return WjInputNumber;
        })(angular.WjDirective);

        /**
        * AngularJS directive for the @see:InputMask control.
        *
        * Use the <b>wj-input-mask</b> directive to add @see:InputMask controls to your
        * AngularJS applications.
        * Note that directive and parameter names must be formatted as lower-case with dashes
        * instead of camel-case. For example:
        *
        * <pre>&lt;p&gt;Here is an InputMask control:&lt;/p&gt;
        * &lt;wj-input-mask
        *   mask="99/99/99"
        *   mask-placeholder="*"&gt;
        * &lt;/wj-input-mask&gt;</pre>
        *
        * The <b>wj-input-mask</b> directive supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>ng-model</dt>      <dd><code>@</code> Binds the control's <b>value</b> property using the ng-model Angular
        *                          directive. Binding the property using the ng-model directive provides standard benefits
        *                          like validation, adding the control's state to the form instance, and so on. To redefine
        *                          properties on a control that is bound by the ng-model directive, use the wj-model-property
        *                          attribute.</dd>
        *   <dt>wj-model-property</dt>  <dd><code>@</code> Specifies a control property that is bound to a scope using the
        *                               <b>ng-model</b> directive.</dd>
        *   <dt>control</dt>           <dd><code>=</code> A reference to the @see:InputNumber
        *                              control created by this directive.</dd>
        *   <dt>initialized</dt>          <dd><code>&</code> This event occurs after the binding has finished
        *                                 initializing the control with attribute values.</dd>
        *   <dt>is-initialized</dt>       <dd><code>=</code> A value indicating whether the binding has finished
        *                                 initializing the control with attribute values. </dd>
        *   <dt>mask</dt>              <dd><code>@</code> The string mask used to format the value
        *                              as the user types.</dd>
        *   <dt>prompt-char</dt>       <dd><code>@</code> A character used to show input locations
        *                              within the mask.</dd>
        *   <dt>place-holder</dt>      <dd><code>@</code> The string to show as a hint when the control
        *                              is empty.</dd>
        *   <dt>value</dt>             <dd><code>=</code> The number being edited.</dd>
        *   <dt>value-changed</dt>     <dd><code>&</code> The @see:valueChanged event handler.</dd>
        * </dl>
        */
        var WjInputMask = (function (_super) {
            __extends(WjInputMask, _super);
            function WjInputMask() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(WjInputMask.prototype, "_controlConstructor", {
                // Gets the InputMask control constructor
                get: function () {
                    return wijmo.input.InputMask;
                },
                enumerable: true,
                configurable: true
            });
            return WjInputMask;
        })(angular.WjDirective);

        /**
        * AngularJS directive for the @see:InputTime control.
        *
        * Use the <b>wj-input-time</b> directive to add <b>InputTime</b> controls to your AngularJS applications.
        * Note that directive and parameter names must be formatted as lower-case with dashes
        * instead of camel-case. For example:
        *
        * <pre>&lt;p&gt;Here is an InputTime control:&lt;/p&gt;
        * &lt;wj-input-time
        *   value="theDate"
        *   format="h:mm tt"
        *   min="09:00" max="17:00"
        *   step="15"&gt;
        * &lt;/wj-input-time&gt;</pre>
        *
        * @fiddle:46PhD
        *
        * This example edits a <b>Date</b> value (that includes date and time information)
        * using an @see:InputDate and an an InputTime control. Notice how both controls
        * are bound to the same controller variable, and each edits the appropriate information
        * (either date or time). The example also shows a @see:Calendar control that can be
        * used to select the date with a single click.
        *
        * The <b>wj-input-time</b> directive extends @see:WjComboBox with the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>ng-model</dt>      <dd><code>@</code> Binds the control's <b>value</b> property using the ng-model Angular
        *                          directive. Binding the property using the ng-model directive provides standard benefits
        *                          like validation, adding the control's state to the form instance, and so on. To redefine
        *                          properties on a control that is bound by the ng-model directive, use the wj-model-property
        *                          attribute.</dd>
        *   <dt>wj-model-property</dt>  <dd><code>@</code> Specifies a control property that is bound to a scope using the
        *                               <b>ng-model</b> directive.</dd>
        *   <dt>control</dt>   <dd><code>=</code> A reference to the @see:InputDate control
        *                      created by this directive.</dd>
        *   <dt>format</dt>    <dd><code>@</code> The format used to display the selected time.</dd>
        *   <dt>mask</dt>      <dd><code>@</code> A mask used to validate the input as the
        *                      user types (see @see:InputMask).</dd>
        *   <dt>max</dt>       <dd><code>@</code> The earliest valid time (a string in the format
        *                      "hh:mm").</dd>
        *   <dt>min</dt>       <dd><code>@</code> The latest valid time (a string in the format
        *                      "hh:mm").</dd>
        *   <dt>step</dt>      <dd><code>@</code> The number of minutes between entries in the
        *                      drop-down list.</dd>
        *   <dt>value</dt>     <dd><code>=</code> The time being edited (as a Date object).</dd>
        *   <dt>value-changed</dt>
        *                      <dd><code>&</code> The@see: valueChanged event handler.</dd>
        * </dl>
        *
        * If provided, the <b>min</b> and <b>max</b> attributes are strings in the format
        * "hh:mm". Technically, you can use any full date as defined in the W3C
        * <a href="http://tools.ietf.org/html/rfc3339" target="_blank">[RFC 3339]</a>, which is also the format
        * used with regular HTML5 input elements.
        */
        var WjInputTime = (function (_super) {
            __extends(WjInputTime, _super);
            function WjInputTime() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(WjInputTime.prototype, "_controlConstructor", {
                // Gets the InputTime control constructor
                get: function () {
                    return wijmo.input.InputTime;
                },
                enumerable: true,
                configurable: true
            });
            return WjInputTime;
        })(WjComboBox);

        /**
        * AngularJS directive for the @see:InputColor control.
        *
        * Use the <b>wj-input-color</b> directive to add @see:InputColor controls to your
        * AngularJS applications.
        * Note that directive and parameter names must be formatted as lower-case with dashes
        * instead of camel-case. For example:
        *
        * <pre>&lt;p&gt;Here is an InputColor control:&lt;/p&gt;
        * &lt;wj-input-color
        *   value="theColor"
        *   show-alpha-channel="false"&gt;
        * &lt;/wj-input-color&gt;</pre>
        *
        * The <b>wj-input-color</b> directive supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>ng-model</dt>      <dd><code>@</code> Binds the control's <b>value</b> property using the ng-model Angular
        *                          directive. Binding the property using the ng-model directive provides standard benefits
        *                          like validation, adding the control's state to the form instance, and so on. To redefine
        *                          properties on a control that is bound by the ng-model directive, use the wj-model-property
        *                          attribute.</dd>
        *   <dt>wj-model-property</dt>  <dd><code>@</code> Specifies a control property that is bound to a scope using the
        *                               <b>ng-model</b> directive.</dd>
        *   <dt>control</dt>               <dd><code>=</code> A reference to the InputColor
        *                                  control created by this directive.</dd>
        *   <dt>is-dropped-down</dt>       <dd><code>@</code> A value indicating whether the drop-down
        *                                  is currently visible.</dd>
        *   <dt>initialized</dt>          <dd><code>&</code> This event occurs after the binding has finished
        *                                 initializing the control with attribute values.</dd>
        *   <dt>is-initialized</dt>       <dd><code>=</code> A value indicating whether the binding has finished
        *                                 initializing the control with attribute values. </dd>
        *   <dt>show-alpha-channel</dt>    <dd><code>@</code> A value indicatinbg whether the drop-down
        *                                  displays the alpha channel (transparency) editor.</dd>
        *   <dt>place-holder</dt>          <dd><code>@</code> The string to show as a hint when the
        *                                  control is empty.</dd>
        *   <dt>required</dt>              <dd><code>@</code> A value indicating whether to prevent null
        *                                  values.</dd>
        *   <dt>show-drop-down-button</dt> <dd><code>@</code> A value indicating whether the control
        *                                  displays a drop-down button.</dd>
        *   <dt>text</dt>                  <dd><code>=</code> The text to show in the control.</dd>
        *   <dt>value</dt>                 <dd><code>=</code> The color being edited.</dd>
        *   <dt>is-dropped-down-changed</dt><dd><code>&</code> The @see:isDroppedDownChanged event handler.</dd>
        *   <dt>text-changed</dt>          <dd><code>&</code> The @see:textChanged event handler.</dd>
        *   <dt>value-changed</dt>         <dd><code>&</code> The @see:valueChanged event handler.</dd>
        * </dl>
        */
        var WjInputColor = (function (_super) {
            __extends(WjInputColor, _super);
            function WjInputColor() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(WjInputColor.prototype, "_controlConstructor", {
                // Gets the InputColor control constructor
                get: function () {
                    return wijmo.input.InputColor;
                },
                enumerable: true,
                configurable: true
            });
            return WjInputColor;
        })(WjDropDown);

        /**
        * AngularJS directive for an @see:ICollectionView navigator element.
        *
        * Use the <b>wj-collection-view-navigator</b> directive to add an element that allows users to
        * navigate through the items in an @see:ICollectionView.
        * Note that directive and parameter names must be formatted as lower-case with dashes
        * instead of camel-case. For example:
        *
        * <pre>Here is a CollectionViewNavigator:&lt;/p&gt;
        * &lt;wj-collection-view-navigator
        *   cv="myCollectionView"&gt;
        * &lt;/wj-collection-view-navigator&gt;</pre>
        *
        * @fiddle:s8tT4
        *
        * This example creates a CollectionView with 100,000 items and 20 items per page.
        * It defines a navigator to select the current page, another to select the current item,
        * and shows the data in a @see:FlexGrid.
        *
        * The <b>wj-collection-view-navigator</b> directive has a single attribute:
        *
        * <dl class="dl-horizontal">
        *   <dt>cv</dt>  <dd><code>=</code> A reference to the @see:ICollectionView object to navigate.</dd>
        * </dl>
        */
        var WjCollectionViewNavigator = (function (_super) {
            __extends(WjCollectionViewNavigator, _super);
            // Initializes a new instance of a WjCollectionViewNavigator
            function WjCollectionViewNavigator() {
                _super.call(this);

                this.template = '<div class="wj-control wj-content wj-pager">' + '    <div class="wj-input-group">' + '        <span class="wj-input-group-btn" >' + '            <button class="wj-btn wj-btn-default" type="button"' + '               ng-click="cv.moveCurrentToFirst()"' + '               ng-disabled="cv.currentPosition <= 0">' + '                <span class="wj-glyph-left" style="margin-right: -4px;"></span>' + '                <span class="wj-glyph-left"></span>' + '             </button>' + '        </span>' + '        <span class="wj-input-group-btn" >' + '           <button class="wj-btn wj-btn-default" type="button"' + '               ng-click="cv.moveCurrentToPrevious()"' + '               ng-disabled="cv.currentPosition <= 0">' + '                <span class="wj-glyph-left"></span>' + '           </button>' + '        </span>' + '        <input type="text" class="wj-form-control" value="' + '           {{cv.currentPosition + 1 | number}} / {{cv.itemCount | number}}' + '           " disabled />' + '        <span class="wj-input-group-btn" >' + '            <button class="wj-btn wj-btn-default" type="button"' + '               ng-click="cv.moveCurrentToNext()"' + '               ng-disabled="cv.currentPosition >= cv.itemCount - 1">' + '                <span class="wj-glyph-right"></span>' + '            </button>' + '        </span>' + '        <span class="wj-input-group-btn" >' + '            <button class="wj-btn wj-btn-default" type="button"' + '               ng-click="cv.moveCurrentToLast()"' + '               ng-disabled="cv.currentPosition >= cv.itemCount - 1">' + '                <span class="wj-glyph-right"></span>' + '                <span class="wj-glyph-right" style="margin-left: -4px;"></span>' + '            </button>' + '        </span>' + '    </div>' + '</div>';
            }
            WjCollectionViewNavigator.prototype._getMetaDataId = function () {
                return 'CollectionViewNavigator';
            };

            // Gets the WjCollectionViewNavigator directive's link function. Overrides parent member
            WjCollectionViewNavigator.prototype._postLinkFn = function () {
                return function (scope, tElement, tAttrs, dropDownController) {
                };
            };
            return WjCollectionViewNavigator;
        })(angular.WjDirective);

        /**
        * AngularJS directive for an @see:ICollectionView pager element.
        *
        * Use the <b>wj-collection-view-pager</b> directive to add an element that allows users to
        * navigate through the pages in a paged @see:ICollectionView.
        * Note that directive and parameter names must be formatted as lower-case with dashes
        * instead of camel-case. For example:
        *
        * <pre>Here is a CollectionViewPager:&lt;/p&gt;
        * &lt;wj-collection-view-pager
        *   cv="myCollectionView"&gt;
        * &lt;/wj-collection-view-pager&gt;</pre>
        *
        * @fiddle:s8tT4
        *
        * This example creates a CollectionView with 100,000 items and 20 items per page.
        * It defines a navigator to select the current page, another to select the current item,
        * and shows the data in a @see:FlexGrid.
        *
        * The <b>wj-collection-view-pager</b> directive has a single attribute:
        *
        * <dl class="dl-horizontal">
        *   <dt>cv</dt>  <dd><code>=</code> A reference to the paged @see:ICollectionView object to navigate.</dd>
        * </dl>
        */
        var WjCollectionViewPager = (function (_super) {
            __extends(WjCollectionViewPager, _super);
            // Initializes a new instance of a WjCollectionViewPager
            function WjCollectionViewPager() {
                _super.call(this);

                this.template = '<div class="wj-control wj-content wj-pager" >' + '    <div class="wj-input-group">' + '        <span class="wj-input-group-btn" >' + '            <button class="wj-btn wj-btn-default" type="button"' + '                ng-click="cv.moveToFirstPage()"' + '                ng-disabled="cv.pageIndex <= 0">' + '                <span class="wj-glyph-left" style="margin-right: -4px;"></span>' + '                <span class="wj-glyph-left"></span>' + '            </button>' + '        </span>' + '        <span class="wj-input-group-btn" >' + '        <button class="wj-btn wj-btn-default" type="button"' + '                ng-click="cv.moveToPreviousPage()"' + '                ng-disabled="cv.pageIndex <= 0">' + '                <span class="wj-glyph-left"></span>' + '            </button>' + '        </span>' + '        <input type="text" class="wj-form-control" value="' + '            {{cv.pageIndex + 1 | number}} / {{cv.pageCount | number}}' + '        " disabled />' + '        <span class="wj-input-group-btn" >' + '            <button class="wj-btn wj-btn-default" type="button"' + '                ng-click="cv.moveToNextPage()"' + '                ng-disabled="cv.pageIndex >= cv.pageCount - 1">' + '                <span class="wj-glyph-right"></span>' + '            </button>' + '        </span>' + '        <span class="wj-input-group-btn" >' + '            <button class="wj-btn wj-btn-default" type="button"' + '                ng-click="cv.moveToLastPage()"' + '                ng-disabled="cv.pageIndex >= cv.pageCount - 1">' + '                <span class="wj-glyph-right"></span>' + '                <span class="wj-glyph-right" style="margin-left: -4px;"></span>' + '            </button>' + '        </span>' + '    </div>' + '</div>';
            }
            WjCollectionViewPager.prototype._getMetaDataId = function () {
                return 'CollectionViewPager';
            };

            // Gets the WjCollectionViewPager directive's link function. Overrides parent member
            WjCollectionViewPager.prototype._postLinkFn = function () {
                return function (scope, tElement, tAttrs, dropDownController) {
                };
            };
            return WjCollectionViewPager;
        })(angular.WjDirective);
    })(wijmo.angular || (wijmo.angular = {}));
    var angular = wijmo.angular;
})(wijmo || (wijmo = {}));

var wijmo;
(function (wijmo) {
    //
    // AngularJS directives for wijmo.chart module
    //
    (function (angular) {
        //#region "Charts directives registration"
        var wijmoChart = window['angular'].module('wj.chart', []);

        // register only if module is loaded
        if (wijmo.chart) {
            wijmoChart.directive('wjFlexChart', [function () {
                    return new WjFlexChart();
                }]);

            wijmoChart.directive('wjFlexChartAxis', [function () {
                    return new WjFlexChartAxis();
                }]);

            wijmoChart.directive('wjFlexChartSeries', [function () {
                    return new WjFlexChartSeries();
                }]);

            wijmoChart.directive('wjFlexChartLegend', [function () {
                    return new WjFlexChartLegend();
                }]);

            wijmoChart.directive('wjFlexChartDataLabel', [function () {
                    return new WjFlexChartDataLabel();
                }]);

            wijmoChart.directive('wjFlexPieDataLabel', [function () {
                    return new WjFlexPieDataLabel();
                }]);

            wijmoChart.directive('wjFlexChartLineMarker', [function () {
                    return new WjFlexChartLineMarker();
                }]);

            if (wijmo.chart.interaction) {
                wijmoChart.directive('wjFlexChartRangeSelector', [function () {
                        return new WjFlexChartRangeSelector();
                    }]);
            }

            wijmoChart.directive('wjFlexPie', [function () {
                    return new WjFlexPie();
                }]);
        }

        //#endregion "Charts directives definitions"
        //#region "Charts directives classes"
        // Base class for WjFlexChart and FlexPie directives with common prop and event dictionaries
        var WjFlexChartBase = (function (_super) {
            __extends(WjFlexChartBase, _super);
            // Initializes a new instance of a WjFlexChart
            function WjFlexChartBase() {
                _super.call(this);

                var self = this;

                this.template = '<div ng-transclude />';
                this.transclude = true;
            }
            Object.defineProperty(WjFlexChartBase.prototype, "_controlConstructor", {
                get: function () {
                    return wijmo.chart.FlexChartBase;
                },
                enumerable: true,
                configurable: true
            });

            WjFlexChartBase.prototype._initProps = function () {
                _super.prototype._initProps.call(this);
                var self = this;
                var tooltipDesc = angular.MetaFactory.findProp('tooltipContent', this._props);
                tooltipDesc.customHandler = function (scope, control, value, oldValue, link) {
                    if (value != null) {
                        control.tooltip.content = value;
                    }
                };
            };
            return WjFlexChartBase;
        })(angular.WjDirective);

        /**
        * AngularJS directive for the @see:FlexChart control.
        *
        * Use the <b>wj-flex-chart</b> directive to add charts to your AngularJS applications.
        * Note that directive and parameter names must be formatted using lower-case letters
        * with dashes instead of camel case. For example:
        *
        * <pre>&lt;p&gt;Here is a FlexChart control:&lt;/p&gt;
        * &lt;wj-flex-chart
        *   style="height:300px"
        *   items-source="data"
        *   binding-x="country"&gt;
        *   &lt;wj-flex-chart-axis
        *     wj-property="axisY"
        *     major-unit="5000"&gt;
        *   &lt;/wj-flex-chart-axis&gt;
        *   &lt;wj-flex-chart-series
        *     binding="sales"
        *     name="Sales"&gt;
        *   &lt;/wj-flex-chart-series&gt;
        *   &lt;wj-flex-chart-series
        *     binding="expenses"
        *     name="Expenses"&gt;
        *   &lt;/wj-flex-chart-series&gt;
        *   &lt;wj-flex-chart-series
        *     binding="downloads"
        *     name="Downloads"
        *     chart-type="LineSymbols"&gt;
        *   &lt;/wj-flex-chart-series&gt;
        * &lt;/wj-flex-chart&gt;</pre>
        *
        * The example below creates a @see:FlexChart control and binds it to a 'data' array
        * exposed by the controller. The chart has three series objects, each corresponding to
        * a property in the objects contained in the source array. The last series in the
        * example uses the 'chart-type' attribute to override the default chart type used
        * for the other series objects.
        *
        * @fiddle:QNb9X
        *
        * The wj-flex-chart directive supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>binding</dt>           <dd><code>@</code> The name of the property that contains Y
        *                              values for the chart. You can override this at the series level.</dd>
        *   <dt>binding-x</dt>         <dd><code>@</code> The name of the property that contains X
        *                              values for the chart. You can override this at the series level.</dd>
        *   <dt>chart-type</dt>        <dd><code>@</code> The default chart type to use in rendering series
        *                              objects. You can override this at the series level. See @see:ChartType.</dd>
        *   <dt>control</dt>           <dd><code>=</code> A reference to the @see:FlexChart control
        *                              that this directive creates.</dd>
        *   <dt>footer</dt>            <dd><code>@</code> The text to display in the chart footer (plain
        *                              text).</dd>
        *   <dt>footer-style</dt>       <dd><code>=</code> The style to apply to the chart footer.</dd>
        *   <dt>header</dt>            <dd><code>@</code> The text to display in the chart header (plain
        *                              text).</dd>
        *   <dt>header-style</dt>      <dd><code>=</code> The style to apply to the chart header.</dd>
        *   <dt>initialized</dt>       <dd><code>&</code> This event occurs after the binding has finished
        *                              initializing the control with attribute values.</dd>
        *   <dt>is-initialized</dt><dd><code>=</code> A value indicating whether the binding has finished
        *                              initializing the control with attribute values. </dd>
        *   <dt>interpolate-nulls</dt> <dd><code>@</code> The value indicating whether to interpolate or
        *                              leave gaps when there are null values in the data.</dd>
        *   <dt>item-formatter</dt>    <dd><code>=</code> The formatter function that customizes the
        *                              appearance of data points.</dd>
        *   <dt>items-source</dt>      <dd><code>=</code> An array or @see:ICollectionView object that contains
        *                              the data used to create the chart.</dd>
        *   <dt>legend-toggle</dt>     <dd><code>@</code> The value indicating whether clicking legend items
        *                              toggles series visibility.</dd>
        *   <dt>options</dt>           <dd><code>=</code> Chart options that only apply to certain chart types.
        *                              See <b>options</b> under @see:FlexChart for details.</dd>
        *   <dt>palette</dt>           <dd><code>=</code> An array that contains the default colors used for
        *                              displaying each series.</dd>
        *   <dt>plot-margin</dt>       <dd><code>=</code> The number of pixels of space to leave between the
        *                              edges of the control and the plot area, or CSS-style margins.</dd>
        *   <dt>rotated</dt>           <dd><code>@</code> The value indicating whether to flip the axes so that
        *                              X is vertical and Y is horizontal.</dd>
        *   <dt>selection</dt>         <dd><code>=</code> The series object that is selected.</dd>
        *   <dt>selection-mode</dt>    <dd><code>@</code> The @see:SelectionMode value indicating whether or what is
        *                              selected when the user clicks a series.</dd>
        *   <dt>stacking</dt>          <dd><code>@</code> The @see:Stacking value indicating whether or how series
        *                              objects are stacked or plotted independently.</dd>
        *   <dt>symbol-size</dt>       <dd><code>@</code> The size in pixels of the symbols used to render data
        *                              points in Scatter, LineSymbols, and SplineSymbols charts. You can override
        *                              this at the series level.</dd>
        *   <dt>tooltip-content</dt>   <dd><code>@</code> The value to display in the
        *                              @see:ChartTooltip content property.</dd>
        *   <dt>rendering</dt>          <dd><code>&</code> The @see:rendering event handler.</dd>
        *   <dt>rendered</dt>          <dd><code>&</code> The @see:rendered event handler.</dd>
        *   <dt>series-visibility-changed</dt>
        *                              <dd><code>&</code> The @see:seriesVisibilityChanged event handler.</dd>
        *   <dt>selection-changed</dt> <dd><code>&</code> The @see:selectionChanged event handler.</dd>
        * </dl>
        *
        * The wj-flex-chart directive may contain the following child directives:
        * @see:WjFlexChartAxis, @see:WjFlexChartSeries, @see:WjFlexChartLegend and @see:WjFlexChartDataLabel.
        */
        var WjFlexChart = (function (_super) {
            __extends(WjFlexChart, _super);
            function WjFlexChart() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(WjFlexChart.prototype, "_controlConstructor", {
                // gets the Wijmo FlexChart control constructor
                get: function () {
                    return wijmo.chart.FlexChart;
                },
                enumerable: true,
                configurable: true
            });

            WjFlexChart.prototype._initProps = function () {
                _super.prototype._initProps.call(this);
                var self = this;

                var lblContentDesc = angular.MetaFactory.findProp('labelContent', this._props);
                lblContentDesc.customHandler = function (scope, control, value, oldValue, link) {
                    if (value != null) {
                        control.dataLabel.content = value;
                    }
                };
            };
            return WjFlexChart;
        })(WjFlexChartBase);

        /**
        * AngularJS directive for the @see:FlexChart @see:Axis object.
        *
        * The <b>wj-flex-chart-axis</b> directive must be contained in a @see:WjFlexChart directive.
        * It supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>wj-property</dt>     <dd><code>@</code> Defines the @see:FlexChart property name,
        *                            axis-x or axis-y, to initialize with the directive.</dd>
        *   <dt>axis-line</dt>       <dd><code>@</code> The value indicating whether the axis line is visible.</dd>
        *   <dt>format</dt>          <dd><code>@</code> The format string used for the axis labels
        *                            (see @see:wijmo.Globalize).</dd>
        *   <dt>labels</dt>          <dd><code>@</code> The value indicating whether the axis labels are visible.</dd>
        *   <dt>label-angle</dt>     <dd><code>@</code> The rotation angle of axis labels in degrees.</dd>
        *   <dt>major-grid</dt>      <dd><code>@</code> The value indicating whether the axis includes grid lines.</dd>
        *   <dt>major-tick-marks</dt><dd><code>@</code> Defines the appearance of tick marks on the axis
        *                            (see @see:TickMark).</dd>
        *   <dt>major-unit</dt>      <dd><code>@</code> The number of units between axis labels.</dd>
        *   <dt>max</dt>             <dd><code>@</code> The minimum value shown on the axis.</dd>
        *   <dt>min</dt>             <dd><code>@</code> The maximum value shown on the axis.</dd>
        *   <dt>minor-grid</dt>      <dd><code>@</code> The value indicating whether the axis includes minor grid lines.</dd>
        *   <dt>minor-tick-marks</dt><dd><code>@</code> Defines the appearance of minor tick marks on the axis
        *                            (see @see:TickMark).</dd>
        *   <dt>minor-unit</dt>      <dd><code>@</code> The number of units between minor axis ticks.</dd>
        *   <dt>origin</dt>          <dd><code>@</code> The axis origin.</dd>
        *   <dt>position</dt>        <dd><code>@</code> The @see:Position value indicating the position of the axis.</dd>
        *   <dt>reversed</dt>        <dd><code>@</code> The value indicating whether the axis is reversed (top to
        *                            bottom or right to left).</dd>
        *   <dt>title</dt>           <dd><code>@</code> The title text shown next to the axis.</dd>
        * </dl>
        */
        var WjFlexChartAxis = (function (_super) {
            __extends(WjFlexChartAxis, _super);
            // Initializes a new instance of a WjFlexCharAxis.
            function WjFlexChartAxis() {
                _super.call(this);

                this.require = ['?^wjFlexChartSeries', '?^wjFlexChart'];
                this.template = '<div class="wjFlexChartAxis" />';
            }
            Object.defineProperty(WjFlexChartAxis.prototype, "_controlConstructor", {
                get: function () {
                    return wijmo.chart.Axis;
                },
                enumerable: true,
                configurable: true
            });
            return WjFlexChartAxis;
        })(angular.WjDirective);

        /**
        * AngularJS directive for the @see:FlexChart @see:Legend object.
        *
        * The <b>wj-flex-chart-legend</b> directive must be contained in a @see:WjFlexChart directive.
        * It supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>position</dt>       <dd><code>@</code> The @see:Position value indicating the position of the
        *                           legend.</dd>
        * </dl>
        *
        * The example below shows how you can use the wj-flex-chart-legend directive
        * to change the position of the chart legend:
        *
        * <pre>&lt;wj-flex-chart
        *   items-source="data"
        *   binding-x="country"&gt;
        *   &lt;wj-flex-chart-axis
        *       wj-property="axisY"
        *       major-unit="5000"&gt;
        *     &lt;/wj-flex-chart-axis&gt;
        *     &lt;wj-flex-chart-series
        *       binding="sales"
        *       name="Sales"&gt;
        *     &lt;/wj-flex-chart-series&gt;
        *   &lt;wj-flex-chart-legend
        *     position="Bottom"&gt;
        *   &lt;/wj-flex-chart-legend&gt;
        * &lt;/wj-flex-chart&gt;</pre>
        */
        var WjFlexChartLegend = (function (_super) {
            __extends(WjFlexChartLegend, _super);
            // Initializes a new instance of a WjFlexChartLegend.
            function WjFlexChartLegend() {
                _super.call(this);

                this.require = ['?^wjFlexChart', '?^wjFlexPie'];
                this.template = '<div />';
            }
            Object.defineProperty(WjFlexChartLegend.prototype, "_controlConstructor", {
                get: function () {
                    return wijmo.chart.Legend;
                },
                enumerable: true,
                configurable: true
            });
            return WjFlexChartLegend;
        })(angular.WjDirective);

        // abstract
        var WjFlexChartDataLabelBase = (function (_super) {
            __extends(WjFlexChartDataLabelBase, _super);
            function WjFlexChartDataLabelBase() {
                _super.call(this);

                this.require = ['?^wjFlexChart', '?^wjFlexPie'];
                this.template = '<div />';
            }
            Object.defineProperty(WjFlexChartDataLabelBase.prototype, "_controlConstructor", {
                get: function () {
                    return wijmo.chart.DataLabelBase;
                },
                enumerable: true,
                configurable: true
            });
            return WjFlexChartDataLabelBase;
        })(angular.WjDirective);

        /**
        * AngularJS directive for the @see:FlexChart @see:DataLabel object.
        *
        * The <b>wj-flex-chart-data-label</b> directive must be contained in a @see:WjFlexChart directive.
        * It supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>content</dt>       <dd><code>=</code> A string or function that gets or sets the content of data labels.</dd>
        *   <dt>border</dt>        <dd><code>@</code> Gets or sets a value indicating whether data labels have borders.</dd>
        *   <dt>position</dt>      <dd><code>@</code> The @see:LabelPosition value indicating the position of data labels.</dd>
        * </dl>
        */
        var WjFlexChartDataLabel = (function (_super) {
            __extends(WjFlexChartDataLabel, _super);
            function WjFlexChartDataLabel() {
                _super.call(this);
                this.require = '^wjFlexChart';
            }
            Object.defineProperty(WjFlexChartDataLabel.prototype, "_controlConstructor", {
                get: function () {
                    return wijmo.chart.DataLabel;
                },
                enumerable: true,
                configurable: true
            });
            return WjFlexChartDataLabel;
        })(WjFlexChartDataLabelBase);

        /**
        * AngularJS directive for the @see:FlexPie @see:PieDataLabel object.
        *
        * The <b>wj-flex-pie-data-label</b> directive must be contained in a @see:WjFlexPie directive.
        * It supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>content</dt>       <dd><code>=</code> A string or function that gets or sets the content of data labels.</dd>
        *   <dt>border</dt>        <dd><code>@</code> Gets or sets a value indicating whether data labels have borders.</dd>
        *   <dt>position</dt>      <dd><code>@</code> The @see:PieLabelPosition value indicating the position of data labels.</dd>
        * </dl>
        */
        var WjFlexPieDataLabel = (function (_super) {
            __extends(WjFlexPieDataLabel, _super);
            function WjFlexPieDataLabel() {
                _super.call(this);
                this.require = '^wjFlexPie';
            }
            Object.defineProperty(WjFlexPieDataLabel.prototype, "_controlConstructor", {
                get: function () {
                    return wijmo.chart.PieDataLabel;
                },
                enumerable: true,
                configurable: true
            });
            return WjFlexPieDataLabel;
        })(WjFlexChartDataLabelBase);

        /**
        * AngularJS directive for the @see:FlexChart @see:Series object.
        *
        * The <b>wj-flex-chart-series</b> directive must be contained in a @see:WjFlexChart directive.
        * It supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>binding</dt>      <dd><code>@</code> The name of the property that contains Y values for the
        *                         series. This value overrides any binding set for the chart.</dd>
        *   <dt>binding-x</dt>    <dd><code>@</code> The name of the property that contains X values for the
        *                         series. This value overrides any binding set for the chart.</dd>
        *   <dt>chart-type</dt>   <dd><code>@</code> The chart type to use in rendering objects for this series
        *                         objects. This value overrides the default chart type set on the chart. See
        *                         @see:ChartType.</dd>
        *   <dt>css-class</dt>    <dd><code>@</code> The CSS class to use for the series.</dd>
        *   <dt>items-source</dt> <dd><code>=</code> An array or @see:ICollectionView object that contains
        *                         data for this series.</dd>
        *   <dt>name</dt>         <dd><code>@</code> The name of the series to show in the legend.</dd>
        *   <dt>style</dt>        <dd><code>=</code> The series style. Use ng-attr-style to specify the series
        *                         style object as an object. See the section on ngAttr attribute bindings in
        *                         <a target="_blank" href="https://docs.angularjs.org/guide/directive">
        *                         AngularJS Creating Custom Directives</a> and the <a target="_blank" href=
        *                         "http://demos.componentone.com/wijmo/5/Angular/FlexChartIntro/FlexChartIntro/#Styling">
        *                         FlexChart 101 Styling Series</a> sample for more information.</dd>
        *   <dt>symbol-marker</dt><dd><code>@</code> The shape of marker to use for the series. This value
        *                         overrides the default marker set on the chart. See @see:Marker.</dd>
        *   <dt>symbol-size</dt>  <dd><code>@</code> The size in pixels of the symbols used to render data
        *                         points in this series for Scatter, LineSymbols, and SplineSymbols charts.
        *                         This value overrides any set at the chart level.</dd>
        *   <dt>symbol-style</dt> <dd><code>=</code> The style of the symbols used to render data
        *                         points in this series for Scatter, LineSymbols, and SplineSymbols charts.
        *                         This value overrides any set at the chart level.</dd>
        *   <dt>visibility</dt>   <dd><code>=</code> The @see:SeriesVisibility value indicating whether and where to
        *                         display the series.</dd>
        * </dl>
        *
        * In most cases, the <b>wj-flex-chart-series</b> specifies only the "name" and "binding" properties.
        * The remaining values are inherited from the parent <b>wj-flex-chart</b> directive.
        */
        var WjFlexChartSeries = (function (_super) {
            __extends(WjFlexChartSeries, _super);
            // Initializes a new instance of a WjFlexChartSeries
            function WjFlexChartSeries() {
                _super.call(this);
                this.require = '^wjFlexChart';
                this.template = '<div class="wjFlexChartSeries" ng-transclude />';
                this.transclude = true;
            }
            Object.defineProperty(WjFlexChartSeries.prototype, "_controlConstructor", {
                // Returns constructor of related Wijmo object. Abstract member, must be overridden in inherited class
                get: function () {
                    return wijmo.chart.Series;
                },
                enumerable: true,
                configurable: true
            });
            return WjFlexChartSeries;
        })(angular.WjDirective);

        /**
        * AngularJS directive for the @see:FlexChart @see:LineMarker object.
        *
        * The <b>wj-flex-line-marker</b> directive must be contained in a @see:WjFlexChart directive.
        * It supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>isVisible</dt>             <dd><code>@</code> The value indicating whether the LineMarker is visible.</dd>
        *   <dt>seriesIndex</dt>           <dd><code>@</code> The index of the series in the chart in which the LineMarker appears.</dd>
        *   <dt>horizontalPosition</dt>    <dd><code>@</code> The horizontal position of the LineMarker relative to the plot area.</dd>
        *   <dt>content</dt>               <dd><code>@</code> The function that allows you to customize the text content of the LineMarker.</dd>
        *   <dt>verticalPosition</dt>      <dd><code>@</code> The vertical position of the LineMarker relative to the plot area.</dd>
        *   <dt>alignment</dt>             <dd><code>@</code> The @see:LineMarkerAlignment value indicating the alignment of the LineMarker content.</dd>
        *   <dt>lines</dt>                 <dd><code>@</code> The @see:LineMarkerLines value indicating the appearance of the LineMarker's lines.</dd>
        *   <dt>interaction</dt>           <dd><code>@</code> The @see:LineMarkerInteraction value indicating the interaction mode of the LineMarker.</dd>
        *   <dt>dragThreshold</dt>         <dd><code>@</code> The maximum distance from the horizontal or vertical line that you can drag the marker.</dd>
        *   <dt>dragContent</dt>           <dd><code>@</code> The value indicating whether you can drag the content of the marker when the interaction mode is "Drag."</dd>
        *   <dt>dragLines</dt>             <dd><code>@</code> The value indicating whether the lines are linked when you drag the horizontal or vertical line when the interaction mode is "Drag."</dd>
        * </dl>
        */
        var WjFlexChartLineMarker = (function (_super) {
            __extends(WjFlexChartLineMarker, _super);
            // Initializes a new instance of a WjFlexChartLineMarker
            function WjFlexChartLineMarker() {
                _super.call(this);
                this.require = '^wjFlexChart';
            }
            Object.defineProperty(WjFlexChartLineMarker.prototype, "_controlConstructor", {
                get: function () {
                    return wijmo.chart.LineMarker;
                },
                enumerable: true,
                configurable: true
            });
            return WjFlexChartLineMarker;
        })(angular.WjDirective);

        /**
        * AngularJS directive for the @see:FlexChart @see:wijmo.chart.interaction.RangeSelector object.
        *
        * The <b>wj-flex-range-selector</b> directive must be contained in a @see:WjFlexChart directive.
        * It supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>isVisible</dt>     <dd><code>@</code> The value indicating whether the RangeSelector is visible.</dd>
        *   <dt>min</dt>           <dd><code>@</code> The minimum value of the range.</dd>
        *   <dt>max</dt>           <dd><code>@</code> The maximum value of the range.</dd>
        *   <dt>orientation</dt>   <dd><code>@</code> The orientation of the RangeSelector.</dd>
        * </dl>
        */
        var WjFlexChartRangeSelector = (function (_super) {
            __extends(WjFlexChartRangeSelector, _super);
            // Initializes a new instance of a WjFlexChartRangeSelector
            function WjFlexChartRangeSelector() {
                _super.call(this);
                this.require = '^wjFlexChart';
            }
            Object.defineProperty(WjFlexChartRangeSelector.prototype, "_controlConstructor", {
                get: function () {
                    return wijmo.chart.interaction.RangeSelector;
                },
                enumerable: true,
                configurable: true
            });
            return WjFlexChartRangeSelector;
        })(angular.WjDirective);

        /**
        * AngularJS directive for the @see:FlexPie control.
        *
        * <dl class="dl-horizontal">
        *   <dt>items-source</dt>      <dd><code>=</code> An array or @see:ICollectionView
        *                              object that contains data for the chart.</dd>
        *   <dt>binding</dt>           <dd><code>@</code> The name of the property that
        *                              contains item values.</dd>
        *   <dt>binding-name</dt>      <dd><code>@</code> The name of the property that
        *                              contains item names.</dd>
        *   <dt>footer</dt>            <dd><code>@</code> The text to display in the chart footer (plain
        *                              text).</dd>
        *   <dt>footer-style</dt>       <dd><code>=</code> The style to apply to the chart footer.</dd>
        *   <dt>header</dt>            <dd><code>@</code> The text to display in the chart header (plain
        *                              text).</dd>
        *   <dt>header-style</dt>      <dd><code>=</code> The style to apply to the chart header.</dd>
        *   <dt>initialized</dt>       <dd><code>&</code> This event occurs after the binding has finished
        *                              initializing the control with attribute values.</dd>
        *   <dt>is-initialized</dt><dd><code>=</code> A value indicating whether the binding has finished
        *                              initializing the control with attribute values. </dd>
        *   <dt>inner-radius</dt>      <dd><code>@</code> The size of the hole inside the
        *                              pie, measured as a fraction of the pie radius.</dd>
        *   <dt>is-animated</dt>       <dd><code>@</code> A value indicating whether to use animation
        *                              to move selected items to the selectedItemPosition.</dd>
        *   <dt>item-formatter</dt>    <dd><code>=</code> The formatter function that customizes the
        *                              appearance of data points.</dd>
        *   <dt>offset</dt>            <dd><code>@</code> The extent to which pie slices are pulled
        *                              out from the center, as a fraction of the pie radius.</dd>
        *   <dt>palette</dt>           <dd><code>=</code> An array that contains the default colors used for
        *                              displaying pie slices.</dd>
        *   <dt>plot-margin</dt>       <dd><code>=</code> The number of pixels of space to leave between the
        *                              edges of the control and the plot area, or CSS-style margins.</dd>
        *   <dt>reversed</dt>          <dd><code>@</code> A value indicating whether to draw pie
        *                              slices in a counter-clockwise direction.</dd>
        *   <dt>start-angle</dt>       <dd><code>@</code> The starting angle for pie slices,
        *                              measured clockwise from the 9 o'clock position.</dd>
        *   <dt>selected-item-offset</dt>
        *                              <dd><code>@</code> The extent to which the selected pie slice is
        *                              pulled out from the center, as a fraction of the pie radius.</dd>
        *   <dt>selected-item-position</dt>
        *                              <dd><code>@</code> The @see:Position value indicating where to display
        *                              the selected slice.</dd>
        *   <dt>selection-mode</dt>    <dd><code>@</code> The @see:SelectionMode value indicating whether or what is
        *                              selected when the user clicks a series.</dd>
        *   <dt>tooltip-content</dt>   <dd><code>@</code> The value to display in the
        *                              @see:ChartTooltip content property.</dd>
        *   <dt>rendering</dt>         <dd><code>&</code> The @see:rendering event handler.</dd>
        *   <dt>rendered</dt>          <dd><code>&</code> The @see:rendered event handler.</dd>
        * </dl>
        *
        * The wj-flex-pie directive may contain the following child directives:
        * @see:WjFlexChartLegend and @see:WjFlexPieDataLabel.
        */
        var WjFlexPie = (function (_super) {
            __extends(WjFlexPie, _super);
            function WjFlexPie() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(WjFlexPie.prototype, "_controlConstructor", {
                // gets the Wijmo FlexPie control constructor
                get: function () {
                    return wijmo.chart.FlexPie;
                },
                enumerable: true,
                configurable: true
            });

            WjFlexPie.prototype._initProps = function () {
                _super.prototype._initProps.call(this);
                var self = this;

                var lblContentDesc = angular.MetaFactory.findProp('labelContent', this._props);
                lblContentDesc.customHandler = function (scope, control, value, oldValue, link) {
                    if (value != null) {
                        control.dataLabel.content = value;
                    }
                };
            };
            return WjFlexPie;
        })(WjFlexChartBase);
    })(wijmo.angular || (wijmo.angular = {}));
    var angular = wijmo.angular;
})(wijmo || (wijmo = {}));

var wijmo;
(function (wijmo) {
    //
    // AngularJS directives for wijmo.gauge module
    //
    (function (angular) {
        //#region "Gauge directives registration"
        var wijmoGauge = window['angular'].module('wj.gauge', []);

        // register only if module is loaded
        if (wijmo.gauge) {
            wijmoGauge.directive('wjLinearGauge', [function () {
                    return new WjLinearGauge();
                }]);

            wijmoGauge.directive('wjBulletGraph', [function () {
                    return new WjBulletGraph();
                }]);

            wijmoGauge.directive('wjRadialGauge', [function () {
                    return new WjRadialGauge();
                }]);

            wijmoGauge.directive('wjRange', [function () {
                    return new WjRange();
                }]);
        }

        //#endregion "Gauge directives definitions"
        //#region "Gauge directives classes"
        // Gauge control directive
        // Provides base setup for all directives related to controls derived from Gauge
        // Abstract class, not for use in markup
        var WjGauge = (function (_super) {
            __extends(WjGauge, _super);
            // Creates a new instance of a WjGauge
            function WjGauge() {
                _super.call(this);
                this.template = '<div ng-transclude />';
                this.transclude = true;
            }
            Object.defineProperty(WjGauge.prototype, "_controlConstructor", {
                get: function () {
                    return wijmo.gauge.Gauge;
                },
                enumerable: true,
                configurable: true
            });
            return WjGauge;
        })(angular.WjDirective);

        /**
        * AngularJS directive for the @see:LinearGauge control.
        *
        * Use the <b>wj-linear-gauge</b> directive to add linear gauges to your AngularJS applications.
        * Note that directive and parameter names must be formatted in lower-case with dashes
        * instead of camel-case. For example:
        *
        * <pre>&lt;wj-linear-gauge
        *   value="ctx.gauge.value"
        *   show-text="Value"
        *   is-read-only="false"&gt;
        *   &lt;wj-range
        *     wj-property="pointer"
        *     thickness="0.2"&gt;
        *     &lt;wj-range
        *       min="0"
        *       max="33"
        *       color="green"&gt;
        *     &lt;/wj-range&gt;
        *     &lt;wj-range
        *       min="33"
        *       max="66"
        *       color="yellow"&gt;
        *     &lt;/wj-range&gt;
        *     &lt;wj-range
        *       min="66"
        *       max="100"
        *       color="red"&gt;
        *     &lt;/wj-range&gt;
        *   &lt;/wj-range&gt;
        * &lt;/wj-linear-gauge&gt;</pre>
        *
        * The <b>wj-linear-gauge</b> directive supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>ng-model</dt>      <dd><code>@</code> Binds the control's <b>value</b> property using the ng-model Angular
        *                          directive. Binding the property using the ng-model directive provides standard benefits
        *                          like validation, adding the control's state to the form instance, and so on. To redefine
        *                          properties on a control that is bound by the ng-model directive, use the wj-model-property
        *                          attribute.</dd>
        *   <dt>wj-model-property</dt>  <dd><code>@</code> Specifies a control property that is bound to a scope using the
        *                               <b>ng-model</b> directive.</dd>
        *   <dt>control</dt>       <dd><code>=</code> A reference to the @see:LinearGauge
        *                          control created by this directive.</dd>
        *   <dt>direction</dt>     <dd><code>@</code> The @see:GaugeDirection value in
        *                          which the gauge fills as the value grows.</dd>
        *   <dt>format</dt>        <dd><code>@</code> The format string used for displaying
        *                          the gauge values as text.</dd>
        *   <dt>has-shadow</dt>    <dd><code>@</code> A value indicating whether the gauge
        *                          displays a shadow effect.</dd>
        *   <dt>initialized</dt>   <dd><code>&</code> This event occurs after the binding has finished
        *                          initializing the control with attribute values.</dd>
        *   <dt>is-initialized</dt> <dd><code>=</code> A value indicating whether the binding has finished
        *                           initializing the control with attribute values. </dd>
        *   <dt>is-animated</dt>   <dd><code>@</code> A value indicating whether the gauge
        *                          animates value changes.</dd>
        *   <dt>is-read-only</dt>  <dd><code>@</code> A value indicating whether users are
        *                          prevented from editing the value.</dd>
        *   <dt>min</dt>           <dd><code>@</code> The minimum value that the gauge
        *                          can display.</dd>
        *   <dt>max</dt>           <dd><code>@</code> The maximum value that the gauge
        *                          can display.</dd>
        *   <dt>show-text</dt>     <dd><code>@</code> The @see:ShowText value indicating
        *                          which values display as text within the gauge.</dd>
        *   <dt>step</dt>          <dd><code>@</code> The amount to add or subtract to the value
        *                          property when the user presses the arrow keys.</dd>
        *   <dt>thickness</dt>     <dd><code>@</code> The thickness of the gauge, on a scale
        *                          of zero to one.</dd>
        *   <dt>value</dt>         <dd><code>=</code> The value displayed on the gauge.</dd>
        * </dl>
        *
        * The <b>wj-linear-gauge</b> directive may contain one or more @see:WjRange directives.
        *
        * @fiddle:t842jozb
        */
        var WjLinearGauge = (function (_super) {
            __extends(WjLinearGauge, _super);
            // Initializes a new instance of a WjLinearGauge
            function WjLinearGauge() {
                _super.call(this);
            }
            Object.defineProperty(WjLinearGauge.prototype, "_controlConstructor", {
                // gets the Wijmo LinearGauge control constructor
                get: function () {
                    return wijmo.gauge.LinearGauge;
                },
                enumerable: true,
                configurable: true
            });
            return WjLinearGauge;
        })(WjGauge);

        /**
        * AngularJS directive for the @see:BulletGraph control.
        *
        * Use the <b>wj-bullet-graph</b> directive to add bullet graphs to your AngularJS applications.
        * Note that directive and parameter names must be formatted as lower-case with dashes
        * instead of camel-case. For example:
        *
        * <pre>&lt;wj-bullet-graph
        *   value="ctx.gauge.value"
        *   min="0" max="10"
        *   target="{&#8203;{item.target}}"
        *   bad="{&#8203;{item.target * .75}}"
        *   good="{&#8203;{item.target * 1.25}}"&gt;
        * &lt;/wj-bullet-graph&gt;</pre>
        *
        * The <b>wj-bullet-graph</b> directive supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>control</dt>       <dd><code>=</code> A reference to the BulletGraph control
        *                          created by this directive.</dd>
        *   <dt>direction</dt>     <dd><code>@</code> The @see:GaugeDirection value
        *                          indicating which direction the gauge fills as the value grows.</dd>
        *   <dt>initialized</dt>   <dd><code>&</code> This event occurs after the binding has finished
        *                          initializing the control with attribute values.</dd>
        *   <dt>is-initialized</dt> <dd><code>=</code> A value indicating whether the binding has finished
        *                           initializing the control with attribute values. </dd>
        *   <dt>target</dt>        <dd><code>@</code> The target value for the measure.</dd>
        *   <dt>good</dt>          <dd><code>@</code> A reference value considered good for the
        *                          measure.</dd>
        *   <dt>bad</dt>           <dd><code>@</code> A reference value considered bad for the
        *                          measure.</dd>
        *   <dt>value</dt>         <dd><code>=</code> The actual value of the measure.</dd>
        * </dl>
        *
        * The <b>wj-bullet-graph</b> directive may contain one or more @see:WjRange directives.
        *
        * @fiddle:8uxb1vwf
        */
        var WjBulletGraph = (function (_super) {
            __extends(WjBulletGraph, _super);
            // Initializes a new instance of a WjBulletGraph
            function WjBulletGraph() {
                _super.call(this);
            }
            Object.defineProperty(WjBulletGraph.prototype, "_controlConstructor", {
                // gets the Wijmo BulletGraph control constructor
                get: function () {
                    return wijmo.gauge.BulletGraph;
                },
                enumerable: true,
                configurable: true
            });
            return WjBulletGraph;
        })(WjLinearGauge);

        /**
        * AngularJS directive for the @see:RadialGauge control.
        *
        * Use the <b>wj-radial-gauge</b> directive to add radial gauges to your AngularJS applications.
        * Note that directive and parameter names must be formatted as lower-case with dashes
        * instead of camel-case. For example:
        *
        * <pre>Here is a &lt;b&gt;RadialGauge&lt;/b&gt; control:&lt;/p&gt;
        * &lt;wj-radial-gauge
        *   style="height:300px"
        *   value="count"
        *   min="0" max="10"
        *   is-read-only="false"&gt;
        * &lt;/wj-radial-gauge&gt;</pre>
        *
        * The <b>wj-radial-gauge</b> directive supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>ng-model</dt>      <dd><code>@</code> Binds the control's <b>value</b> property using the ng-model Angular
        *                          directive. Binding the property using the ng-model directive provides standard benefits
        *                          like validation, adding the control's state to the form instance, and so on. To redefine
        *                          properties on a control that is bound by the ng-model directive, use the wj-model-property
        *                          attribute.</dd>
        *   <dt>wj-model-property</dt>  <dd><code>@</code> Specifies a control property that is bound to a scope using the
        *                               <b>ng-model</b> directive.</dd>
        *   <dt>control</dt>       <dd><code>=</code> A reference to the RadialGauge
        *                          control created by this directive.</dd>
        *   <dt>auto-scale</dt>    <dd><code>@</code> A value indicating whether the gauge
        *                          scales the display to fill the host element.</dd>
        *   <dt>format</dt>        <dd><code>@</code> The format string used for displaying
        *                          gauge values as text.</dd>
        *   <dt>has-shadow</dt>    <dd><code>@</code> A value indicating whether the gauge
        *                          displays a shadow effect.</dd>
        *   <dt>initialized</dt>   <dd><code>&</code> This event occurs after the binding has finished
        *                          initializing the control with attribute values.</dd>
        *   <dt>is-initialized</dt> <dd><code>=</code> A value indicating whether the binding has finished
        *                           initializing the control with attribute values. </dd>
        *   <dt>is-animated</dt>   <dd><code>@</code> A value indicating whether the gauge
        *                          animates value changes.</dd>
        *   <dt>is-read-only</dt>  <dd><code>@</code> A value indicating whether users are
        *                          prevented from editing the value.</dd>
        *   <dt>min</dt>           <dd><code>@</code> The minimum value that the gauge
        *                          can display.</dd>
        *   <dt>max</dt>           <dd><code>@</code> The maximum value that the gauge
        *                          can display.</dd>
        *   <dt>show-text</dt>     <dd><code>@</code> A @see:ShowText value indicating
        *                          which values display as text within the gauge.</dd>
        *   <dt>step</dt>          <dd><code>@</code> The amount to add or subtract to the
        *                          value property when the user presses the arrow keys.</dd>
        *   <dt>start-angle</dt>   <dd><code>@</code> The starting angle for the gauge, in
        *                          degreees, measured clockwise from the 9 o'clock position.</dd>
        *   <dt>sweep-angle</dt>   <dd><code>@</code> The sweeping angle for the gauge in degrees
        *                          (may be positive or negative).</dd>
        *   <dt>thickness</dt>     <dd><code>@</code> The thickness of the gauge, on a scale
        *                          of zero to one.</dd>
        *   <dt>value</dt>         <dd><code>=</code> The value displayed on the gauge.</dd>
        * </dl>
        *
        * The <b>wj-radial-gauge</b> directive may contain one or more @see:WjRange directives.
        *
        * @fiddle:7ec2144u
        */
        var WjRadialGauge = (function (_super) {
            __extends(WjRadialGauge, _super);
            // Initializes a new instance of a WjRadialGauge
            function WjRadialGauge() {
                _super.call(this);
            }
            Object.defineProperty(WjRadialGauge.prototype, "_controlConstructor", {
                // gets the Wijmo RadialGauge control constructor
                get: function () {
                    return wijmo.gauge.RadialGauge;
                },
                enumerable: true,
                configurable: true
            });
            return WjRadialGauge;
        })(WjGauge);

        /**
        * AngularJS directive for the @see:Range object.
        *
        * The <b>wj-range</b> directive must be contained in a @see:WjLinearGauge, @see:WjRadialGauge
        * or @see:WjBulletGraph directive. It adds the Range object to the 'ranges' array property
        * of the parent directive. You may also initialize other Range type properties of the parent
        * directive by specifying the property name with the wj-property attribute.
        *
        * For example:
        * <pre>&lt;wj-radial-gauge
        *     min="0"
        *     max="200"
        *     step="20"
        *     value="theValue"
        *     is-read-only="false"&gt;
        *     &lt;wj-range
        *       min="0"
        *       max="100"
        *       color="red"&gt;
        *     &lt;/wj-range&gt;
        *     &lt;wj-range
        *       min="100"
        *       max="200"
        *       color="green"&gt;
        *     &lt;/wj-range&gt;
        *     &lt;wj-range
        *       wj-property="pointer"
        *       color="blue"&gt;
        *     &lt;/wj-range&gt;
        * &lt;/wj-radial-gauge&gt;</pre>
        *
        * The <b>wj-range</b> directive supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>min</dt>           <dd><code>@</code> The minimum value in the range.</dd>
        *   <dt>max</dt>           <dd><code>@</code> The maximum value in the range.</dd>
        *   <dt>color</dt>         <dd><code>@</code> The color used to display the range.</dd>
        *   <dt>thickness</dt>     <dd><code>@</code> The thickness of the range, on a scale
        *                          of zero to one.</dd>
        *   <dt>name</dt>          <dd><code>@</code> The name of the range.</dd>
        *   <dt>wj-property</dt>   <dd><code>@</code> The name of the property to initialize
        *                          with this directive.</dd>
        * </dl>
        */
        var WjRange = (function (_super) {
            __extends(WjRange, _super);
            // Initializes a new instance of a WjRange
            function WjRange() {
                _super.call(this);
                this.require = ['?^wjLinearGauge', '?^wjRadialGauge', '?^wjBulletGraph'];
                this.template = '<div ng-transclude />';
                this.transclude = true;

                // set up as a child directive
                this._property = 'ranges';
                this._isPropertyArray = true;
            }
            Object.defineProperty(WjRange.prototype, "_controlConstructor", {
                // Returns constructor of related Wijmo object. Abstract member, must be overridden in inherited class
                get: function () {
                    return wijmo.gauge.Range;
                },
                enumerable: true,
                configurable: true
            });
            return WjRange;
        })(angular.WjDirective);
    })(wijmo.angular || (wijmo.angular = {}));
    var angular = wijmo.angular;
})(wijmo || (wijmo = {}));

var wijmo;
(function (wijmo) {
    //
    // AngularJS directives for wijmo.grid module
    //
    (function (angular) {
        //#region "Grid directives registration"
        var wijmoGrid = window['angular'].module('wj.grid', []);

        // register only if module is loaded
        if (wijmo.grid) {
            wijmoGrid.directive('wjFlexGrid', [
                '$compile', '$interpolate', function ($compile, $interpolate) {
                    return new WjFlexGrid($compile, $interpolate);
                }]);

            wijmoGrid.directive('wjFlexGridColumn', [
                '$compile', function ($compile) {
                    return new WjFlexGridColumn($compile);
                }]);

            wijmoGrid.directive('wjFlexGridCellTemplate', [function () {
                    return new WjFlexGridCellTemplate();
                }]);

            if (wijmo.grid.filter) {
                wijmoGrid.directive('wjFlexGridFilter', [function () {
                        return new WjFlexGridFilter();
                    }]);
            }

            if (wijmo.grid.grouppanel) {
                wijmoGrid.directive('wjGroupPanel', [function () {
                        return new WjGroupPanel();
                    }]);
            }
        }

        //#endregion "Grid directives definitions"
        //#region "Grid directives classes"
        /**
        * AngularJS directive for the @see:FlexGrid control.
        *
        * Use the <b>wj-flex-grid</b> directive to add grids to your AngularJS applications.
        * Note that directive and parameter names must be formatted as lower-case with dashes
        * instead of camel-case. For example:
        *
        * <pre>&lt;p&gt;Here is a FlexGrid control:&lt;/p&gt;
        * &lt;wj-flex-grid items-source="data"&gt;
        *   &lt;wj-flex-grid-column
        *     header="Country"
        *     binding="country"&gt;
        *   &lt;/wj-flex-grid-column&gt;
        *   &lt;wj-flex-grid-column
        *     header="Sales"
        *     binding="sales"&gt;
        *   &lt;/wj-flex-grid-column&gt;
        *   &lt;wj-flex-grid-column
        *     header="Expenses"
        *     binding="expenses"&gt;
        *   &lt;/wj-flex-grid-column&gt;
        *   &lt;wj-flex-grid-column
        *     header="Downloads"
        *     binding="downloads"&gt;
        *   &lt;/wj-flex-grid-column&gt;
        * &lt;/wj-flex-grid&gt;</pre>
        *
        * The example below creates a FlexGrid control and binds it to a 'data' array
        * exposed by the controller. The grid has three columns, each corresponding to
        * a property of the objects contained in the source array.
        *
        * @fiddle:QNb9X
        *
        * The <b>wj-flex-grid</b> directive supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>allowAddNew</dt>              <dd><code>@</code> A value indicating whether to show a new row
        *                                     template so users can add items to the source collection.</dd>
        *   <dt>allow-delete</dt>             <dd><code>@</code> A value indicating whether the grid deletes
        *                                     selected rows when the user presses the Delete key.</dd>
        *   <dt>allow-dragging</dt>           <dd><code>@</code> An @see:AllowDragging value indicating
        *                                     whether and how the user can drag rows and columns with the mouse.</dd>
        *   <dt>allow-merging</dt>            <dd><code>@</code> An @see:AllowMerging value indicating
        *                                     which parts of the grid provide cell merging.</dd>
        *   <dt>allow-resizing</dt>           <dd><code>@</code> An @see:AllowResizing value indicating
        *                                     whether users are allowed to resize rows and columns with the mouse.</dd>
        *   <dt>allow-sorting</dt>            <dd><code>@</code> A boolean value indicating whether users can sort
        *                                     columns by clicking the column headers.</dd>
        *   <dt>auto-generate-columns</dt>    <dd><code>@</code> A boolean value indicating whether the grid generates
        *                                     columns automatically based on the <b>items-source</b>.</dd>
        *   <dt>child-items-path</dt>         <dd><code>@</code> The name of the property used to generate
        *                                     child rows in hierarchical grids.</dd>
        *   <dt>control</dt>                  <dd><code>=</code> A reference to the @see:FlexGrid control
        *                                     created by this directive.</dd>
        *   <dt>defer-resizing</dt>           <dd><code>=</code> A boolean value indicating whether row and column
        *                                     resizing should be deferred until the user releases the mouse button.</dd>
        *   <dt>frozen-columns</dt>           <dd><code>@</code> The number of frozen (non-scrollable) columns in the grid.</dd>
        *   <dt>frozen-rows</dt>              <dd><code>@</code> The number of frozen (non-scrollable) rows in the grid.</dd>
        *   <dt>group-header-format</dt>      <dd><code>@</code> The format string used to create the group
        *                                     header content.</dd>
        *   <dt>headers-visibility</dt>       <dd><code>=</code> A @see:HeadersVisibility value
        *                                     indicating whether the row and column headers are visible. </dd>
        *   <dt>initialized</dt>              <dd><code>&</code> This event occurs after the binding has finished
        *                                     initializing the control with attribute values.</dd>
        *   <dt>is-initialized</dt>           <dd><code>=</code> A value indicating whether the binding has finished
        *                                     initializing the control with attribute values. </dd>
        *   <dt>item-formatter</dt>           <dd><code>=</code> A function that customizes
        *                                     cells on this grid.</dd>
        *   <dt>items-source</dt>             <dd><code>=</code> An array or @see:ICollectionView object that
        *                                     contains the items shown on the grid.</dd>
        *   <dt>is-read-only</dt>             <dd><code>@</code> A boolean value indicating whether the user is
        *                                     prevented from editing grid cells by typing into them.</dd>
        *   <dt>merge-manager</dt>            <dd><code>=</code> A @see:MergeManager object that specifies
        *                                     the merged extent of the specified cell.</dd>
        *   <dt>selection-mode</dt>           <dd><code>@</code> A @see:SelectionMode value
        *                                     indicating whether and how the user can select cells.</dd>
        *   <dt>show-groups</dt>              <dd><code>@</code> A boolean value indicating whether to insert group
        *                                     rows to delimit data groups.</dd>
        *   <dt>show-sort</dt>                <dd><code>@</code> A boolean value indicating whether to display sort
        *                                     indicators in the column headers.</dd>
        *   <dt>sort-row-index</dt>           <dd><code>@</code> A number specifying the index of row in the column
        *                                     header panel that shows and changes the current sort.</dd>
        *   <dt>tree-indent</dt>              <dd><code>@</code> The indentation, in pixels, used to offset row
        *                                     groups of different levels.</dd>
        *   <dt>beginning-edit</dt>           <dd><code>&</code> Handler for the @see:beginningEdit event.</dd>
        *   <dt>cell-edit-ended</dt>          <dd><code>&</code> Handler for the @see:cellEditEnded event.</dd>
        *   <dt>cell-edit-ending</dt>         <dd><code>&</code> Handler for the @see:cellEditEnding event.</dd>
        *   <dt>prepare-cell-for-edit</dt>    <dd><code>&</code> Handler for the @see:prepareCellForEdit event.</dd>
        *   <dt>resizing-column</dt>          <dd><code>&</code> Handler for the @see:resizingColumn event.</dd>
        *   <dt>resized-column</dt>           <dd><code>&</code> Handler for the @see:resizedColumn event.</dd>
        *   <dt>dragged-column</dt>           <dd><code>&</code> Handler for the @see:draggedColumn event.</dd>
        *   <dt>dragging-column</dt>          <dd><code>&</code> Handler for the @see:draggingColumn event.</dd>
        *   <dt>sorted-column</dt>            <dd><code>&</code> Handler for the @see:sortedColumn event.</dd>
        *   <dt>sorting-column</dt>           <dd><code>&</code> Handler for the @see:sortingColumn event.</dd>
        *   <dt>deleting-row</dt>             <dd><code>&</code> Handler for the @see:deletingRow event.</dd>
        *   <dt>dragging-row</dt>             <dd><code>&</code> Handler for the @see:draggingRow event.</dd>
        *   <dt>dragged-row</dt>              <dd><code>&</code> Handler for the @see:draggedRow event.</dd>
        *   <dt>resizing-row</dt>             <dd><code>&</code> Handler for the @see:resizingRow event.</dd>
        *   <dt>resized-row</dt>              <dd><code>&</code> Handler for the @see:resizedRow event.</dd>
        *   <dt>row-added</dt>                <dd><code>&</code> Handler for the @see:rowAdded event.</dd>
        *   <dt>row-edit-ended</dt>           <dd><code>&</code> Handler for the @see:rowEditEnded event.</dd>
        *   <dt>row-edit-ending</dt>          <dd><code>&</code> Handler for the @see:rowEditEnding event.</dd>
        *   <dt>loaded-rows</dt>              <dd><code>&</code> Handler for the @see:loadedRows event.</dd>
        *   <dt>loading-rows</dt>             <dd><code>&</code> Handler for the @see:loadingRows event.</dd>
        *   <dt>group-collapsed-changed</dt>  <dd><code>&</code> Handler for the @see:groupCollapsedChanged event.</dd>
        *   <dt>group-collapsed-changing</dt> <dd><code>&</code> Handler for the @see:groupCollapsedChanging event.</dd>
        *   <dt>items-source-changed</dt>     <dd><code>&</code> Handler for the @see:itemsSourceChanged event.</dd>
        *   <dt>selection-changing</dt>       <dd><code>&</code> Handler for the @see:selectionChanging event.</dd>
        *   <dt>selection-changed</dt>        <dd><code>&</code> Handler for the @see:selectionChanged event.</dd>
        *   <dt>scroll-position-changed</dt>  <dd><code>&</code> Handler for the @see:scrollPositionChanged event.</dd>
        * </dl>
        *
        * The <b>wj-flex-grid</b> directive may contain @see:WjFlexGridColumn and @see:WjFlexGridCellTemplate child directives.
        */
        var WjFlexGrid = (function (_super) {
            __extends(WjFlexGrid, _super);
            // Initializes a new instance of a WjFlexGrid
            function WjFlexGrid($compile, $interpolate) {
                this._$compile = $compile;
                this._$interpolate = $interpolate;
                var self = this;

                _super.call(this);

                this.transclude = true;
                this.template = '<div ng-transclude />';
            }
            Object.defineProperty(WjFlexGrid.prototype, "_controlConstructor", {
                // Gets the Wijmo FlexGrid control constructor
                get: function () {
                    return wijmo.grid.FlexGrid;
                },
                enumerable: true,
                configurable: true
            });

            WjFlexGrid.prototype._createLink = function () {
                return new WjFlexGridLink();
            };

            // Initializes WjFlexGrid property map
            WjFlexGrid.prototype._initProps = function () {
                angular.MetaFactory.findProp('childItemsPath', this._props).initialize({ scopeBindingMode: '@' });
                angular.MetaFactory.findProp('itemFormatter', this._props).initialize({ customHandler: this._formatterPropHandler.bind(this) });
            };

            // Under 1.1.4+ undefined scope value is updated by processing control's events, as a result of
            // scope- > control transfer.
            // The _wrapperFormatter handler overwrites user's item formatter. This handler tries to retain it.
            WjFlexGrid.prototype._formatterPropHandler = function (scope, control, value, oldValue, link) {
                var name = 'itemFormatter';
                var ctrlVal = control[name], isNew = ctrlVal !== link._wrapperFormatter, isOld = oldValue !== link._wrapperFormatter;
                if (isNew || isOld) {
                    var newUserFormatter = isNew ? ctrlVal : oldValue;
                    link._userFormatter = newUserFormatter;
                    if (isNew) {
                        control[name] = link._wrapperFormatter;
                        if (link._canApply(scope, name)) {
                            scope[name] = link._wrapperFormatter;
                        }
                    }
                }
            };
            return WjFlexGrid;
        })(angular.WjDirective);
        angular.WjFlexGrid = WjFlexGrid;

        var WjFlexGridLink = (function (_super) {
            __extends(WjFlexGridLink, _super);
            function WjFlexGridLink() {
                _super.apply(this, arguments);
            }
            //override
            WjFlexGridLink.prototype._onChildrenReady = function () {
                // apply column info
                var self = this, control = this.control, scope = this.scope;

                // honor cell templates
                self._userFormatter = control.itemFormatter;
                control.itemFormatter = self._wrapperFormatter = function (panel, rowIndex, colIndex, cell) {
                    // restore overflow for any cell
                    if (cell.style.overflow) {
                        cell.style.overflow = '';
                    }

                    // call original formatter if any
                    if (self._userFormatter) {
                        self._userFormatter(panel, rowIndex, colIndex, cell);
                    }

                    //var editRange = panel.grid.editRange;
                    var grid = panel.grid, editRange = grid.editRange, templateType, row = panel.rows[rowIndex], dataItem = row.dataItem, isGridCtx = false, needCellValue = false, isEdit = false, isCvGroup = false;

                    switch (panel.cellType) {
                        case wijmo.grid.CellType.Cell:
                            if (row instanceof wijmo.grid.GroupRow) {
                                isCvGroup = dataItem instanceof wijmo.collections.CollectionViewGroup;
                                var isHierNonGroup = !(isCvGroup || row.hasChildren);
                                if (colIndex == panel.columns.firstVisibleIndex) {
                                    //templateType = isHierNonGroup ? CellTemplateType.Group : CellTemplateType.GroupHeader;
                                    templateType = isHierNonGroup ? CellTemplateType.Cell : CellTemplateType.GroupHeader;
                                } else {
                                    //templateType = CellTemplateType.Group;
                                    templateType = isHierNonGroup ? CellTemplateType.Cell : CellTemplateType.Group;
                                    needCellValue = true;
                                }
                            } else if (editRange && editRange.row === rowIndex && editRange.col === colIndex) {
                                templateType = CellTemplateType.CellEdit;
                                needCellValue = isEdit = true;
                            } else {
                                templateType = CellTemplateType.Cell;
                            }
                            break;
                        case wijmo.grid.CellType.ColumnHeader:
                            templateType = CellTemplateType.ColumnHeader;
                            break;
                        case wijmo.grid.CellType.RowHeader:
                            templateType = grid.collectionView && grid.collectionView.currentEditItem === dataItem ? CellTemplateType.RowHeaderEdit : CellTemplateType.RowHeader;
                            isGridCtx = true;
                            break;
                        case wijmo.grid.CellType.TopLeft:
                            templateType = CellTemplateType.TopLeft;
                            isGridCtx = true;
                            break;
                    }

                    if (templateType == null) {
                        return;
                    }

                    var col = (isCvGroup && templateType == CellTemplateType.GroupHeader ? grid.columns.getColumn(dataItem.groupDescription['propertyName']) : panel.columns[colIndex]);
                    if (!col) {
                        return;
                    }
                    var templContextProp = WjFlexGridCellTemplate._getTemplContextProp(templateType), templContext = (isGridCtx ? grid : col)[templContextProp];

                    // maintain template inheritance
                    if (!templContext) {
                        if (templateType === CellTemplateType.RowHeaderEdit) {
                            templateType = CellTemplateType.RowHeader;
                            templContextProp = WjFlexGridCellTemplate._getTemplContextProp(templateType);
                            templContext = grid[templContextProp];
                        } else if (templateType === CellTemplateType.Group || templateType === CellTemplateType.GroupHeader) {
                            if (!isCvGroup) {
                                templateType = CellTemplateType.Cell;
                                templContextProp = WjFlexGridCellTemplate._getTemplContextProp(templateType);
                                templContext = col[templContextProp];
                            }
                        }
                        if (!templContext) {
                            return;
                        }
                    }

                    // apply directive template and style
                    var tpl = self._getCellTemplate(templContext.cellTemplate), cellValue;
                    if (needCellValue) {
                        cellValue = panel.getCellData(rowIndex, colIndex, false);
                    }

                    // apply cell template
                    if (!wijmo.isNullOrWhiteSpace(tpl)) {
                        // if this is false then we can't reuse previously cached scope and linked tree.
                        var cellContext = (cell[templContextProp] || {}), isForeignCell = cellContext.column !== col;

                        // create a new cell scope, as a child of the column's parent scope
                        // (which could be ng-repeat with its specific properties),
                        // or reuse the one created earlier for this cell and cached in the
                        // cellContext.cellScope property.
                        // in any case initialize the scope with cell specific properties.
                        var cellScope = cellContext.cellScope;
                        if (isForeignCell) {
                            cellContext.cellScope = cellScope = templContext.templLink.scope.$parent.$new();
                            cellContext.column = col;
                            cell[templContextProp] = cellContext;
                        }

                        self._initCellScope(cellScope, row, col, dataItem, cellValue);

                        // compile column template to get a link function, or reuse the
                        // link function got earlier for this column and cached in the
                        // templContext.cellLink property.
                        var cellLink = templContext.cellLink;
                        if (!cellLink) {
                            cellLink = templContext.cellLink = self.directive._$compile('<div>' + tpl + '</div>');
                        }

                        // link the cell template to the cell scope and get a bound DOM
                        // subtree to use as the cell content,
                        // or reuse the bound subtree linked earlier and cached in the
                        // cellContext.clonedElement property.
                        // we pass a clone function to the link function to force it to
                        // return a clone of the template.
                        var clonedElement = cellContext.clonedElement;
                        if (isForeignCell) {
                            cellContext.clonedElement = clonedElement = cellLink(cellScope, function (clonedEl, scope) {
                            });
                        }

                        // insert the bound content subtree to the cell,
                        // after $apply to prevent flickering.
                        var replaceFirst = false;
                        if (isEdit) {
                            var rootEl = cell.childNodes[0];
                            if (rootEl && rootEl.nodeType == 1) {
                                cell.childNodes[0].style.display = 'none';
                            }
                        } else {
                            replaceFirst = cell.childNodes.length == 1;
                            if (!replaceFirst) {
                                cell.textContent = '';
                            }
                        }
                        if (replaceFirst) {
                            cell.replaceChild(clonedElement[0], cell.firstChild);
                        } else {
                            cell.appendChild(clonedElement[0]);
                        }
                        if (templContext.cellOverflow) {
                            cell.style.overflow = templContext.cellOverflow;
                        }

                        // apply new scope values to the bound cell content subtree.
                        // Do it before inserting the subtree in order to prevent flickering.
                        if (!cellScope.$root.$$phase) {
                            cellScope.$apply();
                        }

                        // increase row height if cell doesn't fit in the current row height.
                        setTimeout(function () {
                            var cellHeight = cell.scrollHeight, panelRows = panel.rows;
                            if (rowIndex < panelRows.length && panelRows[rowIndex].renderHeight < cellHeight) {
                                panelRows.defaultSize = cellHeight;
                            }
                        }, 0);
                    }

                    // apply cell style
                    var cellStyle = templContext.cellStyle;
                    if (cellStyle) {
                        // build cell style object
                        var cellScope = self._initCellScope({}, row, col, dataItem, cellValue), style = scope.$parent.$eval(cellStyle, cellScope);

                        // apply style to cell
                        if (style) {
                            var rootElement = document.createElement('div');

                            while (cell.firstChild) {
                                rootElement.appendChild(cell.firstChild);
                            }
                            cell.appendChild(rootElement);
                            for (var key in style) {
                                rootElement.style[key] = style[key];
                            }
                        }
                    }

                    if (isEdit) {
                        var editEndingEH = function (s, e) {
                            grid.cellEditEnding.removeHandler(editEndingEH);
                            if (!e.cancel) {
                                e.cancel = true;
                                panel.grid.setCellData(rowIndex, colIndex, cellScope.$value);
                            }

                            // close all open dropdowns
                            var dropDowns = cell.querySelectorAll('.wj-dropdown');
                            [].forEach.call(dropDowns, function (el) {
                                var ctrl = wijmo.Control.getControl(el);
                                if (ctrl && ctrl instanceof wijmo.input.DropDown) {
                                    ctrl.isDroppedDown = false;
                                }
                            });
                        };

                        // subscribe the handler to the cellEditEnding event
                        grid.cellEditEnding.addHandler(editEndingEH);
                    }
                };
            };

            WjFlexGridLink.prototype._initCellScope = function (scope, row, col, dataItem, cellValue) {
                scope.$row = row;
                scope.$col = col;
                scope.$item = dataItem;
                scope.$value = cellValue;
                return scope;
            };

            WjFlexGridLink.prototype._getCellTemplate = function (tpl) {
                if (tpl) {
                    tpl = tpl.replace(/ class\=\"ng\-scope\"( \"ng\-binding\")?/g, '');
                    tpl = tpl.replace(/<span>\s*<\/span>/g, '');
                    tpl = tpl.trim();
                }
                return tpl;
            };
            return WjFlexGridLink;
        })(angular.WjLink);
        angular.WjFlexGridLink = WjFlexGridLink;

        /**
        * AngularJS directive for the @see:FlexGrid @see:Column object.
        *
        * The <b>wj-flex-grid-column</b> directive must be contained in a @see:WjFlexGrid directive.
        * It supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>aggregate</dt>         <dd><code>@</code> The @see:Aggregate object to display in
        *                              the group header rows for this column.</dd>
        *   <dt>align</dt>             <dd><code>@</code> The string value that sets the horizontal
        *                              alignment of items in the column to left, right, or center.</dd>
        *   <dt>allow-dragging</dt>    <dd><code>@</code> The value indicating whether the user can move
        *                              the column to a new position with the mouse.</dd>
        *   <dt>allow-sorting</dt>     <dd><code>@</code> The value indicating whether the user can sort
        *                              the column by clicking its header.</dd>
        *   <dt>allow-resizing</dt>    <dd><code>@</code> The value indicating whether the user can
        *                              resize the column with the mouse.</dd>
        *   <dt>allow-merging</dt>     <dd><code>@</code> The value indicating whether the user can merge
        *                              cells in the column.</dd>
        *   <dt>binding</dt>           <dd><code>@</code> The name of the property to which the column is
        *                              bound.</dd>
        *   <dt>css-class</dt>         <dd><code>@</code> The name of a CSS class to use when
        *                              rendering the column.</dd>
        *   <dt>data-map</dt>          <dd><code>=</code> The @see:DataMap object to use to convert raw
        *                              values into display values for the column.</dd>
        *   <dt>data-type</dt>         <dd><code>@</code> The enumerated @see:DataType value that indicates
        *                              the type of value stored in the column.</dd>
        *   <dt>format</dt>            <dd><code>@</code> The format string to use to convert raw values
        *                              into display values for the column (see @see:Globalize).</dd>
        *   <dt>header</dt>            <dd><code>@</code> The string to display in the column header.</dd>
        *   <dt>input-type</dt>        <dd><code>@</code> The type attribute to specify the input element
        *                              used to edit values in the column. The default is "tel" for numeric
        *                              columns, and "text" for all other non-Boolean columns.</dd>
        *   <dt>is-content-html</dt>   <dd><code>@</code> The value indicating whether cells in the column
        *                              contain HTML content rather than plain text.</dd>
        *   <dt>is-read-only</dt>      <dd><code>@</code> The value indicating whether the user is prevented
        *                              from editing values in the column.</dd>
        *   <dt>is-selected</dt>       <dd><code>@</code> The value indicating whether the column is selected.</dd>
        *   <dt>mask</dt>              <dd><code>@</code> The mask string used to edit values in the
        *                              column.</dd>
        *   <dt>max-width</dt>         <dd><code>@</code> The maximum width for the column.</dd>
        *   <dt>min-width</dt>         <dd><code>@</code> The minimum width for the column.</dd>
        *   <dt>name</dt>              <dd><code>@</code> The column name. You can use it to retrieve the
        *                              column.</dd>
        *   <dt>required</dt>          <dd><code>@</code> The value indicating whether the column must contain
        *                              non-null values.</dd>
        *   <dt>show-drop-down</dt>    <dd><code>@</code> The value indicating whether to show drop-down buttons
        *                              for editing based on the column's @see:DataMap.</dd>
        *   <dt>visible</dt>           <dd><code>@</code> The value indicating whether the column is visible.</dd>
        *   <dt>width</dt>             <dd><code>@</code> The width of the column in pixels or as a
        *                              star value.</dd>
        *   <dt>word-wrap</dt>         <dd><code>@</code> The value indicating whether cells in the column wrap
        *                              their content.</dd>
        * </dl>
        *
        * Any html content within the <b>wj-flex-grid-column</b> directive is treated as a template for the cells in that column.
        * The template is applied only to regular cells. If you wish to apply templates to specific cell types such as
        * column or group headers, then please see the @see:WjFlexGridCellTemplate directive.
        *
        * The following example creates two columns with a template and a conditional style:
        *
        * @fiddle:5L423
        *
        * The <b>wj-flex-grid-column</b> directive may contain @see:WjFlexGridCellTemplate child directives.
        */
        var WjFlexGridColumn = (function (_super) {
            __extends(WjFlexGridColumn, _super);
            // Initializes a new instance of a WjGridColumn
            function WjFlexGridColumn($compile) {
                _super.call(this);

                this._$compile = $compile;

                // The 'data-map' HTML attribute is converted to 'map' by Angular, so we give it the 'map' alias.
                this.scope["dataMap"] += "map";
                this.scope["dataType"] += "type";

                this.require = '^wjFlexGrid';

                this['terminal'] = true;

                // If Angular supports template definition via a function (available starting with ver. 1.1.4) then we utilize this
                // possibility, because this is the only entry point where we have an access to an unprocessed version of a column
                // cell template with element level directives definitions in their original state.
                if (angular.WjDirective._dynaTemplates) {
                    // must be false, otherwise directive's subtree will no be available in the template function
                    this.transclude = false;

                    // should be less then at ng-repeat/ng-if etc (to let them take a control over a column directive creation),
                    // but bigger than at ordinal directives (like ng-style, to not allow them to evaluate during the column directive
                    // linking).
                    this['priority'] = 100;
                    this.template = function (tElement, tAttrs) {
                        // stores cell template definition, tAttrs is the only object that allows us to share a data
                        // with the link function.
                        tAttrs[WjFlexGridColumn._colTemplateProp] = tElement[0].innerHTML;
                        return '<div class="wjGridColumn"/>';
                    };
                    // under old Angular work in the degraded mode without element level directives support,
                    // retrieve cell template in the link function where element level directives are already compiled.
                } else {
                    this.transclude = true;
                    this.template = '<div class="wjGridColumn" ng-transclude/>';
                }
            }
            Object.defineProperty(WjFlexGridColumn.prototype, "_controlConstructor", {
                get: function () {
                    return wijmo.grid.Column;
                },
                enumerable: true,
                configurable: true
            });

            WjFlexGridColumn.prototype._initControl = function (element) {
                return new wijmo.grid.Column();
            };

            WjFlexGridColumn.prototype._createLink = function () {
                return new WjFlexGridColumnLink();
            };
            WjFlexGridColumn._colTemplateProp = '$__wjColTemplate';
            WjFlexGridColumn._colWjLinkProp = '$__wjLink';
            WjFlexGridColumn._cellCtxProp = '$_cellCtxProp';
            return WjFlexGridColumn;
        })(angular.WjDirective);

        var WjFlexGridColumnLink = (function (_super) {
            __extends(WjFlexGridColumnLink, _super);
            function WjFlexGridColumnLink() {
                _super.apply(this, arguments);
            }
            WjFlexGridColumnLink.prototype._initParent = function () {
                var grid = this.parent.control;
                if (grid.autoGenerateColumns) {
                    grid.autoGenerateColumns = false;
                    this._safeApply(this.scope, 'autoGenerateColumns', false);
                    grid.columns.clear();
                }

                _super.prototype._initParent.call(this);

                // Assign cell template defined without WjFlexGridCellTemplate tag if the latter was not specified.
                var cellCtxProp = WjFlexGridCellTemplate._getTemplContextProp(CellTemplateType.Cell), cellCtxByTag = this.control[cellCtxProp], cellCtxWoTag = this[WjFlexGridColumn._cellCtxProp];
                if (!cellCtxByTag && cellCtxWoTag) {
                    this.control[cellCtxProp] = cellCtxWoTag;
                }

                this.control[WjFlexGridColumn._colWjLinkProp] = this;
            };

            WjFlexGridColumnLink.prototype._link = function () {
                // get column template (HTML content)
                var rootEl = this.tElement[0], dynaTempl = this.tAttrs[WjFlexGridColumn._colTemplateProp], template = dynaTempl != null ? dynaTempl : WjFlexGridCellTemplateLink._removeTransclude(rootEl.innerHTML), cellTemplContext = {};
                if (!wijmo.isNullOrWhiteSpace(template)) {
                    //this.control['cellTemplate'] = template;
                    var templRoot = document.createElement('div');
                    templRoot.innerHTML = template;
                    var childElements = [];
                    [].forEach.call(templRoot.children, function (value) {
                        childElements.push(value);
                    });
                    var linkScope;
                    for (var i = 0; i < childElements.length; i++) {
                        var curTempl = childElements[i];
                        if (curTempl.tagName.toLocaleLowerCase() === WjFlexGridCellTemplate._tagName) {
                            if (!linkScope) {
                                //linkScope = this.scope.$parent;
                                linkScope = this.scope.$parent.$new();
                            }

                            // remove cell template directive from cell's template
                            templRoot.removeChild(curTempl);

                            // compile and link cell template directive
                            rootEl.appendChild(curTempl);
                            this.directive._$compile(curTempl)(linkScope);
                        }
                    }

                    var cleanCellTempl = templRoot.innerHTML;
                    if (!wijmo.isNullOrWhiteSpace(cleanCellTempl)) {
                        cellTemplContext.cellTemplate = cleanCellTempl;
                    }
                }

                // get column style
                var style = this.tAttrs['ngStyle'];
                if (style) {
                    cellTemplContext.cellStyle = style;
                }

                if (cellTemplContext.cellTemplate || cellTemplContext.cellStyle) {
                    cellTemplContext.templLink = this;
                    this[WjFlexGridColumn._cellCtxProp] = cellTemplContext;
                }

                _super.prototype._link.call(this);
            };
            return WjFlexGridColumnLink;
        })(angular.WjLink);

        /**
        * Defines the type of cell to which to apply the template. This value is specified in the <b>cell-type</b> attribute
        * of the @see:WjFlexGridCellTemplate directive.
        */
        (function (CellTemplateType) {
            /** Defines a regular (data) cell. */
            CellTemplateType[CellTemplateType["Cell"] = 0] = "Cell";

            /** Defines a cell in edit mode. */
            CellTemplateType[CellTemplateType["CellEdit"] = 1] = "CellEdit";

            /** Defines a column header cell. */
            CellTemplateType[CellTemplateType["ColumnHeader"] = 2] = "ColumnHeader";

            /** Defines a row header cell. */
            CellTemplateType[CellTemplateType["RowHeader"] = 3] = "RowHeader";

            /** Defines a row header cell in edit mode. */
            CellTemplateType[CellTemplateType["RowHeaderEdit"] = 4] = "RowHeaderEdit";

            /** Defines a top left cell. */
            CellTemplateType[CellTemplateType["TopLeft"] = 5] = "TopLeft";

            /** Defines a group header cell in a group row. */
            CellTemplateType[CellTemplateType["GroupHeader"] = 6] = "GroupHeader";

            /** Defines a regular cell in a group row. */
            CellTemplateType[CellTemplateType["Group"] = 7] = "Group";
        })(angular.CellTemplateType || (angular.CellTemplateType = {}));
        var CellTemplateType = angular.CellTemplateType;

        /**
        * AngularJS directive for the @see:FlexGrid cell templates.
        *
        * The <b>wj-flex-grid-cell-template</b> directive defines a template for a certain
        * cell type in @see:FlexGrid, and must contain a <b>cell-type</b> attribute that
        * specifies the @see:CellTemplateType. Depending on the template's cell type,
        * the <b>wj-flex-grid-cell-template</b> directive must be a child of either @see:WjFlexGrid
        * or @see:WjFlexGridColumn directives.
        *
        * Column-specific cell templates must be contained in <b>wj-flex-grid-column</b>
        * directives, and cells that are not column-specific (like row header or top left cells)
        * must be contained in the <b>wj-flex-grid directive</b>.
        *
        * In addition to an HTML fragment, <b>wj-flex-grid-cell-template</b> directives may
        * contain an <b>ng-style</b> attribute that provides conditional formatting for cells.
        *
        * Both the <b>ng-style</b> attribute and the HTML fragment can use the <b>$col</b>,
        * <b>$row</b> and <b>$item</b> template variables that refer to the @see:Column,
        * @see:Row and <b>Row.dataItem</b> objects pertaining to the cell.
        *
        * For cell types like <b>Group</b> and <b>CellEdit</b>, an additional <b>$value</b>
        * variable containing an unformatted cell value is provided. For example, here is a
        * FlexGrid control with templates for row headers and for the Country column's regular
        * and column header cells:
        *
        * <pre>&lt;wj-flex-grid items-source="data"&gt;
        *   &lt;wj-flex-grid-cell-template cell-type="RowHeader"&gt;
        *     {&#8203;{$row.index}}
        *   &lt;/wj-flex-grid-cell-template&gt;
        *   &lt;wj-flex-grid-cell-template cell-type="RowHeaderEdit"&gt;
        *     ...
        *   &lt;/wj-flex-grid-cell-template&gt;
        * &nbsp;
        *   &lt;wj-flex-grid-column header="Country" binding="country"&gt;
        *     &lt;wj-flex-grid-cell-template cell-type="ColumnHeader"&gt;
        *       &lt;img ng-src="resources/globe.png" /&gt;
        *         {&#8203;{$col.header}}
        *       &lt;/wj-flex-grid-cell-template&gt;
        *       &lt;wj-flex-grid-cell-template cell-type="Cell"&gt;
        *         &lt;img ng-src="resources/{&#8203;{$item.country}}.png" /&gt;
        *         {&#8203;{$item.country}}
        *       &lt;/wj-flex-grid-cell-template&gt;
        *     &lt;/wj-flex-grid-column&gt;
        *   &lt;wj-flex-grid-column header="Sales" binding="sales"&gt;&lt;/wj-flex-grid-column&gt;
        * &lt;/wj-flex-grid&gt;</pre>
        *
        * For more detailed information on specific cell type templates refer to the
        * documentation for the @see:CellTemplateType enumeration.
        *
        * Note that the <b>wj-flex-grid-column</b> directive may also contain arbitrary content
        * that is treated as a template for a regular data cell (<i>cell-type="Cell"</i>). But if
        * a <b>wj-flex-grid-cell-template</b> directive exists and is set to <i>cell-type="Cell"</i>
        * under the <b>wj-flex-grid-column</b> directive, it takes priority and overrides the
        * arbitrary content.
        *
        * The <b>wj-flex-grid-cell-template</b> directive supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>cell-type</dt>
        *   <dd><code>@</code>
        *     The @see:CellTemplateType value defining the type of cell the template applies to.
        *   </dd>
        *   <dt>cell-overflow</dt>
        *   <dd><code>@</code>
        *     Defines the <b>style.overflow</b> property value for cells.
        *   </dd>
        * </dl>
        *
        * The <b>cell-type</b> attribute takes any of the following enumerated values:
        *
        * <p><b>Cell</b><p>
        * Defines a regular (data) cell template. Must be a child of the @see:WjFlexGridColumn directive.
        * For example, this cell template shows flags in the Country column's cells:
        *
        * <pre>&lt;wj-flex-grid-column header="Country" binding="country"&gt;
        *   &lt;wj-flex-grid-cell-template cell-type="Cell"&gt;
        *     &lt;img ng-src="resources/{&#8203;{$item.country}}.png" /&gt;
        *     {&#8203;{$item.country}}
        *   &lt;/wj-flex-grid-cell-template&gt;
        * &lt;/wj-flex-grid-column&gt;</pre>
        *
        * For a hierarchical @see:FlexGrid (that is, one with the <b>childItemsPath</b> property
        * specified), if no <b>Group</b> template is provided, non-header cells in group rows in
        * this @see:Column also use this template.
        *
        * <p><b>CellEdit</b></p>
        *
        * Defines a template for a cell in edit mode. Must be a child of the @see:WjFlexGridColumn directive.
        * This cell type has an additional <b>$value</b> property available for binding. It contains the
        * original cell value before editing, and the updated value after editing.
        
        * For example, here is a template that uses the Wijmo @see:InputNumber control as an editor
        * for the "Sales" column:
        *
        * <pre>&lt;wj-flex-grid-column header="Sales" binding="sales"&gt;
        *   &lt;wj-flex-grid-cell-template cell-type="CellEdit"&gt;
        *     &lt;wj-input-number value="$value" step="1"&gt;&lt;/wj-input-number&gt;
        *   &lt;/wj-flex-grid-cell-template&gt;
        * &lt;/wj-flex-grid-column&gt;</pre>
        *
        * <p><b>ColumnHeader</b></p>
        *
        * Defines a template for a column header cell. Must be a child of the @see:WjFlexGridColumn directive.
        * For example, this template adds an image to the header of the "Country" column:
        *
        * <pre>&lt;wj-flex-grid-column header="Country" binding="country"&gt;
        *   &lt;wj-flex-grid-cell-template cell-type="ColumnHeader"&gt;
        *     &lt;img ng-src="resources/globe.png" /&gt;
        *     {&#8203;{$col.header}}
        *   &lt;/wj-flex-grid-cell-template&gt;
        * &lt;/wj-flex-grid-column&gt;</pre>
        *
        * <p><b>RowHeader</b></p>
        *
        * Defines a template for a row header cell. Must be a child of the @see:WjFlexGrid directive.
        * For example, this template shows row indices in the row headers:
        *
        * <pre>&lt;wj-flex-grid items-source="data"&gt;
        *   &lt;wj-flex-grid-cell-template cell-type="RowHeader"&gt;
        *     {&#8203;{$row.index}}
        *   &lt;/wj-flex-grid-cell-template&gt;
        * &lt;/wj-flex-grid&gt;</pre>
        *
        * Note that this template is applied to a row header cell, even if it is in a row that is
        * in edit mode. In order to provide an edit-mode version of a row header cell with alternate
        * content, define the <b>RowHeaderEdit</b> template.
        *
        * <p><b>RowHeaderEdit</b></p>
        *
        * Defines a template for a row header cell in edit mode. Must be a child of the
        * @see:WjFlexGrid directive. For example, this template shows dots in the header
        * of rows being edited:
        *
        * <pre>&lt;wj-flex-grid items-source="data"&gt;
        *   &lt;wj-flex-grid-cell-template cell-type="RowHeaderEdit"&gt;
        *       ...
        *   &lt;/wj-flex-grid-cell-template&gt;
        * &lt;/wj-flex-grid&gt;</pre>
        *
        * To add the standard edit-mode indicator to cells where the <b>RowHeader</b> template
        * applies, use the following <b>RowHeaderEdit</b> template:
        *
        * <pre>&lt;wj-flex-grid items-source="data"&gt;
        *   &lt;wj-flex-grid-cell-template cell-type="RowHeaderEdit"&gt;
        *     {&#8203;{&amp;#x270e;}}
        *   &lt;/wj-flex-grid-cell-template&gt;
        * &lt;/wj-flex-grid&gt;</pre>
        *
        * <p><b>TopLeft</b></p>
        *
        * Defines a template for the top left cell. Must be a child of the @see:WjFlexGrid directive.
        * For example, this template shows a down/right glyph in the top-left cell of the grid:
        *
        * <pre>&lt;wj-flex-grid items-source="data"&gt;
        *   &lt;wj-flex-grid-cell-template cell-type="TopLeft"&gt;
        *     &lt;span class="wj-glyph-down-right"&gt;&lt;/span&gt;
        *   &lt;/wj-flex-grid-cell-template&gt;
        * &lt;/wj-flex-grid&gt;</pre>
        *
        * <p><b>GroupHeader</b></p>
        *
        * Defines a template for a group header cell in a @see:GroupRow, Must be a child of the @see:WjFlexGridColumn directive.
        *
        * The <b>$row</b> variable contains an instance of the <b>GroupRow</b> class. If the grouping comes
        * from the a @see:CollectionView, the <b>$item</b> variable references the @see:CollectionViewGroup object.
        *
        * For example, this template uses a checkbox element as an expand/collapse toggle:
        *
        * <pre>&lt;wj-flex-grid-column header="Country" binding="country"&gt;
        *   &lt;wj-flex-grid-cell-template cell-type="GroupHeader"&gt;
        *     &lt;input type="checkbox" ng-model="$row.isCollapsed"/&gt;
        *     {&#8203;{$item.name}} ({&#8203;{$item.items.length}} items)
        *   &lt;/wj-flex-grid-cell-template&gt;
        * &lt;/wj-flex-grid-column&gt;</pre>
        *
        * <p><b>Group</b></p>
        *
        * Defines a template for a regular cell (not a group header) in a @see:GroupRow. Must be a child of the
        * @see:WjFlexGridColumn directive. This cell type has an additional <b>$value</b> varible available for
        * binding. In cases where columns have the <b>aggregate</b> property specified, it contains the unformatted
        * aggregate value.
        *
        * For example, this template shows an aggregate's value and kind for group row cells in the "Sales"
        * column:
        *
        * <pre>&lt;wj-flex-grid-column header="Sales" binding="sales" aggregate="Avg"&gt;
        *   &lt;wj-flex-grid-cell-template cell-type="Group"&gt;
        *     Average: {&#8203;{$value | number:2}}
        *   &lt;/wj-flex-grid-cell-template&gt;
        * &lt;/wj-flex-grid-column&gt;</pre>
        */
        var WjFlexGridCellTemplate = (function (_super) {
            __extends(WjFlexGridCellTemplate, _super);
            function WjFlexGridCellTemplate() {
                _super.call(this);

                this.require = ['?^wjFlexGridColumn', '?^wjFlexGrid'];

                // The same approach like in WjFlexGridColumn
                this['terminal'] = true;
                if (angular.WjDirective._dynaTemplates) {
                    this.transclude = false;
                    this['priority'] = 100;
                    this.template = function (tElement, tAttrs) {
                        tAttrs[WjFlexGridColumn._colTemplateProp] = tElement[0].innerHTML;
                        return '<div />';
                    };
                } else {
                    this.transclude = true;
                    this.template = '<div ng-transclude/>';
                }
            }
            // returns the name of the property on control instance that stores info for the specified cell template type.
            WjFlexGridCellTemplate._getTemplContextProp = function (templateType) {
                return '$__cellTempl' + CellTemplateType[templateType];
            };

            WjFlexGridCellTemplate.prototype._initControl = function (element) {
                return {};
            };

            WjFlexGridCellTemplate.prototype._createLink = function () {
                return new WjFlexGridCellTemplateLink();
            };

            WjFlexGridCellTemplate.prototype._getMetaDataId = function () {
                return 'FlexGridCellTemplate';
            };
            WjFlexGridCellTemplate._tagName = 'wj-flex-grid-cell-template';
            return WjFlexGridCellTemplate;
        })(angular.WjDirective);

        var WjFlexGridCellTemplateLink = (function (_super) {
            __extends(WjFlexGridCellTemplateLink, _super);
            function WjFlexGridCellTemplateLink() {
                _super.apply(this, arguments);
            }
            WjFlexGridCellTemplateLink.prototype._initParent = function () {
                _super.prototype._initParent.call(this);

                var cts = this.scope['cellType'], cellType;
                if (cts) {
                    cellType = CellTemplateType[cts];
                } else {
                    return;
                }

                // get column template (HTML content)
                var dynaTempl = this.tAttrs[WjFlexGridColumn._colTemplateProp], template = dynaTempl != null ? dynaTempl : WjFlexGridCellTemplateLink._removeTransclude(this.tElement[0].innerHTML), control = this.control;
                if (!wijmo.isNullOrWhiteSpace(template)) {
                    control.cellTemplate = template;
                }

                // get column style
                var style = this.tAttrs['ngStyle'];
                if (style) {
                    control.cellStyle = style;
                }

                if (control.cellTemplate || control.cellStyle) {
                    control.templLink = this;
                    this.parent.control[WjFlexGridCellTemplate._getTemplContextProp(cellType)] = control;
                }

                WjFlexGridCellTemplateLink._invalidateGrid(this.parent.control);
            };

            WjFlexGridCellTemplateLink.prototype._destroy = function () {
                var parentControl = this.parent && this.parent.control, cts = this.scope['cellType'];
                _super.prototype._destroy.call(this);
                if (cts) {
                    parentControl[WjFlexGridCellTemplate._getTemplContextProp(CellTemplateType[cts])] = undefined;
                    WjFlexGridCellTemplateLink._invalidateGrid(parentControl);
                }
            };

            WjFlexGridCellTemplateLink._invalidateGrid = function (parentControl) {
                var grid = parentControl;
                if (grid) {
                    if (grid instanceof wijmo.grid.Column) {
                        grid = grid.grid;
                    }
                    if (grid) {
                        grid.invalidate();
                    }
                }
            };

            WjFlexGridCellTemplateLink._removeTransclude = function (html) {
                if (!html) {
                    return html;
                }
                var root = document.createElement('div');
                root.innerHTML = html;
                var transNodes = root.querySelectorAll('[ng-transclude]');
                [].forEach.call(transNodes, function (elem, idx) {
                    elem.removeAttribute('ng-transclude');
                });

                return root.innerHTML;
            };
            return WjFlexGridCellTemplateLink;
        })(angular.WjLink);

        /**
        * AngularJS directive for the @see:FlexGrid @see:FlexGridFilter object.
        *
        * The <b>wj-flex-grid-filter</b> directive must be contained in a @see:WjFlexGrid directive. For example:
        *
        * <pre>&lt;p&gt;Here is a FlexGrid control with column filters:&lt;/p&gt;
        * &lt;wj-flex-grid items-source="data"&gt;
        *   &lt;wj-flex-grid-filter filter-columns="['country', 'expenses']"&gt;&lt;/wj-flex-grid-filter&gt;
        * &nbsp;
        *   &lt;wj-flex-grid-column
        *     header="Country"
        *     binding="country"&gt;
        *   &lt;/wj-flex-grid-column&gt;
        *   &lt;wj-flex-grid-column
        *     header="Sales"
        *     binding="sales"&gt;
        *   &lt;/wj-flex-grid-column&gt;
        *   &lt;wj-flex-grid-column
        *     header="Expenses"
        *     binding="expenses"&gt;
        *   &lt;/wj-flex-grid-column&gt;
        *   &lt;wj-flex-grid-column
        *     header="Downloads"
        *     binding="downloads"&gt;
        *   &lt;/wj-flex-grid-column&gt;
        * &lt;/wj-flex-grid&gt;</pre>
        *
        * The <b>wj-flex-grid-filter</b> directive supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>filter-columns</dt>    <dd><code>=</code> An array containing the names or bindings of the columns
        *                              to filter.</dd>
        *   <dt>show-filter-icons</dt> <dd><code>@</code>  The value indicating whether filter editing buttons
        *                              appear in the grid's column headers.</dd>
        *   <dt>filter-applied</dt>    <dd><code>&</code> Handler for the @see:filterApplied event.</dd>
        * </dl>
        */
        var WjFlexGridFilter = (function (_super) {
            __extends(WjFlexGridFilter, _super);
            // Initializes a new instance of a WjGridColumn
            function WjFlexGridFilter() {
                _super.call(this);

                this.require = '^wjFlexGrid';

                //this.transclude = true;
                this.template = '<div />';
            }
            Object.defineProperty(WjFlexGridFilter.prototype, "_controlConstructor", {
                get: function () {
                    return wijmo.grid.filter.FlexGridFilter;
                },
                enumerable: true,
                configurable: true
            });
            return WjFlexGridFilter;
        })(angular.WjDirective);

        /**
        * AngularJS directive for the @see:FlexGrid @see:GroupPanel control.
        *
        * The <b>wj-group-panel</b> directive connects to the <b>FlexGrid</b> control via the <b>grid</b> property.
        * For example:
        *
        * <pre>&lt;p&gt;Here is a FlexGrid control with GroupPanel:&lt;/p&gt;
        * &nbsp;
        * &lt;wj-group-panel grid="flex" placeholder="Drag columns here to create groups."&gt;&lt;/wj-group-panel&gt;
        * &nbsp;
        * &lt;wj-flex-grid control="flex" items-source="data"&gt;
        *   &lt;wj-flex-grid-column
        *     header="Country"
        *     binding="country"&gt;
        *   &lt;/wj-flex-grid-column&gt;
        *   &lt;wj-flex-grid-column
        *     header="Sales"
        *     binding="sales"&gt;
        *   &lt;/wj-flex-grid-column&gt;
        *   &lt;wj-flex-grid-column
        *     header="Expenses"
        *     binding="expenses"&gt;
        *   &lt;/wj-flex-grid-column&gt;
        *   &lt;wj-flex-grid-column
        *     header="Downloads"
        *     binding="downloads"&gt;
        *   &lt;/wj-flex-grid-column&gt;
        * &lt;/wj-flex-grid&gt;</pre>
        *
        * The <b>wj-group-panel</b> directive supports the following attributes:
        *
        * <dl class="dl-horizontal">
        *   <dt>grid</dt>                      <dd><code>@</code>The <b>FlexGrid</b> that is connected to this <b>GroupPanel</b>.</dd>
        *   <dt>hide-grouped-columns</dt>      <dd><code>@</code>A value indicating whether the panel hides grouped columns
        *                                      in the owner grid.</dd>
        *   <dt>max-groups</dt>                <dd><code>@</code>The maximum number of groups allowed.</dd>
        *   <dt>placeholder</dt>               <dd><code>@</code>A string to display in the control when it
        *                                      contains no groups.</dd>
        * </dl>
        *
        */
        var WjGroupPanel = (function (_super) {
            __extends(WjGroupPanel, _super);
            function WjGroupPanel() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(WjGroupPanel.prototype, "_controlConstructor", {
                get: function () {
                    return wijmo.grid.grouppanel.GroupPanel;
                },
                enumerable: true,
                configurable: true
            });
            return WjGroupPanel;
        })(angular.WjDirective);
    })(wijmo.angular || (wijmo.angular = {}));
    var angular = wijmo.angular;
})(wijmo || (wijmo = {}));

var wijmo;
(function (wijmo) {
    /**
    * Contains AngularJS directives for the Wijmo controls.
    *
    * The directives allow you to add Wijmo controls to
    * <a href="https://angularjs.org/" target="_blank">AngularJS</a>
    * applications using simple markup in HTML pages.
    *
    * You can use directives as regular HTML tags in the page markup. The
    * tag name corresponds to the control name, prefixed with "wj-," and the
    * attributes correspond to the names of control properties and events.
    *
    * All control, property, and event names within directives follow
    * the usual AngularJS convention of replacing camel-casing with hyphenated
    * lower-case names.
    *
    * AngularJS directive parameters come in three flavors, depending on the
    * type of binding they use. The table below describes each one:
    *
    * <dl class="dl-horizontal">
    *   <dt><code>@</code></dt>   <dd>By value, or one-way binding. The attribute
    *                             value is interpreted as a literal.</dd>
    *   <dt><code>=</code></dt>   <dd>By reference, or two-way binding. The
    *                             attribute value is interpreted as an expression.</dd>
    *   <dt><code>&</code></dt>   <dd>Function binding. The attribute value
    *                             is interpreted as a function call, including the parameters.</dd>
    * </dl>
    *
    * For more details on the different binding types, please see <a href=
    * "http://weblogs.asp.net/dwahlin/creating-custom-angularjs-directives-part-2-isolate-scope"
    * target="_blank"> Dan Wahlin's blog on directives</a>.
    *
    * The documentation does not describe directive events because they are identical to
    * the control events, and the binding mode is always the same (function binding).
    *
    * To illustrate, here is the markup used to create a @see:ComboBox control:
    *
    * <pre>&lt;wj-combo-box
    *   text="ctx.theCountry"
    *   items-source="ctx.countries"
    *   is-editable="true"
    *   selected-index-changed="ctx.selChanged(s, e)"&gt;
    * &lt;/wj-combo-box&gt;</pre>
    *
    * Notice that the <b>text</b> property of the @see:ComboBox is bound to a controller
    * variable called "ctx.theCountry." The binding goes two ways; changes in the control
    * update the scope, and changes in the scope update the control. To
    * initialize the <b>text</b> property with a string constant, enclose
    * the attribute value in single quotes (for example, <code>text="'constant'"</code>).
    *
    * Notice also that the <b>selected-index-changed</b> event is bound to a controller
    * method called "selChanged," and that the binding includes the two event parameters
    * (without the parameters, the method is not called).
    * Whenever the control raises the event, the directive invokes the controller method.
    */
    (function (angular) {
        // define the wijmo module with all dependencies
        var wijModule = window['angular'].module('wj', ['wj.input', 'wj.grid', 'wj.chart', 'wj.container', 'wj.gauge']);
    })(wijmo.angular || (wijmo.angular = {}));
    var angular = wijmo.angular;
})(wijmo || (wijmo = {}));
//# sourceMappingURL=wijmo.angular.js.map
