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
module wijmo {
    // prevent double loading
    if (wijmo && wijmo.interop) {
        return;
    }

    export module interop {
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
        export class ControlMetaFactory {
            // Creates a property descriptor object. A specific interop service should override this method in the derived 
            // metadata factory class to create platrorm specific descriptor object.
            public static CreateProp(propertyName: string, propertyType: PropertyType, bindingMode?: BindingMode, enumType?,
                isNativeControlProperty?: boolean, priority?: number): PropDescBase {

                return new PropDescBase(propertyName, propertyType, bindingMode, enumType, isNativeControlProperty, priority);
            }

            // Creates an event descriptor object. A specific interop service should override this method in the derived 
            // metadata factory class to create platrorm specific descriptor object.
            public static CreateEvent(eventName: string, isPropChanged?: boolean): EventDescBase {
                return new EventDescBase(eventName, isPropChanged);
            }

            // Creates a complex property descriptor object. A specific interop service should override this method in the derived 
            // metadata factory class to create platrorm specific descriptor object.
            public static CreateComplexProp(propertyName: string, isArray: boolean, ownsObject?: boolean): ComplexPropDescBase {
                return new ComplexPropDescBase(propertyName, isArray, ownsObject);
            }

            // Finds a property descriptor by the property name in the specified array.
            public static findProp(propName: string, props: PropDescBase[]): PropDescBase {
                return this.findInArr(props, 'propertyName', propName); 
            }

            // Finds an event descriptor by the event name in the specified array.
            public static findEvent(eventName: string, events: EventDescBase[]): EventDescBase {
                return this.findInArr(events, 'eventName', eventName);
            }

            // Finds a complex property descriptor by the property name in the specified array.
            public static findComplexProp(propName: string, props: ComplexPropDescBase[]): ComplexPropDescBase {
                return this.findInArr(props, 'propertyName', propName);
            }

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
            public static getMetaData(metaDataId: any): MetaDataBase {
                switch (metaDataId) {

                    // wijmo.input *************************************************************
                    case wijmo.input && wijmo.input.DropDown:
                        return new MetaDataBase(
                            [
                                this.CreateProp('isDroppedDown', PropertyType.Boolean, BindingMode.TwoWay),
                                this.CreateProp('showDropDownButton', PropertyType.Boolean),
                                this.CreateProp('placeholder', PropertyType.String),
                                this.CreateProp('text', PropertyType.String, BindingMode.TwoWay, null, true, 1000)
                            ],
                            [
                                this.CreateEvent('isDroppedDownChanged', true),
                                this.CreateEvent('textChanged', true)
                            ]);

                    case wijmo.input && wijmo.input.ComboBox:
                        return this.getMetaData(wijmo.input.DropDown).add(
                            [
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
                                this.CreateProp('selectedValue', PropertyType.Any, BindingMode.TwoWay, null, true, 1000),
                            ],
                            [
                                this.CreateEvent('selectedIndexChanged', true)
                            ])
                            .addOptions({ ngModelProperty: 'selectedValue' });

                    case wijmo.input && wijmo.input.AutoComplete:
                        return this.getMetaData(wijmo.input.ComboBox).add(
                            [
                                this.CreateProp('delay', PropertyType.Number),
                                this.CreateProp('maxItems', PropertyType.Number),
                                this.CreateProp('minLength', PropertyType.Number),
                                this.CreateProp('cssMatch', PropertyType.String),
                                this.CreateProp('itemsSourceFunction', PropertyType.Function)
                            ]);

                    case wijmo.input && wijmo.input.Calendar:
                        return new MetaDataBase(
                            [
                                this.CreateProp('itemFormatter', PropertyType.Function),
                                this.CreateProp('monthView', PropertyType.Boolean),
                                this.CreateProp('showHeader', PropertyType.Boolean),
                                this.CreateProp('max', PropertyType.Date),
                                this.CreateProp('min', PropertyType.Date),
                                this.CreateProp('value', PropertyType.Date, BindingMode.TwoWay),
                                this.CreateProp('displayMonth', PropertyType.Date, BindingMode.TwoWay),
                                this.CreateProp('firstDayOfWeek', PropertyType.Number),
                            ],
                            [
                                this.CreateEvent('valueChanged', true),
                                this.CreateEvent('displayMonthChanged', true),
                            ])
                            .addOptions({ ngModelProperty: 'value' });

                    case wijmo.input && wijmo.input.ColorPicker:
                        return new MetaDataBase(
                            [
                                this.CreateProp('showAlphaChannel', PropertyType.Boolean),
                                this.CreateProp('showColorString', PropertyType.Boolean),
                                this.CreateProp('palette', PropertyType.Any),
                                this.CreateProp('value', PropertyType.String, BindingMode.TwoWay)
                            ],
                            [
                                this.CreateEvent('valueChanged', true)
                            ])
                            .addOptions({ ngModelProperty: 'value' });

                    case wijmo.input && wijmo.input.ListBox:
                        return new MetaDataBase(
                            [
                                this.CreateProp('isContentHtml', PropertyType.Boolean),
                                this.CreateProp('maxHeight', PropertyType.Number),
                                this.CreateProp('selectedValuePath', PropertyType.String),
                                this.CreateProp('itemFormatter', PropertyType.Function),
                                this.CreateProp('displayMemberPath', PropertyType.String),
                                this.CreateProp('checkedMemberPath', PropertyType.String),
                                this.CreateProp('itemsSource', PropertyType.Any),
                                this.CreateProp('selectedIndex', PropertyType.Number, BindingMode.TwoWay),
                                this.CreateProp('selectedItem', PropertyType.Any, BindingMode.TwoWay),
                                this.CreateProp('selectedValue', PropertyType.Any, BindingMode.TwoWay),
                            ],
                            [
                                this.CreateEvent('itemsChanged', true),
                                this.CreateEvent('itemChecked', false),
                                this.CreateEvent('selectedIndexChanged', true)
                            ])
                            .addOptions({ ngModelProperty: 'selectedValue' });

                    case wijmo.input && wijmo.input.Menu:
                        return this.getMetaData(wijmo.input.ComboBox).add(
                            [
                                this.CreateProp('header', PropertyType.String),
                                this.CreateProp('commandParameterPath', PropertyType.String),
                                this.CreateProp('commandPath', PropertyType.String),
                                this.CreateProp('isButton', PropertyType.Boolean),
                                this.CreateProp('value', PropertyType.Any, BindingMode.TwoWay, null, false, 1000)
                            ],
                            [
                                this.CreateEvent('itemClicked')
                            ]);

                    case 'MenuItem':
                        return new MetaDataBase(
                            [
                                //TBD: check whether they should be two-way
                                //this.CreateProp('value', PropertyType.String, BindingMode.TwoWay),
                                //this.CreateProp('cmd', PropertyType.String, BindingMode.TwoWay),
                                //this.CreateProp('cmdParam', PropertyType.String, BindingMode.TwoWay)

                                this.CreateProp('value', PropertyType.Any, BindingMode.OneWay),
                                this.CreateProp('cmd', PropertyType.Any, BindingMode.OneWay),
                                this.CreateProp('cmdParam', PropertyType.Any, BindingMode.OneWay)
                            ]);

                    case wijmo.input && wijmo.input.InputDate:
                        return this.getMetaData(wijmo.input.DropDown).add(
                            [
                                this.CreateProp('required', PropertyType.Boolean),
                                this.CreateProp('format', PropertyType.String),
                                this.CreateProp('mask', PropertyType.String),
                                this.CreateProp('max', PropertyType.Date),
                                this.CreateProp('min', PropertyType.Date),
                                this.CreateProp('value', PropertyType.Date, BindingMode.TwoWay, null, true, 1000),
                            ],
                            [
                                this.CreateEvent('valueChanged', true)
                            ])
                            .addOptions({ ngModelProperty: 'value' });

                    case wijmo.input && wijmo.input.InputNumber:
                        return new MetaDataBase(
                            [
                                this.CreateProp('showSpinner', PropertyType.Boolean),
                                this.CreateProp('max', PropertyType.Number),
                                this.CreateProp('min', PropertyType.Number),
                                this.CreateProp('step', PropertyType.Number),
                                this.CreateProp('required', PropertyType.Boolean),
                                this.CreateProp('placeholder', PropertyType.String),
                                this.CreateProp('inputType', PropertyType.String),
                                this.CreateProp('value', PropertyType.Number, BindingMode.TwoWay),
                                this.CreateProp('text', PropertyType.String, BindingMode.TwoWay),
                                this.CreateProp('format', PropertyType.String),
                            ],
                            [
                                this.CreateEvent('valueChanged', true),
                                this.CreateEvent('textChanged', true)
                            ])
                            .addOptions({ ngModelProperty: 'value' });

                    case wijmo.input && wijmo.input.InputMask:
                        return new MetaDataBase(
                            [
                                this.CreateProp('mask', PropertyType.String),
                                this.CreateProp('promptChar', PropertyType.String),
                                this.CreateProp('placeholder', PropertyType.String),
                                this.CreateProp('value', PropertyType.String, BindingMode.TwoWay)
                            ],
                            [
                                this.CreateEvent('valueChanged', true),
                            ])
                            .addOptions({ ngModelProperty: 'value' });

                    case wijmo.input && wijmo.input.InputTime:
                        return this.getMetaData(wijmo.input.ComboBox).add(
                            [
                                this.CreateProp('max', PropertyType.Date),
                                this.CreateProp('min', PropertyType.Date),
                                this.CreateProp('step', PropertyType.Number),
                                this.CreateProp('format', PropertyType.String),
                                this.CreateProp('mask', PropertyType.String),
                                this.CreateProp('value', PropertyType.Date, BindingMode.TwoWay, null, true, 1000),
                            ],
                            [
                                this.CreateEvent('valueChanged', true)
                            ])
                            .addOptions({ ngModelProperty: 'value' });

                    case wijmo.input && wijmo.input.InputColor:
                        return this.getMetaData(wijmo.input.DropDown).add(
                            [
                                this.CreateProp('required', PropertyType.Boolean),
                                this.CreateProp('showAlphaChannel', PropertyType.Boolean),
                                this.CreateProp('value', PropertyType.String, BindingMode.TwoWay)
                            ],
                            [
                                this.CreateEvent('valueChanged', true)
                            ])
                            .addOptions({ ngModelProperty: 'value' });

                    case 'CollectionViewNavigator':
                        return new MetaDataBase(
                            [
                                this.CreateProp('cv', PropertyType.Any)
                            ]);

                    case 'CollectionViewPager':
                        return new MetaDataBase(
                            [
                                this.CreateProp('cv', PropertyType.Any)
                            ]);



                    // wijmo.grid *************************************************************
                    case wijmo.grid && wijmo.grid.FlexGrid:
                        return new MetaDataBase(
                            [
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
                                // REVIEW: This breaks the grid too, see TFS 82636
                                //this.CreateProp('scrollPosition', PropertyType.Any, '='),
                                // REVIEW: this screws up the grid when selectionMode == ListBox.
                                // When the directive applies a selection to the grid and selectionMode == ListBox,
                                // the grid clears the row[x].isSelected properties of rows that are not in the selection.
                                // I think a possible fix would be for the directive to not set the grid's selection if it 
                                // is the same range as the current selection property. I cannot do that in the grid because
                                // when the user does it, this side-effect is expected.
                                //this.CreateProp('selection', PropertyType.Any, '='),
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
                            ],
                            [
                                // Cell events
                                this.CreateEvent('beginningEdit'),
                                this.CreateEvent('cellEditEnded'),
                                this.CreateEvent('cellEditEnding'),
                                this.CreateEvent('prepareCellForEdit'),
                                this.CreateEvent('formatItem'),

                                // Column events
                                this.CreateEvent('resizingColumn'),
                                this.CreateEvent('resizedColumn'),
                                this.CreateEvent('autoSizingColumn'),
                                this.CreateEvent('autoSizedColumn'),
                                this.CreateEvent('draggingColumn'),
                                this.CreateEvent('draggedColumn'),
                                this.CreateEvent('sortingColumn'),
                                this.CreateEvent('sortedColumn'),

                                // Row Events
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
                                this.CreateEvent('scrollPositionChanged', false), // AlexI: TBD: true freezes scrolling with mouse wheel

                                // Clipboard events
                                this.CreateEvent('pasting'),
                                this.CreateEvent('pasted'),
                                this.CreateEvent('copying'),
                                this.CreateEvent('copied')
                            ]);

                    case wijmo.grid && wijmo.grid.Column:
                        return new MetaDataBase(
                            [
                                this.CreateProp('name', PropertyType.String),
                                this.CreateProp('dataMap', PropertyType.Any), //Angular converts this to 'map'
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
                            ],
                            [], [], 'columns', true); 

                    case 'FlexGridCellTemplate':
                        return new MetaDataBase(
                            [
                                this.CreateProp('cellType', PropertyType.String, BindingMode.OneWay, null, false),
                                this.CreateProp('cellOverflow', PropertyType.String, BindingMode.OneWay),
                            ],
                            [], [], undefined, undefined, undefined, 'owner'); 

                    case wijmo.grid && wijmo.grid.filter && wijmo.grid.filter.FlexGridFilter:
                        return new MetaDataBase(
                            [
                                this.CreateProp('showFilterIcons', PropertyType.Boolean),
                                this.CreateProp('filterColumns', PropertyType.Any),
                            ],
                            [
                                this.CreateEvent('filterApplied')
                            ],
                            [], undefined, undefined, undefined, ''); 

                    case wijmo.grid && wijmo.grid.grouppanel && wijmo.grid.grouppanel.GroupPanel:
                        return new MetaDataBase(
                            [
                                this.CreateProp('hideGroupedColumns', PropertyType.Boolean),
                                this.CreateProp('maxGroups', PropertyType.Number),
                                this.CreateProp('placeholder', PropertyType.String),
                                this.CreateProp('grid', PropertyType.Any),
                            ]); 


                    // Chart *************************************************************
                    case wijmo.chart && wijmo.chart.FlexChartBase:
                        return new MetaDataBase(
                            [
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
                            ],
                            [
                                this.CreateEvent('rendering'),
                                this.CreateEvent('rendered'),
                            ]);

                    case wijmo.chart && wijmo.chart.FlexChart:
                        return this.getMetaData(wijmo.chart.FlexChartBase).add(
                            [
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
                                this.CreateProp('labelContent', PropertyType.String, BindingMode.OneWay, null, false),
                            ],
                            [
                                this.CreateEvent('seriesVisibilityChanged'),
                                this.CreateEvent('selectionChanged', true),
                            ],
                            [
                                this.CreateComplexProp('axisX', false, false),
                                this.CreateComplexProp('axisY', false, false),
                                this.CreateComplexProp('axes', true)
                            ]);

                    case wijmo.chart && wijmo.chart.FlexPie:
                        return this.getMetaData(wijmo.chart.FlexChartBase).add(
                            [
                                this.CreateProp('bindingName', PropertyType.String),
                                this.CreateProp('innerRadius', PropertyType.Number),
                                this.CreateProp('isAnimated', PropertyType.Boolean),
                                this.CreateProp('offset', PropertyType.Number),
                                this.CreateProp('reversed', PropertyType.Boolean),
                                this.CreateProp('startAngle', PropertyType.Number),
                                this.CreateProp('selectedItemPosition', PropertyType.Enum, BindingMode.OneWay, wijmo.chart.Position),
                                this.CreateProp('selectedItemOffset', PropertyType.Number),
                                this.CreateProp('itemFormatter', PropertyType.Function),
                                this.CreateProp('labelContent', PropertyType.String, BindingMode.OneWay, null, false),
                            ]);

                    case wijmo.chart && wijmo.chart.Axis:
                        return new MetaDataBase(
                            [
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
                                this.CreateProp('logBase', PropertyType.Number),
                            ],
                            [], [], 'axes', true); //use wj-property attribute on directive to define axisX or axisY

                    case wijmo.chart && wijmo.chart.Legend:
                        return new MetaDataBase(
                            [
                                this.CreateProp('position', PropertyType.Enum, BindingMode.OneWay, wijmo.chart.Position)
                            ],
                            [], [], 'legend', false, false);

                    case wijmo.chart && wijmo.chart.DataLabelBase:
                        return new MetaDataBase(
                            [
                                this.CreateProp('content', PropertyType.Any, BindingMode.OneWay),
                                this.CreateProp('border', PropertyType.Boolean),
                            ],
                            [], [], 'dataLabel', false, false);

                    case wijmo.chart && wijmo.chart.DataLabel:
                        return this.getMetaData(wijmo.chart.DataLabelBase).add(
                            [
                                this.CreateProp('position', PropertyType.Enum, BindingMode.OneWay, wijmo.chart.LabelPosition),
                            ]);

                    case wijmo.chart && wijmo.chart.PieDataLabel:
                        return this.getMetaData(wijmo.chart.DataLabelBase).add(
                            [
                                this.CreateProp('position', PropertyType.Enum, BindingMode.OneWay, wijmo.chart.PieLabelPosition),
                            ]);

                    case wijmo.chart && wijmo.chart.Series:
                        return new MetaDataBase(
                            [
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
                                this.CreateProp('itemsSource', PropertyType.Any),
                            ],
                            [],
                            [
                                this.CreateComplexProp('axisX', false, true),
                                this.CreateComplexProp('axisY', false, true),
                            ],
                            'series', true);

                    case wijmo.chart && wijmo.chart.LineMarker:
                        return new MetaDataBase(
                            [
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
                                this.CreateProp('dragContent', PropertyType.Boolean),
                            ],
                            [
                                this.CreateEvent('positionChanged'),
                            ],
                            [],
                            undefined, undefined, undefined, '');

                    case wijmo.chart && wijmo.chart.interaction && wijmo.chart.interaction.RangeSelector:
                        return new MetaDataBase(
                            [
                                this.CreateProp('isVisible', PropertyType.Boolean),
                                this.CreateProp('min', PropertyType.Number),
                                this.CreateProp('max', PropertyType.Number),
                                this.CreateProp('orientation', PropertyType.Enum, BindingMode.OneWay, wijmo.chart.interaction.Orientation),
                            ],
                            [
                                this.CreateEvent('rangeChanged'),
                            ],
                            [],
                            undefined, undefined, undefined, '');


                    // *************************** Gauge *************************************************************
                    //case 'Gauge':
                    case wijmo.gauge && wijmo.gauge.Gauge:
                        return new MetaDataBase(
                            [
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
                            ],
                            [
                                this.CreateEvent('valueChanged', true)
                            ],
                            [
                                this.CreateComplexProp('ranges', true),
                                this.CreateComplexProp('pointer', false, false),
                                this.CreateComplexProp('face', false, false)
                            ])
                            .addOptions({ ngModelProperty: 'value' });

                    //case 'LinearGauge':
                    case wijmo.gauge && wijmo.gauge.LinearGauge:
                        return this.getMetaData(wijmo.gauge.Gauge).add(
                            [
                                this.CreateProp('direction', PropertyType.Enum, BindingMode.OneWay, wijmo.gauge.GaugeDirection)
                            ]);

                    case wijmo.gauge && wijmo.gauge.BulletGraph:
                        return this.getMetaData(wijmo.gauge.LinearGauge).add(
                            [
                                this.CreateProp('target', PropertyType.Number),
                                this.CreateProp('good', PropertyType.Number),
                                this.CreateProp('bad', PropertyType.Number)
                            ]);

                    case wijmo.gauge && wijmo.gauge.RadialGauge:
                        return this.getMetaData(wijmo.gauge.Gauge).add(
                            [
                                this.CreateProp('autoScale', PropertyType.Boolean),
                                this.CreateProp('startAngle', PropertyType.Number),
                                this.CreateProp('sweepAngle', PropertyType.Number)
                            ]);

                    case wijmo.gauge && wijmo.gauge.Range:
                        return new MetaDataBase(
                            [
                                this.CreateProp('color', PropertyType.String),
                                this.CreateProp('min', PropertyType.Number),
                                this.CreateProp('max', PropertyType.Number),
                                this.CreateProp('name', PropertyType.String),
                                this.CreateProp('thickness', PropertyType.Number)
                            ],
                            [], [], 'ranges', true);

                }

                return new MetaDataBase([]);
            }

