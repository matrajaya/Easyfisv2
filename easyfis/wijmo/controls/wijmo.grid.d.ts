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
* Defines the @see:FlexGrid control and associated classes.
*
* The example below creates a @see:FlexGrid control and binds it to a
* 'data' array. The grid has three columns, specified by explicitly
* populating the grid's @see:columns array.
*
* @fiddle:6GB66
*/
declare module wijmo.grid {
    /**
    * Specifies constants that specify the visibility of row and column headers.
    */
    enum HeadersVisibility {
        /** No header cells are displayed. */
        None = 0,
        /** Only column header cells are displayed. */
        Column = 1,
        /** Only row header cells are displayed. */
        Row = 2,
        /** Both column and row header cells are displayed. */
        All = 3,
    }
    /**
    * The @see:FlexGrid control provides a powerful and flexible way to
    * display and edit data in a tabular format.
    *
    * The @see:FlexGrid control is a full-featured grid, providing all the
    * features you are used to including several selection modes, sorting,
    * column reordering, grouping, filtering, editing,  custom cells,
    * XAML-style star-sizing columns, row and column virtualization, etc.
    */
    class FlexGrid extends Control {
        private _root;
        private _eCt;
        private _eTL;
        private _eCHdr;
        private _eRHdr;
        private _eCHdrCt;
        private _eRHdrCt;
        private _eTLCt;
        private _eSz;
        private _gpCells;
        private _gpCHdr;
        private _gpRHdr;
        private _gpTL;
        private _maxOffsetY;
        private _heightBrowser;
        private _szClient;
        private _offsetY;
        private _rcBounds;
        private _lastCount;
        public _ptScrl: Point;
        public _rtl: boolean;
        private _keyHdl;
        public _mouseHdl: _MouseHandler;
        public _edtHdl: _EditHandler;
        public _selHdl: _SelectionHandler;
        private _addHdl;
        private _mrgMgr;
        private _autoGenCols;
        private _autoClipboard;
        private _readOnly;
        private _indent;
        private _allowResizing;
        private _autoSizeMode;
        private _allowDragging;
        private _hdrVis;
        private _alSorting;
        private _alAddNew;
        private _alDelete;
        private _alMerging;
        private _shSort;
        private _shGroups;
        private _gHdrFmt;
        private _rows;
        private _cols;
        private _hdrRows;
        private _hdrCols;
        private _cf;
        private _itemFormatter;
        private _items;
        private _cv;
        private _childItemsPath;
        private _sortRowIndex;
        private _deferResizing;
        private _bndSortConverter;
        /**
        * Gets or sets the template used to instantiate @see:FlexGrid controls.
        */
        static controlTemplate: string;
        /**
        * Initializes a new instance of a @see:FlexGrid control.
        *
        * @param element The DOM element that will host the control, or a selector for the host element (e.g. '#theCtrl').
        * @param options JavaScript object containing initialization data for the control.
        */
        constructor(element: any, options?: any);
        public _handleResize(): void;
        /**
        * Gets or sets a value that determines whether the row and column headers
        * are visible.
        */
        public headersVisibility : HeadersVisibility;
        /**
        * Gets or sets whether the grid should generate columns automatically based on the @see:itemsSource.
        */
        public autoGenerateColumns : boolean;
        /**
        * Gets or sets whether the grid should handle clipboard shortcuts.
        *
        * The clipboard commands are as follows:
        *
        * <dl class="dl-horizontal">
        *   <dt>ctrl+C, ctrl+Ins</dt>    <dd>Copy grid selection to cliboard.</dd>
        *   <dt>ctrl+V, shift+Ins</dt>   <dd>Paste clipboard text to grid selection.</dd>
        * </dl>
        *
        * Only visible rows and columns are included in clipboard operations.
        *
        * Read-only cells are not affected by paste operations.
        */
        public autoClipboard : boolean;
        /**
        * Gets or sets a JSON string that defines the current column layout.
        *
        * The column layout string represents an array with the columns and their
        * properties. It can be used to persist column layouts defined by users so
        * they are preserved across sessions, and can also be used to implement undo/redo
        * functionality in applications that allow users to modify the column layout.
        *
        * The column layout string does not include <b>dataMap</b> properties, because
        * data maps are not serializable.
        */
        public columnLayout : string;
        /**
        * Gets or whether the user can edit the grid cells by typing into them.
        */
        public isReadOnly : boolean;
        /**
        * Gets or sets whether users may resize rows and/or columns
        * with the mouse.
        *
        * If resizing is enabled, users can resize columns by dragging
        * the right edge of column header cells, or rows by dragging the
        * bottom edge of row header cells.
        *
        * Users may also double-click the edge of the header cells to
        * automatically resize rows and columns to fit their content.
        * The autosize behavior can be customized using the @see:autoSizeMode
        * property.
        */
        public allowResizing : AllowResizing;
        /**
        * Gets or sets whether row and column resizing should be deferred until
        * the user releases the mouse button.
        *
        * By default, @see:deferResizing is set to false, causing rows and columns
        * to be resized as the user drags the mouse. Setting this property to true
        * causes the grid to show a resizing marker and to resize the row or column
        * only when the user releases the mouse button.
        */
        public deferResizing : boolean;
        /**
        * Gets or sets which cells should be taken into account when auto-sizing a
        * row or column.
        *
        * This property controls what happens when users double-click the edge of
        * a column header.
        *
        * By default, the grid will automatically set the column width based on the
        * content of the header and data cells in the column. This property allows
        * you to change that to include only the headers or only the data.
        */
        public autoSizeMode : AutoSizeMode;
        /**
        * Gets or sets whether users are allowed to sort columns by clicking the column header cells.
        */
        public allowSorting : boolean;
        /**
        * Gets or sets a value that indicates whether the grid should provide a new row
        * template so users can add items to the source collection.
        *
        * The new row template will not be displayed if the @see:isReadOnly property
        * is set to true.
        */
        public allowAddNew : boolean;
        /**
        * Gets or sets a value that indicates whether the grid should delete
        * selected rows when the user presses the Delete key.
        *
        * Selected rows will not be deleted if the @see:isReadOnly property
        * is set to true.
        */
        public allowDelete : boolean;
        /**
        * Gets or sets which parts of the grid provide cell merging.
        */
        public allowMerging : AllowMerging;
        /**
        * Gets or sets whether the grid should display sort indicators in the column headers.
        *
        * Sorting is controlled by the @see:sortDescriptions property of the
        * @see:ICollectionView object used as a the grid's @see:itemsSource.
        */
        public showSort : boolean;
        /**
        * Gets or sets whether the grid should insert group rows to delimit data groups.
        *
        * Data groups are created by modifying the @see:groupDescriptions property of the
        * @see:ICollectionView object used as a the grid's @see:itemsSource.
        */
        public showGroups : boolean;
        /**
        * Gets or sets the format string used to create the group header content.
        *
        * The string may contain any text, plus the following replacement strings:
        * <ul>
        *   <li><b>{name}</b>: The name of the property being grouped on.</li>
        *   <li><b>{value}</b>: The value of the property being grouped on.</li>
        *   <li><b>{level}</b>: The group level.</li>
        *   <li><b>{count}</b>: The total number of items in this group.</li>
        * </ul>
        *
        * The default value for this property is
        * '{name}: &lt;b&gt;{value}&lt;/b&gt;({count:n0} items)',
        * which creates group headers similar to
        * 'Country: <b>UK</b> (12 items)' or 'Country: <b>Japan</b> (8 items)'.
        */
        public groupHeaderFormat : string;
        /**
        * Gets or sets whether users are allowed to drag rows and/or columns with the mouse.
        */
        public allowDragging : AllowDragging;
        /**
        * Gets or sets the array or @see:ICollectionView that contains items shown on the grid.
        */
        public itemsSource : any;
        /**
        * Gets the @see:ICollectionView that contains the grid data.
        */
        public collectionView : collections.ICollectionView;
        /**
        * Gets or sets the name of the property used to generate child rows in hierarchical grids.
        */
        public childItemsPath : string;
        /**
        * Gets the @see:GridPanel that contains the data cells.
        */
        public cells : GridPanel;
        /**
        * Gets the @see:GridPanel that contains the column header cells.
        */
        public columnHeaders : GridPanel;
        /**
        * Gets the @see:GridPanel that contains the row header cells.
        */
        public rowHeaders : GridPanel;
        /**
        * Gets the @see:GridPanel that contains the top left cells.
        */
        public topLeftCells : GridPanel;
        /**
        * Gets the grid's row collection.
        */
        public rows : RowCollection;
        /**
        * Gets the grid's column collection.
        */
        public columns : ColumnCollection;
        /**
        * Gets or sets the number of frozen rows.
        *
        * Frozen rows do not scroll, but the cells they contain
        * may be selected and edited.
        */
        public frozenRows : number;
        /**
        * Gets or sets the number of frozen columns.
        *
        * Frozen columns do not scroll, but the cells they contain
        * may be selected and edited.
        */
        public frozenColumns : number;
        /**
        * Gets or sets the index of row in the column header panel that
        * shows and changes the current sort.
        *
        * This property is set to null by default, causing the last row
        * in the @see:columnHeaders panel to act as the sort row.
        */
        public sortRowIndex : number;
        /**
        * Gets or sets a @see:Point that represents the value of the grid's scrollbars.
        */
        public scrollPosition : Point;
        /**
        * Gets the client size of the control (control size minus headers minus scrollbars).
        */
        public clientSize : Size;
        /**
        * Gets the bounding rectangle of the control in page coordinates.
        */
        public controlRect : Rect;
        /**
        * Gets the size of the grid content in pixels.
        */
        public scrollSize : Size;
        /**
        * Gets the range of cells currently in view.
        */
        public viewRange : CellRange;
        /**
        * Gets the @see:CellFactory that creates and updates cells for this grid.
        */
        public cellFactory : CellFactory;
        /**
        * Gets or sets a formatter function used to customize cells on this grid.
        *
        * The formatter function can add any content to any cell. It provides
        * complete flexibility over the appearance and behavior of grid cells.
        *
        * If specified, the function should take four parameters: the @see:GridPanel
        * that contains the cell, the row and column indices of the cell, and the
        * HTML element that represents the cell. The function will typically change
        * the <b>innerHTML</b> property of the cell element.
        *
        * For example:
        * <pre>
        * flex.itemFormatter = function(panel, r, c, cell) {
        *   if (panel.cellType == CellType.Cell) {
        *     // draw sparklines in the cell
        *     var col = panel.columns[c];
        *     if (col.name == 'sparklines') {
        *       cell.innerHTML = getSparklike(panel, r, c);
        *     }
        *   }
        * }
        * </pre>
        *
        * Note that the FlexGrid recycles cells, so if your @see:itemFormatter
        * modifies the cell's style attributes, you must make sure that it resets
        * these attributes for cells that should not have them. For example:
        *
        * <pre>
        * flex.itemFormatter = function(panel, r, c, cell) {
        *   // reset attributes we are about to customize
        *   var s = cell.style;
        *   s.color = '';
        *   s.backgroundColor = '';
        *   // customize color and backgroundColor attributes for this cell
        *   ...
        * }
        * </pre>
        *
        * If you have a scenario where multiple clients may want to customize the
        * grid rendering (for example when creating directives or re-usable libraries),
        * consider using the @see:formatItem event instead. The event allows multiple
        * clients to attach their own handlers.
        */
        public itemFormatter : Function;
        /**
        * Gets the value stored in a cell in the scrollable area of the grid.
        *
        * @param r Index of the row that contains the cell.
        * @param c Index of the column that contains the cell.
        * @param formatted Whether to format the value for display.
        */
        public getCellData(r: number, c: number, formatted: boolean): any;
        /**
        * Gets a the bounds of a cell element in viewport coordinates.
        *
        * This method returns the bounds of cells in the @see:cells
        * panel (scrollable data cells). To get the bounds of cells
        * in other panels, use the @see:getCellBoundingRect method
        * in the appropriate @see:GridPanel object.
        *
        * The returned value is a @see:Rect object which contains the
        * position and dimensions of the cell in viewport coordinates.
        * The viewport coordinates are the same used by the
        * <a href="https://developer.mozilla.org/en-US/docs/Web/API/Element.getBoundingClientRect">getBoundingClientRect</a>
        * method.
        *
        * @param r Index of the row that contains the cell.
        * @param c Index of the column that contains the cell.
        */
        public getCellBoundingRect(r: number, c: number): Rect;
        /**
        * Sets the value of a cell in the scrollable area of the grid.
        *
        * @param r Index of the row that contains the cell.
        * @param c Index, name, or binding of the column that contains the cell.
        * @param value Value to store in the cell.
        * @param coerce Whether to change the value automatically to match the column's data type.
        * @return True if the value was stored successfully, false otherwise.
        */
        public setCellData(r: number, c: any, value: any, coerce?: boolean): boolean;
        /**
        * Gets a @see:HitTestInfo object with information about a given point.
        *
        * For example:
        *
        * <pre>
        * // hit test a point when the user clicks on the grid
        * flex.hostElement.addEventListener('click', function (e) {
        *   var ht = flex.hitTest(e.pageX, e.pageY);
        *   console.log('you clicked a cell of type "' +
        *               wijmo.grid.CellType[ht.cellType] +
        *               '".');
        * });
        * </pre>
        *
        * @param pt @see:Point to investigate, in page coordinates, or a MouseEvent object, or x coordinate of the point.
        * @param y Y coordinate of the point in page coordinates (if the first parameter is a number).
        * @return A @see:HitTestInfo object with information about the point.
        */
        public hitTest(pt: any, y?: number): HitTestInfo;
        /**
        * Gets the content of a @see:CellRange as a string suitable for
        * copying to the clipboard.
        *
        * Hidden rows and columns are not included in the clip string.
        *
        * @param rng @see:CellRange to copy. If omitted, the current selection is used.
        */
        public getClipString(rng?: CellRange): string;
        /**
        * Parses a string into rows and columns and applies the content to a given range.
        *
        * Hidden rows and columns are skipped.
        *
        * @param text Tab and newline delimited text to parse into the grid.
        * @param rng @see:CellRange to copy. If omitted, the current selection is used.
        */
        public setClipString(text: string, rng?: CellRange): void;
        public _expandClipString(text: string, rng: CellRange): string;
        /**
        * Refreshes the grid display.
        *
        * @param fullUpdate Whether to update the grid layout and content, or just the content.
        */
        public refresh(fullUpdate?: boolean): void;
        /**
        * Refreshes the grid display.
        *
        * @param fullUpdate Whether to update the grid layout and content, or just the content.
        * @param recycle Whether to recycle existing elements.
        * @param cells List of @see:CellRange objects that specifies which cells must be updated.
        */
        public refreshCells(fullUpdate: boolean, recycle?: boolean, cells?: CellRange[]): void;
        /**
        * Resizes a column to fit its content.
        *
        * @param c Index of the column to resize.
        * @param header Whether the column index refers to a regular or a header row.
        * @param extra Extra spacing, in pixels.
        */
        public autoSizeColumn(c: number, header?: boolean, extra?: number): void;
        /**
        * Resizes a range of columns to fit their content.
        *
        * The grid will always measure all rows in the current view range, plus up to 2,000 rows
        * not currently in view. If the grid contains a large amount of data (say 50,000 rows),
        * then not all rows will be measured since that could potentially take a long time.
        *
        * @param firstColumn Index of the first column to resize (defaults to the first column).
        * @param lastColumn Index of the last column to resize (defaults to the last column).
        * @param header Whether the column indices refer to regular or header columns.
        * @param extra Extra spacing, in pixels.
        */
        public autoSizeColumns(firstColumn?: number, lastColumn?: number, header?: boolean, extra?: number): void;
        /**
        * Resizes a row to fit its content.
        *
        * @param r Index of the row to resize.
        * @param header Whether the row index refers to a regular or a header row.
        * @param extra Extra spacing, in pixels.
        */
        public autoSizeRow(r: number, header?: boolean, extra?: number): void;
        /**
        * Resizes a range of rows to fit their content.
        *
        * @param firstRow Index of the first row to resize.
        * @param lastRow Index of the last row to resize.
        * @param header Whether the row indices refer to regular or header rows.
        * @param extra Extra spacing, in pixels.
        */
        public autoSizeRows(firstRow?: number, lastRow?: number, header?: boolean, extra?: number): void;
        /**
        * Gets or sets the indent used to offset row groups of different levels.
        */
        public treeIndent : number;
        /**
        * Collapses all the group rows to a given level.
        *
        * @param level Maximum group level to show.
        */
        public collapseGroupsToLevel(level: number): void;
        /**
        * Gets or sets the current selection mode.
        */
        public selectionMode : SelectionMode;
        /**
        * Gets or sets the current selection.
        */
        public selection : CellRange;
        /**
        * Selects a cell range and optionally scrolls it into view.
        *
        * @param rng Range to select.
        * @param show Whether to scroll the new selection into view.
        */
        public select(rng: any, show?: any): void;
        /**
        * Gets a @see:SelectedState value that indicates the selected state of a cell.
        *
        * @param r Row index of the cell to inspect.
        * @param c Column index of the cell to inspect.
        */
        public getSelectedState(r: number, c: number): SelectedState;
        /**
        * Scrolls the grid to bring a specific cell into view.
        *
        * @param r Index of the row to scroll into view.
        * @param c Index of the column to scroll into view.
        * @return True if the grid scrolled.
        */
        public scrollIntoView(r: number, c: number): boolean;
        /**
        * Checks whether a given CellRange is valid for this grid's row and column collections.
        *
        * @param rng Range to check.
        */
        public isRangeValid(rng: CellRange): boolean;
        /**
        * Starts editing a given cell.
        *
        * Editing in the @see:FlexGrid is very similar to editing in Excel:
        * Pressing F2 or double-clicking a cell puts the grid in <b>full-edit</b> mode.
        * In this mode, the cell editor remains active until the user presses Enter, Tab,
        * or Escape, or until he moves the selection with the mouse. In full-edit mode,
        * pressing the cursor keys does not cause the grid to exit edit mode.
        *
        * Typing text directly into a cell puts the grid in <b>quick-edit mode</b>.
        * In this mode, the cell editor remains active until the user presses Enter,
        * Tab, or Escape, or any arrow keys.
        *
        * Full-edit mode is normally used to make changes to existing values.
        * Quick-edit mode is normally used for entering new data quickly.
        *
        * While editing, the user can toggle between full and quick modes by
        * pressing the F2 key.
        *
        * @param fullEdit Whether to stay in edit mode when the user presses the cursor keys. Defaults to false.
        * @param r Index of the row to be edited. Defaults to the currently selected row.
        * @param c Index of the column to be edited. Defaults to the currently selected column.
        * @return True if the edit operation started successfully.
        */
        public startEditing(fullEdit?: boolean, r?: number, c?: number): boolean;
        /**
        * Commits any pending edits and exits edit mode.
        *
        * @param cancel Whether pending edits should be canceled or committed.
        * @return True if the edit operation finished successfully.
        */
        public finishEditing(cancel?: boolean): boolean;
        /**
        * Gets the <b>HTMLInputElement</b> that represents the cell editor currently active.
        */
        public activeEditor : HTMLInputElement;
        /**
        * Gets a @see:CellRange that identifies the cell currently being edited.
        */
        public editRange : CellRange;
        /**
        * Gets or sets the @see:MergeManager object responsible for determining how cells
        * should be merged.
        */
        public mergeManager : MergeManager;
        /**
        * Gets a @see:CellRange that specifies the merged extent of a cell
        * in a @see:GridPanel.
        *
        * @param panel @see:GridPanel that contains the range.
        * @param r Index of the row that contains the cell.
        * @param c Index of the column that contains the cell.
        */
        public getMergedRange(panel: GridPanel, r: number, c: number): CellRange;
        /**
        * Occurs after the grid has been bound to a new items source.
        */
        public itemsSourceChanged: Event;
        /**
        * Raises the @see:itemsSourceChanged event.
        */
        public onItemsSourceChanged(): void;
        /**
        * Occurs after the control has scrolled.
        */
        public scrollPositionChanged: Event;
        /**
        * Raises the @see:scrollPositionChanged event.
        */
        public onScrollPositionChanged(): void;
        /**
        * Occurs before selection changes.
        */
        public selectionChanging: Event;
        /**
        * Raises the @see:selectionChanging event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        * @return True if the event was not canceled.
        */
        public onSelectionChanging(e: CellRangeEventArgs): boolean;
        /**
        * Occurs after selection changes.
        */
        public selectionChanged: Event;
        /**
        * Raises the @see:selectionChanged event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        * @return True if the event was not canceled.
        */
        public onSelectionChanged(e: CellRangeEventArgs): boolean;
        /**
        * Occurs before the grid rows are bound to the data source.
        */
        public loadingRows: Event;
        /**
        * Raises the @see:loadingRows event.
        */
        public onLoadingRows(e: CancelEventArgs): void;
        /**
        * Occurs after the grid rows have been bound to the data source.
        */
        public loadedRows: Event;
        /**
        * Raises the @see:loadedRows event.
        */
        public onLoadedRows(e: EventArgs): void;
        /**
        * Occurs as columns are resized.
        */
        public resizingColumn: Event;
        /**
        * Raises the @see:resizingColumn event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        * @return True if the event was not canceled.
        */
        public onResizingColumn(e: CellRangeEventArgs): boolean;
        /**
        * Occurs when the user finishes resizing a column.
        */
        public resizedColumn: Event;
        /**
        * Raises the @see:resizedColumn event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        */
        public onResizedColumn(e: CellRangeEventArgs): void;
        /**
        * Occurs before the user auto-sizes a column by double-clicking the
        * right edge of a column header cell.
        */
        public autoSizingColumn: Event;
        /**
        * Raises the @see:autoSizingColumn event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        */
        public onAutoSizingColumn(e: CellRangeEventArgs): boolean;
        /**
        * Occurs after the user auto-sizes a column by double-clicking the
        * right edge of a column header cell.
        */
        public autoSizedColumn: Event;
        /**
        * Raises the @see:autoSizedColumn event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        */
        public onAutoSizedColumn(e: CellRangeEventArgs): void;
        /**
        * Occurs when the user starts dragging a column.
        */
        public draggingColumn: Event;
        /**
        * Raises the @see:draggingColumn event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        * @return True if the event was not canceled.
        */
        public onDraggingColumn(e: CellRangeEventArgs): boolean;
        /**
        * Occurs when the user finishes dragging a column.
        */
        public draggedColumn: Event;
        /**
        * Raises the @see:draggedColumn event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        */
        public onDraggedColumn(e: CellRangeEventArgs): void;
        /**
        * Occurs as rows are resized.
        */
        public resizingRow: Event;
        /**
        * Raises the @see:resizingRow event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        * @return True if the event was not canceled.
        */
        public onResizingRow(e: CellRangeEventArgs): boolean;
        /**
        * Occurs when the user finishes resizing rows.
        */
        public resizedRow: Event;
        /**
        * Raises the @see:resizedRow event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        */
        public onResizedRow(e: CellRangeEventArgs): void;
        /**
        * Occurs before the user auto-sizes a row by double-clicking the
        * bottom edge of a row header cell.
        */
        public autoSizingRow: Event;
        /**
        * Raises the @see:autoSizingRow event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        */
        public onAutoSizingRow(e: CellRangeEventArgs): boolean;
        /**
        * Occurs after the user auto-sizes a row by double-clicking the
        * bottom edge of a row header cell.
        */
        public autoSizedRow: Event;
        /**
        * Raises the @see:autoSizedRow event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        */
        public onAutoSizedRow(e: CellRangeEventArgs): void;
        /**
        * Occurs when the user starts dragging a row.
        */
        public draggingRow: Event;
        /**
        * Raises the @see:draggingRow event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        * @return True if the event was not canceled.
        */
        public onDraggingRow(e: CellRangeEventArgs): boolean;
        /**
        * Occurs when the user finishes dragging a row.
        */
        public draggedRow: Event;
        /**
        * Raises the @see:draggedRow event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        */
        public onDraggedRow(e: CellRangeEventArgs): void;
        /**
        * Occurs when a group is about to be expanded or collapsed.
        */
        public groupCollapsedChanging: Event;
        /**
        * Raises the @see:groupCollapsedChanging event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        * @return True if the event was not canceled.
        */
        public onGroupCollapsedChanging(e: CellRangeEventArgs): boolean;
        /**
        * Occurs after a group has been expanded or collapsed.
        */
        public groupCollapsedChanged: Event;
        /**
        * Raises the @see:groupCollapsedChanged event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        */
        public onGroupCollapsedChanged(e: CellRangeEventArgs): void;
        /**
        * Occurs before the user applies a sort by clicking on a column header.
        */
        public sortingColumn: Event;
        /**
        * Raises the @see:sortingColumn event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        * @return True if the event was not canceled.
        */
        public onSortingColumn(e: CellRangeEventArgs): boolean;
        /**
        * Occurs after the user applies a sort by clicking on a column header.
        */
        public sortedColumn: Event;
        /**
        * Raises the @see:sortedColumn event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        */
        public onSortedColumn(e: CellRangeEventArgs): void;
        /**
        * Occurs before a cell enters edit mode.
        */
        public beginningEdit: Event;
        /**
        * Raises the @see:beginningEdit event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        * @return True if the event was not canceled.
        */
        public onBeginningEdit(e: CellRangeEventArgs): boolean;
        /**
        * Occurs when an editor cell is created and before it becomes active.
        */
        public prepareCellForEdit: Event;
        /**
        * Raises the @see:prepareCellForEdit event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        */
        public onPrepareCellForEdit(e: CellRangeEventArgs): void;
        /**
        * Occurs when a cell edit is ending.
        */
        public cellEditEnding: Event;
        /**
        * Raises the @see:cellEditEnding event.
        *
        * You can use this event to perform validation and prevent invalid edits.
        * For example, the code below prevents users from entering values that
        * do not contain the letter 'a'. The code demonstrates how you can obtain
        * the old and new values before the edits are applied.
        *
        * <pre>function cellEditEnding (sender, e) {
        *   // get old and new values
        *   var flex = sender,
        *   oldVal = flex.getCellData(e.row, e.col),
        *   newVal = flex.activeEditor.value;
        *   // cancel edits if newVal doesn't contain 'a'
        *   e.cancel = newVal.indexOf('a') &lt; 0;
        * }</pre>
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        * @return True if the event was not canceled.
        */
        public onCellEditEnding(e: CellRangeEventArgs): boolean;
        /**
        * Occurs when a cell edit has been committed or canceled.
        */
        public cellEditEnded: Event;
        /**
        * Raises the @see:cellEditEnded event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        */
        public onCellEditEnded(e: CellRangeEventArgs): void;
        /**
        * Occurs when a row edit is ending, before the changes are committed or canceled.
        */
        public rowEditEnding: Event;
        /**
        * Raises the @see:rowEditEnding event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        */
        public onRowEditEnding(e: CellRangeEventArgs): void;
        /**
        * Occurs when a row edit has been committed or canceled.
        */
        public rowEditEnded: Event;
        /**
        * Raises the @see:rowEditEnded event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        */
        public onRowEditEnded(e: CellRangeEventArgs): void;
        /**
        * Occurs when the user creates a new item by editing the new row template
        * (see the @see:allowAddNew property).
        *
        * The event handler may customize the content of the new item or cancel
        * the new item creation.
        */
        public rowAdded: Event;
        /**
        * Raises the @see:rowAdded event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        */
        public onRowAdded(e: CellRangeEventArgs): void;
        /**
        * Occurs when the user is deleting a selected row by pressing the Delete
        * key (see the @see:allowDelete property).
        *
        * The event handler may cancel the row deletion.
        */
        public deletingRow: Event;
        /**
        * Raises the @see:deletingRow event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        */
        public onDeletingRow(e: CellRangeEventArgs): void;
        /**
        * Occurs when the user is copying the selection content to the
        * clipboard by pressing one of the clipboard shortcut keys
        * (see the @see:autoClipboard property).
        *
        * The event handler may cancel the copy operation.
        */
        public copying: Event;
        /**
        * Raises the @see:copying event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        * @return True if the event was not canceled.
        */
        public onCopying(e: CellRangeEventArgs): boolean;
        /**
        * Occurs after the user has copied the selection content to the
        * clipboard by pressing one of the clipboard shortcut keys
        * (see the @see:autoClipboard property).
        */
        public copied: Event;
        /**
        * Raises the @see:copied event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        */
        public onCopied(e: CellRangeEventArgs): void;
        /**
        * Occurs when the user is pasting content from the clipboard
        * by pressing one of the clipboard shortcut keys
        * (see the @see:autoClipboard property).
        *
        * The event handler may cancel the copy operation.
        */
        public pasting: Event;
        /**
        * Raises the @see:pasting event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        * @return True if the event was not canceled.
        */
        public onPasting(e: CellRangeEventArgs): boolean;
        /**
        * Occurs after the user has pasted content from the
        * clipboard by pressing one of the clipboard shortcut keys
        * (see the @see:autoClipboard property).
        */
        public pasted: Event;
        /**
        * Raises the @see:pasted event.
        *
        * @param e @see:CellRangeEventArgs that contains the event data.
        */
        public onPasted(e: CellRangeEventArgs): void;
        /**
        * Occurs when an element representing a cell has been created.
        *
        * This event can be used to format cells for display. It is similar
        * in purpose to the @see:itemFormatter property, but has the advantage
        * of allowing multiple independent handlers.
        *
        * For example, this code removes the 'wj-wrap' class from cells in
        * group rows:
        *
        * <pre>flex.formatItem.addHandler(function (s, e) {
        *   if (flex.rows[e.row] instanceof wijmo.grid.GroupRow) {
        *     wijmo.removeClass(e.cell, 'wj-wrap');
        *   }
        * });</pre>
        */
        public formatItem: Event;
        /**
        * Raises the @see:formatItem event.
        *
        * @param e @see:FormatItemEventArgs that contains the event data.
        */
        public onFormatItem(e: FormatItemEventArgs): void;
        public _getDesiredWidth(p: GridPanel, r: number, c: number, e: HTMLElement): number;
        public _getDesiredHeight(p: GridPanel, r: number, c: number, e: HTMLElement): number;
        public _getSortRowIndex(): number;
        public _mappedColumns: any;
        private _sortConverter(sd, item, value, init);
        public _bindGrid(full: boolean): void;
        private _cvCollectionChanged(sender, e);
        private _cvCurrentChanged(sender, e);
        private _getRowIndex(index);
        public _getCvIndex(index: number): number;
        private _findRow(data);
        private _createChildren();
        private _updateLayout();
        private _updateScrollPosition();
        private _updateContent(recycle, cells?);
        private _bindColumns();
        private _updateColumnTypes();
        private _bindRows();
        private _addGroup(g);
        private _addTreeNode(item, level);
        private static _getSerializableProperties(obj);
        public _copy(key: string, value: any): boolean;
        public _hasAttribute(e: any, att: string): boolean;
        private static _maxCssHeight;
        private static _getMaxSupportedCssHeight();
        static _rtlMode: string;
        private static _getRtlMode();
    }
}

