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
// initialize groupHeaderFormat
wijmo.culture.FlexGrid = {
    groupHeaderFormat: '{name}: <b>{value} </b>({count:n0} items)'
};

/**
 * Defines the @see:FlexGrid control and associated classes.
 *
 * The example below creates a @see:FlexGrid control and binds it to a
 * 'data' array. The grid has three columns, specified by explicitly
 * populating the grid's @see:columns array.
 *
 * @fiddle:6GB66
 */
module wijmo.grid {
    'use strict';

    /**
     * Specifies constants that specify the visibility of row and column headers.
     */
    export enum HeadersVisibility {
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
    export class FlexGrid extends Control {

        // child elements
        private _root: HTMLDivElement;
        private _eCt: HTMLDivElement;
        private _eTL: HTMLDivElement;
        private _eCHdr: HTMLDivElement;
        private _eRHdr: HTMLDivElement;
        private _eCHdrCt: HTMLDivElement;
        private _eRHdrCt: HTMLDivElement;
        private _eTLCt: HTMLDivElement;
        private _eSz: HTMLDivElement;

        // child panels
        private _gpCells: GridPanel;
        private _gpCHdr: GridPanel;
        private _gpRHdr: GridPanel;
        private _gpTL: GridPanel;

        // private stuff
        private _maxOffsetY: number;
        private _heightBrowser: number;
        private _szClient = new wijmo.Size(0, 0);
        private _offsetY: number;
        private _rcBounds: Rect;
        private _lastCount: number;
        //private _rafScrl: number;
        /*private*/ _ptScrl = new Point(0, 0); // accessible to GridPanel
        /*private*/ _rtl = false; // accessible to cell factory

        // selection/key/mouse handlers
        private _keyHdl: _KeyboardHandler;
        /*private*/_mouseHdl: _MouseHandler; // acessible to key handler
        /*private*/_edtHdl: _EditHandler; // acessible to key handler
        /*private*/_selHdl: _SelectionHandler; // acessible to key handler
        private _addHdl: _AddNewHandler;
        private _mrgMgr: MergeManager;

        // property storage
        private _autoGenCols = true;
        private _autoClipboard = true;
        private _readOnly = false;
        private _indent = 14;
        private _allowResizing = AllowResizing.Columns;
        private _autoSizeMode = AutoSizeMode.Both;
        private _allowDragging = AllowDragging.Columns;
        private _hdrVis = HeadersVisibility.All;
        private _alSorting = true;
        private _alAddNew = false;
        private _alDelete = false;
        private _alMerging = AllowMerging.None;
        private _shSort = true;
        private _shGroups = true;
        private _gHdrFmt: string;
        private _rows: RowCollection;
        private _cols: ColumnCollection;
        private _hdrRows: RowCollection;
        private _hdrCols: ColumnCollection;
        private _cf: CellFactory;
        private _itemFormatter: Function;
        private _items: any; // any[] or ICollectionView
        private _cv: wijmo.collections.ICollectionView;
        private _childItemsPath: string;
        private _sortRowIndex: number;
        private _deferResizing = false;
        private _bndSortConverter;

        /**
         * Gets or sets the template used to instantiate @see:FlexGrid controls.
         */
        static controlTemplate = '<div style="position:relative;width:100%;height:100%;max-height:inherit;overflow:hidden">' +
            '<div wj-part="root" style="position:absolute;width:100%;height:100%;max-height:inherit;overflow:auto;-webkit-overflow-scrolling:touch">' + // cell container
                '<div wj-part="cells" style ="position:relative"></div>' + // cells
            '</div>' +
            '<div wj-part="tl" style="position:absolute;overflow:hidden">' + // top-left container
                '<div wj-part="tlcells" class="wj-topleft" style="position:relative"></div>' + // top-left cells
            '</div>' +
            '<div wj-part="ch" style="position:absolute;overflow:hidden">' + // col header container
                '<div wj-part="chcells" class="wj-colheaders" style="position:relative"></div>' + // col header cells
            '</div>' +
            '<div wj-part="rh" style="position:absolute;overflow:hidden">' + // row header container
                '<div wj-part="rhcells" class="wj-rowheaders" style="position:relative"></div>' + // row header cells
            '</div>' +
            '<div wj-part="sz" style="position:relative;visibility:hidden;"></div>' + // auto sizing
        '</div>';

        /**
         * Initializes a new instance of a @see:FlexGrid control.
         *
         * @param element The DOM element that will host the control, or a selector for the host element (e.g. '#theCtrl').
         * @param options JavaScript object containing initialization data for the control.
         */
        constructor(element: any, options?) {
            super(element, null, true);
            var e = element,
                self = this,
                host = this.hostElement,
                cs = getComputedStyle(host.parentElement ? host : document.body),
                defRowHei = parseInt(cs.fontSize) * 2;

            // make 100% sure we have a default font height!
            if (defRowHei <= 0) {
                defRowHei = 28;
            }

            this.deferUpdate(function () {

                // create row and column collections
                // (need these to create the child GridPanels!)
                self._rows = new RowCollection(self, defRowHei);
                self._cols = new ColumnCollection(self, defRowHei * 4);
                self._hdrRows = new RowCollection(self, defRowHei);
                self._hdrCols = new ColumnCollection(self, Math.round(defRowHei * 1.25));

                // add row and column headers
                self._hdrRows.push(new Row());
                self._hdrCols.push(new Column());
                self._hdrCols[0].align = 'center';

                // create child elements (GridPanels)
                self._createChildren();

                // initialize control
                self._cf = new CellFactory();
                self._keyHdl = new _KeyboardHandler(self);
                self._mouseHdl = new _MouseHandler(self);
                self._edtHdl = new _EditHandler(self);
                self._selHdl = new _SelectionHandler(self);
                self._addHdl = new _AddNewHandler(self);
                self._mrgMgr = new MergeManager(self);
                self._bndSortConverter = self._sortConverter.bind(self);

                // apply options after grid is fully initialized
                self.initialize(options);
            });

            // update content when user scrolls the control
            this._root.addEventListener('scroll', function () {

                // finish editing when scrolling
                // (or edits will be lost when the grid refreshes)
                self.finishEditing();

                // update grid's scrollPosition to match element's
                if (self._updateScrollPosition()) {

                    // update the content
                    self._updateContent(true);

                    // update the content using requestAnimationFrame (not great)
                    //if (window.requestAnimationFrame) {
                    //    if (self._rafScrl) {
                    //        cancelAnimationFrame(self._rafScrl);
                    //    }
                    //    self._rafScrl = requestAnimationFrame(function() {
                    //        self._updateContent(true);
                    //        self._rafScrl = null;
                    //    });
                    //} else { // IE9 doesn't have requestAnimationFrame...
                    //    self._updateContent(true);
                    //}
                }
            });
        }

        // reset rcBounds when window is resized
        // (even if the control size didn't change, because it may have moved: TFS 112961)
        _handleResize() {
            super._handleResize();
            this._rcBounds = null;
        }

        //--------------------------------------------------------------------------
        //#region ** object model

        /** 
         * Gets or sets a value that determines whether the row and column headers
         * are visible.
         */
        get headersVisibility(): HeadersVisibility {
            return this._hdrVis;
        }
        set headersVisibility(value: HeadersVisibility) {
            if (value != this._hdrVis) {
                this._hdrVis = asEnum(value, HeadersVisibility);
                this.invalidate();
            }
        }
        /**
         * Gets or sets whether the grid should generate columns automatically based on the @see:itemsSource.
         */
        get autoGenerateColumns(): boolean {
            return this._autoGenCols;
        }
        set autoGenerateColumns(value: boolean) {
            this._autoGenCols = asBoolean(value);
        }
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
        get autoClipboard(): boolean {
            return this._autoClipboard;
        }
        set autoClipboard(value: boolean) {
            this._autoClipboard = asBoolean(value);
        }
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
        get columnLayout(): string {
            var props = FlexGrid._getSerializableProperties(Column),
                defs = new Column(),
                proxyCols = [];

            // populate array with proxy columns
            // save only primitive value and non-default settings
            // don't save 'size', we are already saving 'width'
            for (var i = 0; i < this.columns.length; i++) {
                var col = this.columns[i],
                    proxyCol = {};
                for (var j = 0; j < props.length; j++) {
                    var prop = props[j],
                        value = col[prop];
                    if (value != defs[prop] && isPrimitive(value) && prop != 'size') {
                        proxyCol[prop] = value;
                    }
                }
                proxyCols.push(proxyCol)
            }

            // return JSON string with proxy columns
            return JSON.stringify({ columns: proxyCols });
        }
        set columnLayout(value: string) {
            var colOptions = JSON.parse(asString(value));
            if (!colOptions || colOptions.columns == null) {
                throw 'Invalid columnLayout data.';
            }
            this.columns.clear();
            this.initialize(colOptions);
        }
        /**
         * Gets or whether the user can edit the grid cells by typing into them.
         */
        get isReadOnly(): boolean {
            return this._readOnly;
        }
        set isReadOnly(value: boolean) {
            if (value != this._readOnly) {
                this._readOnly = asBoolean(value);
                this.invalidate();
            }
        }
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
        get allowResizing(): AllowResizing {
            return this._allowResizing;
        }
        set allowResizing(value: AllowResizing) {
            this._allowResizing = asEnum(value, AllowResizing);
        }
        /**
         * Gets or sets whether row and column resizing should be deferred until
         * the user releases the mouse button.
         *
         * By default, @see:deferResizing is set to false, causing rows and columns
         * to be resized as the user drags the mouse. Setting this property to true
         * causes the grid to show a resizing marker and to resize the row or column
         * only when the user releases the mouse button.
         */
        get deferResizing() : boolean {
            return this._deferResizing;
        }
        set deferResizing(value: boolean) {
            this._deferResizing = asBoolean(value);
        }
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
        get autoSizeMode(): AutoSizeMode {
            return this._autoSizeMode;
        }
        set autoSizeMode(value: AutoSizeMode) {
            this._autoSizeMode = asEnum(value, AutoSizeMode);
        }
        /**
         * Gets or sets whether users are allowed to sort columns by clicking the column header cells.
         */
        get allowSorting(): boolean {
            return this._alSorting;
        }
        set allowSorting(value: boolean) {
            this._alSorting = asBoolean(value);
        }
        /**
         * Gets or sets a value that indicates whether the grid should provide a new row
         * template so users can add items to the source collection.
         *
         * The new row template will not be displayed if the @see:isReadOnly property
         * is set to true.
         */
        get allowAddNew(): boolean {
            return this._alAddNew;
        }
        set allowAddNew(value: boolean) {
            if (value != this._alAddNew) {
                this._alAddNew = asBoolean(value);
                this._addHdl.updateNewRowTemplate();
            }
        }
        /**
         * Gets or sets a value that indicates whether the grid should delete 
         * selected rows when the user presses the Delete key.
         *
         * Selected rows will not be deleted if the @see:isReadOnly property
         * is set to true.
         */
        get allowDelete(): boolean {
            return this._alDelete;
        }
        set allowDelete(value: boolean) {
            if (value != this._alDelete) {
                this._alDelete = asBoolean(value);
            }
        }
        /**
         * Gets or sets which parts of the grid provide cell merging.
         */
        get allowMerging(): AllowMerging {
            return this._alMerging;
        }
        set allowMerging(value: AllowMerging) {
            if (value != this._alMerging) {
                this._alMerging = asEnum(value, AllowMerging);
                this.invalidate();
            }
        }
        /**
         * Gets or sets whether the grid should display sort indicators in the column headers.
         *
         * Sorting is controlled by the @see:sortDescriptions property of the
         * @see:ICollectionView object used as a the grid's @see:itemsSource.
         */
        get showSort(): boolean {
            return this._shSort;
        }
        set showSort(value: boolean) {
            if (value != this._shSort) {
                this._shSort = asBoolean(value);
                this.invalidate();
            }
        }
        /**
         * Gets or sets whether the grid should insert group rows to delimit data groups.
         *
         * Data groups are created by modifying the @see:groupDescriptions property of the
         * @see:ICollectionView object used as a the grid's @see:itemsSource.
         */
        get showGroups(): boolean {
            return this._shGroups;
        }
        set showGroups(value: boolean) {
            if (value != this._shGroups) {
                this._shGroups = asBoolean(value);
                this._bindGrid(false);
            }
        }
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
        get groupHeaderFormat(): string {
            return this._gHdrFmt;
        }
        set groupHeaderFormat(value: string) {
            if (value != this._gHdrFmt) {
                this._gHdrFmt = asString(value)
                this._bindGrid(false);
            }
        }
        /**
         * Gets or sets whether users are allowed to drag rows and/or columns with the mouse.
         */
        get allowDragging(): AllowDragging {
            return this._allowDragging;
        }
        set allowDragging(value: AllowDragging) {
            if (value != this._allowDragging) {
                this._allowDragging = asEnum(value, AllowDragging);
                this.invalidate(); // to re-create row/col headers
            }
        }
        /**
         * Gets or sets the array or @see:ICollectionView that contains items shown on the grid.
         */
        get itemsSource(): any {
            return this._items;
        }
        set itemsSource(value: any) {
            if (this._items != value) {

                // unbind current collection view
                if (this._cv) {
                    var cv = tryCast(this._cv, wijmo.collections.CollectionView);
                    if (cv && cv.sortConverter == this._bndSortConverter) {
                        cv.sortConverter = null;
                    }
                    this._cv.currentChanged.removeHandler(this._cvCurrentChanged, this);
                    this._cv.collectionChanged.removeHandler(this._cvCollectionChanged, this);
                    this._cv = null;
                }

                // save new data source and collection view
                this._items = value;
                this._cv = asCollectionView(value);
                this._lastCount = 0;

                // bind new collection view
                if (this._cv != null) {
                    this._cv.currentChanged.addHandler(this._cvCurrentChanged, this);
                    this._cv.collectionChanged.addHandler(this._cvCollectionChanged, this);
                    var cv = tryCast(this._cv, wijmo.collections.CollectionView);
                    if (cv && cv.sortConverter == null) {
                        cv.sortConverter = this._bndSortConverter;
                    }
                }

                // bind grid
                this._bindGrid(true);

                // raise itemsSourceChanged
                this.onItemsSourceChanged();
            }
        }
        /**
         * Gets the @see:ICollectionView that contains the grid data.
         */
        get collectionView(): wijmo.collections.ICollectionView {
            return this._cv;
        }
        /**
         * Gets or sets the name of the property used to generate child rows in hierarchical grids.
         */
        get childItemsPath(): string {
            return this._childItemsPath;
        }
        set childItemsPath(value: string) {
            if (value != this._childItemsPath) {
                this._childItemsPath = value;
                this._bindGrid(true);
            }
        }
        /**
         * Gets the @see:GridPanel that contains the data cells.
         */
        get cells(): GridPanel {
            return this._gpCells;
        }
        /**
         * Gets the @see:GridPanel that contains the column header cells.
         */
        get columnHeaders(): GridPanel {
            return this._gpCHdr;
        }
        /**
         * Gets the @see:GridPanel that contains the row header cells.
         */
        get rowHeaders(): GridPanel {
            return this._gpRHdr;
        }
        /**
         * Gets the @see:GridPanel that contains the top left cells.
         */
        get topLeftCells(): GridPanel {
            return this._gpTL;
        }
        /**
         * Gets the grid's row collection.
         */
        get rows(): RowCollection {
            return this._rows;
        }
        /**
         * Gets the grid's column collection.
         */
        get columns(): ColumnCollection {
            return this._cols;
        }
        /**
         * Gets or sets the number of frozen rows.
         *
         * Frozen rows do not scroll, but the cells they contain 
         * may be selected and edited.
         */
        get frozenRows(): number {
            return this.rows.frozen;
        }
        set frozenRows(value: number) {
            this.rows.frozen = value;
        }
        /**
         * Gets or sets the number of frozen columns.
         *
         * Frozen columns do not scroll, but the cells they contain 
         * may be selected and edited.
         */
        get frozenColumns(): number {
            return this.columns.frozen;
        }
        set frozenColumns(value: number) {
            this.columns.frozen = value;
        }
        /**
         * Gets or sets the index of row in the column header panel that
         * shows and changes the current sort.
         *
         * This property is set to null by default, causing the last row
         * in the @see:columnHeaders panel to act as the sort row.
         */
        get sortRowIndex(): number {
            return this._sortRowIndex;
        }
        set sortRowIndex(value: number) {
            if (value != this._sortRowIndex) {
                this._sortRowIndex = asNumber(value, true);
                this.invalidate();
            }
        }
        /**
         * Gets or sets a @see:Point that represents the value of the grid's scrollbars.
         */
        get scrollPosition(): Point {
            return this._ptScrl.clone();
        }
        set scrollPosition(pt: Point) {
            var root = this._root,
                left = -pt.x;

            // IE/Chrome/FF handle scrollLeft differently under RTL:
            // Chrome reverses direction, FF uses negative values, IE does the right thing (nothing)
            if (this._rtl) {
                switch(FlexGrid._getRtlMode()) {
                    case 'rev':
                        left = (root.scrollWidth - root.clientWidth) + pt.x;
                        break;
                    case 'neg':
                        left = pt.x;
                        break;
                    default:
                        left = -pt.x;
                        break;
                }
            }
            //if (root.scrollLeft != left) {
                root.scrollLeft = left;
            //}
            //if (root.scrollTop != -pt.y) {
                root.scrollTop = -pt.y;
            //}
        }
        /**
         * Gets the client size of the control (control size minus headers minus scrollbars).
         */
        get clientSize(): Size {
            return this._szClient;
        }
        /**
         * Gets the bounding rectangle of the control in page coordinates.
         */
        get controlRect(): Rect {
            if (!this._rcBounds) {
                this._rcBounds = getElementRect(this._root);
            }
            return this._rcBounds;
        }
        /**
         * Gets the size of the grid content in pixels.
         */
        get scrollSize(): Size {
            return new Size(this._gpCells.width, this._heightBrowser);
        }
        /**
         * Gets the range of cells currently in view.
         */
        get viewRange(): CellRange {
            return this._gpCells.viewRange;
        }
        /**
         * Gets the @see:CellFactory that creates and updates cells for this grid.
         */
        get cellFactory(): CellFactory {
            return this._cf;
        }
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
        get itemFormatter(): Function {
            return this._itemFormatter;
        }
        set itemFormatter(value: Function) {
            if (value != this._itemFormatter) {
                this._itemFormatter = asFunction(value);
                this.invalidate();
            }
        }
        /**
         * Gets the value stored in a cell in the scrollable area of the grid.
         *
         * @param r Index of the row that contains the cell.
         * @param c Index of the column that contains the cell.
         * @param formatted Whether to format the value for display.
         */
        getCellData(r: number, c: number, formatted: boolean): any {
            return this.cells.getCellData(r, c, formatted);
        }
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
        getCellBoundingRect(r: number, c: number): Rect {
            return this.cells.getCellBoundingRect(r, c);
        }
        /**
         * Sets the value of a cell in the scrollable area of the grid.
         *
         * @param r Index of the row that contains the cell.
         * @param c Index, name, or binding of the column that contains the cell.
         * @param value Value to store in the cell.
         * @param coerce Whether to change the value automatically to match the column's data type.
         * @return True if the value was stored successfully, false otherwise.
         */
        setCellData(r: number, c: any, value: any, coerce = true): boolean {
            return this.cells.setCellData(r, c, value, coerce);
        }
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
        hitTest(pt: any, y?: number): HitTestInfo {
            if (isNumber(pt) && isNumber(y)) { // accept hitTest(x, y) as well
                pt = new Point(pt, y);
            }
            return new HitTestInfo(this, pt);
        }
        /**
         * Gets the content of a @see:CellRange as a string suitable for 
         * copying to the clipboard.
         *
         * Hidden rows and columns are not included in the clip string.
         *
         * @param rng @see:CellRange to copy. If omitted, the current selection is used.
         */
        getClipString(rng?: CellRange): string {
            rng = rng ? asType(rng, CellRange) : this.selection;
            var clipString = '';
            for (var r = rng.topRow; r <= rng.bottomRow; r++) {
                if (!this.rows[r].isVisible) continue;
                var first = true;
                if (clipString) clipString += '\n';
                for (var c = rng.leftCol; c <= rng.rightCol; c++) {
                    if (!this.columns[c].isVisible) continue;
                    if (!first) clipString += '\t';
                    first = false;
                    var cell = this.cells.getCellData(r, c, true).toString();
                    cell = cell.replace(/\t/g, ' ');
                    clipString += cell;
                }
            }
            return clipString;
        }
        /**
         * Parses a string into rows and columns and applies the content to a given range.
         *
         * Hidden rows and columns are skipped.
         *
         * @param text Tab and newline delimited text to parse into the grid.
         * @param rng @see:CellRange to copy. If omitted, the current selection is used.
         */
        setClipString(text: string, rng?: CellRange) {

            // get target range
            var autoRange = rng == null;
            rng = rng ? asType(rng, CellRange) : this.selection;

            // normalize text
            text = asString(text).replace(/\r\n/g, '\n').replace(/\r/g, '\n');
            if (text && text[text.length - 1] == '\n') {
                text = text.substring(0, text.length - 1);
            }
            if (autoRange && !rng.isSingleCell) {
                text = this._expandClipString(text, rng);
            }

            // keep track of paste range to select later
            var rngPaste = new CellRange(rng.topRow, rng.leftCol);

            // copy lines to rows
            this.beginUpdate();
            var row = rng.topRow,
                lines = text.split('\n');
            for (var i = 0; i < lines.length && row < this.rows.length; i++, row++) {

                // skip invisible row, keep clip line
                if (!this.rows[row].isVisible) {
                    i--;
                    continue;
                }

                // copy cells to columns
                var cells = lines[i].split('\t'),
                    col = rng.leftCol;
                for (var j = 0; j < cells.length && col < this.columns.length; j++, col++) {

                    // skip invisible column, keep clip cell
                    if (!this.columns[col].isVisible) {
                        j--;
                        continue;
                    }

                    // assign cell
                    if (!this.columns[col].isReadOnly && !this.rows[row].isReadOnly) {

                        // raise edit events so user can cancel the assignment
                        var e = new CellRangeEventArgs(this.cells, new CellRange(row, col));
                        if (this.onBeginningEdit(e)) {

                            // make the assignment
                            if (this.cells.setCellData(row, col, cells[j])) {

                                // raise post-edit events
                                this.onCellEditEnding(e);
                                this.onCellEditEnded(e);

                                // update paste range
                                rngPaste.row2 = Math.max(rngPaste.row2, row);
                                rngPaste.col2 = Math.max(rngPaste.col2, col);
                            }
                        }
                    }
                }
            }
            this.endUpdate();

            // done, refresh view to update sorting/filtering 
            if (this.collectionView) {
                this.collectionView.refresh();
            }

            // select pasted range
            this.select(rngPaste);
        }

