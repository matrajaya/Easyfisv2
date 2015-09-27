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
/**
* Extension that provides an Excel-style filtering UI for @see:FlexGrid controls.
*/
declare module wijmo.grid.filter {
    /**
    * Specifies types of column filter.
    */
    enum FilterType {
        /** No filter. */
        None = 0,
        /** A filter based on two conditions. */
        Condition = 1,
        /** A filter based on a set of values. */
        Value = 2,
        /** A filter that combines condition and value filters. */
        Both = 3,
    }
    /**
    * Implements an Excel-style filter for @see:FlexGrid controls.
    *
    * To enable filtering on a @see:FlexGrid control, create an instance
    * of the @see:FlexGridFilter and pass the grid as a parameter to the
    * constructor. For example:
    *
    * <pre>
    * // create FlexGrid
    * var flex = new wijmo.grid.FlexGrid('#gridElement');
    * // enable filtering on the FlexGrid
    * var filter = new wijmo.grid.filter.FlexGridFilter(flex);
    * </pre>
    *
    * Once this is done, a filter icon is added to the grid's column headers.
    * Clicking the icon shows an editor where the user can edit the filter
    * conditions for that column.
    *
    * The @see:FlexGridFilter class depends on the <b>wijmo.grid</b> and
    * <b>wijmo.input</b> modules.
    */
    class FlexGridFilter {
        static _WJA_FILTER: string;
        private _grid;
        private _filters;
        private _filterColumns;
        private _divEdt;
        private _edtCol;
        private _showIcons;
        private _showSort;
        private _defFilterType;
        /**
        * Initializes a new instance of the @see:FlexGridFilter.
        *
        * @param grid The @see:FlexGrid to filter.
        */
        constructor(grid: FlexGrid);
        /**
        * Gets a reference to the @see:FlexGrid that owns this filter.
        */
        public grid : FlexGrid;
        /**
        * Gets or sets a value indicating whether the @see:FlexGridFilter adds filter
        * editing buttons to the grid's column headers.
        *
        * If you set this property to false, then you are responsible for providing
        * a way for users to edit, clear, and apply the filters.
        */
        public showFilterIcons : boolean;
        /**
        * Gets or sets a value indicating whether the filter editor should include
        * sort buttons.
        *
        * By default, the editor shows sort buttons like Excel does. But since users
        * can sort columns by clicking their headers, sort buttons in the filter editor
        * may not be desirable in some circumstances.
        */
        public showSortButtons : boolean;
        /**
        * Gets the filter for the given column.
        *
        * @param col The @see:Column that the filter applies to.
        * @param create Whether to create the filter if it does not exist.
        */
        public getColumnFilter(col: Column, create?: boolean): ColumnFilter;
        /**
        * Gets or sets the default filter type to use.
        *
        * This value can be overridden in filters for specific columns.
        * For example, the code below creates a filter that filters by
        * conditions on all columns except the "ByValue" column:
        *
        * <pre>
        * var f = new wijmo.grid.filter.FlexGridFilter(flex);
        * f.defaultFilterType = FilterType.Condition;
        * var col = flex.columns.getColumn('ByValue'),
        *     cf = f.getColumnFilter(col);
        * cf.filterType = FilterType.Value;
        * </pre>
        */
        public defaultFilterType : FilterType;
        /**
        * Shows the filter editor for the given grid column.
        *
        * @param col The @see:Column that contains the filter to edit.
        */
        public editColumnFilter(col: any): void;
        /**
        * Closes the filter editor.
        */
        public closeEditor(): void;
        /**
        * Applies the current column filters to the grid.
        */
        public apply(): void;
        /**
        * Clears all column filters.
        */
        public clear(): void;
        /**
        * Occurs after the filter is applied.
        */
        public filterApplied: Event;
        /**
        * Raises the @see:filterApplied event.
        */
        public onFilterApplied(): void;
        private _filter(item);
        private _formatItem(sender, e);
        public _mouseDown(e: any): void;
        public _hasAttribute(e: any, att: string): boolean;
    }
}

declare module wijmo.grid.filter {
    /**
    * Defines a filter for a column on a @see:FlexGrid control.
    *
    * This class is used by the @see:FlexGridFilter class; you
    * rarely use it directly.
    */
    interface IColumnFilter {
        column: Column;
        isActive: boolean;
        apply(value: any): boolean;
        clear(): void;
    }
}