declare module wijmo.grid {
    /**
    * Provides arguments for @see:CellRange events.
    */
    class CellRangeEventArgs extends CancelEventArgs {
        public _panel: GridPanel;
        public _rng: CellRange;
        /**
        * Initializes a new instance of a @see:CellRangeEventArgs.
        *
        * @param panel @see:GridPanel that contains the range.
        * @param rng Range of cells affected by the event.
        */
        constructor(panel: GridPanel, rng: CellRange);
        /**
        * Gets the @see:GridPanel affected by this event.
        */
        public panel : GridPanel;
        /**
        * Gets the @see:CellRange affected by this event.
        */
        public cellRange : CellRange;
        /**
        * Gets the row affected by this event.
        */
        public row : number;
        /**
        * Gets the column affected by this event.
        */
        public col : number;
    }
    /**
    * Provides arguments for the @see:formatItem event.
    */
    class FormatItemEventArgs extends CellRangeEventArgs {
        public _cell: HTMLElement;
        /**
        * Initializes a new instance of a @see:FormatItemEventArgs.
        *
        * @param panel @see:GridPanel that contains the range.
        * @param rng Range of cells affected by the event.
        * @param cell Element that represents the grid cell to be formatted.
        */
        constructor(panel: GridPanel, rng: CellRange, cell: HTMLElement);
        /**
        * Gets a reference to the element that represents the grid cell to be formatted.
        */
        public cell : HTMLElement;
    }
}

