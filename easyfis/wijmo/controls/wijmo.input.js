var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var wijmo;
(function (wijmo) {
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
    (function (input) {
        'use strict';

        /**
        * DropDown control (abstract).
        *
        * Contains an input element and a button used to show or hide the drop-down.
        *
        * Derived classes must override the _createDropDown method to create whatever
        * editor they want to show in the drop down area (a list of items, a calendar,
        * a color editor, etc).
        */
        var DropDown = (function (_super) {
            __extends(DropDown, _super);
            /**
            * Initializes a new instance of a @see:DropDown control.
            *
            * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
            * @param options The JavaScript object containing initialization data for the control.
            */
            function DropDown(element, options) {
                _super.call(this, element);
                // property storage
                this._showBtn = true;
                /**
                * Occurs when the value of the @see:text property changes.
                */
                this.textChanged = new wijmo.Event();
                /**
                * Occurs after the drop down is shown or hidden.
                */
                this.isDroppedDownChanged = new wijmo.Event();

                // instantiate and apply template
                var tpl = this.getTemplate();
                this.applyTemplate('wj-control wj-dropdown wj-content', tpl, {
                    _tbx: 'input',
                    _btn: 'btn',
                    _dropDown: 'dropdown'
                }, 'input');

                // set reference element (used for positioning the drop-down)
                this._elRef = this._tbx;

                // create drop-down element, update button display
                this._createDropDown();
                this._updateBtn();

                // host element events
                var self = this;
                this.hostElement.addEventListener('focus', function () {
                    if (!self.isTouching) {
                        self._tbx.focus();
                    }
                });

                // use blur+capture to emulate focusout (not supported in FireFox)
                this.hostElement.addEventListener('blur', function () {
                    setTimeout(function () {
                        if (!self.containsFocus()) {
                            self.isDroppedDown = false;
                        }
                    }, 100);
                }, true);
                this._dropDown.addEventListener('blur', function () {
                    setTimeout(function () {
                        if (!self.containsFocus()) {
                            self.isDroppedDown = false;
                        }
                    }, 100);
                }, true);

                // textbox events
                this._tbx.addEventListener('input', function () {
                    self._setText(self.text, false);
                });
                this._tbx.addEventListener('focus', function () {
                    setTimeout(function () {
                        if (document.activeElement == self._tbx) {
                            self.selectAll();
                        }
                    }, 0);
                });

                // drop-down button event
                this._btn.addEventListener('click', function (e) {
                    self.isDroppedDown = !self.isDroppedDown;
                    e.preventDefault();
                    e.stopPropagation();

                    // if this was a tap, keep focus on button; OW transfer to textbox
                    if (self.isTouching && self.showDropDownButton) {
                        self._btn.focus();
                    } else {
                        self._tbx.focus();
                    }
                });

                // initializing from <input> tag
                if (this._orgTag == 'INPUT') {
                    this._copyOriginalAttributes(this._tbx);
                }
            }
            //--------------------------------------------------------------------------
            //#region ** overrides
            /**
            * Checks whether this control or its drop-down contain the focused element.
            */
            DropDown.prototype.containsFocus = function () {
                return _super.prototype.containsFocus.call(this) || wijmo.contains(this._dropDown, document.activeElement);
            };

            Object.defineProperty(DropDown.prototype, "text", {
                //#endregion
                //--------------------------------------------------------------------------
                //#region ** object model
                /**
                * Gets or sets the text shown on the control.
                */
                get: function () {
                    return this._tbx.value;
                },
                set: function (value) {
                    if (value != this.text) {
                        this._setText(value, true);
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(DropDown.prototype, "inputElement", {
                /**
                * Gets the HTML input element hosted by the control.
                *
                * Use this property in situations where you want to customize the
                * attributes of the input element.
                */
                get: function () {
                    return this._tbx;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(DropDown.prototype, "placeholder", {
                /**
                * Gets or sets the string shown as a hint when the control is empty.
                */
                get: function () {
                    return this._tbx.placeholder;
                },
                set: function (value) {
                    this._tbx.placeholder = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(DropDown.prototype, "isDroppedDown", {
                /**
                * Gets or sets a value indicating whether the drop down is currently visible.
                */
                get: function () {
                    return this._dropDown.style.display != 'none';
                },
                set: function (value) {
                    if (value != this.isDroppedDown) {
                        var dd = this._dropDown;
                        if (value) {
                            // set minWidth (if not already set)
                            if (!dd.style.minWidth) {
                                dd.style.minWidth = this.hostElement.getBoundingClientRect().width + 'px';
                            }
                            dd.style.display = 'block';
                            this._updateDropDown();
                        } else {
                            // retain focus in the control; or Chrome will move focus to BODY
                            if (this.containsFocus()) {
                                if (this.isTouching && this.showDropDownButton) {
                                    this._btn.focus();
                                } else {
                                    this._tbx.focus();
                                }
                            }
                            wijmo.hidePopup(dd);
                        }
                        this.onIsDroppedDownChanged();
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(DropDown.prototype, "dropDown", {
                /**
                * Gets the drop down element shown when the @see:isDroppedDown
                * property is set to true.
                */
                get: function () {
                    return this._dropDown;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(DropDown.prototype, "showDropDownButton", {
                /**
                * Gets or sets a value indicating whether the control should display a drop-down button.
                */
                get: function () {
                    return this._showBtn;
                },
                set: function (value) {
                    this._showBtn = wijmo.asBoolean(value);
                    this._updateBtn();
                },
                enumerable: true,
                configurable: true
            });

            /**
            * Sets the focus to the control and selects all its content.
            */
            DropDown.prototype.selectAll = function () {
                if (this._elRef == this._tbx) {
                    this._tbx.setSelectionRange(0, this.text.length);
                }
            };

            /**
            * Raises the @see:textChanged event.
            */
            DropDown.prototype.onTextChanged = function (e) {
                this.textChanged.raise(this, e);
            };

            /**
            * Raises the @see:isDroppedDownChanged event.
            */
            DropDown.prototype.onIsDroppedDownChanged = function (e) {
                this.isDroppedDownChanged.raise(this, e);
            };

            //#endregion
            //--------------------------------------------------------------------------
            //#region ** implementation
            // update text in textbox
            DropDown.prototype._setText = function (text, fullMatch) {
                // make sure we don't have nulls
                if (text == null)
                    text = '';

                // update element
                if (text != this._tbx.value) {
                    this._tbx.value = text;
                }

                // fire change event
                if (text != this._oldText) {
                    this._oldText = text;
                    this.onTextChanged();
                }
            };

            // update drop-down button visibility
            DropDown.prototype._updateBtn = function () {
                this._btn.tabIndex = -1;
                this._btn.style.display = this._showBtn ? '' : 'none';
            };

            // create the drop-down element
            DropDown.prototype._createDropDown = function () {
                // override in derived classes
            };

            // update drop down content before showing it
            DropDown.prototype._updateDropDown = function () {
                if (this.isDroppedDown) {
                    wijmo.showPopup(this._dropDown, this.hostElement);
                }
            };
            DropDown.controlTemplate = '<div style="position:relative" class="wj-template">' + '<div class="wj-input">' + '<div class="wj-input-group wj-input-btn-visible">' + '<input wj-part="input" type="text" class="wj-form-control" />' + '<span wj-part="btn" class="wj-input-group-btn" tabindex="-1">' + '<button class="wj-btn wj-btn-default" type="button" tabindex="-1">' + '<span class="wj-glyph-down"></span>' + '</button>' + '</span>' + '</div>' + '</div>' + '<div wj-part="dropdown" class="wj-content wj-dropdown-panel" ' + 'style="display:none;position:absolute;z-index:100;width:auto">' + '</div>' + '</div>';
            return DropDown;
        })(wijmo.Control);
        input.DropDown = DropDown;
    })(wijmo.input || (wijmo.input = {}));
    var input = wijmo.input;
})(wijmo || (wijmo = {}));
var wijmo;
(function (wijmo) {
    (function (input) {
        'use strict';

        /**
        * The @see:Calendar control displays a one-month calendar and allows users
        * to select a date.
        *
        * You may use the @see:min and @see:max properties to restrict the range
        * of dates that the user can select.
        *
        * Use the @see:value property to get or set the currently selected date.
        *
        * The example below shows a <b>Date</b> value with date and time information
        * using an @see:InputDate and an @see:InputTime control. Notice how both controls
        * are bound to the same controller variable, and each edits the appropriate information
        * (either date or time). The example also shows a @see:Calendar control that allows
        * users to select the date with a single click.
        *
        * @fiddle:vgc3Y
        */
        var Calendar = (function (_super) {
            __extends(Calendar, _super);
            /**
            * Initializes a new instance of a @see:Calendar.
            *
            * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
            * @param options The JavaScript object containing initialization data for the control.
            */
            function Calendar(element, options) {
                _super.call(this, element);
                /**
                * Occurs after a new date is selected.
                */
                this.valueChanged = new wijmo.Event();
                /**
                * Occurs after the @see:displayMonth property changes.
                */
                this.displayMonthChanged = new wijmo.Event();

                // initialize value (current date)
                this._value = new Date();
                this._currMonth = this._getMonth(this._value);

                // create child elements
                this._createChildren();

                // update the control
                this.refresh(true);

                // handle mouse and keyboard
                // The 'click' event may not be triggered on iOS Safari if focus changed
                // during previous tap. So use 'mouseup' instead.
                //this.hostElement.addEventListener('click', this._click.bind(this));
                this.hostElement.addEventListener('mouseup', this._click.bind(this));
                this.hostElement.addEventListener('keydown', this._keydown.bind(this));

                // initialize control options
                this.initialize(options);
            }
            Object.defineProperty(Calendar.prototype, "value", {
                //--------------------------------------------------------------------------
                //#region ** object model
                /**
                * Gets or sets the currently selected date.
                */
                get: function () {
                    return this._value;
                },
                set: function (value) {
                    if (!wijmo.DateTime.equals(this._value, value)) {
                        // check type
                        value = wijmo.asDate(value, true);

                        // honor ranges (but keep the time)
                        if (value) {
                            if (this._min != null) {
                                var min = wijmo.DateTime.fromDateTime(this._min, value);
                                if (value < min) {
                                    value = min;
                                }
                            }
                            if (this._max != null && value > this._max) {
                                var max = wijmo.DateTime.fromDateTime(this._max, value);
                                if (value > max) {
                                    value = max;
                                }
                            }
                        }

                        // update control
                        if (!wijmo.DateTime.equals(this._value, value)) {
                            this._value = value;
                            this.displayMonth = this._getMonth(value);
                            this.invalidate(false);
                            this.onValueChanged();
                        }
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Calendar.prototype, "min", {
                /**
                * Gets or sets the earliest date that the user can select in the calendar.
                */
                get: function () {
                    return this._min;
                },
                set: function (value) {
                    if (value != this.min) {
                        this._min = wijmo.asDate(value, true);
                        this.refresh();
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Calendar.prototype, "max", {
                /**
                * Gets or sets the latest date that the user can select in the calendar.
                */
                get: function () {
                    return this._max;
                },
                set: function (value) {
                    if (value != this.max) {
                        this._max = wijmo.asDate(value, true);
                        this.refresh();
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Calendar.prototype, "firstDayOfWeek", {
                /**
                * Gets or sets a value that represents the first day of the week,
                * the one displayed in the first column of the calendar.
                *
                * Setting this property to null causes the calendar to use the default
                * for the current culture. In the English culture, the first day of the
                * week is Sunday (0); in most European cultures, the first day of the
                * week is Monday (1).
                */
                get: function () {
                    return this._fdw;
                },
                set: function (value) {
                    if (value != this._fdw) {
                        this._fdw = wijmo.asNumber(value, true);
                        if (this._fdw && (this._fdw > 6 || this._fdw < 0)) {
                            throw 'firstDayOfWeek must be between 0 and 6 (Sunday to Saturday).';
                        }
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Calendar.prototype, "displayMonth", {
                /**
                * Gets or sets the month displayed in the calendar.
                */
                get: function () {
                    return this._currMonth;
                },
                set: function (value) {
                    if (!wijmo.DateTime.equals(this.displayMonth, value)) {
                        value = wijmo.asDate(value);
                        if (this._isValidMonth(value)) {
                            this._currMonth = this._getMonth(value);
                            this.invalidate(true);
                            this.onDisplayMonthChanged();
                        }
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Calendar.prototype, "showHeader", {
                /**
                * Gets or sets a value indicating whether the control displays the header
                * area with the current month and navigation buttons.
                */
                get: function () {
                    return this._tblHdr.style.display != 'none';
                },
                set: function (value) {
                    this._tblHdr.style.display = wijmo.asBoolean(value) ? '' : 'none';
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Calendar.prototype, "monthView", {
                /**
                * Gets or sets a value indicating whether the calendar displays a month or a year.
                */
                get: function () {
                    return this._tblMonth.style.display != 'none';
                },
                set: function (value) {
                    if (value != this.monthView) {
                        this._tblMonth.style.display = value ? '' : 'none';
                        this._tblYear.style.display = value ? 'none' : '';
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Calendar.prototype, "itemFormatter", {
                /**
                * Gets or sets a formatter function to customize dates in the calendar.
                *
                * The formatter function can add any content and any date. It allows
                * complete customization of the appearance and behavior of the calendar.
                *
                * If specified, the function takes two parameters:
                * <ul>
                *     <li>the date being formatted </li>
                *     <li>the HTML element that represents the date</li>
                * </ul>
                *
                * For example, the code below shows weekends in a disabled state:
                * <pre>
                * cal.itemFormatter = function(date, element) {
                *   var weekday = date.getDay();
                *   if (weekday == 0 || weekday == 6) {
                *     wijmo.addClass(element, 'wj-state-disabled');
                *   }
                * }
                * </pre>
                */
                get: function () {
                    return this._itemFormatter;
                },
                set: function (value) {
                    if (value != this._itemFormatter) {
                        this._itemFormatter = wijmo.asFunction(value);
                        this.invalidate();
                    }
                },
                enumerable: true,
                configurable: true
            });

            /**
            * Raises the @see:valueChanged event.
            */
            Calendar.prototype.onValueChanged = function (e) {
                this.valueChanged.raise(this, e);
            };

            /**
            * Raises the @see:displayMonthChanged event.
            */
            Calendar.prototype.onDisplayMonthChanged = function (e) {
                this.displayMonthChanged.raise(this, e);
            };

            /**
            * Refreshes the control.
            *
            * @param fullUpdate Indicates whether to update the control layout as well as the content.
            */
            Calendar.prototype.refresh = function (fullUpdate) {
                if (typeof fullUpdate === "undefined") { fullUpdate = true; }
                var cells, day, s, enabled, today = new Date(), fdw = this.firstDayOfWeek != null ? this.firstDayOfWeek : wijmo.Globalize.getFirstDayOfWeek();

                // call base class to suppress any pending invalidations
                _super.prototype.refresh.call(this, fullUpdate);

                // calculate first day of the calendar
                this._firstDay = wijmo.DateTime.addDays(this.displayMonth, -(this.displayMonth.getDay() - fdw + 7) % 7);

                // update current month (e.g. January 2014)
                this._spMonth.textContent = wijmo.Globalize.format(this.displayMonth, 'y');

                // update week day headers (localizable)
                cells = this._tblMonth.querySelectorAll('td');
                for (var i = 0; i < 7 && i < cells.length; i++) {
                    day = wijmo.DateTime.addDays(this._firstDay, i);
                    cells[i].textContent = wijmo.Globalize.format(day, 'ddd');
                }

                for (var i = 7; i < cells.length; i++) {
                    day = wijmo.DateTime.addDays(this._firstDay, i - 7);
                    cells[i].textContent = day.getDate().toString();
                    enabled = day.getMonth() == this.displayMonth.getMonth() && this._isValidDay(day);
                    wijmo.toggleClass(cells[i], 'wj-state-disabled', !enabled);
                    s = cells[i].style;
                    s.fontWeight = wijmo.DateTime.sameDate(day, today) ? 'bold' : '';
                    this._highlightElement(cells[i], wijmo.DateTime.sameDate(day, this.value));

                    // customize the display
                    if (this.itemFormatter) {
                        this.itemFormatter(day, cells[i]);
                    }
                }

                // hide rows that belong to the next month
                var rows = this._tblMonth.querySelectorAll('tr');
                if (rows.length) {
                    day = wijmo.DateTime.addDays(this._firstDay, 28);
                    rows[rows.length - 2].style.display = (day.getMonth() == this.displayMonth.getMonth()) ? '' : 'none';
                    day = wijmo.DateTime.addDays(this._firstDay, 35);
                    rows[rows.length - 1].style.display = (day.getMonth() == this.displayMonth.getMonth()) ? '' : 'none';
                }

                // update current year
                cells = this._tblYear.querySelectorAll('td');
                if (cells.length) {
                    cells[0].textContent = this.displayMonth.getFullYear().toString();
                }

                for (var i = 1; i < cells.length; i++) {
                    day = new Date(this.displayMonth.getFullYear(), i - 1, 1);
                    cells[i].textContent = wijmo.Globalize.format(day, 'MMM');
                    enabled = this._isValidMonth(day);
                    wijmo.toggleClass(cells[i], 'wj-state-disabled', !enabled);
                    this._highlightElement(cells[i], wijmo.DateTime.sameDate(this._getMonth(day), this.displayMonth));
                }
            };

            //#endregion
            //--------------------------------------------------------------------------
            //#region ** implementation
            // check whether a day is within the valid range
            Calendar.prototype._isValidDay = function (value) {
                if (this.min && value < wijmo.DateTime.fromDateTime(this.min, value))
                    return false;
                if (this.max && value > wijmo.DateTime.fromDateTime(this.max, value))
                    return false;
                return true;
            };

            // check whether a month is within the valid range
            Calendar.prototype._isValidMonth = function (value) {
                var y = value.getFullYear(), m = value.getMonth(), first = new Date(y, m, 1), last = wijmo.DateTime.addDays(new Date(y, m + 1), -1);
                return this._isValidDay(first) || this._isValidDay(last);
            };

            // create child elements
            Calendar.prototype._createChildren = function () {
                // instantiate and apply template
                var tpl = this.getTemplate();
                this.applyTemplate('wj-control wj-calendar', tpl, {
                    _tblHdr: 'tbl-header',
                    _tdMonth: 'btn-month',
                    _spMonth: 'span-month',
                    _btnPrev: 'btn-prev',
                    _btnToday: 'btn-today',
                    _btnNext: 'btn-next',
                    _tblMonth: 'tbl-month',
                    _tblYear: 'tbl-year'
                });

                // populate month calendar
                var tr = document.createElement('TR');
                wijmo.addClass(tr, 'wj-header');
                for (var d = 0; d < 7; d++) {
                    tr.appendChild(document.createElement('TD'));
                }
                this._tblMonth.appendChild(tr);
                for (var w = 0; w < 6; w++) {
                    tr = document.createElement('TR');
                    for (var d = 0; d < 7; d++) {
                        tr.appendChild(document.createElement('TD'));
                    }
                    this._tblMonth.appendChild(tr);
                }

                // populate year calendar
                tr = document.createElement('TR');
                wijmo.addClass(tr, 'wj-header');
                var td = document.createElement('TD');
                td.setAttribute('colspan', '4');
                tr.appendChild(td);
                this._tblYear.appendChild(tr);
                for (var i = 0; i < 3; i++) {
                    tr = document.createElement('TR');
                    for (var j = 0; j < 4; j++) {
                        tr.appendChild(document.createElement('TD'));
                    }
                    this._tblYear.appendChild(tr);
                }
            };

            // handle clicks on the calendar
            Calendar.prototype._click = function (e) {
                var handled = false;

                // get element that was clicked
                var elem = e.target;

                // switch month/year view
                if (wijmo.contains(this._tdMonth, elem)) {
                    this.monthView = !this.monthView;
                    handled = true;
                } else if (wijmo.contains(this._btnPrev, elem)) {
                    this._navigateDate(this.monthView, -1);
                    handled = true;
                } else if (wijmo.contains(this._btnNext, elem)) {
                    this._navigateDate(this.monthView, +1);
                    handled = true;
                } else if (wijmo.contains(this._btnToday, elem)) {
                    this._navigateDate(this.monthView, 0);
                    handled = true;
                }

                // select day/month
                if (!handled && elem) {
                    elem = this._closest(elem, 'TD');
                    if (elem) {
                        if (this.monthView) {
                            var index = this._getCellIndex(this._tblMonth, elem);
                            if (index > 6) {
                                this.value = wijmo.DateTime.fromDateTime(wijmo.DateTime.addDays(this._firstDay, index - 7), this.value);
                                handled = true;
                            }
                        } else {
                            var index = this._getCellIndex(this._tblYear, elem);
                            if (index > 0) {
                                this.displayMonth = new Date(this.displayMonth.getFullYear(), index - 1, 1);
                                this.monthView = true;
                                handled = true;
                            }
                        }
                    }
                }

                // if we handled the mouse, prevent browser from seeing it
                if (handled) {
                    e.preventDefault();
                    //e.stopPropagation();
                }
            };

            // gets the nearest ancestor of an element by tag
            Calendar.prototype._closest = function (e, tag) {
                while (e && e.tagName != tag) {
                    e = e.parentElement;
                }
                return e;
            };

            // gets the index of a cell in a table
            Calendar.prototype._getCellIndex = function (tbl, cell) {
                var cells = tbl.querySelectorAll('TD');
                for (var i = 0; i < cells.length; i++) {
                    if (cells[i] == cell)
                        return i;
                }
                return -1;
            };

            // handle keyboard events
            Calendar.prototype._keydown = function (e) {
                // not interested in ctrl/shift keys
                if (!e.ctrlKey && !e.shiftKey) {
                    var handled = true;
                    if (this.monthView) {
                        switch (e.keyCode) {
                            case wijmo.Key.Left:
                                this.value = this._addDays(-1);
                                break;
                            case wijmo.Key.Right:
                                this.value = this._addDays(+1);
                                break;
                            case wijmo.Key.Up:
                                this.value = this._addDays(-7);
                                break;
                            case wijmo.Key.Down:
                                this.value = this._addDays(+7);
                                break;
                            case wijmo.Key.PageDown:
                                this.displayMonth = this._addMonths(+1);
                                break;
                            case wijmo.Key.PageUp:
                                this.displayMonth = this._addMonths(-1);
                                break;
                            default:
                                handled = false;
                                break;
                        }
                    } else {
                        switch (e.keyCode) {
                            case wijmo.Key.Left:
                                this.displayMonth = this._addMonths(-1);
                                break;
                            case wijmo.Key.Right:
                                this.displayMonth = this._addMonths(+1);
                                break;
                            case wijmo.Key.Up:
                                this.displayMonth = this._addMonths(-4);
                                break;
                            case wijmo.Key.Down:
                                this.displayMonth = this._addMonths(+4);
                                break;
                            case wijmo.Key.PageDown:
                                this.displayMonth = this._addMonths(+12);
                                break;
                            case wijmo.Key.PageUp:
                                this.displayMonth = this._addMonths(-12);
                                break;
                            case wijmo.Key.Enter:
                                this.monthView = true;
                                break;
                            default:
                                handled = false;
                                break;
                        }
                    }

                    // if we handled the key, prevent browser from seeing it
                    if (handled) {
                        e.preventDefault();
                        //e.stopPropagation();
                    }
                }
            };

            // gets the month being displayed in the calendar
            Calendar.prototype._getMonth = function (date) {
                if (!date)
                    date = new Date();
                return new Date(date.getFullYear(), date.getMonth(), 1);
            };

            // adds a given number of days to the current value (preserving time)
            Calendar.prototype._addDays = function (days) {
                var dt = wijmo.DateTime.addDays(this.value, days);
                return wijmo.DateTime.fromDateTime(dt, this.value);
            };

            // adds a given number of months to the current display month
            Calendar.prototype._addMonths = function (months) {
                return wijmo.DateTime.addMonths(this.displayMonth, months);
            };

            // change display month by a month or a year, or skip to the current
            Calendar.prototype._navigateDate = function (month, skip) {
                switch (skip) {
                    case 0:
                        if (month) {
                            this.value = new Date();
                        } else {
                            this.displayMonth = this._getMonth(new Date());
                        }
                        break;
                    case +1:
                        if (month) {
                            this.displayMonth = wijmo.DateTime.addMonths(this.displayMonth, +1);
                        } else {
                            this.displayMonth = new Date(this.displayMonth.getFullYear() + 1, 0, 1);
                        }
                        break;
                    case -1:
                        if (month) {
                            this.displayMonth = wijmo.DateTime.addMonths(this.displayMonth, -1);
                        } else {
                            this.displayMonth = new Date(this.displayMonth.getFullYear() - 1, 11, 1);
                        }
                        break;
                }
            };

            // highlight selected day/month elements
            Calendar.prototype._highlightElement = function (e, highlight) {
                wijmo.toggleClass(e, 'wj-state-selected', highlight);
            };
            Calendar.controlTemplate = '<div style="cursor:default;-ms-user-select: none">' + '<div class="wj-calendar-outer wj-content">' + '<div wj-part="tbl-header" class="wj-calendar-header">' + '<div wj-part="btn-month" class="wj-month-select">' + '<span wj-part="span-month"></span> <span class="wj-glyph-down"></span>' + '</div>' + '<div class="wj-btn-group">' + '<button type="button" wj-part="btn-prev" class="wj-btn wj-btn-default"><span class="wj-glyph-left"></span></button>' + '<button type="button" wj-part="btn-today" class="wj-btn wj-btn-default"><span class="wj-glyph-circle"></span></button>' + '<button type="button" wj-part="btn-next" class="wj-btn wj-btn-default"><span class="wj-glyph-right"></span></button>' + '</div>' + '</div>' + '<table wj-part="tbl-month" class="wj-calendar-month" />' + '<table wj-part="tbl-year" class="wj-calendar-year" style="display:none" />' + '</div>' + '</div>';
            return Calendar;
        })(wijmo.Control);
        input.Calendar = Calendar;
    })(wijmo.input || (wijmo.input = {}));
    var input = wijmo.input;
})(wijmo || (wijmo = {}));
var wijmo;
(function (wijmo) {
    (function (input) {
        'use strict';

        /**
        * The @see:ColorPicker control allows users to select a color by clicking
        * on panels to adjust color channels (hue, saturation, brightness, alpha).
        *
        * Use the @see:value property to get or set the currently selected color.
        *
        * The control is used as a drop-down for the @see:InputColor control.
        *
        * @fiddle:84xvsz90
        */
        var ColorPicker = (function (_super) {
            __extends(ColorPicker, _super);
            /**
            * Initializes a new instance of a @see:ColorPicker.
            *
            * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
            * @param options The JavaScript object containing initialization data for the control.
            */
            function ColorPicker(element, options) {
                _super.call(this, element);
                this._hsb = [.5, 1, 1];
                this._alpha = 1;
                /**
                * Occurs when the color changes.
                */
                this.valueChanged = new wijmo.Event();

                // instantiate and apply template
                var tpl = this.getTemplate();
                this.applyTemplate('wj-control wj-colorpicker wj-content', tpl, {
                    _eSB: 'div-sb',
                    _eHue: 'div-hue',
                    _eAlpha: 'div-alpha',
                    _ePreview: 'div-pv',
                    _ePal: 'div-pal',
                    _eText: 'div-text'
                });

                // build palette
                this._palette = '#FFF,#000, #F00,#FFC000,#FFFF00,#92D050,#00B050,#00B0F0,#0070C0,#7030A0'.split(',');
                this._updatePalette();

                // build hue gradient
                // (use an image since IE9 doesn't support multi-stop gradients)
                this._eHue.style.backgroundImage = 'url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAD4CAIAAACi6hsPAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuM4zml1AAAAGvSURBVDhPXdBPaM9xHMfxz4pWaxcmtoOhpdXSVpiyHWxqmVpDjaU5rK34XfypjTJ/p+ZPay6jhsOsRrKwaJElf9IQq03WIkv4FeMwMq221tfje1ByeFzfvd7PEKWGEKWTQRZLySWfVRRTQjmVbKWGOhLsZT+HaeY0bbTTQSfdXOcWffTzmAFeMcwoYyT5ygS/mA5hNgphip98J8kHRnnNSwZ4yH1uc4OrdHGR87RximYO0cgedlLLdqqoYAPrWMtKVrCcJSxiPmnMJUQp/Bsyk2xyyKOAQooopYwKtlDNDur5G7SBJo7RQiv/B+2hl3s84CkvGGKEOOYnxolj/mYmhBmDJ5ngCx95xxsGecYj4pB3iENeoZMO2mmlhaMcpIE4ZII6aqhmM3HMMkooopB88sghm0wySCeVlCjMCVFIYx4LWUwOeRSwhmLWU84mqqihll3sppEmjnOSs5zjEl1c4yZ99POE5wwxwns+840fTDFLFKaZZIJxkozxlmEGGSC+GF++Sy89dHOZC8Rr4lVnOMERDrCPBPXEX22jko2UEn+/mnxyWUYWC0gnNUQh/AEc0HJs6cex0gAAAABJRU5ErkJggg==)';
                this._eHue.style.backgroundSize = 'contain';

                // add filter gradients to IE 9
                if (navigator.appVersion.indexOf('MSIE 9') > -1) {
                    this._eSB.children[0].style.filter = 'progid:DXImageTransform.Microsoft.gradient(startColorstr=#ffffffff,endColorstr=#00ffffff,GradientType=1)';
                    this._eSB.children[1].style.filter = 'progid:DXImageTransform.Microsoft.gradient(startColorstr=#00000000,endColorstr=#ff000000,GradientType=0)';
                }

                // add cursors to panels
                tpl = ColorPicker._tplCursor;
                this._cSB = wijmo.createElement(tpl);
                this._cHue = wijmo.createElement(tpl);
                this._cHue.style.width = '100%';
                this._cAlpha = wijmo.createElement(tpl);
                this._cAlpha.style.height = '100%';
                this._eSB.appendChild(this._cSB);
                this._eHue.appendChild(this._cHue);
                this._eAlpha.appendChild(this._cAlpha);

                // handle mouse
                var self = this;
                this.hostElement.addEventListener('mousedown', function (e) {
                    document.addEventListener('mousemove', mouseMove);
                    document.addEventListener('mouseup', mouseUp);
                    self._mouseDown(e);
                });
                var mouseMove = function (e) {
                    self._mouseMove(e);
                };
                var mouseUp = function (e) {
                    document.removeEventListener('mousemove', mouseMove);
                    document.removeEventListener('mouseup', mouseUp);
                    self._mouseUp(e);
                };

                // handle clicks on the palette
                this.hostElement.addEventListener('click', function (e) {
                    var el = e.target;
                    if (el && el.tagName == 'DIV' && wijmo.contains(self._ePal, el)) {
                        var color = el.style.backgroundColor;
                        if (color) {
                            self.value = new wijmo.Color(color).toString();
                        }
                    }
                });

                // initialize value to white
                this.value = '#ffffff';

                // initialize control options
                this.initialize(options);

                // initialize control
                this._updatePanels();
            }
            Object.defineProperty(ColorPicker.prototype, "showAlphaChannel", {
                /**
                * Gets or sets a value indicating whether the @see:ColorPicker allows users
                * to edit the color's alpha channel (transparency).
                */
                get: function () {
                    return this._eAlpha.parentElement.style.display != 'none';
                },
                set: function (value) {
                    this._eAlpha.parentElement.style.display = wijmo.asBoolean(value) ? '' : 'none';
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ColorPicker.prototype, "showColorString", {
                /**
                * Gets or sets a value indicating whether the @see:ColorPicker shows a string representation
                * of the current color.
                */
                get: function () {
                    return this._eText.style.display != 'none';
                },
                set: function (value) {
                    this._eText.style.display = wijmo.asBoolean(value) ? '' : 'none';
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ColorPicker.prototype, "value", {
                /**
                * Gets or sets the currently selected color.
                */
                get: function () {
                    return this._value;
                },
                set: function (value) {
                    if (value != this.value) {
                        // save new value
                        this._value = wijmo.asString(value);
                        this._eText.innerText = this._value;

                        // parse new color, convert to hsb values
                        var c = new wijmo.Color(this._value), hsb = c.getHsb();

                        // check whether the color really changed
                        if (this._hsb[0] != hsb[0] || this._hsb[1] != hsb[1] || this._hsb[2] != hsb[2] || this._alpha != c.a) {
                            // update hsb channels (but keep hue when s/b go to zero)
                            if (hsb[2] == 0) {
                                hsb[0] = this._hsb[0];
                                hsb[1] = this._hsb[1];
                            } else if (hsb[1] == 0) {
                                hsb[0] = this._hsb[0];
                            }
                            this._hsb = hsb;
                            this._alpha = c.a;

                            // raise valueChanged event
                            this.onValueChanged();
                        }
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ColorPicker.prototype, "palette", {
                /**
                * Gets or sets an array that contains the colors in the palette.
                *
                * The palette contains ten colors, represented by an array with
                * ten strings. The first two colors are usually white and black.
                */
                get: function () {
                    return this._palette;
                },
                set: function (value) {
                    value = wijmo.asArray(value);
                    for (var i = 0; i < value.length && i < this._palette.length; i++) {
                        var entry = wijmo.asString(value[i]);
                        this._palette[i] = entry;
                    }
                    this._updatePalette();
                },
                enumerable: true,
                configurable: true
            });

            /**
            * Raises the @see:valueChanged event.
            */
            ColorPicker.prototype.onValueChanged = function () {
                this._updatePanels();
                this.valueChanged.raise(this);
            };

            // ** event handlers
            ColorPicker.prototype._mouseDown = function (e) {
                this._htDown = this._getTargetPanel(e);
                if (this._htDown) {
                    e.preventDefault();
                    this.focus();
                    this._mouseMove(e);
                }
            };
            ColorPicker.prototype._mouseMove = function (e) {
                if (this._htDown) {
                    var rc = this._htDown.getBoundingClientRect();
                    if (this._htDown == this._eHue) {
                        this._hsb[0] = wijmo.clamp((e.clientY - rc.top) / rc.height, 0, .99);
                        this._updateColor();
                    } else if (this._htDown == this._eSB) {
                        this._hsb[1] = wijmo.clamp((e.clientX - rc.left) / rc.width, 0, 1);
                        this._hsb[2] = wijmo.clamp(1 - (e.clientY - rc.top) / rc.height, 0, 1);
                        this._updateColor();
                    } else if (this._htDown == this._eAlpha) {
                        this._alpha = wijmo.clamp((e.clientX - rc.left) / rc.width, 0, 1);
                        this._updateColor();
                    }
                }
            };
            ColorPicker.prototype._mouseUp = function (e) {
                this._htDown = null;
            };

            // update color value to reflect new hsb values
            ColorPicker.prototype._updateColor = function () {
                var c = wijmo.Color.fromHsb(this._hsb[0], this._hsb[1], this._hsb[2], this._alpha);
                this.value = c.toString();
                this._updatePanels();
            };

            // updates the color elements in the palette
            ColorPicker.prototype._updatePalette = function () {
                var white = new wijmo.Color('#fff'), black = new wijmo.Color('#000');

                // clear the current palette
                this._ePal.innerHTML = '';

                for (var i = 0; i < this._palette.length; i++) {
                    var div = wijmo.createElement('<div style="float:left;width:10%;box-sizing:border-box;padding:1px">'), clr = new wijmo.Color(this._palette[i]), hsb = clr.getHsb();

                    // add palette color
                    div.appendChild(this._makePalEntry(clr, 4));

                    for (var r = 0; r < 5; r++) {
                        if (hsb[1] == 0) {
                            var pct = r * .1 + (hsb[2] > .5 ? .05 : .55);
                            clr = wijmo.Color.interpolate(white, black, pct);
                        } else {
                            clr = wijmo.Color.fromHsb(hsb[0], 0.1 + r * 0.2, 1 - r * 0.1);
                        }
                        div.appendChild(this._makePalEntry(clr, 0));
                    }

                    // add color and shades to palette
                    this._ePal.appendChild(div);
                }
            };

            // creates a palette entry with the given color
            ColorPicker.prototype._makePalEntry = function (color, margin) {
                var e = document.createElement('div');
                wijmo.setCss(e, {
                    width: '100%',
                    cursor: 'pointer',
                    backgroundColor: color.toString(),
                    marginBottom: margin
                });
                e.innerHTML = '&nbsp';
                return e;
            };

            // update color and cursor on all panels
            ColorPicker.prototype._updatePanels = function () {
                var clrHue = wijmo.Color.fromHsb(this._hsb[0], 1, 1, 1), clrSolid = wijmo.Color.fromHsb(this._hsb[0], this._hsb[1], this._hsb[2], 1);
                this._eSB.style.backgroundColor = clrHue.toString();
                this._eAlpha.style.background = 'linear-gradient(to right, transparent 0%, ' + clrSolid.toString() + ' 100%)';
                if (navigator.appVersion.indexOf('MSIE 9') > -1) {
                    this._eAlpha.style.filter = 'progid:DXImageTransform.Microsoft.gradient(startColorstr=#00000000,endColorstr=' + clrSolid.toString() + ', GradientType = 1)';
                }
                this._ePreview.style.backgroundColor = this.value;

                this._cHue.style.top = (this._hsb[0] * 100).toFixed(0) + '%';
                this._cSB.style.left = (this._hsb[1] * 100).toFixed(0) + '%';
                this._cSB.style.top = (100 - this._hsb[2] * 100).toFixed(0) + '%';
                this._cAlpha.style.left = (this._alpha * 100).toFixed(0) + '%';
            };

            // gets the design panel that contains the mouse target
            ColorPicker.prototype._getTargetPanel = function (e) {
                var target = e.target;
                if (wijmo.contains(this._eSB, target))
                    return this._eSB;
                if (wijmo.contains(this._eHue, target))
                    return this._eHue;
                if (wijmo.contains(this._eAlpha, target))
                    return this._eAlpha;
                return null;
            };
            ColorPicker.controlTemplate = '<div style="position:relative;width:100%;height:100%">' + '<div style="float:left;width:50%;height:100%;box-sizing:border-box;padding:2px">' + '<div wj-part="div-pal">' + '<div style="float:left;width:10%;box-sizing:border-box;padding:2px">' + '<div style="background-color:black;width:100%">&nbsp;</div>' + '<div style="height:6px"></div>' + '</div>' + '</div>' + '<div wj-part="div-text" style="position:absolute;bottom:0px;display:none"></div>' + '</div>' + '<div style="float:left;width:50%;height:100%;box-sizing:border-box;padding:2px">' + '<div wj-part="div-sb" class="wj-colorbox" style="position:relative;float:left;width:89%;height:89%">' + '<div style="position:absolute;width:100%;height:100%;background:linear-gradient(to right, white 0%,transparent 100%)"></div>' + '<div style="position:absolute;width:100%;height:100%;background:linear-gradient(to top, black 0%,transparent 100%)"></div>' + '</div>' + '<div style="float:left;width:1%;height:89%"></div>' + '<div style="float:left;width:10%;height:89%">' + '<div wj-part="div-hue" class="wj-colorbox" style="position:relative;width:100%;height:100%;cursor:pointer"></div>' + '</div>' + '<div style="float:left;width:89%;height:1%"></div>' + '<div style="float:left;width:89%;height:10%">' + '<div style="width:100%;height:100%;background-image:url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAgAAAAICAIAAABLbSncAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuM4zml1AAAAAcSURBVBhXY/iPBBYgAWpKQGkwgMqDAdUk/v8HAM7Mm6GatDUYAAAAAElFTkSuQmCC)">' + '<div wj-part="div-alpha" class="wj-colorbox" style="position:relative;width:100%;height:100%;cursor:pointer"></div>' + '</div>' + '</div>' + '<div style="float:left;width:1%;height:10%"></div>' + '<div style="float:left;width:10%;height:10%">' + '<div wj-part="div-pv" class="wj-colorbox" style="width:100%;height:100%"></div>' + '</div>' + '</div>' + '</div>';
            ColorPicker._tplCursor = '<div style="position:absolute;left:50%;top:50%;width:7px;height:7px;transform:translate(-50%,-50%);border:2px solid #f0f0f0;border-radius:50px;box-shadow:0px 0px 4px 2px #0f0f0f;"></div>';
            return ColorPicker;
        })(wijmo.Control);
        input.ColorPicker = ColorPicker;
    })(wijmo.input || (wijmo.input = {}));
    var input = wijmo.input;
})(wijmo || (wijmo = {}));

var wijmo;
(function (wijmo) {
    (function (input) {
        'use strict';

        /**
        * The @see:ListBox control displays a list of items which may contain
        * plain text or HTML, and allows users to select items with the mouse or
        * the keyboard.
        *
        * Use the @see:selectedIndex property to determine which item is currently
        * selected.
        *
        * You can populate a @see:ListBox using an array of strings or you can use
        * an array of objects, in which case the @see:displayPath property determines
        * which object property is displayed in the list.
        *
        * To display HTML rather than plain text, set the @see:isContentHtml property
        * to true.
        *
        * The example below creates a @see:ListBox control and populates it using
        * a 'countries' array. The control updates its @see:selectedIndex and
        * @see:selectedItem properties as the user moves the selection.
        *
        * @fiddle:8HnLx
        */
        var ListBox = (function (_super) {
            __extends(ListBox, _super);
            /**
            * Initializes a new instance of a @see:ListBox.
            *
            * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
            * @param options The JavaScript object containing initialization data for the control.
            */
            function ListBox(element, options) {
                _super.call(this, element);
                this._html = false;
                /**
                * Occurs when the value of the @see:selectedIndex property changes.
                */
                this.selectedIndexChanged = new wijmo.Event();
                /**
                * Occurs when the list of items changes.
                */
                this.itemsChanged = new wijmo.Event();
                /**
                * Occurs when the current item is checked or unchecked.
                *
                * This event is raised when the @see:checkedMemberPath is set to the name of a
                * property to add checkboxes to each item in the control.
                *
                * Use the @see:selectedItem property to retrieve the item that was checked or
                * unchecked.
                */
                this.itemChecked = new wijmo.Event();

                // instantiate and apply template
                var tpl = this.getTemplate();
                this.applyTemplate('wj-control wj-listbox wj-content', tpl, {
                    _div: 'lb-content'
                });

                // handle mouse and keyboard
                var e = this.hostElement;
                e.addEventListener('click', this._click.bind(this));
                e.addEventListener('keydown', this._keydown.bind(this));
                e.addEventListener('keypress', this._keypress.bind(this));

                // initializing from <select> tag
                if (this._orgTag == 'SELECT') {
                    this._copyOriginalAttributes(this.hostElement);
                    this._populateSelectElement(this.hostElement);
                }

                // initialize control options
                this.initialize(options);
            }
            //--------------------------------------------------------------------------
            //#region ** ovrerrides
            /**
            * Refreshes the list.
            */
            ListBox.prototype.refresh = function () {
                _super.prototype.refresh.call(this);
                this._populateList();
            };

            Object.defineProperty(ListBox.prototype, "itemsSource", {
                //#endregion
                //--------------------------------------------------------------------------
                //#region ** object model
                /**
                * Gets or sets the array or @see:ICollectionView object that contains the list items.
                */
                get: function () {
                    return this._items;
                },
                set: function (value) {
                    if (this._items != value) {
                        // unbind current collection view
                        if (this._cv) {
                            this._cv.currentChanged.removeHandler(this._cvCurrentChanged, this);
                            this._cv.collectionChanged.removeHandler(this._cvCollectionChanged, this);
                            this._cv = null;
                        }

                        // save new data source and collection view
                        this._items = value;
                        this._cv = wijmo.asCollectionView(value);

                        // bind new collection view
                        if (this._cv != null) {
                            this._cv.currentChanged.addHandler(this._cvCurrentChanged, this);
                            this._cv.collectionChanged.addHandler(this._cvCollectionChanged, this);
                        }

                        // update the list
                        this._populateList();
                        this.onItemsChanged();
                        this.onSelectedIndexChanged();
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ListBox.prototype, "collectionView", {
                /**
                * Gets the @see:ICollectionView object used as the item source.
                */
                get: function () {
                    return this._cv;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ListBox.prototype, "isContentHtml", {
                /**
                * Gets or sets a value indicating whether items contain plain text or HTML.
                */
                get: function () {
                    return this._html;
                },
                set: function (value) {
                    if (value != this._html) {
                        this._html = wijmo.asBoolean(value);
                        this._populateList();
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ListBox.prototype, "itemFormatter", {
                /**
                * Gets or sets a function used to customize the values shown in the list.
                * The function takes two arguments, the item index and the default text or html, and
                * returns the new text or html to display.
                *
                * NOTE: If the formatting function needs a scope (i.e. a meaningful 'this'
                * value), then remember to set the filter using the 'bind' function to
                * specify the 'this' object. For example:
                * <pre>
                *   listBox.itemFormatter = customItemFormatter.bind(this);
                *   function customItemFormatter(index, content) {
                *     if (this.makeItemBold(index)) {
                *       content = '&lt;b&gt;' + content + '&lt;/b&gt;';
                *     }
                *     return content;
                *   }
                * </pre>
                */
                get: function () {
                    return this._itemFormatter;
                },
                set: function (value) {
                    if (value != this._itemFormatter) {
                        this._itemFormatter = wijmo.asFunction(value);
                        this._populateList();
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ListBox.prototype, "displayMemberPath", {
                /**
                * Gets or sets the name of the property to use as the visual representation of the items.
                */
                get: function () {
                    return this._pathDisplay;
                },
                set: function (value) {
                    if (value != this._pathDisplay) {
                        this._pathDisplay = wijmo.asString(value);
                        this._populateList();
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ListBox.prototype, "selectedValuePath", {
                /**
                * Gets or sets the name of the property used to get the @see:selectedValue
                * from the @see:selectedItem.
                */
                get: function () {
                    return this._pathValue;
                },
                set: function (value) {
                    this._pathValue = wijmo.asString(value);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ListBox.prototype, "checkedMemberPath", {
                /**
                * Gets or sets the name of the property used to control checkboxes placed next
                * to each item.
                *
                * Use this property to create multi-select lisboxes.
                * When an item is checked or unchecked, the control raises the @see:itemChecked event.
                * Use the @see:selectedItem property to retrieve the item that was checked or unchecked.
                */
                get: function () {
                    return this._pathChecked;
                },
                set: function (value) {
                    if (value != this._pathChecked) {
                        this._pathChecked = wijmo.asString(value);
                        this._populateList();
                    }
                },
                enumerable: true,
                configurable: true
            });

            /**
            * Gets the string displayed for the item at a given index.
            *
            * The string may be plain text or HTML, depending on the setting
            * of the @see:isContentHtml property.
            *
            * @param index The index of the item.
            */
            ListBox.prototype.getDisplayValue = function (index) {
                // get the text or html
                var item = null;
                if (index > -1 && this._cv && this._cv.items) {
                    item = this._cv.items[index];
                    if (item && this.displayMemberPath) {
                        //assert(this.displayMemberPath in item, 'item does not define displayMemberPath property "' + this.displayMemberPath + '".');
                        item = item[this.displayMemberPath];
                    }
                }
                var text = item != null ? item.toString() : '';

                // allow caller to override/modify the text or html
                if (this.itemFormatter) {
                    text = this.itemFormatter(index, text);
                }

                // return the result
                return text;
            };

            /**
            * Gets the text displayed for the item at a given index (as plain text).
            *
            * @param index The index of the item.
            */
            ListBox.prototype.getDisplayText = function (index) {
                var item;
                if (index > -1 && index < this._div.children.length) {
                    item = this._div.children[index];
                }
                return item != null ? item.textContent : '';
            };

            Object.defineProperty(ListBox.prototype, "selectedIndex", {
                /**
                * Gets or sets the index of the currently selected item.
                */
                get: function () {
                    return this._cv ? this._cv.currentPosition : -1;
                },
                set: function (value) {
                    if (this._cv) {
                        this._cv.moveCurrentToPosition(wijmo.asNumber(value));
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ListBox.prototype, "selectedItem", {
                /**
                * Gets or sets the item that is currently selected.
                */
                get: function () {
                    return this._cv ? this._cv.currentItem : null;
                },
                set: function (value) {
                    if (this._cv) {
                        this._cv.moveCurrentTo(value);
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ListBox.prototype, "selectedValue", {
                /**
                * Gets or sets the value of the @see:selectedItem obtained using the @see:selectedValuePath.
                */
                get: function () {
                    var item = this.selectedItem;
                    if (item && this.selectedValuePath) {
                        item = item[this.selectedValuePath];
                    }
                    return item;
                },
                set: function (value) {
                    var path = this.selectedValuePath, index = -1;
                    if (this._cv) {
                        for (var i = 0; i < this._cv.items.length; i++) {
                            var item = this._cv.items[i];
                            if ((path && item[path] == value) || (!path && item == value)) {
                                index = i;
                                break;
                            }
                        }
                        this.selectedIndex = index;
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ListBox.prototype, "maxHeight", {
                /**
                * Gets or sets the maximum height of the list.
                */
                get: function () {
                    return parseFloat(this._div.style.maxHeight);
                },
                set: function (value) {
                    this._div.style.maxHeight = wijmo.asNumber(value) + 'px';
                },
                enumerable: true,
                configurable: true
            });

            /**
            * Highlights the selected item and scrolls it into view.
            */
            ListBox.prototype.showSelection = function () {
                var index = this.selectedIndex, count = this._div.childElementCount, e;

                for (var i = 0; i < count; i++) {
                    e = this._div.children[i];
                    wijmo.toggleClass(e, 'wj-state-selected', i == index);
                }

                // scroll into view
                if (index > -1 && index < count) {
                    e = this._div.children[index];
                    var rco = e.getBoundingClientRect();
                    var rcc = this._div.getBoundingClientRect();
                    if (rco.bottom > rcc.bottom) {
                        this._div.scrollTop += rco.bottom - rcc.bottom;
                    } else if (rco.top < rcc.top) {
                        this._div.scrollTop -= rcc.top - rco.top;
                    }
                }
            };

            /**
            * Raises the @see:selectedIndexChanged event.
            */
            ListBox.prototype.onSelectedIndexChanged = function (e) {
                this.selectedIndexChanged.raise(this, e);
            };

            /**
            * Raises the @see:itemsChanged event.
            */
            ListBox.prototype.onItemsChanged = function (e) {
                this.itemsChanged.raise(this, e);
            };

            /**
            * Raises the @see:itemCheched event.
            */
            ListBox.prototype.onItemChecked = function (e) {
                this.itemChecked.raise(this, e);
            };

            //#endregion
            //--------------------------------------------------------------------------
            //#region ** implementation
            // handle changes to the data source
            ListBox.prototype._cvCollectionChanged = function (sender, e) {
                this._populateList();
                this.onItemsChanged();
            };
            ListBox.prototype._cvCurrentChanged = function (sender, e) {
                this.showSelection();
                this.onSelectedIndexChanged();
            };

            // populate the list from the current itemsSource
            ListBox.prototype._populateList = function () {
                // remember if we have focus
                var focus = this.containsFocus();

                // populate
                this._div.innerHTML = '';
                if (this._cv) {
                    for (var i = 0; i < this._cv.items.length; i++) {
                        // get item text
                        var text = this.getDisplayValue(i);
                        if (this._html != true) {
                            text = wijmo.escapeHtml(text);
                        }

                        // add checkbox
                        if (this.checkedMemberPath) {
                            var checked = this._cv.items[i][this.checkedMemberPath];
                            text = '<label><input type="checkbox"' + (checked ? ' checked' : '') + '> ' + text + '</label>';
                        }

                        // build item
                        var item = document.createElement('div');
                        item.innerHTML = text;
                        item.className = 'wj-listbox-item';
                        if (wijmo.hasClass(item.firstChild, 'wj-separator')) {
                            item.className += ' wj-separator';
                        }

                        // add item to list
                        this._div.appendChild(item);
                    }
                }

                // restore focus
                if (focus && !this.containsFocus()) {
                    this.focus();
                }

                // scroll selection into view
                this.showSelection();
            };

            // click to select elements
            ListBox.prototype._click = function (e) {
                // get the listbox element (child of div)
                var elem = e.target;
                while (elem && elem.parentNode != this._div) {
                    elem = elem.parentNode;
                }

                for (var index = 0; index < this._div.childElementCount; index++) {
                    if (this._div.childNodes[index] == elem) {
                        this.selectedIndex = index;
                        e.stopPropagation();
                        break;
                    }
                }

                // handle checkboxes
                if (this.checkedMemberPath && this.selectedItem) {
                    var cb = wijmo.tryCast(e.target, HTMLInputElement);
                    if (cb && cb.type == 'checkbox') {
                        var ecv = wijmo.tryCast(this._cv, 'IEditableCollectionView');
                        if (ecv) {
                            ecv.editItem(this.selectedItem);
                            this.selectedItem[this.checkedMemberPath] = cb.checked;
                            ecv.commitEdit();
                        } else {
                            this.selectedItem[this.checkedMemberPath] = cb.checked;
                        }
                        this.onItemChecked();
                    }
                }
            };

            // handle keyboard events
            ListBox.prototype._keydown = function (e) {
                var handled = true, index = this.selectedIndex, count = this._div.childElementCount;
                switch (e.keyCode) {
                    case wijmo.Key.Down:
                        if (index < count - 1) {
                            this.selectedIndex++;
                        }
                        break;
                    case wijmo.Key.Up:
                        if (index > 0) {
                            this.selectedIndex--;
                        }
                        break;
                    case wijmo.Key.Home:
                        this.selectedIndex = 0;
                        break;
                    case wijmo.Key.End:
                        this.selectedIndex = count - 1;
                        break;
                    case wijmo.Key.PageDown:
                        if (this.selectedIndex > -1) {
                            var index = this.selectedIndex, height = this._div.offsetHeight, offset = 0;
                            for (var i = index + 1; i < this._cv.items.length; i++) {
                                var itemHeight = this._div.children[i].scrollHeight;
                                if (offset + itemHeight > height) {
                                    this.selectedIndex = i;
                                    break;
                                }
                                offset += itemHeight;
                            }
                            if (this.selectedIndex == index) {
                                this._cv.moveCurrentToLast();
                            }
                        }
                        break;
                    case wijmo.Key.PageUp:
                        if (this.selectedIndex > -1) {
                            var index = this.selectedIndex, height = this._div.offsetHeight, offset = 0;
                            for (var i = index - 1; i > 0; i--) {
                                var itemHeight = this._div.children[i].scrollHeight;
                                if (offset + itemHeight > height) {
                                    this.selectedIndex = i;
                                    break;
                                }
                                offset += itemHeight;
                            }
                            if (this.selectedIndex == index) {
                                this._cv.moveCurrentToFirst();
                            }
                        }
                        break;
                    default:
                        handled = false;
                        break;
                }
                if (handled) {
                    e.preventDefault();
                }
            };
            ListBox.prototype._keypress = function (e) {
                if (e.charCode > 0) {
                    var count = this._div.childElementCount;
                    for (var off = 1; off < count; off++) {
                        var index = (this.selectedIndex + off) % count, text = this.getDisplayText(index).trim().toLowerCase();
                        if (text.charCodeAt(0) == e.charCode) {
                            this.selectedIndex = index;
                            e.preventDefault();
                            break;
                        }
                    }
                }
            };

            // build collectionView from OPTION elements items in a SELECT element
            ListBox.prototype._populateSelectElement = function (hostElement) {
                var children = hostElement.children, items = [], selIndex = -1;
                for (var i = 0; i < children.length; i++) {
                    var child = children[i];
                    if (child.tagName == 'OPTION') {
                        // keep track of selected item
                        if (child.hasAttribute('selected')) {
                            selIndex = items.length;
                        }

                        // add option to collectionView
                        if (child.innerHTML) {
                            items.push({
                                hdr: child.innerHTML,
                                val: child.getAttribute('value'),
                                cmdParam: child.getAttribute('cmd-param')
                            });
                        } else {
                            items.push({
                                hdr: '<div class="wj-separator" style="width:100%;height:1px;margin:3px 0px;background-color:rgba(0,0,0,.2)"/>'
                            });
                        }

                        // remove child from host
                        hostElement.removeChild(child);
                        i--;
                    }
                }

                // apply items to control
                if (items) {
                    this.displayMemberPath = 'hdr';
                    this.selectedValuePath = 'val';
                    this.itemsSource = items;
                    this.selectedIndex = selIndex;
                }
            };
            ListBox.controlTemplate = '<div ' + 'wj-part="lb-content" ' + 'style="cursor:default;height:100%;overflow:auto;overflow-x:hidden;-webkit-overflow-scrolling:touch;">' + '</div>';
            return ListBox;
        })(wijmo.Control);
        input.ListBox = ListBox;
    })(wijmo.input || (wijmo.input = {}));
    var input = wijmo.input;
})(wijmo || (wijmo = {}));
var wijmo;
(function (wijmo) {
    (function (input) {
        'use strict';

        /**
        * The @see:ComboBox control allows users to pick strings from lists.
        *
        * The control automatically completes entries as the user types, and allows users
        * to show a drop-down list with the items available.
        *
        * Use the @see:selectedIndex or the @see:text properties to determine which
        * item is currently selected.
        *
        * The @see:isEditable property determines whether users can enter values that
        * are not present in the list.
        *
        * The example below creates a @see:ComboBox control and populates it with a list
        * of countries. The @see:ComboBox searches for the country as the user types.
        * The <b>isEditable</b> property is set to false, so the user is forced to
        * select one of the items in the list.
        *
        * The example also shows how to create and populate a @see:ComboBox using
        * an HTML <b>&lt;select;&gt</b> element with <b>&lt;option;&gt</b> child
        * elements.
        *
        * @fiddle:8HnLx
        */
        var ComboBox = (function (_super) {
            __extends(ComboBox, _super);
            /**
            * Initializes a new instance of a @see:ComboBox control.
            *
            * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
            * @param options The JavaScript object containing initialization data for the control.
            */
            function ComboBox(element, options) {
                _super.call(this, element);
                // property storage
                this._required = true;
                this._editable = false;
                // private stuff
                this._composing = false;
                this._deleting = false;
                this._settingText = false;
                /**
                * Occurs when the value of the @see:selectedIndex property changes.
                */
                this.selectedIndexChanged = new wijmo.Event();

                // handle cursor keys
                this._tbx.addEventListener('keydown', this._handleKeyDown.bind(this));

                // handle IME
                var self = this;
                this._tbx.addEventListener('compositionstart', function () {
                    self._composing = true;
                });
                this._tbx.addEventListener('compositionend', function () {
                    self._composing = false;
                    self._setText(self.text, true);
                });

                // initializing from <select> tag
                if (this._orgTag == 'SELECT') {
                    this._copyOriginalAttributes(this.hostElement);
                    this._lbx._populateSelectElement(this.hostElement);
                }

                // initialize control options
                this.initialize(options);
            }
            Object.defineProperty(ComboBox.prototype, "itemsSource", {
                //--------------------------------------------------------------------------
                //#region ** object model
                /**
                * Gets or sets the array or @see:ICollectionView object that contains the items to select from.
                */
                get: function () {
                    return this._lbx.itemsSource;
                },
                set: function (value) {
                    this._lbx.itemsSource = value;
                    this._updateBtn();
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ComboBox.prototype, "collectionView", {
                /**
                * Gets the @see:ICollectionView object used as the item source.
                */
                get: function () {
                    return this._lbx.collectionView;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ComboBox.prototype, "displayMemberPath", {
                /**
                * Gets or sets the name of the property to use as the visual representation of the items.
                */
                get: function () {
                    return this._lbx.displayMemberPath;
                },
                set: function (value) {
                    this._lbx.displayMemberPath = value;
                    var text = this.getDisplayText();
                    if (this.text != text) {
                        this._setText(text, true);
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ComboBox.prototype, "selectedValuePath", {
                /**
                * Gets or sets the name of the property used to get the @see:selectedValue from the @see:selectedItem.
                */
                get: function () {
                    return this._lbx.selectedValuePath;
                },
                set: function (value) {
                    this._lbx.selectedValuePath = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ComboBox.prototype, "isContentHtml", {
                /**
                * Gets or sets a value indicating whether the drop-down list displays items as plain text or as HTML.
                */
                get: function () {
                    return this._lbx.isContentHtml;
                },
                set: function (value) {
                    if (value != this.isContentHtml) {
                        this._lbx.isContentHtml = wijmo.asBoolean(value);
                        var text = this.getDisplayText();
                        if (this.text != text) {
                            this._setText(text, true);
                        }
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ComboBox.prototype, "itemFormatter", {
                /**
                * Gets or sets a function used to customize the values shown in the drop-down list.
                * The function takes two arguments, the item index and the default text or html, and
                * returns the new text or html to display.
                *
                * NOTE: If the formatting function needs a scope (i.e. a meaningful 'this'
                * value), then remember to set the filter using the 'bind' function to
                * specify the 'this' object. For example:
                * <pre>
                *   comboBox.itemFormatter = customItemFormatter.bind(this);
                *   function customItemFormatter(index, content) {
                *     if (this.makeItemBold(index)) {
                *       content = '&lt;b&gt;' + content + '&lt;/b&gt;';
                *     }
                *     return content;
                *   }
                * </pre>
                */
                get: function () {
                    return this._lbx.itemFormatter;
                },
                set: function (value) {
                    this._lbx.itemFormatter = wijmo.asFunction(value);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ComboBox.prototype, "selectedIndex", {
                /**
                * Gets or sets the index of the currently selected item in the drop-down list.
                */
                get: function () {
                    return this._lbx.selectedIndex;
                },
                set: function (value) {
                    if (value != this.selectedIndex) {
                        this._lbx.selectedIndex = value;
                    }
                    var text = this.getDisplayText(value);
                    if (this.text != text) {
                        this._setText(text, true);
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ComboBox.prototype, "selectedItem", {
                /**
                * Gets or sets the item that is currently selected in the drop-down list.
                */
                get: function () {
                    return this._lbx.selectedItem;
                },
                set: function (value) {
                    this._lbx.selectedItem = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ComboBox.prototype, "selectedValue", {
                /**
                * Gets or sets the value of the @see:selectedItem, obtained using the @see:selectedValuePath.
                */
                get: function () {
                    return this._lbx.selectedValue;
                },
                set: function (value) {
                    this._lbx.selectedValue = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ComboBox.prototype, "required", {
                /**
                * Gets or sets whether the control value must be set to a non-null value
                * or whether it can be set to null (by deleting the content of the control).
                */
                get: function () {
                    return this._required;
                },
                set: function (value) {
                    this._required = wijmo.asBoolean(value);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ComboBox.prototype, "isEditable", {
                /**
                * Gets or sets a value that enables or disables editing of the text
                * in the input element of the @see:ComboBox (defaults to false).
                */
                get: function () {
                    return this._editable;
                },
                set: function (value) {
                    this._editable = wijmo.asBoolean(value);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ComboBox.prototype, "maxDropDownHeight", {
                /**
                * Gets or sets the maximum height of the drop-down list.
                */
                get: function () {
                    return this._lbx.maxHeight;
                },
                set: function (value) {
                    this._lbx.maxHeight = wijmo.asNumber(value);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ComboBox.prototype, "maxDropDownWidth", {
                /**
                * Gets or sets the maximum width of the drop-down list.
                */
                get: function () {
                    var lb = this._dropDown.children[0];
                    return parseInt(lb.style.maxWidth);
                },
                set: function (value) {
                    var lb = this._dropDown.children[0];
                    lb.style.maxWidth = wijmo.asNumber(value) + 'px';
                },
                enumerable: true,
                configurable: true
            });

            /**
            * Gets the string displayed for the item at a given index (as plain text).
            *
            * @param index The index of the item to retrieve the text for.
            */
            ComboBox.prototype.getDisplayText = function (index) {
                if (typeof index === "undefined") { index = this.selectedIndex; }
                return this._lbx.getDisplayText(index);
            };

            /**
            * Raises the @see:selectedIndexChanged event.
            */
            ComboBox.prototype.onSelectedIndexChanged = function (e) {
                this._updateBtn();
                this.selectedIndexChanged.raise(this, e);
            };

            /**
            * Gets the index of the first item that matches a given string.
            *
            * @param text The text to search for.
            * @param fullMatch A value indicating whether to look for a full match or just the start of the string.
            * @return The index of the item, or -1 if not found.
            */
            ComboBox.prototype.indexOf = function (text, fullMatch) {
                var cv = this.collectionView;
                if (cv && cv.items && text) {
                    text = text.toLowerCase();
                    for (var i = 0; i < cv.items.length; i++) {
                        var t = this.getDisplayText(i).toLowerCase();
                        if (fullMatch) {
                            if (t == text) {
                                return i;
                            }
                        } else {
                            if (t.indexOf(text) == 0) {
                                return i;
                            }
                        }
                    }
                }
                return -1;
            };

            //#endregion ** object model
            //--------------------------------------------------------------------------
            //#region ** overrides
            // show current selection when dropping down
            ComboBox.prototype.onIsDroppedDownChanged = function (e) {
                _super.prototype.onIsDroppedDownChanged.call(this, e);
                if (this.isDroppedDown) {
                    this._lbx.showSelection();
                }
            };

            // update button visibility and value list
            ComboBox.prototype._updateBtn = function () {
                var cv = this.collectionView;
                this._btn.style.display = this._showBtn && cv && cv.items.length ? '' : 'none';
            };

            // create the drop-down element
            ComboBox.prototype._createDropDown = function () {
                var self = this;

                // create the drop-down element
                this._lbx = new input.ListBox(this._dropDown);

                // limit the size of the drop-down
                this._lbx.maxHeight = 200;

                // update our selection when user picks an item from the ListBox
                // or when the selected index changes because the list changed
                this._lbx.selectedIndexChanged.addHandler(function () {
                    self._updateBtn();
                    self.selectedIndex = self._lbx.selectedIndex;
                    self.onSelectedIndexChanged();
                });

                // update button display when item list changes
                this._lbx.itemsChanged.addHandler(function () {
                    self.isDroppedDown = false;
                });

                // close the drop-down when the user clicks to select an item
                this._dropDown.addEventListener('click', function () {
                    self.isDroppedDown = false;
                });
            };

            //#endregion ** overrides
            //--------------------------------------------------------------------------
            //#region ** implementation
            // update text in textbox
            ComboBox.prototype._setText = function (text, fullMatch) {
                // not while composing IME text...
                if (this._composing)
                    return;

                // prevent reentrant calls while moving CollectionView cursor
                if (this._settingText)
                    return;
                this._settingText = true;

                // get variables we need
                var index = this.selectedIndex, cv = this.collectionView, start = this._getSelStart(), len = -1;

                // make sure we don't have nulls
                if (text == null)
                    text = '';

                // try autocompletion
                if (!this.isEditable || !this._deleting) {
                    index = this.indexOf(text, fullMatch);
                    if (index < 0 && fullMatch) {
                        index = this.indexOf(text, false);
                    }
                    if (index < 0 && start > 0) {
                        index = this.indexOf(text.substr(0, start), false);
                    }
                    if (index < 0 && !this.isEditable && this.required && cv && cv.items) {
                        index = Math.max(0, this.indexOf(this._oldText, false));
                    }
                    if (index > -1) {
                        len = start;
                        text = this.getDisplayText(index);
                    }
                }

                // update collectionView
                if (cv) {
                    cv.moveCurrentToPosition(index);
                }

                // update element
                if (text != this._tbx.value) {
                    this._tbx.value = text;
                }

                // update text selection
                if (len > -1 && this.containsFocus() && !this.isTouching) {
                    this._setSelectionRange(len, text.length);
                }

                // call base class to fire textChanged event
                _super.prototype._setText.call(this, text, fullMatch);

                // clear flags
                this._deleting = false;
                this._settingText = false;
            };

            // skip to the next/previous item that starts with a given string, wrapping
            ComboBox.prototype._findNext = function (text, step) {
                if (this.collectionView) {
                    text = text.toLowerCase();
                    var len = this.collectionView.items.length, index, t;
                    for (var i = 1; i < len; i++) {
                        index = (this.selectedIndex + i * step + len) % len;
                        t = this.getDisplayText(index).toLowerCase();
                        if (t.indexOf(text) == 0) {
                            return index;
                        }
                    }
                }
                return this.selectedIndex;
            };

            // select items with the keyboard
            ComboBox.prototype._handleKeyDown = function (e) {
                // remember we pressed a key when handling the TextChanged event
                if (e.keyCode == wijmo.Key.Back || e.keyCode == wijmo.Key.Delete) {
                    this._deleting = true;
                }

                // not if we have no items
                var cv = this.collectionView;
                if (!cv || !cv.items) {
                    return;
                }

                // handle key
                var start = -1;
                switch (e.keyCode) {
                    case wijmo.Key.Up:
                        start = this._getSelStart();
                        this.selectedIndex = this._findNext(this.text.substr(0, start), -1);
                        this._setSelectionRange(start, this.text.length);
                        e.preventDefault();
                        break;

                    case wijmo.Key.Down:
                        if (!this.isDroppedDown && (e.ctrlKey || e.shiftKey)) {
                            this.isDroppedDown = true;
                        } else {
                            start = this._getSelStart();
                            this.selectedIndex = this._findNext(this.text.substr(0, start), +1);
                            this._setSelectionRange(start, this.text.length);
                        }
                        e.preventDefault();
                        break;

                    case wijmo.Key.Escape:
                    case wijmo.Key.Enter:
                        if (this.isDroppedDown) {
                            this.isDroppedDown = false;
                            e.preventDefault();
                        }
                        if (e.keyCode == wijmo.Key.Enter) {
                            this._setSelectionRange(0, this.text.length);
                            e.preventDefault();
                        }
                        break;

                    case wijmo.Key.Tab:
                        this.isDroppedDown = false;
                        break;

                    case wijmo.Key.F4:
                        this.isDroppedDown = !this.isDroppedDown;
                        e.preventDefault();
                        break;
                }
            };

            // set selection range in input element (if it is visible)
            ComboBox.prototype._setSelectionRange = function (start, end) {
                if (this._elRef == this._tbx) {
                    this._tbx.setSelectionRange(start, end);
                }
            };

            // get selection start in an extra-safe way (TFS 82372)
            ComboBox.prototype._getSelStart = function () {
                return this._tbx && this._tbx.value ? this._tbx.selectionStart : 0;
            };
            return ComboBox;
        })(input.DropDown);
        input.ComboBox = ComboBox;
    })(wijmo.input || (wijmo.input = {}));
    var input = wijmo.input;
})(wijmo || (wijmo = {}));

var wijmo;
(function (wijmo) {
    /**
    * Defines input controls for strings, numbers, dates, times, and colors.
    */
    (function (input) {
        'use strict';

        /**
        * The @see:AutoComplete control is an input control that allows callers
        * to customize the item list as the user types.
        *
        * The control is similar to the @see:ComboBox, except the item source is a
        * function (@see:itemsSourceFunction) rather than a static list. For example,
        * you can look up items on remote databases as the user types.
        *
        * The example below creates an @see:AutoComplete control and populates it using
        * a 'countries' array. The @see:AutoComplete searches for the country as the user
        * types, and narrows down the list of countries that match the current input.
        *
        * @fiddle:8HnLx
        */
        var AutoComplete = (function (_super) {
            __extends(AutoComplete, _super);
            //private _itemFormatter: Function;
            /**
            * Initializes a new instance of an @see:AutoComplete control.
            *
            * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
            * @param options The JavaScript object containing initialization data for the control.
            */
            function AutoComplete(element, options) {
                _super.call(this, element);
                this._minLength = 2;
                this._maxItems = 6;
                this._itemCount = 0;
                this._delay = 500;
                this._query = '';
                this._handlingCallback = false;
                this.isEditable = true;
                this.isContentHtml = true;
                this.itemFormatter = this._defaultFormatter.bind(this);
                if (options) {
                    this.initialize(options);
                }
            }
            Object.defineProperty(AutoComplete.prototype, "minLength", {
                //--------------------------------------------------------------------------
                //#region ** object model
                /**
                * Gets or sets the minimum input length to trigger autocomplete suggestions.
                */
                get: function () {
                    return this._minLength;
                },
                set: function (value) {
                    this._minLength = wijmo.asNumber(value, false, true);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(AutoComplete.prototype, "maxItems", {
                /**
                * Gets or sets the maximum number of items to display in the drop-down list.
                */
                get: function () {
                    return this._maxItems;
                },
                set: function (value) {
                    this._maxItems = wijmo.asNumber(value, false, true);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(AutoComplete.prototype, "delay", {
                /**
                * Gets or sets the delay, in milliseconds, between when a keystroke occurs
                * and when the search is performed.
                */
                get: function () {
                    return this._delay;
                },
                set: function (value) {
                    this._delay = wijmo.asNumber(value, false, true);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(AutoComplete.prototype, "itemsSourceFunction", {
                /**
                * Gets or sets a function that provides list items dynamically as the user types.
                *
                * The function takes three parameters:
                * <ul>
                *     <li>the query string typed by the user</li>
                *     <li>the maximum number of items to return</li>
                *     <li>the callback function to call when the results become available</li>
                * </ul>
                *
                * For example:
                * <pre>
                * autoComplete.itemsSourceFunction = function (query, max, callback) {
                *   // get results from the server
                *   var params = { query: query, max: max };
                *   $.getJSON('companycatalog.ashx', params, function (response) {
                *     // return results to the control
                *     callback(response);
                *   });
                * };
                * </pre>
                */
                get: function () {
                    return this._itemsSourceFn;
                },
                set: function (value) {
                    this._itemsSourceFn = wijmo.asFunction(value);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(AutoComplete.prototype, "cssMatch", {
                /**
                * Gets or sets the name of the css class used to highlight any parts
                * of the content that match the search terms.
                *
                * By default, this property is set to null, which causes the matching
                * terms to be shown in bold. You can set it to the name of a css class
                * to change the way the matches are displayed.
                *
                * The example below shows how you could use a specific css class called
                * 'match' to highlight the matches:
                *
                * <pre>
                * &lt;!-- css style for highlighting matches --&gt;
                * .match {
                *   background-color: yellow;
                *   color:black;
                * }
                * // assign css style to cssMatch property
                * autoComplete.cssMatch = 'match';
                * </pre>
                */
                get: function () {
                    return this._cssMatch;
                },
                set: function (value) {
                    this._cssMatch = wijmo.asString(value);
                },
                enumerable: true,
                configurable: true
            });

            //#endregion ** object model
            //--------------------------------------------------------------------------
            //#region ** overrides
            // update text in textbox
            AutoComplete.prototype._setText = function (text) {
                // don't call base class (to avoid autocomplete)
                // don't do this while handling the itemsSourcefunction callback
                if (this._handlingCallback) {
                    return;
                }

                // resetting...
                if (!text && this.selectedIndex > -1) {
                    this.selectedIndex = -1;
                }

                // raise textChanged
                if (text != this._oldText) {
                    // assign only if necessary to prevent occasionally swapping chars (Android 4.4.2)
                    if (this._tbx.value != text) {
                        this._tbx.value = text;
                    }
                    this._oldText = text;
                    this.onTextChanged();
                }

                // no text? no filter...
                if (!text && this.collectionView) {
                    this.collectionView.filter = null;
                }

                // update list when user types in some text
                var self = this;
                if (self._toSearch) {
                    clearTimeout(self._toSearch);
                }
                if (text != this.getDisplayText()) {
                    // get new search terms on a timeOut (so the control doesn't update too often)
                    self._toSearch = setTimeout(function () {
                        self._toSearch = null;

                        // get search terms
                        var terms = self.text.trim().toLowerCase();
                        if (terms.length < self._minLength) {
                            self.isDroppedDown = false; // too short? close drop-down
                        } else if (terms != self._query) {
                            // save new search terms
                            self._query = terms;

                            // in case the query contains regex characters
                            // http://stackoverflow.com/questions/2593637/how-to-escape-regular-expression-in-javascript
                            terms = terms.replace(/([.?*+^$[\]\\(){}|-])/g, "\\$1");

                            // build regular expressions for searching and highlighting the items
                            // http://stackoverflow.com/questions/13911053/regular-expression-to-match-all-words-in-a-query-in-any-order
                            self._rxMatch = new RegExp('(?=.*' + terms.replace(/ /g, ')(?=.*') + ')', 'ig');
                            self._rxHighlight = new RegExp('(' + terms.replace(/ /g, '|') + ')', 'ig');

                            // update list
                            if (self.itemsSourceFunction) {
                                self.isDroppedDown = false;
                                self.itemsSourceFunction(terms, self.maxItems, self._itemSourceFunctionCallback.bind(self));
                            } else {
                                self._updateItems();
                            }
                        }
                    }, this._delay);
                }
            };

            // populate list with results from itemSourceFunction
            AutoComplete.prototype._itemSourceFunctionCallback = function (result) {
                this._handlingCallback = true;
                var cv = wijmo.asCollectionView(result);
                if (cv) {
                    cv.moveCurrentToPosition(-1);
                }
                this.itemsSource = cv;
                this.isDroppedDown = true;
                this._handlingCallback = false;
            };

            // select items with the keyboard
            AutoComplete.prototype._handleKeyDown = function (e) {
                // not if we have no items
                var cv = this.collectionView;
                if (!cv || !cv.items) {
                    return;
                }

                // handle key
                var idx = this.selectedIndex, count = cv.items.length;
                switch (e.keyCode) {
                    case wijmo.Key.Up:
                        this.selectedIndex = Math.max(idx - 1, 0);
                        e.preventDefault();
                        break;

                    case wijmo.Key.Down:
                        if (!this.isDroppedDown && (e.ctrlKey || e.shiftKey)) {
                            this.isDroppedDown = true;
                        } else {
                            this.selectedIndex = Math.min(idx + 1, count - 1);
                        }
                        e.preventDefault();
                        break;

                    case wijmo.Key.Escape:
                    case wijmo.Key.Enter:
                        if (this.isDroppedDown) {
                            this.isDroppedDown = false;
                            e.preventDefault();
                        }
                        break;

                    case wijmo.Key.Tab:
                        this.isDroppedDown = false;
                        break;
                }
            };

            // closing the drop-down? commit the change
            AutoComplete.prototype.onIsDroppedDownChanged = function (e) {
                _super.prototype.onIsDroppedDownChanged.call(this, e);
                this._query = '';
                if (!this.isDroppedDown && this.selectedIndex > -1) {
                    this._setText(this.getDisplayText());
                    if (!this.isTouching) {
                        this.selectAll();
                    }
                }
            };

            //#endregion ** overrides
            //--------------------------------------------------------------------------
            //#region ** implementation
            // apply the filter to show only the matches
            AutoComplete.prototype._updateItems = function () {
                var cv = this.collectionView;
                if (cv) {
                    // apply the filter
                    this._handlingCallback = true;
                    cv.beginUpdate();
                    this._itemCount = 0;
                    cv.filter = this._filter.bind(this);
                    cv.moveCurrentToPosition(-1);
                    cv.endUpdate();
                    this._handlingCallback = false;

                    // show/hide the drop-down
                    if (cv.items.length > 0) {
                        if (this.containsFocus()) {
                            this.isDroppedDown = true;
                        }
                    } else {
                        this.selectedIndex = -1;
                    }
                }
            };

            // filter the items and show only the matches
            AutoComplete.prototype._filter = function (item) {
                // honor maxItems
                if (this._itemCount >= this._maxItems) {
                    return false;
                }

                // apply filter to item
                if (this.displayMemberPath) {
                    item = item[this.displayMemberPath];
                }
                var text = item != null ? item.toString() : '';

                // count matches
                if (this._rxMatch.test(text)) {
                    this._itemCount++;
                    return true;
                }

                // no pass
                return false;
            };

            // default item formatter: show matches in bold
            AutoComplete.prototype._defaultFormatter = function (index, text) {
                var r = '<b>$1</b>';
                if (this._cssMatch) {
                    r = '<span class=' + this._cssMatch + '>$1</span>';
                }
                return text.replace(this._rxHighlight, r);
            };
            return AutoComplete;
        })(input.ComboBox);
        input.AutoComplete = AutoComplete;
    })(wijmo.input || (wijmo.input = {}));
    var input = wijmo.input;
})(wijmo || (wijmo = {}));
var wijmo;
(function (wijmo) {
    (function (input) {
        'use strict';

        /**
        * The @see:Menu control shows a text element with a drop-down list of commands that the user
        * can invoke by click or touch.
        *
        * The @see:Menu control inherits from @see:ComboBox, so you populate and style it
        * in the same way that you do the @see:ComboBox (see the @see:itemsSource property).
        *
        * The @see:Menu control adds an @see:itemClicked event that fires when the user selects
        * an item from the menu. The event handler can inspect the @see:Menu control to determine
        * which item was clicked. For example:
        *
        * <pre>
        * var menu = new wijmo.input.Menu(hostElement);
        * menu.header = 'Main Menu';
        * menu.itemsSource = ['option 1', 'option 2', 'option 3'];
        * menu.itemClicked.addHandler(function(sender, args) {
        * var menu = sender;
        *   alert('Thanks for selecting item ' + menu.selectedIndex + ' from menu ' + menu.header + '!');
        * });
        * </pre>
        *
        * The example below illustrates how you can create value pickers, command-based menus, and
        * menus that respond to the <b>itemClicked</b> event. The menus in this example are based
        * on HTML <b>&lt;select;&gt</b> and <b>&lt;option;&gt</b> elements.
        *
        * @fiddle:BX853
        */
        var Menu = (function (_super) {
            __extends(Menu, _super);
            /**
            * Initializes a new instance of a @see:Menu control.
            *
            * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
            * @param options The JavaScript object containing initialization data for the control.
            */
            function Menu(element, options) {
                _super.call(this, element);
                /**
                * Occurs when the user picksk an item from the menu.
                *
                * The handler can determine which item was picked by reading the event sender's
                * @see:selectedIndex property.
                */
                this.itemClicked = new wijmo.Event();

                // replace textbox with header div
                this._tbx.style.display = 'none';
                var tpl = '<div wj-part="header" class="wj-form-control" style="cursor:default"/>';
                this._header = wijmo.createElement(tpl);
                this._tbx.parentElement.insertBefore(this._header, this._tbx);
                this._elRef = this._header;

                // this is not required
                this.required = false;

                // initializing from <select> tag
                if (this._orgTag == 'SELECT') {
                    this.header = this.hostElement.getAttribute('header');
                    if (this._lbx.itemsSource) {
                        this.commandParameterPath = 'cmdParam';
                    }
                }

                // toggle drop-down when clicking on the header
                // or fire the click event if this menu is a split-button
                var self = this;
                this._header.addEventListener('click', function () {
                    if (self._isButton) {
                        self.isDroppedDown = false;
                        self._raiseCommand(wijmo.EventArgs.empty);
                    } else {
                        self.isDroppedDown = !self.isDroppedDown;
                    }
                });

                // change some defaults
                this.isContentHtml = true;
                this.maxDropDownHeight = 500;

                // initialize control options
                this.initialize(options);
            }
            Object.defineProperty(Menu.prototype, "header", {
                /**
                * Gets or sets the HTML text shown in the @see:Menu element.
                */
                get: function () {
                    return this._header.innerHTML;
                },
                set: function (value) {
                    this._header.innerHTML = wijmo.asString(value);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Menu.prototype, "command", {
                /**
                * Gets or sets the command to execute when an item is clicked.
                *
                * Commands are objects that implement two methods:
                * <ul>
                *  <li><b>executeCommand(parameter)</b> This method executes the command.</li>
                *  <li><b>canExecuteCommand(parameter)</b> This method returns a Boolean value
                *      that determines whether the controller can execute the command.
                *      If this method returns false, the menu option is disabled.</li>
                * </ul>
                *
                * You can also set commands on individual items using the @see:commandPath
                * property.
                */
                get: function () {
                    return this._command;
                },
                set: function (value) {
                    this._command = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Menu.prototype, "commandPath", {
                /**
                * Gets or sets the name of the property that contains the command to
                * execute when the user clicks an item.
                *
                * Commands are objects that implement two methods:
                * <ul>
                *  <li><b>executeCommand(parameter)</b> This method executes the command.</li>
                *  <li><b>canExecuteCommand(parameter)</b> This method returns a Boolean value
                *      that determines whether the controller can execute the command.
                *      If this method returns false, the menu option is disabled.</li>
                * </ul>
                */
                get: function () {
                    return this._cmdPath;
                },
                set: function (value) {
                    this._cmdPath = wijmo.asString(value);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Menu.prototype, "commandParameterPath", {
                /**
                * Gets or sets the name of the property that contains a parameter to use with
                * the command specified by the @see:commandPath property.
                */
                get: function () {
                    return this._cmdParamPath;
                },
                set: function (value) {
                    this._cmdParamPath = wijmo.asString(value);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(Menu.prototype, "isButton", {
                /**
                * Gets or sets a value that determines whether this @see:Menu should act
                * as a split button instead of a regular menu.
                *
                * The difference between regular menus and split buttons is what happens
                * when the user clicks the menu header.
                * In regular menus, clicking the header shows or hides the menu options.
                * In split buttons, clicking the header raises the @see:menuItemClicked event
                * and/or invokes the command associated with the last option selected by
                * the user as if the user had picked the item from the drop-down list.
                *
                * If you want to differentiate between clicks on menu items and the button
                * part of a split button, check the value of the @see:isDroppedDown property
                * of the event sender. If that is true, then a menu item was clicked; if it
                * is false, then the button was clicked.
                *
                * For example, the code below implements a split button that uses the drop-down
                * list only to change the default item/command, and triggers actions only when
                * the button is clicked:
                *
                * <pre>&lt;-- view --&gt;
                * &lt;wj-menu is-button="true" header="Run" value="browser"
                *   item-clicked="menuItemClicked(s, e)"&gt;
                *   &lt;wj-menu-item value="'Internet Explorer'"&gt;Internet Explorer&lt;/wj-menu-item&gt;
                *   &lt;wj-menu-item value="'Chrome'"&gt;Chrome&lt;/wj-menu-item&gt;
                *   &lt;wj-menu-item value="'FireFox'"&gt;FireFox&lt;/wj-menu-item&gt;
                *   &lt;wj-menu-item value="'Safari'"&gt;Safari&lt;/wj-menu-item&gt;
                *   &lt;wj-menu-item value="'Opera'"&gt;Opera&lt;/wj-menu-item&gt;
                * &lt;/wj-menu&gt;
                *
                * // controller
                * $scope.browser = 'Internet Explorer';
                * $scope.menuItemClicked = function (s, e) {
                *   // if not dropped down, click was on the button
                *   if (!s.isDroppedDown) {
                *     alert('running ' + $scope.browser);
                *   }
                *}</pre>
                */
                get: function () {
                    return this._isButton;
                },
                set: function (value) {
                    this._isButton = wijmo.asBoolean(value);
                },
                enumerable: true,
                configurable: true
            });

            /**
            * Raises the @see:itemClicked event.
            */
            Menu.prototype.onItemClicked = function (e) {
                this.itemClicked.raise(this, e);
            };

            // override onTextChanged to raise itemClicked and/or to invoke commands
            // if the change was made by the user
            Menu.prototype.onTextChanged = function (e) {
                _super.prototype.onTextChanged.call(this, e);
                if (!this._closing && this.isDroppedDown) {
                    this._raiseCommand(e);
                }
            };

            // override onIsDroppedDownChanged to clear the selection when showing the menu
            Menu.prototype.onIsDroppedDownChanged = function (e) {
                _super.prototype.onIsDroppedDownChanged.call(this, e);
                if (this.isDroppedDown) {
                    // suspend events
                    this._closing = true;

                    // reset menu
                    this.required = false;
                    this.selectedIndex = -1;

                    // enable/disable items
                    if (this.collectionView && (this.command || this.commandPath)) {
                        var items = this.collectionView.items;
                        for (var i = 0; i < items.length; i++) {
                            var cmd = this._getCommand(items[i]), parm = this.commandParameterPath ? items[i][this.commandParameterPath] : null;
                            if (cmd) {
                                var element = this._lbx._div.childNodes[i];
                                element.style.opacity = this._canExecuteCommand(cmd, parm) ? '' : '.5';
                            }
                        }
                    }

                    // restore events
                    this._closing = false;
                }
            };

            // ** implementation
            // raise itemClicked and/or invoke the current command
            Menu.prototype._raiseCommand = function (e) {
                // execute command if available
                var item = this.selectedItem, cmd = this._getCommand(item);
                if (cmd) {
                    var parm = this._cmdParamPath ? item[this._cmdParamPath] : null;
                    if (!this._canExecuteCommand(cmd, parm)) {
                        return;
                    }
                    this._executeCommand(cmd, parm);
                }

                // raise itemClicked
                this.onItemClicked(e);
            };

            // gets the command to be executed when an item is clicked
            Menu.prototype._getCommand = function (item) {
                var cmd = item && this.commandPath ? item[this.commandPath] : null;
                return cmd ? cmd : this.command;
            };

            // execute a command
            // cmd may be an object that implements the ICommand interface or it may be just a function
            // parm is an optional parameter passed to the command.
            Menu.prototype._executeCommand = function (cmd, parm) {
                if (cmd && !wijmo.isFunction(cmd)) {
                    cmd = cmd['executeCommand'];
                }
                if (wijmo.isFunction(cmd)) {
                    cmd(parm);
                }
            };

            // checks whether a command can be executed
            Menu.prototype._canExecuteCommand = function (cmd, parm) {
                if (cmd) {
                    var x = cmd['canExecuteCommand'];
                    if (wijmo.isFunction(x)) {
                        return x(parm);
                    }
                }
                return true;
            };
            return Menu;
        })(input.ComboBox);
        input.Menu = Menu;
    })(wijmo.input || (wijmo.input = {}));
    var input = wijmo.input;
})(wijmo || (wijmo = {}));
var wijmo;
(function (wijmo) {
    (function (input) {
        'use strict';

        /**
        * The @see:InputDate control allows users to type in dates using any format
        * supported by the @see:Globalize class, or to pick dates from a drop-down box
        * that shows a @see:Calendar control.
        *
        * Use the @see:min and @see:max properties to restrict the range of
        * values that the user can enter.
        *
        * Use the @see:value property to gets or set the currently selected date.
        *
        * The example below shows a <b>Date</b> value (that includes date and time information)
        * using an @see:InputDate and an an @see:InputTime control. Notice how both controls
        * are bound to the same controller variable, and each edits the appropriate information
        * (either date or time). The example also shows a @see:Calendar control that you can
        * use to select the date with a single click.
        *
        * @fiddle:vgc3Y
        */
        var InputDate = (function (_super) {
            __extends(InputDate, _super);
            // private stuff
            /**
            * Initializes a new instance of a @see:InputDate control.
            *
            * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
            * @param options The JavaScript object containing initialization data for the control.
            */
            function InputDate(element, options) {
                _super.call(this, element);
                // property storage
                this._value = new Date();
                this._format = 'd';
                this._required = true;
                /**
                * Occurs after a new date is selected.
                */
                this.valueChanged = new wijmo.Event();

                // initialize mask provider
                this._maskProvider = new wijmo._MaskProvider(this._tbx);

                // track changes to text
                this.hostElement.addEventListener('keydown', this._handleKeyDown.bind(this));
                this._tbx.addEventListener('blur', this._commitText.bind(this));

                // initializing from <input> tag
                if (this._orgTag == 'INPUT') {
                    var value = this._tbx.getAttribute('value');
                    if (value) {
                        this.value = wijmo.Globalize.parseDate(value, 'yyyy-MM-dd');
                    }
                }

                // initialize control options
                this.initialize(options);
            }
            Object.defineProperty(InputDate.prototype, "value", {
                //--------------------------------------------------------------------------
                //#region ** object model
                /**
                * Gets or sets the current date.
                */
                get: function () {
                    return this._value;
                },
                set: function (value) {
                    if (wijmo.DateTime.equals(this._value, value)) {
                        this._tbx.value = wijmo.Globalize.format(value, this.format);
                    } else {
                        // check type
                        value = wijmo.asDate(value, !this.required);

                        // honor ranges (but keep the time)
                        if (value) {
                            if (this.min) {
                                var min = wijmo.DateTime.fromDateTime(this.min, value);
                                if (value < min) {
                                    value = min;
                                }
                            }
                            if (this.max) {
                                var max = wijmo.DateTime.fromDateTime(this.max, value);
                                if (value > max) {
                                    value = max;
                                }
                            }
                        }

                        // update control
                        this._value = value;
                        this._tbx.value = value ? wijmo.Globalize.format(value, this.format) : '';
                        this.onValueChanged();
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputDate.prototype, "text", {
                /**
                * Gets or sets the text shown on the control.
                */
                get: function () {
                    return this._tbx.value;
                },
                set: function (value) {
                    if (value != this.text) {
                        this._setText(value, true);
                        this._commitText();
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputDate.prototype, "required", {
                /**
                * Gets or sets a value indicating whether the control value must be a Date or whether it
                * can be set to null (by deleting the content of the control).
                */
                get: function () {
                    return this._required;
                },
                set: function (value) {
                    this._required = wijmo.asBoolean(value);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputDate.prototype, "min", {
                /**
                * Gets or sets the earliest date that the user can enter.
                */
                get: function () {
                    return this._min;
                },
                set: function (value) {
                    this._min = wijmo.asDate(value, true);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputDate.prototype, "max", {
                /**
                * Gets or sets the latest date that the user can enter.
                */
                get: function () {
                    return this._max;
                },
                set: function (value) {
                    this._max = wijmo.asDate(value, true);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputDate.prototype, "format", {
                /**
                * Gets or sets the format used to display the selected date.
                *
                * The format string is expressed as a .NET-style
                * <a href="http://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx" target="_blank">
                * Date format string</a>.
                */
                get: function () {
                    return this._format;
                },
                set: function (value) {
                    if (value != this.format) {
                        this._format = wijmo.asString(value);
                        this._tbx.value = wijmo.Globalize.format(this.value, this.format);
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputDate.prototype, "mask", {
                /**
                * Gets or sets a mask to use while editing.
                *
                * The mask format is the same one that the @see:wijmo.input.InputMask
                * control uses.
                *
                * If specified, the mask must be compatible with the value of
                * the @see:format property. For example, the mask '99/99/9999' can
                * be used for entering dates formatted as 'MM/dd/yyyy'.
                */
                get: function () {
                    return this._maskProvider.mask;
                },
                set: function (value) {
                    this._maskProvider.mask = wijmo.asString(value);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputDate.prototype, "calendar", {
                /**
                * Gets a reference to the @see:Calendar control shown in the drop-down box.
                */
                get: function () {
                    return this._calendar;
                },
                enumerable: true,
                configurable: true
            });

            /**
            * Raises the @see:valueChanged event.
            */
            InputDate.prototype.onValueChanged = function (e) {
                this.valueChanged.raise(this, e);
            };

            //#endregion ** object model
            //--------------------------------------------------------------------------
            //#region ** overrides
            // update value display in case culture changed
            InputDate.prototype.refresh = function () {
                this.isDroppedDown = false;
                if (this._maskProvider) {
                    this._maskProvider.refresh();
                }
                if (this._calendar) {
                    this._calendar.refresh();
                }
                this._tbx.value = wijmo.Globalize.format(this.value, this.format);
            };

            // override to send focus to calendar when dropping down
            InputDate.prototype.onIsDroppedDownChanged = function (e) {
                // transfer focus to Calendar
                if (this.isDroppedDown) {
                    this._calendar.focus();
                }

                // raise event as usual
                _super.prototype.onIsDroppedDownChanged.call(this, e);
            };

            // create the drop-down element
            InputDate.prototype._createDropDown = function () {
                var self = this;

                // create the drop-down element
                this._calendar = new input.Calendar(this._dropDown);
                this._dropDown.tabIndex = -1;

                // update our value to match calendar's
                this._calendar.valueChanged.addHandler(function () {
                    self.value = wijmo.DateTime.fromDateTime(self._calendar.value, self.value);
                });

                // close the drop-down when the user changes the date with the mouse
                var dtDown = self.value;
                this._dropDown.addEventListener('mousedown', function () {
                    dtDown = self.value;
                });

                // The 'click' event may not be triggered on iOS Safari if focus change happend during previous user's tap.
                // We use 'mouseup' instead.
                //this._dropDown.addEventListener('click', function () {
                this._dropDown.addEventListener('mouseup', function () {
                    var value = self._calendar.value;
                    if (value && !wijmo.DateTime.sameDate(dtDown, value)) {
                        self.isDroppedDown = false;
                    }
                });
            };

            // update drop down content and position before showing it
            InputDate.prototype._updateDropDown = function () {
                // update selected date, range
                this._calendar.value = this.value;
                this._calendar.min = this.min;
                this._calendar.max = this.max;

                // update size
                var cs = getComputedStyle(this.hostElement);
                this._dropDown.style.minWidth = parseFloat(cs.fontSize) * 18 + 'px';
                this._calendar.refresh(); // update layout/size now

                // let base class update position
                _super.prototype._updateDropDown.call(this);
            };

            //#endregion ** overrides
            //--------------------------------------------------------------------------
            //#region ** implementation
            // handle keyboard
            InputDate.prototype._handleKeyDown = function (e) {
                switch (e.keyCode) {
                    case wijmo.Key.F4:
                        this.isDroppedDown = !this.isDroppedDown;
                        e.preventDefault();
                        break;
                    case wijmo.Key.Down:
                        if (!this.isDroppedDown && (e.ctrlKey || e.shiftKey)) {
                            this.isDroppedDown = true;
                            e.preventDefault();
                        }
                        break;
                    case wijmo.Key.Up:
                        if (this.isDroppedDown && (e.ctrlKey || e.shiftKey)) {
                            this.isDroppedDown = false;
                            e.preventDefault();
                        }
                        break;
                    case wijmo.Key.Enter:
                    case wijmo.Key.Escape:
                        if (this.isDroppedDown) {
                            this.isDroppedDown = false;
                            e.preventDefault();
                        }
                        if (e.keyCode == wijmo.Key.Enter) {
                            this._commitText();
                            e.preventDefault();
                        }
                        break;
                }
            };

            // parse date, commit if successful or revert
            InputDate.prototype._commitText = function () {
                var txt = this._tbx.value;
                if (!txt && !this.required) {
                    this.value = null;
                } else {
                    var dt = wijmo.Globalize.parseDate(txt, this.format);
                    if (dt) {
                        this.value = wijmo.DateTime.fromDateTime(dt, this.value);
                    } else {
                        this._tbx.value = wijmo.Globalize.format(this.value, this.format);
                    }
                }
            };

            // merge date and time information from two Date objects into a new Date object
            InputDate.prototype._setDate = function (value, time) {
                return new Date(value.getFullYear(), value.getMonth(), value.getDate(), time.getHours(), time.getMinutes(), time.getSeconds());
            };
            return InputDate;
        })(input.DropDown);
        input.InputDate = InputDate;
    })(wijmo.input || (wijmo.input = {}));
    var input = wijmo.input;
})(wijmo || (wijmo = {}));
var wijmo;
(function (wijmo) {
    (function (input) {
        'use strict';

        /**
        * The @see:InputTime control allows users to enter times using any format
        * supported by the @see:Globalize class, or to pick times from a drop-down
        * list.
        *
        * The @see:min, @see:max, and @see:step properties determine the values shown
        * in the list.
        *
        * The @see:value property gets or sets a Date object that represents the time
        * selected by the user.
        *
        * The example below shows a <b>Date</b> value (that includes date and time information)
        * using an @see:InputDate and an @see:InputTime control. Notice how both controls
        * are bound to the same controller variable, and each edits the appropriate information
        * (either date or time). The example also shows a @see:Calendar control that can be
        * used to select the date with a single click.
        *
        * @fiddle:vgc3Y
        */
        var InputTime = (function (_super) {
            __extends(InputTime, _super);
            // private stuff
            /**
            * Initializes a new instance of a @see:InputTime control.
            *
            * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
            * @param options The JavaScript object containing initialization data for the control.
            */
            function InputTime(element, options) {
                _super.call(this, element);
                // property storage
                this._value = new Date();
                this._format = 't';
                /**
                * Occurs after a new time is selected.
                */
                this.valueChanged = new wijmo.Event();

                // initialize mask provider
                this._maskProvider = new wijmo._MaskProvider(this._tbx);

                // commit text when losing focus
                // use blur+capture to emulate focusout (not supported in FireFox)
                var self = this;
                this.hostElement.addEventListener('blur', function () {
                    self._commitText();
                }, true);

                // initializing from <input> tag
                if (this._orgTag == 'INPUT') {
                    var value = this._tbx.getAttribute('value');
                    if (value) {
                        this.value = wijmo.Globalize.parseDate(value, 'HH:mm:ss');
                    }
                }

                // initialize control options
                this.initialize(options);
            }
            Object.defineProperty(InputTime.prototype, "value", {
                //--------------------------------------------------------------------------
                //#region ** object model
                /**
                * Gets or sets the current input time.
                */
                get: function () {
                    return this._value;
                },
                set: function (value) {
                    // check type
                    value = wijmo.asDate(value, !this.required);

                    // honor ranges (but keep the dates)
                    if (value) {
                        if (this._min != null && this._getTime(value) < this._getTime(this._min)) {
                            value = wijmo.DateTime.fromDateTime(value, this._min);
                        }
                        if (this._max != null && this._getTime(value) > this._getTime(this._max)) {
                            value = wijmo.DateTime.fromDateTime(value, this._max);
                        }
                    }

                    // update control
                    if (value != this._value) {
                        this._setText(value ? wijmo.Globalize.format(value, this.format) : '', true);
                        this._value = value;
                        this.onValueChanged();
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputTime.prototype, "text", {
                /**
                * Gets or sets the text shown in the control.
                */
                get: function () {
                    return this._tbx.value;
                },
                set: function (value) {
                    if (value != this.text) {
                        this._setText(value, true);
                        this._commitText();
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputTime.prototype, "min", {
                /**
                * Gets or sets the earliest time that the user can enter.
                */
                get: function () {
                    return this._min;
                },
                set: function (value) {
                    this._min = wijmo.asDate(value, true);
                    this.isDroppedDown = false;
                    this._updateItems();
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputTime.prototype, "max", {
                /**
                * Gets or sets the latest time that the user can enter.
                */
                get: function () {
                    return this._max;
                },
                set: function (value) {
                    this._max = wijmo.asDate(value, true);
                    this.isDroppedDown = false;
                    this._updateItems();
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputTime.prototype, "step", {
                /**
                * Gets or sets the number of minutes between entries in the drop-down list.
                */
                get: function () {
                    return this._step;
                },
                set: function (value) {
                    this._step = wijmo.asNumber(value, true);
                    this.isDroppedDown = false;
                    this._updateItems();
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputTime.prototype, "format", {
                /**
                * Gets or sets the format used to display the selected time (see @see:Globalize).
                *
                * The format string is expressed as a .NET-style
                * <a href="http://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx" target="_blank">
                * time format string</a>.
                */
                get: function () {
                    return this._format;
                },
                set: function (value) {
                    if (value != this.format) {
                        this._format = wijmo.asString(value);
                        this._tbx.value = wijmo.Globalize.format(this.value, this.format);
                        if (this.collectionView && this.collectionView.items.length) {
                            this._updateItems();
                        }
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputTime.prototype, "mask", {
                /**
                * Gets or sets a mask to use while the user is editing.
                *
                * The mask format is the same used by the @see:wijmo.input.InputMask
                * control.
                *
                * If specified, the mask must be compatible with the value of
                * the @see:format property. For example, you can use the mask '99:99 >LL'
                * for entering short times (format 't').
                */
                get: function () {
                    return this._maskProvider.mask;
                },
                set: function (value) {
                    this._maskProvider.mask = wijmo.asString(value);
                },
                enumerable: true,
                configurable: true
            });

            /**
            * Raises the @see:valueChanged event.
            */
            InputTime.prototype.onValueChanged = function (e) {
                this.valueChanged.raise(this, e);
            };

            //#endregion ** object model
            //--------------------------------------------------------------------------
            //#region ** overrides
            // update value display in case culture changed
            InputTime.prototype.refresh = function () {
                this.isDroppedDown = false;
                this._maskProvider.refresh();
                this._tbx.value = wijmo.Globalize.format(this.value, this.format);
                this._updateItems();
            };

            // commit changes when the user picks a value from the list
            InputTime.prototype.onSelectedIndexChanged = function (e) {
                if (this.selectedIndex > -1) {
                    this._commitText();
                }
                _super.prototype.onSelectedIndexChanged.call(this, e);
            };

            // update items in drop-down list
            InputTime.prototype._updateItems = function () {
                var min = new Date(0, 0, 0, 0, 0), max = new Date(0, 0, 0, 23, 59, 59), step = wijmo.isNumber(this.step) ? this.step : 30, value = this.value, items = [];
                if (this.min) {
                    min.setHours(this.min.getHours(), this.min.getMinutes(), this.min.getSeconds());
                }
                if (this.max) {
                    max.setHours(this.max.getHours(), this.max.getMinutes(), this.max.getSeconds());
                }
                if (step > 0) {
                    for (var dt = min; dt <= max; dt = wijmo.DateTime.addMinutes(dt, step)) {
                        items.push(wijmo.Globalize.format(dt, this.format));
                    }
                }

                // update item source
                this.itemsSource = items;

                // set value back to original in case setting the itemsSource changed it
                this.value = value;
            };

            // raise textChanged and valueChanged when text changes
            InputTime.prototype.onTextChanged = function (e) {
                _super.prototype.onTextChanged.call(this, e);
                this.onValueChanged(e);
            };

            //#endregion ** overrides
            //--------------------------------------------------------------------------
            //#region ** implementation
            // handle keyboard
            InputTime.prototype._handleKeyDown = function (e) {
                _super.prototype._handleKeyDown.call(this, e);
                switch (e.keyCode) {
                    case wijmo.Key.Enter:
                        this._commitText();
                        break;
                }
            };

            // parse date, commit if successful or revert
            InputTime.prototype._commitText = function () {
                if (!this.text && !this.required) {
                    this.value = null;
                } else {
                    var dt = wijmo.Globalize.parseDate(this.text, this.format);
                    if (dt) {
                        this.value = wijmo.DateTime.fromDateTime(this.value, dt);
                    } else {
                        this._tbx.value = wijmo.Globalize.format(this.value, this.format);
                    }
                }
            };

            // gets the time of day in seconds
            InputTime.prototype._getTime = function (value) {
                return value.getHours() * 3600 + value.getMinutes() * 60 + value.getSeconds();
            };
            return InputTime;
        })(input.ComboBox);
        input.InputTime = InputTime;
    })(wijmo.input || (wijmo.input = {}));
    var input = wijmo.input;
})(wijmo || (wijmo = {}));
var wijmo;
(function (wijmo) {
    (function (input) {
        'use strict';

        /**
        * The @see:InputNumber control allows users to enter numbers.
        *
        * The control prevents users from accidentally entering invalid data and
        * formats the number as it is edited.
        *
        * You may use the @see:min and @see:max properties to limit the range of
        * acceptable values, and the @see:step property to provide spinner buttons
        * that increase or decrease the value with a click.
        *
        * Use the @see:value property to get or set the currently selected number.
        *
        * The example below creates several @see:InputNumber controls and shows
        * the effect of using different formats, ranges, and step values.
        *
        * @fiddle:Cf9L9
        */
        var InputNumber = (function (_super) {
            __extends(InputNumber, _super);
            /**
            * Initializes a new instance of an @see:InputNumber control.
            *
            * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
            * @param options The JavaScript object containing initialization data for the control.
            */
            function InputNumber(element, options) {
                _super.call(this, element);
                this._showBtn = true;
                this._required = true;
                this._decChar = '.';
                this._ehSelectAll = this.selectAll.bind(this);
                /**
                * Occurs when the value of the @see:text property changes.
                */
                this.textChanged = new wijmo.Event();
                /**
                * Occurs when the value of the @see:value property changes.
                */
                this.valueChanged = new wijmo.Event();

                // instantiate and apply template
                var tpl = this.getTemplate();
                this.applyTemplate('wj-control wj-inputnumber wj-content', tpl, {
                    _tbx: 'input',
                    _btnUp: 'btn-inc',
                    _btnDn: 'btn-dec'
                }, 'input');

                // initializing from <input> tag
                if (this._orgTag == 'INPUT') {
                    // copy original attributes to new <input> element
                    this._copyOriginalAttributes(this._tbx);

                    // initialize value from attribute
                    var value = this._tbx.getAttribute('value');
                    if (value) {
                        this.value = wijmo.Globalize.parseFloat(value);
                    }
                }

                // get localized decimal symbol
                this._decChar = wijmo.Globalize.getNumberDecimalSeparator();

                // update button display
                this._updateBtn();

                // hook up events
                var self = this;

                // textbox events
                var tb = self._tbx;
                tb.addEventListener('keypress', this._keypress.bind(this));
                tb.addEventListener('keydown', this._keydown.bind(this));
                tb.addEventListener('input', this._input.bind(this));
                tb.addEventListener('focus', function () {
                    setTimeout(self._ehSelectAll, 0);
                });

                // inc/dec buttons: change value
                // if this was a tap, keep focus on button; OW transfer to textbox
                this._btnUp.addEventListener('click', function (e) {
                    if (self.value != null) {
                        self.value += self.step;
                        if (!self.isTouching) {
                            setTimeout(self._ehSelectAll, 0);
                        }
                    }
                });
                this._btnDn.addEventListener('click', function (e) {
                    if (self.value != null) {
                        self.value -= self.step;
                        if (!self.isTouching) {
                            setTimeout(self._ehSelectAll, 0);
                        }
                    }
                });

                // host element
                this.hostElement.addEventListener('focus', function () {
                    if (!this.isTouching) {
                        tb.focus();
                    }
                });

                // use blur+capture to emulate focusout (not supported in FireFox)
                this.hostElement.addEventListener('blur', function () {
                    var text = wijmo.Globalize.format(self.value, self.format);
                    self._setText(text);
                }, true);

                // initialize value
                this.value = 0;

                // initialize control options
                this.initialize(options);
            }
            Object.defineProperty(InputNumber.prototype, "inputElement", {
                //--------------------------------------------------------------------------
                //#region ** object model
                /**
                * Gets the HTML input element hosted by the control.
                *
                * Use this property in situations where you want to customize the
                * attributes of the input element.
                */
                get: function () {
                    return this._tbx;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputNumber.prototype, "inputType", {
                /**
                * Gets or sets the "type" attribute of the HTML input element hosted by the control.
                *
                * By default, this property is set to "tel," a value that causes mobile devices to
                * show a numeric keypad that includes a negative sign and a decimal separator.
                *
                * Use this property to change the default setting if the default does not work well
                * for the current culture, device, or application. In these cases, try changing
                * the value to "number" or "text."
                */
                get: function () {
                    return this._tbx.type;
                },
                set: function (value) {
                    this._tbx.type = wijmo.asString(value);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputNumber.prototype, "value", {
                /**
                * Gets or sets the current value of the control.
                */
                get: function () {
                    return this._value;
                },
                set: function (value) {
                    if (value != this._value) {
                        value = wijmo.asNumber(value, !this.required);
                        if (value == null) {
                            this._setText('');
                        } else if (!isNaN(value)) {
                            value = this._clamp(value);
                            var text = wijmo.Globalize.format(value, this.format);
                            this._setText(text);
                        }
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputNumber.prototype, "required", {
                /**
                * Gets or sets a value indicating whether the control value must be a number or whether it
                * can be set to null (by deleting the content of the control).
                */
                get: function () {
                    return this._required;
                },
                set: function (value) {
                    this._required = wijmo.asBoolean(value);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputNumber.prototype, "min", {
                /**
                * Gets or sets the smallest number that the user can enter.
                */
                get: function () {
                    return this._min;
                },
                set: function (value) {
                    this._min = wijmo.asNumber(value, true);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputNumber.prototype, "max", {
                /**
                * Gets or sets the largest number that the user can enter.
                */
                get: function () {
                    return this._max;
                },
                set: function (value) {
                    this._max = wijmo.asNumber(value, true);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputNumber.prototype, "step", {
                /**
                * Gets or sets the amount to add or subtract to the @see:value property
                * when the user clicks the spinner buttons.
                */
                get: function () {
                    return this._step;
                },
                set: function (value) {
                    this._step = wijmo.asNumber(value, true);
                    this._updateBtn();
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputNumber.prototype, "format", {
                /**
                * Gets or sets the format used to display the number being edited (see @see:Globalize).
                *
                * The format string is expressed as a .NET-style
                * <a href="http://msdn.microsoft.com/en-us/library/dwhawy9k(v=vs.110).aspx" target="_blank">
                * standard numeric format string</a>.
                */
                get: function () {
                    return this._format;
                },
                set: function (value) {
                    if (value != this.format) {
                        this._format = wijmo.asString(value);
                        this.refresh();
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputNumber.prototype, "text", {
                /**
                * Gets or sets the text shown in the control.
                */
                get: function () {
                    return this._tbx.value;
                },
                set: function (value) {
                    if (value != this.text) {
                        this._setText(value);
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputNumber.prototype, "placeholder", {
                /**
                * Gets or sets the string shown as a hint when the control is empty.
                */
                get: function () {
                    return this._tbx.placeholder;
                },
                set: function (value) {
                    this._tbx.placeholder = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputNumber.prototype, "showSpinner", {
                /**
                * Gets or sets a value indicating whether the control displays spinner buttons to increment
                * or decrement the value (the step property must be set to a non-zero value).
                */
                get: function () {
                    return this._showBtn;
                },
                set: function (value) {
                    this._showBtn = wijmo.asBoolean(value);
                    this._updateBtn();
                },
                enumerable: true,
                configurable: true
            });

            /**
            * Sets the focus to the control and selects all its content.
            */
            InputNumber.prototype.selectAll = function () {
                var rng = this._getInputRange();
                this._tbx.setSelectionRange(rng[0], rng[1]);
            };

            /**
            * Raises the @see:textChanged event.
            */
            InputNumber.prototype.onTextChanged = function (e) {
                this.textChanged.raise(this, e);
            };

            /**
            * Raises the @see:valueChanged event.
            */
            InputNumber.prototype.onValueChanged = function (e) {
                this.valueChanged.raise(this, e);
            };

            //#endregion
            //--------------------------------------------------------------------------
            //#region ** overrides
            InputNumber.prototype.refresh = function (fullUpdate) {
                this._decChar = wijmo.Globalize.getNumberDecimalSeparator();
                this._setText(wijmo.Globalize.format(this.value, this.format));
            };

            //#endregion
            //--------------------------------------------------------------------------
            //#region ** implementation
            // make sure a value is > min and < max
            InputNumber.prototype._clamp = function (value) {
                return wijmo.clamp(value, this.min, this.max);
            };

            // checks whether a character is a digit, sign, or decimal point
            InputNumber.prototype._isNumeric = function (chr, digitsOnly) {
                if (typeof digitsOnly === "undefined") { digitsOnly = false; }
                var isNum = (chr == this._decChar) || (chr >= '0' && chr <= '9');
                if (!isNum && !digitsOnly) {
                    isNum = '+-()'.indexOf(chr) > -1;
                }
                return isNum;
            };

            // get the range of numeric characters within the current text
            InputNumber.prototype._getInputRange = function (digitsOnly) {
                if (typeof digitsOnly === "undefined") { digitsOnly = false; }
                var rng = [0, 0], text = this.text, hasStart = false;
                for (var i = 0; i < text.length; i++) {
                    if (this._isNumeric(text[i], digitsOnly)) {
                        if (!hasStart) {
                            rng[0] = i;
                            hasStart = true;
                        }
                        rng[1] = i + 1;
                    }
                }
                return rng;
            };

            // move the cursor to the left of the first digit
            InputNumber.prototype._moveToDigit = function () {
                var rng = this._getInputRange(true);
                this._tbx.setSelectionRange(rng[0], rng[1]);
            };

            // update button visibility
            InputNumber.prototype._updateBtn = function () {
                if (this._showBtn && this._step) {
                    // show buttons and add class
                    this._btnUp.style.display = this._btnDn.style.display = '';
                    wijmo.addClass(this.hostElement, 'wj-input-show-spinner');
                } else {
                    // hide buttons and remove class
                    this._btnUp.style.display = this._btnDn.style.display = 'none';
                    wijmo.removeClass(this.hostElement, 'wj-input-show-spinner');
                }
            };

            // update text in textbox
            InputNumber.prototype._setText = function (text) {
                // handle nulls
                if (!text) {
                    // if not required, allow setting to null
                    if (!this._required) {
                        this._tbx.value = '';
                        if (this._value != null) {
                            this._value = null;
                            this.onValueChanged();
                        }
                        if (this._oldText) {
                            this._oldText = text;
                            this.onTextChanged();
                        }
                        return;
                    }

                    // required, change text to zero
                    text = '0';
                }

                // let user start typing negative numbers
                if (text == '-' || text == ')') {
                    this._tbx.value = text;
                    this._tbx.setSelectionRange(1, 1);
                    return;
                }

                // handle case when user deletes the opening parenthesis...
                if (text.length > 1 && text[text.length - 1] == ')' && text[0] != '(') {
                    text = text.substr(0, text.length - 1);
                }

                // parse input
                var value = wijmo.Globalize.parseFloat(text);
                if (isNaN(value)) {
                    this._tbx.value = this._oldText;
                    return;
                }

                // handle percentages
                if (this._oldText && this._oldText.indexOf('%') > -1 && text.indexOf('%') < 1) {
                    value /= 100;
                }

                // update text with formatted value
                text = wijmo.Globalize.format(value, this.format);

                // update text
                if (text != this._tbx.value) {
                    this._tbx.value = text;
                }

                // update value, raise valueChanged
                value = this._clamp(value);
                if (value != this._value) {
                    this._value = value;
                    this.onValueChanged();
                }

                // raise textChanged
                if (text != this._oldText) {
                    this._oldText = text;
                    this.onTextChanged();
                }
            };

            // handle the keypress events
            InputNumber.prototype._keypress = function (e) {
                if (e.charCode) {
                    // prevent invalid chars
                    var chr = String.fromCharCode(e.charCode);
                    if (!this._isNumeric(chr)) {
                        e.preventDefault();
                    }

                    // handle minus sign
                    if (chr == '-') {
                        if (this.value != 0) {
                            this.value *= -1;
                            this._moveToDigit();
                        } else {
                            this._setText('-');
                        }
                        e.preventDefault();
                    }

                    // handle plus sign
                    if (chr == '+') {
                        this.value = Math.abs(this.value);
                        this._moveToDigit();
                        e.preventDefault();
                    }

                    // prevent extra decimals
                    if (chr == this._decChar) {
                        var pos = this._tbx.value.indexOf(chr);
                        if (pos > -1) {
                            if (this._getSelStart() <= pos) {
                                pos++;
                            }
                            this._tbx.setSelectionRange(pos, pos);
                            e.preventDefault();
                        }
                    }
                }
            };

            // handle the keydown event
            InputNumber.prototype._keydown = function (e) {
                switch (e.keyCode) {
                    case wijmo.Key.Up:
                    case wijmo.Key.Down:
                        if (this.step) {
                            this.value = this._clamp(this.value + this.step * (e.keyCode == wijmo.Key.Up ? +1 : -1));
                            setTimeout(this.selectAll.bind(this), 0);
                            e.preventDefault();
                        }
                        break;

                    case wijmo.Key.Back:
                        if (this._getSelStart() == this._tbx.selectionEnd) {
                            var sel = this._getSelStart();
                            if (sel > 0 && this.text[sel - 1] == this._decChar) {
                                this._tbx.setSelectionRange(sel - 1, sel - 1);
                                e.preventDefault();
                            }
                        }
                        break;

                    case wijmo.Key.Delete:
                        if (this._getSelStart() == this._tbx.selectionEnd) {
                            var sel = this._getSelStart();
                            if (sel > 0 && this.text[sel] == this._decChar) {
                                this._tbx.setSelectionRange(sel + 1, sel + 1);
                                e.preventDefault();
                            }
                        }
                        break;
                }
            };

            // handle user input
            InputNumber.prototype._input = function (e) {
                // remember cursor position
                var tbx = this._tbx, text = tbx.value, sel = this._getSelStart(), dec = text.indexOf(this._decChar);

                // set the text
                this._setText(text);

                // update cursor position
                if (text) {
                    // keep cursor position from the right
                    if (dec < 0 || sel <= dec) {
                        sel += tbx.value.length - text.length;
                    }

                    // handle cases when user deletes everything
                    if (this._oldText && this._oldText.indexOf(this._decChar) > -1 && tbx.value.indexOf(this._decChar) > -1 && dec < 0) {
                        sel = tbx.value.indexOf(this._decChar);
                    }

                    // make sure it's within the valid range
                    var rng = this._getInputRange();
                    if (sel < rng[0])
                        sel = rng[0];
                    if (sel > rng[1])
                        sel = rng[1];

                    // set cursor position
                    tbx.setSelectionRange(sel, sel);
                }
            };

            // get selection start
            InputNumber.prototype._getSelStart = function () {
                return this._tbx && this._tbx.value ? this._tbx.selectionStart : 0;
            };
            InputNumber.controlTemplate = '<div class="wj-input">' + '<div class="wj-input-group">' + '<span wj-part="btn-dec" class="wj-input-group-btn" tabindex="-1">' + '<button class="wj-btn wj-btn-default" type="button" tabindex="-1">-</button>' + '</span>' + '<input type="tel" wj-part="input" class="wj-form-control wj-numeric"/>' + '<span wj-part="btn-inc" class="wj-input-group-btn" tabindex="-1">' + '<button class="wj-btn wj-btn-default" type="button" tabindex="-1">+</button>' + '</span>' + '</div>';
            return InputNumber;
        })(wijmo.Control);
        input.InputNumber = InputNumber;
    })(wijmo.input || (wijmo.input = {}));
    var input = wijmo.input;
})(wijmo || (wijmo = {}));
var wijmo;
(function (wijmo) {
    (function (input) {
        'use strict';

        /**
        * The @see:InputMask control provides a way to govern what a user is allowed to input.
        *
        * The control prevents users from accidentally entering invalid data and
        * saves time by skipping over literals (such as slashes in dates) as the user types.
        *
        * The mask used to validate the input is defined by the @see:mask property,
        * which may contain one or more of the following special characters:
        *
        *  <dl class="dl-horizontal">
        *      <dt>0</dt>      <dd>Digit.</dd>
        *      <dt>9</dt>      <dd>Digit or space.</dd>
        *      <dt>#</dt>      <dd>Digit, sign, or space.</dd>
        *      <dt>L</dt>      <dd>Letter.</dd>
        *      <dt>l</dt>      <dd>Letter or space.</dd>
        *      <dt>A</dt>      <dd>Alphanumeric.</dd>
        *      <dt>a</dt>      <dd>Alphanumeric or space.</dd>
        *      <dt>.</dt>      <dd>Localized decimal point.</dd>
        *      <dt>,</dt>      <dd>Localized thousand separator.</dd>
        *      <dt>:</dt>      <dd>Localized time separator.</dd>
        *      <dt>/</dt>      <dd>Localized date separator.</dd>
        *      <dt>$</dt>      <dd>Localized currency symbol.</dd>
        *      <dt>&lt;</dt>   <dd>Converts characters that follow to lowercase.</dd>
        *      <dt>&gt;</dt>   <dd>Converts characters that follow to uppercase.</dd>
        *      <dt>|</dt>      <dd>Disables case conversion.</dd>
        *      <dt>\</dt>      <dd>Escapes any character, turning it into a literal.</dd>
        *      <dt>All others</dt><dd>Literals.</dd>
        *  </dl>
        */
        var InputMask = (function (_super) {
            __extends(InputMask, _super);
            /**
            * Initializes a new instance of an @see:InputMask control.
            *
            * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
            * @param options The JavaScript object containing initialization data for the control.
            */
            function InputMask(element, options) {
                _super.call(this, element);
                /**
                * Occurs when the value of the @see:value property changes.
                */
                this.valueChanged = new wijmo.Event();

                // instantiate and apply template
                var tpl = this.getTemplate();
                this.applyTemplate('wj-control wj-inputmask wj-content', tpl, {
                    _tbx: 'input'
                }, 'input');

                // initializing from <input> tag
                if (this._orgTag == 'INPUT') {
                    // copy original attributes to new <input> element
                    this._copyOriginalAttributes(this._tbx);

                    // initialize value from attribute
                    var value = this._tbx.getAttribute('value');
                    if (value) {
                        this.value = value;
                    }
                }

                // create mask provider
                this._maskProvider = new wijmo._MaskProvider(this._tbx);

                // initialize control options
                this.initialize(options);

                // select all content when getting the focus
                var self = this;
                this._tbx.addEventListener('focus', function () {
                    self.selectAll();
                });
                this._tbx.addEventListener('input', function () {
                    self.onValueChanged();
                });
            }
            Object.defineProperty(InputMask.prototype, "inputElement", {
                //--------------------------------------------------------------------------
                //#region ** object model
                /**
                * Gets the HTML input element hosted by the control.
                *
                * Use this property in situations where you want to customize the
                * attributes of the input element.
                */
                get: function () {
                    return this._tbx;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputMask.prototype, "value", {
                /**
                * Gets or sets the text currently shown in the control.
                */
                get: function () {
                    return this._tbx.value;
                },
                set: function (value) {
                    if (value != this.value) {
                        this._tbx.value = wijmo.asString(value);
                        this.onValueChanged();
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputMask.prototype, "mask", {
                /**
                * Gets or sets the mask used to validate the input as the user types.
                *
                * The mask is defined as a string with one or more of the masking
                * characters listed in the @see:InputMask topic.
                */
                get: function () {
                    return this._maskProvider.mask;
                },
                set: function (value) {
                    this._maskProvider.mask = wijmo.asString(value);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputMask.prototype, "promptChar", {
                /**
                * Gets or sets the symbol used to show input positions in the control.
                */
                get: function () {
                    return this._maskProvider.promptChar;
                },
                set: function (value) {
                    this._maskProvider.promptChar = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputMask.prototype, "placeholder", {
                /**
                * Gets or sets the string shown as a hint when the control is empty.
                */
                get: function () {
                    return this._tbx.placeholder;
                },
                set: function (value) {
                    this._tbx.placeholder = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputMask.prototype, "maskFull", {
                /**
                * Gets a value that indicates whether the mask has been completely filled.
                */
                get: function () {
                    return this._maskProvider.maskFull;
                },
                enumerable: true,
                configurable: true
            });

            /**
            * Sets the focus to the control and selects all its content.
            */
            InputMask.prototype.selectAll = function () {
                var rng = this._maskProvider.getMaskRange();
                this._tbx.setSelectionRange(rng[0], rng[1] + 1);
            };

            /**
            * Raises the @see:valueChanged event.
            */
            InputMask.prototype.onValueChanged = function (e) {
                this.valueChanged.raise(this, e);
            };

            //#endregion
            //--------------------------------------------------------------------------
            //#region ** overrides
            InputMask.prototype.refresh = function (fullUpdate) {
                this._maskProvider.refresh();
            };
            InputMask.controlTemplate = '<div class="wj-input">' + '<div class="wj-input-group">' + '<input wj-part="input" class="wj-form-control"/>' + '</div>';
            return InputMask;
        })(wijmo.Control);
        input.InputMask = InputMask;
    })(wijmo.input || (wijmo.input = {}));
    var input = wijmo.input;
})(wijmo || (wijmo = {}));
var wijmo;
(function (wijmo) {
    (function (input) {
        'use strict';

        /**
        * The @see:InputColor control allows users to select colors by typing in
        * HTML-supported color strings, or to pick colors from a drop-down
        * that shows a @see:ColorPicker control.
        *
        * Use the @see:value property to get or set the currently selected color.
        *
        * @fiddle:84xvsz90
        */
        var InputColor = (function (_super) {
            __extends(InputColor, _super);
            /**
            * Initializes a new instance of an @see:InputColor control.
            *
            * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
            * @param options The JavaScript object containing initialization data for the control.
            */
            function InputColor(element, options) {
                _super.call(this, element);
                this._required = true;
                /**
                * Occurs after a new color is selected.
                */
                this.valueChanged = new wijmo.Event();

                // create preview element
                this._tbx.style.paddingLeft = '24px';
                this._ePreview = wijmo.createElement('<div style="position:absolute;left:6px;top:6px;width:12px;bottom:6px;border:1px solid black"></div>');
                this.hostElement.style.position = 'relative';
                this.hostElement.appendChild(this._ePreview);

                // initializing from <input> tag
                if (this._orgTag == 'INPUT') {
                    this._tbx.type = '';
                    this._commitText();
                }

                // initialize value to white
                this.value = '#ffffff';

                // initialize control options
                this.initialize(options);

                // handle keyboard
                var self = this;
                this._tbx.addEventListener('keydown', function (e) {
                    switch (e.keyCode) {
                        case wijmo.Key.Enter:
                            self._commitText();
                            break;
                        case wijmo.Key.Escape:
                            self.text = self.value;
                            break;
                    }
                });
            }
            Object.defineProperty(InputColor.prototype, "value", {
                //--------------------------------------------------------------------------
                //#region ** object model
                /**
                * Gets or sets the current color.
                */
                get: function () {
                    return this._value;
                },
                set: function (value) {
                    if (value != this.value) {
                        if (!this.required || value) {
                            this.text = wijmo.asString(value);
                        }
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputColor.prototype, "text", {
                /**
                * Gets or sets the text shown on the control.
                */
                get: function () {
                    return this._tbx.value;
                },
                set: function (value) {
                    if (value != this.text) {
                        this._setText(wijmo.asString(value), true);
                        this._commitText();
                    }
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputColor.prototype, "required", {
                /**
                * Gets or sets a value indicating whether the control value must be a color or whether it
                * can be set to null (by deleting the content of the control).
                */
                get: function () {
                    return this._required;
                },
                set: function (value) {
                    this._required = wijmo.asBoolean(value);
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputColor.prototype, "showAlphaChannel", {
                /**
                * Gets or sets a value indicating whether the @see:ColorPicker allows users
                * to edit the color's alpha channel (transparency).
                */
                get: function () {
                    return this._colorPicker.showAlphaChannel;
                },
                set: function (value) {
                    this._colorPicker.showAlphaChannel = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(InputColor.prototype, "colorPicker", {
                /**
                * Gets a reference to the @see:ColorPicker control shown in the drop-down.
                */
                get: function () {
                    return this._colorPicker;
                },
                enumerable: true,
                configurable: true
            });

            /**
            * Raises the @see:valueChanged event.
            */
            InputColor.prototype.onValueChanged = function (e) {
                this.valueChanged.raise(this, e);
            };

            //#endregion ** object model
            //--------------------------------------------------------------------------
            //#region ** overrides
            // create the drop-down element
            InputColor.prototype._createDropDown = function () {
                var self = this;

                // create the drop-down element
                this._colorPicker = new input.ColorPicker(this._dropDown);
                wijmo.setCss(this._dropDown, {
                    minWidth: 420,
                    minHeight: 200
                });

                // update our value to match colorPicker's
                this._colorPicker.valueChanged.addHandler(function () {
                    self.value = self._colorPicker.value;
                });
            };

            //#endregion ** overrides
            //--------------------------------------------------------------------------
            //#region ** implementation
            // assign new color to ColorPicker
            InputColor.prototype._commitText = function () {
                if (this.value != this.text) {
                    // allow empty values
                    if (!this.required && !this.text) {
                        this._value = this.text;
                        this._ePreview.style.backgroundColor = '';
                        return;
                    }

                    // parse and assign color to control
                    var c = wijmo.Color.fromString(this.text);
                    if (c) {
                        this._colorPicker.value = this.text;
                        this._value = this._colorPicker.value;
                        this._ePreview.style.backgroundColor = this.value;
                        this.onValueChanged();
                    } else {
                        this.text = this._value ? this._value : '';
                    }
                }
            };
            return InputColor;
        })(input.DropDown);
        input.InputColor = InputColor;
    })(wijmo.input || (wijmo.input = {}));
    var input = wijmo.input;
})(wijmo || (wijmo = {}));
//# sourceMappingURL=wijmo.input.js.map