declare module wijmo.grid.filter {
    /**
    * Defines a filter for a column on a @see:FlexGrid control.
    *
    * The @see:ColumnFilter contains a @see:ConditionFilter and a
    * @see:ValueFilter; only one of them may be active at a time.
    *
    * This class is used by the @see:FlexGridFilter class; you
    * rarely use it directly.
    */
    class ColumnFilter implements IColumnFilter {
        private _owner;
        private _col;
        private _valueFilter;
        private _conditionFilter;
        private _filterType;
        /**
        * Initializes a new instance of a @see:ColumnFilter.
        *
        * @param owner The @see:FlexGridFilter that owns this column filter.
        * @param column The @see:Column to filter.
        */
        constructor(owner: FlexGridFilter, column: Column);
        /**
        * Gets or sets the types of filtering provided by this filter.
        *
        * Setting this property to null causes the filter to use the value
        * defined by the owner filter's @see:defaultFilterType property.
        */
        public filterType : FilterType;
        /**
        * Gets the @see:ValueFilter in this @see:ColumnFilter.
        */
        public valueFilter : ValueFilter;
        /**
        * Gets the @see:ConditionFilter in this @see:ColumnFilter.
        */
        public conditionFilter : ConditionFilter;
        /**
        * Gets the @see:Column being filtered.
        */
        public column : Column;
        /**
        * Gets a value indicating whether the filter is active.
        */
        public isActive : boolean;
        /**
        * Gets a value indicating whether a value passes the filter.
        *
        * @param value The value to test.
        */
        public apply(value: any): boolean;
        /**
        * Clears the filter.
        */
        public clear(): void;
        /**
        * Returns true if the caller queries for a supported interface.
        *
        * @param interfaceName Name of the interface to look for.
        */
        public implementsInterface(interfaceName: string): boolean;
    }
}

declare module wijmo.grid.filter {
    /**
    * The editor used to inspect and modify column filters.
    *
    * This class is used by the @see:FlexGridFilter class; you
    * rarely use it directly.
    */
    class ColumnFilterEditor extends Control {
        private _filter;
        private _edtVal;
        private _edtCnd;
        private _divSort;
        private _btnAsc;
        private _btnDsc;
        private _divType;
        private _aCnd;
        private _aVal;
        private _divEdtVal;
        private _divEdtCnd;
        private _btnApply;
        private _btnClear;
        /**
        * Gets or sets the template used to instantiate @see:ColumnFilterEditor controls.
        */
        static controlTemplate: string;
        public '</div>': any;
        /**
        * Initializes a new instance of the @see:ColumnFilterEditor.
        *
        * @param element The DOM element that hosts the control, or a selector
        * for the host element (e.g. '#theCtrl').
        * @param filter The @see:ColumnFilter to edit.
        * @param sortButtons Whether to show sort buttons in the editor.
        */
        constructor(element: any, filter: ColumnFilter, sortButtons?: boolean);
        /**
        * Gets a reference to the @see:ColumnFilter being edited.
        */
        public filter : ColumnFilter;
        /**
        * Updates editor with current filter settings.
        */
        public updateEditor(): void;
        /**
        * Updates filter to reflect the current editor values.
        */
        public updateFilter(): void;
        /**
        * Occurs after the filter is modified.
        */
        public filterChanged: Event;
        /**
        * Raises the @see:filterChanged event.
        */
        public onFilterChanged(e?: EventArgs): void;
        private _showFilter(filterType);
        public _enableLink(a: HTMLLinkElement, enable: boolean): void;
        private _getFilterType();
        private _btnClicked(e);
    }
}

declare module wijmo.grid.filter {
    /**
    * Defines a value filter for a column on a @see:FlexGrid control.
    *
    * Value filters contain an explicit list of values that should be
    * displayed by the grid.
    */
    class ValueFilter implements IColumnFilter {
        private _col;
        private _bnd;
        private _values;
        private _filterText;
        /**
        * Initializes a new instance of a @see:ValueFilter.
        *
        * @param column The column to filter.
        */
        constructor(column: Column);
        /**
        * Gets or sets an object with all the formatted values that should be shown.
        */
        public showValues : any;
        /**
        * Gets or sets a string used to filter the list of display values.
        */
        public filterText : string;
        /**
        * Gets the @see:Column to filter.
        */
        public column : Column;
        /**
        * Gets a value indicating whether the filter is active.
        *
        * The filter is active if at least one of the two conditions
        * has its operator set to a non-null value.
        */
        public isActive : boolean;
        /**
        * Gets a value indicating whether a value passes the filter.
        *
        * @param value The value to test.
        */
        public apply(value: any): boolean;
        /**
        * Clears the filter.
        */
        public clear(): void;
        /**
        * Returns true if the caller queries for a supported interface.
        *
        * @param interfaceName Name of the interface to look for.
        */
        public implementsInterface(interfaceName: string): boolean;
    }
}