declare module wijmo.grid {
    /**
    * Identifies the type of cell in a @see:GridPanel.
    */
    enum CellType {
        /** Unknown or invalid cell type. */
        None = 0,
        /** Regular data cell. */
        Cell = 1,
        /** Column header cell. */
        ColumnHeader = 2,
        /** Row header cell. */
        RowHeader = 3,
        /** Top-left cell. */
        TopLeft = 4,
    }
    /**
    * Represents a logical part of the grid, such as the column headers, row headers,
    * and scrollable data part.
    */
    class GridPanel {
        private _g;
        private _ct;
        private _e;
        private _rows;
        private _cols;
        private _rng;
        private _offsetY;
        /**
        * Initializes a new instance of a @see:GridPanel.
        *
        * @param grid The @see:FlexGrid object that owns the panel.
        * @param cellType The type of cell in the panel.
        * @param rows The rows displayed in the panel.
        * @param cols The columns displayed in the panel.
        * @param element The HTMLElement that hosts the cells in the control.
        */
        constructor(grid: FlexGrid, cellType: CellType, rows: RowCollection, cols: ColumnCollection, element: HTMLElement);
        /**
        * Gets the grid that owns the panel.
        */
        public grid : FlexGrid;
        /**
        * Gets the type of cell contained in the panel.
        */
        public cellType : CellType;
        /**
        * Gets a @see:CellRange that indicates the range of cells currently visible on the panel.
        */
        public viewRange : CellRange;
        /**
        * Gets the total width of the content in the panel.
        */
        public width : number;
        /**
        * Gets the total height of the content in this panel.
        */
        public height : number;
        /**
        * Gets the panel's row collection.
        */
        public rows : RowCollection;
        /**
        * Gets the panel's column collection.
        */
        public columns : ColumnCollection;
        /**
        * Gets the value stored in a cell in the panel.
        *
        * @param r The row index of the cell.
        * @param c The column index of the cell.
        * @param formatted A value indicating whether to format the value for display.
        */
        public getCellData(r: number, c: number, formatted: boolean): any;
        /**
        * Sets the content of a cell in the panel.
        *
        * @param r The index of the row that contains the cell.
        * @param c The index, name, or binding of the column that contains the cell.
        * @param value The value to store in the cell.
        * @param coerce A value indicating whether to change the value automatically to match the column's data type.
        * @return Returns true if the value is stored successfully, false otherwise (failed cast).
        */
        public setCellData(r: number, c: any, value: any, coerce?: boolean): boolean;
        /**
        * Gets a cell's bounds in viewport coordinates.
        *
        * The returned value is a @see:Rect object which contains the
        * position and dimensions of the cell in viewport coordinates.
        * The viewport coordinates are the same as those used by the
        * <a href="https://developer.mozilla.org/en-US/docs/Web/API/Element.getBoundingClientRect"
        * target="_blank">getBoundingClientRect</a> method.
        *
        * @param r The index of the row that contains the cell.
        * @param c The index of the column that contains the cell.
        */
        public getCellBoundingRect(r: number, c: number): Rect;
        /**
        * Gets the host element for the panel.
        */
        public hostElement : HTMLElement;
        public _getOffsetY(): number;
        public _updateContent(recycle: boolean, offsetY: number, cells?: CellRange[]): void;
        public _reorderCells(newRange: CellRange, oldRange: CellRange): void;
        public _renderRow(r: number, rng: CellRange, frozen: boolean, cells: CellRange[], ctr: number): number;
        public _renderCell(r: number, c: number, rng: CellRange, cells: CellRange[], ctr: number): number;
        public _getViewRange(buffer: boolean): CellRange;
        public _getFrozenPos(): Point;
    }
}