            // For the specified class reference returns its name as a string, e.g.
            // getClassName(wijmo.input.ComboBox) returns 'ComboBox'.
            public static getClassName(classRef: any): string {
                return (classRef.toString().match(/function (.+?)\(/) || [, ''])[1];
            }

            // Returns a camel case representation of the dash delimited name.
            public static toCamelCase(s) {
                return s.toLowerCase().replace(/-(.)/g, function (match, group1) {
                    return group1.toUpperCase();
                });
            }


            private static findInArr(arr: any[], propName: string, value: any): any {
                for (var i in arr) {
                    if (arr[i][propName] === value) {
                        return arr[i];
                    }
                }
                return null;
            }

        }

        // Describes a scope property: name, type, binding mode. 
        // Also defines enum type and custom watcher function extender
        export class PropDescBase {
            private _propertyName: string;
            private _propertyType: PropertyType;
            private _enumType: any;
            private _bindingMode: BindingMode;
            private _isNativeControlProperty: boolean;
            private _priority: number = 0;

            // Initializes a new instance of a PropDesc
            constructor(propertyName: string, propertyType: PropertyType, bindingMode: BindingMode = BindingMode.OneWay,
                enumType?: any, isNativeControlProperty: boolean = true, priority: number = 0) {
                this._propertyName = propertyName;
                this._propertyType = propertyType;
                this._bindingMode = bindingMode;
                this._enumType = enumType;
                this._isNativeControlProperty = isNativeControlProperty;
                this._priority = priority;
            }

            // Gets the property name 
            get propertyName(): string {
                return this._propertyName;
            }

            // Gets the property type (number, string, boolean, enum, or any)
            get propertyType(): PropertyType {
                return this._propertyType;
            }

            // Gets the property enum type
            get enumType(): any { return this._enumType; }

            // Gets the property binding mode
            get bindingMode(): BindingMode {
                return this._bindingMode;
            }

            // Gets whether the property belongs to the control is just to the directive
            get isNativeControlProperty(): boolean {
                return this._isNativeControlProperty;
            }

            // Gets an initialization priority. Properties with higher priority are assigned to directive's underlying control
            // property later than properties with lower priority. Properties with the same priority are assigned in the order of
            // their index in the _props collection. 
            get priority(): number {
                return this._priority;
            }

            // Indicates whether a bound 'controller' property should be updated on this property change (i.e. two-way binding).
            get shouldUpdateSource(): boolean {
                return this.bindingMode === BindingMode.TwoWay && this.propertyType != PropertyType.EventHandler;
            }

            initialize(options: any) {
                wijmo.copy(this, options);
            }

        }

        // Property types as used in the PropDesc class.
        export enum PropertyType {
            Boolean,
            Number,
            Date,
            String,
            Enum, // IMPORTANT: All new simple types must be added before Enum, all complex types after Enum.
            Function,
            EventHandler,
            Any
        }

        // Gets a value indicating whether the specified type is simple (true) or complex (false).
        export function isSimpleType(type: PropertyType): boolean {
            return type <= PropertyType.Enum;
        }

        export enum BindingMode {
            OneWay,
            TwoWay
        }

        // Describes a scope event
        export class EventDescBase {
            private _eventName: string;
            private _isPropChanged: boolean;

            // Initializes a new instance of an EventDesc
            constructor(eventName: string, isPropChanged?: boolean) {
                this._eventName = eventName;
                this._isPropChanged = isPropChanged;
            }

            // Gets the event name
            get eventName(): string {
                return this._eventName;
            }

            // Gets whether this event is a property change notification
            get isPropChanged(): boolean {
                return this._isPropChanged === true;
            }
        }

        // Describe property info for nested directives.
        export class ComplexPropDescBase {
            public propertyName: string;
            public isArray: boolean = false;
            private _ownsObject: boolean = false;

            constructor(propertyName: string, isArray: boolean, ownsObject: boolean = false) {
                this.propertyName = propertyName;
                this.isArray = isArray;
                this._ownsObject = ownsObject;
            }

            get ownsObject(): boolean {
                return this.isArray || this._ownsObject;
            }
        }

        // Stores a control metadata as arrays of property, event and complex property descriptors.
        export class MetaDataBase {
            private _props: PropDescBase[] = [];
            private _events: EventDescBase[] = []; 
            private _complexProps: ComplexPropDescBase[] = [];
            // For a child directive, the name of parent's property to assign to. Being assigned indicates that this is a child directive.
            // Beign assigned to an empty string indicates that this is a child directive but parent property name should be defined
            // by the wj-property attribute on directive's tag.
            parentProperty: string;
            // For a child directive indicates whether the parent _propert is a collection.
            isParentPropertyArray: boolean;
            // For a child directive which is not a collection item indicates whether it should create an object or retrieve it 
            // from parent's _property.
            ownsObject: boolean;
            // For a child directive, the name of the property of the directive's underlying object that receives the reference
            // to the parent, or an empty string that indicates that the reference to the parent should be passed as the 
            // underlying object's constructor parameter.
            parentReferenceProperty: string;
            // The name of the control property represented by ng-model directive defined on the control's directive.
            ngModelProperty: string;

            constructor(props: PropDescBase[], events?: EventDescBase[], complexProps?: ComplexPropDescBase[],
                parentProperty?: string, isParentPropertyArray?: boolean, ownsObject?: boolean,
                parentReferenceProperty?: string, ngModelProperty?: string) {
                this.props = props;
                this.events = events;
                this.complexProps = complexProps;
                this.parentProperty = parentProperty;
                this.isParentPropertyArray = isParentPropertyArray;
                this.ownsObject = ownsObject;
                this.parentReferenceProperty = parentReferenceProperty;
                this.ngModelProperty = ngModelProperty;
            }

            get props(): PropDescBase[]{
                return this._props;
            }
            set props(value: PropDescBase[]) {
                this._props = value || [];
            }

            get events(): EventDescBase[] {
                return this._events;
            }
            set events(value: EventDescBase[]) {
                this._events = value || [];
            }

            get complexProps(): ComplexPropDescBase[] {
                return this._complexProps;
            }
            set complexProps(value: ComplexPropDescBase[]) {
                this._complexProps = value || [];
            }

            // Adds the specified arrays to the end of corresponding arrays of this object, and overwrite the simple properties
            // if specified. Returns 'this'.
            add(props: PropDescBase[], events?: EventDescBase[], complexProps?: ComplexPropDescBase[],
                parentProperty?: string, isParentPropertyArray?: boolean, ownsObject?: boolean,
                parentReferenceProperty?: string, ngModelProperty?: string): MetaDataBase {

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
            }

            addOptions(options: any) {
                for (var prop in options) {
                    var thisValue = this[prop],
                        optionsValue = options[prop];
                    if (thisValue instanceof Array) {
                        this[prop] = thisValue.concat(optionsValue || []);
                    }
                    else if (optionsValue !== undefined) {
                        this[prop] = optionsValue;
                    }
                }
                return this;
            }

            // Prepares a raw defined metadata for a usage, for exmple sorts the props array on priority.
            prepare() {
                // stable sort of props on priority
                var baseArr: PropDescBase[] = [].concat(this._props);
                this._props.sort(function (a: PropDescBase, b: PropDescBase): number {
                    var ret = a.priority - b.priority;
                    if (!ret) {
                        ret = baseArr.indexOf(a) - baseArr.indexOf(b);
                    }
                    return ret;
                });
            }
        }
    }
}
module wijmo.knockout {
    export class MetaFactory extends wijmo.interop.ControlMetaFactory {
        // Override to return wijmo.knockout.PropDesc
        public static CreateProp(propertyName: string, propertyType: wijmo.interop.PropertyType,
            bindingMode?: wijmo.interop.BindingMode, enumType?,
            isNativeControlProperty?: boolean, priority?: number): PropDesc {

            return new PropDesc(propertyName, propertyType, bindingMode, enumType, isNativeControlProperty, priority);
        }

        // Override to return wijmo.knockout.EventDesc
        public static CreateEvent(eventName: string, isPropChanged?: boolean): EventDesc {
            return new EventDesc(eventName, isPropChanged);
        }

        // Override to return wijmo.knockout.ComplexPropDesc
        public static CreateComplexProp(propertyName: string, isArray: boolean, ownsObject?: boolean): ComplexPropDesc {
            return new ComplexPropDesc(propertyName, isArray, ownsObject);
        }

        // Typecasted override.
        public static findProp(propName: string, props: PropDesc[]): PropDesc {
            return <PropDesc>wijmo.interop.ControlMetaFactory.findProp(propName, props);
        }

        // Typecasted override.
        public static findEvent(eventName: string, events: EventDesc[]): EventDesc {
            return <EventDesc>wijmo.interop.ControlMetaFactory.findEvent(eventName, events);
        }

        // Typecasted override.
        public static findComplexProp(propName: string, props: ComplexPropDesc[]): ComplexPropDesc {
            return <ComplexPropDesc>wijmo.interop.ControlMetaFactory.findComplexProp(propName, props);
        }

    }

    // Defines a delegate performing a custom assignment logic of a control property with a source value.
    // TBD: the plan is to move this platform agnostic definition to the shared metadata.
    export interface IUpdateControlHandler {
        // The link parameter references a 'link' object (WjLink in Angular, WjContext in Knockout).
        (link: any, propDesc: PropDesc, control: any, unconvertedValue: any, convertedValue: any): boolean;
    }

