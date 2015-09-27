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
* Extension that provides a drag and drop UI for editing
* groups in bound @see:FlexGrid controls.
*/
declare module wijmo.grid.grouppanel {
    /**
    * The @see:GroupPanel control provides a drag and drop UI for editing
    * groups in a bound @see:FlexGrid control.
    *
    * It allows users to drag columns from the @see:FlexGrid into the
    * panel and to move groups within the panel. Users may click the
    * group markers in the panel to sort based on the group column or to
    * remove groups.
    *
    * In order to use a @see:GroupPanel, add it to a page that contains a
    * @see:FlexGrid control and set the panel's @see:grid property to the
    * @see:FlexGrid control. For example:
    *
    * <pre>// create a FlexGrid
    * var flex = new wijmo.grid.FlexGrid('#flex-grid');
    * flex.itemsSource = getData();
    * // add a GroupPanel to edit data groups
    * var groupPanel = new wijmo.grid.grouppanel.GroupPanel('#group-panel');
    * groupPanel.placeholder = "Drag columns here to create groups.";
    * groupPanel.grid = flex;</pre>
    */
    class GroupPanel extends Control {
        public _g: any;
        public _gds: collections.ObservableArray;
        public _hideGroupedCols: boolean;
        public _maxGroups: number;
        public _dragCol: Column;
        public _dragMarker: HTMLElement;
        public _divMarkers: HTMLElement;
        public _divPH: HTMLElement;
        /**
        * Gets or sets the template used to instantiate @see:GroupPanel controls.
        */
        static controlTemplate: string;
        /**
        * Initializes a new instance of a @see:GroupPanel control.
        *
        * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
        * @param options The JavaScript object containing initialization data for the control.
        */
        constructor(element: any, options?: any);
        /**
        * Gets or sets a value indicating whether the panel hides grouped columns in the owner grid.
        *
        * The @see:FlexGrid displays grouping information in row headers, so it is
        * usually a good idea to hide grouped columns since they display redundant
        * information.
        */
        public hideGroupedColumns : boolean;
        /**
        * Gets or sets the maximum number of groups allowed.
        */
        public maxGroups : number;
        /**
        * Gets or sets a string to display in the control when it contains no groups.
        */
        public placeholder : string;
        /**
        * Gets or sets the @see:FlexGrid that is connected to this @see:GroupPanel.
        *
        * Once a grid is connected to the panel, the panel displays the groups
        * defined in the grid's data source. Users can drag grid columns
        * into the panel to create new groups, drag groups within the panel to
        * re-arrange the groups, or delete items in the panel to remove the groups.
        */
        public grid : FlexGrid;
        /**
        * Updates the panel to show the current groups.
        */
        public refresh(): void;
        public _addGroup(col: Column, e: MouseEvent): void;
        public _moveGroup(marker: HTMLElement, e: MouseEvent): void;
        public _removeGroup(index: number): void;
        public _getIndex(e: MouseEvent): number;
        public _getElementIndex(e: HTMLElement): number;
        public _draggingColumn(s: FlexGrid, e: CellRangeEventArgs): void;
        public _itemsSourceChanged(s: FlexGrid, e: EventArgs): void;
        public _groupsChanged(s: FlexGrid, e: EventArgs): void;
        public _dragStart(e: any): void;
        public _dragOver(e: any): void;
        public _drop(e: MouseEvent): void;
        public _dragEnd(e: any): void;
        public _click(e: any): void;
    }
}