        // expand clip string to get Excel-like behavior
        _expandClipString(text: string, rng: CellRange): string {

            // sanity
            if (!text) return text;

            // get clip string dimensions and cells
            var lines = text.split('\n'),
                srcRows = lines.length,
                srcCols = 0,
                rows = [];
            for (var r = 0; r < srcRows; r++) {
                var cells = lines[r].split('\t');
                rows.push(cells);
                if (r > 1 && cells.length != srcCols) return text;
                srcCols = cells.length;
            }

            // expand if destination size is a multiple of source size (like Excel)
            var dstRows = rng.rowSpan,
                dstCols = rng.columnSpan;
            if (dstRows > 1 || dstCols > 1) {
                if (dstRows == 1) dstRows = srcRows;
                if (dstCols == 1) dstCols = srcCols;
                if (dstCols % srcCols == 0 && dstRows % srcRows == 0) {
                    text = '';
                    for (var r = 0; r < dstRows; r++) {
                        for (var c = 0; c < dstCols; c++) {
                            if (r > 0 && c == 0) text += '\n';
                            if (c > 0) text += '\t';
                            text += rows[r % srcRows][c % srcCols];
                        }
                    }
                }
            }

            // done
            return text;
        }
        /**
         * Refreshes the grid display.
         *
         * @param fullUpdate Whether to update the grid layout and content, or just the content.
         */
        refresh(fullUpdate = true) {

            // always call base class to handle being/endUpdate logic
            super.refresh(fullUpdate);

            // close any open drop-downs
            this.finishEditing();

            // on full updates, get missing column types based on bindings and
            // update scroll position in case the control just became visible
            // and IE wrongly reset the element's scroll position to the origin
            // http://wijmo.com/topic/flexgrid-refresh-issue-when-hidden/
            if (fullUpdate) {
                this._updateColumnTypes();
                this._updateScrollPosition(); 
            }

            // perform the update
            this.refreshCells(fullUpdate);
        }
        /**
         * Refreshes the grid display.
         *
         * @param fullUpdate Whether to update the grid layout and content, or just the content.
         * @param recycle Whether to recycle existing elements.
         * @param cells List of @see:CellRange objects that specifies which cells must be updated.
         */
        refreshCells(fullUpdate: boolean, recycle = false, cells?: CellRange[]) {
            if (!this.isUpdating) {
                if (fullUpdate) {
                    this._updateLayout();
                } else {
                    this._updateContent(recycle, cells);
                }
            }
        }
        /**
         * Resizes a column to fit its content.
         *
         * @param c Index of the column to resize.
         * @param header Whether the column index refers to a regular or a header row.
         * @param extra Extra spacing, in pixels.
         */
        autoSizeColumn(c: number, header = false, extra = 4) {
            this.autoSizeColumns(c, c, header, extra);
        }
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
        autoSizeColumns(firstColumn?: number, lastColumn?: number, header = false, extra = 4) {
            var self = this,
                max = 0,
                pHdr = header ? this.topLeftCells : this.columnHeaders,
                pCells = header ? this.rowHeaders : this.cells,
                rowRange = this.viewRange,
                text: string, lastText: string;
            firstColumn = firstColumn == null ? 0 : asInt(firstColumn);
            lastColumn = lastColumn == null ? pCells.columns.length - 1 : asInt(firstColumn);
            asBoolean(header);
            asNumber(extra);

            // choose row range to measure
            // (viewrange by default, everything if we have only a few items)
            rowRange.row = Math.max(0, rowRange.row - 1000);
            rowRange.row2 = Math.min(rowRange.row2 + 1000, this.rows.length - 1);

            this.deferUpdate(function () {

                // create element to measure content
                var eMeasure = document.createElement('div');
                eMeasure.style.visibility = 'hidden';
                self.hostElement.appendChild(eMeasure);

                // measure cells in the range
                for (var c = firstColumn; c <= lastColumn && c > -1 && c < pCells.columns.length; c++) {
                    max = 0;

                    // headers
                    if (self.autoSizeMode & AutoSizeMode.Headers) {
                        for (var r = 0; r < pHdr.rows.length; r++) {
                            if (pHdr.rows[r].isVisible) {
                                var w = self._getDesiredWidth(pHdr, r, c, eMeasure);
                                max = Math.max(max, w);
                            }
                        }
                    }

                    // cells
                    if (self.autoSizeMode & AutoSizeMode.Cells) {
                        lastText = null;
                        for (var r = rowRange.row; r <= rowRange.row2 && r > -1 && r < pCells.rows.length; r++) {
                            if (pCells.rows[r].isVisible) {

                                if (!header && c == pCells.columns.firstVisibleIndex && pCells.rows.maxGroupLevel > -1) {

                                    // ignore last text for outline cells
                                    var w = self._getDesiredWidth(pCells, r, c, eMeasure);
                                    max = Math.max(max, w);

                                } else {

                                    // regular cells
                                    text = pCells.getCellData(r, c, true);
                                    if (text != lastText) {
                                        lastText = text;
                                        var w = self._getDesiredWidth(pCells, r, c, eMeasure);
                                        max = Math.max(max, w);
                                    }
                                }
                            }
                        }
                    }

                    // set size
                    pCells.columns[c].width = max + extra + 2;
                }

                // done with measuring element
                self.hostElement.removeChild(eMeasure);
            });
        }
        /**
         * Resizes a row to fit its content.
         *
         * @param r Index of the row to resize.
         * @param header Whether the row index refers to a regular or a header row.
         * @param extra Extra spacing, in pixels.
         */
        autoSizeRow(r: number, header = false, extra = 0) {
            this.autoSizeRows(r, r, header, extra);
        }
        /**
         * Resizes a range of rows to fit their content.
         *
         * @param firstRow Index of the first row to resize.
         * @param lastRow Index of the last row to resize.
         * @param header Whether the row indices refer to regular or header rows.
         * @param extra Extra spacing, in pixels.
         */
        autoSizeRows(firstRow?: number, lastRow?: number, header = false, extra = 0) {
            var self = this,
                max = 0,
                pHdr = header ? this.topLeftCells : this.rowHeaders,
                pCells = header ? this.columnHeaders : this.cells;
            header = asBoolean(header);
            extra = asNumber(extra);
            firstRow = firstRow == null ? 0 : asInt(firstRow);
            lastRow = lastRow == null ? pCells.rows.length - 1 : asInt(lastRow);

            this.deferUpdate(function () {

                // create element to measure content
                var eMeasure = document.createElement('div');
                eMeasure.style.visibility = 'hidden';
                self.hostElement.appendChild(eMeasure);

                // measure cells in the range
                for (var r = firstRow; r <= lastRow && r > -1 && r < pCells.rows.length; r++) {
                    max = 0;

                    // headers
                    if (self.autoSizeMode & AutoSizeMode.Headers) {
                        for (var c = 0; c < pHdr.columns.length; c++) {
                            if (pHdr.columns[c].renderSize > 0) {
                                var h = self._getDesiredHeight(pHdr, r, c, eMeasure);
                                max = Math.max(max, h);
                            }
                        }
                    }

                    // cells
                    if (self.autoSizeMode & AutoSizeMode.Cells) {
                        for (var c = 0; c < pCells.columns.length; c++) {
                            if (pCells.columns[c].renderSize > 0) {
                                var h = self._getDesiredHeight(pCells, r, c, eMeasure);
                                max = Math.max(max, h);
                            }
                        }
                    }

                    // update size
                    pCells.rows[r].height = max + extra;
                }

                // done with measuring element
                self.hostElement.removeChild(eMeasure);
            });
        }
        /**
         * Gets or sets the indent used to offset row groups of different levels.
         */
        get treeIndent(): number {
            return this._indent;
        }
        set treeIndent(value: number) {
            if (value != this._indent) {
                this._indent = asNumber(value, false, true);
                this.columns.onCollectionChanged();
            }
        }
        /**
         * Collapses all the group rows to a given level.
         *
         * @param level Maximum group level to show.
         */
        collapseGroupsToLevel(level: number) {

            // finish editing first (this may change the collection)
            if (this.finishEditing()) {

                // set collapsed state for all rows in the grid
                var rows = this.rows;
                rows.deferUpdate(function () {
                    for (var r = 0; r < rows.length; r++) {
                        var gr = tryCast(rows[r], GroupRow);
                        if (gr) {
                            gr.isCollapsed = gr.level >= level;
                        }
                    }
                });
            }
        }
        /**
         * Gets or sets the current selection mode.
         */
        get selectionMode(): SelectionMode {
            return this._selHdl.selectionMode;
        }
        set selectionMode(value: SelectionMode) {
            if (value != this.selectionMode) {
                this._selHdl.selectionMode = asEnum(value, SelectionMode);
                this.invalidate();
            }
        }
        /**
         * Gets or sets the current selection.
         */
        get selection(): CellRange {
            return this._selHdl.selection.clone();
        }
        set selection(value: CellRange) {
            this._selHdl.selection = value;
        }
        /**
         * Selects a cell range and optionally scrolls it into view.
         *
         * @param rng Range to select.
         * @param show Whether to scroll the new selection into view.
         */
        select(rng: any, show: any = true) {
            if (isInt(rng) && isInt(show)) { // accept select(r, c) as well
                rng = new CellRange(rng, show);
                show = true;
            }
            rng = asType(rng, CellRange);
            this._selHdl.select(rng, show);
        }
        /**
         * Gets a @see:SelectedState value that indicates the selected state of a cell.
         *
         * @param r Row index of the cell to inspect.
         * @param c Column index of the cell to inspect.
         */
        getSelectedState(r: number, c: number): SelectedState {
            return this._selHdl.getSelectedState(asInt(r), asInt(c));
        }
        /**
         * Scrolls the grid to bring a specific cell into view.
         *
         * @param r Index of the row to scroll into view.
         * @param c Index of the column to scroll into view.
         * @return True if the grid scrolled.
         */
        scrollIntoView(r: number, c: number): boolean {

            // make sure we have our dimensions set
            if (this._maxOffsetY == null) {
                this._updateLayout();
            }

            // and go to work
            var sp = this.scrollPosition,
                wid = this._szClient.width,
                hei = this._szClient.height,
                ptFrz = this.cells._getFrozenPos();

            // scroll to show row
            r = asInt(r);
            if (r > -1 && r < this._rows.length && r >= this._rows.frozen) {
                var row = <Row>this._rows[r],
                    pct = Math.round(row.pos / (this.cells.height - hei) * 100) / 100,
                    offsetY = Math.round(this._maxOffsetY * pct),
                    rpos = row.pos - offsetY;
                if (row.pos + row.renderSize > -sp.y + hei) {
                    sp.y = Math.max(-rpos, hei - (row.pos + row.renderSize));
                }
                if (rpos - ptFrz.y < -sp.y) {
                    sp.y = -(rpos - ptFrz.y);
                }
            }

            // scroll to show column
            c = asInt(c);
            if (c > -1 && c < this._cols.length && c >= this._cols.frozen) {
                var col = <Column>this._cols[c];
                if (col.pos + col.renderSize > -sp.x + wid) {
                    sp.x = Math.max(-col.pos, wid - (col.pos + col.renderSize));
                }
                if (col.pos - ptFrz.x < -sp.x) {
                    sp.x = -(col.pos - ptFrz.x);
                }
            }

            // update scroll position
            if (!sp.equals(this.scrollPosition)) {
                this.scrollPosition = sp;
                return true;
            }

            // no change
            return false;
        }
        /**
         * Checks whether a given CellRange is valid for this grid's row and column collections.
         *
         * @param rng Range to check.
         */
        isRangeValid(rng: CellRange): boolean {
            return rng.isValid && rng.bottomRow < this.rows.length && rng.rightCol < this.columns.length;
        }
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
        startEditing(fullEdit = true, r?: number, c?: number): boolean {
            return this._edtHdl.startEditing(fullEdit, r, c);
        }
        /**
         * Commits any pending edits and exits edit mode.
         *
         * @param cancel Whether pending edits should be canceled or committed.
         * @return True if the edit operation finished successfully.
         */
        finishEditing(cancel = false): boolean {
            return this._edtHdl.finishEditing(cancel);
        }
        /**
         * Gets the <b>HTMLInputElement</b> that represents the cell editor currently active.
         */
        get activeEditor(): HTMLInputElement {
            return this._edtHdl.activeEditor;
        }
        /**
         * Gets a @see:CellRange that identifies the cell currently being edited.
         */
        get editRange(): CellRange {
            return this._edtHdl.editRange;
        }
        /**
         * Gets or sets the @see:MergeManager object responsible for determining how cells
         * should be merged.
         */
        get mergeManager(): MergeManager {
            return this._mrgMgr;
        }
        set mergeManager(value: MergeManager) {
            if (value != this._mrgMgr) {
                this._mrgMgr = asType(value, "IMergeManager", true);
                this.invalidate();
            }
        }
        /**
         * Gets a @see:CellRange that specifies the merged extent of a cell
         * in a @see:GridPanel.
         *
         * @param panel @see:GridPanel that contains the range.
         * @param r Index of the row that contains the cell.
         * @param c Index of the column that contains the cell.
         */
        getMergedRange(panel: GridPanel, r: number, c: number): CellRange {
            return this._mrgMgr ? this._mrgMgr.getMergedRange(panel, r, c) : null;
        }

        //#endregion

        //--------------------------------------------------------------------------
        //#region ** events

        /**
         * Occurs after the grid has been bound to a new items source.
         */
        itemsSourceChanged = new Event();
        /**
         * Raises the @see:itemsSourceChanged event.
         */
        onItemsSourceChanged() {
            this.itemsSourceChanged.raise(this);
        }
        /**
         * Occurs after the control has scrolled.
         */
        scrollPositionChanged = new Event();
        /**
         * Raises the @see:scrollPositionChanged event.
         */
        onScrollPositionChanged() {
            this.scrollPositionChanged.raise(this);
        }
        /**
         * Occurs before selection changes.
         */
        selectionChanging = new Event();
        /**
         * Raises the @see:selectionChanging event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         * @return True if the event was not canceled.
         */
        onSelectionChanging(e: CellRangeEventArgs): boolean {
            this.selectionChanging.raise(this, e);
            return !e.cancel;
        }
        /**
         * Occurs after selection changes.
         */
        selectionChanged = new Event();
        /**
         * Raises the @see:selectionChanged event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         * @return True if the event was not canceled.
         */
        onSelectionChanged(e: CellRangeEventArgs): boolean {
            this.selectionChanged.raise(this, e);
            return !e.cancel;
        }
        /**
         * Occurs before the grid rows are bound to the data source.
         */
        loadingRows = new Event();
        /**
         * Raises the @see:loadingRows event.
         */
        onLoadingRows(e: CancelEventArgs) {
            this.loadingRows.raise(this, e);
        }
        /**
         * Occurs after the grid rows have been bound to the data source.
         */
        loadedRows = new Event();
        /**
         * Raises the @see:loadedRows event.
         */
        onLoadedRows(e: EventArgs) {
            this.loadedRows.raise(this, e);
        }
        /**
         * Occurs as columns are resized.
         */
        resizingColumn = new Event();
        /**
         * Raises the @see:resizingColumn event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         * @return True if the event was not canceled.
         */
        onResizingColumn(e: CellRangeEventArgs): boolean {
            this.resizingColumn.raise(this, e);
            return !e.cancel;
        }
        /**
         * Occurs when the user finishes resizing a column.
         */
        resizedColumn = new Event();
        /**
         * Raises the @see:resizedColumn event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         */
        onResizedColumn(e: CellRangeEventArgs) {
            this.resizedColumn.raise(this, e);
        }
        /**
         * Occurs before the user auto-sizes a column by double-clicking the 
         * right edge of a column header cell.
         */
        autoSizingColumn = new Event();
        /**
         * Raises the @see:autoSizingColumn event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         */
        onAutoSizingColumn(e: CellRangeEventArgs): boolean {
            this.autoSizingColumn.raise(this, e);
            return !e.cancel;
        }
        /**
         * Occurs after the user auto-sizes a column by double-clicking the 
         * right edge of a column header cell.
         */
        autoSizedColumn = new Event();
        /**
         * Raises the @see:autoSizedColumn event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         */
        onAutoSizedColumn(e: CellRangeEventArgs) {
            this.autoSizedColumn.raise(this, e);
        }
        /**
         * Occurs when the user starts dragging a column.
         */
        draggingColumn = new Event();
        /**
         * Raises the @see:draggingColumn event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         * @return True if the event was not canceled.
         */
        onDraggingColumn(e: CellRangeEventArgs): boolean {
            this.draggingColumn.raise(this, e);
            return !e.cancel;
        }
        /**
         * Occurs when the user finishes dragging a column.
         */
        draggedColumn = new Event();
        /**
         * Raises the @see:draggedColumn event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         */
        onDraggedColumn(e: CellRangeEventArgs) {
            this.draggedColumn.raise(this, e);
        }
        /**
         * Occurs as rows are resized.
         */
        resizingRow = new Event();
        /**
         * Raises the @see:resizingRow event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         * @return True if the event was not canceled.
         */
        onResizingRow(e: CellRangeEventArgs): boolean {
            this.resizingRow.raise(this, e);
            return !e.cancel;
        }
        /**
         * Occurs when the user finishes resizing rows.
         */
        resizedRow = new Event();
        /**
         * Raises the @see:resizedRow event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         */
        onResizedRow(e: CellRangeEventArgs) {
            this.resizedRow.raise(this, e);
        }
        /**
         * Occurs before the user auto-sizes a row by double-clicking the 
         * bottom edge of a row header cell.
         */
        autoSizingRow = new Event();
        /**
         * Raises the @see:autoSizingRow event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         */
        onAutoSizingRow(e: CellRangeEventArgs): boolean {
            this.autoSizingRow.raise(this, e);
            return !e.cancel;
        }
        /**
         * Occurs after the user auto-sizes a row by double-clicking the 
         * bottom edge of a row header cell.
         */
        autoSizedRow = new Event();
        /**
         * Raises the @see:autoSizedRow event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         */
        onAutoSizedRow(e: CellRangeEventArgs) {
            this.autoSizedRow.raise(this, e);
        }
        /**
         * Occurs when the user starts dragging a row.
         */
        draggingRow = new Event();
        /**
         * Raises the @see:draggingRow event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         * @return True if the event was not canceled.
         */
        onDraggingRow(e: CellRangeEventArgs): boolean {
            this.draggingRow.raise(this, e);
            return !e.cancel;
        }
        /**
         * Occurs when the user finishes dragging a row.
         */
        draggedRow = new Event();
        /**
         * Raises the @see:draggedRow event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         */
        onDraggedRow(e: CellRangeEventArgs) {
            this.draggedRow.raise(this, e);
        }
        /**
         * Occurs when a group is about to be expanded or collapsed.
         */
        groupCollapsedChanging = new Event();
        /**
         * Raises the @see:groupCollapsedChanging event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         * @return True if the event was not canceled.
         */
        onGroupCollapsedChanging(e: CellRangeEventArgs): boolean {
            this.groupCollapsedChanging.raise(this, e);
            return !e.cancel;
        }
        /**
         * Occurs after a group has been expanded or collapsed.
         */
        groupCollapsedChanged = new Event();
        /**
         * Raises the @see:groupCollapsedChanged event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         */
        onGroupCollapsedChanged(e: CellRangeEventArgs) {
            this.groupCollapsedChanged.raise(this, e);
        }
        /**
         * Occurs before the user applies a sort by clicking on a column header.
         */
        sortingColumn = new Event();
        /**
         * Raises the @see:sortingColumn event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         * @return True if the event was not canceled.
         */
        onSortingColumn(e: CellRangeEventArgs): boolean {
            this.sortingColumn.raise(this, e);
            return !e.cancel;
        }
        /**
         * Occurs after the user applies a sort by clicking on a column header.
         */
        sortedColumn = new Event();
        /**
         * Raises the @see:sortedColumn event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         */
        onSortedColumn(e: CellRangeEventArgs) {
            this.sortedColumn.raise(this, e);
        }
        /**
         * Occurs before a cell enters edit mode.
         */
        beginningEdit = new Event();
        /**
         * Raises the @see:beginningEdit event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         * @return True if the event was not canceled.
         */
        onBeginningEdit(e: CellRangeEventArgs): boolean {
            this.beginningEdit.raise(this, e);
            return !e.cancel;
        }
        /**
         * Occurs when an editor cell is created and before it becomes active.
         */
        prepareCellForEdit = new Event();
        /**
         * Raises the @see:prepareCellForEdit event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         */
        onPrepareCellForEdit(e: CellRangeEventArgs) {
            this.prepareCellForEdit.raise(this, e);
        }
        /**
         * Occurs when a cell edit is ending.
         */
        cellEditEnding = new Event();
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
        onCellEditEnding(e: CellRangeEventArgs): boolean {
            this.cellEditEnding.raise(this, e);
            return !e.cancel;
        }
        /**
         * Occurs when a cell edit has been committed or canceled.
         */
        cellEditEnded = new Event();
        /**
         * Raises the @see:cellEditEnded event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         */
        onCellEditEnded(e: CellRangeEventArgs) {
            this.cellEditEnded.raise(this, e);
        }
        /**
         * Occurs when a row edit is ending, before the changes are committed or canceled.
         */
        rowEditEnding = new Event();
        /**
         * Raises the @see:rowEditEnding event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         */
        onRowEditEnding(e: CellRangeEventArgs) {
            this.rowEditEnding.raise(this, e);
        }
        /**
         * Occurs when a row edit has been committed or canceled.
         */
        rowEditEnded = new Event();
        /**
         * Raises the @see:rowEditEnded event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         */
        onRowEditEnded(e: CellRangeEventArgs) {
            this.rowEditEnded.raise(this, e);
        }
        /**
         * Occurs when the user creates a new item by editing the new row template
         * (see the @see:allowAddNew property).
         *
         * The event handler may customize the content of the new item or cancel
         * the new item creation.
         */
        rowAdded = new Event();
        /**
         * Raises the @see:rowAdded event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         */
        onRowAdded(e: CellRangeEventArgs) {
            this.rowAdded.raise(this, e);
        }
        /**
         * Occurs when the user is deleting a selected row by pressing the Delete 
         * key (see the @see:allowDelete property).
         *
         * The event handler may cancel the row deletion.
         */
        deletingRow = new Event();
        /**
         * Raises the @see:deletingRow event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         */
        onDeletingRow(e: CellRangeEventArgs) {
            this.deletingRow.raise(this, e);
        }
        /**
         * Occurs when the user is copying the selection content to the 
         * clipboard by pressing one of the clipboard shortcut keys
         * (see the @see:autoClipboard property).
         *
         * The event handler may cancel the copy operation.
         */
        copying = new Event();
        /**
         * Raises the @see:copying event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         * @return True if the event was not canceled.
         */
        onCopying(e: CellRangeEventArgs): boolean {
            this.copying.raise(this, e);
            return !e.cancel;
        }
        /**
         * Occurs after the user has copied the selection content to the 
         * clipboard by pressing one of the clipboard shortcut keys
         * (see the @see:autoClipboard property).
         */
        copied = new Event();
        /**
         * Raises the @see:copied event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         */
        onCopied(e: CellRangeEventArgs) {
            this.copied.raise(this, e);
        }
        /**
         * Occurs when the user is pasting content from the clipboard 
         * by pressing one of the clipboard shortcut keys
         * (see the @see:autoClipboard property).
         *
         * The event handler may cancel the copy operation.
         */
        pasting = new Event();
        /**
         * Raises the @see:pasting event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         * @return True if the event was not canceled.
         */
        onPasting(e: CellRangeEventArgs): boolean {
            this.pasting.raise(this, e);
            return !e.cancel;
        }
        /**
         * Occurs after the user has pasted content from the 
         * clipboard by pressing one of the clipboard shortcut keys
         * (see the @see:autoClipboard property).
         */
        pasted = new Event();
        /**
         * Raises the @see:pasted event.
         *
         * @param e @see:CellRangeEventArgs that contains the event data.
         */
        onPasted(e: CellRangeEventArgs) {
            this.pasted.raise(this, e);
        }
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
        formatItem = new Event();
        /**
         * Raises the @see:formatItem event.
         *
         * @param e @see:FormatItemEventArgs that contains the event data.
         */
        onFormatItem(e: FormatItemEventArgs) {
            this.formatItem.raise(this, e);
        }
        //#endregion

        //--------------------------------------------------------------------------
        //#region ** implementation

        // measures the desired width of a cell
        _getDesiredWidth(p: GridPanel, r: number, c: number, e: HTMLElement) {
            var rng = this.getMergedRange(p, r, c);
            this.cellFactory.updateCell(p, r, c, e, rng);
            e.style.width = '';
            var w = e.offsetWidth;
            return rng && rng.columnSpan > 1
                ? w / rng.columnSpan
                : w;
        }

        // measures the desired height of a cell
        _getDesiredHeight(p: GridPanel, r: number, c: number, e: HTMLElement) {
            var rng = this.getMergedRange(p, r, c);
            this.cellFactory.updateCell(p, r, c, e, rng);
            e.style.height = '';
            var h = e.offsetHeight;
            return rng && rng.rowSpan > 1
                ? h / rng.rowSpan
                : h;
        }

        // gets the index of the sort row, with special handling for nulls
        _getSortRowIndex(): number {
            return this._sortRowIndex != null
                ? this._sortRowIndex
                : this.columnHeaders.rows.length - 1;
        }

        // sort converter used to sort mapped columns by display value
        _mappedColumns = null;
        private _sortConverter(sd: wijmo.collections.SortDescription, item: any, value: any, init: boolean) {

            // initialize mapped column object
            if (init) {
                this._mappedColumns = null;
                for (var i = 0; i < this.columns.length; i++) {
                    var col = this.columns[i];
                    if (col.dataMap) {
                        if (this._mappedColumns == null) {
                            this._mappedColumns = {};
                        }
                        this._mappedColumns[col.binding] = col.dataMap;
                    }
                }

                // priority to column that was clicked
                // (in case multiple columns map the same property)
                if (this._mouseHdl._htDown && this._mouseHdl._htDown.col > -1) {
                    var col = this.columns[this._mouseHdl._htDown.col];
                    if (col.dataMap) {
                        this._mappedColumns[col.binding] = col.dataMap;
                    }
                }
            }

            // convert value if we have a map
            if (this._mappedColumns) {
                var map = <DataMap>this._mappedColumns[sd.property];
                if (map) {
                    value = map.getDisplayValue(value);
                }
            }

            // return the value to use for sorting
            return value;
        }

        // binds the grid to the current data source.
        _bindGrid(full: boolean) {
            var self = this,
                sel: CellRange;
            self.deferUpdate(function () {

                // do a full binding if we didn't have any data when we did it the first time
                if (self._lastCount == 0 && self._cv && self._cv.items && self._cv.items.length) {
                    full = true;
                }

                // save selected items
                var selItems = [];
                if (self.selectionMode == SelectionMode.ListBox) {
                    for (var i = 0; i < self.rows.length; i++) {
                        var row = self.rows[i];
                        if (row.isSelected && row.dataItem) {
                            selItems.push(row.dataItem);
                        }
                    }
                }

                // update rows
                self.rows.deferUpdate(function () {
                    self._bindRows();
                });

                // update columns
                if (full) {
                    self.columns.deferUpdate(function () {
                        self._bindColumns();
                    });
                }

                // restore/initialize listbox selection
                var cnt = 0;
                if (selItems.length) {
                    for (var i = 0; i < self.rows.length && cnt < selItems.length; i++) {
                        if (selItems.indexOf(self.rows[i].dataItem) > -1) {
                            self.rows[i].isSelected = true;
                            cnt++;
                        }
                    }
                }

                // failed to restore listbox selection by object, update by index
                if (self.selectionMode == SelectionMode.ListBox && cnt == 0) {
                    sel = self.selection;
                    for (var i = sel.topRow; i <= sel.bottomRow && i > -1 && i < self.rows.length; i++) {
                        self.rows[i].isSelected = true;
                    }
                }

                // save item count for next time
                if (!self._lastCount && self._cv && self._cv.items) {
                    self._lastCount = self._cv.items.length;
                }
            });

            // update selection
            if (self.collectionView) {
                self._cvCurrentChanged(self.collectionView, EventArgs.empty);
            }
        }

        // updates grid rows to sync with data source
        private _cvCollectionChanged(sender, e: wijmo.collections.NotifyCollectionChangedEventArgs) {
            if (this.autoGenerateColumns && this.columns.length == 0) {

                // bind rows and columns
                this._bindGrid(true);

            } else {

                // synchronize grid with updated CollectionView
                var index: number;
                switch (e.action) {

                    // an item has changed, invalidate the grid to show the changes
                    case wijmo.collections.NotifyCollectionChangedAction.Change:
                        this.invalidate();
                        return;

                    // an item has been added, insert a row
                    case wijmo.collections.NotifyCollectionChangedAction.Add:
                        if (e.index == this.collectionView.items.length - 1) {
                            index = this.rows.length;
                            if (this.rows[index - 1] instanceof _NewRowTemplate) {
                                index--;
                            }
                            this.rows.insert(index, new Row(e.item));
                            return;
                        }
                        assert(false, 'added item should be the last one.');
                        break;

                    // an item has been removed, delete the row
                    case wijmo.collections.NotifyCollectionChangedAction.Remove:
                        var index = this._findRow(e.item);
                        if (index > -1) {
                            this.rows.removeAt(index);
                            this._cvCurrentChanged(sender, e);
                            return;
                        }
                        assert(false, 'removed item not found in grid.');
                        break;
                }

                // reset (sort, new source, etc): re-create all rows
                this._bindGrid(false);
            }
        }