    export class PropDesc extends wijmo.interop.PropDescBase {
        // A callback allowing to perform a custom update of the control with the new source value.
        // Should return true if update is handled and standard assignment logic should not be applied; otherwise, should return false.
        updateControl: IUpdateControlHandler;
    }

    // Describes a scope event
    export class EventDesc extends wijmo.interop.EventDescBase {
    }

    // Describe property info for nested directives.
    export class ComplexPropDesc extends wijmo.interop.ComplexPropDescBase {
    }

} 
/**
 * Contains KnockoutJS bindings for the Wijmo controls.
 *
 * The bindings allow you to add Wijmo controls to
 * <a href="http://knockoutjs.com/" target="_blank">KnockoutJS</a>
 * applications using simple markup in HTML pages.
 *
 * To add a Wijmo control to a certain place in a page's markup, add the <b>&lt;div&gt;</b> 
 * element and define a binding for the control in the <b>data-bind</b> attribute.
 * The binding name corresponds to the control name with a wj prefix. For example, the @see:wjInputNumber 
 * binding represents the Wijmo @see:InputNumber control. The binding value is an object literal containing 
 * properties corresponding to the control's read-write property and event names, with their values defining 
 * the corresponding control property values and event handlers.
 *
 * The following markup creates a Wijmo <b>InputNumber</b> control with the <b>value</b> property bound to the 
 * view model's <b>theValue</b> property, the <b>step</b> property set to 1 and the <b>inputType</b> property set to 'text':
 *
 * <pre>&lt;div data-bind="wjInputNumber: { value: theValue, step: 1, inputType: 'text' }"&gt;&lt;/div&gt;</pre>
 * 
 * <h3>Custom elements</h3>
 * As an alternative to the standard Knockout binding syntax, the Wijmo for Knockout provides a possibility to declare controls 
 * in the page markup as custom elements, where the tag name corresponds to the control binding name and the attribute names 
 * correspond to the control property names. The element and parameter names must be formatted as lower-case with dashes instead 
 * of camel-case. The control in the example above can be defined as follows using the custom element syntax:
 * 
 * <pre>&lt;wj-input-number value="theValue" step="1" input-type="'text'"&gt;&lt;/wj-input-number&gt;</pre>
 * 
 * Note that attribute values should be defined using exactly the same JavaScript expressions syntax as you use in 
 * data-bind definitions. The Wijmo for Knockout preprocessor converts such elements to the conventional data-bind form, 
 * see the <b>Custom elements preprocessor</b> topic for more details.
 * 
 * 
 * <h3>Binding to control properties</h3>
 * Wijmo binding for KnockoutJS supports binding to any read-write properties on the control. You can assign any 
 * valid KnockoutJS expressions (e.g. constants, view model observable properties, or complex expressions) to the 
 * property. 
 *
 * Most of the properties provide one-way binding, which means that changes in the bound observable view model 
 * property cause changes in the control property that the observable is bound to, but not vice versa. 
 * But some properties support two-way binding, which means that changes made in the control property are 
 * propagated back to an observable bound to the control property as well. Two-way bindings are used for properties 
 * that can be changed by the control itself, by user interaction with the control, 
 * or by other occurences. For example, the InputNumber control provides two-way binding for the 
 * <b>value</b> and <b>text</b> properties, which are changed by the control while a user is typing a new value. 
 * The rest of the InputNumber properties operate in the one-way binding mode.
 * 
 * <h3>Binding to control events</h3>
 * To attach a handler to a control event, specify the event name as a property of the object literal defining 
 * the control binding, and the function to call on this event as a value of this property. 
 * Wijmo bindings follow the same rules for defining an event handler as used for the intrinsic KnockoutJS bindings 
 * like <b>click</b> and <b>event</b>. The event handler receives the following set of parameters, in the specified order:
 * <ul>
 * 	<li><b>data:</b> The current model value, the same as for native KnockoutJS bindings like <b>click</b> and <b>event</b>. </li>
 * 	<li><b>sender:</b> The sender of the event. </li>
 * 	<li><b>args:</b> The event arguments. </li>
 * </ul>
 * 
 * The following example creates an <b>InputNumber</b> control and adds an event handler for the <b>valueChanged</b>
 * event showing a dialog with a new control value.
 *
 * <pre>&lt;!-- HTML --&gt;
 * &lt;div data-bind="wjInputNumber: { value: theValue, step: 1, valueChanged: valueChangedEH }"&gt;&lt;/div&gt;
 * &nbsp;
 * //View Model
 * this.valueChangedEH = function (data, sender, args) {
 *     alert('The new value is: ' + sender.value);
 * }</pre>
 * 
 * The same control defined using the custom element syntax:
 * 
 * <pre>&lt;wj-input-number value="theValue" step="1" value-changed="valueChangedEH"&gt;&lt;/wj-input-number&gt;</pre>
 * 
 * <h3>Binding to undefined observables</h3>
 * View model observable properties assigned to an <i>undefined</i> value get special treatment by Wijmo 
 * bindings during the initialization phase. For example, if you create an observable as ko.observable(undefined) 
 * or ko.observable() and bind it to a control property, Wijmo does not assign a value to the control. Instead,  
 * for properties supporting two-way bindings, this is the way to initialize the observable with the control's 
 * default value, because after initialization the control binding updates bound observables with the control 
 * values of such properties. Note that an observable with a <i>null</i> value, e.g. ko.observable(null), gets 
 * the usual treatment and assigns null to the control property that it is bound to. After the primary 
 * initialization has finished, observables with undefined values go back to getting the usual treatment from 
 * Wijmo, and assign the control property with undefined.
 *
 * In the example below, the <b>value</b> property of the <b>InputNumber</b> control has its default value of 0
 * after initialization, and this same value is assigned to the view model <b>theValue</b> property:
 * <pre>&lt;!-- HTML --&gt;
 * &lt;div data-bind="wjInputNumber: { value: theValue }"&gt;&lt;/div&gt;
 * &nbsp;
 * //View Model
 * this.theValue = ko.observable();</pre>
 *
 * <h3>Defining complex and array properties</h3>
 * Some Wijmo controls have properties that contain an array or a complex object. For example, the 
 * @see:FlexChart control exposes <b>axisX</b> and <b>axisY</b> properties that represent an @see:Axis object; 
 * and the <b>series</b> property is an array of @see:Series objects. Wijmo provides special 
 * bindings for such types that we add to child elements of the control element. If the control exposes
 * multiple properties of the same complex type, then the <b>wjProperty</b> property of the complex 
 * type binding specifies which control property it defines. 
 *
 * The following example shows the markup used to create a <b>FlexChart</b> with <b>axisX</b> and <b>axisY</b> 
 * properties and two series objects defined:
 *
 * <pre>&lt;div data-bind="wjFlexChart: { itemsSource: data, bindingX: 'country' }"&gt;
 *     &lt;div data-bind="wjFlexChartAxis: { wjProperty: 'axisX', title: chartProps.titleX }"&gt;&lt;/div&gt;
 *     &lt;div data-bind="wjFlexChartAxis: { wjProperty: 'axisY', title: chartProps.titleY }"&gt;&lt;/div&gt;
 *     &lt;div data-bind="wjFlexChartSeries: { name: 'Sales', binding: 'sales' }"&gt;&lt;/div&gt;
 *     &lt;div data-bind="wjFlexChartSeries: { name: 'Downloads', binding: 'downloads' }"&gt;&lt;/div&gt;
 * &lt;/div&gt;</pre>
 *
 * The same control defined using the custom element syntax:
 * 
 * <pre>&lt;wj-flex-chart items-source="data" binding-x="'country'"&gt;
 *     &lt;wj-flex-chart-axis wj-property="'axisX'" title="chartProps.titleX"&gt;&lt;/wj-flex-chart-axis&gt;
 *     &lt;wj-flex-chart-axis wj-property="'axisY'" title="chartProps.titleY"&gt;&lt;/wj-flex-chart-axis&gt;
 *     &lt;wj-flex-chart-series name="'Sales'" binding"'sales'"&gt;&lt;/wj-flex-chart-series&gt;
 *     &lt;wj-flex-chart-series name="'Downloads'" binding"'downloads'"&gt;&lt;/wj-flex-chart-series&gt;
 * &lt;/wj-flex-chart&gt;</pre>
 * 
 * <h3>The <b>control</b> property </h3>
 * Each Wijmo control binding exposes a <b>control</b> property that references the Wijmo control instance created 
 * by the binding. This allows you to reference the control in view model code or in other bindings.
 *  
 * For example, the following markup creates a @see:FlexGrid control whose reference is stored in the <b>flex</b> 
 * observable property of a view model and is used in the button click event handler to move to the next grid record:
 *
 * <pre>&lt;!-- HTML --&gt;
 * &lt;div data-bind="'wjFlexGrid': { itemsSource: data, control: flex }"&gt;&lt;/div&gt;
 * &lt;button data-bind="click: moveToNext"&gt;Next&lt;/button&gt;
 * &nbsp;
 * //View Model
 * this.flex = ko.observable();
 * this.moveToNext = function () {
 *     this.flex().collectionView.moveCurrentToNext();
 * }</pre>
 *
 * <h3>The <b>initialized</b> event</h3>
 * Each Wijmo control binding exposes an <b>initialized</b> event and a Boolean <b>isInitialized</b> 
 * property. The event occurs right after the binding creates the control and fully initializes it 
 * with the values specified in the binding attributes. For bindings containing child bindings, for 
 * example, a <b>wjFlexGrid</b> with child <b>wjFlexGridColumn</b> bindings, this also means that 
 * child bindings have fully initialized and have been applied to the control represented by the 
 * parent binding. The isInitialized property is set to true right before triggering the initialized 
 * event. You can bind a view model observable property to the binding’s <b>isInitialized</b> property 
 * to access its value.
 *
 * The following example adjusts FlexGridColumn formatting after the control fully initializes with its 
 * bindings, which guarantees that these formats are not overwritten with formats defined in the bindings:
 * 
 * <pre>&lt;!-- HTML --&gt;
 * &lt;div data-bind="'wjFlexGrid': { itemsSource: dataArray, initialized: flexInitialized }"&gt;
 *      &lt;div data-bind="wjFlexGridColumn: { binding: 'sales', format: 'n2' }"&gt;&lt;/div&gt;
 *      &lt;div data-bind="wjFlexGridColumn: { binding: 'downloads', format: 'n2' }"&gt;&lt;/div&gt;
 * &lt;/div&gt;
 * &nbsp;
 * //View Model
 * this.flexInitialized = function (data, sender, args) {
 *     var columns = sender.columns;
 *     for (var i = 0; i &lt; columns.length; i++) {
 *         if (columns[i].dataType = wijmo.DataType.Number) {
 *             columns[i].format = 'n0’;
 *         }
 *     }
 * }</pre>
 *
 * <h3 id="custom_elem_preproc">Custom elements preprocessor</h3>
 * The Wijmo Knockout preprocessor uses the standard Knockout <a target="_blank" 
 * href="http://knockoutjs.com/documentation/binding-preprocessing.html">ko.bindingProvider.instance.preprocessNode</a> 
 * API. This may cause problems in cases where other custom preprocessors are used on the same page, because Knockout 
 * offers a single instance property for attaching a preprocessor function, and the next registering preprocessor 
 * removes the registration of the previous one.
 * 
 * To honor another attached preprocessor, the Wijmo Knockout preprocessor stores the currently registered preprocessor 
 * during initialization and delegates the work to it in cases where another processing node is not recognized 
 * as a Wijmo control element, thus organizing a preprocessor stack. But if you register another preprocessor 
 * after the Wijmo for Knockout preprocessor (that is, after the &lt;script&gt; reference to the <b>wijmo.knockout.js</b> 
 * module is executed) then you need to ensure that the other preprocessor behaves in a similar way; 
 * otherwise, the Wijmo Knockout preprocessor is disabled.
 * 
 * If you prefer to disable the Wijmo Knockout preprocessor, set the <b>wijmo.disableKnockoutTags</b> property 
 * to false before the <b>wijmo.knockout.js</b> module reference and after the references to the core Wijmo 
 * modules, for example:
 * 
 * <pre>&lt;script src="scripts/wijmo.js" type="text/javascript"&gt;&lt;/script&gt;
 * &lt;script src="scripts/wijmo.input.js" type="text/javascript"&gt;&lt;/script&gt;
 * &nbsp;
 * &lt;script type="text/javascript"&gt;
 *     wijmo.disableKnockoutTags = true;
 * &lt;/script&gt;
 * &lt;script src="scripts/wijmo.knockout.js" type="text/javascript"&gt;&lt;/script&gt;</pre>
 *
 * Note that in this case you can use only the conventional data-bind syntax for adding Wijmo controls to the page 
 * markup; the Wijmo custom elements are not recognized.

 */
module wijmo.knockout {

    // Represents a base class for Wijmo custom bindings. Technically corresponds to an object assigning to ko.bindingHandlers
    // in order to register a custom binding. Represents a Wijmo control or a child object like FlexGrid Column.
    // This is a singleton class. For each tag that uses the custom binding it creates a separate WjContext class instance
    // that services lifetime of the control created for the tag.
    export class WjBinding implements KnockoutBindingHandler {

        // Defines html element property name used to store WjContext object associated with the element.
        static _wjContextProp = '__wjKoContext';
        // The name of the nested binding attribute defining a parent property name to assign to.
        static _parPropAttr = 'wjProperty';
        // The name of the attribute providing the reference to the control.
        static _controlPropAttr = 'control';
        // Name of the attribute that provides the 'initialized' state value.
        static _initPropAttr = 'isInitialized';
        // Name of the attribute representing the 'initialized' event.
        static _initEventAttr = 'initialized';

        // Stores the binding metadata.
        _metaData: wijmo.interop.MetaDataBase;
        // #region Native API
        //options: any;
        init = function (element: any, valueAccessor: () => any, allBindings: KnockoutAllBindingsAccessor, viewModel: any, bindingContext: KnockoutBindingContext): any {
            this.ensureMetaData();
            //if (!this._metaData) {
            //    this._metaData = MetaFactory.getMetaData(this._getMetaDataId());
            //    this._initialize();
            //    this._metaData.prepare();
            //}
            return (<WjBinding>this)._initImpl(element, valueAccessor, allBindings, viewModel, bindingContext);
        }.bind(this);

        update = function (element: any, valueAccessor: () => any, allBindings: KnockoutAllBindingsAccessor, viewModel: any, bindingContext: KnockoutBindingContext): void {
            //console.log('#' + this['__DebugID'] + ' WjBinding.update');
            (<WjBinding>this)._updateImpl(element, valueAccessor, allBindings, viewModel, bindingContext);
        }.bind(this);
        // #endregion Native API

