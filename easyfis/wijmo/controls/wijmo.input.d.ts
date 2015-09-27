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
declare module wijmo.input {
    /**
    * DropDown control (abstract).
    *
    * Contains an input element and a button used to show or hide the drop-down.
    *
    * Derived classes must override the _createDropDown method to create whatever
    * editor they want to show in the drop down area (a list of items, a calendar,
    * a color editor, etc).
    */
    class DropDown extends Control {
        public _tbx: HTMLInputElement;
        public _elRef: HTMLElement;
        public _btn: HTMLElement;
        public _dropDown: HTMLElement;
        public _showBtn: boolean;
        public _oldText: string;
        /**
        * Gets or sets the template used to instantiate @see:DropDown controls.
        */
        static controlTemplate: string;
        /**
        * Initializes a new instance of a @see:DropDown control.
        *
        * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
        * @param options The JavaScript object containing initialization data for the control.
        */
        constructor(element: any, options?: any);
        /**
        * Checks whether this control or its drop-down contain the focused element.
        */
        public containsFocus(): boolean;
        /**
        * Gets or sets the text shown on the control.
        */
        public text : string;
        /**
        * Gets the HTML input element hosted by the control.
        *
        * Use this property in situations where you want to customize the
        * attributes of the input element.
        */
        public inputElement : HTMLInputElement;
        /**
        * Gets or sets the string shown as a hint when the control is empty.
        */
        public placeholder : string;
        /**
        * Gets or sets a value indicating whether the drop down is currently visible.
        */
        public isDroppedDown : boolean;
        /**
        * Gets the drop down element shown when the @see:isDroppedDown
        * property is set to true.
        */
        public dropDown : HTMLElement;
        /**
        * Gets or sets a value indicating whether the control should display a drop-down button.
        */
        public showDropDownButton : boolean;
        /**
        * Sets the focus to the control and selects all its content.
        */
        public selectAll(): void;
        /**
        * Occurs when the value of the @see:text property changes.
        */
        public textChanged: Event;
        /**
        * Raises the @see:textChanged event.
        */
        public onTextChanged(e?: EventArgs): void;
        /**
        * Occurs after the drop down is shown or hidden.
        */
        public isDroppedDownChanged: Event;
        /**
        * Raises the @see:isDroppedDownChanged event.
        */
        public onIsDroppedDownChanged(e?: EventArgs): void;
        public _setText(text: string, fullMatch: boolean): void;
        public _updateBtn(): void;
        public _createDropDown(): void;
        public _updateDropDown(): void;
    }
}

declare module wijmo.input {
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
    class Calendar extends Control {
        private _tblHdr;
        private _tblMonth;
        private _tblYear;
        private _tdMonth;
        private _spMonth;
        private _btnPrev;
        private _btnToday;
        private _btnNext;
        private _value;
        private _currMonth;
        private _firstDay;
        private _min;
        private _max;
        private _fdw;
        private _itemFormatter;
        /**
        * Gets or sets the template used to instantiate @see:Calendar controls.
        */
        static controlTemplate: string;
        /**
        * Initializes a new instance of a @see:Calendar.
        *
        * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
        * @param options The JavaScript object containing initialization data for the control.
        */
        constructor(element: any, options?: any);
        /**
        * Gets or sets the currently selected date.
        */
        public value : Date;
        /**
        * Gets or sets the earliest date that the user can select in the calendar.
        */
        public min : Date;
        /**
        * Gets or sets the latest date that the user can select in the calendar.
        */
        public max : Date;
        /**
        * Gets or sets a value that represents the first day of the week,
        * the one displayed in the first column of the calendar.
        *
        * Setting this property to null causes the calendar to use the default
        * for the current culture. In the English culture, the first day of the
        * week is Sunday (0); in most European cultures, the first day of the
        * week is Monday (1).
        */
        public firstDayOfWeek : number;
        /**
        * Gets or sets the month displayed in the calendar.
        */
        public displayMonth : Date;
        /**
        * Gets or sets a value indicating whether the control displays the header
        * area with the current month and navigation buttons.
        */
        public showHeader : boolean;
        /**
        * Gets or sets a value indicating whether the calendar displays a month or a year.
        */
        public monthView : boolean;
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
        public itemFormatter : Function;
        /**
        * Occurs after a new date is selected.
        */
        public valueChanged: Event;
        /**
        * Raises the @see:valueChanged event.
        */
        public onValueChanged(e?: EventArgs): void;
        /**
        * Occurs after the @see:displayMonth property changes.
        */
        public displayMonthChanged: Event;
        /**
        * Raises the @see:displayMonthChanged event.
        */
        public onDisplayMonthChanged(e?: EventArgs): void;
        /**
        * Refreshes the control.
        *
        * @param fullUpdate Indicates whether to update the control layout as well as the content.
        */
        public refresh(fullUpdate?: boolean): void;
        private _isValidDay(value);
        private _isValidMonth(value);
        private _createChildren();
        private _click(e);
        private _closest(e, tag);
        private _getCellIndex(tbl, cell);
        private _keydown(e);
        private _getMonth(date);
        private _addDays(days);
        private _addMonths(months);
        private _navigateDate(month, skip);
        public _highlightElement(e: HTMLElement, highlight: boolean): void;
    }
}