declare module wijmo.grid {
    /**
    * Creates HTML elements that represent cells within a @see:FlexGrid control.
    */
    class CellFactory {
        static _WJA_COLLAPSE: string;
        static _WJA_DROPDOWN: string;
        static _ddIcon: HTMLElement;
        /**
        * Creates or updates a cell in the grid.
        *
        * @param panel The part of the grid that owns the cell.
        * @param r The index of the row containing the cell.
        * @param c The index of the column containing the cell.
        * @param cell The element that represents the cell.
        * @param rng The @see:CellRange object that contains the cell's
        * merged range, or null if the cell is not merged.
        */
        public updateCell(panel: GridPanel, r: number, c: number, cell: HTMLElement, rng?: CellRange): void;
        private _isEditingCell(g, r, c);
        private _getGroupHeader(gr);
        private _getTreeIcon(gr);
        private _getSortIcon(col);
    }
}

declare module wijmo.grid {
    /**
    * Represents a rectangular group of cells defined by two row indices and
    * two column indices.
    */
    class CellRange {
        public _row: number;
        public _col: number;
        public _row2: number;
        public _col2: number;
        /**
        * Initializes a new instance of a @see:CellRange.
        *
        * @param r The index of the first row in the range.
        * @param c The index of the first column in the range.
        * @param r2 The index of the last row in the range.
        * @param c2 The index of the first column in the range.
        */
        constructor(r?: number, c?: number, r2?: number, c2?: number);
        /**
        * Gets or sets the index of the first row in the range.
        */
        public row : number;
        /**
        * Gets or sets the index of the first column in the range.
        */
        public col : number;
        /**
        * Gets or sets the index of the second row in the range.
        */
        public row2 : number;
        /**
        * Gets or sets the index of the second column in the range.
        */
        public col2 : number;
        /**
        * Creates a copy of the range.
        */
        public clone(): CellRange;
        /**
        * Gets the number of rows in the range.
        */
        public rowSpan : number;
        /**
        * Gets the number of columns in the range.
        */
        public columnSpan : number;
        /**
        * Gets the index of the top row in the range.
        */
        public topRow : number;
        /**
        * Gets the index of the bottom row in the range.
        */
        public bottomRow : number;
        /**
        * Gets the index of the leftmost column in the range.
        */
        public leftCol : number;
        /**
        * Gets the index of the rightmost column in the range.
        */
        public rightCol : number;
        /**
        * Checks whether the range contains valid row and column indices
        * (row and column values are zero or greater).
        */
        public isValid : boolean;
        /**
        * Checks whether this range corresponds to a single cell (beginning and ending rows have
        * the same index, and beginning and ending columns have the same index).
        */
        public isSingleCell : boolean;
        /**
        * Checks whether the range contains another range or a specific cell.
        *
        * @param r The CellRange object or row index to find.
        * @param c The column index (required if the r parameter is not a CellRange object).
        */
        public contains(r: any, c?: number): boolean;
        /**
        * Checks whether the range contains a given row.
        *
        * @param r The index of the row to find.
        */
        public containsRow(r: number): boolean;
        /**
        * Checks whether the range contains a given column.
        *
        * @param c The index of the column to find.
        */
        public containsColumn(c: number): boolean;
        /**
        * Checks whether the range intersects another range.
        *
        * @param rng The CellRange object to check.
        */
        public intersects(rng: CellRange): boolean;
        /**
        * Gets the rendered size of this range.
        *
        * @param panel The @see:GridPanel object that contains the range.
        * @return A @see:Size object that represents the sum of row heights and column widths in the range.
        */
        public getRenderSize(panel: GridPanel): Size;
        /**
        * Checks whether the range equals another range.
        * @param rng The CellRange object to compare to this range.
        */
        public equals(rng: CellRange): boolean;
    }
}