        // update selection to sync with data source
        private _cvCurrentChanged(sender, e) {
            if (this.collectionView) {
                var sel = this.selection,
                    index = this._getRowIndex(this.collectionView.currentPosition);
                if (sel.row != index) {

                    // update selection
                    sel.row = sel.row2 = index;
                    this.select(sel, false);

                    // scroll selected row into view (keep column)
                    this.scrollIntoView(sel.row, -1); 
                }
            }
        }

        // convert CollectionView index to row index
        private _getRowIndex(index: number): number {
            if (this.collectionView) {

                // look up item, then scan rows to find it
                if (index > -1) {
                    var item = this.collectionView.items[index];
                    for (; index < this.rows.length; index++) {
                        if (this.rows[index].dataItem === item) {
                            return index;
                        }
                    }
                    return -1; // item not found, shouldn't happen!
                } else {

                    // no item to look up, so return current unbound row (group header) or -1 (no selection)
                    var index = this.selection.row;
                    return index > -1 && this.rows[index] instanceof GroupRow ? index : -1;
                }
            }

            // not bound
            return this.selection.row;
        }

        // convert row index to CollectionView index
        _getCvIndex(index: number): number {
            if (index > -1 && this.collectionView) {
                var item = this.rows[index].dataItem;
                index = Math.min(index, this.collectionView.items.length);
                for (; index > -1; index--) {
                    if (this.collectionView.items[index] === item) {
                        return index;
                    }
                }
            }
            return -1;
        }

        // gets the index of the row that represents a given data item
        private _findRow(data: any): number {
            for (var i = 0; i < this.rows.length; i++) {
                if (this.rows[i].dataItem == data) {
                    return i;
                }
            }
            return -1;
        }

        // creates the child HTMLElements within this grid.
        private _createChildren() {

            // instantiate and apply template
            var tpl = this.getTemplate();
            this.applyTemplate('wj-control wj-flexgrid wj-content', tpl, {
                _root: 'root',
                _eSz: 'sz',
                _eCt: 'cells',
                _eTL: 'tl',
                _eCHdr: 'ch',
                _eRHdr: 'rh',
                _eTLCt: 'tlcells',
                _eCHdrCt: 'chcells',
                _eRHdrCt: 'rhcells'
            });

            // create grid panels
            this._gpCells = new GridPanel(this, CellType.Cell, this._rows, this._cols, this._eCt);
            this._gpCHdr = new GridPanel(this, CellType.ColumnHeader, this._hdrRows, this._cols, this._eCHdrCt);
            this._gpRHdr = new GridPanel(this, CellType.RowHeader, this._rows, this._hdrCols, this._eRHdrCt);
            this._gpTL = new GridPanel(this, CellType.TopLeft, this._hdrRows, this._hdrCols, this._eTLCt);
        }

        // re-arranges the child HTMLElements within this grid.
        private _updateLayout() {

            // compute content height, max height supported by browser,
            // and max offset so things match up when you scroll all the way down.
            var heightReal = this._rows.getTotalSize(),
                tlw = (this._hdrVis & HeadersVisibility.Row) ? this._hdrCols.getTotalSize() : 0,
                tlh = (this._hdrVis & HeadersVisibility.Column) ? this._hdrRows.getTotalSize() : 0;

            // make sure scrollbars are functional even if we have no rows (TFS 110441)
            if (heightReal < 1) {
                heightReal = 1;
            }

            // keep track of relevant variables
            this._rtl = getComputedStyle(this.hostElement).direction == 'rtl';
            this._heightBrowser = Math.min(heightReal, FlexGrid._getMaxSupportedCssHeight());
            this._maxOffsetY = Math.max(0, heightReal - this._heightBrowser);

            // set sizes that do *not* depend on scrollbars being visible
            if (this._rtl) {
                setCss(this._eTL, { right: 0, top: 0, width: tlw, height: tlh });
                setCss(this._eCHdr, { top: 0, right: tlw, height: tlh });
                setCss(this._eRHdr, { right: 0, top: tlh, width: tlw });
                setCss(this._eCt, { right: tlw, top: tlh, width: this._gpCells.width, height: this._heightBrowser });
            } else {
                setCss(this._eTL, { left: 0, top: 0, width: tlw, height: tlh });
                setCss(this._eCHdr, { top: 0, left: tlw, height: tlh });
                setCss(this._eRHdr, { left: 0, top: tlh, width: tlw });
                setCss(this._eCt, { left: tlw, top: tlh, width: this._gpCells.width, height: this._heightBrowser });
            }

            // update autosizer element
            var rc = this._root.getBoundingClientRect(),
                sbW = rc.width - this._root.clientWidth,
                sbH = rc.height - this._root.clientHeight;
            setCss(this._eSz, {
                width: tlw + sbW + this._gpCells.width,
                height: tlh + sbH + this._heightBrowser
            });

            // update star sizes and re-adjust content width to handle round-offs
            var clientWidth = null;
            if (this.columns._updateStarSizes(this._root.clientWidth - tlw)) {
                clientWidth = this._root.clientWidth;
                this._eCt.style.width = this._gpCells.width + 'px';
            }

            // store control size
            this._szClient = new Size(this._root.clientWidth - tlw, this._root.clientHeight - tlh);
            this._rcBounds = null;

            // refresh content
            this._updateContent(false);

            // update autosizer after refreshing content
            rc = this._root.getBoundingClientRect(),
            sbW = rc.width - this._root.clientWidth,
            sbH = rc.height - this._root.clientHeight;
            setCss(this._eSz, {
                width: tlw + sbW + this._gpCells.width,
                height: tlh + sbH + this._heightBrowser
            });

            // update client size after refreshing content
            this._szClient = new Size(this._root.clientWidth - tlw, this._root.clientHeight - tlh);

            // adjust star sizes for vertical scrollbars
            if (clientWidth && clientWidth != this._root.clientWidth) {
                if (this.columns._updateStarSizes(this._root.clientWidth - tlw)) {
                    this._eCt.style.width = this._gpCells.width + 'px';
                    this._updateContent(false);
                }
            }

            // set sizes that *do* depend on scrollbars being visible
            this._eCHdr.style.width = this._szClient.width + 'px';
            this._eRHdr.style.height = this._szClient.height + 'px';

            // REVIEW: add onLayoutUpdated()?
        }

        // updates the scrollPosition property based on the element's scroll position
        // note that IE/Chrome/FF handle scrollLeft differently under RTL:
        // - Chrome reverses direction,
        // - FF uses negative values, 
        // - IE does the right thing (nothing)
        private _updateScrollPosition() : boolean {
            var root = this._root,
                top = root.scrollTop,
                left = root.scrollLeft;
            if (this._rtl && FlexGrid._getRtlMode() == 'rev') {
                left = (root.scrollWidth - root.clientWidth) - left;
            }
            var pt = new Point(-Math.abs(left), -top);

            // raise scrollPositionChanged
            if (!this._ptScrl.equals(pt)) {
                this._ptScrl= pt;
                this.onScrollPositionChanged();
                return true;
            }

            // no change
            return false;
        }

        // updates the cell elements within this grid.
        private _updateContent(recycle: boolean, cells?: CellRange[]) {
            var self = this,
                focus = this.containsFocus(),
                pct = 1;

            // calculate offset to work around IE limitations
            if (this._heightBrowser > this._szClient.height) {
                pct = Math.round((-this._ptScrl.y) / (this._heightBrowser - this._szClient.height) * 100) / 100;
            }
            this._offsetY = Math.round(this._maxOffsetY * pct);

            // update cells
            if (cells) { // update specific cells (sel change)
                this._gpCells._updateContent(recycle, this._offsetY, cells);
            } else { 

                // update cells first
                this._gpCells._updateContent(recycle, this._offsetY);

                // update visible headers
                if (this._hdrVis & HeadersVisibility.Column) {
                    this._gpCHdr._updateContent(recycle, 0);
                }
                if (this._hdrVis & HeadersVisibility.Row) {
                    this._gpRHdr._updateContent(recycle, this._offsetY);
                }

                // update top/left
                if (this._hdrVis) {
                    this._gpTL._updateContent(recycle, 0);
                }
            }

            // restore focus
            setTimeout(function () { // to work with Android (see HeaderFilters demo)
                if (focus && !self.containsFocus()) {
                    self.focus();
                }
            }, 10);

            // REVIEW: add onViewUpdated()?
        }

        // bind columns
        private _bindColumns() {

            // remove old auto-generated columns
            for (var i = 0; i < this.columns.length; i++) {
                var col = this.columns[i];
                if (col._getFlag(RowColFlags.AutoGenerated)) {
                    this.columns.removeAt(i);
                    i--;
                }
            }

            // get first item to infer data types
            var item = null,
                cv = this.collectionView;
            if (cv && cv.sourceCollection && cv.sourceCollection.length) {
                item = cv.sourceCollection[0];
            }

            // auto-generate new columns
            // (skipping unwanted types: array and object)
            if (item && this.autoGenerateColumns) {
                for (var key in item) {
                    if (isPrimitive(item[key])) {
                        col = new Column();
                        col._setFlag(RowColFlags.AutoGenerated, true);
                        col.binding = col.name = key;
                        col.dataType = getType(item[key]);
                        var pdesc = Object.getOwnPropertyDescriptor(item, key);
                        if (pdesc && !pdesc.writable) {
                            col._setFlag(RowColFlags.ReadOnly, true);
                        }
                        if (col.dataType == DataType.Number) {
                            col.width = 80;
                        }
                        this.columns.push(col);
                    }
                }
            }

            // update missing column types
            if (item) {
                this._updateColumnTypes();
            }

            // REVIEW: add onColumnsCreated()?
        }

        // update missing column types to match data
        private _updateColumnTypes() {
            var cv = this.collectionView;
            if (cv && cv.items && cv.items.length) {
                var item = cv.items[0],
                    col: Column;
                for (var i = 0; i < this.columns.length; i++) {
                    col = this.columns[i];
                    if (col.dataType == null && col.binding) {
                        col.dataType = getType(item[col.binding]);
                    }
                }
            }
        }

        // bind rows
        private _bindRows() {

            // raise loading rows event
            var e = new CancelEventArgs();
            this.onLoadingRows(e);
            if (e.cancel) {
                return;
            }

            // clear rows
            this.rows.clear();

            // re-populate
            if (this.collectionView && this.collectionView.items) {
                var list = this.collectionView.items;
                var groups = this.collectionView.groups;

                // bind to hierarchical sources (childItemsPath)
                if (this.childItemsPath) {
                    for (var i = 0; i < list.length; i++) {
                        this._addTreeNode(list[i], 0)
                    }

                    // bind to grouped sources
                } else if (groups != null && groups.length > 0 && this.showGroups) {
                    for (var i = 0; i < groups.length; i++) {
                        this._addGroup(groups[i]);
                    }

                    // bind to regular sources
                } else {
                    for (var i = 0; i < list.length; i++) {
                        var r = new Row(list[i]);
                        this.rows.push(r);
                    }
                }
            }

            // done binding rows
            this.onLoadedRows(e);
        }
        private _addGroup(g: wijmo.collections.CollectionViewGroup) {

            // add group row
            var gr = new GroupRow();
            gr.level = g.level;
            gr.dataItem = g;
            this.rows.push(gr);

            // add child rows
            if (g.isBottomLevel) {
                for (var i = 0; i < g.items.length; i++) {
                    var r = new Row(g.items[i]);
                    this.rows.push(r);
                }
            } else {
                for (var i = 0; i < g.groups.length; i++) {
                    this._addGroup(g.groups[i]);
                }
            }
        }
        private _addTreeNode(item: any, level: number) {
            var gr = new GroupRow(),
                children = item[this.childItemsPath];

            // add main node
            gr.dataItem = item;
            gr.level = level;
            this.rows.push(gr);

            // add child nodes
            if (children) {
                for (var i = 0; i < children.length; i++) {
                    this._addTreeNode(children[i], level + 1);
                }
            }
        }

        // gets a list of the properties defined by a class and its ancestors
        // that have getters, setters, and whose names don't start with '_'.
        private static _getSerializableProperties(obj: any) {
            var arr = [];

            // travel up class hierarchy saving public properties that can be get/set.
            // NOTE: use getPrototypeOf instead of __proto__ for IE9 compatibility.
            for (obj = obj.prototype; obj; obj = Object.getPrototypeOf(obj)) {
                var names = Object.getOwnPropertyNames(obj);
                for (var i = 0; i < names.length; i++) {
                    var name = names[i],
                        pd = Object.getOwnPropertyDescriptor(obj, name);
                    if (pd && pd.set && pd.get && name[0] != '_') {
                        arr.push(name);
                    }
                }
            }

            // done
            return arr;
        }

        // method used in JSON-style initialization
        _copy(key: string, value: any): boolean {
            if (key == 'columns') {
                var arr = asArray(value);
                for (var i = 0; i < arr.length; i++) {
                    var c = new Column();
                    wijmo.copy(c, arr[i]);
                    this.columns.push(c);
                }
                return true;
            }
            return false;
        }

        // checks whether an element or any of its ancestors contains an attribute
        _hasAttribute(e: any, att: string) {
            for (; e; e = e.parentNode) {
                if (e.getAttribute && e.getAttribute(att) != null) return true;
            }
            return false;
        }

        // get max supported element height (adapted from SlickGrid)
        // IE limits it to about 1.5M, FF to 6M, Chrome to 30M
        private static _maxCssHeight: number;
        private static _getMaxSupportedCssHeight() : number {
            if (!FlexGrid._maxCssHeight) {
                var maxHeight = 1e6,
                    testUpTo = 60e6,
                    div = document.createElement('div');
                div.style.visibility = 'hidden';
                document.body.appendChild(div);
                for (var test = maxHeight; test <= testUpTo; test += 500000) {
                    div.style.height = test + 'px';
                    if (div.offsetHeight != test) {
                        break;
                    }
                    maxHeight = test;
                }
                document.body.removeChild(div);
                FlexGrid._maxCssHeight = maxHeight;
            }
            return FlexGrid._maxCssHeight;
        }

        // IE/Chrome/Safari/FireFox handle RTL differently; this function
        // returns a value that indicates the RTL mode:
        // 'rev': reverse direction so origin is +max, end is zero (Chrome/Safari)
        // 'neg': switches signs, so origin is -max, end is zero (Firefox)
        // 'std': standard, origin is zero, end is +max (IE)
        static _rtlMode: string;
        private static _getRtlMode() : string {
            if (!FlexGrid._rtlMode) {
                var el = wijmo.createElement(
                    '<div dir="rtl" style="visibility:hidden;width:100px;height:100px;overflow:auto">' +
                        '<div style="width:2000px;height:2000px"></div>' +
                    '</div>');

                document.body.appendChild(el);
                var sl = el.scrollLeft;
                el.scrollLeft = -1000;
                var sln = el.scrollLeft;
                document.body.removeChild(el);

                FlexGrid._rtlMode = sln < 0 ? 'neg' : sl > 0 ? 'rev' : 'std';
                //console.log('rtlMode: ' + FlexGrid._rtlMode);
            }
            return FlexGrid._rtlMode;
        }

        //#endregion
    }
}

module wijmo.grid {
    'use strict';

    /**
     * Provides arguments for @see:CellRange events.
     */
    export class CellRangeEventArgs extends CancelEventArgs {
        _panel: GridPanel;
        _rng: CellRange;

        /**
         * Initializes a new instance of a @see:CellRangeEventArgs.
         *
         * @param panel @see:GridPanel that contains the range.
         * @param rng Range of cells affected by the event.
         */
        constructor(panel: GridPanel, rng: CellRange) {
            super();
            this._panel = asType(panel, GridPanel);
            this._rng = asType(rng, CellRange);
        }
        /**
         * Gets the @see:GridPanel affected by this event.
         */
        get panel(): GridPanel {
            return this._panel;
        }
        /**
         * Gets the @see:CellRange affected by this event.
         */
        get cellRange(): CellRange {
            return this._rng.clone();
        }
        /**
         * Gets the row affected by this event.
         */
        get row(): number {
            return this._rng.row;
        }
        /**
         * Gets the column affected by this event.
         */
        get col(): number {
            return this._rng.col;
        }
    }

    /**
     * Provides arguments for the @see:formatItem event.
     */
    export class FormatItemEventArgs extends CellRangeEventArgs {
        _cell: HTMLElement;

        /**
        * Initializes a new instance of a @see:FormatItemEventArgs.
        *
        * @param panel @see:GridPanel that contains the range.
        * @param rng Range of cells affected by the event.
        * @param cell Element that represents the grid cell to be formatted.
        */
        constructor(panel: GridPanel, rng: CellRange, cell: HTMLElement) {
            super(panel, rng);
            this._cell = asType(cell, HTMLElement);
        }
        /**
         * Gets a reference to the element that represents the grid cell to be formatted.
         */
        get cell(): HTMLElement {
            return this._cell;
        }
   }
}
module wijmo.grid {
    'use strict';

    /**
     * Identifies the type of cell in a @see:GridPanel.
     */
    export enum CellType {
        /** Unknown or invalid cell type. */
        None,
        /** Regular data cell. */
        Cell,
        /** Column header cell. */
        ColumnHeader,
        /** Row header cell. */
        RowHeader,
        /** Top-left cell. */
        TopLeft
    }

    /**
     * Represents a logical part of the grid, such as the column headers, row headers,
     * and scrollable data part.
     */
    export class GridPanel {
        private _g: FlexGrid;
        private _ct: CellType;
        private _e: HTMLElement;
        private _rows: RowCollection;
        private _cols: ColumnCollection;
        private _rng: CellRange; // buffered view range
        private _offsetY = 0;

        /**
         * Initializes a new instance of a @see:GridPanel.
         *
         * @param grid The @see:FlexGrid object that owns the panel.
         * @param cellType The type of cell in the panel.
         * @param rows The rows displayed in the panel.
         * @param cols The columns displayed in the panel.
         * @param element The HTMLElement that hosts the cells in the control.
         */
        constructor(grid: FlexGrid, cellType: CellType, rows: RowCollection, cols: ColumnCollection, element: HTMLElement) {
            this._g = asType(grid, FlexGrid);
            this._ct = asInt(cellType);
            this._rows = asType(rows, RowCollection);
            this._cols = asType(cols, ColumnCollection);
            this._e = asType(element, HTMLElement);
            this._rng = new CellRange();
        }
        /**
         * Gets the grid that owns the panel.
         */
        get grid(): FlexGrid {
            return this._g;
        }
        /**
         * Gets the type of cell contained in the panel.
         */
        get cellType(): CellType {
            return this._ct;
        }
        /**
         * Gets a @see:CellRange that indicates the range of cells currently visible on the panel.
         */
        get viewRange(): CellRange {
            return this._getViewRange(false);
        }
        /**
         * Gets the total width of the content in the panel.
         */
        get width(): number {
            return this._cols.getTotalSize();
        }
        /**
         * Gets the total height of the content in this panel.
         */
        get height(): number {
            return this._rows.getTotalSize();
        }
        /**
         * Gets the panel's row collection.
         */
        get rows(): RowCollection {
            return this._rows;
        }
        /**
         * Gets the panel's column collection.
         */
        get columns(): ColumnCollection {
            return this._cols;
        }
        /**
         * Gets the value stored in a cell in the panel.
         *
         * @param r The row index of the cell.
         * @param c The column index of the cell.
         * @param formatted A value indicating whether to format the value for display.
         */
        getCellData(r: number, c: number, formatted: boolean): any {
            var col = <Column>this._cols[c],
                row = <Row>this._rows[r],
                value = null;

            // get bound value from data item using binding
            if (col.binding && row.dataItem && 
                !(row.dataItem instanceof wijmo.collections.CollectionViewGroup)) { // TFS 108841
                value = col._binding.getValue(row.dataItem);
            } else if (row._ubv) { // get unbound value
                value = row._ubv[col._hash];
            }

            // special values for row and column headers, aggregates
            if (value == null) {
                switch (this._ct) {
                    case CellType.ColumnHeader:
                        if (r == this._rows.length - 1 && col.header) {
                            value = col.header;
                        }
                        break;
                    case CellType.Cell:
                        if (col.aggregate != Aggregate.None && row instanceof GroupRow) {
                            var group = <wijmo.collections.CollectionViewGroup>tryCast(row.dataItem, wijmo.collections.CollectionViewGroup);
                            if (group) {
                                value = group.getAggregate(col.aggregate, col.binding, this._g.collectionView);
                            }
                        }
                        break;
                }
            }

            // format value if requested, never return null
            if (formatted) {
                if (this.cellType == CellType.Cell && col.dataMap) {
                    value = col.dataMap.getDisplayValue(value);
                }
                value = value != null ? Globalize.format(value, col.format) : '';
            }

            // done
            return value;
        }
        /**
         * Sets the content of a cell in the panel.
         *
         * @param r The index of the row that contains the cell.
         * @param c The index, name, or binding of the column that contains the cell.
         * @param value The value to store in the cell.
         * @param coerce A value indicating whether to change the value automatically to match the column's data type.
         * @return Returns true if the value is stored successfully, false otherwise (failed cast).
         */
        setCellData(r: number, c: any, value: any, coerce = true): boolean {
            var row = <Row>this._rows[asNumber(r, false, true)],
                col: Column;

            // get column index by name or binding
            if (isString(c)) {
                c = this._cols.indexOf(c);
                if (c < 0) {
                    throw 'Invalid column name or binding.';
                }
            }

            // get column
            col = <Column>this._cols[asNumber(c, false, true)];

            // handle dataMap, coercion, type-checking
            if (this._ct == CellType.Cell) {

                // honor dataMap
                if (col.dataMap && value != null) {
                    if (col.required || (value != '' && value != null)) { // TFS 107058
                        value = col.dataMap.getKeyValue(value);
                        if (value == null) return false; // not in map
                    }
                }

                // get target type
                var targetType = wijmo.DataType.Object;
                if (col.dataType) {
                    targetType = col.dataType;
                } else {
                    var current = this.getCellData(r, c, false);
                    targetType = getType(current);
                }

                // honor 'required' property
                if (isBoolean(col.required)) {
                    if (!col.required && (value === '' || value === null)) {
                        value = null; // setting to null
                        coerce = false;
                    } else if (col.required && (value === '' || value === null)) {
                        return false; // value is required
                    }
                }

                // coerce type if required
                if (coerce) {
                    value = changeType(value, targetType, col.format);
                    if (targetType != DataType.Object && getType(value) != targetType) {
                        return false; // wrong data type
                    }
                }
            }

            // store value
            if (row.dataItem && col.binding) {
                col._binding.setValue(row.dataItem, value);
            } else {
                if (!row._ubv) row._ubv = {};
                row._ubv[col._hash] = value;
            }

            // done
            return true;
        }
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
        getCellBoundingRect(r: number, c: number): Rect {

            // get rect in panel coordinates
            var row = this.rows[r],
                col = this.columns[c],
                rc = new Rect(col.pos, row.pos, col.renderSize, row.renderSize);

            // ajust for rtl
            if (this._g._rtl) {
                rc.left = this.width - rc.left - rc.width;
            }

            // adjust for panel position
            var rcp = this.hostElement.getBoundingClientRect();
            rc.left += rcp.left;
            rc.top += rcp.top;

            // account for frozen rows/columns (TFS 105593)
            if (r < this.rows.frozen) {
                rc.top -= this._g.scrollPosition.y;
            }
            if (c < this.columns.frozen) {
                rc.left -= this._g.scrollPosition.x;
            }

            // done
            return rc;
        }
        /**
         * Gets the host element for the panel.
         */
        get hostElement(): HTMLElement {
            return this._e;
        }

        // ** implementation

        /* -- do not document, this is internal --
         * Gets the Y offset for cells in the panel.
         */
        _getOffsetY(): number {
            return this._offsetY;
        }

        /* -- do not document, this is internal --
         * Updates the cell elements in the panel.
         * @param recycle Whether to recycle existing elements or start from scratch.
         * @param offsetY Scroll position to use when updating the panel.
         * @param cells List of @see:CellRange objects that specify cells that changed state.
         */
        _updateContent(recycle: boolean, offsetY: number, cells?: CellRange[]) {
            var r: number, c: number,
                ctr: number,
                cell: HTMLElement,
                g = this._g,
                rows = this._rows,
                cols = this._cols;

            // scroll headers into position
            if (this._ct == CellType.ColumnHeader || this._ct == CellType.RowHeader) {
                var sp = g._ptScrl,
                    s = this._e.style;
                if (this.cellType == CellType.ColumnHeader) {
                    if (g._rtl) {
                        s.right = sp.x + 'px';
                    } else {
                        s.left = sp.x + 'px';
                    }
                } else if (this.cellType == CellType.RowHeader) {
                    s.top = sp.y + 'px';
                }
            }

            // update offset (and don't recycle if it changed!)
            if (this._offsetY != offsetY) {
                recycle = false;
                this._offsetY = offsetY;
            }

            // calculate view range and cell (buffered) range
            // add buffer while handling touch gestures to improve inertial scrolling
            var rng = this._getViewRange(g.isTouching);

            // done if recycling, no specific cells to refresh, and old range contains new
            // happens a lot while scrolling by small amounts (< 1 cell)
            if (recycle && !cells && this._rng.contains(rng) && !rows.frozen && !cols.frozen) {
                return;
            }

            // if not recycling or if the range changed, ignore 'cells' refresh list
            if (!recycle || !rng.equals(this._rng)) {
                cells = null;
            }

            // clear content if not recycling
            if (!recycle) {
                this._e.textContent = '';
            }

            // reorder cells to optimize scrolling
            if (recycle && this._ct == CellType.Cell && !rows.frozen && !cols.frozen) {
                this._reorderCells(rng, this._rng);
            }

            // save new range
            this._rng = rng;

            // go create/update the cells
            // (render frozen cells last so we don't need z-index!)
            ctr = 0;
            for (r = rng.topRow; r <= rng.bottomRow && r > -1; r++) { // not frozen
                ctr = this._renderRow(r, rng, false, cells, ctr);
            }
            for (r = rng.topRow; r <= rng.bottomRow && r > -1; r++) { // frozen col
                ctr = this._renderRow(r, rng, true, cells, ctr);
            }
            for (r = 0; r < rows.frozen && r < rows.length; r++) { // frozen row
                ctr = this._renderRow(r, rng, false, cells, ctr);
            }
            for (r = 0; r < rows.frozen && r < rows.length; r++) { // frozen row/col
                ctr = this._renderRow(r, rng, true, cells, ctr);
            }

            // remove unused children
            //var cnt = this._e.childElementCount - ctr;
            //for (var i = 0; i < cnt; i++) {
            //    this._e.removeChild(this._e.lastElementChild);
            //}

            // show the cells we are using, hide the others
            var cnt = this._e.childElementCount;
            for (var i = 0; i < cnt; i++) {
                cell = <HTMLElement>this._e.childNodes[i];
                if (i < ctr) {
                    cell.style.display = '';
                } else {
                    cell.style.display = 'none';
                    if (cell.firstElementChild) {
                        cell.textContent = '';
                    }
                }
            }
        }