declare module wijmo.input {
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
    class ColorPicker extends Control {
        public _hsb: number[];
        public _alpha: number;
        public _value: string;
        public _palette: string[];
        public _eSB: HTMLElement;
        public _eHue: HTMLElement;
        public _eAlpha: HTMLElement;
        public _cSB: HTMLElement;
        public _cHue: HTMLElement;
        public _cAlpha: HTMLElement;
        public _ePal: HTMLElement;
        public _ePreview: HTMLElement;
        public _eText: HTMLElement;
        public _htDown: Element;
        /**
        * Gets or sets the template used to instantiate @see:ColorPicker controls.
        */
        static controlTemplate: string;
        static _tplCursor: string;
        /**
        * Initializes a new instance of a @see:ColorPicker.
        *
        * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
        * @param options The JavaScript object containing initialization data for the control.
        */
        constructor(element: any, options?: any);
        /**
        * Gets or sets a value indicating whether the @see:ColorPicker allows users
        * to edit the color's alpha channel (transparency).
        */
        public showAlphaChannel : boolean;
        /**
        * Gets or sets a value indicating whether the @see:ColorPicker shows a string representation
        * of the current color.
        */
        public showColorString : boolean;
        /**
        * Gets or sets the currently selected color.
        */
        public value : string;
        /**
        * Gets or sets an array that contains the colors in the palette.
        *
        * The palette contains ten colors, represented by an array with
        * ten strings. The first two colors are usually white and black.
        */
        public palette : string[];
        /**
        * Occurs when the color changes.
        */
        public valueChanged: Event;
        /**
        * Raises the @see:valueChanged event.
        */
        public onValueChanged(): void;
        public _mouseDown(e: MouseEvent): void;
        public _mouseMove(e: MouseEvent): void;
        public _mouseUp(e: MouseEvent): void;
        public _updateColor(): void;
        public _updatePalette(): void;
        public _makePalEntry(color: Color, margin: number): HTMLElement;
        public _updatePanels(): void;
        public _getTargetPanel(e: MouseEvent): HTMLElement;
    }
}