declare module wijmo.grid {
    /**
    * Flags that specify the state of a grid row or column.
    */
    enum RowColFlags {
        /** The row or column is visible. */
        Visible = 1,
        /** The row or column can be resized. */
        AllowResizing = 2,
        /** The row or column can be dragged to a new position with the mouse. */
        AllowDragging = 4,
        /** The row or column can contain merged cells. */
        AllowMerging = 8,
        /** The column can be sorted by clicking its header with the mouse. */
        AllowSorting = 16,
        /** The column was generated automatically. */
        AutoGenerated = 32,
        /** The group row is collapsed. */
        Collapsed = 64,
        /** The row has a parent group that is collapsed. */
        ParentCollapsed = 128,
        /** The row or column is selected. */
        Selected = 256,
        /** The row or column is read-only (cannot be edited). */
        ReadOnly = 512,
        /** Cells in this row or column contain HTML text. */
        HtmlContent = 1024,
        /** Cells in this row or column may contain wrapped text. */
        WordWrap = 2048,
        /** Default settings for new rows. */
        RowDefault,
        /** Default settings for new columns. */
        ColumnDefault,
    }
    /**
    * An abstract class that serves as a base for the @see:Row and @see:Column classes.
    */
    class RowCol {
        public _sz: number;
        public _cssClass: string;
        public _szMin: number;
        public _szMax: number;
        public _list: any;
        public _f: RowColFlags;
        public _pos: number;
        public _idx: number;
        /**
        * Gets or sets a value indicating whether the row or column is visible.
        */
        public visible : boolean;
        /**
        * Gets a value indicating whether the row or column is visible and not collapsed.
        *
        * This property is read-only. To change the visibility of a
        * row or column, use the @see:visible property instead.
        */
        public isVisible : boolean;
        /**
        * Gets the position of the row or column.
        */
        public pos : number;
        /**
        * Gets the index of the row or column in the parent collection.
        */
        public index : number;
        /**
        * Gets or sets the size of the row or column.
        * Setting this property to null or negative values causes the element to use the
        * parent collection's default size.
        */
        public size : number;
        /**
        * Gets the render size of the row or column.
        * This property accounts for visibility, default size, and min and max sizes.
        */
        public renderSize : number;
        /**
        * Gets or sets a value indicating whether the user can resize the row or column with the mouse.
        */
        public allowResizing : boolean;
        /**
        * Gets or sets a value indicating whether the user can move the row or column to a new position with the mouse.
        */
        public allowDragging : boolean;
        /**
        * Gets or sets a value indicating whether cells in the row or column can be merged.
        */
        public allowMerging : boolean;
        /**
        * Gets or sets a value indicating whether the row or column is selected.
        */
        public isSelected : boolean;
        /**
        * Gets or sets a value indicating whether cells in the row or column can be edited.
        */
        public isReadOnly : boolean;
        /**
        * Gets or sets a value indicating whether cells in the row or column contain HTML content rather than plain text.
        */
        public isContentHtml : boolean;
        /**
        * Gets or sets a value indicating whether cells in the row or column wrap their content.
        */
        public wordWrap : boolean;
        /**
        * Gets or sets a CSS class name to use when rendering
        * non-header cells in the row or column.
        */
        public cssClass : string;
        /**
        * Gets the FlexGrid that owns the row or column.
        */
        public grid : FlexGrid;
        /**
        * Marks the owner list as dirty and refreshes the owner grid.
        */
        public onPropertyChanged(): void;
        public _getFlag(flag: RowColFlags): boolean;
        public _setFlag(flag: RowColFlags, value: boolean, quiet?: boolean): boolean;
    }
    /**
    * Represents a column on the grid.
    */
    class Column extends RowCol {
        private static _ctr;
        private _hdr;
        private _name;
        private _type;
        private _align;
        private _map;
        private _fmt;
        private _agg;
        private _inpType;
        private _mask;
        private _required;
        private _showDropDown;
        public _binding: Binding;
        public _bindingSort: Binding;
        public _szStar: string;
        public _hash: string;
        /**
        * Initializes a new instance of a @see:Column.
        *
        * @param options Initialization options for the column.
        */
        constructor(options?: any);
        /**
        * Gets or sets the name of the column.
        *
        * The column name can be used to retrieve the column using the @see:getColumn method.
        */
        public name : string;
        /**
        * Gets or sets the type of value stored in the column.
        *
        * Values are coerced into the proper type when editing the grid.
        */
        public dataType : DataType;
        /**
        * Gets or sets whether values in the column are required.
        *
        * By default, this property is set to null, which means values
        * are required, but string columns may contain empty strings.
        *
        * When set to true, values are required and empty strings are
        * not allowed.
        *
        * When set to false, null values and empty strings are allowed.
        */
        public required : boolean;
        /**
        * Gets or sets a value indicating whether the grid adds drop-down buttons to the
        * cells in this column.
        *
        * The drop-down buttons are shown only if the column has a @see:dataMap
        * set and is editable. Clicking on the drop-down buttons causes the grid
        * to show a list where users can select the value for the cell.
        *
        * Cell drop-downs require the wijmo.input module to be loaded.
        */
        public showDropDown : boolean;
        /**
        * Gets or sets the "type" attribute of the HTML input element used to edit values
        * in this column.
        *
        * By default, this property is set to "tel" for numeric columns, and to "text" for
        * all other non-boolean column types. The "tel" input type causes mobile devices
        * to show a numeric keyboard that includes a negative sign and a decimal separator.
        *
        * Use this property to change the default setting if the default does not work well
        * for the current culture, device, or application. In these cases, try setting the
        * property to "number" or simply "text."
        */
        public inputType : string;
        /**
        * Gets or sets a mask to use while editing values in this column.
        *
        * The mask format is the same used by the @see:wijmo.input.InputMask
        * control.
        *
        * If specified, the mask must be compatible with the value of
        * the @see:format property. For example, the mask '99/99/9999' can
        * be used for entering dates formatted as 'MM/dd/yyyy'.
        */
        public mask : string;
        /**
        * Gets or sets the name of the property the column is bound to.
        */
        public binding : string;
        /**
        * Gets or sets the name of the property to use when sorting this column.
        *
        * Use this property in cases where you want the sorting to be performed
        * based on values other than the ones speficied by the @see:binding property.
        *
        * Setting this property is null causes the grid to use the value of the
        * @see:binding property to sort the column.
        */
        public sortMemberPath : string;
        /**
        * Gets or sets the width of the column.
        *
        * Column widths may be positive numbers (sets the column width in pixels),
        * null or negative numbers (uses the collection's default column width), or
        * strings in the format '{number}*' (star sizing).
        *
        * The star-sizing option performs a XAML-style dynamic sizing where column
        * widths are proportional to the number before the star. For example, if
        * a grid has three columns with widths "100", "*", and "3*", the first column
        * will be 100 pixels wide, the second will take up 1/4th of the remaining
        * space, and the last will take up the remaining 3/4ths of the remaining space.
        *
        * Star-sizing allows you to define columns that automatically stretch to fill
        * the width available. For example, set the width of the last column to "*"
        * and it will automatically extend to fill the entire grid width so there's
        * no empty space. You may also want to set the column's @see:minWidth property
        * to prevent the column from getting too narrow.
        */
        public width : any;
        /**
        * Gets or sets the minimum width of the column.
        */
        public minWidth : number;
        /**
        * Gets or sets the maximum width of the column.
        */
        public maxWidth : number;
        /**
        * Gets the render width of the column.
        *
        * The value returned takes into account the column's visibility, default size, and min and max sizes.
        */
        public renderWidth : number;
        /**
        * Gets or sets the horizontal alignment of items in the column.
        *
        * The default value for this property is null, which causes the grid to select
        * the alignment automatically based on the column's @see:dataType (numbers are
        * right-aligned, Boolean values are centered, and other types are left-aligned).
        *
        * If you want to override the default alignment, set this property
        * to 'left,' 'right,' or 'center,'
        */
        public align : string;
        /**
        * Gets the actual column alignment.
        *
        * Returns the value of the @see:align property if it is not null, or
        * selects the alignment based on the column's @see:dataType.
        */
        public getAlignment(): string;
        /**
        * Gets or sets the text displayed in the column header.
        */
        public header : string;
        /**
        * Gets or sets the @see:DataMap used to convert raw values into display
        * values for the column.
        *
        * Columns with an associated @see:dataMap show drop-down buttons that
        * can be used for quick editing. If you do not want to show the drop-down
        * buttons, set the column's @see:showDropDown property to false.
        *
        * Cell drop-downs require the wijmo.input module to be loaded.
        */
        public dataMap : DataMap;
        /**
        * Gets or sets the format string used to convert raw values into display
        * values for the column (see @see:wijmo.Globalize).
        */
        public format : string;
        /**
        * Gets or sets a value indicating whether the user can sort the column by clicking its header.
        */
        public allowSorting : boolean;
        /**
        * Gets a string that describes the current sorting applied to the column.
        * Possible values are '+' for ascending order, '-' for descending order, or
        * null for unsorted columns.
        */
        public currentSort : string;
        /**
        * Gets or sets the @see:Aggregate to display in the group header rows
        * for the column.
        */
        public aggregate : Aggregate;
        private _getBindingSort();
        static _parseStarSize(value: any): number;
    }
    /**
    * Represents a row in the grid.
    */
    class Row extends RowCol {
        private _data;
        public _ubv: any;
        /**
        * Initializes a new instance of a @see:Row.
        *
        * @param dataItem The data item that this row is bound to.
        */
        constructor(dataItem?: any);
        /**
        * Gets or sets the item in the data collection that the item is bound to.
        */
        public dataItem : any;
        /**
        * Gets or sets the height of the row.
        * Setting this property to null or negative values causes the element to use the
        * parent collection's default size.
        */
        public height : number;
        /**
        * Gets the render height of the row.
        *
        * The value returned takes into account the row's visibility, default size, and min and max sizes.
        */
        public renderHeight : number;
    }
    /**
    * Represents a row that serves as a header for a group of rows.
    */
    class GroupRow extends Row {
        public _level: number;
        /**
        * Initializes a new instance of a @see:GroupRow.
        */
        constructor();
        /**
        * Gets or sets the hierarchical level of the group associated with the GroupRow.
        */
        public level : number;
        /**
        * Gets a value that indicates whether the group row has child rows.
        */
        public hasChildren : boolean;
        /**
        * Gets or sets a value that indicates whether the GroupRow is collapsed
        * (child rows are hidden) or expanded (child rows are visible).
        */
        public isCollapsed : boolean;
        public _setCollapsed(collapsed: boolean): void;
        /**
        * Gets a CellRange object that contains all of the rows in the group represented
        * by the GroupRow and all of the columns in the grid.
        */
        public getCellRange(): CellRange;
    }
    /**
    * Abstract class that serves as a base for row and column collections.
    */
    class RowColCollection extends collections.ObservableArray {
        public _g: FlexGrid;
        public _frozen: number;
        public _szDef: number;
        public _szTot: number;
        public _dirty: boolean;
        public _szMin: number;
        public _szMax: number;
        /**
        * Initializes a new instance of a @see:_RowColCollection.
        *
        * @param grid The @see:FlexGrid that owns the collection.
        * @param defaultSize The default size of the elements in the collection.
        */
        constructor(grid: FlexGrid, defaultSize: number);
        /**
        * Gets or sets the default size of elements in the collection.
        */
        public defaultSize : number;
        /**
        * Gets or sets the number of frozen rows or columns in the collection.
        *
        * Frozen rows and columns do not scroll, and instead remain at the top or left of
        * the grid, next to the fixed cells. Unlike fixed cells, however, frozen
        * cells may be selected and edited like regular cells.
        */
        public frozen : number;
        /**
        * Checks whether a row or column is frozen.
        *
        * @param index The index of the row or column to check.
        */
        public isFrozen(index: number): boolean;
        /**
        * Gets or sets the minimum size of elements in the collection.
        */
        public minSize : number;
        /**
        * Gets or sets the maximum size of elements in the collection.
        */
        public maxSize : number;
        /**
        * Gets the total size of the elements in the collection.
        */
        public getTotalSize(): number;
        /**
        * Gets the index of the element at a given physical position.
        * @param position Position of the item in the collection, in pixels.
        */
        public getItemAt(position: number): number;
        /**
        * Finds the next visible cell for a selection change.
        * @param index Starting index for the search.
        * @param move Type of move (size and direction).
        * @param pageSize Size of a page (in case the move is a page up/down).
        */
        public getNextCell(index: number, move: SelMove, pageSize: number): any;
        /**
        * Checks whether an element can be moved from one position to another.
        *
        * @param src The index of the element to move.
        * @param dst The position to which to move the element, or specify -1 to append the element.
        * @return Returns true if the move is valid, false otherwise.
        */
        public canMoveElement(src: number, dst: number): boolean;
        /**
        * Moves an element from one position to another.
        * @param src Index of the element to move.
        * @param dst Position where the element should be moved to (-1 to append).
        */
        public moveElement(src: number, dst: number): void;
        /**
        * Keeps track of dirty state and invalidate grid on changes.
        */
        public onCollectionChanged(e?: collections.NotifyCollectionChangedEventArgs): void;
        /**
        * Appends an item to the array.
        *
        * @param item Item to add to the array.
        * @return The new length of the array.
        */
        public push(item: any): number;
        /**
        * Removes or adds items to the array.
        *
        * @param index Position where items are added or removed.
        * @param count Number of items to remove from the array.
        * @param item Item to add to the array.
        * @return An array containing the removed elements.
        */
        public splice(index: number, count: number, item?: any): any[];
        /**
        * Suspends notifications until the next call to @see:endUpdate.
        */
        public beginUpdate(): void;
        public _update(): boolean;
    }
    /**
    * Represents a collection of @see:Column objects in a @see:FlexGrid control.
    */
    class ColumnCollection extends RowColCollection {
        public _firstVisible: number;
        /**
        * Gets a column by name or by binding.
        *
        * The method searches the column by name. If a column with the given name
        * is not found, it searches by binding. The searches are case-sensitive.
        *
        * @param name The name or binding to find.
        * @return The column with the specified name or binding, or null if not found.
        */
        public getColumn(name: string): Column;
        /**
        * Gets the index of a column by name or binding.
        *
        * The method searches the column by name. If a column with the given name
        * is not found, it searches by binding. The searches are case-sensitive.
        *
        * @param name The name or binding to find.
        * @return The index of column with the specified name or binding, or -1 if not found.
        */
        public indexOf(name: any): number;
        /**
        * Gets the index of the first visible column (where the outline tree is displayed).
        */
        public firstVisibleIndex : number;
        public _update(): boolean;
        public _updateStarSizes(szAvailable: number): boolean;
    }
    /**
    * Represents a collection of @see:Row objects in a @see:FlexGrid control.
    */
    class RowCollection extends RowColCollection {
        public _maxLevel: number;
        /**
        * Gets the maximum group level in the grid.
        *
        * @return The maximum group level or -1 if the grid has no group rows.
        */
        public maxGroupLevel : number;
        public _update(): boolean;
    }
}