        // reorder cells within the panel to optimize scrolling performance
        _reorderCells(newRange: CellRange, oldRange: CellRange) {

            // calculate range delta, watch out for bad ranges
            var dr = newRange.row > -1 && oldRange.row > -1
                ? newRange.row - oldRange.row
                : 0;

            // scrolling down by up to 5 lines
            if (dr > 0 && dr <= 5) {
                var row = this._g.rows[newRange.row],
                    newTop = row.pos,
                    frag = document.createDocumentFragment();
                for (;;) {
                    var cell = <HTMLElement>this._e.firstChild;
                    if (!cell) break;
                    var top = parseInt(cell.style.top);
                    if (top >= newTop) break;
                    this._e.removeChild(cell);
                    frag.appendChild(cell);
                }
                this._e.appendChild(frag);
            }

            // scrolling up by up to 5 lines
            if (dr < 0 && dr >= -5) {
                var row = this._g.rows[newRange.row2],
                    newBot = row.pos + row.renderSize,
                    frag = document.createDocumentFragment();
                for (;;) {
                    var cell = <HTMLElement>this._e.lastChild;
                    if (!cell) break;
                    var bot = parseInt(cell.style.top) + parseInt(cell.style.height);
                    if (bot <= newBot) break;
                    this._e.removeChild(cell);
                    if (frag.firstChild) {
                        frag.insertBefore(cell, frag.firstChild);
                    } else { 
                        frag.appendChild(cell);
                    }
                }
                if (this._e.firstChild) {
                    this._e.insertBefore(frag, this._e.firstChild);
                } else {
                    this._e.appendChild(frag);
                }
            }
        }

        // renders a row
        _renderRow(r: number, rng: CellRange, frozen: boolean, cells: CellRange[], ctr: number) : number {

            // skip hidden rows
            if ((<Row>this.rows[r]).renderSize <= 0) {
                return ctr;
            }

            // render each cell in the row
            if (frozen) {
                for (var c = 0; c < this.columns.frozen && c < this.columns.length; c++) {
                    ctr = this._renderCell(r, c, rng, cells, ctr);
                }
            } else {
                for (var c = <number>rng.leftCol; c <= rng.rightCol && c > -1; c++) {
                    ctr = this._renderCell(r, c, rng, cells, ctr);
                }
            }

            // return updated counter
            return ctr;
        }

        // renders a cell
        _renderCell(r: number, c: number, rng: CellRange, cells: CellRange[], ctr: number) : number {

            // skip hidden columns
            if ((<Column>this.columns[c]).renderSize <= 0) {
                return ctr;
            }

            // skip over cells that have been merged over
            var mr = this._g.getMergedRange(this, r, c);
            if (mr) {
                if ((r > rng.topRow && mr.row < r) ||
                    (c > rng.leftCol && mr.col < c)) {
                    return ctr;
                }
            }

            // try recycling a cell
            var cell = <HTMLElement>this._e.childNodes[ctr++];

            // skip if the new cell is not on the refresh list
            if (cell && cells) {
                var contains = false;
                for (var i = 0; i < cells.length && !contains; i++) {
                    contains = mr ? cells[i].intersects(mr) : cells[i].contains(r, c);
                }

                // the cell is on the refresh list, so update the selected state and continue
                // this avoids re-creating templated cells and makes the click event work as usual
                if (contains) {
                    var selState = this._g.getSelectedState(r, c),
                        isGroup = this.rows[r] instanceof GroupRow;
                    toggleClass(cell, 'wj-state-selected', selState == SelectedState.Cursor);
                    toggleClass(cell, 'wj-state-multi-selected', selState == SelectedState.Selected);
                    toggleClass(cell, 'wj-group', selState == SelectedState.None && isGroup);
                }

                // we're done here...
                return ctr;
            }

            // create or recyle cell
            if (!cell) {
                cell = document.createElement('div');
                this._e.appendChild(cell);
            } else {

                // clear cells that have child elements before re-using them
                // this is a workaround for a bug in IE that affects templates
                // strangely, setting the cell's innerHTML to '' doesn't help...
                if (cell.firstElementChild) {
                    cell.textContent = '';
                }
            }

            // set/update cell content/style
            this._g.cellFactory.updateCell(this, r, c, cell, mr);

            // return updated counter
            return ctr;
        }

        // gets the range of cells currently visible, 
        // optionally adding a buffer for inertial scrolling
        _getViewRange(buffer: boolean): CellRange {
            var g = this._g,
                sp = g._ptScrl,
                rows = this._rows,
                cols = this._cols,
                rng = new CellRange(0, 0, rows.length - 1, cols.length - 1);

            // calculate range
            if (this._ct == CellType.Cell || this._ct == CellType.RowHeader) {
                var y = -sp.y + this._offsetY,
                    h = g.clientSize.height + 1,
                    fz = Math.min(rows.frozen, rows.length - 1);

                // account for frozen rows
                if (fz > 0) {
                    var fzs = rows[fz - 1].pos;
                    y += fzs;
                    h -= fzs;
                    buffer = false;
                }

                // add off-screen row buffer (current + 1/2 above + 1/2 below)
                if (buffer) {
                    y -= h / 2;
                    h *= 2;
                }
                rng.row = Math.min(rows.length - 1, Math.max(rows.frozen, rows.getItemAt(y)));
                rng.row2 = rows.getItemAt(y + h);
            }
            if (this._ct == CellType.Cell || this._ct == CellType.ColumnHeader) {
                var x = -sp.x,
                    w = g.clientSize.width + 1,
                    fz = Math.min(cols.frozen, cols.length - 1);

                // account for frozen columns
                if (fz > 0) {
                    var fzs = cols[fz - 1].pos;
                    x += fzs;
                    w -= fzs;
                    buffer = false;
                }

                // add off-screen column buffer (current + 1/2 above + 1/2 below)
                if (buffer) {
                    x -= w / 2;
                    w *= 2;
                }
                rng.col = Math.min(cols.length - 1, Math.max(cols.frozen, cols.getItemAt(x)));
                rng.col2 = cols.getItemAt(x + w);
            }

            // return viewrange
            return rng;
        }

        // gets the point where the frozen area ends
        _getFrozenPos() : Point {
            var fzr = this._rows.frozen,
                fzc = this._cols.frozen,
                fzrow = fzr > 0 ? this._rows[fzr - 1] : null,
                fzcol = fzc > 0 ? this._cols[fzc - 1] : null,
                fzy = fzrow ? fzrow.pos + fzrow.renderSize : 0,
                fzx = fzcol ? fzcol.pos + fzcol.renderSize : 0;
            return new Point(fzx, fzy);
        }
    }
}

module wijmo.grid {
    'use strict';

    /**
     * Creates HTML elements that represent cells within a @see:FlexGrid control.
     */
    export class CellFactory {
        static _WJA_COLLAPSE = 'wj-collapse';
        static _WJA_DROPDOWN = 'wj-dropdown';
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
        public updateCell(panel: GridPanel, r: number, c: number, cell: HTMLElement, rng?: CellRange) {
            var g = panel.grid,
                ct = panel.cellType,
                rows = panel.rows,
                cols = panel.columns,
                row = rows[r],
                col = cols[c],
                r2 = r,
                gr = <GroupRow>tryCast(row, GroupRow),
                nr = tryCast(row, _NewRowTemplate),
                content = panel.getCellData(r, c, true),
                cellWidth = col.renderWidth,
                cellHeight = row.renderHeight,
                cl = 'wj-cell',
                css: any = {};

            // adjust for merged ranges
            if (rng && !rng.isSingleCell) {
                r = rng.row;
                c = rng.col;
                r2 = rng.bottomRow;
                row = rows[r];
                col = cols[c];
                gr = <GroupRow>tryCast(row, GroupRow),
                content = panel.getCellData(r, c, true);
                var sz = rng.getRenderSize(panel);
                cellHeight = sz.height;
                cellWidth = sz.width;
            }

            // get cell position accounting for frozen rows/columns
            var cpos = col.pos,
                rpos = row.pos;
            if (r < rows.frozen) {
                rpos -= g._ptScrl.y;
            }
            if (c < cols.frozen) {
                cpos -= g._ptScrl.x;
            }

            // size and position
            if (g._rtl) {
                css.right = cpos + 'px';
            } else {
                css.left = cpos + 'px';
            }
            css.top = (rpos - panel._getOffsetY()) + 'px';
            css.width = cellWidth + 'px';
            css.height = cellHeight + 'px';

            // background and borders for regular cells and headers
            if (ct == CellType.Cell) {

                // get selected state for painting cells
                var selState = panel.grid.getSelectedState(r, c);

                // paint text editor as regular cell
                if (g.editRange && g.editRange.contains(r, c) && col.dataType != DataType.Boolean) {
                    selState = SelectedState.None;
                }

                // set selected state
                switch (selState) {
                    case SelectedState.Cursor:
                        cl += ' wj-state-selected';
                        break;
                    case SelectedState.Selected:
                        cl += ' wj-state-multi-selected';
                        break;
                    case SelectedState.None:
                        if (gr) {
                            cl += ' wj-group';
                        } else if (r % 2 != 0) {
                            cl += ' wj-alt';
                        }
                        break;
                }

                // mark frozen cells
                if (r < rows.frozen || c < cols.frozen) {
                    cl += ' wj-frozen';
                }

                // new row template?
                if (nr) {
                    cl += ' wj-new';
                }

                // add custom row/column classes if any
                if (row.cssClass) {
                    cl += ' ' + row.cssClass;
                }
                if (col.cssClass) {
                    cl += ' ' + col.cssClass;
                }
            } else {
                cl += ' wj-header';
            }

            // allow text to wrap within the cell
            if (col.wordWrap || row.wordWrap) {
                cl += ' wj-wrap';
            }

            // mark frozen areas
            if (r == rows.frozen - 1) {
                cl += ' wj-frozen-row';
            }
            if (c == cols.frozen - 1) {
                cl += ' wj-frozen-col';
            }

            // alignment
            css.textAlign = col.getAlignment();
            // TODO: vertical alignment?

            // padding
            if (ct == CellType.Cell) {
                css.paddingLeft = css.paddingRight = '';
                if (g.rows.maxGroupLevel > -1 && c == g.columns.firstVisibleIndex) {
                    cell.style.paddingLeft = cell.style.paddingRight = '';
                    var level = gr ? Math.max(0, gr.level) : (g.rows.maxGroupLevel + 1),
                        padding = parseInt(getComputedStyle(cell).paddingLeft), // REVIEW: optimize?
                        indent = g.treeIndent * level + padding + 'px';
                    if (g._rtl) {
                        css.paddingRight = indent;
                    } else {
                        css.paddingLeft = indent;
                    }
                }
            }

            // cell content
            if (ct == CellType.Cell && c == g.columns.firstVisibleIndex &&
                gr && gr.hasChildren && !this._isEditingCell(g, r, c)) {

                // collapse/expand outline
                if (!content) {
                    content = this._getGroupHeader(gr);
                }
                cell.innerHTML = this._getTreeIcon(gr) + ' ' + content;
                css.textAlign = '';

            } else if (ct == CellType.ColumnHeader && col.currentSort && g.showSort && r2 == g._getSortRowIndex()) {

                // add sort class names to allow easier customization
                cl += ' wj-sort-' + (col.currentSort == '+' ? 'asc' : 'desc');

                // column header with sort sign
                cell.innerHTML = escapeHtml(content) + '&nbsp;' + this._getSortIcon(col);

            } else if (ct == CellType.RowHeader && c == g.rowHeaders.columns.length - 1 && !content) {

                // edit/new item template indicators
                var ecv = <wijmo.collections.IEditableCollectionView>g.collectionView,
                    editItem = ecv ? ecv.currentEditItem : null;
                if (editItem && row.dataItem == editItem) {
                    content = '\u270E'; // pencil icon indicates item being edited
                } else if (tryCast(row, _NewRowTemplate)) {
                    content = '*'; // asterisk indicates new row template
                }
                cell.textContent = content;

            } else if (ct == CellType.Cell && col.dataType == DataType.Boolean && !gr) {

                // re-use/create checkbox
                // (re-using allows selecting and checking/unchecking with a single click)
                var chk = cell.children ? <HTMLInputElement>tryCast(cell.children[0], HTMLInputElement): null;
                if (!chk || chk.type != 'checkbox') {
                    cell.innerHTML = '<input type="checkbox"/>';
                    chk = <HTMLInputElement>tryCast(cell.children[0], HTMLInputElement);
                }

                // initialize/update checkbox value
                content = panel.getCellData(r, c, false);
                chk.checked = content == true ? true : false;
                chk.indeterminate = content == null;

                // disable checkbox if it is not editable (so user can't click it)
                chk.disabled = !g._edtHdl._allowEditing(r, c);
                if (chk.disabled) {
                    chk.style.cursor = 'default';
                }

                // assign editor to grid
                if (g.editRange && g.editRange.contains(r, c)) {
                    g._edtHdl._edt = chk;
                }

            } else if (ct == CellType.Cell && this._isEditingCell(g, r, c)) {

                // select input type (important for mobile devices)
                var inpType = col.inputType;
                if (!col.inputType) {
                    inpType = col.dataType == DataType.Number && !col.dataMap ? 'tel' : 'text';
                }

                // create editor
                cell.innerHTML = '<input type="' + inpType + '" class="wj-grid-editor wj-form-control">';

                // initialize editor
                var edt = <HTMLInputElement>cell.children[0];
                edt.value = content;
                edt.style.textAlign = cell.style.textAlign;
                css.padding = '0px'; // no padding on cell div (the editor has it)

                // apply mask, if any
                if (col.mask) {
                    var mp = new _MaskProvider(edt, col.mask);
                }

                // assign editor to grid
                g._edtHdl._edt = edt;

            } else {

                // regular content (textContent is faster than innerHTML)
                if (ct == CellType.Cell && (row.isContentHtml || col.isContentHtml)) {
                    cell.innerHTML = content;
                } else {
                    var fc = cell.firstChild;
                    if (cell.childNodes.length == 1 && fc.nodeType == 3) {
                        if (fc.nodeValue != content) {
                            fc.nodeValue = content; // update text directly in the text node
                        }
                    } else if (fc || content) {
                        cell.textContent = content; // something else, set the textContent
                    }
                }
            }

            // make row/col headers draggable
            switch (ct) {
                case CellType.RowHeader:
                    cell.draggable = !gr && !nr && (g.allowDragging & AllowDragging.Rows) != 0;
                    break;
                case CellType.ColumnHeader:
                    cell.draggable = (g.allowDragging & AllowDragging.Columns) != 0;
                    break;
                default:
                    cell.removeAttribute('draggable');
                    break;
            }

            // add drop-down element to the cell if the column:
            // a) has a dataMap, 
            // b) has showDropDown set to not false (null or true)
            // c) is editable
            if (ct == CellType.Cell && wijmo.input &&
                col.dataMap && col.showDropDown !== false && g._edtHdl._allowEditing(r, c)) {

                // create icon once
                if (!CellFactory._ddIcon) {
                    var ddstyle = 'position:absolute;top:0px;padding:3px 6px;opacity:.25;right:0px';
                    CellFactory._ddIcon = wijmo.createElement('<div style="' + ddstyle + '" ' + CellFactory._WJA_DROPDOWN + '><span class="wj-glyph-down"></span></div>');
                }

                // clone icon and add clone to cell
                var dd = <HTMLElement>CellFactory._ddIcon.cloneNode(true);
                if (g._rtl) {
                    dd.style.left = '0px';
                    dd.style.right = '';
                }
                cell.appendChild(dd);
            }

            // apply class specifier to cell
            if (cell.className != cl) {
                cell.className = cl;
            }

            // apply style to cell
            setCss(cell, css);

            // customize the cell
            if (g.itemFormatter) {
                g.itemFormatter(panel, r, c, cell);
            }
            if (g.formatItem.hasHandlers) {
                var e = new FormatItemEventArgs(panel, new CellRange(r, c), cell);
                g.onFormatItem(e);
            }
        }

        // determines whether the grid is currently editing a cell
        private _isEditingCell(g: FlexGrid, r: number, c: number): boolean {
            return g.editRange && g.editRange.contains(r, c);
        }

        // gets the header text for a group row
        private _getGroupHeader(gr: GroupRow): string {
            var
                grid = gr.grid,
                fmt = grid.groupHeaderFormat ? grid.groupHeaderFormat : wijmo.culture.FlexGrid.groupHeaderFormat,
                group = <wijmo.collections.CollectionViewGroup>tryCast(gr.dataItem, wijmo.collections.CollectionViewGroup);
            if (group && fmt) {

                // get group info
                var propName = group.groupDescription['propertyName'],
                    value = group.name,
                    col = grid.columns.getColumn(propName);

                // customize with column info if possible
                if (col) {
                    if (col.header) {
                        propName = col.header;
                    }
                    if (col.dataMap) {
                        value = col.dataMap.getDisplayValue(value);
                    } else if (col.format) {
                        value = Globalize.format(value, col.format);
                    }
                }

                // build header text
                return format(fmt, {
                    name: escapeHtml(propName),
                    value: escapeHtml(value),
                    level: group.level,
                    count: group.items.length
                });
            }
            return '';
        }

        // get an element to create a collapse/expand icon
        // NOTE: the _WJA_COLLAPSE is used by the mouse handler to identify
        // the collapse/expand button/element.
        private _getTreeIcon(gr: GroupRow): string {

            // get class
            var cls = 'wj-glyph-';
            if (gr.grid._rtl) {
                cls += gr.isCollapsed ? 'left' : 'down-left';
            } else {
                cls += gr.isCollapsed ? 'right' : 'down-right';
            }

            // return span
            return '<span ' + CellFactory._WJA_COLLAPSE + ' class="' + cls +'"></span>&nbsp;';
        }

        // get an element to create a sort up/down icon
        private _getSortIcon(col: Column): string {
            return '<span class="wj-glyph-' + (col.currentSort == '+' ? 'up' : 'down') + '"></span>';
        }
    }
}

module wijmo.grid {
    'use strict';

    /**
     * Represents a rectangular group of cells defined by two row indices and
     * two column indices.
     */
    export class CellRange {
        _row: number;
        _col: number;
        _row2: number;
        _col2: number;

        /**
         * Initializes a new instance of a @see:CellRange.
         *
         * @param r The index of the first row in the range.
         * @param c The index of the first column in the range.
         * @param r2 The index of the last row in the range.
         * @param c2 The index of the first column in the range.
         */
        constructor(r: number = -1, c: number = -1, r2: number = r, c2: number = c) {
            this._row = asInt(r);
            this._col = asInt(c);
            this._row2 = asInt(r2);
            this._col2 = asInt(c2);
        }

        /**
         * Gets or sets the index of the first row in the range.
         */
        get row(): number {
            return this._row;
        }
        set row(value: number) {
            this._row = asInt(value);
        }
        /**
         * Gets or sets the index of the first column in the range.
         */
        get col(): number {
            return this._col;
        }
        set col(value: number) {
            this._col = asInt(value);
        }
        /**
         * Gets or sets the index of the second row in the range.
         */
        get row2(): number {
            return this._row2;
        }
        set row2(value: number) {
            this._row2 = asInt(value);
        }
        /**
         * Gets or sets the index of the second column in the range.
         */
        get col2(): number {
            return this._col2;
        }
        set col2(value: number) {
            this._col2 = asInt(value);
        }
        /**
         * Creates a copy of the range.
         */
        clone(): CellRange {
            return new CellRange(this._row, this._col, this._row2, this._col2);
        }
        /**
         * Gets the number of rows in the range.
         */
        get rowSpan(): number {
            return Math.abs(this._row2 - this._row) + 1;
        }
        /**
         * Gets the number of columns in the range.
         */
        get columnSpan(): number {
            return Math.abs(this._col2 - this._col) + 1;
        }
        /**
         * Gets the index of the top row in the range.
         */
        get topRow(): number {
            return Math.min(this._row, this._row2);
        }
        /**
         * Gets the index of the bottom row in the range.
         */
        get bottomRow(): number {
            return Math.max(this._row, this._row2);
        }
        /**
         * Gets the index of the leftmost column in the range.
         */
        get leftCol(): number {
            return Math.min(this._col, this._col2);
        }
        /**
         * Gets the index of the rightmost column in the range.
         */
        get rightCol(): number {
            return Math.max(this._col, this._col2);
        }
        /**
         * Checks whether the range contains valid row and column indices 
         * (row and column values are zero or greater).
         */
        get isValid(): boolean {
            return this._row > -1 && this._col > -1 && this._row2 > -1 && this._col2 > -1;
        }
        /**
         * Checks whether this range corresponds to a single cell (beginning and ending rows have 
         * the same index, and beginning and ending columns have the same index).
         */
        get isSingleCell(): boolean {
            return this._row == this._row2 && this._col == this._col2;
        }
        /**
         * Checks whether the range contains another range or a specific cell.
         *
         * @param r The CellRange object or row index to find.
         * @param c The column index (required if the r parameter is not a CellRange object).
         */
        contains(r: any, c?: number): boolean {

            // check other cell range
            var rng = <CellRange>tryCast(r, CellRange);
            if (rng) {
                return rng.topRow >= this.topRow && rng.bottomRow <= this.bottomRow &&
                    rng.leftCol >= this.leftCol && rng.rightCol <= this.rightCol;
            }

            // check specific cell
            if (isInt(r) && isInt(c)) {
                return r >= this.topRow && r <= this.bottomRow &&
                       c >= this.leftCol && c <= this.rightCol;
            }

            // anything else is an error
            throw 'contains expects a CellRange or row/column indices.';
        }
        /**
         * Checks whether the range contains a given row.
         *
         * @param r The index of the row to find.
         */
        containsRow(r: number): boolean {
            asInt(r);
            return r >= this.topRow && r <= this.bottomRow;
        }
        /**
         * Checks whether the range contains a given column.
         *
         * @param c The index of the column to find.
         */
        containsColumn(c: number): boolean {
            asInt(c);
            return c >= this.leftCol && c <= this.rightCol;
        }
        /**
         * Checks whether the range intersects another range.
         *
         * @param rng The CellRange object to check.
         */
        intersects(rng: CellRange): boolean {
            if (this.rightCol < rng.leftCol || this.leftCol > rng.rightCol ||
                this.bottomRow < rng.topRow || this.topRow > rng.bottomRow) {
                return false;
            }
            return true;
        }
        /**
         * Gets the rendered size of this range.
         *
         * @param panel The @see:GridPanel object that contains the range.
         * @return A @see:Size object that represents the sum of row heights and column widths in the range.
         */
        getRenderSize(panel: GridPanel): Size {
            var sz = new Size(0, 0);
            for (var r = this.topRow; r <= this.bottomRow; r++) {
                sz.height += panel.rows[r].renderSize;
            }
            for (var c = this.leftCol; c <= this.rightCol; c++) {
                sz.width += panel.columns[c].renderSize;
            }
            return sz;
        }
        /**
         * Checks whether the range equals another range.
         * @param rng The CellRange object to compare to this range.
         */
        equals(rng: CellRange): boolean {
            return (rng instanceof CellRange) &&
                this._row == rng._row && this._col == rng._col &&
                this._row2 == rng._row2 && this._col2 == rng._col2;
        }
    }
}
module wijmo.grid {
    'use strict';

    /**
     * Flags that specify the state of a grid row or column.
     */
    export enum RowColFlags {
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
        RowDefault = Visible | AllowResizing,
        /** Default settings for new columns. */
        ColumnDefault = Visible | AllowDragging | AllowResizing | AllowSorting
    }

    /**
     * An abstract class that serves as a base for the @see:Row and @see:Column classes.
     */
    export class RowCol {
        _sz: number; // null or < 0 means use default
        _cssClass: string;
        _szMin: number;
        _szMax: number;
        _list = null;
        _f: RowColFlags;
        _pos = 0;
        _idx = -1;