declare module wijmo.grid.filter {
    /**
    * The editor used to inspect and modify @see:ValueFilter objects.
    *
    * This class is used by the @see:FlexGridFilter class; you
    * rarely use it directly.
    */
    class ValueFilterEditor extends Control {
        private _divFilter;
        private _cmbFilter;
        private _cbSelectAll;
        private _spSelectAll;
        private _divValues;
        private _lbValues;
        private _filter;
        private _toText;
        private _filterText;
        private _view;
        /**
        * Gets or sets the template used to instantiate @see:ColumnFilterEditor controls.
        */
        static controlTemplate: string;
        /**
        * Initializes a new instance of the @see:ValueFilterEditor.
        *
        * @param element The DOM element that hosts the control, or a selector
        * for the host element (e.g. '#theCtrl').
        * @param filter The @see:ValueFilter to edit.
        */
        constructor(element: any, filter: ValueFilter);
        /**
        * Gets a reference to the @see:ValueFilter being edited.
        */
        public filter : ValueFilter;
        /**
        * Updates editor with current filter settings.
        */
        public updateEditor(): void;
        /**
        * Updates filter to reflect the current editor values.
        */
        public updateFilter(): void;
        private _filterTextChanged();
        private _filterValues(value);
        private _cbSelectAllClicked(e);
        private _updateSelectAllCheck();
    }
}

declare module wijmo.grid.filter {
    /**
    * Defines a condition filter for a column on a @see:FlexGrid control.
    *
    * Condition filters contain two conditions that may be combined
    * using an 'and' or an 'or' operator.
    *
    * This class is used by the @see:FlexGridFilter class; you will
    * rarely use it directly.
    */
    class ConditionFilter implements IColumnFilter {
        private _col;
        private _bnd;
        private _c1;
        private _c2;
        private _and;
        /**
        * Initializes a new instance of a @see:ConditionFilter.
        *
        * @param column The column to filter.
        */
        constructor(column: Column);
        /**
        * Gets the first condition in the filter.
        */
        public condition1 : FilterCondition;
        /**
        * Gets the second condition in the filter.
        */
        public condition2 : FilterCondition;
        /**
        * Gets a value indicating whether to combine the two conditions
        * with an AND or an OR operator.
        */
        public and : boolean;
        /**
        * Gets the @see:Column to filter.
        */
        public column : Column;
        /**
        * Gets a value indicating whether the filter is active.
        *
        * The filter is active if at least one of the two conditions
        * has its operator set to a non-null value.
        */
        public isActive : boolean;
        /**
        * Returns a value indicating whether a value passes this filter.
        *
        * @param value The value to test.
        */
        public apply(value: any): boolean;
        /**
        * Clears the filter.
        */
        public clear(): void;
        /**
        * Returns true if the caller queries for a supported interface.
        *
        * @param interfaceName Name of the interface to look for.
        */
        public implementsInterface(interfaceName: string): boolean;
    }
}

declare module wijmo.grid.filter {
    /**
    * The editor used to inspect and modify @see:ConditionFilter objects.
    *
    * This class is used by the @see:FlexGridFilter class; you
    * rarely use it directly.
    */
    class ConditionFilterEditor extends Control {
        private _filter;
        private _cmb1;
        private _val1;
        private _cmb2;
        private _val2;
        private _divHdr;
        private _divCmb1;
        private _divVal1;
        private _divCmb2;
        private _divVal2;
        private _spAnd;
        private _spOr;
        private _btnAnd;
        private _btnOr;
        /**
        * Gets or sets the template used to instantiate @see:ConditionFilterEditor controls.
        */
        static controlTemplate: string;
        /**
        * Initializes a new instance of a @see:ConditionFilterEditor.
        *
        * @param element The DOM element that hosts the control, or a selector
        * for the host element (e.g. '#theCtrl').
        * @param filter The @see:ConditionFilter to edit.
        */
        constructor(element: any, filter: ConditionFilter);
        /**
        * Gets a reference to the @see:ConditionFilter being edited.
        */
        public filter : ConditionFilter;
        /**
        * Updates editor with current filter settings.
        */
        public updateEditor(): void;
        /**
        * Updates filter to reflect the current editor values.
        */
        public updateFilter(): void;
        private _createOperatorCombo(element);
        private _createValueInput(element);
        private _isTimeFormat(fmt);
        private _btnAndOrChanged(e);
    }
}

declare module wijmo.grid.filter {
    /**
    * Defines a filter condition.
    *
    * This class is used by the @see:FlexGridFilter class; you
    * rarely use it directly.
    */
    class FilterCondition {
        private _op;
        private _val;
        private _strVal;
        /**
        * Gets or sets the operator used by this @see:FilterCondition.
        */
        public operator : Operator;
        /**
        * Gets or sets the value used by this @see:FilterCondition.
        */
        public value : any;
        /**
        * Returns a value that determines whether the given value passes this @see:FilterCondition.
        *
        * @param value The value to test.
        */
        public apply(value: any): boolean;
    }
    /**
    * Specifies filter condition operators.
    */
    enum Operator {
        /** Equals. */
        EQ = 0,
        /** Does not equal. */
        NE = 1,
        /** Greater than. */
        GT = 2,
        /** Greater than or equal to. */
        GE = 3,
        /** Less than. */
        LT = 4,
        /** Less than or equal to. */
        LE = 5,
        /** Begins with. */
        BW = 6,
        /** Ends with. */
        EW = 7,
        /** Contains. */
        CT = 8,
        /** Does not contain. */
        NC = 9,
    }
}