declare module wijmo.grid {
    /**
    * Contains information about the part of a @see:FlexGrid control that exists at
    * a specified page coordinate.
    */
    class HitTestInfo {
        public _g: FlexGrid;
        public _p: GridPanel;
        public _pt: Point;
        public _row: number;
        public _col: number;
        public _edge: number;
        static _EDGESIZE: number;
        static _BADANDROID: boolean;
        /**
        * Initializes a new instance of a @see:HitTestInfo object.
        *
        * @param grid The @see:FlexGrid control or @see:GridPanel to investigate.
        * @param pt The @see:Point object in page coordinates to investigate.
        */
        constructor(grid: any, pt: any);
        /**
        * Gets the point in control coordinates that the HitTestInfo refers to.
        */
        public point : Point;
        /**
        * Gets the cell type at the specified position.
        */
        public cellType : CellType;
        /**
        * Gets the grid panel at the specified position.
        */
        public gridPanel : GridPanel;
        /**
        * Gets the row index of the cell at the specified position.
        */
        public row : number;
        /**
        * Gets the column index of the cell at the specified position.
        */
        public col : number;
        /**
        * Gets the cell range at the specified position.
        */
        public cellRange : CellRange;
        /**
        * Gets a value indicating whether the mouse is near the left edge of the cell.
        */
        public edgeLeft : boolean;
        /**
        * Gets a value indicating whether the mouse is near the top edge of the cell.
        */
        public edgeTop : boolean;
        /**
        * Gets a value indicating whether the mouse is near the right edge of the cell.
        */
        public edgeRight : boolean;
        /**
        * Gets a value indicating whether the mouse is near the bottom edge of the cell.
        */
        public edgeBottom : boolean;
        public _isBadAndroid(): boolean;
    }
}