        /**
         * Gets or sets a value indicating whether the row or column is visible.
         */
        get visible(): boolean {
            return this._getFlag(RowColFlags.Visible);
        }
        set visible(value: boolean) {
            this._setFlag(RowColFlags.Visible, value);
        }
        /**
         * Gets a value indicating whether the row or column is visible and not collapsed.
         *
         * This property is read-only. To change the visibility of a
         * row or column, use the @see:visible property instead.
         */
        get isVisible(): boolean {
            return this._getFlag(RowColFlags.Visible) && !this._getFlag(RowColFlags.ParentCollapsed);
        }
        /**
         * Gets the position of the row or column.
         */
        get pos(): number {
            if (this._list) this._list._update();
            return this._pos;
        }
        /**
         * Gets the index of the row or column in the parent collection.
         */
        get index(): number {
            if (this._list) this._list._update();
            return this._idx;
        }
        /**
         * Gets or sets the size of the row or column.
         * Setting this property to null or negative values causes the element to use the 
         * parent collection's default size.
         */
        get size(): number {
            return this._sz;
        }
        set size(value: number) {
            if (value != this._sz) {
                this._sz = asNumber(value, true);
                this.onPropertyChanged();
            }
        }
        /**
         * Gets the render size of the row or column.
         * This property accounts for visibility, default size, and min and max sizes.
         */
        get renderSize(): number {
            if (!this.isVisible) {
                return 0;
            }
            var sz = this._sz,
                list = this._list;

            // default size
            if ((sz == null || sz < 0) && list != null) {
                return Math.round((<RowColCollection>(list)).defaultSize);
            }
            
            // min/max
            if (list != null) {
                if (list.minSize != null && sz < list.minSize) {
                    sz = list.minSize;
                }
                if (list.maxSize != null && sz > list.maxSize) {
                    sz = list.maxSize;
                }
            }
            if (this._szMin != null && sz < this._szMin) {
                sz = this._szMin;
            }
            if (this._szMax != null && sz > this._szMax) {
                sz = this._szMax;
            }

            // done
            return Math.round(sz);
        }
        /**
         * Gets or sets a value indicating whether the user can resize the row or column with the mouse.
         */
        get allowResizing(): boolean {
            return this._getFlag(RowColFlags.AllowResizing);
        }
        set allowResizing(value: boolean) {
            this._setFlag(RowColFlags.AllowResizing, value);
        }
        /**
         * Gets or sets a value indicating whether the user can move the row or column to a new position with the mouse.
         */
        get allowDragging(): boolean {
            return this._getFlag(RowColFlags.AllowDragging);
        }
        set allowDragging(value: boolean) {
            this._setFlag(RowColFlags.AllowDragging, value);
        }
        /**
         * Gets or sets a value indicating whether cells in the row or column can be merged.
         */
        get allowMerging(): boolean {
            return this._getFlag(RowColFlags.AllowMerging);
        }
        set allowMerging(value: boolean) {
            this._setFlag(RowColFlags.AllowMerging, value);
        }
        /**
         * Gets or sets a value indicating whether the row or column is selected.
         */
        get isSelected(): boolean {
            return this._getFlag(RowColFlags.Selected);
        }
        set isSelected(value: boolean) {
            this._setFlag(RowColFlags.Selected, value);
        }
        /**
         * Gets or sets a value indicating whether cells in the row or column can be edited.
         */
        get isReadOnly(): boolean {
            return this._getFlag(RowColFlags.ReadOnly);
        }
        set isReadOnly(value: boolean) {
            this._setFlag(RowColFlags.ReadOnly, value);
        }
        /**
         * Gets or sets a value indicating whether cells in the row or column contain HTML content rather than plain text.
         */
        get isContentHtml(): boolean {
            return this._getFlag(RowColFlags.HtmlContent);
        }
        set isContentHtml(value: boolean) {
            if (this.isContentHtml != value) {
                this._setFlag(RowColFlags.HtmlContent, value);
                if (this.grid) {
                    this.grid.invalidate();
                }
            }
        }
        /**
         * Gets or sets a value indicating whether cells in the row or column wrap their content.
         */
        get wordWrap(): boolean {
            return this._getFlag(RowColFlags.WordWrap);
        }
        set wordWrap(value: boolean) {
            this._setFlag(RowColFlags.WordWrap, value);
        }
        /**
         * Gets or sets a CSS class name to use when rendering 
         * non-header cells in the row or column.
         */
        get cssClass(): string {
            return this._cssClass;
        }
        set cssClass(value: string) {
            if (value != this._cssClass) {
                this._cssClass = asString(value);
                if (this.grid) {
                    this.grid.invalidate(false);
                }
            }
        }
        /**
         * Gets the FlexGrid that owns the row or column.
         */
        get grid(): FlexGrid {
            return this._list? (<RowColCollection>this._list)._g: null;
        }
        /**
         * Marks the owner list as dirty and refreshes the owner grid.
         */
        onPropertyChanged() {
            if (this._list) {
                this._list._dirty = true;
                this.grid.invalidate();
            }
        }

        // Gets the value of a flag.
        _getFlag(flag: RowColFlags): boolean {
            return (this._f & flag) != 0;
        }

        // Sets the value of a flag, with optional notification.
        _setFlag(flag: RowColFlags, value: boolean, quiet?: boolean): boolean {
            if (value != this._getFlag(flag)) {
                this._f = value ? (this._f | flag) : (this._f & ~flag);
                if (!quiet) {
                    this.onPropertyChanged();
                }
                return true;
            }
            return false;
        }
    }

    /**
     * Represents a column on the grid.
     */
    export class Column extends RowCol {
        private static _ctr = 0;
        private _hdr: string;
        private _name: string;
        private _type: DataType;
        private _align: string;
        private _map: DataMap;
        private _fmt: string;
        private _agg: Aggregate;
        private _inpType: string;
        private _mask: string;
        private _required: boolean;
        private _showDropDown: boolean;

        /*private*/ _binding: Binding;
        /*private*/ _bindingSort: Binding;
        /*private*/ _szStar: string;
        /*private*/ _hash: string; // unique column id

        /**
         * Initializes a new instance of a @see:Column.
         *
         * @param options Initialization options for the column.
         */
        constructor(options? : any) {
            super();
            this._f = RowColFlags.ColumnDefault;
            this._hash = Column._ctr.toString(36); // unique column key (used for unbound rows)
            Column._ctr++;
            if (options) {
                copy(this, options);
            }
        }
        /**
         * Gets or sets the name of the column.
         *
         * The column name can be used to retrieve the column using the @see:getColumn method.
         */
        get name(): string {
            return this._name;
        }
        set name(value: string) {
            this._name = value;
        }
        /**
         * Gets or sets the type of value stored in the column.
         *
         * Values are coerced into the proper type when editing the grid.
         */
        get dataType(): DataType {
            return this._type;
        }
        set dataType(value: DataType) {
            if (this._type != value) {
                this._type = asEnum(value, DataType);
                if (this.grid) {
                    this.grid.invalidate();
                }
            }
        }
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
        get required(): boolean {
            return this._required;
        }
        set required(value: boolean) {
            this._required = asBoolean(value, true);
        }
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
        get showDropDown(): boolean {
            return this._showDropDown;
        }
        set showDropDown(value: boolean) {
            if (value != this._showDropDown) {
                this._showDropDown = asBoolean(value, true);
                if (this.grid) {
                    this.grid.invalidate();
                }
            }
        }
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
        get inputType(): string {
            return this._inpType;
        }
        set inputType(value: string) {
            this._inpType = asString(value, true);
        }
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
        get mask(): string {
            return this._mask;
        }
        set mask(value: string) {
            this._mask = asString(value, true);
        }
        /**
         * Gets or sets the name of the property the column is bound to.
         */
        get binding(): string {
            return this._binding ? this._binding.path : null;
        }
        set binding(value: string) {
            if (value != this.binding) {
                var path = asString(value);
                this._binding = path ? new Binding(path) : null;
                if (!this._type && this.grid && this._binding) {
                    var cv = this.grid.collectionView;
                    if (cv && cv.sourceCollection && cv.sourceCollection.length) {
                        var item = cv.sourceCollection[0];
                        this._type = getType(this._binding.getValue(item));
                    }
                }
                this.onPropertyChanged();
            }
        }
        /**
         * Gets or sets the name of the property to use when sorting this column.
         *
         * Use this property in cases where you want the sorting to be performed
         * based on values other than the ones speficied by the @see:binding property.
         *
         * Setting this property is null causes the grid to use the value of the
         * @see:binding property to sort the column.
         */
        get sortMemberPath(): string {
            return this._bindingSort ? this._bindingSort.path : null;
        }
        set sortMemberPath(value: string) {
            if (value != this.sortMemberPath) {
                var path = asString(value);
                this._bindingSort = path ? new Binding(path) : null;
                this.onPropertyChanged();
            }
        }
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
        get width() : any {
            if (this._szStar != null) {
                return this._szStar;
            } else {
                return this.size;
            }
        }
        set width(value: any) {
            if (Column._parseStarSize(value) != null) {
                this._szStar = value;
                this.onPropertyChanged();
            } else {
                this._szStar = null;
                this.size = asNumber(value, true);
            }
        }
        /**
         * Gets or sets the minimum width of the column.
         */
        get minWidth(): number {
            return this._szMin;
        }
        set minWidth(value: number) {
            if (value != this._szMin) {
                this._szMin = asNumber(value, true, true);
                this.onPropertyChanged();
            }
        }
        /**
         * Gets or sets the maximum width of the column.
         */
        get maxWidth(): number {
            return this._szMax;
        }
        set maxWidth(value: number) {
            if (value != this._szMax) {
                this._szMax = asNumber(value, true, true);
                this.onPropertyChanged();
            }
        }
        /**
         * Gets the render width of the column.
         *
         * The value returned takes into account the column's visibility, default size, and min and max sizes.
         */
        get renderWidth(): number {
            return this.renderSize;
        }
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
        get align(): string {
            return this._align;
        }
        set align(value: string) {
            if (this._align != value) {
                this._align = value;
                this.onPropertyChanged();
            }
        }
        /**
         * Gets the actual column alignment. 
         *
         * Returns the value of the @see:align property if it is not null, or
         * selects the alignment based on the column's @see:dataType.
         */
        getAlignment(): string {
            var value = this._align;
            if (value == null) {
                value = '';
                if (!this._map) {
                    switch (this._type) {
                        case DataType.Boolean:
                            value = 'center';
                            break;
                        case DataType.Number:
                            value = 'right';
                            break;
                    }
                }
            }
            return value;
        }
        /**
         * Gets or sets the text displayed in the column header.
         */
        get header(): string {
            return this._hdr ? this._hdr : this.binding;
        }
        set header(value: string) {
            if (this._hdr != value) {
                this._hdr = value;
                this.onPropertyChanged();
            }
        }
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
        get dataMap(): DataMap {
            return this._map;
        }
        set dataMap(value: DataMap) {
            if (this._map != value) {

                // disconnect old map
                if (this._map) {
                    this._map.mapChanged.removeHandler(this.onPropertyChanged, this);
                }

                // convert arrays into DataMaps
                if (isArray(value)) {
                    value = new DataMap(value, null, null);
                }

                // set new map
                this._map = asType(value, DataMap, true);

                // connect new map
                if (this._map) {
                    this._map.mapChanged.addHandler(this.onPropertyChanged, this);
                }
                this.onPropertyChanged();
            }
        }
        /**
         * Gets or sets the format string used to convert raw values into display 
         * values for the column (see @see:wijmo.Globalize).
         */
        get format(): string {
            return this._fmt;
        }
        set format(value: string) {
            if (this._fmt != value) {
                this._fmt = value;
                this.onPropertyChanged();
            }
        }
        /**
         * Gets or sets a value indicating whether the user can sort the column by clicking its header.
         */
        get allowSorting(): boolean {
            return this._getFlag(RowColFlags.AllowSorting);
        }
        set allowSorting(value: boolean) {
            this._setFlag(RowColFlags.AllowSorting, value);
        }
        /**
         * Gets a string that describes the current sorting applied to the column.
         * Possible values are '+' for ascending order, '-' for descending order, or 
         * null for unsorted columns.
         */
        get currentSort(): string {
            if (this.grid && this.grid.collectionView && this.grid.collectionView.canSort) {
                var sds = this.grid.collectionView.sortDescriptions;
                for (var i = 0; i < sds.length; i++) {
                    if (sds[i].property == this._getBindingSort()) {
                        return sds[i].ascending ? '+' : '-';
                    }
                }
            }
            return null;
        }
        /**
         * Gets or sets the @see:Aggregate to display in the group header rows 
         * for the column.
         */
        get aggregate(): Aggregate {
            return this._agg != null ? this._agg : Aggregate.None;
        }
        set aggregate(value: Aggregate) {
            if (value != this._agg) {
                this._agg = asEnum(value, Aggregate);
                this.onPropertyChanged();
            }
        }

        // gets the binding used for sorting (sortMemberPath if specified, binding ow)
        private _getBindingSort() : string {
            return this.sortMemberPath ? this.sortMemberPath :
                this.binding ? this.binding :
                null;
        }

        // parses a string in the format '<number>*' and returns the number (or null if the parsing fails).
        static _parseStarSize(value: any) {
            if (isString(value) && value.length > 0 && value[value.length - 1] == '*') {
                var sz = value.length == 1 ? 1 : value.substr(0, value.length - 1) * 1;
                if (sz > 0 && !isNaN(sz)) {
                    return sz;
                }
            }
            return null;
        }
    }

    /**
     * Represents a row in the grid.
     */
    export class Row extends RowCol {
        private _data: any;
        /*private*/ _ubv: any; // unbound value storage

        /**
         * Initializes a new instance of a @see:Row.
         *
         * @param dataItem The data item that this row is bound to.
         */
        constructor(dataItem?: any) {
            super();
            this._f = RowColFlags.ColumnDefault;
            this._data = dataItem;
        }
        /**
         * Gets or sets the item in the data collection that the item is bound to.
         */
        get dataItem(): any {
            return this._data;
        }
        set dataItem(value: any) {
            this._data = value;
        }
        /**
         * Gets or sets the height of the row.
         * Setting this property to null or negative values causes the element to use the 
         * parent collection's default size.
         */
        get height(): number {
            return this.size;
        }
        set height(value: number) {
            this.size = value;
        }
        /**
         * Gets the render height of the row.
         *
         * The value returned takes into account the row's visibility, default size, and min and max sizes.
         */
        get renderHeight(): number {
            return this.renderSize;
        }
    }

    /**
     * Represents a row that serves as a header for a group of rows.
     */
    export class GroupRow extends Row {
        _level = -1;

        /**
         * Initializes a new instance of a @see:GroupRow.
         */
        constructor() {
            super();
            this.isReadOnly = true; // group rows are read-only by default
        }
        /**
         * Gets or sets the hierarchical level of the group associated with the GroupRow.
         */
        get level(): number {
            return this._level;
        }
        set level(value: number) {
            asInt(value);
            if (value != this._level) {
                this._level = value;
                this.onPropertyChanged();
            }
        }
        /**
         * Gets a value that indicates whether the group row has child rows.
         */
        get hasChildren(): boolean {
            var rNext = null,
                gr = null;
            if (this.grid != null && this._list != null) {

                // get the next row
                this._list._update();
                if (this.index < this._list.length - 1) {
                    rNext = this._list[this.index + 1];
                }

                // check if it's a group row
                gr = tryCast(rNext, wijmo.grid.GroupRow);

                // return true if there is a next row and it's a data row or a deeper group row
                return rNext && (gr == null || gr.level > this.level);
            }
            return true;
        }
        /**
         * Gets or sets a value that indicates whether the GroupRow is collapsed 
         * (child rows are hidden) or expanded (child rows are visible).
         */
        get isCollapsed(): boolean {
            return this._getFlag(RowColFlags.Collapsed);
        }
        set isCollapsed(value: boolean) {
            asBoolean(value);
            if (value != this.isCollapsed && this._list != null) {
                this._setCollapsed(value);
            }
        }

        // sets the collapsed/expanded state of a group row
        _setCollapsed(collapsed: boolean) {
            var self = this,
                g = this.grid,
                rows = g.rows,
                rng = this.getCellRange(),
                e = new wijmo.grid.CellRangeEventArgs(g.cells, new wijmo.grid.CellRange(this.index, -1)),
                gr: GroupRow;

            // fire GroupCollapsedChanging
            g.onGroupCollapsedChanging(e);

            // if user canceled, or edits failed, bail out
            if (e.cancel) { // && TODO: grid.FinishEditing()) {
                return;
            }

            // apply new value
            g.deferUpdate(function () {

                // collapse/expand this group
                self._setFlag(RowColFlags.Collapsed, collapsed);
                for (var r = rng.topRow + 1; r <= rng.bottomRow && r > -1 && r < rows.length; r++) {

                    // apply state to this row
                    rows[r]._setFlag(RowColFlags.ParentCollapsed, collapsed);

                    // if this is a group, skip range to preserve the original state
                    gr = tryCast(rows[r], wijmo.grid.GroupRow);
                    if (gr != null && gr.isCollapsed) {
                        r = gr.getCellRange().bottomRow;
                    }
                }
            });

            // fire GroupCollapsedChanged
            g.onGroupCollapsedChanged(e);
        }

        /**
         * Gets a CellRange object that contains all of the rows in the group represented 
         * by the GroupRow and all of the columns in the grid.
         */
        getCellRange(): CellRange {
            var rows = this._list,
                top = this.index,
                bottom = rows.length - 1;
            for (var r = top + 1; r <= bottom; r++) {
                var gr = tryCast(rows[r], wijmo.grid.GroupRow);
                if (gr != null && gr.level <= this.level) {
                    bottom = r - 1;
                    break;
                }
            }
            return new wijmo.grid.CellRange(top, 0, bottom, this.grid.columns.length - 1);
        }
    }

    /**
     * Abstract class that serves as a base for row and column collections.
     */
    export class RowColCollection extends wijmo.collections.ObservableArray {
        _g: FlexGrid;
        _frozen = 0;
        _szDef = 28;
        _szTot = 0;
        _dirty = false;
        _szMin: number;
        _szMax: number;

        /**
         * Initializes a new instance of a @see:_RowColCollection.
         *
         * @param grid The @see:FlexGrid that owns the collection.
         * @param defaultSize The default size of the elements in the collection.
         */
        constructor(grid: FlexGrid, defaultSize: number) {
            super();
            this._g = asType(grid, wijmo.grid.FlexGrid);
            this._szDef = asNumber(defaultSize, false, true);
        }
        /**
         * Gets or sets the default size of elements in the collection.
         */
        get defaultSize(): number {
            return this._szDef;
        }
        set defaultSize(value: number) {
            if (this._szDef != value) {
                this._szDef = asNumber(value, false, true);
                this._dirty = true;
                this._g.invalidate();
            }
        }
        /**
         * Gets or sets the number of frozen rows or columns in the collection.
         *
         * Frozen rows and columns do not scroll, and instead remain at the top or left of
         * the grid, next to the fixed cells. Unlike fixed cells, however, frozen
         * cells may be selected and edited like regular cells.
         */
        get frozen(): number {
            return this._frozen;
        }
        set frozen(value: number) {
            if (value != this._frozen) {
                this._frozen = asNumber(value, false, true);
                this._dirty = true;
                this._g.invalidate();
            }
        }
        /**
         * Checks whether a row or column is frozen.
         *
         * @param index The index of the row or column to check.
         */
        isFrozen(index: number): boolean {
            return index < this.frozen;
        }
        /**
         * Gets or sets the minimum size of elements in the collection.
         */
        get minSize(): number {
            return this._szMin;
        }
        set minSize(value: number) {
            if (value != this._szMin) {
                this._szMin = asNumber(value, true, true);
                this._dirty = true;
                this._g.invalidate();
            }
        }
        /**
         * Gets or sets the maximum size of elements in the collection.
         */
        get maxSize(): number {
            return this._szMax;
        }
        set maxSize(value: number) {
            if (value != this._szMax) {
                this._szMax = asNumber(value, true, true);
                this._dirty = true;
                this._g.invalidate();
            }
        }
        /**
         * Gets the total size of the elements in the collection.
         */
        getTotalSize(): number {
            this._update();
            return this._szTot;
        }
        /**
         * Gets the index of the element at a given physical position.
         * @param position Position of the item in the collection, in pixels.
         */
        getItemAt(position: number): number {

            // update if necessary
            this._update();

            // shortcut for common case
            if (position <= 0 && this.length > 0) {
                return 0;
            }

            // binary search
            // REVIEW: is this worth it? might be better to use a simpler method?
            var min = 0,
                max = this.length - 1,
                cur, item;
            while (min <= max) {
                cur = (min + max) >>> 1;
                item = <RowCol>this[cur];
                if (item._pos > position) {
                    max = cur - 1;
                } else if (item._pos + item.renderSize < position) {
                    min = cur + 1;
                }
                else {
                    return cur;
                }
            }

            // not found, return max
            return max;
        }
        /**
         * Finds the next visible cell for a selection change.
         * @param index Starting index for the search.
         * @param move Type of move (size and direction).
         * @param pageSize Size of a page (in case the move is a page up/down).
         */
        getNextCell(index: number, move: SelMove, pageSize: number) {
            var i, item;
            switch (move) {
                case SelMove.Next:
                    for (i = index + 1; i < this.length; i++) {
                        if (this[i].renderSize > 0) return i;
                    }
                    break;
                case SelMove.Prev:
                    for (i = index - 1; i >= 0; i--) {
                        if (this[i].renderSize > 0) return i;
                    }
                    break;
                case SelMove.End:
                    for (i = this.length - 1; i >= 0; i--) {
                        if (this[i].renderSize > 0) return i;
                    }
                    break;
                case SelMove.Home:
                    for (i = 0; i < this.length; i++) {
                        if (this[i].renderSize > 0) return i;
                    }
                    break;
                case SelMove.NextPage:
                    item = this.getItemAt(this[index].pos + pageSize);
                    return item < 0
                        ? this.getNextCell(index, SelMove.End, pageSize)
                        : item;
                case SelMove.PrevPage:
                    item = this.getItemAt(this[index].pos - pageSize);
                    return item < 0
                        ? this.getNextCell(index, SelMove.Home, pageSize)
                        : item;
            }
            return index;
        }
        /**
         * Checks whether an element can be moved from one position to another.
         *
         * @param src The index of the element to move.
         * @param dst The position to which to move the element, or specify -1 to append the element.
         * @return Returns true if the move is valid, false otherwise.
         */
        canMoveElement(src: number, dst: number): boolean {

            // no move?
            if (dst == src) {
                return false;
            }

            // invalid move?
            if (src < 0 || src >= this.length || dst >= this.length) {
                return false;
            }

            // illegal move?
            if (dst < 0) dst = this.length - 1;
            var start = Math.min(src, dst),
                end = Math.max(src, dst);
            for (var i = start; i <= end; i++) {
                if (!this[i].allowDragging) {
                    return false;
                }
            }

            // can't move anything past the new row template (TFS 109012)
            if (this[dst] instanceof _NewRowTemplate) {
                return false;
            }

            // all seems OK
            return true;
        }
        /**
         * Moves an element from one position to another.
         * @param src Index of the element to move.
         * @param dst Position where the element should be moved to (-1 to append).
         */
        moveElement(src: number, dst: number) {
            if (this.canMoveElement(src, dst)) {
                var e = this[src];
                this.removeAt(src);
                if (dst < 0) dst = this.length;
                this.insert(dst, e);
            }
        }
        /**
         * Keeps track of dirty state and invalidate grid on changes.
         */
        onCollectionChanged(e = wijmo.collections.NotifyCollectionChangedEventArgs.reset) {
            this._dirty = true;
            this._g.invalidate();
            super.onCollectionChanged(e);
        }
        /**
         * Appends an item to the array.
         *
         * @param item Item to add to the array.
         * @return The new length of the array.
         */
        push(item: any): number {
            item._list = this;
            return super.push(item);
        }
        /**
         * Removes or adds items to the array.
         *
         * @param index Position where items are added or removed.
         * @param count Number of items to remove from the array.
         * @param item Item to add to the array.
         * @return An array containing the removed elements.
         */
        splice(index: number, count: number, item?: any): any[]{
            if (item) {
                item._list = this;
            }
            return super.splice(index, count, item);
        }
        /**
         * Suspends notifications until the next call to @see:endUpdate.
         */
        beginUpdate() {

            // make sure we're up-to-date before suspending the updates
            this._update();

            // OK, now it's OK to suspend things
            super.beginUpdate();
        }

        // updates the index, size and position of the elements in the array.
        _update(): boolean {

            // update only if we're dirty *and* if the collection is not in an update block.
            // this is important for performance, especially when expanding/collapsing nodes.
            if (this._dirty && !this.isUpdating) {
                this._dirty = false;
                var
                    pos = 0,
                    rc: RowCol;
                for (var i = 0; i < this.length; i++) {
                    rc = this[i];
                    rc._idx = i;
                    rc._list = this;
                    rc._pos = pos;
                    pos += rc.renderSize;
                }
                this._szTot = pos;
                return true;
            }
            return false;
        }
    }

    /**
     * Represents a collection of @see:Column objects in a @see:FlexGrid control.
     */
    export class ColumnCollection extends RowColCollection {
        _firstVisible = -1;

        /**
         * Gets a column by name or by binding.
         *
         * The method searches the column by name. If a column with the given name 
         * is not found, it searches by binding. The searches are case-sensitive.
         *
         * @param name The name or binding to find.
         * @return The column with the specified name or binding, or null if not found.
         */
        getColumn(name: string): Column {
            var index = this.indexOf(name);
            return index > -1 ? this[index] : null;
        }
        /**
         * Gets the index of a column by name or binding.
         *
         * The method searches the column by name. If a column with the given name 
         * is not found, it searches by binding. The searches are case-sensitive.
         *
         * @param name The name or binding to find.
         * @return The index of column with the specified name or binding, or -1 if not found.
         */
        indexOf(name: any): number {

            // direct lookup
            if (name instanceof Column) {
                return super.indexOf(name);
            }

            // by name
            for (var i = 0; i < this.length; i++) {
                if ((<Column>this[i]).name == name) {
                    return i;
                }
            }

            // by binding
            for (var i = 0; i < this.length; i++) {
                if ((<Column>this[i]).binding == name) {
                    return i;
                }
            }
            return -1;
        }
        /**
         * Gets the index of the first visible column (where the outline tree is displayed).
         */
        get firstVisibleIndex() {
            this._update();
            return this._firstVisible;
        }

        // override to keep track of first visible column (and later to handle star sizes)
        _update(): boolean {
            if (super._update()) {
                this._firstVisible = -1;
                for (var i = 0; i < this.length; i++) {
                    if (<Column>(this[i]).visible) {
                        this._firstVisible = i;
                        break;
                    }
                }
                return true;
            }
            return false;
        }

