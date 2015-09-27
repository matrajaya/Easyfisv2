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
declare module wijmo {
    module interop {
        class ControlMetaFactory {
            static CreateProp(propertyName: string, propertyType: PropertyType, bindingMode?: BindingMode, enumType?: any, isNativeControlProperty?: boolean, priority?: number): PropDescBase;
            static CreateEvent(eventName: string, isPropChanged?: boolean): EventDescBase;
            static CreateComplexProp(propertyName: string, isArray: boolean, ownsObject?: boolean): ComplexPropDescBase;
            static findProp(propName: string, props: PropDescBase[]): PropDescBase;
            static findEvent(eventName: string, events: EventDescBase[]): EventDescBase;
            static findComplexProp(propName: string, props: ComplexPropDescBase[]): ComplexPropDescBase;
            static getMetaData(metaDataId: any): MetaDataBase;
            static getClassName(classRef: any): string;
            static toCamelCase(s: any): any;
            private static findInArr(arr, propName, value);
        }
        class PropDescBase {
            private _propertyName;
            private _propertyType;
            private _enumType;
            private _bindingMode;
            private _isNativeControlProperty;
            private _priority;
            constructor(propertyName: string, propertyType: PropertyType, bindingMode?: BindingMode, enumType?: any, isNativeControlProperty?: boolean, priority?: number);
            public propertyName : string;
            public propertyType : PropertyType;
            public enumType : any;
            public bindingMode : BindingMode;
            public isNativeControlProperty : boolean;
            public priority : number;
            public shouldUpdateSource : boolean;
            public initialize(options: any): void;
        }
        enum PropertyType {
            Boolean = 0,
            Number = 1,
            Date = 2,
            String = 3,
            Enum = 4,
            Function = 5,
            EventHandler = 6,
            Any = 7,
        }
        function isSimpleType(type: PropertyType): boolean;
        enum BindingMode {
            OneWay = 0,
            TwoWay = 1,
        }
        class EventDescBase {
            private _eventName;
            private _isPropChanged;
            constructor(eventName: string, isPropChanged?: boolean);
            public eventName : string;
            public isPropChanged : boolean;
        }
        class ComplexPropDescBase {
            public propertyName: string;
            public isArray: boolean;
            private _ownsObject;
            constructor(propertyName: string, isArray: boolean, ownsObject?: boolean);
            public ownsObject : boolean;
        }
        class MetaDataBase {
            private _props;
            private _events;
            private _complexProps;
            public parentProperty: string;
            public isParentPropertyArray: boolean;
            public ownsObject: boolean;
            public parentReferenceProperty: string;
            public ngModelProperty: string;
            constructor(props: PropDescBase[], events?: EventDescBase[], complexProps?: ComplexPropDescBase[], parentProperty?: string, isParentPropertyArray?: boolean, ownsObject?: boolean, parentReferenceProperty?: string, ngModelProperty?: string);
            public props : PropDescBase[];
            public events : EventDescBase[];
            public complexProps : ComplexPropDescBase[];
            public add(props: PropDescBase[], events?: EventDescBase[], complexProps?: ComplexPropDescBase[], parentProperty?: string, isParentPropertyArray?: boolean, ownsObject?: boolean, parentReferenceProperty?: string, ngModelProperty?: string): MetaDataBase;
            public addOptions(options: any): MetaDataBase;
            public prepare(): void;
        }
    }
}