declare module wijmo.grid {
    /**
    * Specifies constants that define which areas of the grid support cell merging.
    */
    enum AllowMerging {
        /** No merging. */ 
        None = 0,
        /** Merge scrollable cells. */ 
        Cells = 1,
        /** Merge column headers. */
        ColumnHeaders = 2,
        /** Merge row headers. */
        RowHeaders = 4,
        /** Merge column and row headers. */
        AllHeaders,
        /** Merge all areas. */
        All,
    }
    /**
    * Defines the @see:FlexGrid's cell merging behavior.
    *
    * An instance of this class is automatically created and assigned to
    * the grid's @see:mergeManager property to implement the grid's default
    * merging behavior.
    *
    * If you want to customize the default merging behavior, create a class
    * that derives from @see:MergeManager and override the @see:getMergedRange method.
    */
    class MergeManager {
        public _g: FlexGrid;
        /**
        * Initializes a new instance of a @see:MergeManager object.
        *
        * @param grid The @see:FlexGrid object that owns this @see:MergeManager.
        */
        constructor(grid: FlexGrid);
        /**
        * Gets a @see:CellRange that specifies the merged extent of a cell
        * in a @see:GridPanel.
        *
        * @param panel The @see:GridPanel that contains the range.
        * @param r The index of the row that contains the cell.
        * @param c The index of the column that contains the cell.
        * @return A @see:CellRange that specifies the merged range, or null if the cell is not merged.
        */
        public getMergedRange(panel: GridPanel, r: number, c: number): CellRange;
    }
}

declare module wijmo.grid {
    /**
    * Represents a data map for use with the column's @see:dataMap property.
    *
    * Data maps provide the grid with automatic look up capabilities. For example,
    * you may want to display a customer name instead of his ID, or a color name
    * instead of its RGB value.
    *
    * The code below binds a grid to a collection of products,
    * then assigns a @see:DataMap to the grid's 'CategoryID' column so that the grid
    * displays the category names rather than the raw IDs.
    *
    * <pre>
    * // bind grid to products
    * var flex = new wijmo.grid.FlexGrid();
    * flex.itemsSource = products;
    * // map CategoryID column to show category name instead of ID
    * var col = flex.columns.getColumn('CategoryID');
    * col.dataMap = new wijmo.grid.DataMap(categories, 'CategoryID', 'CategoryName');
    * </pre>
    */
    class DataMap {
        public _cv: collections.ICollectionView;
        public _keyPath: string;
        public _displayPath: string;
        /**
        * Initializes a new instance of a @see:DataMap.
        *
        * @param itemsSource An array or @see:ICollectionView that contains the items to map.
        * @param selectedValuePath The name of the property that contains the keys (data values).
        * @param displayMemberPath The name of the property to use as the visual representation of the items.
        */
        constructor(itemsSource: any, selectedValuePath: string, displayMemberPath: string);
        /**
        * Gets the @see:ICollectionView object that contains the map data.
        */
        public collectionView : collections.ICollectionView;
        /**
        * Gets the name of the property to use as a key for the item (data value).
        */
        public selectedValuePath : string;
        /**
        * Gets the name of the property to use as the visual representation of the item.
        */
        public displayMemberPath : string;
        /**
        * Gets the key that corresponds to a given display value.
        *
        * @param displayValue The display value of the item to retrieve.
        */
        public getKeyValue(displayValue: string): any;
        /**
        * Gets the display value that corresponds to a given key.
        *
        * @param key The key of the item to retrieve.
        */
        public getDisplayValue(key: any): any;
        /**
        * Gets an array with all of the display values on the map.
        */
        public getDisplayValues(): string[];
        /**
        * Gets an array with all of the keys on the map.
        */
        public getKeyValues(): string[];
        /**
        * Occurs when the map data changes.
        */
        public mapChanged: Event;
        /**
        * Raises the @see:mapChanged event.
        */
        public onMapChanged(): void;
        private _indexOf(value, path, caseSensitive);
    }
}