        // update the width of the columns with star sizes
        _updateStarSizes(szAvailable: number): boolean {
            var starCount = 0,
                col: Column,
                lastStarCol: Column,
                lastWidth: number;

            // count stars, remove fixed size columns from available size
            for (var i = 0; i < this.length; i++) {
                col = this[i];
                if (col.isVisible) {
                    if (col._szStar) {
                        starCount += Column._parseStarSize(col._szStar);
                        lastStarCol = col;
                    } else {
                        szAvailable -= col.renderWidth;
                    }
                }
            }

            // update width of star columns
            if (lastStarCol != null) {
                lastWidth = szAvailable;
                for (var i = 0; i < this.length; i++) {
                    col = this[i];
                    if (col.isVisible) {
                        if (col._szStar) {
                            if (col == lastStarCol) {
                                col._sz = lastWidth; // to avoid round-off errors...
                            } else {
                                col._sz = Math.max(0, Math.round(Column._parseStarSize(col._szStar) / starCount * szAvailable));
                                lastWidth -= col.renderWidth;
                            }
                        }
                    }
                }
                this._dirty = true;
                this._update();
                return true;
            }

            // no star sizes...
            return false;
        }
    }

    /**
     * Represents a collection of @see:Row objects in a @see:FlexGrid control.
     */
    export class RowCollection extends RowColCollection {
        _maxLevel = -1;

        /**
         * Gets the maximum group level in the grid.
         *
         * @return The maximum group level or -1 if the grid has no group rows.
         */
        get maxGroupLevel(): number {
            this._update();
            return this._maxLevel;
        }