        // Call this method to ensure that metadata is loaded.
        // DO NOT OVERRIDE this method; instead, override the _initialize method to customize metedata.
        ensureMetaData() {
            if (!this._metaData) {
                this._metaData = MetaFactory.getMetaData(this._getMetaDataId());
                this._initialize();
                this._metaData.prepare();
            }
        }

        // Override this method to initialize the binding settings. Metadata is already loaded when this method is invoked.
        _initialize() {
        }

        _getControlConstructor(): any {
            return null;
        }

        // Gets the metadata ID, see the wijmo.interop.getMetaData method description for details.
        _getMetaDataId(): any {
            return this._getControlConstructor();
        }
        _createControl(element: any): any {
            var ctor = this._getControlConstructor();
            return new ctor(element);
        }

        _createWijmoContext(): WjContext {
            return new WjContext(this);
        }

        // Indicates whether this binding can operate as a child binding.
        _isChild(): boolean {
            return this._isParentInitializer() || this._isParentReferencer();
        }

        // Indicates whether this binding operates as a child binding that initializes a property of its parent.
        _isParentInitializer(): boolean {
            return this._metaData.parentProperty != undefined;
        }

        // Indicates whether this binding operates as a child binding that references a parent in its property or
        // a constructor.
        _isParentReferencer(): boolean {
            return this._metaData.parentReferenceProperty != undefined;
        }

        private _initImpl(element: any, valueAccessor: () => any, allBindings: KnockoutAllBindingsAccessor,
            viewModel: any, bindingContext: KnockoutBindingContext): any {
            var wjContext = this._createWijmoContext();
            element[WjBinding._wjContextProp] = wjContext;
            wjContext.element = element;
            if (this._isChild()) {
                wjContext.parentWjContext = element.parentElement[WjBinding._wjContextProp];
            }
            wjContext.valueAccessor = valueAccessor;
            wjContext.allBindings = allBindings;
            wjContext.viewModel = viewModel;
            wjContext.bindingContext = bindingContext;
            return wjContext.init(element, valueAccessor, allBindings, viewModel, bindingContext);
        }

        private _updateImpl = function (element: any, valueAccessor: () => any, allBindings: KnockoutAllBindingsAccessor, viewModel: any,
            bindingContext: KnockoutBindingContext): void {
            (<WjContext>(element[WjBinding._wjContextProp])).update(element, valueAccessor, allBindings, viewModel, bindingContext);
        }

    } 

    // Represents a context of WjBinding for a specific tag instance (similar to WjLink in Angular).
    export class WjContext {
        element: any;
        valueAccessor: any;
        allBindings: any;
        viewModel: any;
        bindingContext: any;
        control: any; 
        wjBinding: WjBinding;
        parentWjContext: WjContext;

        private _parentPropDesc: ComplexPropDesc;
        private _isInitialized: boolean = false;
        private static _debugId = 0;

        constructor(wjBinding: WjBinding) {
            this.wjBinding = wjBinding;
        }

        init(element: any, valueAccessor: () => any, allBindings: KnockoutAllBindingsAccessor, viewModel: any, bindingContext: KnockoutBindingContext): any {
            var lastAccessor = valueAccessor(),
                props = this.wjBinding._metaData.props,
                events = this.wjBinding._metaData.events;

            if (this._isChild()) {
                var propObs = lastAccessor[WjBinding._parPropAttr],
                    meta = this.wjBinding._metaData,
                    parPropName = propObs && ko.unwrap(propObs) || meta.parentProperty;
                this._parentPropDesc = MetaFactory.findComplexProp(parPropName, this.parentWjContext.wjBinding._metaData.complexProps)
                    || new ComplexPropDesc(parPropName, meta.isParentPropertyArray, meta.ownsObject);
            }
            this._initControl();
            this._safeUpdateSrcAttr(WjBinding._controlPropAttr, this.control);
            //Debug stuff
            //this.control.__DebugID = ++WjContext._debugId;
            //this['__DebugID'] = WjContext._debugId;

            // Initialize children right after control was created but before its properties was assigned with defined bindings.
            // This will allow to correctly apply properties like value or selectedIndex to controls like Menu whose child bindings
            // create an items source, so the mentioned properties will be assigned after collection has created.
            ko.applyBindingsToDescendants(bindingContext, element);

            this._childrenInitialized();


            for (var eIdx in events) {
                this._addEventHandler(events[eIdx]);
            }

            this._updateControl(valueAccessor /* , this.control, props */ );
            // Re-evaluate 'control' binding 
            // in order to simplify bindings to things like control.subProperty (e.g. flexGrid.collectionView).
            this._safeNotifySrcAttr(WjBinding._controlPropAttr);
            this._updateSource();
            this._isInitialized = true;
            this._safeUpdateSrcAttr(WjBinding._initPropAttr, true);
            var evObs = lastAccessor[WjBinding._initEventAttr];
            if (evObs) {
                ko.unwrap(evObs)(this.bindingContext['$data'], this.control, undefined);
            }

            return { controlsDescendantBindings: true };
        }

        update(element: any, valueAccessor: () => any, allBindings: KnockoutAllBindingsAccessor, viewModel: any, bindingContext: KnockoutBindingContext): void {
            this.valueAccessor = valueAccessor;
            this._updateControl(valueAccessor);
        }

        _createControl(): any {
            return this.wjBinding._createControl(this._parentInCtor() ? this.parentWjContext.control : this.element);
        }

        // Initialize the 'control' property, by creating a new or using the parent's object in case of child binding not owning
        // the object.
        // Override this method to perform custom initialization before or after control creation. The 'control' property is
        // undefined before this method call and defined on exit from this method.
        _initControl() {
            if (this._isChild()) {
                this.element.style.display = 'none';
                var parProp = this._getParentProp(),
                    parCtrl = this.parentWjContext.control;
                if (this._useParentObj()) {
                    this.control = parCtrl[parProp];
                }
                else {
                    var ctrl = this.control = this._createControl();
                    if (this._isParentInitializer()) {
                        if (this._isParentArray()) {
                            (<any[]>parCtrl[parProp]).push(ctrl);
                        }
                        else {
                            parCtrl[parProp] = ctrl;
                        }
                    }
                    if (this._isParentReferencer() && !this._parentInCtor()) {
                        ctrl[this._getParentReferenceProperty()] = parCtrl;
                    }
                }
            }
            else
                this.control = this._createControl();
        }

        _childrenInitialized() {
        }

        private _addEventHandler(eventDesc: EventDesc) {
            this.control[eventDesc.eventName].addHandler(
                (s, e) => {
                    this._updateSource();
                    var evObs = this.valueAccessor()[eventDesc.eventName];
                    if (evObs) {
                        ko.unwrap(evObs)(this.bindingContext['$data'], s, e);
                    }
                }, this);
        }

        private static _isUpdatingSource = false;
        private static _pendingSourceUpdates: WjContext[] = [];
        _updateSource() {
            WjContext._isUpdatingSource = true;
            try {
                var props = this.wjBinding._metaData.props;
                for (var idx in props) {
                    var propDesc = props[idx],
                        propName = propDesc.propertyName;
                    if (propDesc.shouldUpdateSource && propDesc.isNativeControlProperty) {
                        this._safeUpdateSrcAttr(propName, this.control[propName]);
                    }
                }
            }
            finally {
                WjContext._isUpdatingSource = false;
                while (WjContext._pendingSourceUpdates.length > 0) {
                    var wjCont = WjContext._pendingSourceUpdates.shift();
                    wjCont._updateControl();
                }
            }
        }

        private _isUpdatingControl = false;
        private _isSourceDirty = false;
        private _oldSourceValues = {};
        private _updateControl(valueAccessor = this.valueAccessor) {
            //console.log('#' + this['__DebugID'] + '_updateControl');
            var valSet = valueAccessor(),
                props = <PropDesc[]>this.wjBinding._metaData.props;
            if (WjContext._isUpdatingSource) {
                if (WjContext._pendingSourceUpdates.indexOf(this) < 0) {
                    WjContext._pendingSourceUpdates.push(this);
                }

                // IMPORTANT: We need to read all bound observable; otherwise, the update will never be called anymore !!!
                for (var i in props) {
                    ko.unwrap(valSet[props[i].propertyName]);
                }
                return;
            }
            try {
                var valArr = [],
                    propArr: PropDesc[] = [];
                // Collect properties/values changed since the last update.
                for (var i in props) {
                    var prop = props[i],
                        propName = prop.propertyName,
                        valObs = valSet[propName];
                    if (valObs !== undefined) {
                        var val = ko.unwrap(valObs);
                        if (val !== this._oldSourceValues[propName]) {
                            this._oldSourceValues[propName] = val;
                            valArr.push(val);
                            propArr.push(prop);
                        }
                    }
                }
                for (var i in valArr) {
                    var prop = propArr[i],
                        val = ko.unwrap(valSet[prop.propertyName]),
                        propName = prop.propertyName;
                    if (val !== undefined || this._isInitialized) {
                        var castedVal = this._castValueToType(val, prop);
                        if (!(prop.updateControl && prop.updateControl(this, prop, this.control, val, castedVal)) &&
                            prop.isNativeControlProperty) {
                            if (this.control[propName] != castedVal) {
                                this.control[propName] = castedVal;
                            }
                        }
                    }
                }
            }
            finally {
                //this._isUpdatingControl = false;
            }

        }