declare module wijmo.input {
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
    class ListBox extends Control {
        public _div: HTMLDivElement;
        public _items: any;
        public _cv: collections.ICollectionView;
        public _itemFormatter: Function;
        public _pathDisplay: string;
        public _pathValue: string;
        public _pathChecked: string;
        public _html: boolean;
        /**
        * Gets or sets the template used to instantiate @see:ListBox controls.
        */
        static controlTemplate: string;
        /**
        * Initializes a new instance of a @see:ListBox.
        *
        * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
        * @param options The JavaScript object containing initialization data for the control.
        */
        constructor(element: any, options?: any);
        /**
        * Refreshes the list.
        */
        public refresh(): void;
        /**
        * Gets or sets the array or @see:ICollectionView object that contains the list items.
        */
        public itemsSource : any;
        /**
        * Gets the @see:ICollectionView object used as the item source.
        */
        public collectionView : collections.ICollectionView;
        /**
        * Gets or sets a value indicating whether items contain plain text or HTML.
        */
        public isContentHtml : boolean;
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
        public itemFormatter : Function;
        /**
        * Gets or sets the name of the property to use as the visual representation of the items.
        */
        public displayMemberPath : string;
        /**
        * Gets or sets the name of the property used to get the @see:selectedValue
        * from the @see:selectedItem.
        */
        public selectedValuePath : string;
        /**
        * Gets or sets the name of the property used to control checkboxes placed next
        * to each item.
        *
        * Use this property to create multi-select lisboxes.
        * When an item is checked or unchecked, the control raises the @see:itemChecked event.
        * Use the @see:selectedItem property to retrieve the item that was checked or unchecked.
        */
        public checkedMemberPath : string;
        /**
        * Gets the string displayed for the item at a given index.
        *
        * The string may be plain text or HTML, depending on the setting
        * of the @see:isContentHtml property.
        *
        * @param index The index of the item.
        */
        public getDisplayValue(index: number): string;
        /**
        * Gets the text displayed for the item at a given index (as plain text).
        *
        * @param index The index of the item.
        */
        public getDisplayText(index: number): string;
        /**
        * Gets or sets the index of the currently selected item.
        */
        public selectedIndex : number;
        /**
        * Gets or sets the item that is currently selected.
        */
        public selectedItem : any;
        /**
        * Gets or sets the value of the @see:selectedItem obtained using the @see:selectedValuePath.
        */
        public selectedValue : any;
        /**
        * Gets or sets the maximum height of the list.
        */
        public maxHeight : number;
        /**
        * Highlights the selected item and scrolls it into view.
        */
        public showSelection(): void;
        /**
        * Occurs when the value of the @see:selectedIndex property changes.
        */
        public selectedIndexChanged: Event;
        /**
        * Raises the @see:selectedIndexChanged event.
        */
        public onSelectedIndexChanged(e?: EventArgs): void;
        /**
        * Occurs when the list of items changes.
        */
        public itemsChanged: Event;
        /**
        * Raises the @see:itemsChanged event.
        */
        public onItemsChanged(e?: EventArgs): void;
        /**
        * Occurs when the current item is checked or unchecked.
        *
        * This event is raised when the @see:checkedMemberPath is set to the name of a
        * property to add checkboxes to each item in the control.
        *
        * Use the @see:selectedItem property to retrieve the item that was checked or
        * unchecked.
        */
        public itemChecked: Event;
        /**
        * Raises the @see:itemCheched event.
        */
        public onItemChecked(e?: EventArgs): void;
        private _cvCollectionChanged(sender, e);
        private _cvCurrentChanged(sender, e);
        private _populateList();
        private _click(e);
        private _keydown(e);
        private _keypress(e);
        public _populateSelectElement(hostElement: HTMLElement): void;
    }
}

declare module wijmo.input {
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
    class ComboBox extends DropDown {
        public _lbx: ListBox;
        public _required: boolean;
        public _editable: boolean;
        public _composing: boolean;
        public _deleting: boolean;
        public _settingText: boolean;
        /**
        * Initializes a new instance of a @see:ComboBox control.
        *
        * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
        * @param options The JavaScript object containing initialization data for the control.
        */
        constructor(element: any, options?: any);
        /**
        * Gets or sets the array or @see:ICollectionView object that contains the items to select from.
        */
        public itemsSource : any;
        /**
        * Gets the @see:ICollectionView object used as the item source.
        */
        public collectionView : collections.ICollectionView;
        /**
        * Gets or sets the name of the property to use as the visual representation of the items.
        */
        public displayMemberPath : string;
        /**
        * Gets or sets the name of the property used to get the @see:selectedValue from the @see:selectedItem.
        */
        public selectedValuePath : string;
        /**
        * Gets or sets a value indicating whether the drop-down list displays items as plain text or as HTML.
        */
        public isContentHtml : boolean;
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
        public itemFormatter : Function;
        /**
        * Gets or sets the index of the currently selected item in the drop-down list.
        */
        public selectedIndex : number;
        /**
        * Gets or sets the item that is currently selected in the drop-down list.
        */
        public selectedItem : any;
        /**
        * Gets or sets the value of the @see:selectedItem, obtained using the @see:selectedValuePath.
        */
        public selectedValue : any;
        /**
        * Gets or sets whether the control value must be set to a non-null value
        * or whether it can be set to null (by deleting the content of the control).
        */
        public required : boolean;
        /**
        * Gets or sets a value that enables or disables editing of the text
        * in the input element of the @see:ComboBox (defaults to false).
        */
        public isEditable : boolean;
        /**
        * Gets or sets the maximum height of the drop-down list.
        */
        public maxDropDownHeight : number;
        /**
        * Gets or sets the maximum width of the drop-down list.
        */
        public maxDropDownWidth : number;
        /**
        * Gets the string displayed for the item at a given index (as plain text).
        *
        * @param index The index of the item to retrieve the text for.
        */
        public getDisplayText(index?: number): string;
        /**
        * Occurs when the value of the @see:selectedIndex property changes.
        */
        public selectedIndexChanged: Event;
        /**
        * Raises the @see:selectedIndexChanged event.
        */
        public onSelectedIndexChanged(e?: EventArgs): void;
        /**
        * Gets the index of the first item that matches a given string.
        *
        * @param text The text to search for.
        * @param fullMatch A value indicating whether to look for a full match or just the start of the string.
        * @return The index of the item, or -1 if not found.
        */
        public indexOf(text: string, fullMatch: boolean): number;
        public onIsDroppedDownChanged(e?: EventArgs): void;
        public _updateBtn(): void;
        public _createDropDown(): void;
        public _setText(text: string, fullMatch: boolean): void;
        private _findNext(text, step);
        public _handleKeyDown(e: KeyboardEvent): void;
        private _setSelectionRange(start, end);
        private _getSelStart();
    }
}