declare module wijmo.grid {
    /**
    * Specifies constants that define the selection behavior.
    */
    enum SelectionMode {
        /** The user cannot select cells with the mouse or keyboard. */
        None = 0,
        /** The user can select only a single cell at a time. */
        Cell = 1,
        /** The user can select contiguous blocks of cells. */
        CellRange = 2,
        /** The user can select a single row at a time. */
        Row = 3,
        /** The user can select contiguous rows. */
        RowRange = 4,
        /** The user can select non-contiguous rows. */
        ListBox = 5,
    }
    /**
    * Specifies the selected state of a cell.
    */
    enum SelectedState {
        /** The cell is not selected. */
        None = 0,
        /** The cell is selected but is not the active cell. */
        Selected = 1,
        /** The cell is selected and is the active cell. */
        Cursor = 2,
    }
    /**
    * Specifies a type of movement for the selection.
    */
    enum SelMove {
        /** Do not change the selection. */
        None = 0,
        /** Select the next visible cell. */
        Next = 1,
        /** Select the previous visible cell. */
        Prev = 2,
        /** Select the first visible cell in the next page. */
        NextPage = 3,
        /** Select the first visible cell in the previous page. */
        PrevPage = 4,
        /** Select the first visible cell. */
        Home = 5,
        /** Select the last visible cell. */
        End = 6,
        /** Select the next visible cell skipping rows if necessary. */
        NextCell = 7,
        /** Select the previous visible cell skipping rows if necessary. */
        PrevCell = 8,
    }
    /**
    * Handles the grid's selection.
    */
    class _SelectionHandler {
        public _g: FlexGrid;
        public _sel: CellRange;
        public _mode: SelectionMode;
        /**
        * Initializes a new instance of a @see:_SelectionHandler.
        *
        * @param grid @see:FlexGrid that owns this @see:_SelectionHandler.
        */
        constructor(grid: FlexGrid);
        /**
        * Gets or sets the current selection mode.
        */
        public selectionMode : SelectionMode;
        /**
        * Gets or sets the current selection.
        */
        public selection : CellRange;
        /**
        * Gets a @see:SelectedState value that indicates the selected state of a cell.
        *
        * @param r Row index of the cell to inspect.
        * @param c Column index of the cell to inspect.
        */
        public getSelectedState(r: number, c: number): SelectedState;
        /**
        * Selects a cell range and optionally scrolls it into view.
        *
        * @param rng Range to select.
        * @param show Whether to scroll the new selection into view.
        */
        public select(rng: any, show?: any): void;
        /**
        * Moves the selection by a specified amount in the vertical and horizontal directions.
        * @param rowMove How to move the row selection.
        * @param colMove How to move the column selection.
        * @param extend Whether to extend the current selection or start a new one.
        */
        public moveSelection(rowMove: SelMove, colMove: SelMove, extend: boolean): void;
        private _getReferenceCell(rowMove, colMove, extend);
        private _adjustSelection(rng);
    }
}

declare module wijmo.grid {
    /**
    * Handles the grid's keyboard commands.
    */
    class _KeyboardHandler {
        public _g: FlexGrid;
        /**
        * Initializes a new instance of a @see:_KeyboardHandler.
        *
        * @param grid @see:FlexGrid that owns this @see:_KeyboardHandler.
        */
        constructor(grid: FlexGrid);
        private _keyDown(e);
        private _keyPress(e);
        private _moveSel(rowMove, colMove, extend);
        private _deleteSel();
    }
}

declare module wijmo.grid {
    /**
    * Specifies constants that define the row/column sizing behavior.
    */
    enum AllowResizing {
        /** The user may not resize rows or columns. */
        None = 0,
        /** The user may resize columns. */
        Columns = 1,
        /** The user may resize rows. */
        Rows = 2,
        /** The user may resize rows and columns. */
        Both,
    }
    /**
    * Specifies constants that define the row/column auto-sizing behavior.
    */
    enum AutoSizeMode {
        /** Autosizing is disabled. */
        None = 0,
        /** Autosizing accounts for header cells. */
        Headers = 1,
        /** Autosizing accounts for data cells. */
        Cells = 2,
        /** Autosizing accounts for header and data cells. */
        Both,
    }
    /**
    * Specifies constants that define the row/column dragging behavior.
    */
    enum AllowDragging {
        /** The user may not drag rows or columns. */
        None = 0,
        /** The user may drag columns. */
        Columns = 1,
        /** The user may drag rows. */
        Rows = 2,
        /** The user may drag rows and columns. */
        Both,
    }
    /**
    * Handles the grid's mouse commands.
    */
    class _MouseHandler {
        public _g: FlexGrid;
        public _htDown: HitTestInfo;
        public _eMouse: MouseEvent;
        public _lbSelState: boolean;
        public _lbSelRows: Object;
        public _szRowCol: RowCol;
        public _szStart: number;
        public _szArgs: CellRangeEventArgs;
        public _dragSource: any;
        public _dvMarker: HTMLElement;
        public _rngTarget: CellRange;
        /**
        * Initializes a new instance of a @see:_MouseHandler.
        *
        * @param grid @see:FlexGrid that owns this @see:_MouseHandler.
        */
        constructor(grid: FlexGrid);
        /**
        * Resets the mouse state.
        */
        public resetMouseState(): void;
        private _mouseDown(e);
        private _mouseMove(e);
        private _mouseUp(e);
        private _dblClick(e);
        private _hover(e);
        private _mouseSelect(e, extend);
        private _handleResizing(e);
        private _dragStart(e);
        private _dragEnd(e);
        private _dragOver(e);
        private _drop(e);
        private _showResizeMarker(sz);
        private _showDragMarker(ht);
        private _finishResizing(e);
        private _startListBoxSelection(row);
        private _handleSelection(ht, extend);
        private _handleSort(e);
    }
}

declare module wijmo.grid {
    /**
    * Handles the grid's editing.
    */
    class _EditHandler {
        public _g: FlexGrid;
        public _rng: CellRange;
        public _edt: HTMLInputElement;
        public _lbx: input.ListBox;
        public _htDown: HitTestInfo;
        public _fullEdit: boolean;
        public _list: any;
        public _evtInput: any;
        /**
        * Initializes a new instance of an @see:_EditHandler.
        *
        * @param grid @see:FlexGrid that owns this @see:_EditHandler.
        */
        constructor(grid: FlexGrid);
        /**
        * Starts editing a given cell.
        *
        * @param fullEdit Whether to stay in edit mode when the user presses the cursor keys. Defaults to false.
        * @param r Index of the row to be edited. Defaults to the currently selected row.
        * @param c Index of the column to be edited. Defaults to the currently selected column.
        * @param focus Whether to give the editor the focus. Defaults to true.
        * @return True if the edit operation started successfully.
        */
        public startEditing(fullEdit?: boolean, r?: number, c?: number, focus?: boolean): boolean;
        public finishEditing(cancel?: boolean): boolean;
        public activeEditor : HTMLInputElement;
        public editRange : CellRange;
        public _allowEditing(r: number, c: number): boolean;
        private _commitRowEdits();
        public _keyDown(e: any): boolean;
        public _keyPress(e: any): void;
        public _toggleDropDown(ht: HitTestInfo): void;
        private _createListBox();
        private _removeListBox();
    }
}

declare module wijmo.grid {
    /**
    * Manages the new row template used to add rows to the grid.
    */
    class _AddNewHandler {
        private _g;
        private _nrt;
        /**
        * Initializes a new instance of an @see:_AddNewHandler.
        *
        * @param grid @see:FlexGrid that owns this @see:_AddNewHandler.
        */
        constructor(grid: FlexGrid);
        /**
        * Update the new row template to ensure it's visible only if the grid is bound
        * to a data source that supports adding new items, and that it is in the
        * right position.
        */
        public updateNewRowTemplate(): void;
        private _beginningEdit(sender, e);
        private _rowEditEnded(sender, e);
    }
    /**
    * Represents a row template used to add items to the source collection.
    */
    class _NewRowTemplate extends Row {
    }
}

