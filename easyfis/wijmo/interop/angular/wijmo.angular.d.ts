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

declare module wijmo.angular {
    class MetaFactory extends interop.ControlMetaFactory {
        static CreateProp(propertyName: string, propertyType: interop.PropertyType, bindingMode?: interop.BindingMode, enumType?: any, isNativeControlProperty?: boolean, priority?: number): PropDesc;
        static CreateEvent(eventName: string, isPropChanged?: boolean): EventDesc;
        static CreateComplexProp(propertyName: string, isArray: boolean, ownsObject?: boolean): ComplexPropDesc;
        static findProp(propName: string, props: PropDesc[]): PropDesc;
        static findEvent(eventName: string, events: EventDesc[]): EventDesc;
        static findComplexProp(propName: string, props: ComplexPropDesc[]): ComplexPropDesc;
    }
    class PropDesc extends interop.PropDescBase {
        private _scopeBindingMode;
        private _customHandler;
        constructor(propertyName: string, propertyType: interop.PropertyType, bindingMode?: interop.BindingMode, enumType?: any, isNativeControlProperty?: boolean, priority?: number);
        public scopeBindingMode : string;
        public customHandler : (scope: ng.IScope, control: Control, value: any, oldValue: any, link: WjLink) => void;
    }
    class EventDesc extends interop.EventDescBase {
    }
    class ComplexPropDesc extends interop.ComplexPropDescBase {
    }
}

declare module wijmo.angular {
    class WjDirective implements ng.IDirective {
        static _parPropAttr: string;
        static _wjModelPropAttr: string;
        static _initPropAttr: string;
        static _initEventAttr: string;
        static _cntrlScopeProp: string;
        static _cntrlLinkProp: string;
        static _scopeChildrenProp: string;
        static _dirIdAttr: string;
        static _optionalAttr: boolean;
        static _dynaTemplates: boolean;
        static _angStripPrefixes: string[];
        private static _dirIdCounter;
        public link: (scope: ng.IScope, templateElement: ng.IAugmentedJQuery, templateAttributes: ng.IAttributes, controller: any) => any;
        public controller: any;
        public replace: boolean;
        public require: any;
        public restrict: string;
        public scope: any;
        public template: any;
        public transclude: any;
        public _property: string;
        public _isPropertyArray: boolean;
        public _ownObject: boolean;
        public _parentReferenceProperty: string;
        public _ngModelProperty: string;
        public _isCustomParentInit: boolean;
        public _props: PropDesc[];
        public _events: EventDesc[];
        public _complexProps: ComplexPropDesc[];
        public _$parse: any;
        private _stripReq;
        private _dirId;
        public _controlConstructor : any;
        public _getMetaDataId(): any;
        public _getMetaData(): interop.MetaDataBase;
        constructor();
        private _initDirective();
        public _initSharedMeta(): void;
        public _initProps(): void;
        public _initEvents(): void;
        public _createLink(): WjLink;
        public _controllerImpl(controller: any, scope: any, tElement: any): void;
        public _initControl(element: any): any;
        public _isChild(): boolean;
        public _isParentInitializer(): boolean;
        public _isParentReferencer(): boolean;
        public _scopeToAttrName(scopeName: string): string;
        public _getComplexPropDesc(propName: string): ComplexPropDesc;
        private _initScopeEvents();
        private _initScopeDescription();
        public _postLinkFn(): (scope: any, tElement: ng.IAugmentedJQuery, tAttrs: ng.IAttributes, controller?: any) => void;
        private _prepareProps();
        private _stripRequire(index);
        public _getId(): string;
        static _versionOk(minVer: string): boolean;
    }
    class WjLink {
        public directive: WjDirective;
        public scope: ng.IScope;
        public tElement: ng.IAugmentedJQuery;
        public tAttrs: ng.IAttributes;
        public controller: any;
        public directiveTemplateElement: JQuery;
        public control: any;
        public parent: WjLink;
        public ngModel: ng.INgModelController;
        private _ngModelPropDesc;
        private _nonAssignable;
        private _parentPropDesc;
        private _definedProps;
        private _definedEvents;
        private _oldValues;
        public _isInitialized: boolean;
        private _scopeSuspend;
        private _suspendedEvents;
        private _siblingInsertedEH;
        public _areChlildrenReady: boolean;
        constructor();
        public _link(): void;
        public _onChildrenReady(): void;
        private _createInstance();
        private _parentReady(parentLink);
        public _initParent(): void;
        public _destroy(): void;
        private _siblingInserted(e);
        private _notifyReady();
        public _initControl(): any;
        private _prepareControl();
        private _setupScopeWithControlProperties();
        private _initNonAssignable();
        public _suspendScope(): void;
        public _resumeScope(): void;
        public _isScopeSuspended(): boolean;
        public _isAttrDefined(name: string): boolean;
        private _isAppliedToParent;
        public _childInitialized(child: WjLink): void;
        private _initialized();
        private _appliedToParent();
        private _checkRaiseInitialized();
        private _addWatchers();
        private _addEventHandlers();
        private _addEventHandler(eventDesc);
        private _updateScope(eventInfo?);
        private _ngModelRender();
        private _castValueToType(value, prop);
        private _parseDate(value);
        private _isChild();
        private _isParentInitializer();
        private _isParentReferencer();
        private _getParentProp();
        private _getParentReferenceProperty();
        private _useParentObj();
        private _isParentArray();
        private _parentInCtor();
        private _getNgModelProperty();
        private _updateNgModelPropDesc();
        public _safeApply(scope: any, name: any, value: any): void;
        public _shouldApply(scope: any, name: any, value: any): boolean;
        public _canApply(scope: any, name: any): boolean;
        public _nullOrValue(value: any): any;
        public _getIndex(): number;
    }
}

declare module wijmo.angular {
}

declare module wijmo.angular {
    class WjMenuLink extends WjLink {
        public _link(): void;
    }
}

declare module wijmo.angular {
}

declare module wijmo.angular {
}

declare module wijmo.angular {
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
    class WjFlexGrid extends WjDirective {
        public _$compile: ng.ICompileService;
        public _$interpolate: ng.IInterpolateService;
        constructor($compile: ng.ICompileService, $interpolate: ng.IInterpolateService);
        public _controlConstructor : typeof grid.FlexGrid;
        public _createLink(): WjLink;
        public _initProps(): void;
        private _formatterPropHandler(scope, control, value, oldValue, link);
    }
    class WjFlexGridLink extends WjLink {
        public _wrapperFormatter: Function;
        public _userFormatter: Function;
        public _onChildrenReady(): void;
        private _initCellScope(scope, row, col, dataItem, cellValue);
        private _getCellTemplate(tpl);
    }
    /**
    * Defines the type of cell to which to apply the template. This value is specified in the <b>cell-type</b> attribute
    * of the @see:WjFlexGridCellTemplate directive.
    */
    enum CellTemplateType {
        /** Defines a regular (data) cell. */
        Cell = 0,
        /** Defines a cell in edit mode. */
        CellEdit = 1,
        /** Defines a column header cell. */
        ColumnHeader = 2,
        /** Defines a row header cell. */
        RowHeader = 3,
        /** Defines a row header cell in edit mode. */
        RowHeaderEdit = 4,
        /** Defines a top left cell. */
        TopLeft = 5,
        /** Defines a group header cell in a group row. */
        GroupHeader = 6,
        /** Defines a regular cell in a group row. */
        Group = 7,
    }
}

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
declare module wijmo.angular {
}