/**
* Defines input controls for strings, numbers, dates, times, and colors.
*/
declare module wijmo.input {
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
    class AutoComplete extends ComboBox {
        private _itemsSourceFn;
        private _minLength;
        private _maxItems;
        private _itemCount;
        private _delay;
        private _cssMatch;
        private _toSearch;
        private _query;
        private _rxMatch;
        private _rxHighlight;
        private _handlingCallback;
        /**
        * Initializes a new instance of an @see:AutoComplete control.
        *
        * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
        * @param options The JavaScript object containing initialization data for the control.
        */
        constructor(element: any, options?: any);
        /**
        * Gets or sets the minimum input length to trigger autocomplete suggestions.
        */
        public minLength : number;
        /**
        * Gets or sets the maximum number of items to display in the drop-down list.
        */
        public maxItems : number;
        /**
        * Gets or sets the delay, in milliseconds, between when a keystroke occurs
        * and when the search is performed.
        */
        public delay : number;
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
        public itemsSourceFunction : Function;
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
        public cssMatch : string;
        public _setText(text: string): void;
        public _itemSourceFunctionCallback(result: any): void;
        public _handleKeyDown(e: KeyboardEvent): void;
        public onIsDroppedDownChanged(e?: EventArgs): void;
        public _updateItems(): void;
        public _filter(item: any): boolean;
        public _defaultFormatter(index: number, text: string): string;
    }
}

declare module wijmo.input {
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
    class Menu extends ComboBox {
        public _header: HTMLElement;
        public _closing: boolean;
        public _command: any;
        public _cmdPath: string;
        public _cmdParamPath: string;
        public _isButton: boolean;
        /**
        * Initializes a new instance of a @see:Menu control.
        *
        * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
        * @param options The JavaScript object containing initialization data for the control.
        */
        constructor(element: any, options?: any);
        /**
        * Gets or sets the HTML text shown in the @see:Menu element.
        */
        public header : string;
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
        public command : any;
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
        public commandPath : string;
        /**
        * Gets or sets the name of the property that contains a parameter to use with
        * the command specified by the @see:commandPath property.
        */
        public commandParameterPath : string;
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
        public isButton : boolean;
        /**
        * Occurs when the user picksk an item from the menu.
        *
        * The handler can determine which item was picked by reading the event sender's
        * @see:selectedIndex property.
        */
        public itemClicked: Event;
        /**
        * Raises the @see:itemClicked event.
        */
        public onItemClicked(e?: EventArgs): void;
        public onTextChanged(e?: EventArgs): void;
        public onIsDroppedDownChanged(e?: EventArgs): void;
        public _raiseCommand(e?: EventArgs): void;
        public _getCommand(item: any): any;
        public _executeCommand(cmd: any, parm: any): void;
        public _canExecuteCommand(cmd: any, parm: any): boolean;
    }
}