declare module wijmo.knockout {
    class MetaFactory extends interop.ControlMetaFactory {
        static CreateProp(propertyName: string, propertyType: interop.PropertyType, bindingMode?: interop.BindingMode, enumType?: any, isNativeControlProperty?: boolean, priority?: number): PropDesc;
        static CreateEvent(eventName: string, isPropChanged?: boolean): EventDesc;
        static CreateComplexProp(propertyName: string, isArray: boolean, ownsObject?: boolean): ComplexPropDesc;
        static findProp(propName: string, props: PropDesc[]): PropDesc;
        static findEvent(eventName: string, events: EventDesc[]): EventDesc;
        static findComplexProp(propName: string, props: ComplexPropDesc[]): ComplexPropDesc;
    }
    interface IUpdateControlHandler {
        (link: any, propDesc: PropDesc, control: any, unconvertedValue: any, convertedValue: any): boolean;
    }
    class PropDesc extends interop.PropDescBase {
        public updateControl: IUpdateControlHandler;
    }
    class EventDesc extends interop.EventDescBase {
    }
    class ComplexPropDesc extends interop.ComplexPropDescBase {
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
declare module wijmo.knockout {
    class WjBinding implements KnockoutBindingHandler {
        static _wjContextProp: string;
        static _parPropAttr: string;
        static _controlPropAttr: string;
        static _initPropAttr: string;
        static _initEventAttr: string;
        public _metaData: interop.MetaDataBase;
        public init: any;
        public update: any;
        public ensureMetaData(): void;
        public _initialize(): void;
        public _getControlConstructor(): any;
        public _getMetaDataId(): any;
        public _createControl(element: any): any;
        public _createWijmoContext(): WjContext;
        public _isChild(): boolean;
        public _isParentInitializer(): boolean;
        public _isParentReferencer(): boolean;
        private _initImpl(element, valueAccessor, allBindings, viewModel, bindingContext);
        private _updateImpl;
    }
    class WjContext {
        public element: any;
        public valueAccessor: any;
        public allBindings: any;
        public viewModel: any;
        public bindingContext: any;
        public control: any;
        public wjBinding: WjBinding;
        public parentWjContext: WjContext;
        private _parentPropDesc;
        private _isInitialized;
        private static _debugId;
        constructor(wjBinding: WjBinding);
        public init(element: any, valueAccessor: () => any, allBindings: KnockoutAllBindingsAccessor, viewModel: any, bindingContext: KnockoutBindingContext): any;
        public update(element: any, valueAccessor: () => any, allBindings: KnockoutAllBindingsAccessor, viewModel: any, bindingContext: KnockoutBindingContext): void;
        public _createControl(): any;
        public _initControl(): void;
        public _childrenInitialized(): void;
        private _addEventHandler(eventDesc);
        private static _isUpdatingSource;
        private static _pendingSourceUpdates;
        public _updateSource(): void;
        private _isUpdatingControl;
        private _isSourceDirty;
        private _oldSourceValues;
        private _updateControl(valueAccessor?);
        private _castValueToType(value, prop);
        private _parseDate(value);
        public _safeUpdateSrcAttr(attrName: string, value: any): void;
        public _safeNotifySrcAttr(attrName: string): void;
        private _isChild();
        private _isParentInitializer();
        private _isParentReferencer();
        private _getParentProp();
        private _getParentReferenceProperty();
        private _useParentObj();
        private _isParentArray();
        private _parentInCtor();
    }
}

declare module wijmo.knockout {
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
    class wjTooltip extends WjBinding {
        public _getControlConstructor(): any;
        public _createWijmoContext(): WjContext;
    }
    class WjTooltipContext extends WjContext {
        public update(element: any, valueAccessor: () => any, allBindings: KnockoutAllBindingsAccessor, viewModel: any, bindingContext: KnockoutBindingContext): void;
        private _updateTooltip();
    }
}

declare module wijmo.knockout {
    class WjDropDownBinding extends WjBinding {
        public _getControlConstructor(): any;
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
    class wjComboBox extends WjDropDownBinding {
        public _getControlConstructor(): any;
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
    class wjAutoComplete extends wjComboBox {
        public _getControlConstructor(): any;
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
    class wjCalendar extends WjBinding {
        public _getControlConstructor(): any;
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
    class wjColorPicker extends WjBinding {
        public _getControlConstructor(): any;
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
    class wjListBox extends WjBinding {
        public _getControlConstructor(): any;
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
    class wjMenu extends wjComboBox {
        public _getControlConstructor(): any;
        public _createWijmoContext(): WjContext;
        public _initialize(): void;
        private _updateControlValue(link, propDesc, control, unconvertedValue, convertedValue);
    }
    class WjMenuContext extends WjContext {
        public _initControl(): void;
        public _childrenInitialized(): void;
        public _updateHeader(): void;
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
    class wjMenuItem extends WjBinding {
        public _getMetaDataId(): any;
        public _createWijmoContext(): WjContext;
        public _initialize(): void;
    }
    class WjMenuItemContext extends WjContext {
        public _createControl(): any;
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
    class wjMenuSeparator extends WjBinding {
        public _getMetaDataId(): any;
        public _initialize(): void;
        public _createControl(element: any): any;
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
    class wjInputDate extends WjDropDownBinding {
        public _getControlConstructor(): any;
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
    class wjInputNumber extends WjBinding {
        public _getControlConstructor(): any;
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
    class wjInputMask extends WjBinding {
        public _getControlConstructor(): any;
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
    class wjInputTime extends wjComboBox {
        public _getControlConstructor(): any;
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
    class wjInputColor extends WjDropDownBinding {
        public _getControlConstructor(): any;
    }
    class WjCollectionViewBaseBinding extends WjBinding {
        public _createControl(element: any): any;
        public _createWijmoContext(): WjContext;
        public _getTemplate(): string;
    }
    class WjCollectionViewContext extends WjContext {
        private _localVM;
        private _emptyCV;
        public init(element: any, valueAccessor: () => any, allBindings: KnockoutAllBindingsAccessor, viewModel: any, bindingContext: KnockoutBindingContext): any;
        public update(element: any, valueAccessor: () => any, allBindings: KnockoutAllBindingsAccessor, viewModel: any, bindingContext: KnockoutBindingContext): void;
        private _subscribeToCV(cv);
        private _unsubscribeFromCV(cv);
        private _forceBindingsUpdate(s, e);
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
    class wjCollectionViewPager extends WjCollectionViewBaseBinding {
        public _getMetaDataId(): any;
        public _getTemplate(): string;
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
    class wjCollectionViewNavigator extends WjCollectionViewBaseBinding {
        public _getMetaDataId(): any;
        public _getTemplate(): string;
    }
}

declare module wijmo.knockout {
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
    class wjFlexGrid extends WjBinding {
        static _columnTemplateProp: string;
        static _cellClonedTemplateProp: string;
        static _cellVMProp: string;
        static _columnStyleBinding: string;
        static _columnStyleProp: string;
        public _getControlConstructor(): any;
        public _createWijmoContext(): WjContext;
        public _initialize(): void;
        private _formatterPropHandler(link, propDesc, control, unconvertedValue, convertedValue);
    }
    class WjFlexGridContext extends WjContext {
        public _wrapperFormatter: any;
        public _userFormatter: Function;
        public _initControl(): void;
        private _itemFormatter(panel, r, c, cell);
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
    class wjFlexGridColumn extends WjBinding {
        public _getControlConstructor(): any;
        public _createControl(element: any): any;
        public _createWijmoContext(): WjContext;
    }
    class WjFlexGridColumnContext extends WjContext {
        public _initControl(): void;
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
    class wjStyle {
        public preprocess: (value: string, name: string, addBinding: (name: string, value: string) => string) => string;
        public init: () => void;
        static quoteString(s: string): string;
        static unquoteString(s: string): string;
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
    class wjFlexGridFilter extends WjBinding {
        public _getControlConstructor(): any;
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
    class wjGroupPanel extends WjBinding {
        public _getControlConstructor(): any;
    }
}

declare module wijmo.knockout {
    class WjFlexChartBaseBinding extends WjBinding {
        public _getControlConstructor(): any;
        public _initialize(): void;
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
    class wjFlexChart extends WjFlexChartBaseBinding {
        public _getControlConstructor(): any;
        public _initialize(): void;
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
    class wjFlexPie extends WjFlexChartBaseBinding {
        public _getControlConstructor(): any;
        public _initialize(): void;
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
    class wjFlexChartAxis extends WjBinding {
        public _getControlConstructor(): any;
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
    class wjFlexChartLegend extends WjBinding {
        public _getControlConstructor(): any;
    }
    /**
    * KnockoutJS binding for the @see:FlexChart @see:Series object.
    *
    * The @see:wjFlexChartSeries binding must be contained in a @see:wjFlexChart binding.
    *
    * The <b>wjFlexChartSeries</b> binding supports all read-write properties and events of
    * the @see:Series class. The <b>visibility</b> property provides two-way binding mode.
    */
    class wjFlexChartSeries extends WjBinding {
        public _getControlConstructor(): any;
        public _createWijmoContext(): WjContext;
    }
    class WjFlexChartSeriesContext extends WjContext {
        public _initControl(): void;
    }
}

declare module wijmo.knockout {
    class WjGaugeBinding extends WjBinding {
        public _getControlConstructor(): any;
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
    class wjLinearGauge extends WjGaugeBinding {
        public _getControlConstructor(): any;
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
    class wjBulletGraph extends wjLinearGauge {
        public _getControlConstructor(): any;
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
    class wjRadialGauge extends WjGaugeBinding {
        public _getControlConstructor(): any;
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
    class wjRange extends WjBinding {
        public _getControlConstructor(): any;
    }
}

declare module wijmo.knockout {
    class WjTagsPreprocessor {
        private static _specialProps;
        private static _getSpecialProps();
        private static _dataBindAttr;
        private static _wjTagPrefix;
        private _foreignProc;
        public register(): void;
        public preprocessNode(node: any): any;
        private _delegate(node);
        private _isWjTag(name);
        private _isWjProp(name, metaData);
    }
}