        // Casts value to the property type
        private _castValueToType(value: any, prop: PropDesc) {
            if (value == undefined) {
                //return undefined;
                return value;
            }

            var type = prop.propertyType;
            switch (type) {
                case wijmo.interop.PropertyType.Number:
                    if (typeof value == 'string') {
                        if (value.indexOf('*') >= 0) { // hack for star width ('*', '2*'...)
                            return value;
                        }
                        if (value.trim() === '') { // binding to an empty html input means null
                            return null;
                        }
                    }
                    return +value; // cast to number
                case wijmo.interop.PropertyType.Boolean:
                    if (value === 'true') {
                        return true;
                    }
                    if (value === 'false') {
                        return false;
                    }
                    return !!value; // cast to bool
                case wijmo.interop.PropertyType.String:
                    return value + ''; // cast to string
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
        }

        // Parsing DateTime values from string
        private _parseDate(value) {
            if (value && wijmo.isString(value)) {

                // For by-val attributes Angular converts a Date object to a
                // string wrapped in quotation marks, so we strip them.
                value = value.replace(/["']/g, '');

                // parse date/time using RFC 3339 pattern
                var dt = changeType(value, DataType.Date, 'r');
                if (isDate(dt)) {
                    return dt;
                }
            }
            return value;
        }

        // Update source attribute if possible (if it's defined and is a writable observable or a non-observable)
        _safeUpdateSrcAttr(attrName: string, value: any) {
            var ctx = this.valueAccessor();
            var attrObs = ctx[attrName];
            if ((<any>ko).isWritableObservable(attrObs)) {
                var val = ko.unwrap(attrObs);
                if (value != val) {
                    attrObs(value);
                }
            }
        }
        _safeNotifySrcAttr(attrName: string) {
            var ctx = this.valueAccessor();
            var attrObs = ctx[attrName];
            if ((<any>ko).isWritableObservable(attrObs) && attrObs.valueHasMutated) {
                attrObs.valueHasMutated();
            }
        }

        //Determines whether this is a child link.
        private _isChild(): boolean {
            return this.wjBinding._isChild();
        }
        // Indicates whether this link operates as a child link that initializes a property of its parent.
        private _isParentInitializer(): boolean {
            return this.wjBinding._isParentInitializer();
        }

        // Indicates whether this link operates as a child link that references a parent in its property or
        // a constructor.
        private _isParentReferencer(): boolean {
            return this.wjBinding._isParentReferencer();
        }

        //For the child directives returns parent's property name that it services. Property name defined via
        //the wjProperty attribute of directive tag has priority over the directive._property definition.
        //IMPORTANT: functionality is based on _parentPropDesc
        private _getParentProp(): string {
            return this._isParentInitializer() ? this._parentPropDesc.propertyName : undefined;
        }
        // For a child directive, the name of the property of the directive's underlying object that receives the reference
        // to the parent, or an empty string that indicates that the reference to the parent should be passed as the 
        // underlying object's constructor parameter.
        private _getParentReferenceProperty(): string {
            return this.wjBinding._metaData.parentReferenceProperty;
        }

        // Determines whether the child link uses an object created by the parent property, instead of creating it by
        // itself, and thus object's initialization should be delayed until parent link's control is created.
        //IMPORTANT: functionality is based on _parentPropDesc
        private _useParentObj(): boolean {
            //return this._isChild() && !this._parentPropDesc.isArray && !this._parentPropDesc.ownsObject;
            return !this._isParentReferencer() &&
                this._isParentInitializer() && !this._parentPropDesc.isArray && !this._parentPropDesc.ownsObject;
        }

        // For the child link, determines whether the servicing parent property is an array.
        //IMPORTANT: functionality is based on _parentPropDesc
        private _isParentArray() {
            return this._parentPropDesc.isArray;
        }

        // For the child referencer directive, indicates whether the parent should be passed as a parameter the object
        // constructor.
        private _parentInCtor(): boolean {
            return this._isParentReferencer() && this._getParentReferenceProperty() == '';
        }

    }


} //end of module


module wijmo.knockout {

    /**
     * KnockoutJS binding for the @see:Tooltip class.
     *
     * Use the @see:wjTooltip binding to add tooltips to elements on the page. 
     * The @see:wjTooltip supports HTML content, smart positioning, and touch.
     *
     * The @see:wjTooltip binding is specified on an 
     * element that the tooltip applies to. The value is the tooltip
     * text or the id of an element that contains the text. For example:
     *
     * <pre>&lt;p data-bind="wjTooltip: '#fineprint'" &gt;
     *     Regular paragraph content...&lt;/p&gt;
     * ...
     * &lt;div id="fineprint" style="display:none" &gt;
     *   &lt;h3&gt;Important Note&lt;/h3&gt;
     *   &lt;p&gt;
     *     Data for the current quarter is estimated by pro-rating etc...&lt;/p&gt;
     * &lt;/div&gt;</pre>
     */
    export class wjTooltip extends WjBinding {
        _getControlConstructor(): any {
            return wijmo.Tooltip;
        }

        _createWijmoContext(): WjContext {
            return new WjTooltipContext(this);
        }

    }

    export class WjTooltipContext extends WjContext {
        update(element: any, valueAccessor: () => any, allBindings: KnockoutAllBindingsAccessor, viewModel: any, bindingContext: KnockoutBindingContext): void {
            super.update(element, valueAccessor, allBindings, viewModel, bindingContext);
            this._updateTooltip();
        }

        private _updateTooltip() {
            (<wijmo.Tooltip><any>this.control).setTooltip(this.element, ko.unwrap(this.valueAccessor()));
        }
    }
} 

(<any>(ko.bindingHandlers)).wjTooltip = new wijmo.knockout.wjTooltip();


module wijmo.knockout {

    // DropDown custom binding.
    // Abstract class, not for use in markup
    export class WjDropDownBinding extends WjBinding {
        _getControlConstructor(): any {
            return wijmo.input.DropDown;
        }
    }

    /**
     * KnockoutJS binding for the @see:ComboBox control.
     *
     * Use the @see:wjComboBox binding to add @see:ComboBox controls to your 
     * KnockoutJS applications. For example:
     * 
     * <pre>&lt;p&gt;Here is a ComboBox control:&lt;/p&gt;
     * &lt;div data-bind="wjComboBox: {
     *   itemsSource: countries,
     *   text: theCountry,
     *   isEditable: false,
     *   placeholder: 'country' }"&gt;
     * &lt;/div&gt;</pre>
     *
     * The <b>wjComboBox</b> binding supports all read-write properties and events of 
     * the @see:ComboBox control. The following properties provide two-way binding mode:
     * <ul>
     * 	<li><b>isDroppedDown</b></li>
     * 	<li><b>text</b></li>
     * 	<li><b>selectedIndex</b></li>
     * 	<li><b>selectedItem</b></li>
     * 	<li><b>selectedValue</b></li>
     * </ul>
     */
    export class wjComboBox extends WjDropDownBinding {
        _getControlConstructor(): any {
            return wijmo.input.ComboBox;
        }
    }

    /**
     * KnockoutJS binding for the @see:AutoComplete control.
     *
     * Use the @see:wjAutoComplete binding to add @see:AutoComplete controls to your 
     * KnockoutJS applications. For example:
     * 
     * <pre>&lt;p&gt;Here is an AutoComplete control:&lt;/p&gt;
     * &lt;div data-bind="wjAutoComplete: {
     *   itemsSource: countries,
     *   text: theCountry,
     *   isEditable: false,
     *   placeholder: 'country' }"&gt;
     * &lt;/div&gt;</pre>
     *
     * The <b>wjAutoComplete</b> binding supports all read-write properties and events of 
     * the @see:AutoComplete control. The following properties provide two-way binding mode:
     * <ul>
     * 	<li><b>isDroppedDown</b></li>
     * 	<li><b>text</b></li>
     * 	<li><b>selectedIndex</b></li>
     * 	<li><b>selectedItem</b></li>
     * 	<li><b>selectedValue</b></li>
     * </ul>
     */
    export class wjAutoComplete extends wjComboBox {
        _getControlConstructor(): any {
            return wijmo.input.AutoComplete;
        }
    }

    /**
     * KnockoutJS binding for the @see:Calendar control.
     *
     * Use the @see:wjCalendar binding to add @see:Calendar controls to your 
     * KnockoutJS applications. For example:
     * 
     * <pre>&lt;p&gt;Here is a Calendar control:&lt;/p&gt;
     * &lt;div 
     *   data-bind="wjCalendar: { value: theDate }"&gt;
     * &lt;/div&gt;</pre>
     *
     * The <b>wjCalendar</b> binding supports all read-write properties and events of 
     * the @see:Calendar control. The following properties provide two-way binding mode:
     * <ul>
     * 	<li><b>value</b></li>
     * 	<li><b>displayMonth</b></li>
     * </ul>
     */
    export class wjCalendar extends WjBinding {
        _getControlConstructor(): any {
            return wijmo.input.Calendar;
        }
    }

    /**
     * KnockoutJS binding for the @see:ColorPicker control.
     *
     * Use the @see:wjColorPicker binding to add @see:ColorPicker controls to your 
     * KnockoutJS applications. For example:
     * 
     * <pre>&lt;p&gt;Here is a ColorPicker control:&lt;/p&gt;
     * &lt;div 
     *   data-bind="wjColorPicker: { value: theColor }"&gt;
     * &lt;/div&gt;</pre>
     *
     * The <b>wjColorPicker</b> binding supports all read-write properties and events of 
     * the @see:ColorPicker control. The following properties provide two-way binding mode:
     * <ul>
     * 	<li><b>value</b></li>
     * </ul>
     */
    export class wjColorPicker extends WjBinding {
        _getControlConstructor(): any {
            return wijmo.input.ColorPicker;
        }
    }

    /**
     * KnockoutJS binding for the @see:ListBox control.
     *
     * Use the @see:wjListBox binding to add @see:ListBox controls to your 
     * KnockoutJS applications. For example:
     * 
     * <pre>&lt;p&gt;Here is a ListBox control:&lt;/p&gt;
     * &lt;div data-bind="wjListBox: {
     *   itemsSource: countries,
     *   selectedItem: theCountry }"&gt;
     * &lt;/div&gt;</pre>
     *
     * The <b>wjListBox</b> binding supports all read-write properties and events of 
     * the @see:ListBox control. The following properties provide two-way binding mode:
     * <ul>
     * 	<li><b>selectedIndex</b></li>
     * 	<li><b>selectedItem</b></li>
     * 	<li><b>selectedValue</b></li>
     * </ul>
     */
    export class wjListBox extends WjBinding {
        _getControlConstructor(): any {
            return wijmo.input.ListBox;
        }
    }

    /**
     * KnockoutJS binding for the @see:Menu control.
     *
     * Use the @see:wjMenu binding to add @see:Menu controls to your 
     * KnockoutJS applications. For example:
     * 
     * <pre>&lt;p&gt;Here is a Menu control used as a value picker:&lt;/p&gt;
     * &lt;div data-bind="wjMenu: { value: tax, header: 'Tax' }"&gt;
     *     &lt;span data-bind="wjMenuItem: { value: 0 }"&gt;Exempt&lt;/span&gt;
     *     &lt;span data-bind="wjMenuSeparator: {}"&gt;&lt;/span&gt;
     *     &lt;span data-bind="wjMenuItem: { value: .05 }"&gt;5%&lt;/span&gt;
     *     &lt;span data-bind="wjMenuItem: { value: .1 }"&gt;10%&lt;/span&gt;
     *     &lt;span data-bind="wjMenuItem: { value: .15 }"&gt;15%&lt;/span&gt;
     * &lt;/div&gt;</pre>
     * 
     * The <b>wjMenu</b> binding may contain the following child bindings: @see:wjMenuItem, @see:wjMenuSeparator.
     *
     * The <b>wjMenu</b> binding supports all read-write properties and events of 
     * the @see:Menu control. The following properties provide two-way binding mode:
     * <ul>
     * 	<li><b>isDroppedDown</b></li>
     * 	<li><b>text</b></li>
     * 	<li><b>selectedIndex</b></li>
     * 	<li><b>selectedItem</b></li>
     * 	<li><b>selectedValue</b></li>
     *  <li><b>value</b></li>
     * </ul>
     */
    export class wjMenu extends wjComboBox {
        _getControlConstructor(): any {
            return wijmo.input.Menu;
        }

        _createWijmoContext(): WjContext {
            return new WjMenuContext(this);
        }

        _initialize() {
            super._initialize();
            var valueDesc = MetaFactory.findProp('value', <PropDesc[]>this._metaData.props);
            valueDesc.updateControl = this._updateControlValue;
        }

        private _updateControlValue(link: any, propDesc: PropDesc, control: any, unconvertedValue: any, convertedValue: any): boolean {
            if (convertedValue != null) {
                control.selectedValue = convertedValue;
                (<WjMenuContext>link)._updateHeader();
            }

            return true;
        }

    }

    export class WjMenuContext extends WjContext {
        _initControl() {
            super._initControl();
            var menuCtrl = <wijmo.input.Menu>this.control;
            menuCtrl.displayMemberPath = 'header';
            menuCtrl.commandPath = 'cmd';
            menuCtrl.commandParameterPath = 'cmdParam';
            menuCtrl.selectedValuePath = 'value';
            menuCtrl.itemsSource = new wijmo.collections.ObservableArray();

            // update 'value' and header when an item is clicked
            menuCtrl.itemClicked.addHandler(() => {
                this._safeUpdateSrcAttr('value', menuCtrl.selectedValue);
                this._updateHeader();
            });
        }

        _childrenInitialized() {
            super._childrenInitialized();
            this.control.selectedIndex = 0;
            this._updateHeader();
        }

        // update header to show the currently selected value
        _updateHeader() {
            var control = <wijmo.input.Menu>this.control,
                valSet = this.valueAccessor(),
                newHeader = ko.unwrap(valSet['header']);
            //control.header = scope.header;
            if (ko.unwrap(valSet['value']) !== undefined && control.selectedItem && control.displayMemberPath) {
                var currentValue = control.selectedItem[control.displayMemberPath];
                if (currentValue != null) {
                    newHeader += ': <b>' + currentValue + '</b>';
                }
            }
            control.header = newHeader;
        }
    }

    /**
     * KnockoutJS binding for menu items.
     *
     * Use the @see:wjMenuItem binding to add menu items to a @see:Menu control. 
     * The @see:wjMenuItem binding must be contained in a @see:wjMenu binding.
     * For example:
     * 
     * <pre>&lt;p&gt;Here is a Menu control with four menu items:&lt;/p&gt;
     * &lt;div data-bind="wjMenu: { value: tax, header: 'Tax' }"&gt;
     *     &lt;span data-bind="wjMenuItem: { value: 0 }"&gt;Exempt&lt;/span&gt;
     *     &lt;span data-bind="wjMenuItem: { value: .05 }"&gt;5%&lt;/span&gt;
     *     &lt;span data-bind="wjMenuItem: { value: .1 }"&gt;10%&lt;/span&gt;
     *     &lt;span data-bind="wjMenuItem: { value: .15 }"&gt;15%&lt;/span&gt;
     * &lt;/div&gt;</pre>
     *
     * The <b>wjMenuItem</b> binding supports the following attributes: 
     *
     * <dl class="dl-horizontal">
     *   <dt>cmd</dt>       <dd>Function to execute in the controller when the item is clicked.</dd>
     *   <dt>cmdParam</dt>  <dd>Parameter passed to the <b>cmd</b> function when the item is clicked.</dd>
     *   <dt>value</dt>     <dd>Value selected when the item is clicked (use either this or <b>cmd</b>).</dd>
     * </dl class="dl-horizontal">
     */
    export class wjMenuItem extends WjBinding {
        _getMetaDataId(): any {
            return 'MenuItem';
        }

        _createWijmoContext(): WjContext {
            return new WjMenuItemContext(this);
        }

        _initialize() {
            super._initialize();
            var meta = this._metaData;
            meta.parentProperty = 'itemsSource';
            meta.isParentPropertyArray = true;
        }

    }

    export class WjMenuItemContext extends WjContext {
        _createControl(): any {
            return { header: this.element.innerHTML, cmd: null, cmdParam: null, value: null };
        }
    }

    /**
     * KnockoutJS binding for menu separators.
     *
     * The the @see:wjMenuSeparator adds a non-selectable separator to a @see:Menu control, and has no attributes.
     * It must be contained in a @see:wjMenu binding. For example:
     * 
     * <pre>&lt;p&gt;Here is a Menu control with four menu items and one separator:&lt;/p&gt;
     * &lt;div data-bind="wjMenu: { value: tax, header: 'Tax' }"&gt;
     *     &lt;span data-bind="wjMenuItem: { value: 0 }"&gt;Exempt&lt;/span&gt;
     *     &lt;span data-bind="wjMenuSeparator: {}"&gt;&lt;/span&gt;
     *     &lt;span data-bind="wjMenuItem: { value: .05 }"&gt;5%&lt;/span&gt;
     *     &lt;span data-bind="wjMenuItem: { value: .1 }"&gt;10%&lt;/span&gt;
     *     &lt;span data-bind="wjMenuItem: { value: .15 }"&gt;15%&lt;/span&gt;
     * &lt;/div&gt;</pre>
     */
    export class wjMenuSeparator extends WjBinding {
        _getMetaDataId(): any {
            return 'MenuSeparator';
        }

        _initialize() {
            super._initialize();
            var meta = this._metaData;
            meta.parentProperty = 'itemsSource';
            meta.isParentPropertyArray = true;
        }

        _createControl(element: any): any {
            return { header: '<div style="width:100%;height:1px;background-color:lightgray;opacity:.2"/>' };
        }
    }

    /**
     * KnockoutJS binding for the @see:InputDate control.
     *
     * Use the @see:wjInputDate binding to add @see:InputDate controls to your 
     * KnockoutJS applications. For example:
     * 
     * <pre>&lt;p&gt;Here is an InputDate control:&lt;/p&gt;
     * &lt;div data-bind="wjInputDate: {
     *   value: theDate,
     *   format: 'M/d/yyyy' }"&gt;
     * &lt;/div&gt;</pre>
     *
     * The <b>wjInputDate</b> binding supports all read-write properties and events of 
     * the @see:InputDate control. The following properties provide two-way binding mode:
     * <ul>
     * 	<li><b>isDroppedDown</b></li>
     * 	<li><b>text</b></li>
     * 	<li><b>value</b></li>
     * </ul>
     */
    export class wjInputDate extends WjDropDownBinding {
        _getControlConstructor(): any {
            return wijmo.input.InputDate;
        }
    }

    /**
     * KnockoutJS binding for the @see:InputNumber control.
     *
     * Use the @see:wjInputNumber binding to add @see:InputNumber controls to your 
     * KnockoutJS applications. For example:
     * 
     * <pre>&lt;p&gt;Here is an InputNumber control:&lt;/p&gt;
     * &lt;div data-bind="wjInputNumber: {
     *   value: theNumber,
     *   min: 0,
     *   max: 10,
     *   format: 'n0',
     *   placeholder: 'number between zero and ten' }"&gt;
     * &lt;/div&gt;</pre>
     *
     * The <b>wjInputNumber</b> binding supports all read-write properties and events of 
     * the @see:InputNumber control. The following properties provide two-way binding mode:
     * <ul>
     * 	<li><b>value</b></li>
     * 	<li><b>text</b></li>
     * </ul>
     */
    export class wjInputNumber extends WjBinding {
        _getControlConstructor(): any {
            return wijmo.input.InputNumber;
        }
    }

    /**
     * KnockoutJS binding for the @see:InputMask control.
     *
     * Use the @see:wjInputMask binding to add @see:InputMask controls to your 
     * KnockoutJS applications. For example:
     * 
     * <pre>&lt;p&gt;Here is an InputMask control:&lt;/p&gt;
     * &lt;div data-bind="wjInputMask: {
     *   mask: '99/99/99',
     *   promptChar: '*' }"&gt;
     * &lt;/div&gt;</pre>
     *
     * The <b>wjInputMask</b> binding supports all read-write properties and events of 
     * the @see:InputMask control. The <b>value</b> property provides two-way binding mode.
     */
    export class wjInputMask extends WjBinding {
        _getControlConstructor(): any {
            return wijmo.input.InputMask;
        }
    }

    /**
     * KnockoutJS binding for the @see:InputTime control.
     *
     * Use the @see:wjInputTime binding to add @see:InputTime controls to your 
     * KnockoutJS applications. For example:
     * 
     * <pre>&lt;p&gt;Here is an InputTime control:&lt;/p&gt;
     * &lt;div data-bind="wjInputTime: {
     *   min: new Date(2014, 8, 1, 9, 0),
     *   max: new Date(2014, 8, 1, 17, 0),
     *   step: 15,
     *   format: 'h:mm tt',
     *   value: theDate }"&gt;
     * &lt;/div&gt;</pre>
     *
     * The <b>wjInputTime</b> binding supports all read-write properties and events of 
     * the @see:InputTime control. The following properties provide two-way binding mode:
     * <ul>
     * 	<li><b>isDroppedDown</b></li>
     * 	<li><b>text</b></li>
     * 	<li><b>selectedIndex</b></li>
     * 	<li><b>selectedItem</b></li>
     * 	<li><b>selectedValue</b></li>
     *  <li><b>value</b></li>
     * </ul>
     */
    export class wjInputTime extends wjComboBox {
        _getControlConstructor(): any {
            return wijmo.input.InputTime;
        }
    }

    /**
     * KnockoutJS binding for the @see:InputColor control.
     *
     * Use the @see:wjInputColor binding to add @see:InputColor controls to your 
     * KnockoutJS applications. For example:
     * 
     * <pre>&lt;p&gt;Here is a InputColor control:&lt;/p&gt;
     * &lt;div 
     *   data-bind="wjInputColor: { value: theColor }"&gt;
     * &lt;/div&gt;</pre>
     *
     * The <b>wjInputColor</b> binding supports all read-write properties and events of 
     * the @see:InputColor control. The following properties provide two-way binding mode:
     * <ul>
     * 	<li><b>isDroppedDown</b></li>
     * 	<li><b>text</b></li>
     * 	<li><b>value</b></li>
     * </ul>
     */
    export class wjInputColor extends WjDropDownBinding {
        _getControlConstructor(): any {
            return wijmo.input.InputColor;
        }
    }

    // Abstract
    export class WjCollectionViewBaseBinding extends WjBinding {
        _createControl(element: any): any {
            return null;
        }

        _createWijmoContext(): WjContext {
            return new WjCollectionViewContext(this);
        }

        // Returns CV template 
        _getTemplate() {
            return '';
        }
    }

    export class WjCollectionViewContext extends WjContext {
        private _localVM: any;
        // WARNING: Never assign a null value to _localVM.cv, because bindings to subproperties (cv.prop) will raise an exception.
        // Instead, assign this dummy _emptyCV.
        private _emptyCV = new wijmo.collections.CollectionView([]);

        init(element: any, valueAccessor: () => any, allBindings: KnockoutAllBindingsAccessor, viewModel: any, bindingContext: KnockoutBindingContext): any {
            element.innerHTML = (<WjCollectionViewBaseBinding>this.wjBinding)._getTemplate();
            var cv = ko.unwrap(valueAccessor().cv) || this._emptyCV;
            this._subscribeToCV(cv);
            this._localVM = {
                cv: ko.observable(cv)
            };
            var innerBindingContext = bindingContext.createChildContext(this._localVM);
            ko.applyBindingsToDescendants(innerBindingContext, element);

            return { controlsDescendantBindings: true };
        }

        update(element: any, valueAccessor: () => any, allBindings: KnockoutAllBindingsAccessor, viewModel: any, bindingContext: KnockoutBindingContext): void {
            var newCV = ko.unwrap(valueAccessor().cv) || this._emptyCV,
                oldCV = ko.unwrap(this._localVM.cv);
            if (newCV !== oldCV) {
                this._unsubscribeFromCV(oldCV);
                this._subscribeToCV(newCV);
                this._localVM.cv(newCV);
            }
        }

        private _subscribeToCV(cv: wijmo.collections.CollectionView) {
            if (cv) {
                cv.collectionChanged.addHandler(this._forceBindingsUpdate, this);
                cv.currentChanged.addHandler(this._forceBindingsUpdate, this);
                cv.pageChanged.addHandler(this._forceBindingsUpdate, this);
            }
        }

        private _unsubscribeFromCV(cv: wijmo.collections.CollectionView) {
            if (cv) {
                cv.collectionChanged.removeHandler(this._forceBindingsUpdate, this);
                cv.currentChanged.removeHandler(this._forceBindingsUpdate, this);
                cv.pageChanged.removeHandler(this._forceBindingsUpdate, this);
            }
        }

        private _forceBindingsUpdate(s, e) {
            this._localVM.cv.valueHasMutated();
        }
    }

    /**
     * KnockoutJS binding for an @see:ICollectionView pager element.
     *
     * Use the @see:wjCollectionViewPager directive to add an element that allows users to
     * navigate through the pages in a paged @see:ICollectionView. For example:
     * 
     * <pre>Here is a CollectionViewPager:&lt;/p&gt;
     * &lt;div 
     *   data-bind="wjCollectionViewPager: { cv: myCollectionView }"&gt;
     * &lt;/div&gt;</pre>
     *
     * The @see:wjCollectionViewPager directive has a single attribute:
     * 
     * <dl class="dl-horizontal">
     *   <dt>cv</dt>  <dd>Reference to the paged @see:ICollectionView object to navigate.</dd>
     * </dl>
     */
    export class wjCollectionViewPager extends WjCollectionViewBaseBinding {
        _getMetaDataId(): any {
            return 'CollectionViewPager';
        }

        _getTemplate() {
            return '<div class="wj-control wj-content wj-pager">' +
                '    <div class="wj-input-group">' +
                '        <span class="wj-input-group-btn" >' +
                '            <button class="wj-btn wj-btn-default" type="button"' +
                '               data-bind="click: function () { cv().moveToFirstPage() },' +
                '               disable: cv().pageIndex <= 0">' +
                '                <span class="wj-glyph-left" style="margin-right: -4px;"></span>' +
                '                <span class="wj-glyph-left"></span>' +
                '            </button>' +
                '        </span>' +
                '        <span class="wj-input-group-btn" >' +
                '           <button class="wj-btn wj-btn-default" type="button"' +
                '               data-bind="click: function () { cv().moveToPreviousPage() },' +
                '               disable: cv().pageIndex <= 0">' +
                '                <span class="wj-glyph-left"></span>' +
                '            </button>' +
                '        </span>' +
                '        <input type="text" class="wj-form-control" data-bind="' +
                '            value: cv().pageIndex + 1 + \' / \' + cv().pageCount' +
                '        " disabled />' +
                '        <span class="wj-input-group-btn" >' +
                '            <button class="wj-btn wj-btn-default" type="button"' +
                '               data-bind="click: function () { cv().moveToNextPage() },' +
                '               disable: cv().pageIndex >= cv().pageCount - 1">' +
                '                <span class="wj-glyph-right"></span>' +
                '            </button>' +
                '        </span>' +
                '        <span class="wj-input-group-btn" >' +
                '            <button class="wj-btn wj-btn-default" type="button"' +
                '               data-bind="click: function () { cv().moveToLastPage() },' +
                '               disable: cv().pageIndex >= cv().pageCount - 1">' +
                '                <span class="wj-glyph-right"></span>' +
                '                <span class="wj-glyph-right" style="margin-left: -4px;"></span>' +
                '            </button>' +
                '        </span>' +
                '    </div>' +
                '</div>';
        }
    }

    /**
     * KnockoutJS binding for an @see:ICollectionView navigator element.
     *
     * Use the @see:wjCollectionViewNavigator directive to add an element that allows users to
     * navigate through the items in an @see:ICollectionView. For example:
     * 
     * <pre>Here is a CollectionViewNavigator:&lt;/p&gt;
     * &lt;div 
     *   data-bind="wjCollectionViewNavigator: { cv: myCollectionView }"&gt;
     * &lt;/div&gt;</pre>
     *
     * The @see:wjCollectionViewNavigator directive has a single attribute:
     * 
     * <dl class="dl-horizontal">
     *   <dt>cv</dt>  <dd>Reference to the @see:ICollectionView object to navigate.</dd>
     * </dl>
     */
    export class wjCollectionViewNavigator extends WjCollectionViewBaseBinding {
        _getMetaDataId(): any {
            return 'CollectionViewNavigator';
        }

        _getTemplate() {
            return '<div class="wj-control wj-content wj-pager">' +
                '    <div class="wj-input-group">' +
                '        <span class="wj-input-group-btn" >' +
                '            <button class="wj-btn wj-btn-default" type="button"' +
                '               data-bind="click: function () { cv().moveCurrentToFirst() },' +
                '               disable: cv().currentPosition <= 0">' +
                '                <span class="wj-glyph-left" style="margin-right: -4px;"></span>' +
                '                <span class="wj-glyph-left"></span>' +
                '            </button>' +
                '        </span>' +
                '        <span class="wj-input-group-btn" >' +
                '           <button class="wj-btn wj-btn-default" type="button"' +
                '               data-bind="click: function () { cv().moveCurrentToPrevious() },' +
                '               disable: cv().currentPosition <= 0">' +
                '                <span class="wj-glyph-left"></span>' +
                '            </button>' +
                '        </span>' +
                '        <input type="text" class="wj-form-control" data-bind="' +
                '            value: cv().currentPosition + 1 + \' / \' + cv().itemCount' +
                '        " disabled />' +
                '        <span class="wj-input-group-btn" >' +
                '            <button class="wj-btn wj-btn-default" type="button"' +
                '               data-bind="click: function () { cv().moveCurrentToNext() },' +
                '               disable: cv().currentPosition >= cv().itemCount - 1">' +
                '                <span class="wj-glyph-right"></span>' +
                '            </button>' +
                '        </span>' +
                '        <span class="wj-input-group-btn" >' +
                '            <button class="wj-btn wj-btn-default" type="button"' +
                '               data-bind="click: function () { cv().moveCurrentToLast() },' +
                '               disable: cv().currentPosition >= cv().itemCount - 1">' +
                '                <span class="wj-glyph-right"></span>' +
                '                <span class="wj-glyph-right" style="margin-left: -4px;"></span>' +
                '            </button>' +
                '        </span>' +
                '    </div>' +
                '</div>';

        }
    }

} 

// Register bindings
(<any>(ko.bindingHandlers)).wjComboBox = new wijmo.knockout.wjComboBox();
(<any>(ko.bindingHandlers)).wjAutoComplete = new wijmo.knockout.wjAutoComplete();
(<any>(ko.bindingHandlers)).wjCalendar = new wijmo.knockout.wjCalendar();
(<any>(ko.bindingHandlers)).wjColorPicker = new wijmo.knockout.wjColorPicker();
(<any>(ko.bindingHandlers)).wjListBox = new wijmo.knockout.wjListBox();
(<any>(ko.bindingHandlers)).wjMenu = new wijmo.knockout.wjMenu();
(<any>(ko.bindingHandlers)).wjMenuItem = new wijmo.knockout.wjMenuItem();
(<any>(ko.bindingHandlers)).wjMenuSeparator = new wijmo.knockout.wjMenuSeparator();
(<any>(ko.bindingHandlers)).wjInputDate = new wijmo.knockout.wjInputDate();
(<any>(ko.bindingHandlers)).wjInputNumber = new wijmo.knockout.wjInputNumber();
(<any>(ko.bindingHandlers)).wjInputMask = new wijmo.knockout.wjInputMask();
(<any>(ko.bindingHandlers)).wjInputTime = new wijmo.knockout.wjInputTime();
(<any>(ko.bindingHandlers)).wjInputColor = new wijmo.knockout.wjInputColor();
(<any>(ko.bindingHandlers)).wjCollectionViewNavigator = new wijmo.knockout.wjCollectionViewNavigator();
(<any>(ko.bindingHandlers)).wjCollectionViewPager = new wijmo.knockout.wjCollectionViewPager();


module wijmo.knockout {
    /**
     * KnockoutJS binding for the @see:FlexGrid control.
     *
     * Use the @see:wjFlexGrid binding to add @see:FlexGrid controls to your 
     * KnockoutJS applications. For example:
     * 
     * <pre>&lt;p&gt;Here is a FlexGrid control:&lt;/p&gt;
     * &lt;div data-bind="wjFlexGrid: { itemsSource: data }"&gt;
     *     &lt;div data-bind="wjFlexGridColumn: { 
     *         header: 'Country', 
     *         binding: 'country', 
     *         width: '*' }"&gt;
     *     &lt;/div&gt;
     *     &lt;div data-bind="wjFlexGridColumn: { 
     *         header: 'Date', 
     *         binding: 'date' }"&gt;
     *     &lt;/div&gt;
     *     &lt;div data-bind="wjFlexGridColumn: { 
     *         header: 'Revenue', 
     *         binding: 'amount', 
     *         format: 'n0' }"&gt;
     *     &lt;/div&gt;
     *     &lt;div data-bind="wjFlexGridColumn: { 
     *         header: 'Active', 
     *         binding: 'active' }"&gt;
     *     &lt;/div&gt;
     * &lt;/div&gt;</pre>
     * 
     * The <b>wjFlexGrid</b> binding may contain @see:wjFlexGridColumn child bindings.
     *
     * The <b>wjFlexGrid</b> binding supports all read-write properties and events of 
     * the @see:FlexGrid control, except for the <b>scrollPosition</b> and 
     * <b>selection</b> properties. 
     */
    export class wjFlexGrid extends WjBinding {
        static _columnTemplateProp = '_wjkoColumnTemplate';
        static _cellClonedTemplateProp = '__wjkoClonedTempl';
        static _cellVMProp = '__wjkoCellVM';
        static _columnStyleBinding = 'wjStyle';
        static _columnStyleProp = '__wjkoStyle';

        _getControlConstructor(): any {
            return wijmo.grid.FlexGrid;
        }

        _createWijmoContext(): WjContext {
            return new WjFlexGridContext(this);
        }

        _initialize() {
            super._initialize();
            var formatterDesc = MetaFactory.findProp('itemFormatter', <PropDesc[]>this._metaData.props);
            formatterDesc.updateControl = this._formatterPropHandler;
        }

        private _formatterPropHandler(link: any, propDesc: PropDesc, control: any, unconvertedValue: any, convertedValue: any): boolean {
            if (unconvertedValue !== link._userFormatter) {
                link._userFormatter = unconvertedValue;
                control.invalidate();
            }
            return true;
        }
    }

    export class WjFlexGridContext extends WjContext {
        _wrapperFormatter = this._itemFormatter.bind(this);
        _userFormatter: Function;

        _initControl() {
            super._initControl();
            (<wijmo.grid.FlexGrid>this.control).itemFormatter = this._wrapperFormatter;
        }

        private _itemFormatter(panel, r, c, cell) {
            var column = panel.columns[c],
                cellTemplate = column[wjFlexGrid._columnTemplateProp],
                cellStyle = column[wjFlexGrid._columnStyleProp];
            if ((cellTemplate || cellStyle) && panel.cellType == wijmo.grid.CellType.Cell) {
                // do not format in edit mode
                var editRange: wijmo.grid.CellRange = panel.grid.editRange;
                if (editRange && editRange.row === r && editRange.col === c) {
                    return;
                }
                // no templates in GroupRows
                if (panel.rows[r] instanceof wijmo.grid.GroupRow) {
                    return;
                }

                var cellVM = cell[wjFlexGrid._cellVMProp],
                    clonedTempl = cell[wjFlexGrid._cellClonedTemplateProp],
                    item = panel.rows[r].dataItem;
                if (!cellVM) {
                    cellVM = {
                        $row: ko.observable(r),
                        $col: ko.observable(c),
                        $item: ko.observable(item)
                    };
                    var cellContext = this.bindingContext.extend(cellVM);
                    if (cellTemplate) {
                        cell.innerHTML = '<div>' + cellTemplate + '</div>';
                        var childEl = cell.childNodes[0];
                        cell[wjFlexGrid._cellClonedTemplateProp] = childEl;
                    }
                    else {
                        cell.setAttribute('data-bind', 'style:' + cellStyle);
                    }
                    cell[wjFlexGrid._cellVMProp] = cellVM;
                    ko.applyBindings(cellContext, cell);
                }
                else {
                    if (clonedTempl) {
                        cell.innerHTML = '';
                        cell.appendChild(clonedTempl);
                    }
                    cellVM.$row(r);
                    cellVM.$col(c);
                    if (cellVM.$item() != item) {
                        cellVM.$item(item);
                    }
                    else {
                        cellVM.$item.valueHasMutated();
                    }
                }
                //Enlarge rows height if cell doesn't fit in the current row height.
                var cellHeight = cell.scrollHeight;
                if (panel.rows[r].renderHeight < cellHeight) {
                    panel.rows.defaultSize = cellHeight;
                }

            }
            else if (this._userFormatter) {
                this._userFormatter(panel, r, c, cell);
            } 
        }
    }

    /**
     * KnockoutJS binding for the @see:FlexGrid @see:Column object.
     *
     * The @see:wjFlexGridColumn binding must be contained in a @see:wjFlexGrid binding. For example:
     * 
     * <pre>&lt;p&gt;Here is a FlexGrid control:&lt;/p&gt;
     * &lt;div data-bind="wjFlexGrid: { itemsSource: data }"&gt;
     *     &lt;div data-bind="wjFlexGridColumn: { 
     *         header: 'Country', 
     *         binding: 'country', 
     *         width: '*' }"&gt;
     *     &lt;/div&gt;
     *     &lt;div data-bind="wjFlexGridColumn: { 
     *         header: 'Date', 
     *         binding: 'date' }"&gt;
     *     &lt;/div&gt;
     *     &lt;div data-bind="wjFlexGridColumn: { 
     *         header: 'Revenue', 
     *         binding: 'amount', 
     *         format: 'n0' }"&gt;
     *     &lt;/div&gt;
     *     &lt;div data-bind="wjFlexGridColumn: { 
     *         header: 'Active', 
     *         binding: 'active' }"&gt;
     *     &lt;/div&gt;
     * &lt;/div&gt;</pre>
     *
     * The <b>wjFlexGridColumn</b> binding supports all read-write properties and events of 
     * the @see:Column class. The <b>isSelected</b> property provides two-way binding mode.
     *
     * In addition to regular attributes that match properties in the <b>Column</b> class,  
     * an element with the @see:wjFlexGridColumn binding may contain a @see:wjStyle binding that
     * provides conditional formatting and an HTML fragment that is used as a cell template. Grid 
     * rows automatically stretch vertically to fit custom cell contents.
     *
     * Both the <b>wjStyle</b> binding and the HTML fragment can use the <b>$item</b> observable variable in 
     * KnockoutJS bindings to refer to the item that is bound to the current row. Also available are the 
     * <b>$row</b> and <b>$col</b> observable variables containing cell row and column indexes. For example:
     *
     * <pre>&lt;div data-bind="wjFlexGridColumn: { 
     *         header: 'Symbol', 
     *         binding: 'symbol', 
     *         readOnly: true, 
     *         width: '*' }"&gt;
     *   &lt;a data-bind="attr: { 
     *         href: 'https://finance.yahoo.com/q?s=' + $item().symbol() }, 
     *         text: $item().symbol"&gt;
     *   &lt;/a&gt;
     * &lt;/div&gt;
     * &lt;div data-bind="wjFlexGridColumn: { 
     *      header: 'Change',
     *         binding: 'changePercent',
     *         format: 'p2',
     *         width: '*'
     *         },
     *         wjStyle: { 
     *         color: getAmountColor($item().change) }"&gt;
     * &lt;/div&gt;</pre>
     *
     * These bindings create two columns. 
     * The first has a template that produces a hyperlink based on the bound item's "symbol" property. 
     * The second has a conditional style that renders values with a color determined by a function
     * implemented in the controller.
     */
    export class wjFlexGridColumn extends WjBinding {

        _getControlConstructor(): any {
            return wijmo.grid.Column;
        }

        _createControl(element: any): any {
            return new wijmo.grid.Column();
        }

        _createWijmoContext(): WjContext {
            return new WjFlexGridColumnContext(this);
        }

    }
    // FlexGrid Column context, contains specific code to add column to the parent grid.
    export class WjFlexGridColumnContext extends WjContext {
        _initControl() {
            var gridContext = this.parentWjContext;
            if (gridContext) {
                var grid: wijmo.grid.FlexGrid = <wijmo.grid.FlexGrid>gridContext.control;
                // Turn off autoGenerateColumns and clear the columns collection before initializing this column.
                if (grid.autoGenerateColumns) {
                    grid.autoGenerateColumns = false;
                    grid.columns.clear();
                }
            }
            super._initControl();
            // Store child content in the Column and clear it.
            this.control[wjFlexGrid._columnTemplateProp] = this.element.innerHTML.trim();
            var wjStyleBind = this.allBindings.get(wjFlexGrid._columnStyleBinding);
            if (wjStyleBind) {
                this.control[wjFlexGrid._columnStyleProp] = wjStyleBind.trim();
            }
            this.element.innerHTML = '';
        }
    }

    /**
     * KnockoutJS binding for conditional formatting of @see:FlexGrid @see:Column cells.
     *
     * Use the @see:wjStyle binding together with the @see:wjFlexGridColumn binding to provide
     * conditional formatting to column cells. 
     * For example:
     * 
     * <pre>&lt;div data-bind="wjFlexGridColumn: { 
     *         header: 'Change',
     *         binding: 'changePercent',
     *         format: 'p2',
     *         width: '*'
     *         },
     *         wjStyle: { color: getAmountColor($item().change) }"&gt;&lt;/div&gt;</pre>
     *
     *
     * The <b>wjStyle</b> uses the same syntax as the native KnockoutJS 
     * <a href="http://knockoutjs.com/documentation/style-binding.html" target="_blank">style</a> binding.
     * In addition to the view model properties, the following observable variables are available in binding
     * expressions:
     *
     * <dl class="dl-horizontal">
     *   <dt>$item</dt>  <dd>References the item that is bound to the current row.</dd>
     *   <dt>$row</dt>  <dd>The row index.</dd>
     *   <dt>$col</dt>  <dd>The column index.</dd>
     * </dl>
     */
    export class wjStyle {

        preprocess = function (value: string, name: string, addBinding: (name: string, value: string) => string) {
            return wjStyle.quoteString(value);
        }

        init = function () {
        }

        static quoteString(s: string): string {
            if (s == null) {
                return s;
            }
            return "'" + s.replace(/'/g, "\\'") + "'";
        }

        static unquoteString(s: string): string {
            if (!s || s.length < 2) {
                return s;
            }
            if (s.charAt(0) === "'") {
                s = s.substr(1, s.length - 1);
            }
            return s.replace(/\\\'/g, "'");
        }

    }

    /**
     * KnockoutJS binding for the @see:FlexGrid @see:FlexGridFilter object.
     *
     * The @see:wjFlexGridFilter binding must be contained in a @see:wjFlexGrid binding. For example:
     * 
     * <pre>&lt;p&gt;Here is a FlexGrid control with column filters:&lt;/p&gt;
     * &lt;div data-bind="wjFlexGrid: { itemsSource: data }"&gt;
     *     &lt;div data-bind="wjFlexGridFilter: { filterColumns: ['country', 'amount']  }"&gt;&lt;/div&gt;
     * &nbsp;
     *     &lt;div data-bind="wjFlexGridColumn: { 
     *         header: 'Country', 
     *         binding: 'country', 
     *         width: '*' }"&gt;
     *     &lt;/div&gt;
     *     &lt;div data-bind="wjFlexGridColumn: { 
     *         header: 'Date', 
     *         binding: 'date' }"&gt;
     *     &lt;/div&gt;
     *     &lt;div data-bind="wjFlexGridColumn: { 
     *         header: 'Revenue', 
     *         binding: 'amount', 
     *         format: 'n0' }"&gt;
     *     &lt;/div&gt;
     *     &lt;div data-bind="wjFlexGridColumn: { 
     *         header: 'Active', 
     *         binding: 'active' }"&gt;
     *     &lt;/div&gt;
     * &lt;/div&gt;</pre>
     *
     * The <b>wjFlexGridFilter</b> binding supports all read-write properties and events of 
     * the @see:FlexGridFilter class. 
     *
     */
    export class wjFlexGridFilter extends WjBinding {

        _getControlConstructor(): any {
            return wijmo.grid.filter.FlexGridFilter;
        }

    }

    /**
     * KnockoutJS binding for the @see:FlexGrid @see:GroupPanel control.
     *
     * The <b>wjGroupPanel</b> binding should be connected to the <b>FlexGrid</b> control using the <b>grid</b> property. 
     * For example:
     * 
     * <pre>&lt;p&gt;Here is a FlexGrid control with GroupPanel:&lt;/p&gt;
     * &nbsp;
     * &lt;div data-bind="wjGroupPanel: { grid: flex(), placeholder: 'Drag columns here to create groups.' }"&gt;&lt;/div&gt;
     * &nbsp;
     * &lt;div data-bind="wjFlexGrid: { control: flex, itemsSource: data }"&gt;
     *     &lt;div data-bind="wjFlexGridColumn: { 
     *         header: 'Country', 
     *         binding: 'country', 
     *         width: '*' }"&gt;
     *     &lt;/div&gt;
     *     &lt;div data-bind="wjFlexGridColumn: { 
     *         header: 'Date', 
     *         binding: 'date' }"&gt;
     *     &lt;/div&gt;
     *     &lt;div data-bind="wjFlexGridColumn: { 
     *         header: 'Revenue', 
     *         binding: 'amount', 
     *         format: 'n0' }"&gt;
     *     &lt;/div&gt;
     *     &lt;div data-bind="wjFlexGridColumn: { 
     *         header: 'Active', 
     *         binding: 'active' }"&gt;
     *     &lt;/div&gt;
     * &lt;/div&gt;</pre>
     *
     * The <b>wjGroupPanel</b> binding supports all read-write properties and events of 
     * the @see:GroupPanel class. 
     *
     */
    export class wjGroupPanel extends WjBinding {

        _getControlConstructor(): any {
            return wijmo.grid.grouppanel.GroupPanel;
        }

    }

} 

// Register bindings
(<any>(ko.bindingHandlers))[wijmo.knockout.wjFlexGrid._columnStyleBinding] = new wijmo.knockout.wjStyle();
(<any>(ko.bindingHandlers)).wjFlexGrid = new wijmo.knockout.wjFlexGrid();
(<any>(ko.bindingHandlers)).wjFlexGridColumn = new wijmo.knockout.wjFlexGridColumn();
(<any>(ko.bindingHandlers)).wjFlexGridFilter = new wijmo.knockout.wjFlexGridFilter();
(<any>(ko.bindingHandlers)).wjGroupPanel = new wijmo.knockout.wjGroupPanel();

module wijmo.knockout {
    // Base abstract class for specific Chart type bindings
    export class WjFlexChartBaseBinding extends WjBinding {
        _getControlConstructor(): any {
            return wijmo.chart.FlexChartBase;
        }

        _initialize() {
            super._initialize();
            var tooltipDesc = MetaFactory.findProp('tooltipContent', <PropDesc[]>this._metaData.props);
            tooltipDesc.updateControl = function (link, propDesc, control, unconvertedValue, convertedValue): boolean {
                if (convertedValue != null) {
                    (<wijmo.chart.FlexChart>control).tooltip.content = convertedValue;
                }
                return true;
            };
        }
    }

    /**
     * KnockoutJS binding for the @see:FlexChart control.
     *
     * Use the @see:wjFlexChart binding to add @see:FlexChart controls to your 
     * KnockoutJS applications. For example:
     * 
     * <pre>&lt;p&gt;Here is a FlexChart control:&lt;/p&gt;
     * &lt;div data-bind="wjFlexChart: { itemsSource: data }"&gt;
     *     &lt;div data-bind="wjFlexChartLegend : { 
     *         position: 'Top' }"&gt;
     *     &lt;/div&gt;
     *     &lt;div data-bind="wjFlexChartAxis: { 
     *         wjProperty: 'axisX', 
     *         title: chartProps.titleX }"&gt;
     *     &lt;/div&gt;
     *     &lt;div data-bind="wjFlexChartAxis: { 
     *         wjProperty: 'axisY', 
     *         majorUnit: 5000 }"&gt;
     *     &lt;/div&gt;
     *     &lt;div data-bind="wjFlexChartSeries: { 
     *         name: 'Sales', 
     *         binding: 'sales' }"&gt;
     *     &lt;/div&gt;
     *     &lt;div data-bind="wjFlexChartSeries: { 
     *         name: 'Expenses', 
     *         binding: 'expenses' }"&gt;
     *     &lt;/div&gt;
     *     &lt;div data-bind="wjFlexChartSeries: { 
     *         name: 'Downloads', 
     *         binding: 'downloads', 
     *         chartType: 'LineSymbols' }"&gt;
     *     &lt;/div&gt;
     * &lt;/div&gt;</pre>
     * 
     * The <b>wjFlexChart</b> binding may contain the following child bindings: 
     * @see:wjFlexChartAxis, @see:wjFlexChartSeries, @see:wjFlexChartLegend.
     *
     * The <b>wjFlexChart</b> binding supports all read-write properties and events of 
     * the @see:FlexChart control, and the additional <b>tooltipContent</b> property
     * that assigns a value to the <b>FlexChart.tooltip.content</b> property.
     * The <b>selection</b> property provides two-way binding mode.
     */
    export class wjFlexChart extends WjFlexChartBaseBinding {
        _getControlConstructor(): any {
            return wijmo.chart.FlexChart;
        }

        _initialize() {
            super._initialize();

            var lblContentDesc = MetaFactory.findProp('labelContent', <PropDesc[]>this._metaData.props);
            lblContentDesc.updateControl = function (link, propDesc, control, unconvertedValue, convertedValue): boolean {
                if (convertedValue != null) {
                    (<wijmo.chart.FlexChart>control).dataLabel.content = convertedValue;
                }
                return true;
            };
        }
    }

    /**
     * KnockoutJS binding for the @see:FlexPie control.
     *
     * Use the @see:wjFlexPie binding to add @see:FlexPie controls to your 
     * KnockoutJS applications. For example:
     * 
     * <pre>&lt;p&gt;Here is a FlexPie control:&lt;/p&gt;
     * &lt;div data-bind="wjFlexPie: {
     *         itemsSource: data,
     *         binding: 'value',
     *         bindingName: 'name',
     *         header: 'Fruit By Value' }"&gt;
     *     &lt;div data-bind="wjFlexChartLegend : { position: 'Top' }"&gt;&lt;/div&gt;
     * &lt;/div&gt;</pre>
     * 
     * The <b>wjFlexPie</b> binding may contain the @see:wjFlexChartLegend child binding. 
     *
     * The <b>wjFlexPie</b> binding supports all read-write properties and events of 
     * the @see:FlexPie control.
     */
    export class wjFlexPie extends WjFlexChartBaseBinding {
        _getControlConstructor(): any {
            return wijmo.chart.FlexPie;
        }

        _initialize() {
            super._initialize();

            var lblContentDesc = MetaFactory.findProp('labelContent', <PropDesc[]>this._metaData.props);
            lblContentDesc.updateControl = function (link, propDesc, control, unconvertedValue, convertedValue): boolean {
                if (convertedValue != null) {
                    (<wijmo.chart.FlexPie>control).dataLabel.content = convertedValue;
                }
                return true;
            };
        }
    }

    /**
     * KnockoutJS binding for the @see:FlexChart @see:Axis object.
     *
     * The @see:wjFlexChartAxis binding must be contained in a @see:wjFlexChart binding. Use the <b>wjProperty</b>
     * attribute to specify the property (<b>axisX</b> or <b>axisY</b>) to initialize with this binding.
     * 
     * The <b>wjFlexChartAxis</b> binding supports all read-write properties and events of 
     * the @see:Axis class.
     */
    export class wjFlexChartAxis extends WjBinding {
        _getControlConstructor(): any {
            return wijmo.chart.Axis;
        }
    }

    /**
     * KnockoutJS binding for the Charts' @see:Legend object.
     *
     * The @see:wjFlexChartLegend binding must be contained in one the following bindings:
     *  @see:wjFlexChart, @see:wjFlexPie. 
     * 
     * The <b>wjFlexChartLegend</b> binding supports all read-write properties and events of 
     * the @see:Legend class.
     */
    export class wjFlexChartLegend extends WjBinding {
        _getControlConstructor(): any {
            return wijmo.chart.Legend;
        }
    }

    /**
     * KnockoutJS binding for the @see:FlexChart @see:Series object.
     *
     * The @see:wjFlexChartSeries binding must be contained in a @see:wjFlexChart binding. 
     * 
     * The <b>wjFlexChartSeries</b> binding supports all read-write properties and events of 
     * the @see:Series class. The <b>visibility</b> property provides two-way binding mode.
     */
    export class wjFlexChartSeries extends WjBinding {
        _getControlConstructor(): any {
            return wijmo.chart.Series;
        }

        _createWijmoContext(): WjContext {
            return new WjFlexChartSeriesContext(this);
        }
    }

    export class WjFlexChartSeriesContext extends WjContext {
        _initControl() {
            super._initControl();
            //Update bindings to the visibility property on parent Chart seriesVisibilityChanged event.
            var parentCtrl = this.parentWjContext.control;
            if (parentCtrl instanceof wijmo.chart.FlexChart) {
                (<wijmo.chart.FlexChart>parentCtrl).seriesVisibilityChanged.addHandler((s, e) => {
                    this._updateSource();
                });
            }
        }
    }

} 

// Register bindings
(<any>(ko.bindingHandlers)).wjFlexChart = new wijmo.knockout.wjFlexChart();
(<any>(ko.bindingHandlers)).wjFlexPie = new wijmo.knockout.wjFlexPie();
(<any>(ko.bindingHandlers)).wjFlexChartAxis = new wijmo.knockout.wjFlexChartAxis();
(<any>(ko.bindingHandlers)).wjFlexChartLegend = new wijmo.knockout.wjFlexChartLegend();
(<any>(ko.bindingHandlers)).wjFlexChartSeries = new wijmo.knockout.wjFlexChartSeries();

module wijmo.knockout {

    // Gauge control binding
    // Provides base setup for all bindings related to controls derived from Gauge
    // Abstract class, not for use in markup
    export class WjGaugeBinding extends WjBinding {
        _getControlConstructor(): any {
            return wijmo.gauge.Gauge;
        }
    }

    /**
     * KnockoutJS binding for the @see:LinearGauge control.
     *
     * Use the @see:wjLinearGauge binding to add @see:LinearGauge controls to your 
     * KnockoutJS applications. For example:
     * 
     * <pre>&lt;p&gt;Here is a LinearGauge control:&lt;/p&gt;
     * &lt;div data-bind="wjLinearGauge: {
     *         value: props.value,
     *         min: props.min,
     *         max: props.max,
     *         format: props.format,
     *         showRanges: props.showRanges }"
     *         &lt;class="linear-gauge"&gt;
     *     &lt;div data-bind="wjRange: { 
     *             wjProperty: 'pointer', 
     *             thickness: props.ranges.pointerThickness }"&gt;
     *     &lt;/div&gt;
     *     &lt;div data-bind="wjRange: { 
     *             min: props.ranges.lower.min, 
     *             max: props.ranges.lower.max, 
     *             color: props.ranges.lower.color }"&gt;
     *     &lt;/div&gt;
     *     &lt;div data-bind="wjRange: { 
     *             min: props.ranges.middle.min, 
     *             max: props.ranges.middle.max, 
     *             color: props.ranges.middle.color }"&gt;
     *     &lt;/div&gt;
     *     &lt;div data-bind="wjRange: { 
     *             min: props.ranges.upper.min, 
     *             max: props.ranges.upper.max, 
     *             color: props.ranges.upper.color }"&gt;
     *     &lt;/div&gt;
     * &lt;/div&gt;</pre>
     * 
     * The <b>wjLinearGauge</b> binding may contain the @see:wjRange child binding. 
     *
     * The <b>wjLinearGauge</b> binding supports all read-write properties and events of 
     * the @see:LinearGauge control. The <b>value</b> property provides two-way binding mode.
     */
    export class wjLinearGauge extends WjGaugeBinding {
        _getControlConstructor(): any {
            return wijmo.gauge.LinearGauge;
        }
    }

    /**
     * KnockoutJS binding for the @see:BulletGraph control.
     *
     * Use the @see:wjBulletGraph binding to add @see:BulletGraph controls to your 
     * KnockoutJS applications. For example:
     * 
     * <pre>&lt;p&gt;Here is a BulletGraph control:&lt;/p&gt;
     * &lt;div data-bind="wjBulletGraph: {
     *         value: props.value,
     *         min: props.min,
     *         max: props.max,
     *         format: props.format,
     *         good: props.ranges.middle.max,
     *         bad: props.ranges.middle.min,
     *         target: props.ranges.target,
     *         showRanges: props.showRanges }"
     *         class="linear-gauge"&gt;
     *     &lt;div data-bind="wjRange: { 
     *             wjProperty: 'pointer', 
     *             thickness: props.ranges.pointerThickness }"&gt;
     *     &lt;/div&gt;
     * &lt;/div&gt;</pre>
     * 
     * The <b>wjBulletGraph</b> binding may contain the @see:wjRange child binding. 
     *
     * The <b>wjBulletGraph</b> binding supports all read-write properties and events of 
     * the @see:BulletGraph control. The <b>value</b> property provides two-way binding mode.
     */
    export class wjBulletGraph extends wjLinearGauge {
        _getControlConstructor(): any {
            return wijmo.gauge.BulletGraph;
        }
    }

    /**
     * KnockoutJS binding for the @see:RadialGauge control.
     *
     * Use the @see:wjRadialGauge binding to add @see:RadialGauge controls to your 
     * KnockoutJS applications. For example:
     * 
     * <pre>&lt;p&gt;Here is a RadialGauge control:&lt;/p&gt;
     * &lt;div data-bind="wjRadialGauge: {
     *         value: props.value,
     *         min: props.min,
     *         max: props.max,
     *         format: props.format,
     *         showRanges: props.showRanges }"
     *         class="radial-gauge"&gt;
     *     &lt;div data-bind="wjRange: { 
     *             wjProperty: 'pointer', 
     *             thickness: props.ranges.pointerThickness }"&gt;
     *     &lt;/div&gt;
     *     &lt;div data-bind="wjRange: { 
     *             min: props.ranges.lower.min, 
     *             max: props.ranges.lower.max, 
     *             color: props.ranges.lower.color }"&gt;
     *     &lt;/div&gt;
     *     &lt;div data-bind="wjRange: { 
     *             min: props.ranges.middle.min, 
     *             max: props.ranges.middle.max, 
     *             color: props.ranges.middle.color }"&gt;
     *     &lt;/div&gt;
     *     &lt;div data-bind="wjRange: { 
     *             min: props.ranges.upper.min, 
     *             max: props.ranges.upper.max, 
     *             color: props.ranges.upper.color }"&gt;
     *     &lt;/div&gt;
     * &lt;/div&gt;</pre>
     * 
     * The <b>wjRadialGauge</b> binding may contain the @see:wjRange child binding. 
     *
     * The <b>wjRadialGauge</b> binding supports all read-write properties and events of 
     * the @see:RadialGauge control. The <b>value</b> property provides two-way binding mode.
     */
    export class wjRadialGauge extends WjGaugeBinding {
        _getControlConstructor(): any {
            return wijmo.gauge.RadialGauge;
        }
    }

    /**
     * KnockoutJS binding for the Gauge's @see:Range object.
     *
     * The @see:wjRange binding must be contained in one of the following bindings:
     * <ul>
     *     <li>@see:wjLinearGauge</li> 
     *     <li>@see:wjRadialGauge</li>
     *     <li>@see:wjBulletGraph</li>
     * </ul> 
     * By default, this binding adds a <b>Range</b> object to the <b>ranges</b> 
     * collection of the Chart control. The <b>wjProperty</b> attribute allows 
     * you to specify another Chart property, for example the <b>pointer</b> 
     * property, to initialize with the binding.
     * 
     * The <b>wjRange</b> binding supports all read-write properties and events of 
     * the @see:Range class.
     */
    export class wjRange extends WjBinding {
        _getControlConstructor(): any {
            return wijmo.gauge.Range;
        }
    }

} 

// Register bindings
(<any>(ko.bindingHandlers)).wjLinearGauge = new wijmo.knockout.wjLinearGauge();
(<any>(ko.bindingHandlers)).wjBulletGraph = new wijmo.knockout.wjBulletGraph();
(<any>(ko.bindingHandlers)).wjRadialGauge = new wijmo.knockout.wjRadialGauge();
(<any>(ko.bindingHandlers)).wjRange = new wijmo.knockout.wjRange();

module wijmo.knockout {
    export class WjTagsPreprocessor {
        private static _specialProps = WjTagsPreprocessor._getSpecialProps();
        private static _getSpecialProps() {
            var ret = {},
                wjBind = wijmo.knockout.WjBinding;
            ret[wjBind._controlPropAttr] = true;
            ret[wjBind._parPropAttr] = true;
            return ret;
        }

        private static _dataBindAttr = 'data-bind';
        private static _wjTagPrefix = 'wj-';

        private _foreignProc;

        register(): void {
            this._foreignProc = ko.bindingProvider.instance['preprocessNode'];
            ko.bindingProvider.instance['preprocessNode'] = this.preprocessNode.bind(this); 
        }

        preprocessNode(node): any {
            var dataBindName = WjTagsPreprocessor._dataBindAttr;
            if (!(node.nodeType == 1 && this._isWjTag(node.tagName))) {
                return this._delegate(node);
            }
            var camelTag = MetaFactory.toCamelCase(node.tagName),
                wjBinding = <wijmo.knockout.WjBinding>ko.bindingHandlers[camelTag];
            if (!wjBinding) {
                return this._delegate(node);
            }
            wjBinding.ensureMetaData();
            var wjBindDef = '',
                attribs = node.attributes,
                retEl = document.createElement("div"),
                dataBindAttr;
            for (var i = 0; i < attribs.length; i++) {
                var attr = attribs[i];
                if (attr.name.toLowerCase() == dataBindName) {
                    dataBindAttr = attr;
                    continue;
                }
                var camelAttr = MetaFactory.toCamelCase(attr.name);
                if (this._isWjProp(camelAttr, wjBinding._metaData)) {
                    if (wjBindDef) {
                        wjBindDef += ',';
                    }
                    wjBindDef += camelAttr + ':' + attr.value;
                }
                else {
                    retEl.setAttribute(attr.name, attr.value);
                }
            }

            wjBindDef = camelTag + ':{' + wjBindDef + '}';
            if (dataBindAttr && dataBindAttr.value && dataBindAttr.value.trim()) {
                wjBindDef += ',' + dataBindAttr.value;
            }

            retEl.setAttribute(dataBindName, wjBindDef);

            while (node.firstChild) {
                retEl.appendChild(node.firstChild);
            }
            node.parentNode.replaceChild(retEl, node);

            return [retEl];
        }

        private _delegate(node) {
            return this._foreignProc ? this._foreignProc(node) : undefined;
        }

        private _isWjTag(name) {
            var wjPfx = WjTagsPreprocessor._wjTagPrefix;
            return name && name.length > wjPfx.length && name.substr(0, wjPfx.length).toLowerCase() === wjPfx;
        }

        private _isWjProp(name, metaData) {
            return WjTagsPreprocessor._specialProps[name] || wijmo.knockout.MetaFactory.findProp(name, metaData.props) ||
                wijmo.knockout.MetaFactory.findEvent(name, metaData.events);
        }

    }
} 

if (!wijmo['disableKnockoutTags']) {
    new wijmo.knockout.WjTagsPreprocessor().register();
}