declare module wijmo.input {
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
    class InputDate extends DropDown {
        public _calendar: Calendar;
        public _value: Date;
        public _min: Date;
        public _max: Date;
        public _format: string;
        public _required: boolean;
        public _maskProvider: _MaskProvider;
        /**
        * Initializes a new instance of a @see:InputDate control.
        *
        * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
        * @param options The JavaScript object containing initialization data for the control.
        */
        constructor(element: any, options?: any);
        /**
        * Gets or sets the current date.
        */
        public value : Date;
        /**
        * Gets or sets the text shown on the control.
        */
        public text : string;
        /**
        * Gets or sets a value indicating whether the control value must be a Date or whether it
        * can be set to null (by deleting the content of the control).
        */
        public required : boolean;
        /**
        * Gets or sets the earliest date that the user can enter.
        */
        public min : Date;
        /**
        * Gets or sets the latest date that the user can enter.
        */
        public max : Date;
        /**
        * Gets or sets the format used to display the selected date.
        *
        * The format string is expressed as a .NET-style
        * <a href="http://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx" target="_blank">
        * Date format string</a>.
        */
        public format : string;
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
        public mask : string;
        /**
        * Gets a reference to the @see:Calendar control shown in the drop-down box.
        */
        public calendar : Calendar;
        /**
        * Occurs after a new date is selected.
        */
        public valueChanged: Event;
        /**
        * Raises the @see:valueChanged event.
        */
        public onValueChanged(e?: EventArgs): void;
        public refresh(): void;
        public onIsDroppedDownChanged(e?: EventArgs): void;
        public _createDropDown(): void;
        public _updateDropDown(): void;
        public _handleKeyDown(e: KeyboardEvent): void;
        public _commitText(): void;
        private _setDate(value, time);
    }
}

declare module wijmo.input {
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
    class InputTime extends ComboBox {
        public _value: Date;
        public _min: Date;
        public _max: Date;
        public _step: number;
        public _format: string;
        public _maskProvider: _MaskProvider;
        /**
        * Initializes a new instance of a @see:InputTime control.
        *
        * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
        * @param options The JavaScript object containing initialization data for the control.
        */
        constructor(element: any, options?: any);
        /**
        * Gets or sets the current input time.
        */
        public value : Date;
        /**
        * Gets or sets the text shown in the control.
        */
        public text : string;
        /**
        * Gets or sets the earliest time that the user can enter.
        */
        public min : Date;
        /**
        * Gets or sets the latest time that the user can enter.
        */
        public max : Date;
        /**
        * Gets or sets the number of minutes between entries in the drop-down list.
        */
        public step : number;
        /**
        * Gets or sets the format used to display the selected time (see @see:Globalize).
        *
        * The format string is expressed as a .NET-style
        * <a href="http://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx" target="_blank">
        * time format string</a>.
        */
        public format : string;
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
        public mask : string;
        /**
        * Occurs after a new time is selected.
        */
        public valueChanged: Event;
        /**
        * Raises the @see:valueChanged event.
        */
        public onValueChanged(e?: EventArgs): void;
        public refresh(): void;
        public onSelectedIndexChanged(e?: EventArgs): void;
        public _updateItems(): void;
        public onTextChanged(e?: EventArgs): void;
        public _handleKeyDown(e: KeyboardEvent): void;
        private _commitText();
        private _getTime(value);
    }
}

declare module wijmo.input {
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
    class InputNumber extends Control {
        public _tbx: HTMLInputElement;
        public _btnUp: HTMLElement;
        public _btnDn: HTMLElement;
        public _value: number;
        public _min: number;
        public _max: number;
        public _format: string;
        public _step: number;
        public _showBtn: boolean;
        public _required: boolean;
        public _decChar: string;
        public _oldText: string;
        public _ehSelectAll: any;
        /**
        * Gets or sets the template used to instantiate @see:InputNumber controls.
        */
        static controlTemplate: string;
        public '</div>': any;
        /**
        * Initializes a new instance of an @see:InputNumber control.
        *
        * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
        * @param options The JavaScript object containing initialization data for the control.
        */
        constructor(element: any, options?: any);
        /**
        * Gets the HTML input element hosted by the control.
        *
        * Use this property in situations where you want to customize the
        * attributes of the input element.
        */
        public inputElement : HTMLInputElement;
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
        public inputType : string;
        /**
        * Gets or sets the current value of the control.
        */
        public value : number;
        /**
        * Gets or sets a value indicating whether the control value must be a number or whether it
        * can be set to null (by deleting the content of the control).
        */
        public required : boolean;
        /**
        * Gets or sets the smallest number that the user can enter.
        */
        public min : number;
        /**
        * Gets or sets the largest number that the user can enter.
        */
        public max : number;
        /**
        * Gets or sets the amount to add or subtract to the @see:value property
        * when the user clicks the spinner buttons.
        */
        public step : number;
        /**
        * Gets or sets the format used to display the number being edited (see @see:Globalize).
        *
        * The format string is expressed as a .NET-style
        * <a href="http://msdn.microsoft.com/en-us/library/dwhawy9k(v=vs.110).aspx" target="_blank">
        * standard numeric format string</a>.
        */
        public format : string;
        /**
        * Gets or sets the text shown in the control.
        */
        public text : string;
        /**
        * Gets or sets the string shown as a hint when the control is empty.
        */
        public placeholder : string;
        /**
        * Gets or sets a value indicating whether the control displays spinner buttons to increment
        * or decrement the value (the step property must be set to a non-zero value).
        */
        public showSpinner : boolean;
        /**
        * Sets the focus to the control and selects all its content.
        */
        public selectAll(): void;
        /**
        * Occurs when the value of the @see:text property changes.
        */
        public textChanged: Event;
        /**
        * Raises the @see:textChanged event.
        */
        public onTextChanged(e?: EventArgs): void;
        /**
        * Occurs when the value of the @see:value property changes.
        */
        public valueChanged: Event;
        /**
        * Raises the @see:valueChanged event.
        */
        public onValueChanged(e?: EventArgs): void;
        public refresh(fullUpdate?: boolean): void;
        private _clamp(value);
        private _isNumeric(chr, digitsOnly?);
        private _getInputRange(digitsOnly?);
        private _moveToDigit();
        private _updateBtn();
        private _setText(text);
        private _keypress(e);
        private _keydown(e);
        private _input(e);
        private _getSelStart();
    }
}