       // override to keep track of the maximum group level
        _update(): boolean {
            if (super._update()) {
                this._maxLevel = -1;
                for (var i = 0; i < this.length; i++) {
                    var gr = tryCast(this[i], wijmo.grid.GroupRow);
                    if (gr && gr.level > this._maxLevel) {
                        this._maxLevel = gr.level;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
module wijmo.grid {
    'use strict';

    /**
     * Contains information about the part of a @see:FlexGrid control that exists at 
     * a specified page coordinate.
     */
    export class HitTestInfo {
        _g: FlexGrid;
        _p: GridPanel;
        _pt: Point;
        _row = -1;
        _col = -1;
        _edge = 0; // left, top, right, bottom: 1, 2, 4, 8
        static _EDGESIZE = 5; // distance to cell border
        static _BADANDROID: boolean;
        
        /**
         * Initializes a new instance of a @see:HitTestInfo object.
         *
         * @param grid The @see:FlexGrid control or @see:GridPanel to investigate.
         * @param pt The @see:Point object in page coordinates to investigate.
         */
        constructor(grid: any, pt: any) {

            // check parameters
            if (grid instanceof FlexGrid) {
                this._g = grid;
            } else if (grid instanceof GridPanel) {
                this._p = grid;
                grid = this._g = this._p.grid;
            } else {
                throw 'First parameter should be a FlexGrid or GridPanel.';
            }
            if (isNumber(pt.pageX) && isNumber(pt.pageY)) {
                pt = new Point(pt.pageX, pt.pageY);
                if (this._isBadAndroid()) { // ugh...
                    pt.x -= window.pageXOffset;
                    pt.y -= window.pageYOffset;
                }
            } else if (!(pt instanceof wijmo.Point)) {
                throw 'Second parameter should be a MouseEvent or wijmo.Point.';
            }
            this._pt = pt.clone();

            // get the variables we need
            var rc = grid.controlRect,
                tlp = grid.topLeftCells,
                hdrVis = grid.headersVisibility,
                hdrWid = (hdrVis & HeadersVisibility.Row) ? tlp.columns.getTotalSize() : 0,
                hdrHei = (hdrVis & HeadersVisibility.Column) ? tlp.rows.getTotalSize() : 0,
                sp = grid.scrollPosition;

            // convert page to control coordinates
            pt.x -= rc.left;
            pt.y -= rc.top;

            // account for right to left
            if (this._g._rtl) {
                pt.x = rc.width - pt.x;
            }

            // find out which panel was clicked
            if (this._p == null && pt.x >= 0 && pt.y >= 0 &&
                grid.clientSize && pt.x <= grid.clientSize.width + hdrWid && pt.y <= grid.clientSize.height + hdrHei) {
                if (pt.x <= hdrWid && pt.y <= hdrHei) {
                    this._p = grid.topLeftCells;
                } else if (pt.x <= hdrWid) {
                    this._p = grid.rowHeaders;
                } else if (pt.y <= hdrHei) {
                    this._p = grid.columnHeaders;
                } else {
                    this._p = grid.cells;
                }
            }

            // if we have a panel, get the coordinates
            if (this._p != null) {

                // account for frozen rows/cols
                var rows = this._p.rows,
                    cols = this._p.columns,
                    ct = this._p.cellType,
                    ptFrz = this._p._getFrozenPos();
                if (ct == CellType.Cell || ct == CellType.RowHeader) {
                    pt.y -= hdrHei;
                    if (pt.y > ptFrz.y) {
                        pt.y = pt.y - sp.y + this._p._getOffsetY();
                    }
                }
                if (ct == CellType.Cell || ct == CellType.ColumnHeader) {
                    pt.x -= hdrWid;
                    if (pt.x > ptFrz.x) {
                        pt.x -= sp.x;
                    }
                }

                // get row and column
                this._row = pt.y > rows.getTotalSize() ? -1 : rows.getItemAt(pt.y);
                this._col = pt.x > cols.getTotalSize() ? -1 : cols.getItemAt(pt.x);
                if (this._row < 0 || this._col < 0) {
                    this._p = null;
                    return;
                }

                // get edges
                this._edge = 0;
                var sz = HitTestInfo._EDGESIZE;
                if (this._col > -1) {
                    var col = cols[this._col];
                    if (pt.x - col.pos - pt.x <= sz) this._edge |= 1; // left
                    if (col.pos + col.renderSize - pt.x <= sz) this._edge |= 4; // right
                }
                if (this._row > -1) {
                    var row = rows[this._row];
                    if (pt.y - row.pos - pt.y <= sz) this._edge |= 2; // top
                    if (row.pos + row.renderSize - pt.y <= sz) this._edge |= 8; // bottom
                }
            }
        }
        /**
         * Gets the point in control coordinates that the HitTestInfo refers to.
         */
        get point(): Point {
            return this._pt;
        }
        /**
         * Gets the cell type at the specified position.
         */
        get cellType(): CellType {
            return this._p ? this._p.cellType : grid.CellType.None;
        }
        /**
         * Gets the grid panel at the specified position.
         */
        get gridPanel(): GridPanel {
            return this._p;
        }
        /**
         * Gets the row index of the cell at the specified position.
         */
        get row(): number {
            return this._row;
        }
        /**
         * Gets the column index of the cell at the specified position.
         */
        get col(): number {
            return this._col;
        }
        /**
         * Gets the cell range at the specified position.
         */
        get cellRange(): CellRange {
            return new CellRange(this._row, this._col);
        }
        /**
         * Gets a value indicating whether the mouse is near the left edge of the cell.
         */
        get edgeLeft(): boolean {
            return (this._edge & 1) != 0;
        }
        /**
         * Gets a value indicating whether the mouse is near the top edge of the cell.
         */
        get edgeTop(): boolean {
            return (this._edge & 2) != 0;
        }
        /**
         * Gets a value indicating whether the mouse is near the right edge of the cell.
         */
        get edgeRight(): boolean {
            return (this._edge & 4) != 0;
        }
        /**
         * Gets a value indicating whether the mouse is near the bottom edge of the cell.
         */
        get edgeBottom(): boolean {
            return (this._edge & 8) != 0;
        }

        // checks whether this is a bad version of Android (ughhhh!!!!)
        // e.pageX/Y started reporting wrong values in build 38.0.2125.102...
        // http://stackoverflow.com/questions/26368863/did-android-chrome-pagey-value-change-with-the-latest-chrome-update
        // https://code.google.com/p/chromium/issues/detail?id=423802
        _isBadAndroid(): boolean {
            if (HitTestInfo._BADANDROID == null) {
                HitTestInfo._BADANDROID = false;
                var match = navigator.appVersion.match(/Chrome\/(.*?) /);
                if (match && match[1].indexOf('38.0.2125.') == 0) {
                    HitTestInfo._BADANDROID = match[1].split('.')[3] >= '102';
                }
            }
            return HitTestInfo._BADANDROID;
        }
    }
}
module wijmo.grid {
    'use strict';

    /**
     * Specifies constants that define which areas of the grid support cell merging.
     */
    export enum AllowMerging
    {
        /** No merging. */ 
        None = 0,
        /** Merge scrollable cells. */ 
        Cells = 1,
        /** Merge column headers. */
        ColumnHeaders = 2,
        /** Merge row headers. */
        RowHeaders = 4,
        /** Merge column and row headers. */
        AllHeaders = ColumnHeaders | RowHeaders,
        /** Merge all areas. */
        All = Cells | AllHeaders
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
    export class MergeManager {
        _g: FlexGrid;

        /**
         * Initializes a new instance of a @see:MergeManager object.
         *
         * @param grid The @see:FlexGrid object that owns this @see:MergeManager.
         */
        constructor(grid: FlexGrid) {
            this._g = grid;
        }

        /**
         * Gets a @see:CellRange that specifies the merged extent of a cell
         * in a @see:GridPanel.
         *
         * @param panel The @see:GridPanel that contains the range.
         * @param r The index of the row that contains the cell.
         * @param c The index of the column that contains the cell.
         * @return A @see:CellRange that specifies the merged range, or null if the cell is not merged.
         */
        getMergedRange(panel: GridPanel, r: number, c: number): CellRange {
            var rng: CellRange,
                vr: CellRange,
                ct = panel.cellType,
                cols = panel.columns,
                rows = panel.rows,
                row = rows[r],
                col = cols[c];

            // no merging in new row template (TFS 82235)
            if (row instanceof _NewRowTemplate) {
                return null;
            }

            // merge cells in group rows
            if (row instanceof GroupRow && row.dataItem instanceof wijmo.collections.CollectionViewGroup) {
                rng = new CellRange(r, c);

                // expand left and right preserving aggregates
                if (col.aggregate == Aggregate.None) {
                    while (rng.col > 0 &&
                        cols[rng.col - 1].aggregate == Aggregate.None &&
                        rng.col != cols.frozen) {
                        rng.col--;
                    }
                    while (rng.col2 < cols.length - 1 &&
                        cols[rng.col2 + 1].aggregate == Aggregate.None &&
                        rng.col2 + 1 != cols.frozen) {
                        rng.col2++;
                    }
                }

                // don't start range with invisible columns
                while (rng.col < c && !cols[rng.col].visible) {
                    rng.col++;
                }

                // return merged range
                return rng.isSingleCell ? null : rng; 
            }

            // honor grid's allowMerging setting
            var done = false;
            switch (this._g.allowMerging) {
                case AllowMerging.None:
                    done = true;
                    break;
                case AllowMerging.Cells:
                    done = ct != CellType.Cell;
                    break;
                case AllowMerging.ColumnHeaders:
                    done = ct != CellType.ColumnHeader && ct != CellType.TopLeft;
                    break;
                case AllowMerging.RowHeaders:
                    done = ct != CellType.RowHeader && ct != CellType.TopLeft;
                    break;
                case AllowMerging.AllHeaders:
                    done = ct == CellType.Cell;
                    break;
            }
            if (done) {
                return null;
            }

            // merge up and down columns
            if (cols[c].allowMerging) {
                rng = new CellRange(r, c);

                // clip to current viewport
                var rMin = 0, 
                    rMax = rows.length - 1;
                if (r >= rows.frozen) {
                    if (ct == CellType.Cell || ct == CellType.RowHeader) {
                        vr = panel._getViewRange(true);
                        rMin = vr.topRow;
                        rMax = vr.bottomRow;
                    }
                } else {
                    rMax = rows.frozen - 1;
                }

                // expand up
                var val = panel.getCellData(r, c, true),
                    frz = rows.isFrozen(r);
                for (var tr = r - 1; val != null && tr >= rMin; tr--) {
                    if (rows[tr] instanceof GroupRow ||
                        rows[tr] instanceof _NewRowTemplate ||
                        rows.isFrozen(tr) != frz ||
                        panel.getCellData(tr, c, true) !== val) {
                        break;
                    }
                    rng.row = tr;
                }

                // expand down
                for (var br = r + 1; val != null && br <= rMax; br++) {
                    if (rows[br] instanceof GroupRow ||
                        rows[br] instanceof _NewRowTemplate ||
                        rows.isFrozen(br) != frz ||
                        panel.getCellData(br, c, true) !== val) {
                        break;
                    }
                    rng.row2 = br;
                }

                // don't start range with invisible rows
                while (rng.row < r && !rows[rng.row].visible) {
                    rng.row++;
                }

                // done
                if (!rng.isSingleCell) {
                    return rng;
                }
            }

            // merge left and right along rows
            if (rows[r].allowMerging) {
                rng = new CellRange(r, c);

                // get merging limits
                var cMin = 0, 
                    cMax = cols.length - 1;
                if (c >= cols.frozen) {
                    if (ct == CellType.Cell || ct == CellType.ColumnHeader) {
                        vr = panel._getViewRange(true);
                        cMin = vr.leftCol;
                        cMax = vr.rightCol;
                    }
                } else {
                    cMax = cols.frozen - 1;
                }

                // expand left
                var val = panel.getCellData(r, c, true),
                    frz = cols.isFrozen(c);
                for (var cl = c - 1; cl >= cMin; cl--) {
                    if (cols.isFrozen(cl) != frz|| panel.getCellData(r, cl, true) !== val) {
                        break;
                    }
                    rng.col = cl;
                }

                // expand right
                for (var cr = c + 1; cr <= cMax; cr++) {
                    if (cols.isFrozen(cr) || panel.getCellData(r, cr, true) !== val) {
                        break;
                    }
                    rng.col2 = cr;
                }

                // don't start range with invisible columns
                while (rng.col < c && !cols[rng.col].visible) {
                    rng.col++;
                }

                // done
                if (!rng.isSingleCell) {
                    return rng;
                }
            }

            // no merging...
            return null;
        }
    }
}

module wijmo.grid {
    'use strict';

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
    export class DataMap {
        _cv: wijmo.collections.ICollectionView;
        _keyPath: string;
        _displayPath: string;

        /**
         * Initializes a new instance of a @see:DataMap.
         *
         * @param itemsSource An array or @see:ICollectionView that contains the items to map.
         * @param selectedValuePath The name of the property that contains the keys (data values).
         * @param displayMemberPath The name of the property to use as the visual representation of the items.
         */
        constructor(itemsSource: any, selectedValuePath: string, displayMemberPath: string) {

            // turn arrays into real maps
            if (isArray(itemsSource) && !selectedValuePath && !displayMemberPath) {
                var arr = [];
                for (var i = 0; i < itemsSource.length; i++) {
                    arr.push({ value: itemsSource[i] });
                }
                itemsSource = arr;
                selectedValuePath = 'value';
                displayMemberPath = 'value';
            }

            // initialize map
            this._cv = asCollectionView(itemsSource);
            this._keyPath = asString(selectedValuePath, false);
            this._displayPath = asString(displayMemberPath, false);

            // notify listeners when the map changes
            this._cv.collectionChanged.addHandler(this.onMapChanged, this);
        }
        /**
         * Gets the @see:ICollectionView object that contains the map data.
         */
        get collectionView(): wijmo.collections.ICollectionView {
            return this._cv;
        }
        /**
         * Gets the name of the property to use as a key for the item (data value).
         */
        get selectedValuePath(): string {
            return this._keyPath;
        }
        /**
         * Gets the name of the property to use as the visual representation of the item.
         */
        get displayMemberPath(): string {
            return this._displayPath;
        }
        /**
         * Gets the key that corresponds to a given display value.
         *
         * @param displayValue The display value of the item to retrieve.
         */
        getKeyValue(displayValue: string): any {
            var index = this._indexOf(displayValue, this._displayPath, false);
            return index > -1 ? this._cv.sourceCollection[index][this._keyPath] : null;//displayValue;
        }
        /**
         * Gets the display value that corresponds to a given key.
         *
         * @param key The key of the item to retrieve.
         */
        getDisplayValue(key: any): any {
            var index = this._indexOf(key, this._keyPath, true);
            return index > -1 ? this._cv.sourceCollection[index][this._displayPath]: key;
        }
        /**
         * Gets an array with all of the display values on the map.
         */
        getDisplayValues(): string[]{
            var values = [];
            if (this._cv && this._displayPath) {
                var items = this._cv.sourceCollection;
                for (var i = 0; i < items.length; i++) {
                    values.push(items[i][this._displayPath]);
                }
            }
            return values;
        }
        /**
         * Gets an array with all of the keys on the map.
         */
        getKeyValues(): string[] {
            var values = [];
            if (this._cv && this._keyPath) {
                var items = this._cv.sourceCollection;
                for (var i = 0; i < items.length; i++) {
                    values.push(items[i][this._keyPath]);
                }
            }
            return values;
        }
        /**
         * Occurs when the map data changes.
         */
        mapChanged = new Event();
        /**
         * Raises the @see:mapChanged event.
         */
        onMapChanged() {
            this.mapChanged.raise(this);
        }

        // implementation

        private _indexOf(value: any, path: string, caseSensitive: boolean) {
            if (this._cv && path) {
                if (!caseSensitive) {
                    value = value.toString().toLowerCase();
                }
                var items = this._cv.sourceCollection;
                for (var i = 0; i < items.length; i++) {
                    var item = items[i][path];
                    if (item == value) {
                        return i;
                    }
                    if (!caseSensitive && item.length == value.length && item.toLowerCase() == value) {
                        return i;
                    }
                }
            }
            return -1;
        }
    }

}
module wijmo.grid {
    'use strict';

    /**
     * Specifies constants that define the selection behavior.
     */
    export enum SelectionMode {
        /** The user cannot select cells with the mouse or keyboard. */
        None,
        /** The user can select only a single cell at a time. */
        Cell,
        /** The user can select contiguous blocks of cells. */
        CellRange,
        /** The user can select a single row at a time. */
        Row,
        /** The user can select contiguous rows. */
        RowRange,
        /** The user can select non-contiguous rows. */
        ListBox
    }

    /**
     * Specifies the selected state of a cell.
     */
    export enum SelectedState {
        /** The cell is not selected. */
        None,
        /** The cell is selected but is not the active cell. */
        Selected,
        /** The cell is selected and is the active cell. */
        Cursor,
    }

    /**
     * Specifies a type of movement for the selection.
     */
    export enum SelMove {
        /** Do not change the selection. */
        None,
        /** Select the next visible cell. */
        Next,
        /** Select the previous visible cell. */
        Prev,
        /** Select the first visible cell in the next page. */
        NextPage,
        /** Select the first visible cell in the previous page. */
        PrevPage,
        /** Select the first visible cell. */
        Home,
        /** Select the last visible cell. */
        End,
        /** Select the next visible cell skipping rows if necessary. */
        NextCell,
        /** Select the previous visible cell skipping rows if necessary. */
        PrevCell
    }

    /**
     * Handles the grid's selection.
     */
    export class _SelectionHandler {
        _g: FlexGrid;
        _sel = new CellRange(0, 0);
        _mode = SelectionMode.CellRange;

        /**
         * Initializes a new instance of a @see:_SelectionHandler.
         *
         * @param grid @see:FlexGrid that owns this @see:_SelectionHandler.
         */
        constructor(grid: FlexGrid) {
            this._g = grid;
        }

        /**
         * Gets or sets the current selection mode.
         */
        get selectionMode(): SelectionMode {
            return this._mode;
        }
        set selectionMode(value: SelectionMode) {
            if (value != this._mode) {

                // update listbox selection when switching modes
                if (value == SelectionMode.ListBox || this._mode == SelectionMode.ListBox) {
                    var rows = this._g.rows;
                    for (var i = 0; i < rows.length; i++) {
                        (<Row>rows[i])._setFlag(
                            RowColFlags.Selected,
                            (value == SelectionMode.ListBox) ? this._sel.containsRow(i) : false,
                            false);
                    }
                }

                // apply new mode
                this._mode = value;
                this._g.invalidate();
            }
        }
        /**
         * Gets or sets the current selection.
         */
        get selection(): CellRange {
            return this._sel;
        }
        set selection(value: CellRange) {
            this.select(value);
        }
        /**
         * Gets a @see:SelectedState value that indicates the selected state of a cell.
         *
         * @param r Row index of the cell to inspect.
         * @param c Column index of the cell to inspect.
         */
        getSelectedState(r: number, c: number): SelectedState {

            // disabled selection
            if (this._mode == SelectionMode.None) {
                return SelectedState.None;
            }

            // get current selection
            var sel = this._sel;

            // handle merged ranges
            var g = this._g;
            var rng = g.getMergedRange(g.cells, r, c);
            if (rng) {
                if (rng.contains(sel.row, sel.col)) {
                    return SelectedState.Cursor;
                } else if (rng.intersects(sel)) {
                    return SelectedState.Selected;
                }
            }

            // cursor
            if (sel.row == r && sel.col == c) {
                return SelectedState.Cursor;
            }

            // special cases: row/col selected property
            if (g.rows[r].isSelected || g.columns[c].isSelected) {
                return SelectedState.Selected;
            }

            // adjust for selection mode
            sel = this._adjustSelection(sel);

            // row/column headers (either row or col is -1)
            if ((r < 0 && sel.containsColumn(c)) ||
                (c < 0 && sel.containsRow(r))) {
                return SelectedState.Selected;
            }

            // ListBox mode (already checked for selected rows/cols)
            if (this._mode == SelectionMode.ListBox) {
                return SelectedState.None;
            }

            // regular ranges
            return sel.containsRow(r) && sel.containsColumn(c)
                ? SelectedState.Selected
                : SelectedState.None;
        }
        /**
         * Selects a cell range and optionally scrolls it into view.
         *
         * @param rng Range to select.
         * @param show Whether to scroll the new selection into view.
         */
        select(rng: any, show: any = true) {

            // allow passing in row and column indices
            if (isNumber(rng) && isNumber(show)) {
                rng = new CellRange(<number>rng, <number>show);
                show = true;
            }
            rng = asType(rng, CellRange);

            // get old and new selections
            var g = this._g,
                oldSel = this._sel,
                newSel = rng;

            // adjust for selection mode
            switch (g.selectionMode) {
                case SelectionMode.Cell:
                    rng.row2 = rng.row;
                    rng.col2 = rng.col;
                    break;
                case SelectionMode.Row:
                    rng.row2 = rng.row;
                    break;
                case SelectionMode.ListBox:
                    var rows = g.rows;
                    for (var i = 0; i < rows.length; i++) {
                        (<Row>rows[i])._setFlag(
                            RowColFlags.Selected,
                            newSel.containsRow(i),
                            false);
                    }
                    g.invalidate();
                    break;
            }

            // no change? done
            if (newSel.equals(oldSel)) {
                if (show) {
                    g.scrollIntoView(newSel.row, newSel.col);
                }
                return;
            }

            // raise selectionChanging event
            var e = new CellRangeEventArgs(g.cells, newSel);
            if (!g.onSelectionChanging(e)) {
                return;
            }

            // validate selection after the change
            newSel.row = Math.min(newSel.row, g.rows.length - 1);
            newSel.row2 = Math.min(newSel.row2, g.rows.length - 1);

            // update selection
            this._sel = newSel;

            // show the new selection
            var vr = g.viewRange;
            if (show) {
                g.scrollIntoView(newSel.row, newSel.col);
            }

            // if the viewRange didn't change, we didn't refresh;
            // so do it now in order to update the selection state
            if (vr.equals(g.viewRange)) {
                g.refreshCells(false, true, [
                    this._adjustSelection(oldSel),
                    this._adjustSelection(newSel)
                ]);
            }

            // update collectionView cursor
            if (g.collectionView) {
                var index = g._getCvIndex(newSel.row);
                g.collectionView.moveCurrentToPosition(index);
            }

            // raise selectionChanged event
            g.onSelectionChanged(e);
        }
        /**
         * Moves the selection by a specified amount in the vertical and horizontal directions.
         * @param rowMove How to move the row selection.
         * @param colMove How to move the column selection.
         * @param extend Whether to extend the current selection or start a new one.
         */
        moveSelection(rowMove: SelMove, colMove: SelMove, extend: boolean) {
            var row, col,
                g = this._g,
                rows = g.rows,
                cols = g.columns,
                rng = this._getReferenceCell(rowMove, colMove, extend),
                pageSize = Math.max(0, g.clientSize.height - g.columnHeaders.height);

            // handle next cell with wrapping
            if (colMove == SelMove.NextCell) {
                col = cols.getNextCell(rng.col, SelMove.Next, pageSize);
                row = rng.row;
                if (col == rng.col) {
                    row = rows.getNextCell(row, SelMove.Next, pageSize);
                    if (row > rng.row) {
                        col = cols.getNextCell(0, SelMove.Next, pageSize);
                        col = cols.getNextCell(col, SelMove.Prev, pageSize);
                    }
                }
                g.select(row, col);

            } else if (colMove == SelMove.PrevCell) {

                col = cols.getNextCell(rng.col, SelMove.Prev, pageSize);
                row = rng.row;
                if (col == rng.col) { // reached first column, wrap to previous row
                    row = rows.getNextCell(row, SelMove.Prev, pageSize);
                    if (row < rng.row) {
                        col = cols.getNextCell(cols.length - 1, SelMove.Prev, pageSize);
                        col = cols.getNextCell(col, SelMove.Next, pageSize);
                    }
                }
                g.select(row, col);

            } else {

                // get target row, column
                row = rows.getNextCell(rng.row, rowMove, pageSize);
                col = cols.getNextCell(rng.col, colMove, pageSize);

                // extend or select
                if (extend) {
                    var sel = g.selection;
                    g.select(new CellRange(row, col, sel.row2, sel.col2));
                } else {
                    g.select(row, col);
                }
            }
        }

        // get reference cell for selection change, taking merging into account
        private _getReferenceCell(rowMove: SelMove, colMove: SelMove, extend: boolean): CellRange
        {
            var g = this._g,
                sel = g.selection,
                rng = sel;

            // get merged range
            if (g.mergeManager) {
                rng = g.mergeManager.getMergedRange(g.cells, sel.row, sel.col);
            }

            // no merging? use selection as a reference
            if (!g.mergeManager || !rng || rng.isSingleCell) {
                return sel;
            }

            // pick reference cell from merged range
            switch (rowMove) {
                case SelMove.Next:
                case SelMove.NextCell:
                    rng.row = rng.bottomRow;
                    break;
                case SelMove.None:
                    rng.row = sel.row;
                    break;
            }
            switch (colMove) {
                case SelMove.Next:
                case SelMove.NextCell:
                    rng.col = rng.rightCol;
                    break;
                case SelMove.None:
                    rng.col = sel.col;
                    break;
            }

            // done
            return rng;
        }

        // adjusts a selection to reflect the current selection mode.
        private _adjustSelection(rng: CellRange): CellRange {
            switch (this._mode) {
                case SelectionMode.Cell:
                    return new CellRange(rng.row, rng.col, rng.row, rng.col);
                case SelectionMode.Row:
                    return new CellRange(rng.row, 0, rng.row, this._g.columns.length - 1);
                case SelectionMode.RowRange:
                case SelectionMode.ListBox:
                    return new CellRange(rng.row, 0, rng.row2, this._g.columns.length - 1);
            }
            return rng;
        }
    }
}

module wijmo.grid {
    'use strict';

    /**
     * Handles the grid's keyboard commands.
     */
    export class _KeyboardHandler {
        _g: FlexGrid;

        /**
         * Initializes a new instance of a @see:_KeyboardHandler.
         *
         * @param grid @see:FlexGrid that owns this @see:_KeyboardHandler.
         */
        constructor(grid: FlexGrid) {
            this._g = grid;
            var e = grid.hostElement;
            e.addEventListener('keypress', this._keyPress.bind(this));
            e.addEventListener('keydown', this._keyDown.bind(this));
        }

        // handles the key down event (selection)
        private _keyDown(e: KeyboardEvent) {
            var g = this._g,
                sel = g.selection,
                ctrl = e.ctrlKey || e.metaKey,
                shift = e.shiftKey,
                handled = true;

            if (g.isRangeValid(sel) && !e.defaultPrevented) {

                // pre-process handle keys while editor is active
                if (g.activeEditor && g._edtHdl._keyDown(e)) {
                    return;
                }

                // get the variables we need
                var gr = <GroupRow>tryCast(g.rows[sel.row], GroupRow),
                    ecv = <wijmo.collections.IEditableCollectionView>tryCast(g.collectionView, 'IEditableCollectionView'),
                    keyCode = e.keyCode;

                // handle clipboard
                if (g.autoClipboard) {

                    // copy: ctrl+c or ctrl+Insert
                    if (ctrl && (e.keyCode == 67 || e.keyCode == 45)) {
                        var args = new CellRangeEventArgs(g.cells, sel);
                        if (g.onCopying(args)) {
                            var text = g.getClipString();
                            Clipboard.copy(text);
                            g.onCopied(args);
                        }
                        return;
                    }

                    // paste: ctrl+v or shift+Insert
                    if ((ctrl && e.keyCode == 86) || (shift && e.keyCode == 45)) {
                        if (!g.isReadOnly) {
                            var args = new CellRangeEventArgs(g.cells, sel);
                            if (g.onPasting(args)) {
                                Clipboard.paste(function (text) {
                                    g.setClipString(text);
                                    g.onPasted(args);
                                });
                            }
                        }
                        return;
                    }
                }

                // reverse left/right keys when rendering in right-to-left
                if (g._rtl) { 
                    switch (keyCode) {
                        case Key.Left:
                            keyCode = Key.Right;
                            break;
                        case Key.Right:
                            keyCode = Key.Left;
                            break;
                    }
                }

                // default key handling
                switch (keyCode) {
                    case Key.Left:
                        if (sel.isValid && sel.col == 0 && gr != null && !gr.isCollapsed && gr.hasChildren) {
                            gr.isCollapsed = true;
                        } else {
                            this._moveSel(SelMove.None, ctrl ? SelMove.Home : SelMove.Prev, shift);
                        }
                        break;
                    case Key.Right:
                        if (sel.isValid && sel.col == 0 && gr != null && gr.isCollapsed) {
                            gr.isCollapsed = false;
                        } else {
                            this._moveSel(SelMove.None, ctrl ? SelMove.End : SelMove.Next, shift);
                        }
                        break;
                    case Key.Up:
                        this._moveSel(ctrl ? SelMove.Home : SelMove.Prev, SelMove.None, shift);
                        break;
                    case Key.Down:
                        this._moveSel(ctrl ? SelMove.End : SelMove.Next, SelMove.None, shift);
                        break;
                    case Key.PageUp:
                        this._moveSel(SelMove.PrevPage, SelMove.None, shift);
                        break;
                    case Key.PageDown:
                        this._moveSel(SelMove.NextPage, SelMove.None, shift);
                        break;
                    case Key.Home:
                        this._moveSel(ctrl ? SelMove.Home : SelMove.None, SelMove.Home, shift);
                        break;
                    case Key.End:
                        this._moveSel(ctrl ? SelMove.End : SelMove.None, SelMove.End, shift);
                        break;
                    case Key.Tab:
                        this._moveSel(SelMove.None, shift ? SelMove.PrevCell : SelMove.NextCell, shift);
                        break;
                    case Key.Enter:
                        this._moveSel(SelMove.Next, SelMove.None, shift);
                        if (ecv && ecv.currentEditItem != null) {
                            ecv.commitEdit(); // in case we're at the last row
                        }
                        break;
                    case Key.Escape:
                        if (ecv) {
                            if (ecv.currentEditItem != null) {
                                ecv.cancelEdit();
                            }
                            if (ecv.currentAddItem != null) {
                                ecv.cancelNew();
                            }
                        }
                        g._mouseHdl.resetMouseState();
                        break;
                    case Key.Delete:
                        handled = this._deleteSel();
                        break;
                    case Key.F2:
                        handled = g.startEditing(true);
                        break;
                    case Key.Space:
                        handled = g.startEditing(true);
                        if (handled) {
                            setTimeout(function () {
                                var edt = g.activeEditor;
                                if (edt) {
                                    if (edt.type == 'checkbox') {
                                        edt.checked = !edt.checked;
                                        g.finishEditing();
                                    } else {
                                        var len = edt.value.length;
                                        edt.setSelectionRange(len, len);
                                    }
                                }
                            }, 0);
                        }
                        break;
                    default:
                        handled = false;
                        break;
                }
                if (handled) {
                    e.preventDefault();
                    e.stopPropagation();
                }
            }
        }

        // handles the key press event (start editing or try auto-complete)
        private _keyPress(e) {
            var g = this._g;
            if (g.activeEditor) {
                g._edtHdl._keyPress(e);
            } else if (e.charCode > Key.Space) {
                if (g.startEditing(false) && g.activeEditor) {
                    if (!g.columns[g.editRange.col].mask) { // don't mess up the mask
                        setTimeout(function () {
                            var edt = g.activeEditor;
                            if (edt && edt.type != 'checkbox') {
                                edt.value = String.fromCharCode(e.charCode); // FireFox needs this...
                                edt.setSelectionRange(1, 1);
                                g._edtHdl._keyPress(e); // start auto-complete
                            }
                        }, 0);
                    }
                }
            }
        }

        // move the selection
        private _moveSel(rowMove: SelMove, colMove: SelMove, extend: boolean) {
            if (this._g.selectionMode != SelectionMode.None) {
                this._g._selHdl.moveSelection(rowMove, colMove, extend);
            }
        }

        // delete the selected rows
        private _deleteSel(): boolean {
            var g = this._g,
                ecv = <wijmo.collections.IEditableCollectionView>tryCast(g.collectionView, 'IEditableCollectionView'),
                sel = g.selection,
                rows = g.rows,
                selRows = [];

            // if g.allowDelete and ecv.canRemove, and not editing/adding, (TFS 87718)
            // and the grid allows deleting items, then delete selected rows
            if (g.allowDelete && !g.isReadOnly &&
                (ecv == null || (ecv.canRemove && !ecv.isAddingNew && !ecv.isEditingItem))) {

                // get selected rows
                switch (g.selectionMode) {
                    case SelectionMode.CellRange:
                        if (sel.leftCol == 0 && sel.rightCol == g.columns.length - 1) {
                            for (var i = sel.topRow; i > -1 && i <= sel.bottomRow; i++) {
                                selRows.push(rows[i]);
                            }
                        }
                        break;
                    case SelectionMode.ListBox:
                        for (var i = 0; i < rows.length; i++) {
                            if (rows[i].isSelected) {
                                selRows.push(rows[i]);
                            }
                        }
                        break;
                    case SelectionMode.Row:
                        if (sel.topRow > -1) {
                            selRows.push(rows[sel.topRow]);
                        }
                        break;
                    case SelectionMode.RowRange:
                        for (var i = sel.topRow; i > -1 && i <= sel.bottomRow; i++) {
                            selRows.push(rows[i]);
                        }
                        break;
                }
            }

            // finish with row deletion
            if (selRows.length > 0) {

                // begin updates
                if (ecv) ecv.beginUpdate();
                g.beginUpdate();

                // delete selected rows
                var rng = new CellRange(),
                    e = new CellRangeEventArgs(g.cells, rng);
                for (var i = selRows.length - 1; i >= 0; i--) {
                    var r = selRows[i];
                    rng.row = rng.row2 = r.index;
                    g.onDeletingRow(e);
                    if (!e.cancel) {
                        if (ecv && r.dataItem) {
                            ecv.remove(r.dataItem);
                        } else {
                            g.rows.removeAt(r.index);
                        }
                    }
                }

                // finish updates
                g.endUpdate();
                if (ecv) ecv.endUpdate();

                // make sure one row is selected in ListBox mode (TFS 82683)
                if (g.selectionMode == SelectionMode.ListBox) {
                    var index = g.selection.row;
                    if (index > -1 && index < g.rows.length) {
                        g.rows[index].isSelected = true;
                    }
                }

                // handle childItemsPath (TFS 87577)
                if (g.childItemsPath && g.collectionView) {
                    g.collectionView.refresh();
                }

                // all done
                return true;
            }

            // delete cell content (if there is any) (TFS 94178)
            if (!g.isReadOnly && selRows.length == 0 && sel.isSingleCell) {
                if (g.getCellData(sel.row, sel.col, true) != null) {
                    if (g.startEditing(false, sel.row, sel.col)) {
                        g.finishEditing(true);
                        if (g.setCellData(sel.row, sel.col, '', true)) {
                            g.invalidate();
                            return true;
                        }
                    }
                }
            }

            // no deletion
            return false;
        }
    }
}

module wijmo.grid {
    'use strict';

    /**
     * Specifies constants that define the row/column sizing behavior.
     */
    export enum AllowResizing {
        /** The user may not resize rows or columns. */
        None = 0,
        /** The user may resize columns. */
        Columns = 1,
        /** The user may resize rows. */
        Rows = 2,
        /** The user may resize rows and columns. */
        Both = Rows | Columns
    }

    /**
     * Specifies constants that define the row/column auto-sizing behavior.
     */
    export enum AutoSizeMode {
        /** Autosizing is disabled. */
        None = 0,
        /** Autosizing accounts for header cells. */
        Headers = 1,
        /** Autosizing accounts for data cells. */
        Cells = 2,
        /** Autosizing accounts for header and data cells. */
        Both = Headers | Cells
    }

    /**
     * Specifies constants that define the row/column dragging behavior.
     */
    export enum AllowDragging {
        /** The user may not drag rows or columns. */
        None = 0,
        /** The user may drag columns. */
        Columns = 1,
        /** The user may drag rows. */
        Rows = 2,
        /** The user may drag rows and columns. */
        Both = Rows | Columns
    }

    /**
     * Handles the grid's mouse commands.
     */
    export class _MouseHandler {
        _g: FlexGrid;
        _htDown: HitTestInfo;
        _eMouse: MouseEvent;
        _lbSelState: boolean;
        _lbSelRows: Object;
        _szRowCol: RowCol;
        _szStart: number;
        _szArgs: CellRangeEventArgs;
        _dragSource: any;
        _dvMarker: HTMLElement;
        _rngTarget: CellRange;

        /**
         * Initializes a new instance of a @see:_MouseHandler.
         *
         * @param grid @see:FlexGrid that owns this @see:_MouseHandler.
         */
        constructor(grid: FlexGrid) {
            this._g = grid;
            var e = grid.hostElement,
                self = this;

            // mouse events: 
            // when the user presses the mouse on the control, hook up handlers to 
            // mouse move/up on the *document*, and unhook on mouse up.
            // this simulates a mouse capture (nice idea from ngGrid).
            // note: use 'document' and not 'window'; that doesn't work on Android.
            e.addEventListener('mousedown', function (e: MouseEvent) {
                if (e.button == 0) { // TFS 114623: start actions on left button only
                    document.addEventListener('mousemove', mouseMove);
                    document.addEventListener('mouseup', mouseUp);
                    self._mouseDown(e);
                }
            });
            var mouseMove = function (e: MouseEvent) {
                self._mouseMove(e);
            };
            var mouseUp = function (e: MouseEvent) {
                document.removeEventListener('mousemove', mouseMove);
                document.removeEventListener('mouseup', mouseUp);
                self._mouseUp(e);
            };

            // offer to resize on mousemove (pressing the button not required)
            e.addEventListener('mousemove', this._hover.bind(this));

            // double-click to auto-size rows/columns and to enter edit mode
            e.addEventListener('dblclick', this._dblClick.bind(this));

            // row and column dragging
            e.addEventListener('dragstart', this._dragStart.bind(this));
            e.addEventListener('dragover', this._dragOver.bind(this));
            e.addEventListener('dragleave', this._dragOver.bind(this));
            e.addEventListener('drop', this._drop.bind(this));
            e.addEventListener('dragend', this._dragEnd.bind(this));

            // create target indicator element
            this._dvMarker = createElement('<div class="wj-marker">&nbsp;</div>');
        }

        /**
         * Resets the mouse state.
         */
        resetMouseState() {

            // because dragEnd fires too late in FireFox...
            if (this._dragSource) {
                this._dragSource.style.opacity = 1;
            }
            this._showDragMarker(null);

            this._htDown = null;
            this._lbSelRows = null;
            this._szRowCol = null;
            this._szArgs = null;
            this._dragSource = null;
            this._g.hostElement.style.cursor = 'default';
        }

        // handles the mouse down event
        private _mouseDown(e: MouseEvent) {
            var g = this._g,
                ht = g.hitTest(e);

            // ignore events that have been handled and clicks on unknown areas
            if (e.defaultPrevented || ht.cellType == CellType.None) {
                return;
            }

            // if the user clicked an active editor, let the editor handle things
            if (g.editRange && g.editRange.contains(ht.cellRange)) {
                return;
            }

            // check where the mouse is
            this._htDown = ht;
            this._eMouse = e;

            // unless the target has the focus, give it to the grid (TFS 81949, 102177)
            if (e.target != document.activeElement) {
                g.focus();
            }

            // handle resizing
            if (this._szRowCol != null) {
                this._handleResizing(e);
                return;
            }

            // starting cell selection? special handling for listbox mode
            switch (ht.cellType) {
                case CellType.Cell:
                    if (e.ctrlKey && g.selectionMode == SelectionMode.ListBox) {
                        this._startListBoxSelection(ht.row);
                    }
                    this._mouseSelect(e, e.shiftKey);
                    break;
                case CellType.RowHeader:
                    if ((this._g.allowDragging & AllowDragging.Rows) == 0) {
                        if (e.ctrlKey && g.selectionMode == SelectionMode.ListBox) {
                            this._startListBoxSelection(ht.row);
                        }
                        this._mouseSelect(e, e.shiftKey);
                    }
                    break;
            }

            // handle collapse/expand (after selecting the cell)
            if (ht.cellType == CellType.Cell && ht.col == g.columns.firstVisibleIndex) {
                var gr = <GroupRow>tryCast(g.rows[ht.row], GroupRow);
                if (gr) {
                    var icon = document.elementFromPoint(e.clientX, e.clientY);
                    if (g._hasAttribute(icon, CellFactory._WJA_COLLAPSE)) {
                        if (e.ctrlKey) {

                            // ctrl+click: collapse/expand entire outline to this level
                            g.collapseGroupsToLevel(gr.isCollapsed ? gr.level + 1 : gr.level);

                        } else {

                            // simple click: toggle this group
                            gr.isCollapsed = !gr.isCollapsed;
                        }

                        // done with the mouse
                        this.resetMouseState();
                        e.preventDefault();
                        return;
                    }
                }
            }
        }

        // handles the mouse move event
        private _mouseMove(e: MouseEvent) {
            if (this._htDown != null) {
                this._eMouse = e;
                if (this._szRowCol) {
                    this._handleResizing(e);
                } else {
                    switch (this._htDown.cellType) {
                        case CellType.Cell:
                            this._mouseSelect(e, true);
                            break;
                        case CellType.RowHeader:
                            if ((this._g.allowDragging & AllowDragging.Rows) == 0) {
                                this._mouseSelect(e, true);
                            }
                            break;
                    }
                }
            }
        }

        // handles the mouse up event
        private _mouseUp(e: MouseEvent) {

            // select all cells, finish resizing, sorting
            var htd = this._htDown;
            if (htd && htd.cellType == CellType.TopLeft && htd.row == 0 && htd.col == 0) {
                var g = this._g,
                    ht = g.hitTest(e);
                if (ht.cellType == CellType.TopLeft && ht.row == 0 && ht.col == 0) {
                    g.select(new CellRange(0, 0, g.rows.length - 1, g.columns.length - 1));
                }
            } else if (this._szArgs) {
                this._finishResizing(e);
            } else {
                this._handleSort(e);
            }

            // done with the mouse
            this.resetMouseState();
        }

        // handles double-clicks
        private _dblClick(e: MouseEvent) {
            var g = this._g,
                ht = g.hitTest(e),
                sel = g.selection,
                rng = ht.cellRange,
                args: CellRangeEventArgs;

            // ignore if already handled
            if (e.defaultPrevented) {
                return;
            }

            // auto-size columns
            if (ht.edgeRight && (g.allowResizing & AllowResizing.Columns)) {
                if (ht.cellType == CellType.ColumnHeader) {
                    if (e.ctrlKey && sel.containsColumn(ht.col)) {
                        rng = sel;
                    }
                    for (var c = rng.leftCol; c <= rng.rightCol; c++) {
                        if (g.columns[c].allowResizing) {
                            args = new CellRangeEventArgs(g.cells, new CellRange(-1, c));
                            if (g.onAutoSizingColumn(args) && g.onResizingColumn(args)) {
                                g.autoSizeColumn(c);
                                g.onResizedColumn(args);
                                g.onAutoSizedColumn(args);
                            }
                        }
                    }
                } else if (ht.cellType == CellType.TopLeft) {
                    if (g.topLeftCells.columns[ht.col].allowResizing) {
                        args = new CellRangeEventArgs(g.topLeftCells, new CellRange(-1, ht.col));
                        if (g.onAutoSizingColumn(args) && g.onResizingColumn(args)) {
                            g.autoSizeColumn(ht.col, true);
                            g.onAutoSizedColumn(args);
                            g.onResizedColumn(args);
                        }
                    }
                }
                return;
            }

            // auto-size rows
            if (ht.edgeBottom && (g.allowResizing & AllowResizing.Rows)) {
                if (ht.cellType == CellType.RowHeader) {
                    if (e.ctrlKey && sel.containsRow(ht.row)) {
                        rng = sel;
                    }
                    for (var r = rng.topRow; r <= rng.bottomRow; r++) {
                        if (g.rows[r].allowResizing) {
                            args = new CellRangeEventArgs(g.cells, new CellRange(r, -1));
                            if (g.onAutoSizingRow(args) && g.onResizingRow(args)) {
                                g.autoSizeRow(r);
                                g.onResizedRow(args);
                                g.onAutoSizedRow(args);
                            }
                        }
                    }
                } else if (ht.cellType == CellType.TopLeft) {
                    if (g.topLeftCells.rows[ht.row].allowResizing) {
                        args = new CellRangeEventArgs(g.topLeftCells, new CellRange(ht.row, -1));
                        if (g.onAutoSizingRow(args) && g.onResizingRow(args)) {
                            g.autoSizeRow(ht.row, true);
                            g.onResizedRow(args);
                            g.onAutoSizedRow(args);
                        }
                    }
                }
            }
        }

        // offer to resize rows/columns
        private _hover(e: MouseEvent) {

            // make sure we're hovering
            if (this._htDown == null) {
                var g = this._g,
                    ht = g.hitTest(e),
                    p = ht.gridPanel,
                    cursor = 'default';

                // find which row/column is being resized
                this._szRowCol = null;
                if (ht.cellType == CellType.ColumnHeader || ht.cellType == CellType.TopLeft) {
                    if (g.allowResizing & AllowResizing.Columns) {
                        if (ht.edgeRight && p.columns[ht.col].allowResizing) {
                            this._szRowCol = p.columns[ht.col];
                        }
                    }
                }
                if (ht.cellType == CellType.RowHeader || ht.cellType == CellType.TopLeft) {
                    if (g.allowResizing & AllowResizing.Rows) {
                        if (ht.edgeBottom && p.rows[ht.row].allowResizing) {
                            this._szRowCol = p.rows[ht.row];
                        }
                    }
                }

                // keep track of element to resize and original size
                if (this._szRowCol instanceof Column) {
                    cursor = 'col-resize';
                } else if (this._szRowCol instanceof Row) {
                    cursor = 'row-resize';
                }
                this._szStart = this._szRowCol ? this._szRowCol.renderSize : 0;

                // update the cursor to provide user feedback
                g.hostElement.style.cursor = cursor;
            }
        }

        // handles mouse moves while the button is pressed on the cell area
        private _mouseSelect(e, extend) {
            if (this._htDown && this._htDown.gridPanel && this._g.selectionMode != SelectionMode.None) {
                var g = this._g,
                    ht = new HitTestInfo(this._htDown.gridPanel, e);

                // handle the selection
                e.preventDefault();
                this._handleSelection(ht, extend);

                // keep calling this if the user keeps the mouse outside the control without moving it
                // but don't do this in IE, it can keep scrolling forever... TFS 110374
                if (document.documentMode == null) {
                    ht = new HitTestInfo(g, e);
                    if (ht.cellType != CellType.Cell && ht.cellType != CellType.RowHeader) {
                        var self = this;
                        setTimeout(function () {
                            self._mouseSelect(self._eMouse, extend);
                        }, 200);
                    }
                }
            }
        }

        // handle row and column resizing
        private _handleResizing(e: MouseEvent) {

            // prevent browser from selecting cell content
            e.preventDefault();

            // resizing column
            if (this._szRowCol instanceof Column) {
                var sz = Math.max(1, this._szStart + (e.pageX - this._htDown.point.x) * (this._g._rtl ? -1 : 1))
                if (this._szRowCol.renderSize != sz) {
                    if (this._szArgs == null) {
                        this._szArgs = new CellRangeEventArgs(this._htDown.gridPanel, new CellRange(-1, this._szRowCol.index));
                    }
                    this._g.onResizingColumn(this._szArgs);
                    if (this._g.deferResizing) {
                        this._showResizeMarker(sz);
                    } else {
                        (<Column>this._szRowCol).width = Math.round(sz);
                    }
                }
            }

            // resizing row
            if (this._szRowCol instanceof Row) {
                var sz = Math.max(1, this._szStart + (e.pageY - this._htDown.point.y));
                if (this._szRowCol.renderSize != sz) {
                    if (this._szArgs == null) {
                        this._szArgs = new CellRangeEventArgs(this._htDown.gridPanel, new CellRange(this._szRowCol.index, -1));
                    }
                    this._g.onResizingRow(this._szArgs);
                    if (this._g.deferResizing) {
                        this._showResizeMarker(sz);
                    } else {
                        (<Row>this._szRowCol).height = Math.round(sz);
                    }
                }
            }
        }

        // drag-drop handling (dragging rows/columns)
        private _dragStart(e: DragEvent) {
            var g = this._g,
                ht = this._htDown;

            // make sure this is event is ours
            if (!ht) {
                return;
            }

            // get drag source element (if we're not resizing)
            this._dragSource = null;
            if (!this._szRowCol) {
                var args = new CellRangeEventArgs(g.cells, ht.cellRange);
                if (ht.cellType == CellType.ColumnHeader && (g.allowDragging & AllowDragging.Columns) &&
                    ht.col > -1 && g.columns[ht.col].allowDragging) {
                    if (g.onDraggingColumn(args)) {
                        this._dragSource = e.target;
                    }
                } else if (ht.cellType == CellType.RowHeader && (g.allowDragging & AllowDragging.Rows) &&
                    ht.row > -1 && g.rows[ht.row].allowDragging) {
                    var row = g.rows[ht.row];
                    if (!(row instanceof GroupRow) && !(row instanceof _NewRowTemplate)) {
                        if (g.onDraggingRow(args)) {
                            this._dragSource = e.target;
                        }
                    }
                }
            }

            // if we have a valid source, set opacity; ow prevent dragging
            if (this._dragSource && e.dataTransfer) {
                e.dataTransfer.effectAllowed = 'move';
                e.dataTransfer.setData('text', ''); // required in FireFox (note: text/html will throw in IE!)
                this._dragSource.style.opacity = .5;
            } else {
                e.preventDefault();
            }
        }
        private _dragEnd(e: DragEvent) {
            this.resetMouseState();
        }
        private _dragOver(e: DragEvent) {
            var g = this._g,
                ht = g.hitTest(e),
                valid = false;

            // check whether the move is valid
            if (this._htDown && ht.cellType == this._htDown.cellType) {
                if (ht.cellType == CellType.ColumnHeader) {
                    valid = g.columns.canMoveElement(this._htDown.col, ht.col);
                } else if (ht.cellType == CellType.RowHeader) {
                    valid = g.rows.canMoveElement(this._htDown.row, ht.row);
                }
            }

            // if valid, prevent default to allow drop
            if (valid) {
                e.dataTransfer.dropEffect = 'move';
                e.preventDefault();
                this._showDragMarker(ht);
            } else {
                this._showDragMarker(null);
            }
        }
        private _drop(e: DragEvent) {
            var g = this._g,
                ht = g.hitTest(e),
                args = new CellRangeEventArgs(g.cells, ht.cellRange);

            // move the row/col to a new position
            if (this._htDown && ht.cellType == this._htDown.cellType) {
                var sel = g.selection;
                if (ht.cellType == CellType.ColumnHeader) {
                    g.columns.moveElement(this._htDown.col, ht.col);
                    g.select(sel.row, ht.col);
                    g.onDraggedColumn(args);
                } else if (ht.cellType == CellType.RowHeader) {
                    g.rows.moveElement(this._htDown.row, ht.row);
                    g.select(ht.row, sel.col);
                    g.onDraggedRow(args);
                }
            }
            this.resetMouseState();
        }

        // updates the marker to show the new size of the row/col being resized
        private _showResizeMarker(sz: number) {
            var g = this._g;

            // add marker element to panel
            var t = this._dvMarker;
            if (!t.parentElement) {
                g.cells.hostElement.appendChild(t);
            }

            // update marker position
            var css: any;
            if (this._szRowCol instanceof Column) {
                css = {
                    left: this._szRowCol.pos + sz,
                    top: 0,
                    right: '',
                    bottom: 0,
                    width: 2,
                    height: ''
                }
                if (g._rtl) {
                    css.left = t.parentElement.clientWidth - css.left - css.width;
                }
            } else { 
                css = {
                    left: 0,
                    top: this._szRowCol.pos + sz,
                    right: 0,
                    bottom: '',
                    width: '',
                    height: 2
                }
            }

            // apply new position
            setCss(t, css);
        }

        // updates the marker to show the position where the row/col will be inserted
        private _showDragMarker(ht: HitTestInfo) {
            var g = this._g;

            // remove target indicator if no HitTestInfo
            var t = this._dvMarker;
            if (!ht) {
                if (t.parentElement) {
                    t.parentElement.removeChild(t);
                }
                this._rngTarget = null;
                return;
            }

            // avoid work/flicker
            if (ht.cellRange.equals(this._rngTarget)) {
                return;
            }
            this._rngTarget = ht.cellRange;

            // add marker element to panel
            if (!t.parentElement) {
                ht.gridPanel.hostElement.appendChild(t);
            }

            // update marker position
            var css: any = { 
                left: 0,
                top: 0,
                width: 6,
                height: 6
            };
            switch (ht.cellType) {
                case CellType.ColumnHeader:
                    css.height = ht.gridPanel.height;
                    var col = g.columns[ht.col];
                    css.left = col.pos - css.width / 2;
                    if (ht.col > this._htDown.col) {
                        css.left += col.renderWidth;
                    }
                    if (g._rtl) {
                        css.left = t.parentElement.clientWidth - css.left - css.width;
                    }
                    break;
                case CellType.RowHeader:
                    css.width = ht.gridPanel.width;
                    var row = g.rows[ht.row];
                    css.top = row.pos - css.height / 2;
                    if (ht.row > this._htDown.row) {
                        css.top += row.renderHeight;
                    }
                    break;
            }

            // update marker
            setCss(t, css);
        }

        // raises the ResizedRow/Column events and 
        // applies the new size to the selection if the control key is pressed
        private _finishResizing(e: MouseEvent) {
            var g = this._g,
                sel = g.selection,
                ctrl = this._eMouse.ctrlKey,
                args = this._szArgs,
                rc: number,
                sz;

            // finish row sizing
            if (args && args.row > -1) {

                // apply new size, fire event
                rc = args.row;
                sz = Math.max(1, this._szStart + (e.pageY - this._htDown.point.y));
                g.rows[rc].height = Math.round(sz);
                g.onResizedRow(args);

                // apply new size to selection if the control key is pressed
                if (ctrl && this._htDown.cellType != CellType.TopLeft && sel.containsRow(rc)) {
                    for (var r = sel.topRow; r <= sel.bottomRow; r++) {
                        if (g.rows[r].allowResizing && r != rc) {
                            args = new CellRangeEventArgs(g.cells, new CellRange(r, -1));
                            g.onResizingRow(args);
                            if (!args.cancel) {
                                g.rows[r].size = g.rows[rc].size;
                                g.onResizedRow(args);
                            }
                        }
                    }
                }
            }

            // finish column sizing
            if (args && args.col > -1) {

                // apply new size, fire event
                rc = args.col;
                sz = Math.max(1, this._szStart + (e.pageX - this._htDown.point.x) * (this._g._rtl ? -1 : 1));
                g.columns[rc].width = Math.round(sz);
                g.onResizedColumn(args);

                // apply new size to selection if the control key is pressed
                if (ctrl && this._htDown.cellType != CellType.TopLeft && sel.containsColumn(rc)) {
                    for (var c = sel.leftCol; c <= sel.rightCol; c++) {
                        if (g.columns[c].allowResizing && c != rc) {
                            args = new CellRangeEventArgs(g.cells, new CellRange(-1, c));
                            g.onResizingColumn(args);
                            if (!args.cancel) {
                                g.columns[c].size = g.columns[rc].size;
                                g.onResizedColumn(args);
                            }
                        }
                    }
                }
            }
        }

        // start listbox selection by keeping track of which rows were selected 
        // when the action started
        private _startListBoxSelection(row: number) {
            var rows = this._g.rows;
            this._lbSelState = !rows[row].isSelected;
            this._lbSelRows = {};
            for (var r = 0; r < rows.length; r++) {
                if (rows[r].isSelected) {
                    this._lbSelRows[r] = true;
                }
            }
        }

        // handle mouse selection
        private _handleSelection(ht: HitTestInfo, extend: boolean) {
            var self = this,
                g = this._g,
                rows = g.rows,
                sel = g.selection,
                rng = new CellRange(ht.row, ht.col),
                selected: boolean,
                e: CellRangeEventArgs;

            // check that the selection is valid
            if (ht.row > -1 && ht.col > -1) {
                if (this._lbSelRows != null) {

                    // special handling for listbox mode
                    rng = new CellRange(ht.row, ht.col, self._htDown.row, self._htDown.col);
                    for (var r = 0; r < rows.length; r++) {
                        selected = rng.containsRow(r) ? self._lbSelState : self._lbSelRows[r] != null;
                        if (selected != rows[r].isSelected) {
                            e = new CellRangeEventArgs(g.cells, new CellRange(r, sel.col, r, sel.col2));
                            if (g.onSelectionChanging(e)) {
                                rows[r].isSelected = selected;
                                g.onSelectionChanged(e);
                            }
                        }
                    }
                    g.scrollIntoView(ht.row, ht.col);

                } else {

                    // row headers, select the whole row
                    if (ht.cellType == CellType.RowHeader) {
                        rng.col = 0;
                        rng.col2 = g.columns.length - 1;
                    }
                    
                    // extend range if that was asked
                    if (extend) {
                        rng.row2 = sel.row2;
                        rng.col2 = sel.col2;
                    }

                    // select
                    g.select(rng);
                }
            }
        }

        // handle mouse sort
        private _handleSort(e: MouseEvent) {
            var g = this._g,
                cv = g.collectionView,
                ht = g.hitTest(e);

            if (this._htDown && ht.cellType == this._htDown.cellType && ht.col == this._htDown.col && 
                ht.cellType == CellType.ColumnHeader && !ht.edgeRight && ht.col > -1 &&
                cv && cv.canSort && g.allowSorting) {

                // get row that was clicked accounting for merging
                var rng = g.getMergedRange(g.columnHeaders, ht.row, ht.col),
                    row = rng ? rng.bottomRow : ht.row;
                
                // if the click was on the sort row, sort
                if (row == g._getSortRowIndex()) {
                    var col = g.columns[ht.col],
                        currSort = col.currentSort,
                        asc = currSort != '+';
                    if (col.allowSorting && col.binding) {

                        // can't remove sort from unsorted column
                        if (!currSort && e.ctrlKey) return;

                        // raise sorting column
                        var args = new CellRangeEventArgs(g.columnHeaders, new CellRange(-1, ht.col));
                        if (g.onSortingColumn(args)) {

                            // update sort
                            var sds = cv.sortDescriptions;
                            if (e.ctrlKey) {
                                sds.clear();
                            } else {
                                sds.splice(0, sds.length, new wijmo.collections.SortDescription(col._getBindingSort(), asc));
                            }

                            // raise sorted column
                            g.onSortedColumn(args);
                        }
                    }
                }
            }
        }
    }
}
module wijmo.grid {
    'use strict';

    /**
     * Handles the grid's editing.
     */
    export class _EditHandler {
        _g: FlexGrid;
        _rng: CellRange;
        _edt: HTMLInputElement;
        _lbx: wijmo.input.ListBox;
        _htDown: HitTestInfo;
        _fullEdit = false;
        _list = null;
        _evtInput: any;

        /**
         * Initializes a new instance of an @see:_EditHandler.
         *
         * @param grid @see:FlexGrid that owns this @see:_EditHandler.
         */
        constructor(grid: FlexGrid) {
            this._g = grid;
            var self = this;

            // to raise input event when selecting from ListBox
            // http://stackoverflow.com/questions/2856513/how-can-i-trigger-an-onchange-event-manually
            this._evtInput = document.createEvent('HTMLEvents');
            this._evtInput.initEvent('input', true, false);

            // finish editing when selection changes (commit row edits if row changed)
            grid.selectionChanging.addHandler(function (s, e: CellRangeEventArgs) {
                self.finishEditing();
                if (e.row != grid.selection.row) {
                    self._commitRowEdits();
                }
            });

            // commit row edits when losing focus
            // use blur+capture to emulate focusout (not supported in FireFox)
            grid.hostElement.addEventListener('blur', function () {
                setTimeout(function () {

                    //---------------------------------------------------------------------------------------------
                    // REVIEW: Ionic/Android transfers the focus to the body so
                    // the keyboard disappears no matter what...
                    // http://forum.ionicframework.com/t/the-focus-gets-lost-after-1-or-2-seconds-later/4011/15
                    var edt = grid.activeEditor,
                        ae = document.activeElement;
                    if (edt && edt.type != 'checkbox' && ae && ae.tagName == 'BODY') {
                        edt.setSelectionRange(0, edt.value.length);
                        return;
                    }
                    // end of Ionic/Android workaround
                    //---------------------------------------------------------------------------------------------

                    // if the grid lost focus, commit row edits (TFS 114960)
                    var hasFocus = grid.containsFocus() || (self._lbx && self._lbx.containsFocus());
                    if (!hasFocus) {
                        self._commitRowEdits();
                    }
                }, 0); // TFS 100250, 112599 (timeOut used to be 200)
            }, true);

            // commit edits when clicking non-cells (e.g. sort, drag, resize)
            // start editing when clicking on checkboxes
            grid.hostElement.addEventListener('mousedown', function (e) {
                var sel = grid.selection,
                    ht = grid.hitTest(e);

                self._htDown = null;
                if (ht.cellType != CellType.Cell && ht.cellType != CellType.None) {

                    // mouse down on non-cell area: commit any pending edits
                    // **REVIEW: this is a fix for TFS 98332
                    if (!self._lbx || !contains(self._lbx.hostElement, <HTMLElement>e.target)) {
                        self._commitRowEdits();
                    }

                } else if (ht.cellType != CellType.None) {

                    // start editing when clicking on checkboxes that are not the active editor
                    var edt = <HTMLInputElement>tryCast(e.target, HTMLInputElement);
                    if (edt && edt.type == 'checkbox' && hasClass(edt.parentElement, 'wj-cell')) {
                        if (edt != self.activeEditor) {

                            // we're handling this (required in Chrome)
                            e.preventDefault();
                            e.stopPropagation();

                            // start editing the item that was clicked
                            self.startEditing(false, ht.row, ht.col);

                            // toggle check after editing started
                            setTimeout(function () {
                                edt = self.activeEditor;
                                if (edt && edt.type == 'checkbox') {
                                    edt.checked = !edt.checked;
                                    self.finishEditing();
                                }
                            }, 0);
                        } else {
                            self.finishEditing();
                        }
                    }

                    // handle drop-down items (even on editors)
                    var icon = document.elementFromPoint(e.clientX, e.clientY);
                    if (grid._hasAttribute(icon, CellFactory._WJA_DROPDOWN)) {
                        self._toggleDropDown(ht);
                        self._htDown = null;
                        e.preventDefault();
                        return;
                    }

                    // if the click was on the cursor cell, save the hit test info
                    // to start editing when we get the click event later 
                    if (edt == null && ht.row == sel.row && ht.col == sel.col) {
                        self._htDown = ht;
                    }
                }
            }, true); // << 'capture' so we get the event before the checkbox!!!

            // start editing when the user clicks the selected cell
            grid.hostElement.addEventListener('click', function (e) {
                if (self._htDown && !self.activeEditor) {
                    var ht = grid.hitTest(e);
                    if (ht.cellRange.equals(self._htDown.cellRange)) {
                        self.startEditing(true, ht.row, ht.col);
                    }
                }
            });
        }
        /**
         * Starts editing a given cell.
         *
         * @param fullEdit Whether to stay in edit mode when the user presses the cursor keys. Defaults to false.
         * @param r Index of the row to be edited. Defaults to the currently selected row.
         * @param c Index of the column to be edited. Defaults to the currently selected column.
         * @param focus Whether to give the editor the focus. Defaults to true.
         * @return True if the edit operation started successfully.
         */
        startEditing(fullEdit = true, r?: number, c?: number, focus?: boolean): boolean {

            // default row/col to current selection
            var g = this._g;
            r = asNumber(r, true, true);
            c = asNumber(c, true, true);
            if (r == null) {
                r = g.selection.row;
            }
            if (c == null) {
                c = g.selection.col;
            }

            // default focus to true
            if (focus == null) {
                focus = true;
            }

            // check that the cell is editable
            if (!this._allowEditing(r, c)) {
                return false;
            }

            // get edit range
            var rng = g.getMergedRange(g.cells, r, c);
            if (!rng) {
                rng = new CellRange(r, c);
            }

            // get item to be edited
            var item = g.rows[r].dataItem;

            // make sure cell is selected
            g.select(rng, true);

            // check that we still have the same item after moving the selection (TFS 110143)
            if (!g.rows[r] || item != g.rows[r].dataItem) {
                return false;
            }

            // no work if we are already editing this cell
            if (rng.equals(this._rng)) {
                return true;
            }

            // start editing cell
            var e = new CellRangeEventArgs(g.cells, rng);
            if (!g.onBeginningEdit(e)) {
                return false;
            }

            // start editing item
            var ecv = tryCast(g.collectionView, 'IEditableCollectionView');
            if (ecv) {
                item = g.rows[r].dataItem;
                ecv.editItem(item);
            }

            // save editing parameters
            this._fullEdit = fullEdit;
            this._rng = rng;
            this._list = null;
            var map = <DataMap>g.columns[c].dataMap;
            if (map) {
                this._list = map.getDisplayValues();
            }

            // refresh to create and activate editor
            g.refresh(false);
            var edt = this._edt;
            if (edt) {
                if (edt.type == 'checkbox') {
                    this._fullEdit = false; // no full edit on checkboxes...
                } else if (focus) {
                    edt.setSelectionRange(0, edt.value.length);
                }
                g.onPrepareCellForEdit(e);

                // give the editor the focus in case it doesn't have it
                // NOTE: this happens on Android, it's strange...
                edt = this._edt;
                if (edt && focus) {
                    edt.focus();
                }
            }

            // done
            return true;
        }
        /* 
         * Commits any pending edits and exits edit mode.
         *
         * @param cancel Whether pending edits should be canceled or committed.
         * @return True if the edit operation finished successfully.
         */
        finishEditing(cancel = false): boolean {

            // remember if we have the focus
            var focus = this._g.containsFocus();

            // always get rid of drop-down
            this._removeListBox();

            // make sure we're editing
            var edt = this._edt;
            if (!edt) {
                return true;
            }

            // get parameters
            var g = this._g,
                rng = this._rng,
                e = new CellRangeEventArgs(g.cells, rng);

            // edit ending
            e.cancel = cancel;
            g.onCellEditEnding(e);

            // apply edits
            if (!e.cancel) {
                var value: any = edt.type == 'checkbox' ? edt.checked : edt.value;
                for (var r = rng.topRow; r <= rng.bottomRow && r < g.rows.length; r++) {
                    for (var c = rng.leftCol; c <= rng.rightCol && c < g.columns.length; c++) {
                        g.cells.setCellData(r, c, value, true);
                    }
                }
            }

            // dispose of editor
            this._edt = null;
            this._rng = null;
            this._list = null;
            g.refresh(false);

            // restore focus // TFS 107464
            if (focus && !this._g.containsFocus()) {
                this._g.focus();
            }

            // edit ended
            g.onCellEditEnded(e);

            // done
            return true;
        }
        /*
         * Gets the <b>HTMLInputElement</b> that represents the cell editor currently active.
         */
        get activeEditor(): HTMLInputElement {
            return this._edt;
        }
        /*
         * Gets a @see:CellRange that identifies the cell currently being edited.
         */
        get editRange(): CellRange {
            return this._rng;
        }

        // ** implementation

        // checks whether a cell can be edited
        _allowEditing(r: number, c: number) {
            var g = this._g;
            if (g.isReadOnly || g.selectionMode == SelectionMode.None) return false;
            if (r < 0 || r >= g.rows.length || g.rows[r].isReadOnly || !g.rows[r].isVisible) return false;
            if (c < 0 || c >= g.columns.length || g.columns[c].isReadOnly || !g.columns[c].isVisible) return false;
            return true;
        }

        // finish editing the current item
        private _commitRowEdits() {
            this.finishEditing();
            var grid = this._g,
                ecv = tryCast(grid.collectionView, 'IEditableCollectionView');
            if (ecv && ecv.currentEditItem) {
                var e = new CellRangeEventArgs(grid.cells, grid.selection);
                grid.onRowEditEnding(e);
                ecv.commitEdit();
                grid.onRowEditEnded(e);
            }
        }

        // handles keyDown events while editing
        // returns true if the key was handled, false if the grid should handle it
        _keyDown(e): boolean {
            switch (e.keyCode) {

                // F2 toggles edit mode
                case Key.F2:
                    this._fullEdit = !this._fullEdit;
                    e.preventDefault();
                    return true;

                // space toggles checkboxes
                case Key.Space:
                    var edt = this._edt;
                    if (edt && edt.type == 'checkbox') {
                        edt.checked = !edt.checked;
                        this.finishEditing();
                        e.preventDefault();
                    }
                    return true;

                // enter, tab, escape finish editing
                case Key.Enter:
                case Key.Tab:
                    this.finishEditing();
                    return false;
                case Key.Escape:
                    this.finishEditing(true);
                    return true;

                // cursor keys finish editing if not in full edit mode
                case Key.Up:
                case Key.Down:
                case Key.Left:
                case Key.Right:
                case Key.PageUp:
                case Key.PageDown:
                case Key.Home:
                case Key.End:
                    if (!this._fullEdit) {
                        this.finishEditing();
                        return false;
                    }
            }

            // return true to let editor handle the key (not the grid)
            return true;
        }

        // handles keyPress events while editing
        _keyPress(e) {

            // auto-complete based on datamap
            var edt = this._edt;
            if (edt && edt.type != 'checkbox' && e.target == edt &&
                this._list && this._list.length > 0 && e.charCode >= 32) {

                // get text up to selection start
                var start = edt.selectionStart;
                var text = edt.value;
                text = text.substr(0, start);

                // add the new char if the source element is the editor
                // (but not if the source element is the grid!)
                if (e.target == edt) {
                    start++;
                    text += String.fromCharCode(e.charCode);
                }

                // convert to lower-case for matching
                text = text.toLowerCase();

                // look for a match
                for (var i = 0; i < this._list.length; i++) {
                    if (this._list[i].toLowerCase().indexOf(text) == 0) {

                        // found the match, update text and selection
                        edt.value = this._list[i];
                        edt.setSelectionRange(start, this._list[i].length);
                        edt.dispatchEvent(this._evtInput);

                        // eat the key and be done
                        e.preventDefault();
                        break;
                    }
                }
            }
        }

        // shows the drop-down element for a cell (if it is not already visible)
        _toggleDropDown(ht: HitTestInfo) {
            var self = this,
                g = this._g;

            // close select element if any;
            // if this is the same cell, we're done
            if (this._lbx) {
                this._removeListBox();
                if (g.selection.contains(ht.cellRange)) {
                    if (g.activeEditor) {
                        g.activeEditor.focus();
                    } else if (!g.containsFocus()) {
                        g.focus();
                    }
                    return;
                }
            }

            // if this was a touch, give focus to ListBox to hide soft keyboard
            var lbxFocus = g.isTouching;

            // start editing so we can position the select element
            if (!wijmo.input || !this.startEditing(true, ht.row, ht.col, !lbxFocus)) {
                return;
            }

            // create and initialize the ListBox
            this._lbx = this._createListBox();
            this._lbx.showSelection();
            if (lbxFocus) {
                this._lbx.focus();
            }

            // attach event handlers
            this._lbx.selectedIndexChanged.addHandler(function () {
                var edt = g.activeEditor;
                if (edt) {
                    edt.value = self._lbx.selectedValue;
                    edt.setSelectionRange(0, edt.value.length);
                    edt.dispatchEvent(self._evtInput);
                    self.finishEditing(); // TFS 105498
                }
                self._removeListBox();
            });
        }

        // create the ListBox and add it to the document
        private _createListBox(): wijmo.input.ListBox {
            var g = this._g,
                rng = this._rng,
                row = g.rows[rng.row],
                col = g.columns[rng.col],
                div = document.createElement('div'),
                lbx = new wijmo.input.ListBox(div);

            // configure listbox
            addClass(div, 'wj-dropdown-panel');
            lbx.maxHeight = row.renderHeight * 4;
            lbx.itemsSource = col.dataMap.getDisplayValues();
            lbx.selectedValue = g.activeEditor
                ? g.activeEditor.value
                : g.getCellData(rng.row, rng.col, true);

            // show the popup
            showPopup(div, g.getCellBoundingRect(rng.row, rng.col));

            // done
            return lbx;
        }

        // remove and clear the ListBox element
        // this looks convoluted, but there's a reason for it:
        // https://stackoverflow.com/questions/21926083/failed-to-execute-removechild-on-node/22934552#22934552
        private _removeListBox() {
            var lbx = this._lbx;
            if (lbx) {
                this._lbx = null;
                var div = lbx.hostElement;
                div.parentElement.removeChild(div);
            }
        }
    }
}
module wijmo.grid {
    'use strict';

    /**
     * Manages the new row template used to add rows to the grid.
     */
    export class _AddNewHandler {
        private _g: FlexGrid;
        private _nrt = new _NewRowTemplate();

        /**
         * Initializes a new instance of an @see:_AddNewHandler.
         *
         * @param grid @see:FlexGrid that owns this @see:_AddNewHandler.
         */
        constructor(grid: FlexGrid) {
            this._g = grid;

            // add handlers to manage the new row template
            grid.beginningEdit.addHandler(this._beginningEdit, this);
            grid.rowEditEnded.addHandler(this._rowEditEnded, this);
            grid.loadedRows.addHandler(this.updateNewRowTemplate, this);
        }

        /**
         * Update the new row template to ensure it's visible only if the grid is bound
         * to a data source that supports adding new items, and that it is in the
         * right position.
         */
        updateNewRowTemplate() {

            // get variables
            var ecv = <wijmo.collections.IEditableCollectionView>tryCast(this._g.collectionView, 'IEditableCollectionView'),
                g = this._g,
                rows = g.rows;

            // see if we need a new row template
            var needTemplate = ecv && ecv.canAddNew && g.allowAddNew && !g.isReadOnly;

            // get current template index
            var index = rows.indexOf(this._nrt);

            // update template position
            if (!needTemplate && index > -1) { // not needed but present, remove it
                rows.removeAt(index);
            } else if (needTemplate) {
                if (index < 0) { // needed but not present, add it now
                    rows.push(this._nrt);
                } else if (index != rows.length - 1) { // position template
                    rows.removeAt(index);
                    rows.push(this._nrt);
                }

                // make sure the new row template is not collapsed
                if (this._nrt) {
                    this._nrt._setFlag(RowColFlags.ParentCollapsed, false);
                }
            }
        }

        // ** implementation

        // beginning edit, add new item if necessary
        private _beginningEdit(sender, e: CellRangeEventArgs) {
            if (!e.cancel) {
                var row = this._g.rows[e.row];
                if (tryCast(row, _NewRowTemplate)) {
                    var ecv = <wijmo.collections.IEditableCollectionView>tryCast(this._g.collectionView, 'IEditableCollectionView');
                    if (ecv && ecv.canAddNew) {

                        // start adding new row
                        var newItem = ecv.isAddingNew ? ecv.currentAddItem : ecv.addNew();
                        ecv.moveCurrentTo(newItem);
                        this.updateNewRowTemplate();

                        // update now to ensure the editor will get a fresh layout (TFS 96705)
                        this._g.refresh(true);

                        // fire row added event (user can customize the new row or cancel)
                        this._g.onRowAdded(e);
                        if (e.cancel) {
                            ecv.cancelNew();
                        }
                    }
                }
            }
        }

        // row has been edited, commit if this is the new row
        private _rowEditEnded(sender, e: CellRangeEventArgs) {
            var ecv = <wijmo.collections.IEditableCollectionView>tryCast(this._g.collectionView, 'IEditableCollectionView');
            if (ecv && ecv.isAddingNew) {
                ecv.commitNew();
            }
        }
    }

    /**
     * Represents a row template used to add items to the source collection.
     */
    export class _NewRowTemplate extends Row {
    }
}
