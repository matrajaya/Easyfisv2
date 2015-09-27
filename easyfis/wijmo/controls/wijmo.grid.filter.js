var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var wijmo;
(function (wijmo) {
    (function (_grid) {
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
        (function (filter) {
            'use strict';

            /**
            * Specifies types of column filter.
            */
            (function (FilterType) {
                /** No filter. */
                FilterType[FilterType["None"] = 0] = "None";

                /** A filter based on two conditions. */
                FilterType[FilterType["Condition"] = 1] = "Condition";

                /** A filter based on a set of values. */
                FilterType[FilterType["Value"] = 2] = "Value";

                /** A filter that combines condition and value filters. */
                FilterType[FilterType["Both"] = 3] = "Both";
            })(filter.FilterType || (filter.FilterType = {}));
            var FilterType = filter.FilterType;

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
            var FlexGridFilter = (function () {
                /**
                * Initializes a new instance of the @see:FlexGridFilter.
                *
                * @param grid The @see:FlexGrid to filter.
                */
                function FlexGridFilter(grid) {
                    this._showIcons = true;
                    this._showSort = true;
                    this._defFilterType = FilterType.Both;
                    /**
                    * Occurs after the filter is applied.
                    */
                    this.filterApplied = new wijmo.Event();
                    // check dependencies
                    var depErr = 'Missing dependency: FlexGridFilter requires ';
                    wijmo.assert(wijmo.grid.FlexGrid != null, depErr + 'wijmo.grid.');
                    wijmo.assert(wijmo.input.ComboBox != null, depErr + 'wijmo.input.');

                    // initialize filter
                    this._filters = [];
                    this._grid = wijmo.asType(grid, _grid.FlexGrid, false);
                    this._grid.formatItem.addHandler(this._formatItem.bind(this));
                    this._grid.itemsSourceChanged.addHandler(this.clear.bind(this));
                    this._grid.hostElement.addEventListener('mousedown', this._mouseDown.bind(this), true);

                    // initialize column filters
                    this._grid.invalidate();
                }
                Object.defineProperty(FlexGridFilter.prototype, "grid", {
                    /**
                    * Gets a reference to the @see:FlexGrid that owns this filter.
                    */
                    get: function () {
                        return this._grid;
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(FlexGridFilter.prototype, "showFilterIcons", {
                    /**
                    * Gets or sets a value indicating whether the @see:FlexGridFilter adds filter
                    * editing buttons to the grid's column headers.
                    *
                    * If you set this property to false, then you are responsible for providing
                    * a way for users to edit, clear, and apply the filters.
                    */
                    get: function () {
                        return this._showIcons;
                    },
                    set: function (value) {
                        if (value != this.showFilterIcons) {
                            this._showIcons = wijmo.asBoolean(value);
                            if (this._grid) {
                                this._grid.invalidate();
                            }
                        }
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(FlexGridFilter.prototype, "showSortButtons", {
                    /**
                    * Gets or sets a value indicating whether the filter editor should include
                    * sort buttons.
                    *
                    * By default, the editor shows sort buttons like Excel does. But since users
                    * can sort columns by clicking their headers, sort buttons in the filter editor
                    * may not be desirable in some circumstances.
                    */
                    get: function () {
                        return this._showSort;
                    },
                    set: function (value) {
                        this._showSort = wijmo.asBoolean(value);
                    },
                    enumerable: true,
                    configurable: true
                });

                /**
                * Gets the filter for the given column.
                *
                * @param col The @see:Column that the filter applies to.
                * @param create Whether to create the filter if it does not exist.
                */
                FlexGridFilter.prototype.getColumnFilter = function (col, create) {
                    if (typeof create === "undefined") { create = true; }
                    for (var i = 0; i < this._filters.length; i++) {
                        if (this._filters[i].column == col) {
                            return this._filters[i];
                        }
                    }

                    // not found, create one now
                    if (create && col.binding) {
                        var cf = new filter.ColumnFilter(this, col);
                        this._filters.push(cf);
                        return cf;
                    }

                    // not found, not created
                    return null;
                };

                Object.defineProperty(FlexGridFilter.prototype, "defaultFilterType", {
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
                    get: function () {
                        return this._defFilterType;
                    },
                    set: function (value) {
                        if (value != this.defaultFilterType) {
                            this._defFilterType = wijmo.asEnum(value, FilterType, false);
                            this.clear();
                        }
                    },
                    enumerable: true,
                    configurable: true
                });

                /**
                * Shows the filter editor for the given grid column.
                *
                * @param col The @see:Column that contains the filter to edit.
                */
                FlexGridFilter.prototype.editColumnFilter = function (col) {
                    // remove current editor
                    this.closeEditor();

                    // get column by name of by reference
                    col = wijmo.isString(col) ? this._grid.columns.getColumn(col) : wijmo.asType(col, _grid.Column, false);

                    // get the header cell to position editor
                    var ch = this._grid.columnHeaders, rc = ch.getCellBoundingRect(ch.rows.length - 1, col.index);

                    // get the filter and the editor
                    var div = document.createElement('div'), flt = this.getColumnFilter(col), edt = new filter.ColumnFilterEditor(div, flt, this.showSortButtons);
                    wijmo.addClass(div, 'wj-dropdown-panel');

                    // close editor when buttons are clicked or when it loses focus
                    var self = this;
                    edt.filterChanged.addHandler(function () {
                        self.closeEditor();
                        self.apply();
                    });

                    // use blur+capture to emulate focusout (not supported in FireFox)
                    div.addEventListener('blur', function () {
                        setTimeout(function () {
                            if (!wijmo.contains(self._divEdt, document.activeElement)) {
                                self.closeEditor();
                            }
                        }, 200); // let others handle it first
                    }, true);

                    // show editor and give it focus
                    var host = document.body;
                    host.appendChild(div);
                    div.tabIndex = -1;
                    wijmo.showPopup(div, rc);
                    div.focus();

                    // save reference to editor
                    this._divEdt = div;
                    this._edtCol = col;
                };

                /**
                * Closes the filter editor.
                */
                FlexGridFilter.prototype.closeEditor = function () {
                    if (this._divEdt) {
                        wijmo.hidePopup(this._divEdt, true);
                        this._divEdt = null;
                        this._edtCol = null;
                    }
                };

                /**
                * Applies the current column filters to the grid.
                */
                FlexGridFilter.prototype.apply = function () {
                    var cv = this._grid.collectionView;
                    if (cv) {
                        if (cv.filter) {
                            cv.refresh();
                        } else {
                            cv.filter = this._filter.bind(this);
                        }
                    }
                    this.onFilterApplied();
                };

                /**
                * Clears all column filters.
                */
                FlexGridFilter.prototype.clear = function () {
                    this._filters = [];
                    this.apply();
                };

                /**
                * Raises the @see:filterApplied event.
                */
                FlexGridFilter.prototype.onFilterApplied = function () {
                    this.filterApplied.raise(this);
                };

                // ** implementation
                // predicate function used to filter the CollectionView
                FlexGridFilter.prototype._filter = function (item) {
                    for (var i = 0; i < this._filters.length; i++) {
                        if (!this._filters[i].apply(item)) {
                            return false;
                        }
                    }
                    return true;
                };

                // handle the formatItem event to add filter icons to the column header cells
                FlexGridFilter.prototype._formatItem = function (sender, e) {
                    // check that this is a filter cell
                    if (e.panel.cellType == _grid.CellType.ColumnHeader && e.row == this._grid.columnHeaders.rows.length - 1) {
                        // check that this column should have a filter
                        var col = this._grid.columns[e.col], cf = this.getColumnFilter(col, this.defaultFilterType != FilterType.None);
                        if (cf && cf.filterType != FilterType.None) {
                            // show filter glyph for this column
                            if (this._showIcons) {
                                var op = cf.isActive ? .85 : .25, filterGlyph = '<div ' + FlexGridFilter._WJA_FILTER + ' style ="float:right;cursor:pointer;padding:0px 4px;opacity:' + op + '">' + '<span class="wj-glyph-filter"></span>' + '</div>';
                                e.cell.innerHTML = filterGlyph + e.cell.innerHTML;
                            }

                            // update filter classes
                            wijmo.toggleClass(e.cell, 'wj-filter-on', cf.isActive);
                            wijmo.toggleClass(e.cell, 'wj-filter-off', !cf.isActive);
                        } else {
                            // remove filter classes
                            wijmo.removeClass(e.cell, 'wj-filter-on');
                            wijmo.removeClass(e.cell, 'wj-filter-off');
                        }
                    }
                };

                // handle mouse down to show/hide the filter editor
                FlexGridFilter.prototype._mouseDown = function (e) {
                    if (this._hasAttribute(e.target, FlexGridFilter._WJA_FILTER)) {
                        var ht = this._grid.hitTest(e);
                        if (ht.gridPanel == this._grid.columnHeaders) {
                            var col = this._grid.columns[ht.col];
                            if (this._divEdt && this._edtCol == col) {
                                this.closeEditor();
                            } else {
                                this.editColumnFilter(col);
                            }
                            e.stopPropagation();
                            e.preventDefault();
                        }
                    }
                };

                // checks whether an element or any of its ancestors contains an attribute
                FlexGridFilter.prototype._hasAttribute = function (e, att) {
                    for (; e; e = e.parentNode) {
                        if (e.getAttribute && e.getAttribute(att) != null)
                            return true;
                    }
                    return false;
                };
                FlexGridFilter._WJA_FILTER = 'wj-filter';
                return FlexGridFilter;
            })();
            filter.FlexGridFilter = FlexGridFilter;
        })(_grid.filter || (_grid.filter = {}));
        var filter = _grid.filter;
    })(wijmo.grid || (wijmo.grid = {}));
    var grid = wijmo.grid;
})(wijmo || (wijmo = {}));
var wijmo;
(function (wijmo) {
    (function (grid) {
        (function (filter) {
            'use strict';

            
        })(grid.filter || (grid.filter = {}));
        var filter = grid.filter;
    })(wijmo.grid || (wijmo.grid = {}));
    var grid = wijmo.grid;
})(wijmo || (wijmo = {}));
var wijmo;
(function (wijmo) {
    (function (grid) {
        (function (filter) {
            'use strict';

            /**
            * Defines a filter for a column on a @see:FlexGrid control.
            *
            * The @see:ColumnFilter contains a @see:ConditionFilter and a
            * @see:ValueFilter; only one of them may be active at a time.
            *
            * This class is used by the @see:FlexGridFilter class; you
            * rarely use it directly.
            */
            var ColumnFilter = (function () {
                /**
                * Initializes a new instance of a @see:ColumnFilter.
                *
                * @param owner The @see:FlexGridFilter that owns this column filter.
                * @param column The @see:Column to filter.
                */
                function ColumnFilter(owner, column) {
                    this._owner = owner;
                    this._col = column;
                    this._valueFilter = new filter.ValueFilter(column);
                    this._conditionFilter = new filter.ConditionFilter(column);
                }
                Object.defineProperty(ColumnFilter.prototype, "filterType", {
                    /**
                    * Gets or sets the types of filtering provided by this filter.
                    *
                    * Setting this property to null causes the filter to use the value
                    * defined by the owner filter's @see:defaultFilterType property.
                    */
                    get: function () {
                        return this._filterType != null ? this._filterType : this._owner.defaultFilterType;
                    },
                    set: function (value) {
                        if (value != this._filterType) {
                            this.clear();
                            this._filterType = wijmo.asEnum(value, filter.FilterType, true);
                            this._owner.apply();
                        }
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(ColumnFilter.prototype, "valueFilter", {
                    /**
                    * Gets the @see:ValueFilter in this @see:ColumnFilter.
                    */
                    get: function () {
                        return this._valueFilter;
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(ColumnFilter.prototype, "conditionFilter", {
                    /**
                    * Gets the @see:ConditionFilter in this @see:ColumnFilter.
                    */
                    get: function () {
                        return this._conditionFilter;
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(ColumnFilter.prototype, "column", {
                    // ** IColumnFilter
                    /**
                    * Gets the @see:Column being filtered.
                    */
                    get: function () {
                        return this._col;
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(ColumnFilter.prototype, "isActive", {
                    /**
                    * Gets a value indicating whether the filter is active.
                    */
                    get: function () {
                        return this._conditionFilter.isActive || this._valueFilter.isActive;
                    },
                    enumerable: true,
                    configurable: true
                });

                /**
                * Gets a value indicating whether a value passes the filter.
                *
                * @param value The value to test.
                */
                ColumnFilter.prototype.apply = function (value) {
                    return this._conditionFilter.apply(value) && this._valueFilter.apply(value);
                };

                /**
                * Clears the filter.
                */
                ColumnFilter.prototype.clear = function () {
                    this._valueFilter.clear();
                    this._conditionFilter.clear();
                };

                // ** IQueryInterface
                /**
                * Returns true if the caller queries for a supported interface.
                *
                * @param interfaceName Name of the interface to look for.
                */
                ColumnFilter.prototype.implementsInterface = function (interfaceName) {
                    return interfaceName == 'IColumnFilter';
                };
                return ColumnFilter;
            })();
            filter.ColumnFilter = ColumnFilter;
        })(grid.filter || (grid.filter = {}));
        var filter = grid.filter;
    })(wijmo.grid || (wijmo.grid = {}));
    var grid = wijmo.grid;
})(wijmo || (wijmo = {}));

var wijmo;
(function (wijmo) {
    (function (grid) {
        (function (_filter) {
            'use strict';

            // globalization info
            wijmo.culture.FlexGridFilter = {
                // filter
                ascending: '\u2191 Ascending',
                descending: '\u2193 Descending',
                apply: 'Apply',
                clear: 'Clear',
                conditions: 'Filter by Condition',
                values: 'Filter by Value',
                // value filter
                search: 'Search',
                selectAll: 'Select All',
                null: '(nothing)',
                // condition filter
                header: 'Show items where the value',
                and: 'And',
                or: 'Or',
                stringOperators: [
                    { name: '(not set)', op: null },
                    { name: 'Equals', op: _filter.Operator.EQ },
                    { name: 'Does not equal', op: _filter.Operator.NE },
                    { name: 'Begins with', op: _filter.Operator.BW },
                    { name: 'Ends with', op: _filter.Operator.EW },
                    { name: 'Contains', op: _filter.Operator.CT },
                    { name: 'Does not contain', op: _filter.Operator.NC }
                ],
                numberOperators: [
                    { name: '(not set)', op: null },
                    { name: 'Equals', op: _filter.Operator.EQ },
                    { name: 'Does not equal', op: _filter.Operator.NE },
                    { name: 'Is Greater than', op: _filter.Operator.GT },
                    { name: 'Is Greater than or equal to', op: _filter.Operator.GE },
                    { name: 'Is Less than', op: _filter.Operator.LT },
                    { name: 'Is Less than or equal to', op: _filter.Operator.LE }
                ],
                dateOperators: [
                    { name: '(not set)', op: null },
                    { name: 'Equals', op: _filter.Operator.EQ },
                    { name: 'Is Before', op: _filter.Operator.LT },
                    { name: 'Is After', op: _filter.Operator.GT }
                ],
                booleanOperators: [
                    { name: '(not set)', op: null },
                    { name: 'Equals', op: _filter.Operator.EQ },
                    { name: 'Does not equal', op: _filter.Operator.NE }
                ]
            };

            /**
            * The editor used to inspect and modify column filters.
            *
            * This class is used by the @see:FlexGridFilter class; you
            * rarely use it directly.
            */
            var ColumnFilterEditor = (function (_super) {
                __extends(ColumnFilterEditor, _super);
                /**
                * Initializes a new instance of the @see:ColumnFilterEditor.
                *
                * @param element The DOM element that hosts the control, or a selector
                * for the host element (e.g. '#theCtrl').
                * @param filter The @see:ColumnFilter to edit.
                * @param sortButtons Whether to show sort buttons in the editor.
                */
                function ColumnFilterEditor(element, filter, sortButtons) {
                    if (typeof sortButtons === "undefined") { sortButtons = true; }
                    _super.call(this, element);
                    /**
                    * Occurs after the filter is modified.
                    */
                    this.filterChanged = new wijmo.Event();

                    // save reference to filter being edited
                    this._filter = wijmo.asType(filter, _filter.ColumnFilter);

                    // instantiate and apply template
                    var tpl = this.getTemplate();
                    this.applyTemplate('wj-control wj-columnfiltereditor wj-content', tpl, {
                        _divSort: 'div-sort',
                        _btnAsc: 'btn-asc',
                        _btnDsc: 'btn-dsc',
                        _divType: 'div-type',
                        _aVal: 'a-val',
                        _aCnd: 'a-cnd',
                        _divEdtVal: 'div-edt-val',
                        _divEdtCnd: 'div-edt-cnd',
                        _btnApply: 'btn-apply',
                        _btnClear: 'btn-clear'
                    });

                    // localization
                    this._btnAsc.textContent = wijmo.culture.FlexGridFilter.ascending;
                    this._btnDsc.textContent = wijmo.culture.FlexGridFilter.descending;
                    this._aVal.textContent = wijmo.culture.FlexGridFilter.values;
                    this._aCnd.textContent = wijmo.culture.FlexGridFilter.conditions;
                    this._btnApply.textContent = wijmo.culture.FlexGridFilter.apply;
                    this._btnClear.textContent = wijmo.culture.FlexGridFilter.clear;

                    // add event listeners
                    var bnd = this._btnClicked.bind(this);
                    this._btnApply.addEventListener('click', bnd);
                    this._btnClear.addEventListener('click', bnd);
                    this._btnAsc.addEventListener('click', bnd);
                    this._btnDsc.addEventListener('click', bnd);
                    this._aVal.addEventListener('click', bnd);
                    this._aCnd.addEventListener('click', bnd);

                    // create filter editors
                    this._edtVal = new _filter.ValueFilterEditor(this._divEdtVal, filter.valueFilter);
                    this._edtCnd = new _filter.ConditionFilterEditor(this._divEdtCnd, filter.conditionFilter);

                    // show the filter that is active
                    var ft = (this.filter.conditionFilter.isActive || (filter.filterType & _filter.FilterType.Value) == 0) ? _filter.FilterType.Condition : _filter.FilterType.Value;
                    this._showFilter(ft);

                    // hide sort buttons if the collection view is not sortable
                    // or if the user doesn't want them
                    var col = this.filter.column, view = col.grid.collectionView;
                    if (!sortButtons || !view || !view.canSort) {
                        this._divSort.style.display = 'none';
                    }

                    // initialize all values
                    this.updateEditor();

                    // dismiss/commit on Esc/Enter
                    var self = this;
                    this.hostElement.addEventListener('keydown', function (e) {
                        switch (e.keyCode) {
                            case wijmo.Key.Enter:
                                self.updateFilter();
                                self.onFilterChanged();
                                break;
                            case wijmo.Key.Escape:
                                self.onFilterChanged();
                                break;
                        }
                    });
                }
                Object.defineProperty(ColumnFilterEditor.prototype, "filter", {
                    /**
                    * Gets a reference to the @see:ColumnFilter being edited.
                    */
                    get: function () {
                        return this._filter;
                    },
                    enumerable: true,
                    configurable: true
                });

                /**
                * Updates editor with current filter settings.
                */
                ColumnFilterEditor.prototype.updateEditor = function () {
                    this._edtVal.updateEditor();
                    this._edtCnd.updateEditor();
                };

                /**
                * Updates filter to reflect the current editor values.
                */
                ColumnFilterEditor.prototype.updateFilter = function () {
                    switch (this._getFilterType()) {
                        case _filter.FilterType.Value:
                            this._edtVal.updateFilter();
                            this.filter.conditionFilter.clear();
                            break;
                        case _filter.FilterType.Condition:
                            this._edtCnd.updateFilter();
                            this.filter.valueFilter.clear();
                            break;
                    }
                };

                /**
                * Raises the @see:filterChanged event.
                */
                ColumnFilterEditor.prototype.onFilterChanged = function (e) {
                    this.filterChanged.raise(this, e);
                };

                // ** implementation
                // shows the value or filter editor
                ColumnFilterEditor.prototype._showFilter = function (filterType) {
                    // show selected editor
                    if ((filterType & this.filter.filterType) != 0) {
                        if (filterType == _filter.FilterType.Value) {
                            this._divEdtVal.style.display = '';
                            this._divEdtCnd.style.display = 'none';
                            this._enableLink(this._aVal, false);
                            this._enableLink(this._aCnd, true);
                        } else {
                            this._divEdtVal.style.display = 'none';
                            this._divEdtCnd.style.display = '';
                            this._enableLink(this._aVal, true);
                            this._enableLink(this._aCnd, false);
                        }
                    }

                    switch (this.filter.filterType) {
                        case _filter.FilterType.None:
                        case _filter.FilterType.Condition:
                        case _filter.FilterType.Value:
                            this._divType.style.display = 'none';
                            break;
                        default:
                            this._divType.style.display = '';
                            break;
                    }
                };

                // enable/disable filter switch links
                ColumnFilterEditor.prototype._enableLink = function (a, enable) {
                    a.style.textDecoration = enable ? '' : 'none';
                    a.style.fontWeight = enable ? '' : 'bold';
                    if (enable) {
                        a.href = '';
                    } else {
                        a.removeAttribute('href');
                    }
                };

                // gets the type of filter currently being edited
                ColumnFilterEditor.prototype._getFilterType = function () {
                    return this._divEdtVal.style.display != 'none' ? _filter.FilterType.Value : _filter.FilterType.Condition;
                };

                // handle buttons
                ColumnFilterEditor.prototype._btnClicked = function (e) {
                    e.preventDefault();
                    e.stopPropagation();

                    // ignore disabled elements
                    if (wijmo.hasClass(e.target, 'wj-state-disabled')) {
                        return;
                    }

                    // switch filters
                    if (e.target == this._aVal) {
                        this._showFilter(_filter.FilterType.Value);
                        this._edtVal.focus();
                        return;
                    }
                    if (e.target == this._aCnd) {
                        this._showFilter(_filter.FilterType.Condition);
                        this._edtCnd.focus();
                        return;
                    }

                    // apply sort
                    if (e.target == this._btnAsc || e.target == this._btnDsc) {
                        var col = this.filter.column, binding = col.sortMemberPath ? col.sortMemberPath : col.binding, view = col.grid.collectionView, sortDesc = new wijmo.collections.SortDescription(binding, e.target == this._btnAsc);
                        view.sortDescriptions.deferUpdate(function () {
                            view.sortDescriptions.clear();
                            view.sortDescriptions.push(sortDesc);
                        });
                    }

                    // apply/clear filter
                    if (e.target == this._btnApply) {
                        this.updateFilter();
                    } else if (e.target == this._btnClear) {
                        this.filter.clear();
                    }

                    // show current filter state
                    this.updateEditor();

                    // raise event so caller can close the editor and apply the new filter
                    this.onFilterChanged();
                };
                ColumnFilterEditor.controlTemplate = '<div>' + '<div wj-part="div-sort">' + '<a wj-part="btn-asc" href="" style="min-width:95px"></a>&nbsp;&nbsp;&nbsp;' + '<a wj-part="btn-dsc" href="" style="min-width:95px"></a>' + '</div>' + '<div style="text-align:right;margin:10px 0px;font-size:80%">' + '<div wj-part="div-type">' + '<a wj-part="a-cnd" href="" tab-index="-1"></a>' + '&nbsp;|&nbsp;' + '<a wj-part="a-val" href="" tab-index="-1"></a>' + '</div>' + '</div>' + '<div wj-part="div-edt-val"></div>' + '<div wj-part="div-edt-cnd"></div>' + '<div style="text-align:right;margin-top:10px">' + '<a wj-part="btn-apply" href=""></a>&nbsp;&nbsp;' + '<a wj-part="btn-clear" href=""></a>' + '</div>';
                return ColumnFilterEditor;
            })(wijmo.Control);
            _filter.ColumnFilterEditor = ColumnFilterEditor;
        })(grid.filter || (grid.filter = {}));
        var filter = grid.filter;
    })(wijmo.grid || (wijmo.grid = {}));
    var grid = wijmo.grid;
})(wijmo || (wijmo = {}));
var wijmo;
(function (wijmo) {
    (function (grid) {
        (function (filter) {
            'use strict';

            /**
            * Defines a value filter for a column on a @see:FlexGrid control.
            *
            * Value filters contain an explicit list of values that should be
            * displayed by the grid.
            */
            var ValueFilter = (function () {
                /**
                * Initializes a new instance of a @see:ValueFilter.
                *
                * @param column The column to filter.
                */
                function ValueFilter(column) {
                    this._col = column;
                    this._bnd = column.binding ? new wijmo.Binding(column.binding) : null;
                }
                Object.defineProperty(ValueFilter.prototype, "showValues", {
                    /**
                    * Gets or sets an object with all the formatted values that should be shown.
                    */
                    get: function () {
                        return this._values;
                    },
                    set: function (value) {
                        this._values = value;
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(ValueFilter.prototype, "filterText", {
                    /**
                    * Gets or sets a string used to filter the list of display values.
                    */
                    get: function () {
                        return this._filterText;
                    },
                    set: function (value) {
                        this._filterText = wijmo.asString(value);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(ValueFilter.prototype, "column", {
                    // ** IColumnFilter
                    /**
                    * Gets the @see:Column to filter.
                    */
                    get: function () {
                        return this._col;
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(ValueFilter.prototype, "isActive", {
                    /**
                    * Gets a value indicating whether the filter is active.
                    *
                    * The filter is active if at least one of the two conditions
                    * has its operator set to a non-null value.
                    */
                    get: function () {
                        return this._values != null;
                    },
                    enumerable: true,
                    configurable: true
                });

                /**
                * Gets a value indicating whether a value passes the filter.
                *
                * @param value The value to test.
                */
                ValueFilter.prototype.apply = function (value) {
                    var col = this.column;

                    // no binding or no values? accept everything
                    if (!col.binding || !this._values || !Object.keys(this._values).length) {
                        return true;
                    }

                    // retrieve the formatted value
                    value = this._bnd.getValue(value);
                    value = col.dataMap ? col.dataMap.getDisplayValue(value) : wijmo.Globalize.format(value, col.format);

                    // apply conditions
                    return this._values[value] != undefined;
                };

                /**
                * Clears the filter.
                */
                ValueFilter.prototype.clear = function () {
                    this.showValues = null;
                    this.filterText = null;
                };

                // ** IQueryInterface
                /**
                * Returns true if the caller queries for a supported interface.
                *
                * @param interfaceName Name of the interface to look for.
                */
                ValueFilter.prototype.implementsInterface = function (interfaceName) {
                    return interfaceName == 'IColumnFilter';
                };
                return ValueFilter;
            })();
            filter.ValueFilter = ValueFilter;
        })(grid.filter || (grid.filter = {}));
        var filter = grid.filter;
    })(wijmo.grid || (wijmo.grid = {}));
    var grid = wijmo.grid;
})(wijmo || (wijmo = {}));
var wijmo;
(function (wijmo) {
    (function (grid) {
        (function (_filter) {
            'use strict';

            /**
            * The editor used to inspect and modify @see:ValueFilter objects.
            *
            * This class is used by the @see:FlexGridFilter class; you
            * rarely use it directly.
            */
            var ValueFilterEditor = (function (_super) {
                __extends(ValueFilterEditor, _super);
                /**
                * Initializes a new instance of the @see:ValueFilterEditor.
                *
                * @param element The DOM element that hosts the control, or a selector
                * for the host element (e.g. '#theCtrl').
                * @param filter The @see:ValueFilter to edit.
                */
                function ValueFilterEditor(element, filter) {
                    _super.call(this, element);

                    // save reference to filter
                    this._filter = wijmo.asType(filter, _filter.ValueFilter, false);

                    // instantiate and apply template
                    var tpl = this.getTemplate();
                    this.applyTemplate('wj-control', tpl, {
                        _divFilter: 'div-filter',
                        _cbSelectAll: 'cb-select-all',
                        _spSelectAll: 'sp-select-all',
                        _divValues: 'div-values'
                    });

                    // localization
                    this._spSelectAll.textContent = wijmo.culture.FlexGridFilter.selectAll;

                    // create sorted/filtered collection view with the values
                    var sortBinding = filter.column.dataMap ? 'text' : 'value';
                    this._view = new wijmo.collections.CollectionView();
                    this._view.sortDescriptions.push(new wijmo.collections.SortDescription(sortBinding, true));
                    this._view.filter = this._filterValues.bind(this);
                    this._view.collectionChanged.addHandler(this._updateSelectAllCheck, this);

                    // create search combo and value list
                    this._cmbFilter = new wijmo.input.ComboBox(this._divFilter, {
                        placeholder: wijmo.culture.FlexGridFilter.search
                    });
                    this._lbValues = new wijmo.input.ListBox(this._divValues, {
                        displayMemberPath: 'text',
                        checkedMemberPath: 'show',
                        itemsSource: this._view,
                        itemFormatter: function (index, item) {
                            return item ? item : wijmo.culture.FlexGridFilter.null;
                        }
                    });

                    // add event listeners
                    this._cmbFilter.textChanged.addHandler(this._filterTextChanged, this);
                    this._cbSelectAll.addEventListener('click', this._cbSelectAllClicked.bind(this));

                    // initialize all values
                    this.updateEditor();
                }
                Object.defineProperty(ValueFilterEditor.prototype, "filter", {
                    /**
                    * Gets a reference to the @see:ValueFilter being edited.
                    */
                    get: function () {
                        return this._filter;
                    },
                    enumerable: true,
                    configurable: true
                });

                /**
                * Updates editor with current filter settings.
                */
                ValueFilterEditor.prototype.updateEditor = function () {
                    // get a list of the values present in the data source
                    var col = this._filter.column, g = col.grid, src = g.collectionView ? g.collectionView.sourceCollection : [], textArr = [], values = [];
                    for (var i = 0; i < src.length; i++) {
                        var value = col._binding.getValue(src[i]), text = col.dataMap ? col.dataMap.getDisplayValue(value) : wijmo.Globalize.format(value, col.format);
                        if (textArr.indexOf(text) < 0) {
                            textArr.push(text);
                            values.push({ value: value, text: text });
                        }
                    }

                    // check the items that are currently selected
                    var showValues = this._filter.showValues;
                    if (!showValues || Object.keys(showValues).length == 0) {
                        for (var i = 0; i < values.length; i++) {
                            values[i].show = true;
                        }
                    } else {
                        for (var key in showValues) {
                            for (var i = 0; i < values.length; i++) {
                                if (values[i].text == key) {
                                    values[i].show = true;
                                    break;
                                }
                            }
                        }
                    }

                    // load filter and apply immeditately
                    this._cmbFilter.text = this._filter.filterText;
                    this._filterText = this._cmbFilter.text.toLowerCase();

                    // show the values
                    this._view.sourceCollection = values;
                    this._view.moveCurrentToPosition(-1);
                };

                /**
                * Updates filter to reflect the current editor values.
                */
                ValueFilterEditor.prototype.updateFilter = function () {
                    // build list of values to show
                    var showValues = {}, items = this._view.items;
                    for (var i = 0; i < items.length; i++) {
                        var item = items[i];
                        if (item.show) {
                            showValues[item.text] = true;
                        }
                    }

                    // save to filter
                    this._filter.showValues = showValues;
                    this._filter.filterText = this._filterText;
                };

                // ** implementation
                // filter items on the list
                ValueFilterEditor.prototype._filterTextChanged = function () {
                    var self = this;
                    if (self._toText) {
                        clearTimeout(self._toText);
                    }
                    self._toText = setTimeout(function () {
                        self._filterText = self._cmbFilter.text.toLowerCase();
                        self._view.refresh();
                    }, 500);
                };

                // filter values for display
                ValueFilterEditor.prototype._filterValues = function (value) {
                    if (this._filterText) {
                        return value && value.text ? value.text.toLowerCase().indexOf(this._filterText) > -1 : false;
                    }
                    return true;
                };

                // handle clicks on 'Select All' checkbox
                ValueFilterEditor.prototype._cbSelectAllClicked = function (e) {
                    var checked = this._cbSelectAll.checked, values = this._view.items;
                    for (var i = 0; i < values.length; i++) {
                        values[i].show = checked;
                    }
                    this._view.refresh();
                };

                // update state of 'Select All' checkbox when values are checked/unchecked
                ValueFilterEditor.prototype._updateSelectAllCheck = function () {
                    // count checked itmes
                    var checked = 0, values = this._view.items;
                    for (var i = 0; i < values.length; i++) {
                        if (values[i].show)
                            checked++;
                    }

                    // update checkbox
                    if (checked == 0) {
                        this._cbSelectAll.checked = false;
                        this._cbSelectAll.indeterminate = false;
                    } else if (checked == values.length) {
                        this._cbSelectAll.checked = true;
                        this._cbSelectAll.indeterminate = false;
                    } else {
                        this._cbSelectAll.indeterminate = true;
                    }
                    // REVIEW: disable Apply button if nothing is selected
                    //toggleClass(this._btnApply, 'wj-state-disabled', checked == 0);
                    //this._btnApply.style.cursor = (checked == 0) ? 'default' : '';
                };
                ValueFilterEditor.controlTemplate = '<div>' + '<div wj-part="div-filter"></div>' + '<br/>' + '<label style="margin-left:11px">' + '<input wj-part="cb-select-all" type="checkbox"> ' + '<span wj-part="sp-select-all"></span>' + '</label>' + '<br/>' + '<div wj-part="div-values" class="wj-dropdown" style="min-height:122px;max-height:150px"></div>' + '</div>';
                return ValueFilterEditor;
            })(wijmo.Control);
            _filter.ValueFilterEditor = ValueFilterEditor;
        })(grid.filter || (grid.filter = {}));
        var filter = grid.filter;
    })(wijmo.grid || (wijmo.grid = {}));
    var grid = wijmo.grid;
})(wijmo || (wijmo = {}));
var wijmo;
(function (wijmo) {
    (function (grid) {
        (function (filter) {
            'use strict';

            /**
            * Defines a condition filter for a column on a @see:FlexGrid control.
            *
            * Condition filters contain two conditions that may be combined
            * using an 'and' or an 'or' operator.
            *
            * This class is used by the @see:FlexGridFilter class; you will
            * rarely use it directly.
            */
            var ConditionFilter = (function () {
                /**
                * Initializes a new instance of a @see:ConditionFilter.
                *
                * @param column The column to filter.
                */
                function ConditionFilter(column) {
                    this._c1 = new filter.FilterCondition();
                    this._c2 = new filter.FilterCondition();
                    this._and = true;
                    this._col = column;
                    this._bnd = column.binding ? new wijmo.Binding(column.binding) : null;
                }
                Object.defineProperty(ConditionFilter.prototype, "condition1", {
                    /**
                    * Gets the first condition in the filter.
                    */
                    get: function () {
                        return this._c1;
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(ConditionFilter.prototype, "condition2", {
                    /**
                    * Gets the second condition in the filter.
                    */
                    get: function () {
                        return this._c2;
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(ConditionFilter.prototype, "and", {
                    /**
                    * Gets a value indicating whether to combine the two conditions
                    * with an AND or an OR operator.
                    */
                    get: function () {
                        return this._and;
                    },
                    set: function (value) {
                        this._and = wijmo.asBoolean(value);
                        this._bnd = this.column && this.column.binding ? new wijmo.Binding(this.column.binding) : null;
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(ConditionFilter.prototype, "column", {
                    // ** IColumnFilter
                    /**
                    * Gets the @see:Column to filter.
                    */
                    get: function () {
                        return this._col;
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(ConditionFilter.prototype, "isActive", {
                    /**
                    * Gets a value indicating whether the filter is active.
                    *
                    * The filter is active if at least one of the two conditions
                    * has its operator set to a non-null value.
                    */
                    get: function () {
                        return this._c1.operator != null || this._c2.operator != null;
                    },
                    enumerable: true,
                    configurable: true
                });

                /**
                * Returns a value indicating whether a value passes this filter.
                *
                * @param value The value to test.
                */
                ConditionFilter.prototype.apply = function (value) {
                    var col = this.column, c1 = this.condition1, c2 = this.condition2;

                    // no binding? accept everything
                    if (!col.binding) {
                        return true;
                    }

                    // retrieve the value
                    value = this._bnd.getValue(value);
                    if (col.dataMap) {
                        value = col.dataMap.getDisplayValue(value);
                    } else if (wijmo.isDate(value)) {
                        if (wijmo.isString(c1.value) || wijmo.isString(c2.value)) {
                            value = wijmo.Globalize.format(value, col.format);
                        }
                    } else if (wijmo.isBoolean(value)) {
                        value = value.toString();
                    }

                    // apply conditions
                    var rv1 = c1.apply(value), rv2 = c2.apply(value);

                    // combine results
                    if (c1.operator != null && c2.operator != null) {
                        return this._and ? rv1 && rv2 : rv1 || rv2;
                    } else {
                        return c1.operator != null ? rv1 : c2.operator != null ? rv2 : true;
                    }
                };

                /**
                * Clears the filter.
                */
                ConditionFilter.prototype.clear = function () {
                    this.condition1.operator = null;
                    this.condition2.operator = null;
                    this.and = true;
                    this.condition1.value = null;
                    this.condition2.value = null;
                };

                // ** IQueryInterface
                /**
                * Returns true if the caller queries for a supported interface.
                *
                * @param interfaceName Name of the interface to look for.
                */
                ConditionFilter.prototype.implementsInterface = function (interfaceName) {
                    return interfaceName == 'IColumnFilter';
                };
                return ConditionFilter;
            })();
            filter.ConditionFilter = ConditionFilter;
        })(grid.filter || (grid.filter = {}));
        var filter = grid.filter;
    })(wijmo.grid || (wijmo.grid = {}));
    var grid = wijmo.grid;
})(wijmo || (wijmo = {}));
var wijmo;
(function (wijmo) {
    (function (grid) {
        (function (_filter) {
            'use strict';

            /**
            * The editor used to inspect and modify @see:ConditionFilter objects.
            *
            * This class is used by the @see:FlexGridFilter class; you
            * rarely use it directly.
            */
            var ConditionFilterEditor = (function (_super) {
                __extends(ConditionFilterEditor, _super);
                /**
                * Initializes a new instance of a @see:ConditionFilterEditor.
                *
                * @param element The DOM element that hosts the control, or a selector
                * for the host element (e.g. '#theCtrl').
                * @param filter The @see:ConditionFilter to edit.
                */
                function ConditionFilterEditor(element, filter) {
                    _super.call(this, element);

                    // save reference to filter
                    this._filter = wijmo.asType(filter, _filter.ConditionFilter, false);

                    // instantiate and apply template
                    var tpl = this.getTemplate();
                    this.applyTemplate('wj-control', tpl, {
                        _divHdr: 'div-hdr',
                        _divCmb1: 'div-cmb1',
                        _divVal1: 'div-val1',
                        _btnAnd: 'btn-and',
                        _btnOr: 'btn-or',
                        _spAnd: 'sp-and',
                        _spOr: 'sp-or',
                        _divCmb2: 'div-cmb2',
                        _divVal2: 'div-val2'
                    });

                    // localization
                    this._divHdr.textContent = wijmo.culture.FlexGridFilter.header;
                    this._spAnd.textContent = wijmo.culture.FlexGridFilter.and;
                    this._spOr.textContent = wijmo.culture.FlexGridFilter.or;

                    // create combos and value editors
                    this._cmb1 = this._createOperatorCombo(this._divCmb1);
                    this._cmb2 = this._createOperatorCombo(this._divCmb2);
                    this._val1 = this._createValueInput(this._divVal1);
                    this._val2 = this._createValueInput(this._divVal2);

                    // add event listeners
                    var andOr = this._btnAndOrChanged.bind(this);
                    this._btnAnd.addEventListener('change', andOr);
                    this._btnOr.addEventListener('change', andOr);

                    // initialize all values
                    this.updateEditor();
                }
                Object.defineProperty(ConditionFilterEditor.prototype, "filter", {
                    /**
                    * Gets a reference to the @see:ConditionFilter being edited.
                    */
                    get: function () {
                        return this._filter;
                    },
                    enumerable: true,
                    configurable: true
                });

                /**
                * Updates editor with current filter settings.
                */
                ConditionFilterEditor.prototype.updateEditor = function () {
                    // initialize conditions
                    var c1 = this._filter.condition1, c2 = this._filter.condition2;
                    this._cmb1.selectedValue = c1.operator;
                    this._cmb2.selectedValue = c2.operator;
                    if (this._val1 instanceof wijmo.input.ComboBox) {
                        this._val1.text = c1.value;
                        this._val2.text = c2.value;
                    } else {
                        this._val1.value = c1.value;
                        this._val2.value = c2.value;
                    }

                    // initialize and/or buttons
                    this._btnAnd.checked = this._filter.and;
                    this._btnOr.checked = !this._filter.and;
                };

                /**
                * Updates filter to reflect the current editor values.
                */
                ConditionFilterEditor.prototype.updateFilter = function () {
                    // initialize conditions
                    var c1 = this._filter.condition1, c2 = this._filter.condition2;
                    c1.operator = this._cmb1.selectedValue;
                    c2.operator = this._cmb2.selectedValue;
                    if (this._val1 instanceof wijmo.input.ComboBox) {
                        c1.value = this._val1.text;
                        c2.value = this._val2.text;
                    } else {
                        c1.value = this._val1.value;
                        c2.value = this._val2.value;
                    }

                    // initialize and/or operator
                    this._filter.and = this._btnAnd.checked;
                };

                // ** implementation
                // create operator combo
                ConditionFilterEditor.prototype._createOperatorCombo = function (element) {
                    // get operator list based on column data type
                    var col = this._filter.column, list = wijmo.culture.FlexGridFilter.stringOperators;
                    if (col.dataType == wijmo.DataType.Date && !this._isTimeFormat(col.format)) {
                        list = wijmo.culture.FlexGridFilter.dateOperators;
                    } else if (col.dataType == wijmo.DataType.Number && !col.dataMap) {
                        list = wijmo.culture.FlexGridFilter.numberOperators;
                    } else if (col.dataType == wijmo.DataType.Boolean && !col.dataMap) {
                        list = wijmo.culture.FlexGridFilter.booleanOperators;
                    }

                    // create and initialize the combo
                    var cmb = new wijmo.input.ComboBox(element);
                    cmb.itemsSource = list;
                    cmb.displayMemberPath = 'name';
                    cmb.selectedValuePath = 'op';

                    // return combo
                    return cmb;
                };

                // create operator input
                ConditionFilterEditor.prototype._createValueInput = function (element) {
                    var col = this._filter.column, ctl = null;
                    if (col.dataType == wijmo.DataType.Date && !this._isTimeFormat(col.format)) {
                        ctl = new wijmo.input.InputDate(element);
                        ctl.format = col.format;
                    } else if (col.dataType == wijmo.DataType.Number && !col.dataMap) {
                        ctl = new wijmo.input.InputNumber(element);
                        ctl.format = col.format;
                    } else {
                        ctl = new wijmo.input.ComboBox(element);
                        if (col.dataMap) {
                            ctl.itemsSource = col.dataMap.getDisplayValues();
                            ctl.isEditable = true;
                        } else if (col.dataType == wijmo.DataType.Boolean) {
                            ctl.itemsSource = ['true', 'false'];
                        }
                    }
                    ctl.required = false;
                    return ctl;
                };

                // checks whether a format represents a time (and not just a date)
                ConditionFilterEditor.prototype._isTimeFormat = function (fmt) {
                    if (!fmt)
                        return false;
                    fmt = wijmo.culture.Globalize.calendar.patterns[fmt] || fmt;
                    return /[Hmst]+/.test(fmt);
                };

                // update and/or buttons
                ConditionFilterEditor.prototype._btnAndOrChanged = function (e) {
                    this._btnAnd.checked = e.target == this._btnAnd;
                    this._btnOr.checked = e.target == this._btnOr;
                };
                ConditionFilterEditor.controlTemplate = '<div>' + '<div wj-part="div-hdr"></div>' + '<div wj-part="div-cmb1"></div><br/>' + '<div wj-part="div-val1"></div><br/>' + '<div style="text-align:center">' + '<label><input wj-part="btn-and" type="radio"> <span wj-part="sp-and"></span> </label>&nbsp;&nbsp;&nbsp;' + '<label><input wj-part="btn-or" type="radio"> <span wj-part="sp-or"></span> </label>' + '</div>' + '<div wj-part="div-cmb2"></div><br/>' + '<div wj-part="div-val2"></div><br/>' + '</div>';
                return ConditionFilterEditor;
            })(wijmo.Control);
            _filter.ConditionFilterEditor = ConditionFilterEditor;
        })(grid.filter || (grid.filter = {}));
        var filter = grid.filter;
    })(wijmo.grid || (wijmo.grid = {}));
    var grid = wijmo.grid;
})(wijmo || (wijmo = {}));
var wijmo;
(function (wijmo) {
    (function (grid) {
        (function (filter) {
            'use strict';

            /**
            * Defines a filter condition.
            *
            * This class is used by the @see:FlexGridFilter class; you
            * rarely use it directly.
            */
            var FilterCondition = (function () {
                function FilterCondition() {
                    this._op = null;
                }
                Object.defineProperty(FilterCondition.prototype, "operator", {
                    /**
                    * Gets or sets the operator used by this @see:FilterCondition.
                    */
                    get: function () {
                        return this._op;
                    },
                    set: function (value) {
                        this._op = wijmo.asEnum(value, Operator, true);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(FilterCondition.prototype, "value", {
                    /**
                    * Gets or sets the value used by this @see:FilterCondition.
                    */
                    get: function () {
                        return this._val;
                    },
                    set: function (value) {
                        this._val = value;
                        this._strVal = wijmo.isString(value) ? value.toString().toLowerCase() : null;
                    },
                    enumerable: true,
                    configurable: true
                });

                /**
                * Returns a value that determines whether the given value passes this @see:FilterCondition.
                *
                * @param value The value to test.
                */
                FilterCondition.prototype.apply = function (value) {
                    // use lower-case strings for all operations
                    var val = this._strVal || this._val;
                    if (wijmo.isString(value)) {
                        value = value.toLowerCase();
                    }

                    switch (this._op) {
                        case null:
                            return true;
                        case Operator.EQ:
                            return wijmo.isDate(value) && wijmo.isDate(val) ? wijmo.DateTime.sameDate(value, val) : value == val;
                        case Operator.NE:
                            return value != val;
                        case Operator.GT:
                            return value > val;
                        case Operator.GE:
                            return value >= val;
                        case Operator.LT:
                            return value < val;
                        case Operator.LE:
                            return value <= val;
                        case Operator.BW:
                            return this._strVal && wijmo.isString(value) ? value.indexOf(this._strVal) == 0 : false;
                        case Operator.EW:
                            return this._strVal && wijmo.isString(value) && value.length >= this._strVal.length ? value.substr(value.length - this._strVal.length) == val : false;
                        case Operator.CT:
                            return this._strVal && wijmo.isString(value) ? value.indexOf(this._strVal) > -1 : false;
                        case Operator.NC:
                            return this._strVal && wijmo.isString(value) ? value.indexOf(this._strVal) < 0 : false;
                    }
                    throw 'Unknown operator';
                };
                return FilterCondition;
            })();
            filter.FilterCondition = FilterCondition;

            /**
            * Specifies filter condition operators.
            */
            (function (Operator) {
                /** Equals. */
                Operator[Operator["EQ"] = 0] = "EQ";

                /** Does not equal. */
                Operator[Operator["NE"] = 1] = "NE";

                /** Greater than. */
                Operator[Operator["GT"] = 2] = "GT";

                /** Greater than or equal to. */
                Operator[Operator["GE"] = 3] = "GE";

                /** Less than. */
                Operator[Operator["LT"] = 4] = "LT";

                /** Less than or equal to. */
                Operator[Operator["LE"] = 5] = "LE";

                /** Begins with. */
                Operator[Operator["BW"] = 6] = "BW";

                /** Ends with. */
                Operator[Operator["EW"] = 7] = "EW";

                /** Contains. */
                Operator[Operator["CT"] = 8] = "CT";

                /** Does not contain. */
                Operator[Operator["NC"] = 9] = "NC";
            })(filter.Operator || (filter.Operator = {}));
            var Operator = filter.Operator;
        })(grid.filter || (grid.filter = {}));
        var filter = grid.filter;
    })(wijmo.grid || (wijmo.grid = {}));
    var grid = wijmo.grid;
})(wijmo || (wijmo = {}));
//# sourceMappingURL=wijmo.grid.filter.js.map