declare module wijmo.input {
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
    class InputMask extends Control {
        public _tbx: HTMLInputElement;
        public _maskProvider: _MaskProvider;
        /**
        * Gets or sets the template used to instantiate @see:InputMask controls.
        */
        static controlTemplate: string;
        public '</div>': any;
        /**
        * Initializes a new instance of an @see:InputMask control.
        *
        * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
        * @param options The JavaScript object containing initialization data for the control.
        */
        constructor(element: any, options?: any);
        /**
        * Gets the HTML input element hosted by the control.
        *
        * Use this property in situations where you want to customize the
        * attributes of the input element.
        */
        public inputElement : HTMLInputElement;
        /**
        * Gets or sets the text currently shown in the control.
        */
        public value : string;
        /**
        * Gets or sets the mask used to validate the input as the user types.
        *
        * The mask is defined as a string with one or more of the masking
        * characters listed in the @see:InputMask topic.
        */
        public mask : string;
        /**
        * Gets or sets the symbol used to show input positions in the control.
        */
        public promptChar : string;
        /**
        * Gets or sets the string shown as a hint when the control is empty.
        */
        public placeholder : string;
        /**
        * Gets a value that indicates whether the mask has been completely filled.
        */
        public maskFull : boolean;
        /**
        * Sets the focus to the control and selects all its content.
        */
        public selectAll(): void;
        /**
        * Occurs when the value of the @see:value property changes.
        */
        public valueChanged: Event;
        /**
        * Raises the @see:valueChanged event.
        */
        public onValueChanged(e?: EventArgs): void;
        public refresh(fullUpdate?: boolean): void;
    }
}

declare module wijmo.input {
    /**
    * The @see:InputColor control allows users to select colors by typing in
    * HTML-supported color strings, or to pick colors from a drop-down
    * that shows a @see:ColorPicker control.
    *
    * Use the @see:value property to get or set the currently selected color.
    *
    * @fiddle:84xvsz90
    */
    class InputColor extends DropDown {
        public _ePreview: HTMLElement;
        public _colorPicker: ColorPicker;
        public _value: string;
        public _required: boolean;
        /**
        * Initializes a new instance of an @see:InputColor control.
        *
        * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
        * @param options The JavaScript object containing initialization data for the control.
        */
        constructor(element: any, options?: any);
        /**
        * Gets or sets the current color.
        */
        public value : string;
        /**
        * Gets or sets the text shown on the control.
        */
        public text : string;
        /**
        * Gets or sets a value indicating whether the control value must be a color or whether it
        * can be set to null (by deleting the content of the control).
        */
        public required : boolean;
        /**
        * Gets or sets a value indicating whether the @see:ColorPicker allows users
        * to edit the color's alpha channel (transparency).
        */
        public showAlphaChannel : boolean;
        /**
        * Gets a reference to the @see:ColorPicker control shown in the drop-down.
        */
        public colorPicker : ColorPicker;
        /**
        * Occurs after a new color is selected.
        */
        public valueChanged: Event;
        /**
        * Raises the @see:valueChanged event.
        */
        public onValueChanged(e?: EventArgs): void;
        public _createDropDown(): void;
        public _commitText(): void;
    }
}

