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
module wijmo.chart {
    'use strict';

    /**
     * Provides arguments for @see:Series events.
     */
    export class RenderEventArgs extends EventArgs {
        _engine: IRenderEngine;

        /**
         * Initializes a new instance of a @see:RenderEventArgs object.
         *
         * @param engine (@see:IRenderEngine) The rendering engine to use.
         */
        constructor(engine: IRenderEngine) {
            super();
            this._engine = engine;
        }

        /**
         * Gets the @see:IRenderEngine object to use for rendering the chart elements.
         */
        get engine(): IRenderEngine {
            return this._engine;
        }
    }

    /**
     * The @see:FlexChartBase control from which the FlexChart and FlexPie derive.
     */
    export class FlexChartBase extends Control implements _IPalette {
        static _WIDTH = 300;
        static _HEIGHT = 200;
        static _SELECTION_THRESHOLD = 15;

        // property storage
        _items: any; // any[] or ICollectionView
        _cv: wijmo.collections.ICollectionView;
        private _binding: string;
        private _palette: string[] = null;
        private _selectionMode = SelectionMode.None;
        private _itemFormatter: Function;
        _selectionIndex: number;
        _options: any;
        private _plotMargin: any;
        _header: string;
        _headerStyle: any;
        _footer: string;
        _footerStyle: any;
        _legend: Legend;

        _defPalette = Palettes.standard;// ['#5DA5DA', '#FAA43A', '#60BD68', '#E5126F', '#9D722A'];
        _notifyCurrentChanged: boolean = true;
        _rectFooter: Rect;
        _rectHeader: Rect;
        _rectChart: Rect;
        _rectLegend: Rect;
        _currentRenderEngine: IRenderEngine;

        _legendHost: SVGGElement = null;

        private _needBind = false;
        private _toShow: number;
        private _toHide: number;
        _tooltip: ChartTooltip;

        //--------------------------------------------------------------------------
        // ** object model

        /**
         * Gets or sets the array or @see:ICollectionView object that contains the data used to create the chart.
         */
        get itemsSource(): any {
            return this._items;
        }
        set itemsSource(value: any) {
            if (this._items != value) {

                // unbind current collection view
                if (this._cv) {
                    this._cv.currentChanged.removeHandler(this._cvCurrentChanged, this);
                    this._cv.collectionChanged.removeHandler(this._cvCollectionChanged, this);
                    this._cv = null;
                }

                // save new data source and collection view
                this._items = value;
                this._cv = asCollectionView(value);

                // bind new collection view
                if (this._cv != null) {
                    this._cv.currentChanged.addHandler(this._cvCurrentChanged, this);
                    this._cv.collectionChanged.addHandler(this._cvCollectionChanged, this);
                }

                this._clearCachedValues();

                // bind chart
                this._bindChart();
            }
        }

        /**
         * Gets the @see:ICollectionView object that contains the chart data.
         */
        get collectionView(): wijmo.collections.ICollectionView {
            return this._cv;
        }

        /**
         * Gets or sets the name of the property that contains the Y values.
         */
        get binding(): string {
            return this._binding;
        }
        set binding(value: string) {
            if (value != this._binding) {
                this._binding = asString(value, true);
                this.invalidate();
            }
        }

        /**
         * Gets or sets an array of default colors to use for displaying each series.
         * 
         * The array contains strings that represents css-colors. For example:
         * <pre>
         * // use colors specified by name
         * chart.palette = ['red', 'green', 'blue'];
         * // or use colors specified as rgba-values
         * chart.palette = [
         *   'rgba(255,0,0,1)', 
         *   'rgba(255,0,0,0.8)', 
         *   'rgba(255,0,0,0.6)',
         *   'rgba(255,0,0,0.4)'];
         * </pre>
         *
         * There is a set of predefined palettes in the @see:Palettes class that you can use, for example:
         * <pre>
         * chart.palette = wijmo.chart.Palettes.coral;
         * </pre>
         */
        get palette(): string[] {
            return this._palette;
        }
        set palette(value: string[]) {
            if (value != this._palette) {
                this._palette = asArray(value);
                this.invalidate();
            }
        }

        /**
         * Gets or sets the plot margin in pixels.
         *
         * The plot margin represents the area between the edges of the control
         * and the plot area.
         *
         * By default, this value is calculated automatically based on the space 
         * required by the axis labels, but you can override it if you want
         * to control the precise position of the plot area within the control
         * (for example, when aligning multiple chart controls on a page).
         *
         * You may set this property to a numeric value or to a CSS-style
         * margin specification. For example:
         *
         * <pre>
         * // set the plot margin to 20 pixels on all sides
         * chart.plotMargin = 20;
         * // set the plot margin for top, right, bottom, left sides
         * chart.plotMargin = '10 15 20 25';
         * // set the plot margin for top/bottom (10px) and left/right (20px)
         * chart.plotMargin = '10 20';
         * </pre>
         */
        get plotMargin(): any {
            return this._plotMargin;
        }
        set plotMargin(value: any) {
            if (value != this._plotMargin) {
                this._plotMargin = value;
                this.invalidate();
            }
        }

        /**
         * Gets the chart legend.
         */
        get legend(): Legend {
            return this._legend;
        }

        /**
         * Gets or sets the text displayed in the chart header.
         */
        get header(): string {
            return this._header;
        }
        set header(value: string) {
            if (value != this._header) {
                this._header = asString(value, true);
                this.invalidate();
            }
        }

        /**
         * Gets or sets the text displayed in the chart footer.
         */
        get footer(): string {
            return this._footer;
        }
        set footer(value: string) {
            if (value != this._footer) {
                this._footer = asString(value, true);
                this.invalidate();
            }
        }

        /**
         * Gets or sets the style of the chart header.
         */
        get headerStyle(): any {
            return this._headerStyle;
        }
        set headerStyle(value: any) {
            if (value != this._headerStyle) {
                this._headerStyle = value;
                this.invalidate();
            }
        }

        /**
         * Gets or sets the style of the chart footer.
         */
        get footerStyle(): any {
            return this._footerStyle;
        }
        set footerStyle(value: any) {
            if (value != this._footerStyle) {
                this._footerStyle = value;
                this.invalidate();
            }
        }

        /**
         * Gets or sets an enumerated value indicating whether or what is 
         * selected when the user clicks the chart.
         */
        get selectionMode(): SelectionMode {
            return this._selectionMode;
        }
        set selectionMode(value: SelectionMode) {
            if (value != this._selectionMode) {
                this._selectionMode = asEnum(value, SelectionMode);
                this.invalidate();
            }
        }

        /**
         * Gets or sets the item formatter function that allows you to customize 
         * the appearance of data points. See the Explorer sample's <a target="_blank" 
         * href="http://demos.componentone.com/wijmo/5/Angular/Explorer/Explorer/#/chart/itemFormatter">
         * Item Formatter</a> for a demonstration.
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
         * Occurs before the chart starts rendering data.
         */
        rendering = new Event();

        /**
         * Occurs after the chart finishes rendering.
         */
        rendered = new Event();

        /**
         * Raises the @see:rendered event.
         *
         * @param e The @see:RenderEventArgs object used to render the chart.
         */
        onRendered(e: RenderEventArgs) {
            this.rendered.raise(this, e);
        }

        /**
         * Raises the @see:rendering event.
         *
         * @param e The @see:RenderEventArgs object used to render the chart.
         */
        onRendering(e: RenderEventArgs) {
            this.rendering.raise(this, e);
        }


        /**
         * Refreshes the chart.
         *
         * @param fullUpdate A value indicating whether to update the control layout as well as the content.
         */
        refresh(fullUpdate = true) {
            if (!this.isUpdating) {
                this._refreshChart();
            }
        }

        //--------------------------------------------------------------------------
        // implementation

        // updates chart to sync with data source
        private _cvCollectionChanged(sender, e) {
            this._clearCachedValues();
            this._bindChart();
        }

        // updates selection to sync with data source
        private _cvCurrentChanged(sender, e) {
            if (this._notifyCurrentChanged) {
                this._bindChart();
            }
        }

        // IPalette 

        /**
        * Gets a color from the palette by index.
        *
        * @param index The index of the color in the palette.
        */
        _getColor(index: number): string {
            var palette = this._defPalette;
            if (this._palette != null && this._palette.length > 0) {
                palette = this._palette;
            }
            return palette[index % palette.length];
        }

        /**
         * Gets a lighter color from the palette by index.
         *
         * @param index The index of the color in the palette.
         */
        _getColorLight(index: number): string {
            var color = this._getColor(index),
                c = new Color(color);
            if (c != null) {
                c.a *= 0.7;
                color = c.toString();
            }
            return color;
        }

        // abstract

        // binds the chart to the current data source.
        _bindChart() {
            this._needBind = true;
            this.invalidate();
        }

        _clearCachedValues() {
        }

        _render(engine: IRenderEngine) {
        }

        _performBind() {
        }

        // render
        _refreshChart() {
            if (this._needBind) {
                this._needBind = false;
                this._performBind();
            }
            this._render(this._currentRenderEngine);
        }

        _drawTitle(engine: IRenderEngine, rect: Rect, title: string, style: any, isFooter: boolean): Rect {
            var lblClass = FlexChart._CSS_TITLE;
            var groupClass = isFooter ? FlexChart._CSS_FOOTER : FlexChart._CSS_HEADER;

            var tsz: Size = null;
            if (isFooter) {
                this._rectFooter = null;
            }
            else {
                this._rectHeader = null;
            }

            if (title != null) {
                var fontSize = null;
                var fg = null;
                var fontFamily = null;
                var halign = null;

                if (style) {
                    if (style.fontSize) {
                        fontSize = style.fontSize;
                    }
                    if (style.foreground) {
                        fg = style.foreground;
                    }
                    if (style.fill) {
                        fg = style.fill;
                    }
                    if (style.fontFamily) {
                        fontFamily = style.fontFamily;
                    }
                    if (style.halign) {
                        halign = style.halign;
                    }
                }

                engine.fontSize = fontSize;
                engine.fontFamily = fontFamily;

                tsz = engine.measureString(title, lblClass, groupClass, style);
                rect.height -= tsz.height;

                if (!fg) {
                    fg = FlexChart._FG;
                }

                engine.textFill = fg;
                if (isFooter) {

                    if (halign == 'left') {
                        FlexChart._renderText(engine, title, new Point(rect.left, rect.bottom), 0, 0, lblClass, groupClass, style);
                    } else if (halign == 'right') {
                        FlexChart._renderText(engine, title, new Point(rect.left + rect.width, rect.bottom), 2, 0, lblClass, groupClass, style);
                    } else { // default center
                        FlexChart._renderText(engine, title, new Point(rect.left + 0.5 * rect.width, rect.bottom), 1, 0, lblClass, groupClass, style);
                    }

                    this._rectFooter = new Rect(rect.left, rect.bottom, rect.width, tsz.height);
                } else {
                    this._rectHeader = new Rect(rect.left, rect.top, rect.width, tsz.height);

                    rect.top += tsz.height;
                    if (halign == 'left') {
                        FlexChart._renderText(engine, title, new Point(rect.left, 0), 0, 0, lblClass, groupClass, style);
                    } else if (halign == 'right') {
                        FlexChart._renderText(engine, title, new Point(rect.left + rect.width, 0), 2, 0, lblClass, groupClass, style);
                    } else { // default center
                        FlexChart._renderText(engine, title, new Point(rect.left + 0.5 * rect.width, 0), 1, 0, lblClass, groupClass, style);
                    }
                }

                engine.textFill = null;
                engine.fontSize = null;
                engine.fontFamily = null;
            }
            return rect;
        }

        // convert page coordinates to control 
        _toControl(pt: any, y?: number): Point {
            if (isNumber(pt) && isNumber(y)) { // accept hitTest(x, y) as well
                pt = new Point(pt, y);
            } else if (pt instanceof MouseEvent) {
                pt = new Point(pt.pageX, pt.pageY);
            }
            asType(pt, Point);

            // control coords
            var cpt = pt.clone();

            // jQuery
            // var host = $(this.hostElement);
            // var offset = host.offset();
            // cpt.x -= offset.left + parseInt(host.css('padding-left'));
            // cpt.y -= offset.top + parseInt(host.css('padding-top'));

            var offset = this._getHostOffset();
            cpt.x -= offset.x;
            cpt.y -= offset.y;

            var cstyle = this._getHostComputedStyle();
            if (cstyle) {
                var padLeft = parseInt(cstyle.paddingLeft.replace('px', ''));
                if (padLeft && !isNaN(padLeft)) {
                    cpt.x -= padLeft;
                }
                var padTop = parseInt(cstyle.paddingTop.replace('px', ''));
                if (padTop && !isNaN(padTop)) {
                    cpt.y -= padTop;
                }
            }

            return cpt;
        }

        _highlightItems(items, cls, selected: boolean) {
            if (selected) {
                for (var i = 0; i < items.length; i++) {
                    wijmo.addClass(items[i], cls);
                }
            } else {
                for (var i = 0; i < items.length; i++) {
                    wijmo.removeClass(items[i], cls);
                }
            }
        }

        _parseMargin(value: any): any {
            var margins = {};
            if (isNumber(value) && !isNaN(value)) {
                margins['top'] = margins['bottom'] = margins['left'] = margins['right'] = asNumber(value);
            } else if (isString(value)) {

                var s = asString(value);
                var ss = s.split(' ', 4);
                var top = NaN,
                    bottom = NaN,
                    left = NaN,
                    right = NaN;

                if (ss) {
                    if (ss.length == 4) {
                        top = parseFloat(ss[0]);
                        right = parseFloat(ss[1]);
                        bottom = parseFloat(ss[2]);
                        left = parseFloat(ss[3]);
                    } else if (ss.length == 2) {
                        top = bottom = parseFloat(ss[0]);
                        left = right = parseFloat(ss[1]);
                    } else if (ss.length == 1) {
                        top = bottom = left = right = parseFloat(ss[1]);
                    }

                    if (!isNaN(top)) {
                        margins['top'] = top;
                    }
                    if (!isNaN(bottom)) {
                        margins['bottom'] = bottom;
                    }
                    if (!isNaN(left)) {
                        margins['left'] = left;
                    }
                    if (!isNaN(right)) {
                        margins['right'] = right;
                    }
                }
            }

            return margins;
        }

        // shows an automatic tooltip
        _showToolTip(content, rect) {
            var self = this,
                showDelay = this._tooltip.showDelay;
            self._clearTimeouts();
            if (showDelay > 0) {
                self._toShow = setTimeout(function () {
                    self._tooltip.show(self.hostElement, content, rect);
                    if (self._tooltip.hideDelay > 0) {
                        self._toHide = setTimeout(function () {
                            self._tooltip.hide();
                        }, self._tooltip.hideDelay);
                    }
                }, showDelay);
            } else {
                self._tooltip.show(self.hostElement, content, rect);
                if (self._tooltip.hideDelay > 0) {
                    self._toHide = setTimeout(function () {
                        self._tooltip.hide();
                    }, self._tooltip.hideDelay);
                }
            }
        }

        // hides an automatic tooltip
        _hideToolTip() {
            this._clearTimeouts();
            this._tooltip.hide();
        }

        // clears the timeouts used to show and hide automatic tooltips
        private _clearTimeouts() {
            if (this._toShow) {
                clearTimeout(this._toShow);
                this._toShow = null;
            }
            if (this._toHide) {
                clearTimeout(this._toHide);
                this._toHide = null;
            }
        }

        _getHostOffset(): Point {
            var rect = getElementRect(this.hostElement);
            return new Point(rect.left, rect.top);
            
            /*var docElem, win,
                offset = new Point(),
                host = this.hostElement, 
                doc =  host && host.ownerDocument;

            if (!doc) {
                return offset;
            }

            docElem = doc.documentElement;

            // Make sure it's not a disconnected DOM node 
            //if (!jQuery.contains(docElem, elem)) { 
            //	return box; 
            //} 

            var box = host.getBoundingClientRect();
            win = doc.defaultView;// getWindow(doc);
            offset.y = box.top + win.pageYOffset - docElem.clientTop;
            offset.x = box.left + win.pageXOffset - docElem.clientLeft;

            return offset;*/
        }

        _getHostSize(): Size {
            var sz = new Size();

            var host = this.hostElement;

            var cstyle = this._getHostComputedStyle();
            var w = host.offsetWidth,
                h = host.offsetHeight;

            if (cstyle) {
                var padLeft = parseFloat(cstyle.paddingLeft.replace('px', ''));
                var padRight = parseFloat(cstyle.paddingRight.replace('px', ''));
                var padTop = parseFloat(cstyle.paddingTop.replace('px', ''));
                var padBottom = parseFloat(cstyle.paddingBottom.replace('px', ''));

                if (!isNaN(padLeft)) {
                    w -= padLeft;
                }
                if (!isNaN(padRight)) {
                    w -= padRight;
                }

                if (!isNaN(padTop)) {
                    h -= padTop;
                }

                if (!isNaN(padBottom)) {
                    h -= padBottom;
                }

                sz.width = w;
                sz.height = h;
            }

            return sz;
        }

        _getHostComputedStyle(): CSSStyleDeclaration {
            var host = this.hostElement;
            if (host && host.ownerDocument && host.ownerDocument.defaultView) {
                return host.ownerDocument.defaultView.getComputedStyle(this.hostElement);
            }
            return null;
        }

        _find(elem: SVGElement, names: string[]): any[]{
            var found = [];

            for (var i = 0; i < elem.childElementCount; i++) {
                var child = elem.childNodes.item(i);
                if (names.indexOf(child.nodeName) >= 0) {
                    found.push(child);
                } else {
                    var items = this._find(<SVGElement>child, names);
                    if (items.length > 0) {
                        for(var j = 0; j < items.length; j++)
                            found.push(items[j]);
                    }
                }
            }

            return found;
        }
    }

    export interface _IHitArea {
        contains(pt: Point): boolean;
        distance(pt: Point): number;
        tag: any;
    }

    export class _KeyWords {
        private _keys = {};

        constructor() {
            this._keys['seriesName'] = null;
            this._keys['pointIndex'] = null;
            this._keys['x'] = null;
            this._keys['y'] = null;
            this._keys['value'] = null;
            this._keys['name'] = null;
        }

        replace(s: string, ht: HitTestInfo): string {
            var kw = this;
            return wijmo.format(s,
                {// empty data - own get/format function
                },
                function (data, name, fmt, val) {
                    return kw.getValue(name, ht, fmt);
                });
        }

        getValue(key: string, ht: HitTestInfo, fmt?: string): string {
            if (key == 'seriesName' && ht.series) {
                return ht.series.name;
            }
            if (key == 'pointIndex' && ht.pointIndex !== null) {
                return ht.pointIndex.toFixed();
            }
            if (key == 'y' && ht.series) {
                return fmt ? Globalize.format(ht.y, fmt) : ht._yfmt;
            }
            if (key == 'x' && ht.series) {
                return fmt ? Globalize.format(ht.x, fmt) : ht._xfmt;
            }
            if (key == 'value') {
                return fmt ? Globalize.format(ht.value, fmt) : ht.value;
            }
            if (key == 'name') {
                return ht.name;
            }

            if (ht.item && ht.item[key])
                return fmt ? Globalize.format(ht.item[key], fmt) : ht.item[key];

            return '';
        }
    }

}


module wijmo.chart {
    'use strict';

    /**
     * The @see:FlexPie control provides pie and doughnut charts with selectable 
     * slices.
     *
     * To use the @see:FlexPie control, set the @see:itemsSource property to an 
     * array containing the data and use the @see:binding and @see:bindingName
     * properties to set the properties that contain the item values and names.
     */
    export class FlexPie extends FlexChartBase {
        private static _MARGIN = 4;

        private _bindingName: string;
        private _areas = [];
        private _keywords: _KeyWords = new _KeyWords();

        private _startAngle = 0;
        private _innerRadius = 0;
        private _offset = 0;
        private _reversed = false;
        private _isAnimated = false;

        private _selectedItemPosition = Position.None;
        private _selectedItemOffset = 0;

        private _pieGroup: SVGGElement;
        private _rotationAngle: number = 0;
        private _center: Point = new Point();
        private _radius: number;
        private _selectedOffset = new Point();
        private _selectedIndex = -1;
        private _angles = [];

        private _selectionAnimationID;

        private _lbl: PieDataLabel;

        _values: number[] = [];
        _labels: string[] = [];
        _pels = [];
        _sum: number = 0;

        /**
         * Initializes a new instance of the @see:FlexPie control.
         *
         * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
         * @param options A Javascript object containing initialization data for the control.
         */
        constructor(element: any, options?) {
            super(element, null, true); // invalidate on resize

            // add classes to host element
            this.applyTemplate('wj-control wj-flexchart', null, null);

            this._currentRenderEngine = new _SvgRenderEngine(this.hostElement);
            this._legend = new Legend(this);
            this._tooltip = new ChartTooltip();
            this._tooltip.content = '<b>{name}</b><br/>{value}';
            this._tooltip.showDelay = 0;

            this._lbl = new PieDataLabel();
            this._lbl._chart = this;

            var self = this;

            // tooltips
            // if (!isTouchDevice()) {
                this.hostElement.addEventListener('mousemove', function (evt) {
                    var tip = self._tooltip;
                    var tc = tip.content;
                    if (tc && !self.isTouching) {
                        var ht = self.hitTest(evt);
                        if (ht.distance <= tip.threshold) {
                            var content = self._getLabelContent(ht, self.tooltip.content);
                            self._showToolTip(content, new wijmo.Rect(evt.clientX, evt.clientY, 5, 5));
                        } else {
                            self._hideToolTip();
                        }
                    }
                });
            //}

            // selection
            this.hostElement.addEventListener('click', function (evt) {
                var showToolTip = true;

                if (self.selectionMode != SelectionMode.None) {
                    var ht = self.hitTest(evt);

                    var thershold = FlexChart._SELECTION_THRESHOLD;
                    if (self.tooltip && self.tooltip.threshold)
                        thershold = self.tooltip.threshold;
                    if (ht.distance <= thershold) {
                        if (ht.pointIndex != self._selectionIndex && self.selectedItemPosition != Position.None) {
                            showToolTip = false;
                        }
                        self._select(ht.pointIndex, true);
                    } else {
                        self._select(null);
                    }
                }

                if (showToolTip && self.isTouching /*isTouchDevice()*/) {
                    var tip = self._tooltip;
                    var tc = tip.content;
                    if (tc) {
                        var ht = self.hitTest(evt);
                        if (ht.distance <= tip.threshold) {
                            var content = self._getLabelContent(ht, self.tooltip.content);
                            self._showToolTip(content, new wijmo.Rect(evt.clientX, evt.clientY, 5, 5));
                        } else {
                            self._hideToolTip();
                        }
                    }
                }
            });


            // apply options only after chart is fully initialized
            this.initialize(options);

            // refresh control to show current state
            this.refresh();
        }

        /**
         * Gets or sets the name of the property that contains the name of the data item.
         */
        get bindingName(): string {
            return this._bindingName;
        }
        set bindingName(value: string) {
            if (value != this._bindingName) {
                this._bindingName = asString(value, true);
                this._bindChart();
            }
        }
        ///**
        // * Gets or sets various chart options.
        // *
        // * The following options are supported: innerRadius, startAngle, reversed, offset.
        // * 
        // */
        //get options(): any {
        //    return this._options;
        //}
        //set options(value: any) {
        //    if (value != this._options) {
        //        this._options = value;
        //        this.invalidate();
        //    }
        //}
        /**
         * Gets or sets the starting angle for the pie slices, in degrees.
         *
         * Angles are measured clockwise, starting at the 9 o'clock position.
         */
        get startAngle(): number {
            return this._startAngle;
        }
        set startAngle(value: number) {
            if (value != this._startAngle) {
                this._startAngle = asNumber(value, true);
                this.invalidate();
            }
        }
        /**
         * Gets or sets the offset of the slices from the pie center.
         *
         * The offset is measured as a fraction of the pie radius.
         */
        get offset(): number {
            return this._offset;
        }
        set offset(value: number) {
            if (value != this._offset) {
                this._offset = asNumber(value, true);
                this.invalidate();
            }
        }
        /**
         * Gets or sets the size of the pie's inner radius.
         *
         * The inner radius is measured as a fraction of the pie radius.
         *
         * The default value for this property is zero, which creates
         * a pie. Setting this property to values greater than zero 
         * creates pies with a hole in the middle, also known as 
         * doughnut charts.
         */
        get innerRadius(): number {
            return this._innerRadius;
        }
        set innerRadius(value: number) {
            if (value != this._innerRadius) {
                this._innerRadius = asNumber(value, true);
                this.invalidate();
            }
        }
        /**
         * Gets or sets whether angles are reversed (counter-clockwise).
         *
         * The default value is false, which causes angles to be measured in
         * the clockwise direction.
         */
        get reversed(): boolean {
            return this._reversed;
        }
        set reversed(value: boolean) {
            if (value != this._reversed) {
                this._reversed = asBoolean(value, true);
                this.invalidate();
            }
        }
        /**
         * Gets or sets the position of the selected slice.
         *
         * Setting this property to a value other than 'None' causes
         * the pie to rotate when an item is selected.
         *
         * Note that in order to select slices by clicking the chart, 
         * you must set the @see:selectionMode property to "Point".
         */
        get selectedItemPosition(): wijmo.chart.Position {
            return this._selectedItemPosition;
        }
        set selectedItemPosition(value: wijmo.chart.Position) {
            if (value != this._selectedItemPosition) {
                this._selectedItemPosition = asEnum(value, wijmo.chart.Position,  true);
                this.invalidate();
            }
        }
        /**
         * Gets or sets the offset of the selected slice from the pie center.
         *
         * Offsets are measured as a fraction of the pie radius.
         */
        get selectedItemOffset(): number {
            return this._selectedItemOffset;
        }
        set selectedItemOffset(value: number) {
            if (value != this._selectedItemOffset) {
                this._selectedItemOffset = asNumber(value, true);
                this.invalidate();
            }
        }
        /**
         * Gets or sets a value indicating whether to use animation when items are selected.
         *
         * See also the @see:selectedItemPosition and @see:selectionMode
         * properties.
         */
        get isAnimated(): boolean {
            return this._isAnimated;
        }
        set isAnimated(value: boolean) {
            if (value != this._isAnimated) {
                this._isAnimated = value;
                //this.invalidate();
            }
        }
        /**
         * Gets the chart's @see:Tooltip.
         */
        get tooltip(): ChartTooltip {
            return this._tooltip;
        }

        /**
         * Gets the point data label. 
         */
        get dataLabel(): PieDataLabel {
            return this._lbl;
        }

        /**
         * Gets a @see:HitTestInfo object with information about the specified point.
         *
         * @param pt The point to investigate, in window coordinates.
         * @param y The Y coordinate of the point (if the first parameter is a number).
         * @return A HitTestInfo object containing information about the point.
         */
        hitTest(pt: any, y?: number): HitTestInfo {

            // control coords
            var cpt = this._toControl(pt, y);
            var hti = new HitTestInfo(this, cpt);
            var si: number = null;
            if (FlexChart._contains(this._rectHeader, cpt)) {
                hti._chartElement = ChartElement.Header;
            } else if (FlexChart._contains(this._rectFooter, cpt)) {
                hti._chartElement = ChartElement.Footer;
            } else if (FlexChart._contains(this._rectLegend, cpt)) {
                hti._chartElement = ChartElement.Legend;
                si = this.legend._hitTest(cpt);
                if (si !== null && si >= 0 && si < this._areas.length) {
                    hti._pointIndex = si;
                }
            } else if (FlexChart._contains(this._rectChart, cpt)) {
                var len = this._areas.length;

                for (var i = 0; i < len; i++) {
                var pt1 = cpt.clone();
                if (this._rotationAngle != 0) {
                        var cx = this._center.x,
                            cy = this._center.y;
                        var dx = -cx + pt1.x;
                        var dy = -cy + pt1.y;
                    var r = Math.sqrt(dx * dx + dy * dy);
                    var a = Math.atan2(dy, dx) - this._rotationAngle * Math.PI / 180;

                        if (i == this._selectedIndex) {
                            r -= this._radius * this.selectedItemOffset;
                        }

                        pt1.x = cx + r * Math.cos(a);
                        pt1.y = cy + r * Math.sin(a);
                }

                    var area = <_IHitArea>this._areas[i];

                    if (area.contains(pt1)) {
                        hti._pointIndex = area.tag;
                        hti._dist = 0;
                        break;
                    }
                }
                hti._chartElement = ChartElement.ChartArea;
            }
            else {
                hti._chartElement = ChartElement.None;
            }
            return hti;
        }

        // binds the chart to the current data source.
        _performBind() {
            this._sum = 0;
            this._values = [];
            this._labels = [];

            if (this._cv) {
                this._selectionIndex = this._cv.currentPosition;
                var items = this._cv.items;
                if (items) {
                    var len = items.length;
                    for (var i = 0; i < len; i++) {
                        var item = items[i];
                        if (this.binding) {
                            item = item[this.binding];
                        }

                        var val = 0;

                        if (isNumber(item) && !isNaN(val) && isFinite(val)) {
                            val = asNumber(item);
                        }
                        else {
                            if (item) {
                                val = parseFloat(item.toString());

                            }
                        }

                        if (!isNaN(val) && isFinite(val)) {
                            this._sum += Math.abs( val);
                            this._values.push(val);
                        }
                        else {
                            val = 0;
                            this._values.push(val);
                        }

                        if (this.bindingName && items[i]) {
                            var name = items[i][this.bindingName];
                            if (name) {
                                name = name.toString();
                            }
                            this._labels.push(name);
                        } else {
                            this._labels.push(val.toString());
                        }
                    }
                }
            }
        }

        _render(engine: IRenderEngine) {
            // cancelAnimationFrame(this._selectionAnimationID);
            if (this._selectionAnimationID) {
                clearInterval(this._selectionAnimationID);
            }

            var el = this.hostElement;

            //  jQuery
            // var w = $(el).width();//el.clientWidth - el.clientLeft;
            // var h = $(el).height(); //el.clientHeight - el.clientTop;

            var sz = this._getHostSize();
            var w = sz.width,
                h = sz.height;

            if (w == 0 || isNaN(w)) {
                w = FlexChart._WIDTH;
            }
            if (h == 0 || isNaN(h)) {
                h = FlexChart._HEIGHT;
            }
            var hostSz = new Size(w, h);
            engine.beginRender();

            if (w > 0 && h > 0) {
                engine.setViewportSize(w, h);
                this._areas = [];

                var legend = this.legend;
                var lsz: Size;
                var tsz: Size;
                var lpos: Point;
                var rect = new Rect(0, 0, w, h);

                this._rectChart = rect.clone();

                engine.startGroup(FlexChart._CSS_HEADER);
                rect = this._drawTitle(engine, rect, this.header, this.headerStyle, false);
                engine.endGroup();

                engine.startGroup(FlexChart._CSS_FOOTER);
                rect = this._drawTitle(engine, rect, this.footer, this.footerStyle, true);
                engine.endGroup();

                w = rect.width;
                h = rect.height;
                //if (w > h) {
                //    rect.width = h;
                //    rect.left += 0.5 * (w - h);
                //    w = h;
                //} else if (w < h) {
                //    rect.height = w;
                //    rect.top += 0.5 * (h - w);
                //    h = w;
                //}

                lsz = legend._getDesiredSize(engine);
                switch (legend.position) {
                    case Position.Right:
                        w -= lsz.width;
                        lpos = new Point(w, rect.top + 0.5 * (h - lsz.height));
                        break;
                    case Position.Left:
                        rect.left += lsz.width;
                        w -= lsz.width;
                        lpos = new Point(0, rect.top + 0.5 * (h - lsz.height));
                        break;
                    case Position.Top:
                        h -= lsz.height;
                        lpos = new Point(0.5 * (w - lsz.width), rect.top);
                        rect.top += lsz.height;
                        break;
                    case Position.Bottom:
                        h -= lsz.height;
                        lpos = new Point(0.5 * (w - lsz.width), rect.top + h);
                        break;
                }

                rect.width = w;
                rect.height = h;

                //

                //engine.startGroup(FlexChart._CSS_PLOT_AREA);
                //var prect = this._plotRect;
                //engine.fill = 'transparent';
                //engine.stroke = null;
                //engine.drawRect(prect.left, prect.top, prect.width, prect.height);
                ///engine.endGroup();

                this.onRendering( new RenderEventArgs(engine));

                this._pieGroup = engine.startGroup(null, null, true); // all series

                var margins = this._parseMargin(this.plotMargin);
                var hasOutLabels = this.dataLabel.content && this.dataLabel.position == PieLabelPosition.Outside;

                if (isNaN(margins.left)) {
                    margins.left = hasOutLabels ? 24 : FlexPie._MARGIN;
                }
                if (isNaN(margins.right)) {
                    margins.right = hasOutLabels ? 24 :FlexPie._MARGIN;
                }
                if (isNaN(margins.top)) {
                    margins.top = hasOutLabels ? 24 : FlexPie._MARGIN;
                }
                if (isNaN(margins.bottom)) {
                    margins.bottom = hasOutLabels ? 24 : FlexPie._MARGIN;
                }

                rect.top += margins.top;
                rect.height -= margins.top + margins.bottom;
                rect.left += margins.left;
                rect.width -= margins.left + margins.right;

                this._renderData(engine, rect, this._pieGroup);

                engine.endGroup();

                if (lsz) {
                    this._legendHost = engine.startGroup(FlexChart._CSS_LEGEND);
                    this._rectLegend = new Rect(lpos.x, lpos.y, lsz.width, lsz.height);
                    this.legend._render(engine, lpos);
                    engine.endGroup();
                } else {
                    this._legendHost = null;
                    this._rectLegend = null;
                }

                this._rotationAngle = 0;
                this._highlightCurrent();

                if (this.dataLabel.content && this.dataLabel.position!=PieLabelPosition.None) {
                    this._renderLabels(engine);
                }

                this.onRendered(new RenderEventArgs(engine));
            }

            engine.endRender();
        }

        _renderData(engine: IRenderEngine, rect:Rect, g:any) {
            this._pels = [];
            this._angles = [];
            //engine.strokeWidth = 2;

            var len = this._values.length;
            var sum = this._sum;

            var startAngle = this.startAngle + 180, // start from 9 o'clock
                reversed = this.reversed == true,
                innerRadius = this.innerRadius,
                offset = this.offset,
                offsets = null;

            if (sum > 0) {
                var angle = startAngle * Math.PI / 180,
                    cx0 = rect.left + 0.5 * rect.width,
                    cy0 = rect.top + 0.5 * rect.height,
                    r = Math.min(0.5 * rect.width, 0.5 * rect.height);
                    
                this._center.x = cx0;
                this._center.y = cy0;

                var maxoff = Math.max(offset, this.selectedItemOffset);
                if (maxoff > 0) {
                    r = r / (1 + maxoff);
                    offset = offset * r;
                }
                this._radius = r;
                var irad = innerRadius * r;

                var selectedAngle = 0;

                for (var i = 0; i < len; i++) {
                    var cx = cx0;
                    var cy = cy0;

                    engine.fill = this._getColorLight(i);
                    engine.stroke = this._getColor(i);

                    var val = Math.abs( this._values[i]);
                    var sweep = Math.abs(val - sum) < 1E-10 ? 2*Math.PI : 2 * Math.PI * val / sum;
                    var pel = engine.startGroup();

                    var currentOffset = offset;

                    var currentAngle = reversed ? angle - 0.5 * sweep : angle + 0.5 * sweep;
                    if (i == this._cv.currentPosition) {
                            selectedAngle = currentAngle;
                    }
                    this._angles.push(currentAngle);

                    if (offsets && i<offsets.length) {
                        currentOffset = offsets[i];
                    }

                    if (currentOffset > 0) {
                        var a = angle + 0.5 * sweep;
                        if (reversed) {
                            a = -a;
                        }
                        cx += offset * Math.cos(currentAngle);
                        cy += offset * Math.sin(currentAngle);
                    }

                    if (sweep >= 2 * Math.PI) {
                        cx = cx0; cy = cy0;
                    }

                    if (this.itemFormatter) {
                        var hti = new HitTestInfo(this, new Point(cx + r * Math.cos(currentAngle), cy + r * Math.sin(currentAngle)));
                        hti._chartElement = ChartElement.SeriesSymbol;
                        hti._pointIndex = i;

                        this.itemFormatter(engine, hti, () => {
                            this._drawSilce(engine, i, reversed, cx, cy, r, irad, angle, sweep);
                        });
                    } else {
                        this._drawSilce(engine, i, reversed, cx, cy, r, irad, angle, sweep);
                    }

                    if (reversed) {
                        angle -= sweep;
                    } else {
                        angle += sweep;
                    }

                    engine.endGroup();
                    this._pels.push(pel);
                }

                this._highlightCurrent();
            }
        }

        _renderLabels(engine: IRenderEngine) {
            var len = this._areas.length;
            var lbl = this.dataLabel;
            var pos = lbl.position;
            var marg = 2;
            var lcss = 'wj-data-label';
            var bcss = 'wj-data-label-border';
            var da = this._rotationAngle;
            engine.stroke = 'null';
            engine.fill = 'transparent';
            engine.strokeWidth = 1;

            for (var i = 0; i < len; i++) {
                var seg = <_ISegment>this._areas[i];
                if (seg) {
                    var r = seg.radius;

                    var a = (seg.langle + da);

                    var ha = 1,
                        va = 1;
                    if (pos == PieLabelPosition.Center) {
                        r *= 0.5;
                    } else {
                        a = _Math.clampAngle(a);
                        if ( a <= -170 || a >= 170) {
                            ha = 2; va = 1;
                        } else if (a >= -100 && a <= -80) {
                            ha = 1; va = 2;
                        } else if (a >= -10 && a <= -10) {
                            ha = 0; va = 1;
                        } else if (a >= 80 && a <= 100) {
                            ha = 1; va = 0;
                        } else if (-180 < a && a < -90) {
                            ha = 2; va = 2;
                        } else if (-90 <= a && a < 0) {
                            ha = 0; va = 2;
                        } else if (0 < a && a < 90) {
                            ha = 0; va = 0;
                        } else if ( 90 < a && a < 180) {
                            ha = 2; va = 0;
                        }

                        if (pos == PieLabelPosition.Inside) {
                            ha = 2 - ha; va = 2 - va;
                        }
                    }

                    a *= Math.PI / 180;
                    var dx = 0,
                        dy = 0,
                        off = 0;
                    if (i == this._selectedIndex && this.selectedItemOffset>0) {
                        off = this.selectedItemOffset;
                    } else {
                        off = this.offset;
                    }
                        if (off > 0) {
                            dx = Math.cos(a) * off * this._radius;
                            dy = Math.sin(a) * off * this._radius;
                        }

                    var pt = new Point(this._center.x + dx + r * Math.cos(a),
                        this._center.y + dy + r * Math.sin(a));

                    if (lbl.border && pos != PieLabelPosition.Center) {
                        if (ha == 0)
                            pt.x += marg;
                        else if (ha == 2)
                            pt.x -= marg;
                        if (va == 0)
                            pt.y += marg;
                        else if (va == 2)
                            pt.y -= marg;
                    }

                    var hti = new HitTestInfo(this, pt);
                    hti._pointIndex = i;
                    var content = this._getLabelContent(hti, lbl.content); 

                    if (content) {
                        var lr = FlexChart._renderText(engine, content, pt, ha, va, lcss);

                        if (lbl.border) {
                            engine.drawRect(lr.left - marg, lr.top - marg, lr.width + 2 * marg, lr.height + 2 * marg, bcss);
                        }
                    }
                }
            }

        }

        _drawSilce(engine: IRenderEngine, i:number, reversed: boolean, cx:number, cy:number, r:number, irad:number, angle:number, sweep:number) {
            var area;
            if (reversed) {
                if (irad > 0) {
                    if (sweep != 0) {
                    engine.drawDonutSegment(cx, cy, r, irad, angle - sweep, sweep);
                    }

                    area = new _DonutSegment(new Point(cx, cy), r, irad, angle - sweep, sweep);
                    area.tag = i;
                    this._areas.push(area);
                } else {
                    if (sweep != 0) {
                    engine.drawPieSegment(cx, cy, r, angle - sweep, sweep);
                    }

                    area = new _PieSegment(new Point(cx, cy), r, angle - sweep, sweep);
                    area.tag = i;
                    this._areas.push(area);
                }
            }
            else {
                if (irad > 0) {
                    if (sweep != 0) {
                    engine.drawDonutSegment(cx, cy, r, irad, angle, sweep);
                    }

                    area = new _DonutSegment(new Point(cx, cy), r, irad, angle, sweep);
                    area.tag = i;
                    this._areas.push(area);
                } else {
                    if (sweep != 0) {
                    engine.drawPieSegment(cx, cy, r, angle, sweep);
                    }

                    area = new _PieSegment(new Point(cx, cy), r, angle, sweep);
                    area.tag = i;
                    this._areas.push(area);
                }
                angle += sweep;
            }
        }

        _measureLegendItem(engine: IRenderEngine, name:string): Size {
            var sz = new Size();
            sz.width = Series._LEGEND_ITEM_WIDTH;
            sz.height = Series._LEGEND_ITEM_HEIGHT;
            if (name) {
                var tsz = engine.measureString(name, FlexChart._CSS_LABEL);
                sz.width += tsz.width;
                if (sz.height < tsz.height) {
                    sz.height = tsz.height;
                }
            };
            sz.width += 3 * Series._LEGEND_ITEM_MARGIN;
            sz.height += 2 * Series._LEGEND_ITEM_MARGIN;
            return sz;
        }

        _drawLegendItem(engine: IRenderEngine, rect: Rect, i:number, name:string ) {
            engine.strokeWidth = 1;

            var marg = Series._LEGEND_ITEM_MARGIN;

            var fill = null;
            var stroke = null;

            if (fill === null)
                fill = this._getColorLight(i);
            if (stroke === null)
                stroke = this._getColor(i);

            engine.fill = fill;
            engine.stroke = stroke;

            var yc = rect.top + 0.5 * rect.height;

            var wsym = Series._LEGEND_ITEM_WIDTH;
            var hsym = Series._LEGEND_ITEM_HEIGHT;
            engine.drawRect(rect.left + marg, yc - 0.5 * hsym, wsym, hsym, null);//, this.style);

            if (name) {
                FlexChart._renderText(engine, name, new Point(rect.left + hsym + 2 * marg, yc), 0, 1, FlexChart._CSS_LABEL);
            }
        }

        //---------------------------------------------------------------------
        // tooltips

        private _getLabelContent(ht: HitTestInfo, content:any): string {
            if (isString(content)) {
                return this._keywords.replace(content, ht);
            } else if (isFunction(content)) {
                return content(ht);
            }

            return null;
        }

        //---------------------------------------------------------------------
        // selection

        private _select(pointIndex: number, animate:boolean=false) {
            this._highlight( false, this._selectionIndex);
            this._selectionIndex = pointIndex;
            this._highlight(true, this._selectionIndex, animate);

            if (this.selectionMode == SelectionMode.Point) {
                    var cv = this._cv;
                    if (cv) {
                        this._notifyCurrentChanged = false;
                        cv.moveCurrentToPosition(pointIndex);
                        this._notifyCurrentChanged = true;
                    }
            }

            if (!this.isAnimated && (this.selectedItemOffset > 0 || this.selectedItemPosition != Position.None)) {
                this.invalidate();
            }
        }

        private _highlightCurrent() {
            if (this.selectionMode != SelectionMode.None) {
                var pointIndex = -1;
                var cv = this._cv;

                    if (cv) {
                        pointIndex = cv.currentPosition;
                    }

                    this._highlight(true, pointIndex);
            }
        }

        private _highlight(selected: boolean, pointIndex: number, animate:boolean=false) {
            if (this.selectionMode == SelectionMode.Point && pointIndex!==undefined && pointIndex!==null && pointIndex>=0) {
                var gs = this._pels[pointIndex];
                // var hs = $(gs);

                if (selected) {
                    gs.parentNode.appendChild(gs);

                    //this._highlightItems(hs.find('path'), FlexChart._CSS_SELECTION, selected);
                    //var ells = hs.find('ellipse');
                    // this._highlightItems(ells, FlexChart._CSS_SELECTION, selected);
                    var ells = this._find(gs, ['ellipse']);
                    this._highlightItems(this._find(gs, ['path', 'ellipse']), FlexChart._CSS_SELECTION, selected);

                    var selectedAngle = this._angles[pointIndex];
                    if (this.selectedItemPosition != Position.None && selectedAngle != 0) {
                        var angle = 0;
                        if (this.selectedItemPosition == Position.Left) {
                            angle = 180;
                        } else if (this.selectedItemPosition == Position.Top) {
                            angle = -90;
                        } else if (this.selectedItemPosition == Position.Bottom) {
                            angle = 90;
                        }

                        var targetAngle = angle * Math.PI / 180 - selectedAngle;// - this._rotationAngle;
                        //this._rotationAngle = targetAngle;
                        targetAngle *= 180 / Math.PI;

                        if (animate && this.isAnimated) {
                            this._animateSelectionAngle(targetAngle, 0.5);
                        } else {
                            this._rotationAngle = targetAngle;
                            this._pieGroup.transform.baseVal.getItem(0).setRotate(targetAngle, this._center.x, this._center.y);
                        }
                    }

                    var off = this.selectedItemOffset;
                    if (off > 0 && ells.length == 0) {
                        var x = this._selectedOffset.x = Math.cos(selectedAngle) * off * this._radius;
                        var y = this._selectedOffset.y = Math.sin(selectedAngle) * off * this._radius;
                        
                        //hs.attr('transform', 'translate(' + x.toFixed() + ',' + y.toFixed() + ')');
                        gs.setAttribute('transform', 'translate(' + x.toFixed() + ',' + y.toFixed() + ')');
                    }
                    this._selectedIndex = pointIndex;

                } else {
                    gs.parentNode.insertBefore(gs, gs.parentNode.childNodes.item(pointIndex));

                    // jQuery
                    // hs.attr('transform', null);
                    // this._highlightItems(hs.find('path'), FlexChart._CSS_SELECTION, selected);
                    // this._highlightItems(hs.find('ellipse'), FlexChart._CSS_SELECTION, selected);

                    gs.removeAttribute('transform');
                    this._highlightItems(this._find(gs, ['path', 'ellipse']), FlexChart._CSS_SELECTION, selected);

                    if (this._selectedIndex == pointIndex) {
                        this._selectedIndex = -1;
                    }
                }
            }
        }

        _animateSelectionAngle(target:number, duration:number) {
            var source = _Math.clampAngle(this._rotationAngle);
            target = _Math.clampAngle(target);

            /*var delta = (target - source) / (60 * duration);
            this._selectionAnimationID = requestAnimationFrame(doAnim);
            var self = this;

            function doAnim() {

                source += delta;

                if ( Math.abs(target-source) < Math.abs(delta)) {
                   self._rotationAngle = source = target;
                }

                self._pieGroup.transform.baseVal.getItem(0).setRotate(source, self._center.x, self._center.y);

                if (target == source) {
                    cancelAnimationFrame(self._selectionAnimationID);
                } else {
                    self._selectionAnimationID = requestAnimationFrame(doAnim);
                }
            }*/

            var delta = (target - source);
            var self = this;
            var start = source;
            var group = self._pieGroup;

            if (self._selectionAnimationID) {
                clearInterval(this._selectionAnimationID);
            }

            this._selectionAnimationID = wijmo.animate(function (pct) {
                if (group == self._pieGroup) {
                    self._rotationAngle = source = start + delta * pct;
                    self._pieGroup.transform.baseVal.getItem(0).setRotate(source, self._center.x, self._center.y);
                    if (pct == 1) {
                        clearInterval(self._selectionAnimationID);
                    }
                    if (pct > 0.99) {
                        if (self.selectedItemOffset > 0 || self.selectedItemPosition != Position.None) {
                            self.invalidate();
                        }
                    }
                }
            }, duration * 1000);
        }
    }

    class _Math {

        // degrees [-180, +180]  
        static clampAngle(angle: number) {
            var a = (angle + 180) % 360 - 180;
            if (a < -180) {
                a += 360;
            }
            return a;
        }
    }

    interface _ISegment {
        center: Point;
        radius: number;
        langle: number;
    }

    class _PieSegment implements _IHitArea, _ISegment {
        private _center: Point;
        private _angle: number;
        private _sweep: number;
        private _radius: number;
        private _radius2: number;
        private _isFull: boolean = false;

        constructor(center: Point, radius: number, angle: number, sweep: number) {
            this._center = center;
            this._radius = radius;
            if (sweep >= 2 * Math.PI) {
                this._isFull = true;
            }
            this._sweep = 0.5 * sweep * 180 / Math.PI;
            this._angle = _Math.clampAngle( angle * 180 / Math.PI + this._sweep);
            this._radius2 = radius * radius;
        }

        contains(pt: Point): boolean {
            var dx = pt.x - this._center.x;
            var dy = pt.y - this._center.y;
            var r2 = dx * dx + dy * dy;

            if (r2 <= this._radius2) {
                var a = Math.atan2(dy, dx) * 180 / Math.PI;
                var delta = _Math.clampAngle(this._angle - a);
                if (this._isFull || Math.abs(delta) <= this._sweep) {
                    return true;
                }
            }
            return false; 
        }

        distance(pt: Point): number {
            if (this.contains(pt)) {
                return 0;
            }

            return undefined;
        }

        get center(): Point {
            return this._center;
        }

        get radius(): number {
            return this._radius;
        }

        get langle(): number {
            return this._angle;
        }

        tag: any;
    }

    class _DonutSegment implements _IHitArea, _ISegment {
        private _center: Point;
        private _angle: number;
        private _sweep: number;
        private _radius: number;
        private _radius2: number;
        private _iradius: number;
        private _iradius2: number;
        private _isFull: boolean = false;

        constructor(center: Point, radius: number, innerRadius:number, angle: number, sweep: number) {
            this._center = center;
            this._radius = radius;
            this._iradius = innerRadius;
            if (sweep >= 2 * Math.PI) {
                this._isFull = true;
            }
            this._sweep = 0.5 * sweep * 180 / Math.PI;
            this._angle = _Math.clampAngle(angle * 180 / Math.PI + this._sweep);
            this._radius2 = radius * radius;
            this._iradius2 = innerRadius * innerRadius;
        }

        contains(pt: Point): boolean {
            var dx = pt.x - this._center.x;
            var dy = pt.y - this._center.y;
            var r2 = dx * dx + dy * dy;

            if (r2 >= this._iradius2 && r2 <= this._radius2) {
                var a = Math.atan2(dy, dx) * 180 / Math.PI;
                var delta = _Math.clampAngle(this._angle - a);
                if (this._isFull || Math.abs(delta) <= this._sweep) {
                    return true;
                }
            }
            return false;
        }

        distance(pt: Point): number {
            if (this.contains(pt)) {
                return 0;
            }

            return undefined;
        }

        get center(): Point {
            return this._center;
        }

        get radius(): number {
            return this._radius;
        }

        get langle(): number {
            return this._angle;
        }

        tag: any;
    }

}


module wijmo.chart {
    'use strict';

    /**
     * Specifies whether and how to stack the chart's data values.
     */
    export enum Stacking {
        /** No stacking. Each series object is plotted independently. */
        None,
        /** Stacked charts show how each value contributes to the total. */
        Stacked,
        /** 100% stacked charts show how each value contributes to the total with the relative size of 
         * each series representing its contribution to the total. */
        Stacked100pc
    }
    /**
     * Specifies what is selected when the user clicks the chart.
     */
    export enum SelectionMode {
        /** Select neither series nor data points when the user clicks the chart. */
        None,
        /** Select the whole @see:Series when the user clicks it on the chart. */
        Series,
        /** Select the data point when the user clicks it on the chart. Since Line, Area, Spline, 
         * and SplineArea charts do not render individual data points, nothing is selected with this 
         * setting on those chart types. */
        Point
    };

    /**
     * The core charting control for @see:FlexChart.
     *
     */
    export class FlexChartCore extends FlexChartBase {
        static _CSS_AXIS_X = 'wj-axis-x';
        static _CSS_AXIS_Y = 'wj-axis-y';

        static _CSS_LINE = 'wj-line';
        static _CSS_GRIDLINE = 'wj-gridline';
        static _CSS_TICK = 'wj-tick';

        static _CSS_GRIDLINE_MINOR = 'wj-gridline-minor';
        static _CSS_TICK_MINOR = 'wj-tick-minor';

        static _CSS_LABEL = 'wj-label';

        static _CSS_LEGEND = 'wj-legend';
        static _CSS_HEADER = 'wj-header';
        static _CSS_FOOTER = 'wj-footer';

        static _CSS_TITLE = 'wj-title';

        static _CSS_SELECTION = 'wj-state-selected';
        static _CSS_PLOT_AREA = 'wj-plot-area';

        static _FG = '#666';

        // property storage
        private _series = new wijmo.collections.ObservableArray();
        private _axes = new AxisCollection();
        private _axisX = new Axis(Position.Bottom);
        private _axisY = new Axis(Position.Left);
        private _selection: SeriesBase;
        private _interpolateNulls = false;
        private _legendToggle = false;
        private _symbolSize = 10;

        private _dataInfo = new _DataInfo();
        _plotRect: Rect;

        private __barPlotter = null;
        private __linePlotter = null;
        private __areaPlotter = null;
        private __bubblePlotter = null;
        private __financePlotter = null;
        private _plotters = [];

        private _bindingX: string;
        private _rotated = false;
        _stacking = Stacking.None;
        private _lbl: DataLabel;

        _xlabels: string[] = [];
        _xvals: number[] = [];
        _xDataType: DataType;

        private _hitTester: _HitTester;

        private _keywords: _KeyWords;

        /**
         * Initializes a new instance of the @see:FlexChart control.
         *
         * @param element The DOM element that will host the control, or a selector for the host element (e.g. '#theCtrl').
         * @param options A JavaScript object containing initialization data for the control.
         */
        constructor(element: any, options?) {
            super(element, null, true); // invalidate on resize

            // add classes to host element
            this.applyTemplate('wj-control wj-flexchart', null, null);

            // handle changes to chartSeries array
            var self = this;
            self._series.collectionChanged.addHandler(function () {

                // check that chartSeries array contains Series objects
                var arr = self._series;
                for (var i = 0; i < arr.length; i++) {
                    var cs = <SeriesBase>tryCast(arr[i], wijmo.chart.SeriesBase);
                    if (!cs) {
                        throw 'chartSeries array must contain SeriesBase objects.';
                    }
                    cs._chart = self;
                    if (cs.axisX) {
                        cs.axisX._chart = self;
                    }
                    if (cs.axisY) {
                        cs.axisY._chart = self;
                    }
                }

                // refresh chart to show the change
                self.refresh();
            });

            this._currentRenderEngine = new _SvgRenderEngine(this.hostElement);
            this._hitTester = new _HitTester(this);
            this._legend = new Legend(this);
            this._tooltip = new ChartTooltip();
            this._tooltip.showDelay = 0;

            this._lbl = new DataLabel();
            this._lbl._chart = this;

            var ax: any = this._axisX;
            var ay: any = this._axisY;

            // default style
            this._axisX.majorGrid = false;
            this._axisX.name = 'axisX';
            this._axisY.majorGrid = true;
            this._axisY.majorTickMarks = TickMark.None;
            this._axisY.name = 'axisY';

            ax._chart = this;
            ay._chart = this;

            this._axes.push(ax);
            this._axes.push(ay);

            self._axes.collectionChanged.addHandler(function () {

                var arr = self._axes;
                for (var i = 0; i < arr.length; i++) {
                    var axis = tryCast(arr[i], wijmo.chart.Axis);
                    if (!axis) {
                        throw 'axes array must contain Axis objects.';
                    }
                    axis._chart = self;
                }

                // refresh chart to show the change
                self.refresh();
            });


            this._keywords = new _KeyWords();

            //if (isTouchDevice()) {
            this.hostElement.addEventListener('click', function (evt) {
                var tip = self._tooltip;
                var tc = tip.content;
                if (tc && self.isTouching) {
                    var ht = self.hitTest(evt);
                    if (ht.distance <= tip.threshold) {
                        var content = self._getLabelContent(ht, self._tooltip.content);
                        self._showToolTip(content, new wijmo.Rect(evt.clientX, evt.clientY, 5, 5));
                    } else {
                        self._hideToolTip();
                    }
                }
            });
            //} else {
            this.hostElement.addEventListener('mousemove', function (evt) {
                var tip = self._tooltip;
                var tc = tip.content;
                if (tc && !self.isTouching) {
                    var ht = self.hitTest(evt);
                    if (ht.distance <= tip.threshold) {
                        var content = self._getLabelContent(ht, self._tooltip.content);
                        self._showToolTip(content, new wijmo.Rect(evt.clientX, evt.clientY, 5, 5));
                    } else {
                        self._hideToolTip();
                    }
                }
            });
            //}

            this.hostElement.addEventListener('click', function (evt) {
                if (self.selectionMode != SelectionMode.None) {
                    var ht = self._hitTestData(evt);

                    var thershold = FlexChart._SELECTION_THRESHOLD;
                    if (self.tooltip && self.tooltip.threshold)
                        thershold = self.tooltip.threshold;
                    if (ht.distance <= thershold && ht.series) {
                        self._select(ht.series, ht.pointIndex);
                    } else {
                        if (self.selectionMode == SelectionMode.Series) {
                            ht = self.hitTest(evt);
                            if (ht.chartElement == ChartElement.Legend && ht.series) {
                                self._select(ht.series, null);
                            } else {
                                self._select(null, null);
                            }
                        }
                        else {
                            self._select(null, null);
                        }
                    }
                }

                if (self.legendToggle === true) {
                    ht = self.hitTest(evt);
                    if (ht.chartElement == ChartElement.Legend && ht.series) {
                        if (ht.series.visibility == SeriesVisibility.Legend) {
                            ht.series.visibility = SeriesVisibility.Visible;
                        }
                        else if (ht.series.visibility == SeriesVisibility.Visible) {
                            ht.series.visibility = SeriesVisibility.Legend;
                        }
                    }
                }
            });

            // apply options only after chart is fully initialized
            this.initialize(options);
        }

        //--------------------------------------------------------------------------
        // ** object model

        /**
         * Gets the collection of @see:Series objects.
         */
        get series(): wijmo.collections.ObservableArray {
            return this._series;
        }

        /**
         * Gets the collection of @see:Axis objects.
         */
        get axes(): wijmo.collections.ObservableArray {
            return this._axes;
        }

        /**
         * Gets the main X axis.
         */
        get axisX(): Axis {
            return this._axisX;
        }

        /**
         * Gets the main Y axis.
         */
        get axisY(): Axis {
            return this._axisY;
        }

        /**
         * Gets or sets the name of the property that contains the X data values.
         */
        get bindingX(): string {
            return this._bindingX;
        }
        set bindingX(value: string) {
            if (value != this._bindingX) {
                this._bindingX = asString(value, true);
                this._bindChart();
            }
        }

        /**
         * Gets or sets the size of the symbols used for all Series objects in this @see:FlexChart.
         * 
         * This property may be overridden by the symbolSize property on each @see:Series object.
         */
        get symbolSize(): number {
            return this._symbolSize;
        }
        set symbolSize(value: number) {
            if (value != this._symbolSize) {
                this._symbolSize = asNumber(value, false, true);
                this.invalidate();
            }
        }

        /**
         * Gets or sets whether to interpolate null values in the data.
         *
         * If true, the chart interpolates the value of any missing data 
         * based on neighboring points. If false, it leaves a break in 
         * lines and areas at the points with null values.
         */
        get interpolateNulls(): boolean {
            return this._interpolateNulls;
        }
        set interpolateNulls(value: boolean) {
            if (value != this._interpolateNulls) {
                this._interpolateNulls = asBoolean(value);
                this.invalidate();
            }
        }

        /**
         * Gets or sets a value indicating whether clicking legend items toggles the
         * series visibility in the chart.
         */
        get legendToggle(): boolean {
            return this._legendToggle;
        }
        set legendToggle(value: boolean) {
            if (value != this._legendToggle) {
                this._legendToggle = asBoolean(value);
            }
        }


        /**
         * Gets or sets a value indicating whether to flip the axes so X is vertical and Y horizontal.
         */
        get rotated(): boolean {
            return this._rotated;
        }
        set rotated(value: boolean) {
            if (value != this._rotated) {
                this._rotated = asBoolean(value);
                this.invalidate();
            }
        }

        /**
         * Gets or sets various chart options.
         *
         * The following options are supported:
         * 
         * <b>bubble.maxSize</b>: Specifies the maximum size 
         * of symbols in the Bubble chart. The default value is 30 pixels.
         *
         * <b>bubble.minSize</b>: Specifies the minimum size 
         * of symbols in the Bubble chart. The default value is 5 pixels.
         *
         * <pre>chart.options = { 
         *   bubble: { minSize: 5, maxSize: 30 }
         * }</pre>
         *
         * <b>groupWidth</b>: Specifies the group width for Column charts, or the group height
         * for Bar charts. The group width can be specified in pixels or percent of 
         * available space. The default value is '70%'.
         *
         * <pre>chart.options = {
         *   groupWidth : 50; // 50 pixels
         * }
         * chart.options = {
         *   groupWidth : '100%'; // 100% pixels
         * }</pre>
         */
        get options(): any {
            return this._options;
        }
        set options(value: any) {
            if (value != this._options) {
                this._options = value;
                this.invalidate();
            }
        }

        /**
         * Gets the chart @see:Tooltip object.
         *
         * Tooltip content is generated using a template that may contain any of the following
         * parameters:
         *
         * <ul>
         *  <li><b>seriesName</b>: The name of the series that contains the chart element.</li>
         *  <li><b>pointIndex</b>: The index of the chart element within the series.</li>
         *  <li><b>x</b>: The <b>x</b> value of the chart element.</li>
         *  <li><b>y</b>: The <b>y</b> value of the chart element.</li>
         * </ul>
         *
         * To modify the template, assign a new value to the tooltip's content property.
         * For example:
         * <pre>
         * chart.tooltip.content = '&lt;b&gt;{seriesName}&lt;/b&gt; ' +
         *    '&lt;img src="resources/{x}.png"/&gt;&lt;br/&gt;{y}';
         * </pre>
         *
         * You can disable chart tooltips by setting the template to an empty string, or by
         * setting the content to a function that takes a @see:HitTestInfo object as a parameter.
         *
         * You can also use the @see:tooltip property to customize tooltip parameters such 
         * as @see:showDelay and @see:hideDelay:
         * <pre>
         * chart.tooltip.showDelay = 1000;
         * </pre>
         * See @see:ChartTooltip properties for other properties that you can set. 
         */
        get tooltip(): ChartTooltip {
            return this._tooltip;
        }

        /**
         * Gets the point data label. 
         */
        get dataLabel(): DataLabel {
            return this._lbl;
        }

        /**
         * Gets or sets the selected chart series.
         */
        get selection(): SeriesBase {
            return this._selection;
        }
        set selection(value: SeriesBase) {
            if (value != this._selection) {
                this._selection = asType(value, SeriesBase, true);
                this.invalidate();
            }
        }

        /**
         * Occurs after the selection changes, whether programmatically
         * or when the user clicks the chart. This is useful, for example,
         * when you want to update details in a textbox showing the current
         * selection.
         */
        selectionChanged = new Event();

        /**
         * Raises the @see:selectionChanged event.
         */
        onSelectionChanged(e?: EventArgs) {
            this.selectionChanged.raise(this, e);
        }

        /**
         * Occurs when the series visibility changes, for example when the legendToggle 
         * property is set to true and the user clicks the legend.
        */
        seriesVisibilityChanged = new Event();

        /**
         * Raises the @see:seriesVisibilityChanged event.
         *
         * @param e The @see:SeriesEventArgs object that contains the event data.
         */
        onSeriesVisibilityChanged(e: SeriesEventArgs) {
            this.seriesVisibilityChanged.raise(this, e);
        }

        /**
         * Gets a @see:HitTestInfo object with information about the specified point.
         *
         * @param pt The point to investigate, in window coordinates.
         * @param y The Y coordinate of the point (if the first parameter is a number).
         * @return A HitTestInfo object with information about the point.
         */
        hitTest(pt: any, y?: number): HitTestInfo {
            // control coords
            var cpt = this._toControl(pt, y);

            var hti = new HitTestInfo(this, cpt);

            var si: number = null;

            if (FlexChart._contains(this._rectHeader, cpt)) {
                hti._chartElement = ChartElement.Header;
            } else if (FlexChart._contains(this._rectFooter, cpt)) {
                hti._chartElement = ChartElement.Footer;
            } else if (FlexChart._contains(this._rectLegend, cpt)) {
                hti._chartElement = ChartElement.Legend;

                si = this.legend._hitTest(cpt);
                if (si !== null && si >= 0 && si < this.series.length) {
                    hti._series = this.series[si];
                }
            } else if (FlexChart._contains(this._rectChart, cpt)) {
                var hr = this._hitTester.hitTest(cpt);

                // custom series hit test
                var ht: HitTestInfo = null;
                var htsi = null;
                for (var i = this.series.length - 1; i >= 0; i--) {
                    if (this.series[i].hitTest !== Series.prototype.hitTest) {
                        var hts = this.series[i].hitTest(pt);
                        if (hts) {
                            if (!ht || hts.distance < ht.distance) {
                                ht = hts;
                                htsi = i;
                            }
                            if (hts.distance === 0) {
                                break;
                            }
                        }
                    }
                }

                if (hr && hr.area) {
                    if (ht && ht.distance < hr.distance) {
                        hti = ht;
                    } else if (ht && ht.distance == hr.distance && htsi > hr.area.tag.seriesIndex) {
                        hti = ht;
                    } else {
                        hti._pointIndex = hr.area.tag.pointIndex;
                        si = hr.area.tag.seriesIndex;
                        if (si !== null && si >= 0 && si < this.series.length)
                            hti._series = this.series[si];

                        hti._dist = hr.distance;
                    }
                } else if (ht) {
                    hti = ht;
                }

                if (FlexChart._contains(this.axisX._axrect, cpt)) {
                    hti._chartElement = ChartElement.AxisX;
                } else if (FlexChart._contains(this.axisY._axrect, cpt)) {
                    hti._chartElement = ChartElement.AxisY;
                } else if (FlexChart._contains(this._plotRect, cpt)) {
                    hti._chartElement = ChartElement.PlotArea;
                } else if (FlexChart._contains(this._rectChart, cpt)) {
                    hti._chartElement = ChartElement.ChartArea;
                }
            }
            else {
                hti._chartElement = ChartElement.None;
            }

            return hti;
        }

        /**
         * Converts a @see:Point from control coordinates to chart data coordinates.
         *
         * @param pt The point to convert, in control coordinates.
         * @param y The Y coordinate of the point (if the first parameter is a number).
         * @return The point in chart data coordinates.
         */
        pointToData(pt: any, y?: number): Point {
            if (isNumber(pt) && isNumber(y)) { // accept hitTest(x, y) as well
                pt = new Point(pt, y);
            } if (pt instanceof MouseEvent) {
                pt = new Point(pt.pageX, pt.pageY);
                pt = this._toControl(pt);
            }
            else {
                pt = pt.clone();
            }

            pt.x = this.axisX.convertBack(pt.x);
            pt.y = this.axisY.convertBack(pt.y);
            return pt;
        }

        /**
         * Converts a @see:Point from data coordinates to control coordinates.
         *
         * @param pt @see:Point in data coordinates, or X coordinate of a point in data coordinates.
         * @param y Y coordinate of the point (if the first parameter is a number).
         * @return The @see:Point in control coordinates.
         */
        dataToPoint(pt: any, y?: number): Point {
            if (isNumber(pt) && isNumber(y)) { // accept (x, y) as well
                pt = new Point(pt, y);
            }
            asType(pt, Point);
            var cpt = pt.clone();
            cpt.x = this.axisX.convert(cpt.x);
            cpt.y = this.axisY.convert(cpt.y);

            return cpt;
        }

        //--------------------------------------------------------------------------
        // implementation

        // method used in JSON-style initialization
        _copy(key: string, value: any): boolean {
            if (key == 'series') {
                var arr = asArray(value);
                for (var i = 0; i < arr.length; i++) {
                    var s = this._createSeries();
                    wijmo.copy(s, arr[i]);
                    this.series.push(s);
                }
                return true;
            }
            return false;
        }

        _createSeries(): SeriesBase {
            return new Series();
        }

        _clearCachedValues() {
            for (var i = 0; i < this._series.length; i++) {
                var series = <Series>this._series[i];
                if (series.itemsSource == null)
                    series._clearValues();
            }
        }

        _performBind() {
            this._xDataType = null;
            this._xlabels.splice(0);
            this._xvals.splice(0);
            if (this._cv) {
                var items = this._cv.items;
                if (items) {
                    var len = items.length;
                    for (var i = 0; i < len; i++) {
                        var item = items[i];
                        if (this._bindingX) {
                            var x = item[this._bindingX];
                            if (isNumber(x)) {
                                this._xvals.push(asNumber(x));
                                this._xDataType = DataType.Number;
                            } else if (isDate(x)) {
                                this._xvals.push(FlexChart._toOADate(asDate(x)));
                                this._xDataType = DataType.Date;
                            }
                            this._xlabels.push(item[this._bindingX]);
                        }
                    }
                    if (this._xvals.length == len) {
                        this._xlabels.splice(0);
                    } else {
                        this._xvals.splice(0);
                    }
                }
            }
        }

        _hitTestSeries(pt: Point, seriesIndex: number): HitTestInfo {
            // control coords
            //var cpt = pt.clone();
            //var host = this.hostElement;

            //cpt.x -= host.offsetLeft;
            //cpt.y -= host.offsetTop;
            var cpt = this._toControl(pt);

            var hti = new HitTestInfo(this, cpt);
            var si = seriesIndex;
            var hr = this._hitTester.hitTestSeries(cpt, seriesIndex);

            if (hr && hr.area) {
                hti._pointIndex = hr.area.tag.pointIndex;
                si = hr.area.tag.seriesIndex;
                if (si !== null && si >= 0 && si < this.series.length)
                    hti._series = this.series[si];
                hti._chartElement = ChartElement.PlotArea;
                hti._dist = hr.distance;
            }

            return hti;
        }

        // hitTest including lines
        _hitTestData(pt: any): HitTestInfo {
            var cpt = this._toControl(pt);
            var hti = new HitTestInfo(this, cpt);
            var hr = this._hitTester.hitTest(cpt, true);

            if (hr && hr.area) {
                hti._pointIndex = hr.area.tag.pointIndex;
                var si = hr.area.tag.seriesIndex;
                if (si !== null && si >= 0 && si < this.series.length)
                    hti._series = this.series[si];

                hti._dist = hr.distance;
            }

            return hti;
        }

        /* private _hitTestLines(hti: HitTestInfo): HitTestInfo {
            if (hti.series) {
                var pi = hti.pointIndex;
                var p0 = hti.series._indexToPoint(pi);

                // jQuery
                //var offset = $(this.hostElement).offset();
                var offset = this._getHostOffset();

                p0 = this.dataToPoint(p0);
                p0.x -= offset.x;
                p0.y -= offset.y;

                var d1 = null,
                    d2 = null;
                var p1 = hti.series._indexToPoint(pi - 1);
                var p2 = hti.series._indexToPoint(pi + 1);
                if (p1) {
                    p1 = this.dataToPoint(p1);
                    p1.x -= offset.x;
                    p1.y -= offset.y;
                    d1 = FlexChart._dist2(p0, p1);
                }
                if (p2) {
                    p2 = this.dataToPoint(p2);
                    p2.x -= offset.x;
                    p2.y -= offset.y;
                    d2 = FlexChart._dist2(p0, p2);
                }

                var pt = hti.point.clone();
                var host = this.hostElement;
                pt.x -= host.offsetLeft;
                pt.y -= host.offsetTop;

                if (d1 && d2) {
                    if (d1 < d2) {
                        hti._dist = FlexChart._dist(pt, p0, p1);
                    }
                    else {
                        hti._dist = FlexChart._dist(pt, p0, p2);
                    }
                } else if (d1) {
                    hti._dist = FlexChart._dist(pt, p0, p1);
                } else if (d2) {
                    hti._dist = FlexChart._dist(pt, p0, p2);
                }
            }

            return hti;
        }*/

        private static _dist2(p1: Point, p2: Point): number {
            var dx = p1.x - p2.x;
            var dy = p1.y - p2.y;
            return dx * dx + dy * dy;
        }

        // line p1-p2 to point p0

        /*static _dist(p0: Point, p1: Point, p2: Point): number {
            var dx = p2.x - p1.x;
            var dy = p2.y - p1.y;
            return Math.sqrt(Math.abs(dy * p0.x - dx * p0.y - p1.x * p2.y + p2.x * p1.y) / Math.sqrt(dx * dx + dy * dy));
        }*/

        static _dist(p0: Point, p1: Point, p2: Point): number {
            return Math.sqrt(FlexChart._distToSegmentSquared(p0, p1, p2));
        }

        static _distToSegmentSquared(p: Point, v: Point, w: Point): number {
            var l2 = FlexChart._dist2(v, w);
            if (l2 == 0)
                return FlexChart._dist2(p, v);
            var t = ((p.x - v.x) * (w.x - v.x) + (p.y - v.y) * (w.y - v.y)) / l2;
            if (t < 0)
                return FlexChart._dist2(p, v);
            if (t > 1)
                return FlexChart._dist2(p, w);
            return FlexChart._dist2(p, new Point(v.x + t * (w.x - v.x), v.y + t * (w.y - v.y)));
        }

        private _isRotated(): boolean {
            return this._getChartType() == ChartType.Bar ? !this._rotated : this._rotated;
        }

        _plotrectId: string;

        _getChartType(): ChartType {
            return null;
        }

        _render(engine: IRenderEngine) {
            var el = this.hostElement;

            //  jQuery
            // var w = $(el).width();//el.clientWidth - el.clientLeft;
            // var h = $(el).height(); //el.clientHeight - el.clientTop;

            var sz = this._getHostSize();
            var w = sz.width,
                h = sz.height;

            if (w == 0 || isNaN(w)) {
                w = FlexChart._WIDTH;
            }
            if (h == 0 || isNaN(h)) {
                h = FlexChart._HEIGHT;
            }
            //

            var hostSz = new Size(w, h);
            engine.beginRender();

            if (w > 0 && h > 0) {
                engine.setViewportSize(w, h);
                this._hitTester.clear();

                var legend = this.legend;
                var lsz: Size;
                var tsz: Size;
                var lpos: Point;
                var rect = new Rect(0, 0, w, h);

                this._rectChart = rect.clone();

                engine.startGroup(FlexChart._CSS_HEADER);
                rect = this._drawTitle(engine, rect, this.header, this.headerStyle, false);
                engine.endGroup();

                engine.startGroup(FlexChart._CSS_FOOTER);
                rect = this._drawTitle(engine, rect, this.footer, this.footerStyle, true);
                engine.endGroup();

                w = rect.width;
                h = rect.height;

                lsz = legend._getDesiredSize(engine);
                switch (legend.position) {
                    case Position.Right:
                        w -= lsz.width;
                        lpos = new Point(w, rect.top + 0.5 * (h - lsz.height));
                        break;
                    case Position.Left:
                        rect.left += lsz.width;
                        w -= lsz.width;
                        lpos = new Point(0, rect.top + 0.5 * (h - lsz.height));
                        break;
                    case Position.Top:
                        h -= lsz.height;
                        lpos = new Point(0.5 * (w - lsz.width), rect.top);
                        rect.top += lsz.height;
                        break;
                    case Position.Bottom:
                        h -= lsz.height;
                        lpos = new Point(0.5 * (w - lsz.width), rect.top + h);
                        break;
                }

                rect.width = w;
                rect.height = h;

                //
                var plotter = this._getPlotter(null);
                plotter.stacking = this._stacking;
                var isRotated = this._isRotated();

                this._dataInfo.analyse(this._series, isRotated, plotter.stacking, this._xvals.length > 0 ? this._xvals : null,
                    this.axisX.logBase > 0, this.axisY.logBase > 0);

                var rect0 = plotter.adjustLimits(this._dataInfo, rect.clone());

                if (isRotated) {
                    var ydt = this._dataInfo.getDataTypeX();
                    if (!ydt) {
                        ydt = this._xDataType;
                    }
                    this.axisX._updateActualLimits(this._dataInfo.getDataTypeY(), rect0.left, rect0.right);
                    this.axisY._updateActualLimits(ydt, rect0.top, rect0.bottom, this._xlabels, this._xvals);
                }
                else {
                    var xdt = this._dataInfo.getDataTypeX();
                    if (!xdt) {
                        xdt = this._xDataType;
                    }
                    this.axisX._updateActualLimits(xdt, rect0.left, rect0.right, this._xlabels, this._xvals);
                    this.axisY._updateActualLimits(this._dataInfo.getDataTypeY(), rect0.top, rect0.bottom);
                }

                var axes = this._getAxes();
                this._updateAuxAxes(axes, isRotated);

                //

                this._layout(rect, hostSz, engine);

                engine.startGroup(FlexChart._CSS_PLOT_AREA);
                var prect = this._plotRect;
                engine.fill = 'transparent';
                engine.stroke = null;
                engine.drawRect(prect.left, prect.top, prect.width, prect.height);
                engine.endGroup();

                var len = this._series.length;

                this._clearPlotters();
                var groups = {};

                for (var i = 0; i < len; i++) {
                    var series = this._series[i];
                    var ay = series._getAxisY();

                    if (ay) {
                        var axid = ay._uniqueId;
                        if (!groups[axid]) {
                            groups[axid] = { count: 1, index: 0 };
                        } else {
                            groups[axid].count += 1;
                        }
                    }

                    var plotter = this._getPlotter(series);
                    plotter.seriesCount++;
                }

                this.onRendering(new RenderEventArgs(engine));

                for (var i = 0; i < axes.length; i++) {
                    var ax: Axis = axes[i];
                    if (ax.axisType == AxisType.X) {
                        engine.startGroup(FlexChart._CSS_AXIS_X);
                    } else {
                        engine.startGroup(FlexChart._CSS_AXIS_Y);
                    }

                    ax._render(engine);
                    engine.endGroup();
                }

                engine.startGroup(); // all series

                this._plotrectId = 'plotRect' + (1000000 * Math.random()).toFixed();

                engine.addClipRect(this._plotRect, this._plotrectId);

                for (var i = 0; i < len; i++) {
                    var series = this._series[i];
                    series._pointIndexes = [];
                    var plotter = this._getPlotter(series);
                    series._hostElement = engine.startGroup(series.cssClass, plotter.clipping ? this._plotrectId : null);
                    var vis = series.visibility;
                    var axisX = series.axisX;
                    var axisY = series.axisY;
                    if (!axisX) {
                        axisX = this.axisX;
                    }
                    if (!axisY) {
                        axisY = this.axisY;
                    }

                    if (vis == SeriesVisibility.Visible || vis == SeriesVisibility.Plot) {
                        var group = groups[axisY._uniqueId];
                        if (group) {
                            if (series.rendering.hasHandlers) {
                                series.onRendering(engine);
                            } else {
                                plotter.plotSeries(engine, axisX, axisY, series, this, group.index, group.count);
                            }
                            group.index++;
                        } else {
                            if (series.rendering.hasHandlers) {
                                series.onRendering(engine);
                            } else {

                                plotter.plotSeries(engine, axisX, axisY, series, this, plotter.seriesIndex, plotter.seriesCount);
                                plotter.seriesIndex++;
                            }
                        }
                    }
                    engine.endGroup();
                }
                engine.endGroup();

                if (this.dataLabel.content && this.dataLabel.position != LabelPosition.None) {
                    this._renderLabels(engine);
                }

                if (lsz) {
                    this._legendHost = engine.startGroup(FlexChart._CSS_LEGEND);
                    this._rectLegend = new Rect(lpos.x, lpos.y, lsz.width, lsz.height);
                    engine.textFill = FlexChart._FG;
                    this.legend._render(engine, lpos);
                    engine.textFill = null;
                    engine.endGroup();
                } else {
                    this._legendHost = null;
                    this._rectLegend = null;
                }

                this._highlightCurrent();
                this.onRendered(new RenderEventArgs(engine));
            }

            engine.endRender();
        }

        private _renderLabels(engine: IRenderEngine) {
            var lbl = this.dataLabel;
            var pos = lbl.position;
            var srs = this.series;
            var slen = srs.length;
            var bdr = lbl.border;
            engine.stroke = 'null';
            engine.fill = 'transparent';
            engine.strokeWidth = 1;
            var marg = 2;
            var lcss = 'wj-data-label';
            var bcss = 'wj-data-label-border';

            for (var i = 0; i < slen; i++) {
                var ser = srs[i];
                var smap = this._hitTester._map[i];
                if (smap) {
                    var len = smap.length;
                    for (var j = 0; j < len; j++) {
                        var dp = <_DataPoint>asType(smap[j].tag, _DataPoint, true);
                        if (dp) {
                            var ht = new HitTestInfo(this, pt);
                            ht._series = ser;
                            ht._pointIndex = dp.pointIndex;
                            var s = this._getLabelContent(ht, lbl.content);

                            var pt = this.dataToPoint(new Point(dp.dataX, dp.dataY));
                            var map = smap[j];
                            if (map instanceof _RectArea) {
                                var ra = <_RectArea>map;
                                if (this._isRotated())
                                    pt.y = ra.rect.top + 0.5 * ra.rect.height;
                                else
                                    pt.x = ra.rect.left + 0.5 * ra.rect.width;
                            }

                            if (!this._plotRect.contains(pt)) {
                                continue;
                            }

                            var lrct: Rect;
                            switch (pos) {
                                case LabelPosition.Top: {
                                    pt.y -= marg;
                                    lrct = FlexChart._renderText(engine, s, pt, 1, 2, lcss);
                                    break;
                                }
                                case LabelPosition.Bottom: {
                                    pt.y += marg;
                                    lrct = FlexChart._renderText(engine, s, pt, 1, 0, lcss);
                                    break;
                                }
                                case LabelPosition.Left: {
                                    pt.x -= marg;
                                    lrct = FlexChart._renderText(engine, s, pt, 2, 1, lcss);
                                    break;
                                }
                                case LabelPosition.Right: {
                                    pt.x += marg;
                                    lrct = FlexChart._renderText(engine, s, pt, 0, 1, lcss);
                                    break;
                                }
                                case LabelPosition.Center:
                                    lrct = FlexChart._renderText(engine, s, pt, 1, 1, lcss);
                                    break;
                            }

                            if (bdr && lrct) {
                                engine.drawRect(lrct.left - marg, lrct.top - marg, lrct.width + 2 * marg, lrct.height + 2 * marg, bcss);
                            }
                        }
                    }
                }
            }
        }

        private _getAxes(): Axis[] {
            var axes = [this.axisX, this.axisY];
            var len = this.series.length;
            for (var i = 0; i < len; i++) {
                var ser = <Series>this.series[i];
                var ax = ser.axisX;
                if (ax && axes.indexOf(ax) === -1) {
                    axes.push(ax);
                }
                var ay = ser.axisY;
                if (ay && axes.indexOf(ay) === -1) {
                    axes.push(ay);
                }
            }

            return axes;
        }

        private _clearPlotters() {
            var len = this._plotters.length;
            for (var i = 0; i < len; i++)
                this._plotters[i].clear();
        }

        _initPlotter(plotter: _IPlotter) {
            plotter.chart = this;
            plotter.dataInfo = this._dataInfo;
            plotter.hitTester = this._hitTester;
            this._plotters.push(plotter);
        }

        private get _barPlotter() {
            if (this.__barPlotter === null) {
                this.__barPlotter = new _BarPlotter();
                this._initPlotter(this.__barPlotter);
            }
            return this.__barPlotter;
        }

        private get _linePlotter() {
            if (this.__linePlotter === null) {
                this.__linePlotter = new _LinePlotter();
                this._initPlotter(this.__linePlotter);
            }
            return this.__linePlotter;
        }

        private get _areaPlotter() {
            if (this.__areaPlotter === null) {
                this.__areaPlotter = new _AreaPlotter();
                this._initPlotter(this.__areaPlotter);
            }
            return this.__areaPlotter;
        }

        private get _bubblePlotter() {
            if (this.__bubblePlotter === null) {
                this.__bubblePlotter = new _BubblePlotter();
                this._initPlotter(this.__bubblePlotter);
            }
            return this.__bubblePlotter;
        }

        private get _financePlotter() {
            if (this.__financePlotter === null) {
                this.__financePlotter = new _FinancePlotter();
                this._initPlotter(this.__financePlotter);
            }
            return this.__financePlotter;
        }

        _getPlotter(series: SeriesBase): _IPlotter {
            var chartType = this._getChartType();
            var isSeries = false;
            if (series){
                var stype = series._getChartType();
                if (stype !== undefined && stype != chartType) {
                    chartType = stype;
                    isSeries = true;
                }
            }

            var plotter: _IPlotter;
            switch (chartType) {
                case ChartType.Column:
                    plotter = this._barPlotter;
                    break;
                case ChartType.Bar:
                    this._barPlotter.rotated = !this._rotated;
                    plotter = this._barPlotter;
                    break;
                case ChartType.Line:
                    this._linePlotter.hasSymbols = false;
                    this._linePlotter.hasLines = true;
                    this._linePlotter.isSpline = false;
                    plotter = this._linePlotter;
                    break;
                case ChartType.Scatter:
                    this._linePlotter.hasSymbols = true;
                    this._linePlotter.hasLines = false;
                    this._linePlotter.isSpline = false;
                    plotter = this._linePlotter;
                    break;
                case ChartType.LineSymbols:
                    this._linePlotter.hasSymbols = true;
                    this._linePlotter.hasLines = true;
                    this._linePlotter.isSpline = false;
                    plotter = this._linePlotter;
                    break;
                case ChartType.Area:
                    this._areaPlotter.isSpline = false;
                    plotter = this._areaPlotter;
                    break;
                case ChartType.Bubble:
                    plotter = this._bubblePlotter;
                    break;
                case ChartType.Candlestick:
                    this._financePlotter.isCandle = true;
                    plotter = this._financePlotter;
                    break;
                case ChartType.HighLowOpenClose:
                    this._financePlotter.isCandle = false;
                    plotter = this._financePlotter;
                    break;
                case ChartType.Spline:
                    this._linePlotter.hasSymbols = false;
                    this._linePlotter.hasLines = true;
                    this._linePlotter.isSpline = true;
                    plotter = this._linePlotter;
                    break;
                case ChartType.SplineSymbols:
                    this._linePlotter.hasSymbols = true;
                    this._linePlotter.hasLines = true;
                    this._linePlotter.isSpline = true;
                    plotter = this._linePlotter;
                    break;
                case ChartType.SplineArea:
                    this._areaPlotter.isSpline = true;
                    plotter = this._areaPlotter;
                    break;
                default:
                    throw 'Invalid chart type.';
            }

            plotter.rotated = this._rotated;
            if (chartType == ChartType.Bar)
                plotter.rotated = !plotter.rotated;
            if (isSeries) {
                plotter.rotated = this._isRotated();
            }

            return plotter;
        }

        private _layout(rect: Rect, size: Size, engine: IRenderEngine) {
            var w = rect.width;
            var h = rect.height;
            var mxsz = new Size(w, 0.75 * h);
            var mysz = new Size(h, 0.75 * w);

            var left = 0, top = 0, right = w, bottom = h;
            var l0 = 0, t0 = 0, r0 = w, b0 = h;

            var axes = this._getAxes();

            for (var i = 0; i < axes.length; i++) {
                var ax: Axis = axes[i];
                var origin = ax.origin;

                if (ax.axisType == AxisType.X) {
                    var ah = ax._getHeight(engine);

                    if (ah > mxsz.height)
                        ah = mxsz.height;

                    ax._desiredSize = new Size(mxsz.width, ah);

                    var hasOrigin = ax._hasOrigin =
                        isNumber(origin) && origin > this.axisY.actualMin && origin < this.axisY.actualMax;

                    if (ax.position == Position.Bottom) {
                        left = Math.max(left, ax._annoSize.width * 0.5);
                        right = Math.min(right, w - ax._annoSize.width * 0.5);

                        if (hasOrigin) {
                            var yorigin = this._convertY(origin, t0, b0);
                            b0 = b0 - Math.max(0, (yorigin + ah) - b0);
                        } else {
                            b0 = b0 - ah;
                        }
                    } else if (ax.position == Position.Top) {
                        left = Math.max(left, ax._annoSize.width * 0.5);
                        right = Math.min(right, w - ax._annoSize.width * 0.5);

                        if (hasOrigin) {
                            var yorigin = this._convertY(origin, t0, b0);
                            t0 = t0 + Math.max(0, t0 - (yorigin - ah));
                        }
                        else {
                            t0 = t0 + ah;
                        }
                    }
                } else if (ax.axisType == AxisType.Y) {
                    var ah = ax._getHeight(engine);
                    if (ah > mysz.height) {
                        ah = mysz.height;
                    }
                    ax._desiredSize = new Size(mysz.width, ah);

                    var hasOrigin = ax._hasOrigin =
                        isNumber(origin) && origin > this.axisX.actualMin && origin < this.axisX.actualMax;

                    if (ax.position == Position.Left) {
                        top = Math.max(top, ax._annoSize.width * 0.5);
                        bottom = Math.min(bottom, h - ax._annoSize.width * 0.5);

                        if (hasOrigin) {
                            var xorigin = this._convertX(origin, l0, r0);
                            l0 += Math.max(0, l0 - (xorigin - ah));
                        } else {
                            l0 += ah;
                        }
                    } else if (ax.position == Position.Right) {
                        top = Math.max(top, ax._annoSize.width * 0.5);
                        bottom = Math.min(bottom, h - ax._annoSize.width * 0.5);

                        if (hasOrigin) {
                            var xorigin = this._convertX(origin, l0, r0);
                            r0 = r0 - Math.max(0, (xorigin + ah) - r0);
                        }
                        else {
                            r0 = r0 - ah;
                        }
                    }
                }
            }

            // todo: custom margins
            var margins = this._parseMargin(this.plotMargin);

            if (!isNaN(margins.left)) {
                left = l0 = margins.left;
            } else {
                left = l0 = Math.max(left, l0) + rect.left;
            }

            if (!isNaN(margins.right)) {
                right = r0 = size.width - margins.right;
            } else {
                right = r0 = Math.min(right, r0) + rect.left;
            }

            if (!isNaN(margins.top)) {
                top = t0 = margins.top;
            } else {
                top = t0 = Math.max(top, t0) + rect.top;
            }
            if (!isNaN(margins.bottom)) {
                bottom = b0 = size.height - margins.bottom;
            } else {
                bottom = b0 = Math.min(bottom, b0) + rect.top;
            }

            w = Math.max(1, right - left);
            h = Math.max(1, bottom - top);
            this._plotRect = new Rect(left, top, w, h);

            engine.stroke = null;
            //engine.setFill(this.plotFill);

            for (var i = 0; i < axes.length; i++) {
                var ax: Axis = axes[i];
                //ax._plot = _plot0;
                var origin = ax.origin;

                if (ax.axisType == AxisType.X) {
                    var axr: Rect;

                    if (!ax._hasOrigin) {
                        if (ax.position == Position.Bottom) {
                            axr = new Rect(left, b0, w, ax._desiredSize.height);
                            b0 += ax._desiredSize.height;
                        } else if (ax.position == Position.Top) {
                            axr = new Rect(left, t0 - ax._desiredSize.height, w, ax._desiredSize.height);
                            t0 -= ax._desiredSize.height;
                        }
                        else {
                            axr = new Rect(left, t0, w, 1);
                        }
                    } else {
                        var yorigin = this._convertY(origin, this._plotRect.top, this._plotRect.bottom);
                        if (ax.position == Position.Bottom) {
                            axr = new Rect(left, yorigin, w, ax._desiredSize.height);
                            b0 += Math.max(0, axr.bottom - this._plotRect.bottom);// ax.DesiredSize.Height;
                        } else if (ax.position == Position.Top) {
                            axr = new Rect(left, yorigin - ax._desiredSize.height, w, ax._desiredSize.height);
                            t0 -= Math.max(0, this._plotRect.top - axr.top); // ax.DesiredSize.Height;
                        }
                    }
                    ax._layout(axr, this._plotRect);
                    //ax.render(engine, axr, this.plotRect);
                } else if (ax.axisType == AxisType.Y) {
                    var ayr: Rect;

                    if (!ax._hasOrigin) {
                        if (ax.position == Position.Left) {
                            ayr = new Rect(l0 - ax._desiredSize.height, top, h, ax._desiredSize.height);
                            l0 -= ax._desiredSize.height;
                        }
                        else if (ax.position == Position.Right) {
                            ayr = new Rect(r0, top, h, ax._desiredSize.height);
                            r0 += ax._desiredSize.height;
                        }
                        else {
                            ayr = new Rect(l0, top, h, 1);
                        }
                    } else {
                        var xorigin = this._convertX(origin, this._plotRect.left, this._plotRect.right);

                        if (ax.position == Position.Left) {
                            ayr = new Rect(xorigin - ax._desiredSize.height, top, h, ax._desiredSize.height);
                            l0 -= ax._desiredSize.height;
                        }
                        else if (ax.position == Position.Right) {
                            ayr = new Rect(xorigin, top, h, ax._desiredSize.height);
                            r0 += ax._desiredSize.height;
                        }
                    }

                    ax._layout(ayr, this._plotRect);
                    //ax.render(engine, ayr, this.plotRect);
                }
            }
        }

        //---------------------------------------------------------------------

        private _convertX(x: number, left: number, right: number) {
            var ax = this.axisX;
            if (ax.reversed)
                return right - (right - left) * (x - ax.actualMin) / (ax.actualMax - ax.actualMin);
            else
                return left + (right - left) * (x - ax.actualMin) / (ax.actualMax - ax.actualMin);
        }

        private _convertY(y: number, top: number, bottom: number): number {
            var ay = this.axisY;
            if (ay.reversed)
                return top + (bottom - top) * (y - ay.actualMin) / (ay.actualMax - ay.actualMin);
            else
                return bottom - (bottom - top) * (y - ay.actualMin) / (ay.actualMax - ay.actualMin);
        }

        // tooltips

        private _getLabelContent(ht: HitTestInfo, content: any): string {
            //var tc = this._tooltip.content;
            if (isString(content)) {
                return this._keywords.replace(content, ht);
            } else if (isFunction(content)) {
                return content(ht);
            }

            return null;
        }


        //---------------------------------------------------------------------
        // selection
        private _select(newSelection: SeriesBase, pointIndex: number) {
            if (this._selection) {
                this._highlight(this._selection, false, this._selectionIndex);
            }
            this._selection = newSelection;
            this._selectionIndex = pointIndex;
            if (this._selection) {
                this._highlight(this._selection, true, this._selectionIndex);
            }

            if (this.selectionMode == SelectionMode.Point) {
                if (newSelection) {
                    var cv = newSelection.collectionView;
                    if (!cv) {
                        cv = this._cv;
                    }

                    if (cv) {
                        this._notifyCurrentChanged = false;
                        cv.moveCurrentToPosition(pointIndex);
                        this._notifyCurrentChanged = true;
                    }
                }
                else {
                    //
                }
            }

            this.onSelectionChanged();
        }

        private _highlightCurrent() {
            if (this.selectionMode != SelectionMode.None) {
                var selection = this._selection;
                var pointIndex = -1;
                if (selection) {
                    var cv = selection.collectionView;
                    if (!cv) {
                        cv = this._cv;
                    }

                    if (cv) {
                        pointIndex = cv.currentPosition;
                    }

                    this._highlight(selection, true, pointIndex);
                }
            }
        }

        private _highlight(series: SeriesBase, selected: boolean, pointIndex: number) {

            // check that the selection is a Series object (or null)
            series = asType(series, SeriesBase, true);

            // select the series or the point
            if (this.selectionMode == SelectionMode.Series) {
                var index = this.series.indexOf(series);
                var gs = series.hostElement;

                // jQuery
                // var hs = $(gs);
                // this._highlightItems(hs.find('rect'), FlexChart._CSS_SELECTION, selected);
                // this._highlightItems(hs.find('ellipse'), FlexChart._CSS_SELECTION, selected);
                // this._highlightItems(hs.find('polyline'), FlexChart._CSS_SELECTION, selected);
                // this._highlightItems(hs.find('polygon'), FlexChart._CSS_SELECTION, selected);
                // this._highlightItems(hs.find('line'), FlexChart._CSS_SELECTION, selected);

                if (selected) {
                    gs.parentNode.appendChild(gs);
                } else {
                    gs.parentNode.insertBefore(gs, gs.parentNode.childNodes.item(index));
                }

                var found = this._find(gs, ['rect', 'ellipse', 'polyline', 'polygon', 'line']);
                this._highlightItems(found, FlexChart._CSS_SELECTION, selected);

                if (series.legendElement) {
                    // jQuery
                    // var ls = $(series.legendElement);
                    // this._highlightItems(ls.find('rect'), FlexChart._CSS_SELECTION, selected);
                    // this._highlightItems(ls.find('ellipse'), FlexChart._CSS_SELECTION, selected);
                    // this._highlightItems(ls.find('line'), FlexChart._CSS_SELECTION, selected);

                    this._highlightItems(this._find(series.legendElement, ['rect', 'ellipse', 'line']), FlexChart._CSS_SELECTION, selected);
                }

            } else if (this.selectionMode == SelectionMode.Point) {
                var index = this.series.indexOf(series);
                var gs = series.hostElement;

                /* jQuery
                var hs = $(gs);

                if (selected) {
                    gs.parentNode.appendChild(gs);
                    var pel = $(series.getPlotElement(pointIndex));
                    if (pel.length) {
                        this._highlightItems(pel, FlexChart._CSS_SELECTION, selected);
                        this._highlightItems(pel.find('line'), FlexChart._CSS_SELECTION, selected);
                        this._highlightItems(pel.find('rect'), FlexChart._CSS_SELECTION, selected);
                    }
                } else {
                    gs.parentNode.insertBefore(gs, gs.parentNode.childNodes.item(index));

                    this._highlightItems(hs.find('rect'), FlexChart._CSS_SELECTION, selected);
                    this._highlightItems(hs.find('ellipse'), FlexChart._CSS_SELECTION, selected);
                    this._highlightItems(hs.find('line'), FlexChart._CSS_SELECTION, selected);
                }
                */

                if (selected) {
                    gs.parentNode.appendChild(gs);
                    var pel = series.getPlotElement(pointIndex);
                    if (pel) {
                        if (pel.nodeName != 'g') {
                            this._highlightItems([pel], FlexChart._CSS_SELECTION, selected);
                        }
                        var found = this._find(pel, ['line', 'rect', 'ellipse']);
                        this._highlightItems(found, FlexChart._CSS_SELECTION, selected);
                    }
                } else {
                    gs.parentNode.insertBefore(gs, gs.parentNode.childNodes.item(index));
                    var found = this._find(gs, ['rect', 'ellipse', 'line']);
                    this._highlightItems(found, FlexChart._CSS_SELECTION, selected);
                }

            }
        }

        // aux axes
        _updateAuxAxes(axes: Axis[], isRotated: boolean) {
            for (var i = 2; i < axes.length; i++) {
                var ax = axes[i];
                ax._chart = this;
                var slist = [];
                for (var iser = 0; iser < this.series.length; iser++) {
                    var ser = this.series[iser];
                    if (ser.axisX == ax || ser.axisY == ax) {
                        slist.push(ser);
                    }
                }
                var dataMin,
                    dataMax;
                for (var iser = 0; iser < slist.length; iser++) {
                    var rect = slist[iser]._getDataRect();
                    if (rect) {
                        if ((ax.axisType == AxisType.X && !isRotated) || (ax.axisType == AxisType.Y && isRotated)) {
                            if (dataMin === undefined || rect.left < dataMin) {
                                dataMin = rect.left;
                            }
                            if (dataMax === undefined || rect.right > dataMax) {
                                dataMax = rect.right;
                            }
                        } else {
                            if (dataMin === undefined || rect.top < dataMin) {
                                dataMin = rect.top;
                            }
                            if (dataMax === undefined || rect.bottom > dataMax) {
                                dataMax = rect.bottom;
                            }
                        }
                    }
                }

                axes[i]._updateActualLimits(DataType.Number, dataMin, dataMax);
            }

        }

        //---------------------------------------------------------------------
        // tools

        static _contains(rect: Rect, pt: Point): boolean {
            if (rect && pt) {
                return pt.x >= rect.left && pt.x <= rect.right && pt.y >= rect.top && pt.y <= rect.bottom;
            }

            return false;
        }

        static _intersects(rect1: Rect, rect2: Rect): boolean {
            if (rect1.left > rect2.right || rect1.right < rect2.left || rect1.top > rect2.bottom || rect1.bottom < rect2.top) {
                return false;
            }

            return true;
        }


        static _epoch = new Date(1899, 11, 30).getTime();
        static _msPerDay = 86400000;

        static _toOADate(date: Date): number {
            return (date.getTime() - FlexChart._epoch) / FlexChart._msPerDay;
        }

        static _fromOADate(val: number): Date {
            var dec = val - Math.floor(val);
            if (val < 0 && dec) {
                val = Math.floor(val) - dec;
            }
            return new Date(val * FlexChart._msPerDay + FlexChart._epoch);
        }

        static _renderText(engine: IRenderEngine, text: string, pos: Point, halign, valign, className?: string, groupName?: string, style?: any, test?: any): Rect {
            var sz = engine.measureString(text, className, groupName, style);
            var x = pos.x;
            var y = pos.y;

            switch (halign) {
                // center
                case 1:
                    x -= 0.5 * sz.width;
                    break;
                // right
                case 2:
                    x -= sz.width;
                    break;
            }
            switch (valign) {
                // center
                case 1:
                    y += 0.5 * sz.height;
                    break;
                // top
                case 0:
                    y += sz.height;
                    break;
            }

            var rect = new Rect(x, y - sz.height, sz.width, sz.height);
            if (test) {
                if (test(rect)) {
                    engine.drawString(text, new Point(x, y), className, style);
                    return rect;
                }
                else
                    return null;
            }
            else {
                engine.drawString(text, new Point(x, y), className, style);
                return rect;
            }
        }

        static _renderRotatedText(engine: IRenderEngine, text: string, pos: Point, halign, valign,
            center: Point, angle: number, className: string, style?: any) {
            var sz = engine.measureString(text, className, style);
            var x = pos.x;
            var y = pos.y;

            switch (halign) {
                case 1:
                    x -= 0.5 * sz.width;
                    break;
                case 2:
                    x -= sz.width;
                    break;
            }
            switch (valign) {
                case 1:
                    y += 0.5 * sz.height;
                    break;
                case 0:
                    y += sz.height;
                    break;
            }

            engine.drawStringRotated(text, new Point(x, y), center, angle, className, style);
        }
        //
    }

    /**
     * Analyzes chart data.
     */
    export class _DataInfo {
        private minY: number;
        private maxY: number;
        private minX: number;
        private maxX: number;
        private minXp: number;
        private minYp: number;

        private dataTypeX: DataType;
        private dataTypeY: DataType;

        private stackAbs: { [key: number]: number } = {};
        private _xvals: Array<number> = null;

        private dx: number;

        constructor() {
        }

        analyse(seriesList: any, isRotated: boolean, stacking: Stacking, xvals: Array<number>, logx: boolean, logy: boolean) {
            this.minY = NaN;
            this.maxY = NaN;
            this.minX = NaN;
            this.maxX = NaN;
            this.minXp = NaN;
            this.minYp = NaN;
            this.dx = 0;

            var stackPos: { [key: number]: number } = {};
            var stackNeg: { [key: number]: number } = {};
            var stackAbs: { [key: number]: number } = {};

            this.dataTypeX = null;
            this.dataTypeY = null;

            this._xvals = xvals;
            if (xvals != null) {
                var len = xvals.length;
                for (var i = 0; i < len; i++) {
                    var xval = xvals[i];
                    if (isNaN(this.minX) || this.minX > xval) {
                        this.minX = xval;
                    }
                    if (isNaN(this.maxX) || this.maxX < xval) {
                        this.maxX = xval;
                    }

                    if (xval > 0) {
                        if (isNaN(this.minXp) || this.minXp > xval) {
                            this.minXp = xval;
                        }
                    }

                    if (i > 0) {
                        var dx = Math.abs(xval - xvals[i - 1]);
                        if (!isNaN(dx) && (dx < this.dx || this.dx == 0)) {
                            this.dx = dx;
                        }
                    }
                }
            }

            for (var i = 0; i < seriesList.length; i++) {
                var series = <Series>seriesList[i];
                var custom = series.chartType !== undefined;
                var vis = series.visibility;
                if (vis == SeriesVisibility.Hidden || vis == SeriesVisibility.Legend) {
                    continue;
                }

                var dr = series.getDataRect();
                if (dr) {
                    if (isNaN(this.minX) || this.minX > dr.left) {
                        this.minX = dr.left;
                    }
                    if (isNaN(this.maxX) || this.maxX < dr.right) {
                        this.maxX = dr.right;
                    }

                    if (isNaN(this.minY) || this.minY > dr.top) {
                        this.minY = dr.top;
                    }
                    if (isNaN(this.maxY) || this.maxY < dr.bottom) {
                        this.maxY = dr.bottom;
                    }
                    continue;
                }

                var xvalues = null;
                if (isRotated) {
                    if (!series._isCustomAxisY()) {
                        xvalues = series.getValues(1);
                    }
                } else {
                    if (!series._isCustomAxisX()) {
                        xvalues = series.getValues(1);
                    }
                }

                if (xvalues) {
                    if (!this.dataTypeX) {
                        this.dataTypeX = series.getDataType(1);
                    }
                    for (var j = 0; j < xvalues.length; j++) {
                        var val = xvalues[j];
                        if (_DataInfo.isValid(val)) {
                            if (isNaN(this.minX) || this.minX > val) {
                                this.minX = val;
                            }
                            if (isNaN(this.maxX) || this.maxX < val) {
                                this.maxX = val;
                            }

                            if (j > 0) {
                                var dx = Math.abs(val - xvalues[j - 1]);
                                if (!isNaN(dx) && (dx < this.dx || this.dx == 0)) {
                                    this.dx = dx;
                                }
                            }
                        }
                    }
                }
                var values = null;
                if (isRotated) {
                    if (!series._isCustomAxisX()) {
                        values = series.getValues(0);
                    }
                } else {
                    if (!series._isCustomAxisY()) {
                        values = series.getValues(0);
                    }
                }

                if (values) {
                    if (!this.dataTypeY) {
                        this.dataTypeY = series.getDataType(0);
                    }
                    if (isNaN(this.minX)) {
                        this.minX = 0;
                    }

                    if (isNaN(this.maxX)) {
                        this.maxX = values.length - 1;
                    } else if (!xvalues) {
                        this.maxX = Math.max(this.maxX, values.length - 1);
                    }

                    for (var j = 0; j < values.length; j++) {
                        var val = values[j];
                        var xval = xvalues ? asNumber(xvalues[j], true) : (xvals ? asNumber(xvals[j], true) : j);
                        if (_DataInfo.isValid(val)) {
                            if (isNaN(this.minY) || this.minY > val) {
                                this.minY = val;
                            }
                            if (isNaN(this.maxY) || this.maxY < val) {
                                this.maxY = val;
                            }
                            if (!custom) {
                                if (val > 0) {
                                    if (isNaN(stackPos[xval])) {
                                        stackPos[xval] = val;
                                    } else {
                                        stackPos[xval] += val;
                                    }
                                    if (isNaN(this.minYp) || this.minYp > val) {
                                        this.minYp = val;
                                    }
                                } else {
                                    if (isNaN(stackNeg[xval])) {
                                        stackNeg[xval] = val;
                                    } else {
                                        stackNeg[xval] += val;
                                    }
                                }
                                if (isNaN(stackAbs[xval])) {
                                    stackAbs[xval] = Math.abs(val);
                                } else {
                                    stackAbs[xval] += Math.abs(val);
                                }
                            }
                        }
                    }
                }
            }

            if (stacking == Stacking.Stacked) {
                for (var key in stackPos) {
                    if (stackPos[key] > this.maxY) {
                        this.maxY = stackPos[key];
                    }
                }
                for (var key in stackNeg) {
                    if (stackNeg[key] < this.minY) {
                        this.minY = stackNeg[key];
                    }
                }
            } else if (stacking == Stacking.Stacked100pc) {
                this.minY = 0;
                this.maxY = 1;
                for (var key in stackAbs) {
                    var sum = stackAbs[key];
                    if (isFinite(sum) && sum != 0) {
                        var vpos = stackPos[key];
                        var vneg = stackNeg[key];
                        if (isFinite(vpos)) {
                            vpos = Math.min(vpos / sum, this.maxY);
                        }
                        if (isFinite(vneg)) {
                            vneg = Math.max(vneg / sum, this.minY);
                        }
                    }
                }
            }
            this.stackAbs = stackAbs;

            if (logx) {
                if (isRotated)
                    this.minY = isNaN(this.minYp) ? 1 : this.minYp;
                else
                    this.minX = isNaN(this.minXp) ? 1 : this.minXp;
            }
            if (logy) {
                if (isRotated)
                    this.minX = isNaN(this.minXp) ? 1 : this.minXp;
                else
                    this.minY = isNaN(this.minYp) ? 1 : this.minYp;
            }
        }

        getMinY(): number {
            return this.minY;
        }

        getMaxY(): number {
            return this.maxY;
        }

        getMinX(): number {
            return this.minX;
        }

        getMaxX(): number {
            return this.maxX;
        }

        getMinXp(): number {
            return this.minXp;
        }

        getMinYp(): number {
            return this.minYp;
        }

        getDeltaX(): number {
            return this.dx;
        }

        getDataTypeX(): DataType {
            return this.dataTypeX;
        }

        getDataTypeY(): DataType {
            return this.dataTypeY;
        }

        getStackedAbsSum(key: number) {
            var sum = this.stackAbs[key];
            return isFinite(sum) ? sum : 0;
        }

        getXVals(): Array<number> {
            return this._xvals;
        }

        static isValid(value: number): boolean {
            return isFinite(value);// && !isNaN(value);
        }
    }

    /**
     * Represents the chart palette.
     */
    export interface _IPalette {
        _getColor(i: number): string;
        _getColorLight(i: number): string;
    }

    /**
     * Extends the @see:Tooltip class to provide chart tooltips.
     */
    export class ChartTooltip extends Tooltip {
        private _content: any = '<b>{seriesName}</b><br/>{x} {y}';
        private _threshold: number = 15;

        /**
         * Initializes a new instance of a @see:ChartTooltip.
         */
        constructor() {
            super();
        }

        /**
         * Gets or sets the tooltip content.
         */
        get content(): any {
            return this._content;
        }
        set content(value: any) {
            if (value != this._content) {
                this._content = value;
            }
        }

        /**
         * Gets or sets the maximum distance from the element to display the tooltip.
         */
        get threshold(): number {
            return this._threshold;
        }
        set threshold(value: number) {
            if (value != this._threshold) {
                this._threshold = asNumber(value);
            }
        }
    }
} 
/**
 * Defines the @see:FlexChart control and its associated classes.
 *
 * The example below creates a @see:FlexChart control and binds it to a data array.
 * The chart has three series, each corresponding to a property in the objects 
 * contained in the source array. The last series in the example uses the 
 * <a href="http://wijmo.com/5/docs/topic/wijmo.chart.ChartType.Enum.html" 
 * target="_blank">chartType property</a> to override the default chart type used 
 * by the other series.
 *
 * @fiddle:6GB66
 */
module wijmo.chart {
    'use strict';

    /**
     * Specifies the chart type.
     */
    export enum ChartType {
        /** Column charts show vertical bars and allow you to compare values of items across categories. */
        Column,
        /** Bar charts show horizontal bars. */
        Bar,
        /** Scatter charts use X and Y coordinates to show patterns within the data. */
        Scatter,
        /** Line charts show trends over a period of time or across categories. */
        Line,
        /** Line and symbol charts are line charts with a symbol on each data point. */
        LineSymbols,
        /** Area charts are line charts with the area below the line filled with color. */
        Area,
        /** Bubble charts are Scatter charts with a 
         * third data value that determines the size of the symbol. */
        Bubble,
        /** Candlestick charts present items with high, low, open, and close values.
         * The size of the wick line is determined by the High and Low values, while the size of the bar is 
         * determined by the Open and Close values. The bar is displayed using different colors, depending on 
         * whether the close value is higher or lower than the open value. */
        Candlestick,
        /** High-low-open-close charts display the same information as a candlestick chart, except that opening 
         * values are displayed using lines to the left, while lines to the right indicate closing values.  */
        HighLowOpenClose,
        /** Spline charts are line charts that plot curves rather than angled lines through the data points. */
        Spline,
        /** Spline and symbol charts are spline charts with symbols on each data point. */
        SplineSymbols,
        /** Spline area charts are spline charts with the area below the line filled with color. */
        SplineArea
    }

    /**
     * The @see:FlexChart control provides a powerful and flexible way to visualize
     * data.
     *
     * You can use the @see:FlexChart control to create charts that display data in
     * several formats, including bar, line, symbol, bubble, and others.
     *
     * To use the @see:FlexChart control, set the @see:itemsSource property to an 
     * array containing the data, then add one or more @see:Series objects
     * to the @see:series property.
     *
     * Use the @see:chartType property to define the @see:ChartType used for all series. 
     * You may override the chart type for each series by setting the @see:chartType 
     * property on each @see:Series object.
    */
    export class FlexChart extends FlexChartCore {

        private _chartType = ChartType.Column;

        /**
         * Initializes a new instance of the @see:FlexChart control.
         *
         * @param element The DOM element that will host the control, or a selector for the host element (e.g. '#theCtrl').
         * @param options A JavaScript object containing initialization data for the control.
         */
        constructor(element: any, options?) {
            super(element, options);
        }

        _getChartType(): ChartType {
            return this._chartType;
        }

        /**
         * Gets or sets the type of chart to create.
         */
        get chartType(): ChartType {
            return this._chartType;
        }
        set chartType(value: ChartType) {
            if (value != this._chartType) {
                this._chartType = asEnum(value, ChartType);
                this.invalidate();
            }
        }

        /**
         * Gets or sets whether and how series objects are stacked.
         */
        get stacking(): Stacking {
            return this._stacking;
        }
        set stacking(value: Stacking) {
            if (value != this._stacking) {
                this._stacking = asEnum(value, Stacking);
                this.invalidate();
            }
        }
    }
}
module wijmo.chart {
    'use strict';

    /**
     * Specifies the position of an axis or legend on the chart.
     */
    export enum Position {
        /** The item is not visible. */
        None,
        /** The item appears to the left of the chart. */
        Left,
        /** The item appears above the chart. */
        Top,
        /** The item appears to the right of the chart. */
        Right,
        /** The item appears below the chart. */
        Bottom
    };

    /**
     * Specifies the axis type.
     */
    export enum AxisType {
        /** Category axis (normally horizontal). */
        X,
        /** Value axis (normally vertical). */
        Y
    }

    /**
     * Specifies how to handle overlapping labels.
     */
    export enum OverlappingLabels {
        /**
         * Hide overlapping labels.
         */
        Auto,
        /**
         * Show all labels, including overlapping ones.
         */
        Show
    }

    /**
     * Axis interface.
     */
    export interface _IAxis {
        actualMin: number;
        actualMax: number;
        convert(val: number): number
    }

    /**
     * Specifies whether and where axis tick marks appear.
     */
    export enum TickMark {
        /** No tick marks appear. */
        None,
        /** Tick marks appear outside the plot area. */
        Outside,
        /** Tick marks appear inside the plot area. */
        Inside,
        /** Tick marks cross the axis. */
        Cross
    }

    /**
     * Represents an axis in the chart.
     */
    export class Axis implements _IAxis {
        private _GRIDLINE_WIDTH = 0.25;
        private _LINE_WIDTH = 1;
        private _TICK_WIDTH = 1;
        private _TICK_HEIGHT = 4;
        private _TICK_OVERLAP = 1;
        private _TICK_LABEL_DISTANCE = 4;

        // property storage
        _chart: FlexChartCore;
        private _type: any;
        private _min: any;
        private _max: any;
        private _position: Position;
        private _majorUnit: any; // number, '1w', '1m', etc
        private _minorUnit: any;
        private _majorGrid = false;
        private _minorGrid = false;
        private _title: string;
        private _labelStyle: any;
        private _reversed: boolean;
        private _format: string;
        private _actualMin: number;
        private _actualMax: number;
        private _axisType: AxisType;
        private _majorTickMarks: TickMark;
        private _minorTickMarks: TickMark;
        private _logBase: number;
        private _labels = true;
        private _labelAngle;
        private _axisLine = true;
        private _plotrect: Rect;
        private _szTitle: Size;
        private _isTimeAxis: boolean = false;
        // private _fgColor = 'black';
        private _lbls: string[];
        private _values: number[];
        private _rects: Rect[];
        private _name: string;
        private _origin: number;
        private _overlap: OverlappingLabels;
        private _items: any;
        private _cv: wijmo.collections.ICollectionView;
        private _binding: string;
        private _ifmt: Function;

        private static _id = 0;
        private __uniqueId: number;

        _axrect: Rect;
        _desiredSize: Size;
        _annoSize: Size;
        _hasOrigin: boolean;

        /**
         * Initializes a new instance of an @see:Axis object.
         *
         * @param position The position of the axis on the chart.
         */
        constructor(position: Position) {
            this.__uniqueId = Axis._id++;

            this._position = position;
            if (position == Position.Bottom || position == Position.Top) {
                this._axisType = AxisType.X;
            } else {
                this._axisType = AxisType.Y;
                this._axisLine = false;
            }

            this._minorTickMarks = TickMark.None;
            this._overlap = OverlappingLabels.Auto;
        }

        //--------------------------------------------------------------------------
        //** object model

        /**
         * Gets the actual axis minimum.
        */
        get actualMin(): number {
            return this._actualMin;
        }

        /**
        * Gets the actual axis maximum.
        */
        get actualMax(): number {
            return this._actualMax;
        }

        /**
         * Gets or sets the minimum value shown on the axis. 
         * If not set, the minimum is calculated automatically.
         */
        get min(): number {
            return this._min;
        }
        set min(value: number) {
            this._min = asNumber(value, true);
        }

        /**
         * Gets or sets the maximum value shown on the axis. 
         * If not set, the maximum is calculated automatically.
         */
        get max(): number {
            return this._max;
        }
        set max(value: number) {
            this._max = asNumber(value, true);
        }

        /**
         * Gets or sets a value indicating whether the axis is 
         * reversed (top to bottom or right to left).
         */
        get reversed(): boolean {
            return this._reversed;
        }
        set reversed(value: boolean) {
            this._reversed = asBoolean(value);
        }

        /**
        * Gets or sets the enumerated axis position.
        */
        get position(): Position {
            return this._position;
        }
        set position(value: Position) {
            if (value != this._position) {
                this._position = asEnum(value, Position, false);
                this._invalidate();
            }
        }

        /**
         * Gets or sets the number of units between axis labels.
         *
         * If the axis contains date values, then the units are 
         * expressed in days.
         */
        get majorUnit(): number {
            return this._majorUnit;
        }
        set majorUnit(value: number) {
            if (value != this._majorUnit) {
                this._majorUnit = asNumber(value, true);
                this._invalidate()
            }
        }

        /**
          * Gets or sets the number of units between minor axis ticks.
          *
          * If the axis contains date values, then the units are 
          * expressed in days.
          */
        get minorUnit(): number {
            return this._minorUnit;
        }
        set minorUnit(value: number) {
            if (value != this._minorUnit) {
                this._minorUnit = asNumber(value, true);
                this._invalidate()
            }
        }

        /**
         * Gets or sets the axis name.
         */
        get name(): string {
            return this._name;
        }
        set name(value: string) {
            if (value != this._name) {
                this._name = asString(value, true);
            }
        }

        /**
         * Gets or sets the title text shown next to the axis.
         */
        get title(): string {
            return this._title;
        }
        set title(value: string) {
            if (value != this._title) {
                this._title = asString(value, true);
                this._invalidate();
            }
        }

        /**
         * Gets or sets the format string used for axis labels 
         * (see @see:wijmo.Globalize).
         */
        get format(): string {
            return this._format;
        }
        set format(value: string) {
            if (value != this._format) {
                this._format = asString(value, true);
                this._invalidate();
            }
        }
        //

        /**
         * Gets or sets a value indicating whether the axis includes grid lines.
         */
        get majorGrid(): boolean {
            return this._majorGrid;
        }
        set majorGrid(value: boolean) {
            if (value != this._majorGrid) {
                this._majorGrid = asBoolean(value, true);
                this._invalidate();
            }
        }

        /**
         * Gets or sets the location of axis tick marks.
         */
        get majorTickMarks(): TickMark {
            return this._majorTickMarks;
        }
        set majorTickMarks(value: TickMark) {
            if (value != this._majorTickMarks) {
                this._majorTickMarks = asEnum(value, TickMark, true);
                this._invalidate();
            }
        }

        /**
         * Gets or sets a value indicating whether the axis includes minor grid lines.
         */
        get minorGrid(): boolean {
            return this._minorGrid;
        }
        set minorGrid(value: boolean) {
            if (value != this._minorGrid) {
                this._minorGrid = asBoolean(value, true);
                this._invalidate();
            }
        }

        /**
         * Gets or sets the location of minor axis tick marks.
         */
        get minorTickMarks(): TickMark {
            return this._minorTickMarks;
        }
        set minorTickMarks(value: TickMark) {
            if (value != this._minorTickMarks) {
                this._minorTickMarks = asEnum(value, TickMark, true);
                this._invalidate();
            }
        }

        /**
         * Gets or sets a value indicating whether the axis line is visible.
         */
        get axisLine(): boolean {
            return this._axisLine;
        }
        set axisLine(value: boolean) {
            if (value != this._axisLine) {
                this._axisLine = asBoolean(value, true);
                this._invalidate();
            }
        }

        /**
         * Gets or sets a value indicating whether axis labels are visible.
         */
        get labels(): boolean {
            return this._labels;
        }
        set labels(value: boolean) {
            if (value != this._labels) {
                this._labels = asBoolean(value, true);
                this._invalidate();
            }
        }

        /**
         * Gets or sets the rotation angle of axis labels.
         *
         * The angle is measured in degrees with valid values 
         * ranging from -90 to 90.
         */
        get labelAngle(): number {
            return this._labelAngle;
        }
        set labelAngle(value: number) {
            if (value != this._labelAngle) {
                this._labelAngle = asNumber(value, true);
                this._invalidate();
            }
        }

        /**
         * Gets or sets the value at which the axis crosses the perpendicular axis.
         **/
        get origin(): number {
            return this._origin;
        }
        set origin(value: number) {
            if (value != this._origin) {
                this._origin = asNumber(value, true);
                this._invalidate();
            }
        }

        /**
         * Gets or sets a value indicating how to handle overlapping axis labels.
         */
        get overlappingLabels(): OverlappingLabels {
            return this._overlap;
        }
        set overlappingLabels(value: OverlappingLabels) {
            if (value != this._overlap) {
                this._overlap = asEnum(value, OverlappingLabels, true);
                this._invalidate();
            }
        }

        /**
         * Gets or sets the items source for axis labels.
         * 
         * The names of properties are specified by the @see:wijmo.chart.Axis.binding.
         *
         * For example:
         *
         * <pre>
         *  // default value for Axis.binding is 'value,text'
         *  chart.axisX.itemsSource = [ { value:1, text:'one' }, { value:2, text:'two' } ];
         * </pre>
         */
        get itemsSource(): any {
            return this._items;
        }
        set itemsSource(value: any) {
            if (this._items != value) {

                // unbind current collection view
                if (this._cv) {
                    this._cv.collectionChanged.removeHandler(this._cvCollectionChanged, this);
                    this._cv = null;
                }

                // save new data source and collection view
                this._items = value;
                this._cv = asCollectionView(value);

                // bind new collection view
                if (this._cv != null) {
                    this._cv.collectionChanged.addHandler(this._cvCollectionChanged, this);
                }

            }
        }

        /**
         * Gets or sets the comma-separated property names for the @see:wijmo.chart.Axis.itemsSource
         * property to use in axis labels.
         *
         * The first name specifies the value on the axis, the second represents the corresponding
         * axis label. The default value is 'value,text.'
         */
        get binding(): string {
            return this._binding;
        }
        set binding(value: string) {
            if (value != this._binding) {
                this._binding = asString(value, true);
                this._invalidate();
            }
        }

        /**
         * Gets or sets the itemFormatter function for axis labels.
         * 
         * If specified, the function takes two parameters:
         * <ul>
         * <li><b>render engine</b>: The @see:wijmo.chart.IRenderEngine object to use 
         * in formatting the labels.</li>
         * <li><b>current label</b>: A string value with the following properties: 
         *   <ul>
         *     <li><b>value</b>: The value of the axis label to format.</li>
         *     <li><b>text</b>: The text to use in the label.</li>
         *     <li><b>pos</b>: The position in control coordinates at which to 
         *       render the label.</li>
         *     <li><b>cls</b>: The CSS class to apply to the label.</li>
         *   </ul></li>
         * </ul>
         *
         * The function returns the label parameters for labels on which to 
         * modify properties.
         * 
         * For example:
         * <pre>
         * chart.axisY.itemFormatter = function(engine, label) {
         *     if(label.val > 5){
         *         engine.textFill = 'red'; // red text
         *         label.cls = null;// no default css
         *      }
         *     return label;
         * }
         * </pre>
         */
        get itemFormatter(): Function {
            return this._ifmt;
        }
        set itemFormatter(value: Function) {
            if (this._items != value) {
                this._ifmt = asFunction(value, true);
            }
        }

        /**
         * Gets or sets the logarithmic base of the axis.
         *
         * If the base is not specified the axis uses the normal scale.
         */
        get logBase(): number {
            return this._logBase;
        }
        set logBase(value: number) {
            if (value != this._logBase) {
                this._logBase = asNumber(value, true, true);
                this._invalidate();
            }
        }

        /**
         * Occurs when the axis range changes.
         */
        rangeChanged = new Event();

        /**
         * Raises the @see:rangeChanged event.
         */
        onRangeChanged() {
            this.rangeChanged.raise(this);
        }

        //--------------------------------------------------------------------------
        // implementation

        /**
         * Calculates the axis height.
         *
         * @param engine Rendering engine.
         */
        _getHeight(engine: IRenderEngine): number {
            var lblClass = FlexChart._CSS_LABEL;
            var titleClass = FlexChart._CSS_TITLE;

            var range = this._actualMax - this._actualMin;
            var prec = this._nicePrecision(range);
            if (prec < 0 || prec > 15)
                prec = 0;

            var delta = 0.1 * range;// r * 0.01 * Math.E;

            var lbls = this._lbls;

            if (this.labels) {
                if (lbls != null && lbls.length > 0) {
                    this._annoSize = new Size();
                    for (var i = 0; i < lbls.length; i++) {
                        var sz = engine.measureString(lbls[i], lblClass);
                        if (sz.width > this._annoSize.width)
                            this._annoSize.width = sz.width;
                        if (sz.height > this._annoSize.height)
                            this._annoSize.height = sz.height;
                    }
                } else {
                    var text = this._formatValue(this._actualMin - delta);
                    var sz = engine.measureString(text, lblClass);
                    this._annoSize = sz;

                    text = this._formatValue(this._actualMax + delta);

                    sz = engine.measureString(text, lblClass);
                    if (sz.width > this._annoSize.width) {
                        this._annoSize.width = sz.width;
                    }
                    if (sz.height > this._annoSize.height)
                        this._annoSize.height = sz.height;
                }
                if (this.labelAngle) {
                    var a = this.labelAngle * Math.PI / 180,
                        w = this._annoSize.width,
                        h = this._annoSize.height;

                    this._annoSize.width = w * Math.abs(Math.cos(a)) + h * Math.abs(Math.sin(a));
                    this._annoSize.height = w * Math.abs(Math.sin(a)) + h * Math.abs(Math.cos(a));
                }
            } else {
                this._annoSize = new Size();
            }

            var h = 2;

            if (this._axisType == AxisType.X) {
                h += this._annoSize.height;
            } else {
                h += this._annoSize.width + this._TICK_LABEL_DISTANCE + 2;
            }

            var th = this._TICK_HEIGHT;
            var tover = this._TICK_OVERLAP;

            if (tickMarks == TickMark.Outside) {
                tover = 1;
            } else if (tickMarks == TickMark.Inside) {
                tover = -1;
            } else if (tickMarks == TickMark.Cross) {
                tover = 0;
            }

            var tickMarks = this.majorTickMarks;
            if (tickMarks === undefined || tickMarks === null) {
                tickMarks = TickMark.Outside;
            }

            if (tickMarks != TickMark.None) {
                h += 0.5 * (1 + tover) * th;
            }

            if (this._title) {
                text = this._title;
                this._szTitle = engine.measureString(text, titleClass);
                h += this._szTitle.height;
            }

            engine.fontSize = null;

            return h;
        }

        /**
         * Update actual axis limits based on specified data range.
         *
         * @param dataType Data type.
         * @param dataMin Data minimum.
         * @param dataMax Data maximum.
         * @param labels Category labels(category axis).
         * @param values Values(value axis).
         */
        _updateActualLimits(dataType: DataType, dataMin: number, dataMax: number, labels: string[]= null, values: number[]= null) {
            var oldmin = this._actualMin,
                oldmax = this._actualMax;

            this._isTimeAxis = (dataType == DataType.Date);
            var ctype = this._chart._getChartType();
            if (labels && labels.length>0 && !this._isTimeAxis && ctype != ChartType.Column && ctype != ChartType.Bar
            //&& ctype != ChartType.Bubble
                ) {
                dataMin -= 0.5;
                dataMax += 0.5;
            }
            this._actualMin = this._min != null ? this._min : dataMin;
            this._actualMax = this._max != null ? this._max : dataMax;

            // todo: validate min&max
            if (this._actualMin == this._actualMax) {
                this._actualMin -= 0.5;
                this._actualMax += 0.5;
            }

            if (this.logBase > 0) {
                var base = this.logBase;
                var k = Math.log(base);

                if (!this.max) {
                    var imax = Math.ceil(Math.log(this._actualMax) / k);
                    this._actualMax = Math.pow(base, imax);
                }
                if (!this.min) {
                    var imin = Math.floor(Math.log(this._actualMin) / k);
                    this._actualMin = Math.pow(base, imin);
                }

                if (this._actualMin <= 0) {
                    this._actualMin = 1;
                }
                if (this._actualMax < this._actualMin) {
                    this._actualMax = this._actualMin + 1;
                }
            }

            if ((oldmin != this._actualMin && (!isNaN(oldmin) || !isNaN(this._actualMin)) )
                || (oldmax != this._actualMax && (!isNaN(oldmax) || !isNaN(this._actualMax)) )) {
                this.onRangeChanged();
            }

            if (this._items) {
                this._values = [];
                this._lbls = [];
                var len = this._items.length;
                var vbnd = 'value';
                var nbnd = 'text';
                if (this.binding) {
                    var bnds = this.binding.split(',');
                    if (bnds.length == 2) {
                        vbnd = bnds[0];
                        nbnd = bnds[1];
                    }
                }
                for (var i = 0; i < len; i++) {
                    var item = this._items[i];
                    var val = item[vbnd];
                    if (isNumber(val)) {
                        this._values.push(val);
                        this._lbls.push( item[nbnd]);
                    }
                }
            } else {
            this._lbls = labels;
            this._values = values;
            }
            
        }

        /**
         * Set axis position.
         *
         * @param axisRect Axis rectangle.
         * @param plotRect Plot area rectangle.
         */
        _layout(axisRect: Rect, plotRect: Rect) {
            var isVert = this.axisType == AxisType.Y;
            var isNear = this._position != Position.Top && this._position != Position.Right;

            this._plotrect = plotRect;

            if (isVert)
                this._axrect = new Rect(axisRect.left, axisRect.top, axisRect.height, axisRect.width);
            else
                this._axrect = axisRect;
        }

        /**
         * Render the axis.
         *
         * @param engine Rendering engine.
         */
        _render(engine: IRenderEngine) {
            if (this.position == Position.None) {
                return;
            }

            var lblClass = FlexChart._CSS_LABEL;
            var titleClass = FlexChart._CSS_TITLE;
            var lineClass = FlexChart._CSS_LINE;
            var glineClass = FlexChart._CSS_GRIDLINE;
            var tickClass = FlexChart._CSS_TICK;
            var labelAngle = 0;
            if (this.labelAngle) {
                labelAngle = this.labelAngle;
                if (labelAngle > 90) {
                    labelAngle = 90;
                } else if (labelAngle < -90) {
                    labelAngle = -90;
                }
            }

            var isVert = this.axisType == AxisType.Y;
            var isNear = this._position != Position.Top && this._position != Position.Right;

            var fg = FlexChart._FG;
            var fontSize = null;

            var range = this._actualMax - this._actualMin;

            if (!isNaN(range)) {

            var delta = this._calcMajorUnit();
            if (delta == 0)
                delta = this._niceTickNumber(range) * 0.1;

            var len = Math.floor(range / delta) + 1;
            var vals = [];
            var lbls = [];
                this._rects = [];

            var st = Math.floor(this._actualMin / delta) * delta;
            if (st < this._actualMin)
                st += delta;

            var isCategory = false;
            // labels
            if (this._lbls && this._lbls.length > 0) {
                    lbls = this._lbls; // category
                    if (this._values.length == 0) {
                isCategory = true;
                for (var i = 0; i < lbls.length; i++) {
                    vals.push(i);
                }
            }
                    else {
                        vals = this._values;
                    }
                }
            else if (this._isTimeAxis) {
                this._createTimeLabels(st, len, vals, lbls); // time 
                } else if (!this.logBase) {
                    this._createLabels(st, len, delta, vals, lbls); // numeric
            } else {
                    this._createLogarithmicLabels(this._actualMin, this._actualMax, this.majorUnit, vals, lbls, true);
            }

            len = Math.min(vals.length, lbls.length);

            engine.textFill = fg;

            var th = this._TICK_HEIGHT;
            var tth = this._TICK_WIDTH;
            var tover = this._TICK_OVERLAP;
            var tstroke = fg;

            var tickMarks = this.majorTickMarks;
            if (tickMarks === undefined || tickMarks === null) {
                tickMarks = TickMark.Outside;
            }

            var gth = this._GRIDLINE_WIDTH;
            var gstroke = fg;

            if (tickMarks == TickMark.Outside) {
                tover = 1;
            } else if (tickMarks == TickMark.Inside) {
                tover = -1;
            } else if (tickMarks == TickMark.Cross) {
                tover = 0;
            }

            var t1 = 0.5 * (tover - 1) * th;
            var t2 = 0.5 * (1 + tover) * th;

            for (var i = 0; i < len; i++) {
                var val = vals[i];
                var sval = lbls[i];
                    var showLabel = this.labels;
                    if (showLabel && isCategory && this.majorUnit) {
                        if (i % this.majorUnit != 0) {
                            showLabel = false;
                        }
                    }

                if (val >= this._actualMin && val <= this._actualMax) {
                        var has_gline = val != this._actualMin && /*val != this._actualMax &&*/ this.majorGrid;
                    if (isVert) {
                        var y = this.convert(val);

                            if (has_gline) {
                        engine.stroke = gstroke;
                        engine.strokeWidth = gth;
                            engine.drawLine(this._plotrect.left, y, this._plotrect.right, y, glineClass);
                        }

                        engine.stroke = tstroke;
                        engine.strokeWidth = tth;
                        if (isNear) {
                            if (tickMarks != TickMark.None) {
                                engine.drawLine(this._axrect.right - t1, y, this._axrect.right - t2, y, tickClass);
                            }
                                if (showLabel) {
                                    var lpt = new Point(this._axrect.right - t2 - this._TICK_LABEL_DISTANCE, y);
                                    if ( labelAngle > 0) {
                                        if ( labelAngle == 90) {
                                            FlexChart._renderRotatedText(engine, sval, lpt, 1, 0, lpt, labelAngle, lblClass);
                                        } else {
                                            FlexChart._renderRotatedText(engine, sval, lpt, 2, 1, lpt, labelAngle, lblClass);
                                        }
                                    } else if (labelAngle < 0) {
                                        if ( labelAngle == -90) {
                                            FlexChart._renderRotatedText(engine, sval, lpt, 1, 2, lpt, labelAngle, lblClass);
                                        } else {
                                            FlexChart._renderRotatedText(engine, sval, lpt, 2, 1, lpt, labelAngle, lblClass);
                                        }
                                    } else {
                                        this._renderLabel(engine, val, sval, lpt, 2, 1, lblClass);
                                    }
                            }
                        } else {
                            if (tickMarks != TickMark.None) {
                                engine.drawLine(this._axrect.left + t1, y, this._axrect.left + t2, y, tickClass);
                            }
                                if (showLabel) {
                                    var lpt = new Point(this._axrect.left + t2 + this._TICK_LABEL_DISTANCE, y);
                                    if ( labelAngle > 0) {
                                        if ( labelAngle == 90) {
                                            FlexChart._renderRotatedText(engine, sval, lpt, 1, 2, lpt, labelAngle, lblClass);
                                        } else {
                                            FlexChart._renderRotatedText(engine, sval, lpt, 0, 1, lpt, labelAngle, lblClass);
                                        }
                                    } else if ( labelAngle < 0) {
                                        if ( labelAngle == -90) {
                                            FlexChart._renderRotatedText(engine, sval, lpt, 1, 0, lpt, labelAngle, lblClass);
                                        } else {
                                            FlexChart._renderRotatedText(engine, sval, lpt, 0, 1, lpt, labelAngle, lblClass);
                                        }
                                    } else {
                                        this._renderLabel(engine, val, sval, lpt, 0, 1, lblClass);
                                    }
                            }
                        }
                    } else {
                        var x = this.convert(val);

                            if (has_gline) {
                        engine.stroke = gstroke;
                        engine.strokeWidth = gth;
                            engine.drawLine(x, this._plotrect.top, x, this._plotrect.bottom, glineClass);
                        }

                        engine.stroke = tstroke;
                        engine.strokeWidth = tth;
                        if (isNear) {
                                if (showLabel) {
                                    var lpt = new Point(x, this._axrect.top + t2);
                                    if (labelAngle != 0) {
                                        var sz = engine.measureString(sval, lblClass);
                                        if (labelAngle != 90 && labelAngle != -90) {
                                        lpt.y += 0.5 * sz.height * Math.abs(Math.sin(labelAngle * Math.PI / 180));
                                        }
                                        if (labelAngle > 0) {
                                            FlexChart._renderRotatedText(engine, sval, lpt, 0, 1, lpt, labelAngle, lblClass);
                                        } else if (labelAngle < 0) {
                                            FlexChart._renderRotatedText(engine, sval, lpt, 2, 1, lpt, labelAngle, lblClass);
                                        }
                                    }else {
                                        this._renderLabel(engine, val, sval, lpt, 1, 0, lblClass);
                                    }
                            }

                            if (tickMarks != TickMark.None) {
                                if (isCategory) {
                                    val = val - 0.5;
                                    if (val >= this._actualMin && val <= this._actualMax) {
                                        x = this.convert(val);
                                        engine.drawLine(x, this._axrect.top + t1, x, this._axrect.top + t2, tickClass);
                                    }
                                    if (i == len - 1) {
                                        val = val + 1;
                                        if (val >= this._actualMin && val <= this._actualMax) {
                                            x = this.convert(val);
                                            engine.drawLine(x, this._axrect.top + t1, x, this._axrect.top + t2, tickClass);
                                        }
                                    }
                                } else {
                                    x = this.convert(val);
                                    engine.drawLine(x, this._axrect.top + t1, x, this._axrect.top + t2, tickClass);
                                }
                            }

                        } else {
                                if (showLabel) {
                                    var lpt = new Point(x, this._axrect.bottom - t2);
                                    if (labelAngle != 0) {
                                        var sz = engine.measureString(sval, lblClass);
                                        if (labelAngle != 90 && labelAngle != -90) {
                                        lpt.y -= 0.5 * sz.height * Math.abs(Math.sin(labelAngle * Math.PI / 180));
                                        }
                                        if (labelAngle > 0) {
                                            FlexChart._renderRotatedText(engine, sval, lpt, 2, 1, lpt, labelAngle, lblClass);
                                        } else if (labelAngle < 0) {
                                            FlexChart._renderRotatedText(engine, sval, lpt, 0, 1, lpt, labelAngle, lblClass);
                                        }
                                    }else {
                                        this._renderLabel(engine, val, sval, lpt, 1, 2, lblClass);
                                    }
                            }

                            if (tickMarks != TickMark.None) {
                                if (isCategory) {
                                    val = val - 0.5;
                                    if (val >= this._actualMin && val <= this._actualMax) {
                                        x = this.convert(val);
                                        engine.drawLine(x, this._axrect.bottom - t1, x, this._axrect.bottom - t2, tickClass);
                                    }
                                        if (i == len - 1) {
                                        val = val + 1;
                                        if (val >= this._actualMin && val <= this._actualMax) {
                                            x = this.convert(val);
                                            engine.drawLine(x, this._axrect.bottom - t1, x, this._axrect.bottom - t2, tickClass);
                                        }
                                    }
                                }
                                else {
                                    x = this.convert(val);
                                    engine.drawLine(x, this._axrect.bottom - t1, x, this._axrect.bottom - t2, tickClass);
                                }
                            }
                        }
                    }
                }
            }
            }

            if (!isCategory && (this.minorGrid || this.minorTickMarks!= TickMark.None) ) {
                if (!this.logBase)
                    this._createMinors(engine, vals, isVert, isNear);
                else {
                    if (this.minorUnit > 0) {
                        var mvals = [];
                        this._createLogarithmicLabels(this._actualMin, this._actualMax, this.minorUnit, mvals, null, false);
                        var ticks = [];
                        for (var i = 0; i < mvals.length; i++) {
                            var val = mvals[i];
                            if (vals.indexOf(val) == -1 && val > this._actualMin)
                                ticks.push(val);
                        }

                        this._renderMinors(engine, ticks, isVert, isNear);
                    }
                }
            }

            engine.stroke = fg;
            engine.fontSize = fontSize;

            // line and title
            if (isVert) {
                if (isNear) {
                    if (this._title) {
                        var center = new Point(this._axrect.left + this._szTitle.height * 0.5, this._axrect.top + 0.5 * this._axrect.height);
                        FlexChart._renderRotatedText(engine, this._title, center, 1, 1, center, -90, titleClass);
                    }

                    if (this.axisLine) {
                        engine.drawLine(this._axrect.right, this._axrect.top, this._axrect.right, this._axrect.bottom, lineClass);
                    }
                } else {
                    if (this._title) {
                        var center = new Point(this._axrect.right - this._szTitle.height * 0.5, this._axrect.top + 0.5 * this._axrect.height);
                        FlexChart._renderRotatedText(engine, this._title, center, 1, 1, center, 90, titleClass);
                    }

                    if (this.axisLine) {
                        engine.drawLine(this._axrect.left, this._axrect.top, this._axrect.left, this._axrect.bottom, lineClass);
                    }
                }
            } else {
                if (isNear) {
                    if (this.axisLine) {
                        engine.drawLine(this._axrect.left, this._axrect.top, this._axrect.right, this._axrect.top, lineClass);
                    }

                    if (this._title) {
                        FlexChart._renderText(engine, this._title,
                            new Point(this._axrect.left + 0.5 * this._axrect.width, this._axrect.bottom), 1, 2, titleClass);
                    }
                } else {
                    if (this.axisLine) {
                        engine.drawLine(this._axrect.left, this._axrect.bottom, this._axrect.right, this._axrect.bottom, lineClass);
                    }

                    if (this._title) {
                        FlexChart._renderText(engine, this._title,
                            new Point(this._axrect.left + 0.5 * this._axrect.width, this._axrect.top), 1, 0, titleClass);
                    }
                }
            }

            engine.stroke = null;
            engine.fontSize = null;
            engine.textFill = null;
            engine.strokeWidth = null;
        }

        _createMinors(engine: IRenderEngine, vals: number[], isVert: boolean, isNear: boolean) {
            if (vals && vals.length > 1) {
                var delta = vals[1] - vals[0];
                var minorUnit = isNumber(this.minorUnit) ? this.minorUnit : delta * 0.5;

                var ticks = [];

                for (var val = vals[0]; val > this._actualMin; val -= minorUnit) {
                    if (vals.indexOf(val) == -1)
                        ticks.push(val);
                }

                for (var val = vals[0]+minorUnit; val < this._actualMax; val += minorUnit) {
                    if (vals.indexOf(val) == -1)
                        ticks.push(val);
                }

                this._renderMinors(engine, ticks, isVert, isNear);
            }
        }

        private _renderMinors(engine:IRenderEngine, ticks:number[], isVert:boolean, isNear:boolean) {
                var th = this._TICK_HEIGHT;
                var tth = this._TICK_WIDTH;
                var tover = this._TICK_OVERLAP;
                var tstroke = FlexChart._FG;

                var tickMarks = this.minorTickMarks;
                var hasTicks = true;

                if (tickMarks == TickMark.Outside) {
                    tover = 1;
                } else if (tickMarks == TickMark.Inside) {
                    tover = -1;
                } else if (tickMarks == TickMark.Cross) {
                    tover = 0;
                } else {
                    hasTicks = false;
                }

                var t1 = 0.5 * (tover - 1) * th;
                var t2 = 0.5 * (1 + tover) * th;

                var cnt = ticks ? ticks.length : 0;

                var grid = this.minorGrid;
                var prect = this._plotrect;

                var gth = this._GRIDLINE_WIDTH;
                var gstroke = FlexChart._FG;

                // css
                var glineClass = FlexChart._CSS_GRIDLINE_MINOR;
                var tickClass = FlexChart._CSS_TICK_MINOR;

                for (var i = 0; i < cnt; i++) {
                    if (isVert) {
                        var y = this.convert(ticks[i]);

                        if (hasTicks) {
                            engine.stroke = tstroke;
                            engine.strokeWidth = tth;

                            if (isNear) {
                                engine.drawLine(this._axrect.right - t1, y, this._axrect.right - t2, y, tickClass);
                            } else {
                                engine.drawLine(this._axrect.left + t1, y, this._axrect.left + t2, y, tickClass);
                            }
                        }

                        if (grid) {
                            engine.stroke = gstroke;
                            engine.strokeWidth = gth;
                            engine.drawLine(prect.left, y, prect.right, y, glineClass);
                        }
                    } else {
                        var x = this.convert(ticks[i]);

                        if (hasTicks) {
                            engine.stroke = tstroke;
                            engine.strokeWidth = tth;

                            if (isNear) {
                                engine.drawLine(x, this._axrect.top + t1, x, this._axrect.top + t2, tickClass);
                            } else {
                                engine.drawLine(x, this._axrect.bottom - t1, x, this._axrect.bottom - t2, tickClass);
                            }
                        }

                        if (grid) {
                            engine.stroke = gstroke;
                            engine.strokeWidth = gth;
                            engine.drawLine(x, prect.top, x, prect.bottom, glineClass);
                        }
                    }
                }
            }

        private _renderLabel(engine: IRenderEngine, val:number, text: string, pos: Point, ha, va, className?: string) {
            if (this.itemFormatter) {
                var pt = pos.clone();
                if (this.axisType == AxisType.X) {
                    if (this.position == Position.Top)
                        pt.y = this._plotrect.top;
                    else
                        pt.y = this._plotrect.bottom;
                } else {
                    if (this.position == Position.Right)
                        pt.x = this._plotrect.right;
                    else
                        pt.x = this._plotrect.left;
                }
                var lbl = { val:val, text:text, pos: pt, cls:className };

                lbl = this.itemFormatter(engine, lbl);

                if (lbl) {
                    text = lbl.text;
                    className = lbl.cls;
                } else {
                    text = null;
                }
            }

            if (text) {
                var rects = this._rects;
                var hide = this.overlappingLabels == OverlappingLabels.Auto;
                var rect = FlexChart._renderText(engine, text, pos, ha, va, className, null, null, function (rect) {
                    if (hide) {
                        var len = rects.length;
                        for (var i = 0; i < len; i++) {
                            if (FlexChart._intersects(rects[i], rect))
                                return false;
                        }
                    }
                    return true;
                });
                if (rect) {
                    rects.push(rect);
                }
            }
        }

        /**
         * Converts the specified value from data to pixel coordinates.
         *
         * @param val The data value to convert.
         */
        convert(val: number): number {
            if (this._actualMax == this._actualMin) {
                return 0;
            }

            var x = this._axrect.left;
            var w = this._axrect.width;
            var y = this._axrect.top;
            var h = this._axrect.height;

            var base = this.logBase;

            if (!base) {
                if (this._reversed) {
                    if (this.axisType == AxisType.Y) {
                        return y + (val - this._actualMin) / (this._actualMax - this._actualMin) * h;
                    } else {
                        return x + w - (val - this._actualMin) / (this._actualMax - this._actualMin) * w;
                    }
                } else {
                    if (this.axisType == AxisType.Y) {
                        return y + h - (val - this._actualMin) / (this._actualMax - this._actualMin) * h;
                    } else {
                        return x + (val - this._actualMin) / (this._actualMax - this._actualMin) * w;
                    }
                }
            }
            else {
                if (val <= 0)
                    return NaN;

                var maxl = Math.log(this._actualMax / this._actualMin);

                if (this._reversed) {
                    if (this.axisType == AxisType.Y)
                        return y + Math.log(val / this._actualMin) / maxl * h;
                    else
                        return x + w - Math.log(val / this._actualMin) / maxl * w;
                } else {
                    if (this.axisType == AxisType.Y)
                        return y + h - Math.log(val / this.actualMin) / maxl * h;
                    else
                        return x + Math.log(val / this._actualMin) / maxl * w;
                }
                }
        }

        /**
         * Converts the specified value from pixel to data coordinates.
         *
         * @param val The pixel coordinates to convert back.
         */
        convertBack(val: number): number {
            if (this._actualMax == this._actualMin) {
                return 0;
            }

            var x = this._plotrect.left;
            var w = this._plotrect.width;
            var y = this._plotrect.top;
            var h = this._plotrect.height;

            var range = this._actualMax - this._actualMin;
            var base = this.logBase;

            if (!base) {
                if (this._reversed) {
                    if (this.axisType == AxisType.Y) {
                        return this._actualMin + (val - y) * range / h;
                    } else {
                        return this._actualMin + (x + w - val) * range / w;
                    }
                } else {
                    if (this.axisType == AxisType.Y) {
                        return this._actualMax - (val - y) * range / h;
                    } else {
                        return this._actualMin + (val - x) * range / w;
                    }
                }
            } else {
                var rval = 0;
                if (this._reversed) {
                    if (this.axisType == AxisType.Y) {
                        rval = (val - y) / h;
                    } else {
                        rval = 1 - (val - x) / w;
                    }
                } else {
                    if (this.axisType == AxisType.Y) {
                        rval = 1 - (val - y) / h;
                    } else {
                        rval = (val - x) / w;
                    }
                }

                return Math.pow(base,
                    (Math.log(this._actualMin) + (Math.log(this._actualMax) - Math.log(this._actualMin)) * rval) / Math.log(base));
            }
        }


        /**
         * Gets the axis type.
         */
        get axisType(): AxisType {
            return this._axisType;
        }

        //---------------------------------------------------------------------
        // private

        private _invalidate() {
            if (this._chart) {
                this._chart.invalidate();
            }
        }

        private _cvCollectionChanged(sender, e) {
            this._invalidate();
        }

        private _createLabels(start: number, len: number, delta: number, vals: number[], lbls: string[]) {
            for (var i = 0; i < len; i++) {
                var val0 = (start + delta * i).toFixed(14);//  Math.round(st + delta * i);//, 14); // 15
                var val = parseFloat(val0);
                //if (val > max)
                //  break;
                var sval = this._formatValue(val);

                vals.push(val);
                lbls.push(sval);
            }
        }

        private _createLogarithmicLabels(min: number, max: number, unit: number, vals: number[], lbls: string[], isLabels: boolean) {
            var base = this.logBase;
            var k = Math.log(base);
            var imin = Math.floor(Math.log(min) / k);
            var imax = Math.ceil(Math.log(max) / k);

            var delta = base;

            var auto = true;
            if (unit > 0) {
                auto = false;
                delta = unit; // islabels = false;
            }

            if (delta < base)
                delta = base;
            //if (delta <= 0)
            //    return;

            var n = ((imax - imin + 1) * base / delta);

            // try some rational number for large values.
            // garyh 21-Apr-05 - VNCHT000250
            /* if (n > 128) {
                if (isPowerOf(logbase, 10))
                    delta = logbase / 10;
                else if (isPowerOf(logbase, 5))
                    delta = logbase / 5;
                else
                    delta = logbase / 16;

                n = (int)((imax - imin + 1) * logbase / delta);

                if (n > 128)		// the user must handle the unitminor
                    delta = logbase;
            }*/

            var step = 1;

            if (isLabels) {
                var na = this._getAnnoNumber(this.position == Position.Left || this.position == Position.Left);
                if (n > na)
                    step = Math.floor(n / na + 1);
                else if (auto) {
                    if (n <= 0.2 * na)
                        delta = 0.2 * base;
                    else if (n <= 0.1 * na)
                        delta = 0.1 * base;
                }
            }

            for (var i = imin; i <= imax; i += step) {
                if (auto) {
                    var baseval = Math.pow(base, i);

                    for (var j = 0; j * delta < (base - 1); j++) {
                        var val = baseval * (1 + j * delta);
                        if (val >= min && val <= max) {
                            if (j == 0) {
                                vals.unshift(val);
                                if(lbls)
                                    lbls.unshift(this._formatValue(val));
                            } else {
                                vals.push(val);
                                if (lbls)
                                    lbls.push(this._formatValue(val));
                            }
                        }
                    }
                } else {
                    var val = Math.pow(delta, i);
                    if (val >= min && val <= max) {
                        vals.push(val);
                        if (lbls)
                            lbls.push(this._formatValue(val));
                    }
                }
            }
        }

        private _createTimeLabels(start: number, len: number, vals: number[], lbls: string[]) {
            var min = this._actualMin;
            var max = this._actualMax;
            var dtmin0 = FlexChart._fromOADate(this._actualMin);
            var dtmax0 = FlexChart._fromOADate(this._actualMax);

            var fmt = this._format;
            //if (string.IsNullOrEmpty(fmt))
            // fmt = TimeAxis.GetTimeDefaultFormat(max, min);

            var anum = this._getAnnoNumber(this._axisType == AxisType.Y);

            // alext 10-Jan-2010 
            // better precision
            var td = (24.0 * 3600.0 * (this._actualMax - this._actualMin) / anum);

            var range = new _timeSpan(td * _timeSpan.TicksPerSecond);

            var delta = isNaN(this._majorUnit) ?
                _timeHelper.NiceTimeSpan(range, fmt) : _timeSpan.fromDays(this._majorUnit);

            var delta_ticks = delta.Ticks;

            var newmin = _timeHelper.RoundTime(min, delta.TotalDays, false);
            if (isFinite(newmin))
                min = newmin;
            var newmax = _timeHelper.RoundTime(max, delta.TotalDays, true);
            if (isFinite(newmax))
                max = newmax;

            var dtmin = FlexChart._fromOADate(min);
            var dtmax = FlexChart._fromOADate(max);

            if (delta.TotalDays >= 365 && isNaN(this._majorUnit)) {
                dtmin = new Date(dtmin0.getFullYear(), 1, 1);
                if (dtmin < dtmin0)
                    //dtmin = dtmin.AddYears(1);
                    dtmin.setFullYear(dtmin.getFullYear() + 1);

                var years = (delta.TotalDays / 365);
                years = years - (years % 1);

                for (var current = dtmin; current <= dtmax0;
                    //current = current.AddYears(nyears)
                    current.setFullYear(current.getFullYear() + years)
                    ) {
                    var val = FlexChart._toOADate(current);
                    vals.push(val);
                    lbls.push(this._formatValue(val));
                }
            }
            else if (delta.TotalDays >= 30 && isNaN(this._majorUnit)) {
                dtmin = new Date(dtmin0.getFullYear(), dtmin0.getMonth(), 1);
                if (dtmin < dtmin0)
                    //dtmin = dtmin.AddMonths(1);
                    dtmin.setMonth(dtmin.getMonth() + 1);

                var nmonths = delta.TotalDays / 30;
                nmonths = nmonths - (nmonths % 1);

                for (var current = dtmin; current <= dtmax0;
                    //current = current.AddMonths(nmonths)
                    current.setMonth(current.getMonth() + nmonths)
                    ) {
                    var val = FlexChart._toOADate(current);
                    vals.push(val);
                    lbls.push(this._formatValue(val));
                }
            } else {
                var dt = (1000 * delta_ticks) / _timeSpan.TicksPerSecond;
                var current = dtmin;
                var timedif = dtmin0.getTime() - current.getTime();
                if ( timedif > dt) {
                    current = new Date( current.getTime() + Math.floor( timedif / dt) * dt);
                }
                for (; current <= dtmax0;
                    //current = current.AddTicks(delta_ticks)
                    current = new Date(current.getTime() + dt) ) {

                    if (current >= dtmin0) {
                    var val = FlexChart._toOADate(current);

                    vals.push(val);
                    lbls.push(this._formatValue(val));
                    }
                }
            }
        }

        _formatValue(val: number): string {
            if (this._isTimeAxis) {
                return Globalize.format(FlexChart._fromOADate(val), this._format);
            } else {
                if (this._format)
                return Globalize.format(val, this._format);
                else {
                    var fmt = val == Math.round(val) ? 'n0' : 'n';
                    return Globalize.format(val, fmt);
                }
            }
            //return val.toString();
        }

        private _calcMajorUnit(): number {
            var delta = this._majorUnit;

            if (isNaN(delta)) {
                var range = this._actualMax - this._actualMin;
                var prec = this._nicePrecision(range);
                var dx = range / this._getAnnoNumber(this.axisType == AxisType.Y);

                delta = this._niceNumber(2 * dx, -prec, true);
                if (delta < dx) {
                    delta = this._niceNumber(dx, -prec + 1, false);
                }
                if (delta < dx) {
                    delta = this._niceTickNumber(dx);
                }
            }

            return delta;
        }

        private _getAnnoNumber(isVert: boolean): number {
            var w0 = isVert ? this._annoSize.height : this._annoSize.width;
            var w = isVert ? this._axrect.height : this._axrect.width;
            if (w0 > 0 && w > 0) {
                var n = Math.floor(w / w0);
                if (n <= 0) {
                    n = 1;
                }
                return n;
            } else {
                return 10;
            }
        }

        private _nicePrecision(range: number): number {
            //
            //	Return a nice precision value for this range.
            //	Doesn't take into account font size, window
            //	size, etc.	Just use the log10 of the range.
            //
            if (range <= 0 || isNaN(range)) {
                return 0;
            }

            var log10 = Math.log(range) / Math.LN10;
            var exp;

            if (log10 >= 0) {
                exp = Math.floor(log10); //(int)(SignedFloor(log10));
            } else {
                exp = Math.ceil(log10);
            }

            var f = range / Math.pow(10.0, exp);

            /* we need the extra digit near the lower end */
            if (f < 3.0) {
                exp = -exp + 1;

                // more precision for more labels
                f = range / Math.pow(10.0, exp);
                if (f < 3.0) {
                    exp = exp + 1;
                }
            }
            return exp;
        }

        private _niceTickNumber(x: number): number {
            if (x == 0) {
                return x;
            } else if (x < 0) {
                x = -x;
            }

            var log10 = Math.log(x) / Math.LN10;
            var exp = Math.floor(log10);// (int) SignedFloor(log10);

            var f = x / Math.pow(10.0, exp);
            var nf = 10.0;

            if (f <= 1.0) {
                nf = 1.0;
            } else if (f <= 2.0) {
                nf = 2.0;
            } else if (f <= 5.0) {
                nf = 5.0;
            }
            return (nf * Math.pow(10.0, exp));
        }

        private _niceNumber(x: number, exp: number, round: boolean) {
            if (x == 0) {
                return x;
            } else if (x < 0) {
                x = -x;
            }

            var f = x / Math.pow(10.0, exp);
            var nf = 10.0;

            if (round) {
                if (f < 1.5) {
                    nf = 1;
                } else if (f < 3) {
                    nf = 2;
                } else if (f < 4.5) {
                    nf = 4;
                } else if (f < 7) {
                    nf = 5;
                }
            } else {
                if (f <= 1) {
                    nf = 1;
                } else if (f <= 2) {
                    nf = 2;
                } else if (f <= 5) {
                    nf = 5;
                }
            }

            return (nf * Math.pow(10.0, exp));
        }

        get _uniqueId(): number {
            return this.__uniqueId;
        }
    }


    /**
     * Represents a collection of @see:Axis objects in a @see:FlexChart control.
     */
    export class AxisCollection extends wijmo.collections.ObservableArray {

        /**
         * Gets an axis by name.
         *
         * @param name The name of the axis to look for.
         * @return The axis object with the specified name, or null if not found.
         */
        getAxis(name: string): Axis {
            var index = this.indexOf(name);
            return index > -1 ? this[index] : null;
        }
        /**
         * Gets the index of an axis by name.
         *
         * @param name The name of the axis to look for.
         * @return The index of the axis with the specified name, or -1 if not found.
         */
        indexOf(name: string): number {
            for (var i = 0; i < this.length; i++) {
                if ((<Axis>this[i]).name == name) {
                    return i;
                }
            }
            return -1;
        }
    }

    enum _tmInc {
        tickf7 = -7,
        tickf6 = -6,
        tickf5 = -5,
        tickf4 = -4,
        tickf3 = -3,
        tickf2 = -2,
        tickf1 = -1,
        second = 1,
        minute = second * 60,
        hour = minute * 60,
        day = hour * 24,
        week = day * 7,
        month = day * 31,
        year = day * 365,
        maxtime = Number.MAX_VALUE
    }

    class _timeSpan {
        private ticks: number;

        public static TicksPerSecond: number = 10000000;

        constructor(ticks: number) {
            this.ticks = ticks;
        }

        public get Ticks(): number {
            return this.ticks;
        }

        public get TotalSeconds(): number {
            return this.ticks / 10000000;
        }

        public get TotalDays(): number {
            return this.ticks / 10000000 / (24 * 60 * 60);
        }

        public static fromSeconds(seconds: number): _timeSpan {
            return new _timeSpan(seconds * 10000000);
        }

        public static fromDays(days: number): _timeSpan {
            return new _timeSpan(days * 10000000 * 24 * 60 * 60);
        }
    }

    class _timeHelper {
        year: number;
        month: number;
        day: number;
        hour: number;
        minute: number;
        second: number;

        private init(dt: Date) {
            this.year = dt.getFullYear();
            this.month = dt.getMonth();
            this.day = dt.getDate();
            this.hour = dt.getHours();
            this.minute = dt.getMinutes();
            this.second = dt.getSeconds();
        }

        constructor(date: any) {
            if (isDate(date))
                this.init(date);
            else if (isNumber(date))
                this.init(FlexChart._fromOADate(date));
        }

        getTimeAsDateTime(): Date {
            var smon = 0, sday = 0, ssec = 0;

            // N3CHT000043
            if (this.hour >= 24) {
                this.hour -= 24;
                this.day += 1;
            }

            if (this.month < 0) {
                smon = -1 - this.day;
                this.month = 1;
            }
            else if (this.month > 11) {
                smon = this.month - 12;
                this.month = 12;
            }

            if (this.day < 1) {
                sday = -1 - this.day;
                this.day = 1;
            }
            else if (this.day > 28 && this.month==2) {
                sday = this.day - 28;
                this.day = 28;
            }
            else if (this.day > 30 && (this.month == 4 || this.month == 4 || this.month == 6 || this.month == 9 || this.month == 11)) {
                sday = this.day - 30;
                this.day = 30;
            }
            else if (this.day > 31) {
                sday = this.day - 31;
                this.day = 31;
            }

            if (this.second > 59) {
                ssec = this.second - 59;
                this.second = 59;
            }

            var smin = 0;
            if (this.minute > 59) {
                smin = this.minute - 59;
                this.minute = 59;
            }

            return new Date(this.year, this.month, this.day, this.hour, this.minute, this.second);
            //AddDays(sday).AddMonths(smon).AddSeconds(ssec).AddMinutes(smin);
        }

        getTimeAsDouble(): number {
            return FlexChart._toOADate(this.getTimeAsDateTime());
        }

        static tround(tval: number, tunit: number, roundup: boolean): number {
            var test = ((tval / tunit) * tunit);
            test = test - (test % 1);
            if (roundup && test != tval) {
                tunit = tunit - (tunit % 1)
                test += tunit;
            }
            return test;
        }

        static RoundTime(timevalue: number, unit: number, roundup: boolean): number {
            //TimeSpan ts = TimeSpan.FromDays(unit);
            var tunit = unit * 24 * 60 * 60; // (long) ts.TotalSeconds;

            if (tunit > 0) {
                var th = new _timeHelper(timevalue);

                if (tunit < _tmInc.minute) {
                    th.second = this.tround(th.second, tunit, roundup);
                    return th.getTimeAsDouble();
                }

                th.second = 0;
                if (tunit < _tmInc.hour) {
                    tunit /= _tmInc.minute;
                    th.minute = this.tround(th.minute, tunit, roundup);
                    return th.getTimeAsDouble();
                }

                th.minute = 0;
                if (tunit < _tmInc.day) {
                    tunit /= _tmInc.hour;
                    th.hour = this.tround(th.hour, tunit, roundup);
                    return th.getTimeAsDouble();
                }

                th.hour = 0;
                if (tunit < _tmInc.month) {
                    tunit /= _tmInc.day;
                    th.day = this.tround(th.day, tunit, roundup);
                    return th.getTimeAsDouble();
                }

                th.day = 1;
                if (tunit < _tmInc.year) {
                    tunit /= _tmInc.month;

                    // Jan - is good enough
                    if (th.month != 1)
                        th.month = this.tround(th.month, tunit, roundup);
                    return th.getTimeAsDouble();
                }

                th.month = 1;
                tunit /= _tmInc.year;
                th.year = this.tround(th.year, tunit, roundup);
                return th.getTimeAsDouble();
            } else {
                // alext 26-Sep-03
                //double td = ts.TotalSeconds;
                var td = timevalue;

                var tx = td - tunit;
                var tz = ((tx / unit)) * unit;// alext 12-Sep-06 int -> long VNCHT000517
                if (roundup && tz != tx)
                    tz += unit;
                td = tunit + tz;
                return td;
            }
        }

        private static secInYear = (24 * 60 * 60);

        private static TimeSpanFromTmInc(ti: _tmInc): _timeSpan {
            var rv = _timeSpan.fromSeconds(1);

            if (ti != _tmInc.maxtime) {
                if (ti > _tmInc.tickf1) {
                    rv = _timeSpan.fromSeconds(ti);
                } else {
                    var rti = ti;
                    var ticks = 1;
                    rti += 7;	// rti is now power of 10 of number of Ticks
                    while (rti > 0) {
                        ticks *= 10;
                        rti--;
                    }
                    rv = new _timeSpan(ticks);
                }
            }
            return rv;
        }

        private static manualTimeInc(manualformat: string): _tmInc {
            var minSpan = _tmInc.second;

            // only interested in the lowest increment of the format,
            // so it is not necessary that the format be valid, but it
            // must exist as a string to process.
            if (manualformat == null || manualformat.length == 0)
                return minSpan;

            var f = manualformat.indexOf('f');
            if (f >= 0) {
                var rv = -1;
                if (f > 0 && manualformat.substr(f - 1, 1) == '%') {
                    rv = -1;
                } else {
                    for (var i = 1; i < 6; i++) {
                        // alext 26-Sep-03
                        if ((f + i) >= manualformat.length)
                            break;
                        //
                        var ss = manualformat.substr(f + i, 1);

                        if (ss != 'f')
                            break;

                        //if (!manualformat.Substring(f + i, 1).Equals('f'))
                        //  break;
                        rv--;
                    }
                }
                minSpan = rv;
            }
            else if (manualformat.indexOf('s') >= 0)
                minSpan = _tmInc.second;
            else if (manualformat.indexOf('m') >= 0)
                minSpan = _tmInc.minute;
            else if (manualformat.indexOf('h') >= 0 || manualformat.indexOf('H'))
                minSpan = _tmInc.hour;
            else if (manualformat.indexOf('d') >= 0)
                minSpan = _tmInc.day;
            else if (manualformat.indexOf('M') >= 0)
                minSpan = _tmInc.month;
            else if (manualformat.indexOf('y') >= 0)
                minSpan = _tmInc.year;

            return minSpan;
        }

        private static getNiceInc(tik: number[], ts: number, mult: number): number {
            for (var i = 0; i < tik.length; i++) {
                var tikm = tik[i] * mult;
                if (ts <= tikm)
                    return tikm;
            }
            return 0;
        }

        public static NiceTimeSpan(ts: _timeSpan, manualformat: string): _timeSpan {
            var minSpan = _tmInc.second;

            if (manualformat != null && manualformat.length > 0)
                minSpan = _timeHelper.manualTimeInc(manualformat);

            var tsinc = 0;
            var tinc = 0;

            // have the minimum required by format.
            if (minSpan < _tmInc.second) {
                // alext 10-Jan-2011
                //if (ts.TotalSeconds < 1.0)
                if (ts.TotalSeconds < 10.0)
                //
                {
                    tsinc = ts.Ticks;
                    tinc = _timeHelper.TimeSpanFromTmInc(minSpan).Ticks;

                    while (tsinc > 10 * tinc)
                        tinc *= 10;

                    // alext 10-Jan-2011
                    var tinc1 = tinc;
                    if (tsinc > tinc1)
                        tinc1 *= 2;
                    if (tsinc > tinc1)
                        tinc1 = 5 * tinc;
                    if (tsinc > tinc1)
                        tinc1 = 10 * tinc;

                    //
                    return new _timeSpan(tinc1);
                }
            }

            // alext 25-Jan-06 
            // when tsinc < ts the annos are overlapping
            // using larger integer 
            // tsinc = (long)ts.TotalSeconds;
            tsinc = Math.ceil(ts.TotalSeconds);

            if (tsinc == 0)
                return _timeHelper.TimeSpanFromTmInc(minSpan);

            tinc = 1;

            if (minSpan < _tmInc.minute) {
                // seconds
                if (tsinc < _tmInc.minute) {
                    tinc = _timeHelper.getNiceInc([1, 2, 5, 10, 15, 30], tsinc, minSpan); // alext 11-Mar-11 TimeSpanFromTmInc(minSpan).Ticks  /*(long)minSpan*/); // alext 25-Jan-06 added 2 as 'nice' number
                    if (tinc != 0) return _timeSpan.fromSeconds(tinc);
                }
                minSpan = _tmInc.minute;
            }
            if (minSpan < _tmInc.hour) {
                // minutes
                if (tsinc < _tmInc.hour) {
                    tinc = _timeHelper.getNiceInc([1, 2, 5, 10, 15, 30], tsinc, minSpan); // alext 25-Jan-06 added 2 as 'nice' number
                    if (tinc != 0) return _timeSpan.fromSeconds(tinc);
                }
                minSpan = _tmInc.hour;
            }
            if (minSpan < _tmInc.day) {
                // hours
                if (tsinc < _tmInc.day) {
                    tinc = _timeHelper.getNiceInc([1, 3, 6, 12], tsinc, minSpan);
                    if (tinc != 0) return _timeSpan.fromSeconds(tinc);
                }
                minSpan = _tmInc.day;
            }
            if (minSpan < _tmInc.month) {
                // days
                if (tsinc < _tmInc.month) {
                    tinc = _timeHelper.getNiceInc([1, 2, 7, 14], tsinc, minSpan);
                    if (tinc != 0) return _timeSpan.fromSeconds(tinc);
                }
                minSpan = _tmInc.month;
            }
            if (minSpan < _tmInc.year) {
                // months
                if (tsinc < _tmInc.year) {
                    tinc = _timeHelper.getNiceInc([1, 2, 3, 4, 6], tsinc, minSpan);
                    if (tinc != 0) return _timeSpan.fromSeconds(tinc);
                }
                minSpan = _tmInc.year;
            }

            // years
            tinc = 100 * _tmInc.year;
            if (tsinc < tinc) {
                tinc = _timeHelper.getNiceInc([1, 2, 5, 10, 20, 50], tsinc, minSpan);
                if (tinc == 0) tinc = 100 * _tmInc.year;
            }
            return _timeSpan.fromSeconds(tinc);
        }

        public static NiceTimeUnit(timeinc: number, manualformat: string): number {
            var tsRange = _timeSpan.fromDays(timeinc);
            tsRange = _timeHelper.NiceTimeSpan(tsRange, manualformat);
            return tsRange.TotalDays;
        }

        public static GetTimeDefaultFormat(maxdate: number, mindate: number): string {
            if (isNaN(maxdate) || isNaN(mindate)) {
                return '';
            }

            var format = 's';
            //DateTime amax = DateTime.FromOADate(maxdate);
            //DateTime amin = DateTime.FromOADate(mindate);

            var tsRange = _timeSpan.fromDays(maxdate - mindate); //amax.Subtract(amin);
            var range = tsRange.TotalSeconds;

            if (range > 2 * _tmInc.year) {
                format = 'yyyy';
            } else if (range > _tmInc.year) {
                format = 'MMM yy';
            } else if (range > 3 * _tmInc.month) {
                format = 'MMM';
            } else if (range > 2 * _tmInc.week) {
                format = 'MMM d';
            } else if (range > 2 * _tmInc.day) {
                format = 'ddd d';
            } else if (range > _tmInc.day) {
                format = 'ddd H:mm';
            } else if (range > _tmInc.hour) {
                format = 'H:mm';
            } else if (range >= 1) {
                format = 'H:mm:ss';
            } else if (range > 0) {
                var ticks = tsRange.Ticks;
                format = 's' + '.';//System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
                while (ticks < _timeSpan.TicksPerSecond) {
                    ticks *= 10;
                    format += 'f';
                }
            }

            return format;
        }
    }
}
module wijmo.chart {
    'use strict';

    /**
     * Specifies whether and where the Series is visible.
     */
    export enum SeriesVisibility {
        /** The series is visible on the plot and in the legend. */
        Visible,
        /** The series is visible only on the plot. */
        Plot,
        /** The series is visible only in the legend. */
        Legend,
        /** The series is hidden. */
        Hidden
    }

    /**
     * Specifies the type of marker to use for the @see:symbolMarker property.
     * Applies to Scatter, LineSymbols, and SplineSymbols chart types.
     */
    export enum Marker {
        /** 
         * Uses a circle to mark each data point.
         */
        Dot,
        /**
         * Uses a square to mark each data point.
         */
        Box
    };

    /**
    * Data series interface
    */
    export interface _ISeries {
        style: any;
        symbolStyle: any;
        getValues: (dim: number) => number[];
        getDataType: (dim: number) => DataType;
        //chartType: ChartType;

        drawLegendItem(engine: IRenderEngine, rect:Rect);
        measureLegendItem(engine: IRenderEngine):Size;
        _setPointIndex(pointIndex: number, elementIndex: number);
    }

    class DataArray
    {
        dataType: DataType;
        values: Array<number>;
    }

    /**
     * Provides arguments for @see:Series events.
     */
    export class SeriesEventArgs extends EventArgs {
        _series: Series;

        /**
         * Initializes a new instance of a @see:SeriesEventArgs object.
         *
         * @param series Specifies the @see:Series object affected by this event.
         */
        constructor(series: SeriesBase) {
            super();
            this._series = asType(series, SeriesBase);
        }

        /**
         * Gets the @see:Series object affected by this event.
         */
        get series(): SeriesBase {
            return this._series;
        }
    }

    /**
     * Represents a series of data points to display in the chart.
     *
     */
    export class SeriesBase implements _ISeries {
        static _LEGEND_ITEM_WIDTH = 10;
        static _LEGEND_ITEM_HEIGHT = 10;
        static _LEGEND_ITEM_MARGIN = 4;
        private static _DEFAULT_SYM_SIZE = 10;

        // property storage
        _chart: FlexChartCore;
        private _name: string;
        private _binding: string;
        private _showValues: boolean;
        private _symbolStyle: any;
        private _symbolSize: number;
        private _style: any;

        private _cv: wijmo.collections.ICollectionView;
        private _itemsSource: any;
        private _values: number[];
        private _valueDataType: DataType;
        _chartType: ChartType;
        private _symbolMarker: Marker = Marker.Dot;

        private _bindingX: string;
        private _xvalues: number[];
        private _xvalueDataType: DataType;
        private _cssClass: string;
        private _visibility: SeriesVisibility = SeriesVisibility.Visible;

        private _axisX: Axis;
        private _axisY: Axis;

        _legendElement: SVGAElement;
        _hostElement: SVGGElement;
        _pointIndexes: number[];

        constructor() {
        }

        //--------------------------------------------------------------------------
        // ** implementation 

        /**
         * Gets or sets the series style.
         */
        get style(): any {
            return this._style;
        }
        set style(value: any) {
            if (value != this._style) {
                this._style = value;
                this._invalidate();
            }
        }

        /**
         * Gets or sets the series symbol style.
         * Applies to Scatter, LineSymbols, and SplineSymbols chart types.
         */
        get symbolStyle(): any {
            return this._symbolStyle;
        }
        set symbolStyle(value: any) {
            if (value != this._symbolStyle) {
                this._symbolStyle = value;
                this._invalidate();
            }
        }

        /**
         * Gets or sets the size in pixels of the symbols used to render this @see:Series.
         * Applies to Scatter, LineSymbols, and SplineSymbols chart types.
         */
        get symbolSize(): number {
            return this._symbolSize;
        }
        set symbolSize(value: number) {
            if (value != this._symbolSize) {
                this._symbolSize = asNumber(value, true, true);
                this._invalidate();
            }
        }
        _getSymbolSize(): number {
            return this.symbolSize != null ? this.symbolSize : this.chart.symbolSize;
        }

        /**
         * Gets or sets the shape of marker to use for each data point in the series.
         * Applies to Scatter, LineSymbols, and SplineSymbols chart types.
         */
        get symbolMarker(): Marker {
            return this._symbolMarker;
        }
        set symbolMarker(value: Marker) {
            if (value != this._symbolMarker) {
                this._symbolMarker = asEnum(value, Marker, true);
                this._invalidate();
            }
        }

        /**
         * Gets or sets the name of the property that contains Y values for the series.
         */
        get binding(): string {
            return this._binding ? this._binding : this._chart ? this._chart.binding : null;
        }
        set binding(value: string) {
            if (value != this._binding) {
                this._binding = asString(value, true);
                this._clearValues();
                this._invalidate();
            }
        }

        /**
         * Gets or sets the name of the property that contains X values for the series.
         */
        get bindingX(): string {
            return this._bindingX ? this._bindingX : this._chart ? this._chart.bindingX : null;
        }
        set bindingX(value: string) {
            if (value != this._bindingX) {
                this._bindingX = asString(value, true);
                this._clearValues();
                this._invalidate();
            }
        }

        /**
         * Gets or sets the series name.
         * 
         * The series name is displayed in the chart legend. Any series without a name
         * does not appear in the legend.
         */
        get name(): string {
            return this._name;
        }
        set name(value: string) {
            this._name = value;
        }

        /**
         * Gets or sets the array or @see:ICollectionView object that contains the series data.
         */
        get itemsSource(): any {
            return this._itemsSource;
        }
        set itemsSource(value: any) {
            if (value != this._itemsSource) {

                // unbind current collection view
                if (this._cv) {
                    this._cv.currentChanged.removeHandler(this._cvCurrentChanged, this);
                    this._cv.collectionChanged.removeHandler(this._cvCollectionChanged, this);
                    this._cv = null;
                }

                // save new data source and collection view
                this._itemsSource = value;
                this._cv = asCollectionView(value);

                // bind new collection view
                if (this._cv != null) {
                    this._cv.currentChanged.addHandler(this._cvCurrentChanged, this);
                    this._cv.collectionChanged.addHandler(this._cvCollectionChanged, this);
                }

                this._clearValues();
                this._itemsSource = value;
                this._invalidate();
            }
        }

        /**
         * Gets the @see:ICollectionView object that contains the data for this series.
         */
        get collectionView(): wijmo.collections.ICollectionView {
            return this._cv ? this._cv : this._chart ? this._chart.collectionView : null;
        }

        /**
         * Gets the @see:FlexChart object that owns this series.
         */
         get chart(): FlexChartCore {
            return this._chart;
        }

        /**
         * Gets the series host element.
         */
        get hostElement(): SVGGElement {
            return this._hostElement;
        }

        /**
         * Gets the series element in the legend.
         */
        get legendElement(): SVGGElement {
            return this._legendElement;
        }

        /**
         * Gets or sets the series CSS class.
         */
        get cssClass(): string {
            return this._cssClass;
        }
        set cssClass(value: string) {
            this._cssClass = asString(value, true);
        }

        /**
         * Gets or sets an enumerated value indicating whether and where the series appears.
         */
        get visibility(): SeriesVisibility {
            return this._visibility;
        }
        set visibility(value: SeriesVisibility) {
            if (value != this._visibility) {
                this._visibility = asEnum(value, SeriesVisibility);
                this._clearValues();
                this._invalidate();
                if (this._chart) {
                    this._chart.onSeriesVisibilityChanged(new SeriesEventArgs(this));
                }
            }
        }

        /**
         * Occurs when series is rendering.
         */
        rendering = new Event();

        /**
         * Raises the @see:rendering event.
         *
         * @param engine The @see:IRenderEngine object used to render the series.
         */
        onRendering(engine: IRenderEngine) {
            this.rendering.raise(this, new RenderEventArgs(engine));
        }

        /**
         * Gets a @see:HitTestInfo object with information about the specified point.
         *
         * @param pt The point to investigate, in window coordinates.
         * @param y The Y coordinate of the point (if the first parameter is a number).
         */
        hitTest(pt: any, y?: number): HitTestInfo {
            if (isNumber(pt) && isNumber(y)) { // accept hitTest(x, y) as well
                pt = new Point(pt, y);
            } else if (pt instanceof MouseEvent) {
                pt = new Point(pt.pageX, pt.pageY);
            }
            asType(pt, Point);

            if (this._chart) {
                return this._chart._hitTestSeries(pt, this._chart.series.indexOf(this));
            }
            else {
                return null;
            }
        }

        /**
         * Gets the plot element that corresponds to the specified point index.
         *
         * @param pointIndex The index of the data point.
         */
        getPlotElement(pointIndex: number): any {
            if (this.hostElement) {
                if (pointIndex < this._pointIndexes.length) {
                    var elementIndex = this._pointIndexes[pointIndex];
                    if (elementIndex < this.hostElement.childNodes.length) {
                        return this.hostElement.childNodes[elementIndex];
                    }
                }
            }
            return null;
        }


        /**
         * Gets or sets the x-axis for the series.
         */
        get axisX(): Axis {
            return this._axisX;
        }
        set axisX(value: Axis) {
            if (value != this._axisX) {
                this._axisX = asType(value, Axis, true);
                if (this._axisX) {
                    var chart = this._axisX._chart = this._chart;
                    if (chart && chart.axes.indexOf(this._axisX) == -1) {
                        chart.axes.push(this._axisX);
                    }
                }
                this._invalidate();
            }
        }

        /**
         * Gets or sets the y-axis for the series.
         */
        get axisY(): Axis {
            return this._axisY;
        }
        set axisY(value: Axis) {
            if (value != this._axisY) {
                this._axisY = asType(value, Axis, true);
                if (this._axisY) {
                    var chart = this._axisY._chart = this._chart;
                    if (chart && chart.axes.indexOf(this._axisY) == -1) {
                        chart.axes.push(this._axisY);
                    }
                }
                this._invalidate();
            }
        }

        //--------------------------------------------------------------------------
        // ** implementation 

        getDataType(dim: number): DataType {
            if (dim == 0) {
                return this._valueDataType;
            }
            else if (dim == 1) {
                return this._xvalueDataType;
            }

            return null;
        }

        getValues(dim: number): number[] {
            if (dim == 0) {
                if (this._values == null) {
                    this._valueDataType = null;
                    if (this._cv != null) {
                        var da = this._bindValues(this._cv.items, this._getBinding(0));
                        this._values = da.values;
                        this._valueDataType = da.dataType;
                            }
                    else if (this.binding != null) {
                        if (this._chart != null && this._chart.collectionView!=null) {
                            var da = this._bindValues(this._chart.collectionView.items, this._getBinding(0));
                            this._values = da.values;
                            this._valueDataType = da.dataType;
                        }
                    }
                }
                return this._values;
            }
            else if (dim == 1) {
                if (this._xvalues == null) {
                    this._xvalueDataType = null;

                    var base: any = this;

                    if (this.bindingX != null) {
                    if (base._cv != null) {
                        var da = this._bindValues(base._cv.items, this.bindingX, true);
                        this._xvalueDataType = da.dataType;
                        this._xvalues = da.values;
                    }
                        else {
                            if (this._bindingX == null) {
                                return null;
                            }
                            
                        if (base._chart != null && base._chart.collectionView != null) {
                                var da = this._bindValues(base._chart.collectionView.items, this.bindingX, true);
                            this._xvalueDataType = da.dataType;
                            this._xvalues = da.values;
                            }
                        }
                    }
                }
                return this._xvalues;
            }

            return null;
        }

       /**
         * Draw a legend item at the specified position.
         *
         * @param engine The rendering engine to use.
         * @param rect The position of the legend item.
         */
        drawLegendItem(engine: IRenderEngine, rect: Rect) {
            var chartType = this._chartType;
            if (chartType == null) {
                chartType = this._chart._getChartType();
            }
            //var style = this.style;

            engine.strokeWidth = 1;

            var marg = Series._LEGEND_ITEM_MARGIN;

            var fill = null;
            var stroke = null;

            //if (style) {
            //    if (style.fill) {
            //        fill = style.fill;
            //    }
            //    if (style.stroke) {
            //        stroke = style.stroke;
            //}
            //}

            if (fill === null)
                fill = this._chart._getColorLight(this._chart.series.indexOf(this));
            if (stroke === null)
                stroke = this._chart._getColor(this._chart.series.indexOf(this));

            engine.fill = fill;
            engine.stroke = stroke;

            var yc = rect.top + 0.5 * rect.height;

            var wsym = Series._LEGEND_ITEM_WIDTH;
            var hsym = Series._LEGEND_ITEM_HEIGHT;
            switch (chartType) {
                case ChartType.Area:
                case ChartType.SplineArea:
                    {
                        engine.drawRect(rect.left + marg, yc - 0.5 * hsym, wsym, hsym, null, this.style);
                    }
                    break;
                case ChartType.Bar:
                case ChartType.Column:
                    {
                        engine.drawRect(rect.left + marg, yc - 0.5 * hsym, wsym, hsym, null, this.symbolStyle? this.symbolStyle : this.style);
                    }
                    break;
                case ChartType.Scatter:
                case ChartType.Bubble:
                    {
                        var rx = 0.3 * wsym;
                        var ry = 0.3 * hsym;
                        if (this.symbolMarker == Marker.Box) {
                            engine.drawRect(rect.left + marg + 0.5 * wsym - rx, yc - ry, 2 * rx, 2 * ry, null, this.symbolStyle);
                        } else {
                            engine.drawEllipse(rect.left + 0.5 * wsym + marg, yc, rx, ry, null, this.symbolStyle);
                        }
                    }
                    break;
                case ChartType.Line:
                case ChartType.Spline:
                    {
                        engine.drawLine(rect.left + marg, yc, rect.left + wsym + marg, yc, null, this.style);
                    }
                    break;
                case ChartType.LineSymbols:
                case ChartType.SplineSymbols:
                    {
                        var rx = 0.3 * wsym;
                        var ry = 0.3 * hsym;
                        if (this.symbolMarker == Marker.Box) {
                            engine.drawRect(rect.left + marg + 0.5 * wsym - rx, yc-ry, 2*rx, 2*ry, null, this.symbolStyle);
                        } else {
                            engine.drawEllipse(rect.left + 0.5 * wsym + marg, yc, rx, ry, null, this.symbolStyle);
                        }

                        engine.drawLine(rect.left + marg, yc, rect.left + wsym + marg, yc, null, this.style);
                    }
                    break;
                case ChartType.Candlestick:
                case ChartType.HighLowOpenClose:
                    {
                        engine.drawLine(rect.left + marg, yc, rect.left + wsym + marg, yc, null, this.symbolStyle ? this.symbolStyle : this.style);
                    }
                    break;
            }
            if (this._name) {
                FlexChart._renderText(engine, this._name, new Point(rect.left + hsym + 2 * marg, yc), 0, 1, FlexChart._CSS_LABEL);
        }
        }

        /**
         * Measures the height and width of the legend item.
         *
         * @param engine The rendering engine to use.
         */
        measureLegendItem(engine: IRenderEngine): Size {
            var sz = new Size();
            sz.width = Series._LEGEND_ITEM_WIDTH;
            sz.height = Series._LEGEND_ITEM_HEIGHT;
            if (this._name) {
                var tsz = engine.measureString(this._name, FlexChart._CSS_LABEL);
                sz.width += tsz.width;
                if (sz.height < tsz.height) {
                    sz.height = tsz.height;
                }
            };
            sz.width += 3 * Series._LEGEND_ITEM_MARGIN;
            sz.height += 2 * Series._LEGEND_ITEM_MARGIN;
            return sz;
        }

        /**
         * Returns series bounding rectangle in data coordinates.
         *
         * If getDataRect() returns null the limits are calculated automatically based on data values. 
         */
        getDataRect(): Rect {
            return null;
        }

        _getChartType(): ChartType {
            return this._chartType;
        }

        /**
         * Clears any cached data values.
         */
        _clearValues() {
            this._values = null;
            this._xvalues = null;
        }

        _getBinding(index: number): string {
            var binding = null;
            if (this.binding) {
                var props = this.binding.split(',');
                if (props && props.length > index) {
                    binding = props[index].trim();
                }
            }
            return binding;
        }

        _getBindingValues(index: number) {
            var items: any[];
            if (this._cv != null) {
                items = this._cv.items;
            }
            else if (this._chart != null && this._chart.collectionView != null) {
                    items =  this._chart.collectionView.items;
            }

            var da = this._bindValues(items, this._getBinding(index));
            return da.values;
        }

        _getItem(pointIndex: number): any {
            var item = null;
            var items = null;
            if (this.itemsSource != null) {
                if (this._cv != null)
                    items = this._cv.items;
                else 
                    items = this.itemsSource;
            }
            else if (this._chart.itemsSource != null) {
                if (this._chart.collectionView != null) {
                    items = this._chart.collectionView.items;
                } else {
                    items = this._chart.itemsSource;
            }
            }
            if (items != null) {
                item = items[pointIndex];
            }

            return item;
        }

        _getLength(): number {
            var len = null;
            if (this.itemsSource != null) {
                len = this.itemsSource.length;
            }
            else if (this._chart.itemsSource != null) {
                len = this._chart.itemsSource.length;
            }
            return len;
        }

        _setPointIndex(pointIndex: number, elementIndex: number)
        {
            this._pointIndexes[pointIndex] = elementIndex;
        }

        private _getDataRect(): Rect {
            var values = this.getValues(0);
            var xvalues = this.getValues(1);
            if (values) {
                var xmin = NaN,
                    ymin = NaN,
                    xmax = NaN,
                    ymax = NaN;

                var len = values.length; 

                for (var i = 0; i < len; i++) {
                    var val = values[i];
                    if (isFinite(val)) {
                        if (isNaN(ymin)) {
                            ymin = ymax = val;
                        } else {
                            if (val < ymin) {
                                ymin = val;
                            } else if (val > ymax) {
                                ymax = val;
                            }
                        }
                    }
                    if (xvalues) {
                        var xval = xvalues[i];
                        if (isFinite(xval)) {
                            if (isNaN(xmin)) {
                                xmin = xmax = xval;
                            } else {
                                if (xval < xmin) {
                                    xmin = xval;
                                } else if (val > ymax) {
                                    xmax = xval;
                                }
                            }
                        }
                    }
                }

                if (!xvalues) {
                    xmin = 0; xmax = len - 1;
                }

                if (!isNaN(ymin)) {
                    return new Rect(xmin, ymin, xmax - xmin, ymax - ymin);
                }
            } 

            return null;
        }

        _isCustomAxisX(): boolean {
            if (this._axisX) {
                if (this._chart) {
                    return this._axisX != this.chart.axisX;
                } else {
                    return true;
                }
            } else {
                return false;
            }
        }

        _isCustomAxisY(): boolean {
            if (this._axisY) {
                if (this._chart) {
                    return this._axisY != this.chart.axisY;
                } else {
                    return true;
                }
            } else {
                return false;
            }
        }

        _getAxisX(): Axis {
            var ax: Axis = null;
            if (this.axisX) {
                ax = this.axisX;
            } else if (this.chart) {
                ax = this.chart.axisX;
            }
            return ax;
        }

        _getAxisY(): Axis {
            var ay: Axis = null;
            if (this.axisY) {
                ay = this.axisY;
            } else if (this.chart) {
                ay = this.chart.axisY;
            } 
            return ay;
        }


        private _cvCollectionChanged(sender, e) {
            this._clearValues();
            this._invalidate();
        }

        // updates selection to sync with data source
        private _cvCurrentChanged(sender, e) {
            this._invalidate();
        }

        private _bindValues(items: Array<any>, binding: string, isX:boolean=false): DataArray {
            var values: Array<number>;
            var dataType: DataType;
            if (items != null) {
                var len = items.length;
                values = new Array<number>(items.length);

                for (var i = 0; i < len; i++) {
                    var val = items[i];
                    if (binding != null) {
                        val = val[binding];
                    }

                    if (isNumber(val)) {
                        values[i] = val;
                        dataType = DataType.Number;
                    }
                    else if (isDate(val)) {
                        values[i] = FlexChart._toOADate(val);
                        dataType = DataType.Date;
                    } else if(isX && val) {
                        // most likely it's category axis
                        // return appropriate values
                        values[i] = i;
                        dataType = DataType.Number;
                    }
                }
            }
            var darr = new DataArray();
            darr.values = values;
            darr.dataType = dataType;
            return darr;  
        }

        _invalidate() {
            if (this._chart) {
                this._chart.invalidate();
            }
        }

        _indexToPoint(pointIndex: number): Point {
            if (pointIndex >= 0 && pointIndex < this._values.length) {
                var y = this._values[pointIndex];
                var x = this._xvalues ? this._xvalues[pointIndex] : pointIndex;

                return new Point(x,y);
            }

            return null;
        }

        _getSymbolFill(seriesIndex?: number): string {
            var fill: string = null;
            if (this.symbolStyle) {
                fill = this.symbolStyle.fill;
            }
            if (!fill && this.style) {
                fill = this.style.fill;
            }
            if (!fill && this.chart) {
                fill = this.chart._getColorLight(seriesIndex);
            }
            return fill;
        }

        _getSymbolStroke(seriesIndex?: number): string {
            var stroke: string = null;
            if (this.symbolStyle) {
                stroke = this.symbolStyle.stroke;
            }
            if (!stroke && this.style) {
                stroke = this.style.stroke;
            }
            if (!stroke && this.chart) {
                stroke = this.chart._getColor(seriesIndex);
            }
            return stroke;
        }
    }
} 
module wijmo.chart {
    'use strict';

    /**
     * Represents a series of data points to display in the chart.
     *
     * The @see:Series class supports all basic chart types. You may define
     * a different chart type on each @see:Series object that you add to the 
     * @see:FlexChart series collection, and it overrides the @see:chartType 
     * property set on the chart that is the default for all @see:Series objects
     * in its collection.
     */
    export class Series extends SeriesBase {
        /**
         * Gets or sets the chart type for a specific series, overriding the chart type 
         * set on the overall chart. 
         */
        get chartType(): ChartType {
            return this._chartType;
        }
        set chartType(value: ChartType) {
            if (value != this._chartType) {
                this._chartType = asEnum(value, ChartType, true);
                this._invalidate();
            }
        }
    }
} 
module wijmo.chart
{
    'use strict';

    /**
     * Represents a rendering engine that performs basic drawing routines.
     */
    export interface IRenderEngine
    {
        /**
         * Clear the viewport and start the rendering cycle.
         */
        beginRender();

        /**
         * Finish the rendering cycle.
         */
        endRender();

        /**
         * Set the size of the viewport.
         * 
         * @param w Viewport width.
         * @param h Viewport height.
         */
        setViewportSize(w: number, h: number);

        /**
         * Gets or sets the color used to fill the element.
         */
        fill: string;

        /**
         * Gets or sets the color used to outline the element.
         */
        stroke: string;

        /**
         * Gets or sets the thickness of the outline.
         */
        strokeWidth: number;

        /**
         * Gets or sets the text color.
         */
        textFill: string;

        /**
         * Gets or sets the font size for text output.
         */
        fontSize: string;

        /**
         * Gets or sets the font family for text output.
        */
        fontFamily: string;

        drawEllipse(cx: number, cy: number, rx: number, ry: number, className?: string, style?: any);
        drawRect(x: number, y: number, w: number, h: number, className?: string, style?: any, clipPath?: string);
        drawLine(x1: number, y1: number, x2: number, y2: number, className?: string, style?: any);
        drawLines(xs: number[], ys: number[], className?: string, style?: any, clipPath?: string);
        drawSplines(xs: number[], ys: number[], className?: string, style?: any, clipPath?: string);

        drawPolygon(xs: number[], ys: number[], className?: string, style?: any, clipPath?: string);

        drawPieSegment(cx: number, cy: number, radius: number, startAngle: number, sweepAngle: number,
            className?: string, style?: any, clipPath?: string);
        drawDonutSegment(cx: number, cy: number, radius: number, innerRadius: number, startAngle: number, sweepAngle: number,
            className?: string, style?: any, clipPath?: string);
        
        drawString(s: string, pt: Point, className?: string, style?: any);
        drawStringRotated(label: string, pt: Point, center: Point, angle: number, className?: string, style?: any);
        
        drawImage(imageHref:string, x: number, y: number, w: number, h: number);

        measureString(s: string, className?: string, groupName?: string, style?: any): Size;

        startGroup(className?: string, clipPath?: string, createTransform?:boolean);
        endGroup();
        addClipRect(clipRect: Rect, id: string);
    }
 
}


module wijmo.chart {
    'use strict';

    /**
     * Render to svg.
     */
    export class _SvgRenderEngine implements IRenderEngine {
        private static svgNS = 'http://www.w3.org/2000/svg';
        private static xlinkNS = 'http://www.w3.org/1999/xlink';

        private _element: HTMLElement;
        private _svg: Element;
        private _text: SVGTextElement;
        private _textGroup: SVGGElement;
        private _defs: SVGDefsElement;

        // 
        private _fill: string;
        private _stroke: string;
        private _textFill: string;

        private _strokeWidth: number = 1;

        private _fontSize: string = null;
        private _fontFamily: string = null;

        private _group: Element;
        private _clipRect: Rect;

        constructor(element: HTMLElement) {
            this._element = element;
            this._create();
            this._element.appendChild(this._svg);
        }

        beginRender() {
            while (this._svg.firstChild) {
                this._svg.removeChild(this._svg.firstChild);
            }
            this._svg.appendChild(this._textGroup);
        }

        endRender() {
            if (this._textGroup.parentNode) {
            this._svg.removeChild(this._textGroup);
            }
        }

        setViewportSize(w: number, h: number) {
            this._svg.setAttribute('width', w.toString());
            this._svg.setAttribute('height', h.toString());
        }

        get fill(): string {
            return this._fill
        }
        set fill(value: string) {
            this._fill = value;
        }

        get fontSize(): string {
            return this._fontSize;
        }
        set fontSize(value: string) {
            this._fontSize = value;
        }

        get fontFamily(): string {
            return this._fontFamily;
        }
        set fontFamily(value: string) {
            this._fontFamily = value;
        }

        get stroke(): string {
            return this._stroke;
        }
        set stroke(value: string) {
            this._stroke = value;
        }

        get strokeWidth(): number {
            return this._strokeWidth;
        }
        set strokeWidth(value: number) {
            this._strokeWidth = value;
        }

        get textFill(): string {
            return this._textFill;
        }
        set textFill(value: string) {
            this._textFill = value;
        }

        addClipRect(clipRect: Rect, id:string) {
            if (clipRect && id) {
                var clipPath = document.createElementNS(_SvgRenderEngine.svgNS, 'clipPath');
                var rect = document.createElementNS(_SvgRenderEngine.svgNS, 'rect');
                rect.setAttribute( 'x', (clipRect.left-1).toFixed());
                rect.setAttribute( 'y', (clipRect.top-1).toFixed());
                rect.setAttribute( 'width', (clipRect.width+2).toFixed());
                rect.setAttribute( 'height', (clipRect.height+2).toFixed());
                clipPath.appendChild(rect);

                clipPath.setAttribute('id', id);

                this._svg.appendChild(clipPath);
                //this._defs.appendChild(clipPath);
            }
        }

        drawEllipse(cx: number, cy: number, rx: number, ry: number, className?: string, style?: any) {
            var ell = <SVGElement>document.createElementNS(_SvgRenderEngine.svgNS, 'ellipse');
            ell.setAttribute('stroke', this._stroke);
            if (this._strokeWidth !== null) {
                ell.setAttribute('stroke-width', this._strokeWidth.toString());
            }
            ell.setAttribute('fill', this._fill);
            ell.setAttribute('cx', cx.toFixed(1));
            ell.setAttribute('cy', cy.toFixed(1));
            ell.setAttribute('rx', rx.toFixed(1));
            ell.setAttribute('ry', ry.toFixed(1));
            //ell.setAttribute('cx', cx.toString());
            //ell.setAttribute('cy', cy.toString());
            //ell.setAttribute('rx', rx.toString());
            //ell.setAttribute('ry', ry.toString());

            if (className) {
                ell.setAttribute('class', className);
            }
            this._applyStyle(ell, style);

            //this._svg.appendChild(ell);
            this._appendChild(ell);
        }

        drawRect(x: number, y: number, w: number, h: number, className?: string, style?:any, clipPath?:string) {
            var rect = <SVGElement>document.createElementNS(_SvgRenderEngine.svgNS, 'rect');

            rect.setAttribute('fill', this._fill);
            rect.setAttribute('stroke', this._stroke);
            if (this._strokeWidth !== null) {
                rect.setAttribute('stroke-width', this._strokeWidth.toString());
            }
            rect.setAttribute('x', x.toFixed(1));
            rect.setAttribute('y', y.toFixed(1));
            rect.setAttribute('width', w.toFixed(1));
            rect.setAttribute('height', h.toFixed(1));
            //rect.setAttribute('x', x.toString());
            //rect.setAttribute('y', y.toString());
            //rect.setAttribute('width', w.toString());
            //rect.setAttribute('height', h.toString());
            if (clipPath) {
                rect.setAttribute('clip-path', 'url(#' + clipPath + ')');
            }

            if (className) {
                rect.setAttribute('class', className);
            }
            this._applyStyle(rect, style);

            this._appendChild(rect);
        }

         drawLine(x1: number, y1: number, x2: number, y2: number, className?: string, style?: any) {
            var line = <SVGAElement>document.createElementNS(_SvgRenderEngine.svgNS, 'line');
            line.setAttribute('stroke', this._stroke);
            if (this._strokeWidth !== null) {
                line.setAttribute('stroke-width', this._strokeWidth.toString());
            }
            line.setAttribute('x1', x1.toFixed(1));
            line.setAttribute('x2', x2.toFixed(1));
            line.setAttribute('y1', y1.toFixed(1));
            line.setAttribute('y2', y2.toFixed(1));
            //line.setAttribute('x1', x1.toString());
            //line.setAttribute('x2', x2.toString());
            //line.setAttribute('y1', y1.toString());
            //line.setAttribute('y2', y2.toString());

            if (className) {
                line.setAttribute('class', className);
            }
            this._applyStyle(line, style);

            this._appendChild(line);
        }

        drawLines(xs: number[], ys: number[], className?: string, style?: any, clipPath?: string) {
            if (xs && ys) {
                var len = Math.min(xs.length, ys.length);
                if (len > 0) {
                    var pline = <SVGElement>document.createElementNS(_SvgRenderEngine.svgNS, 'polyline');

                    pline.setAttribute('stroke', this._stroke);
                    if (this._strokeWidth !== null) {
                        pline.setAttribute('stroke-width', this._strokeWidth.toString());
                    }

                    pline.setAttribute('fill', 'none');
                    var spts = '';
                    for (var i = 0; i < len; i++) {
                        spts += xs[i].toFixed(1) + ',' + ys[i].toFixed(1) + ' ';
                        //spts += xs[i].toString() + ',' + ys[i].toString() + ' ';
                    }
                    pline.setAttribute('points', spts);

                    if (className) {
                        pline.setAttribute('class', className);
                    }
                    if (clipPath) {
                        pline.setAttribute('clip-path', 'url(#' + clipPath + ')');
                    }
                    this._applyStyle(pline, style);

                    this._appendChild(pline);
                }
            }
        }

        drawSplines(xs: number[], ys: number[], className?: string, style?: any, clipPath?: string) {
            if (xs && ys) {
                var spline = new _Spline(xs, ys);
                var s = spline.calculate();
                var sx = s.xs;
                var sy = s.ys;

                var len = Math.min(sx.length, sy.length);
                if (len > 0) {
                    var pline = <SVGElement>document.createElementNS(_SvgRenderEngine.svgNS, 'polyline');

                    pline.setAttribute('stroke', this._stroke);
                    if (this._strokeWidth !== null) {
                        pline.setAttribute('stroke-width', this._strokeWidth.toString());
                    }

                    pline.setAttribute('fill', 'none');
                    var spts = '';
                    for (var i = 0; i < len; i++) {
                        spts += sx[i].toFixed(1) + ',' + sy[i].toFixed(1) + ' ';
                    }
                    pline.setAttribute('points', spts);

                    if (className) {
                        pline.setAttribute('class', className);
                    }
                    if (clipPath) {
                        pline.setAttribute('clip-path', 'url(#' + clipPath + ')');
                    }
                    this._applyStyle(pline, style);

                    this._appendChild(pline);
                }
            }
        }

        drawPolygon(xs: number[], ys: number[], className?: string, style?: any, clipPath?: string) {
            if (xs && ys) {
                var len = Math.min(xs.length, ys.length);
                if (len > 0) {
                    var poly = <SVGElement>document.createElementNS(_SvgRenderEngine.svgNS, 'polygon');

                    poly.setAttribute('stroke', this._stroke);
                    if (this._strokeWidth !== null) {
                        poly.setAttribute('stroke-width', this._strokeWidth.toString());
                    }
                    poly.setAttribute('fill', this._fill);

                    var spts = '';
                    for (var i = 0; i < len; i++) {
                        //spts += xs[i].toString() + ',' + ys[i].toString() + ' ';
                        spts += xs[i].toFixed(1) + ',' + ys[i].toFixed(1) + ' ';
                    }
                    poly.setAttribute( 'points', spts);

                    if (className) {
                        poly.setAttribute('class', className);
                    }
                    if (clipPath) {
                        poly.setAttribute('clip-path', 'url(#' + clipPath + ')');
                    }
                    this._applyStyle(poly, style);

                    this._appendChild(poly);
                }
            }
        }

        drawPieSegment(cx: number, cy: number, r: number, startAngle: number, sweepAngle: number,
            className?: string, style?: any, clipPath?: string) {

            if (sweepAngle >= Math.PI * 2) {
                return this.drawEllipse(cx, cy, r, r, className, style);
            } 

            var path = <SVGElement>document.createElementNS(_SvgRenderEngine.svgNS, 'path');

            path.setAttribute('fill', this._fill);
            path.setAttribute('stroke', this._stroke);
            if (this._strokeWidth !== null) {
                path.setAttribute('stroke-width', this._strokeWidth.toString());
            }

            var p1 = new Point(cx, cy);
            p1.x += r * Math.cos(startAngle);
            p1.y += r * Math.sin(startAngle);

            var a2 = startAngle + sweepAngle;
            var p2 = new Point(cx, cy);
            p2.x += r * Math.cos(a2);
            p2.y += r * Math.sin(a2);

            var opt = ' 0 0,1 ';
            if (Math.abs(sweepAngle) > Math.PI) {
                opt = ' 0 1,1 ';
            }

            var d = 'M ' + cx.toFixed(1) + ',' + cy.toFixed(1);
            d += 'L ' + p1.x.toFixed(1) + ',' + p1.y.toFixed(1);
            d += ' A ' + r.toFixed(1) + ',' + r.toFixed(1) + opt;
            d += p2.x.toFixed(1) + ',' + p2.y.toFixed(1) + ' z';

            path.setAttribute('d', d);


            if (clipPath) {
                path.setAttribute('clip-path', 'url(#' + clipPath + ')');
            }

            if (className) {
                path.setAttribute('class', className);
            }
            this._applyStyle(path, style);

            this._appendChild(path);

        }

        drawDonutSegment(cx: number, cy: number, radius: number, innerRadius: number, startAngle: number, sweepAngle: number,
            className?: string, style?: any, clipPath?: string) {

            var isFull = false;
            if (sweepAngle >= Math.PI * 2) {
                isFull = true;
                sweepAngle -= 0.001;
            }
            var path = <SVGElement>document.createElementNS(_SvgRenderEngine.svgNS, 'path');

            path.setAttribute('fill', this._fill);
            path.setAttribute('stroke', this._stroke);
            if (this._strokeWidth !== null) {
                path.setAttribute('stroke-width', this._strokeWidth.toString());
            }

            var p1 = new Point(cx, cy);
            p1.x += radius * Math.cos(startAngle);
            p1.y += radius * Math.sin(startAngle);

            var a2 = startAngle + sweepAngle;
            var p2 = new Point(cx, cy);
            p2.x += radius * Math.cos(a2);
            p2.y += radius * Math.sin(a2);

            var p3 = new Point(cx, cy);
            p3.x += innerRadius * Math.cos(a2);
            p3.y += innerRadius * Math.sin(a2);

            var p4 = new Point(cx, cy);
            p4.x += innerRadius * Math.cos(startAngle);
            p4.y += innerRadius * Math.sin(startAngle);

            var opt1 = ' 0 0,1 ',
                opt2 = ' 0 0,0 ';
            if (Math.abs(sweepAngle) > Math.PI) {
                opt1 = ' 0 1,1 ';
                opt2 = ' 0 1,0 ';
            }

            var d = 'M ' + p1.x.toFixed(3) + ',' + p1.y.toFixed(3);

            d += ' A ' + radius.toFixed(3) + ',' + radius.toFixed(3) + opt1;
            d += p2.x.toFixed(3) + ',' + p2.y.toFixed(3);
            if (isFull) {
                d += ' M ' + p3.x.toFixed(3) + ',' + p3.y.toFixed(3);
            } else {
                d += ' L ' + p3.x.toFixed(3) + ',' + p3.y.toFixed(3);
            }
            d += ' A ' + innerRadius.toFixed(3) + ',' + innerRadius.toFixed(3) + opt2;
            d += p4.x.toFixed(3) + ',' + p4.y.toFixed(3);
            if (!isFull) {
                d += ' z';
            }

            path.setAttribute('d', d);

            if (clipPath) {
                path.setAttribute('clip-path', 'url(#' + clipPath + ')');
            }

            if (className) {
                path.setAttribute('class', className);
            }
            this._applyStyle(path, style);

            this._appendChild(path);
        }

        drawString(s: string, pt: Point, className?: string, style?: any) {
            var text = this._createText(pt, s);
            if (className) {
                text.setAttribute('class', className);
            }

            this._applyStyle(text, style);

            this._appendChild(text);

            var bb = text.getBBox();
            text.setAttribute( 'y', (pt.y - (bb.y + bb.height - pt.y)).toFixed(1));
        }

        drawStringRotated(s: string, pt: Point, center: Point, angle: number, className?: string, style?: any) {
            var text = this._createText(pt, s);
            if (className) {
                text.setAttribute('class', className);
            }
            this._applyStyle(text, style);

            var g = document.createElementNS(_SvgRenderEngine.svgNS, 'g');
            g.setAttribute('transform', 'rotate(' + angle.toFixed(1) + ',' + center.x.toFixed(1) + ',' + center.y.toFixed(1) + ')');
            //g.setAttribute('transform', 'rotate(' + angle.toString() + ',' + center.x.toString() + ',' + center.y.toString() + ')');
            g.appendChild(text);


            //this._svg.appendChild(g);
            this._appendChild(g);
            var bb = text.getBBox();
            text.setAttribute( 'y', (pt.y - (bb.y + bb.height - pt.y)).toFixed(1));
        }

        measureString(s: string, className?: string, groupName?: string, style?: any): Size {
            var sz = new Size(0, 0);

            if (this._fontSize) {
                this._text.setAttribute('font-size', this._fontSize);
            }
            if (this._fontFamily) {
                this._text.setAttribute('font-family', this._fontFamily);
            }
            if (className) {
                this._text.setAttribute('class', className);
            }
            if (groupName) {
                this._textGroup.setAttribute('class', groupName);
            }

            this._applyStyle(this._text, style);

            this._setText(this._text, s);

            var rect = this._text.getBBox();
            sz.width = rect.width;
            sz.height = rect.height;

            this._text.removeAttribute('font-size');
            this._text.removeAttribute('font-family');
            this._text.removeAttribute('class');

            if (style) {
                for (var key in style) {
                    this._text.removeAttribute(this._deCase(key));
                }
            }

            this._textGroup.removeAttribute('class');
            this._text.textContent = null;
            return sz;
        }

        startGroup(className?: string, clipPath?: string, createTransform:boolean=false) {
            var group = <SVGGElement>document.createElementNS(_SvgRenderEngine.svgNS, 'g');
            if (className) {
                group.setAttribute('class', className);
            }
            if (clipPath) {
                group.setAttribute('clip-path', 'url(#' + clipPath + ')');
            }
            this._appendChild(group);
            if (createTransform) {
                group.transform.baseVal.appendItem((<SVGSVGElement>this._svg).createSVGTransform());
            }
            this._group = group;
            return group;
        }

        endGroup() {
            if (this._group) {
                var parent = <Element>this._group.parentNode;
                if (parent == this._svg) {
                    this._group = null;
                } else {
                    this._group = parent;
                }
            }
        }

        drawImage(imageHref: string, x: number, y: number, w: number, h: number) {
            var img = <SVGGElement>document.createElementNS(_SvgRenderEngine.svgNS, 'image');

            img.setAttributeNS(_SvgRenderEngine.xlinkNS, 'href', imageHref);  
            img.setAttribute('x', x.toFixed(1));
            img.setAttribute('y', y.toFixed(1));
            img.setAttribute('width', w.toFixed(1));
            img.setAttribute('height', h.toFixed(1));

            this._appendChild(img);
        }

        private _appendChild(element: Element) {
            var group = this._group;
            if (!group) {
                group = this._svg;
            }
            group.appendChild(element);
        }

        private _create() {
            this._svg = document.createElementNS(_SvgRenderEngine.svgNS, 'svg');
            this._defs = <SVGDefsElement>document.createElementNS(_SvgRenderEngine.svgNS, 'defs');
            this._svg.appendChild(this._defs);
            this._text = this._createText(new Point(-1000, -1000), '');
            this._textGroup = <SVGGElement>document.createElementNS(_SvgRenderEngine.svgNS, 'g');
            this._textGroup.appendChild(this._text);
            this._svg.appendChild(this._textGroup);
        }

        private _setText(element: Element, s: string) {
            var text = s ? s.toString() : null;
            if (text && text.indexOf('tspan') >= 0) {
                try {
                    element.textContent = null;

                    // Parse the markup into valid nodes.
                    var dXML = new DOMParser();

                    //dXML.async = false;
                    // Wrap the markup into a SVG node to ensure parsing works.
                    var sXML = '<svg xmlns="http://www.w3.org/2000/svg\">' + text + '</svg>';
                    var svgDocElement = dXML.parseFromString(sXML, 'text/xml').documentElement;

                    // Now take each node, import it and append to this element.
                    var childNode = svgDocElement.firstChild;

                    while (childNode) {
                        element.appendChild(element.ownerDocument.importNode(childNode, true));
                        childNode = childNode.nextSibling;
                    }

                } catch (e) {
                    throw new Error('Error parsing XML string.');
                };
            }
            else {
                element.textContent = text;
            }
        }

        private _createText(pos: Point, text: string): SVGTextElement {
            var textel = document.createElementNS(_SvgRenderEngine.svgNS, 'text');

            this._setText(textel, text);
            textel.setAttribute('fill', this._textFill);
            textel.setAttribute('x', pos.x.toFixed(1));
            textel.setAttribute('y', pos.y.toFixed(1));
            //textel.setAttribute('x', pos.x.toString());
            //textel.setAttribute('y', pos.y.toString());

            if (this._fontSize) {
                textel.setAttribute('font-size', this._fontSize);
            }
            if (this._fontFamily) {
                textel.setAttribute('font-family', this._fontFamily);
            }
            return <SVGTextElement>textel;
        }

        private _applyStyle(el: SVGElement, style: any) {
            if (style) {
                for (var key in style) {
                    el.setAttribute(this._deCase(key), style[key]);
                }
            }
        }

        private _deCase(s:string):string {
            return s.replace(/[A-Z]/g, function (a) {return '-' + a.toLowerCase() });
        }
    }
}


module wijmo.chart {
    'use strict';

    /**
     * Represents the chart legend.
     */
    export class Legend {
        _chart: FlexChartBase;
        _position: Position = Position.Right;
        private _areas = new Array<Rect>();
        private _sz: Size = new Size();

        constructor(chart: FlexChartBase) {
            this._chart = chart;
        }

        //--------------------------------------------------------------------------
        //** object model

        /**
         * Gets or sets the enumerated value that determines whether and where the 
         * legend appears in relation to the chart.
         */
        get position(): Position {
            return this._position;
        }
        set position(value: Position) {
            if (this._position != value) {
                this._position = asEnum(value, Position);
                this._chart.invalidate();
            }
        }

        //--------------------------------------------------------------------------
        //** implementation

        _getDesiredSize(engine: IRenderEngine): Size {

            // no legend? no size.
            var pos = this.position;
            if (pos == Position.None) {
                return null;
            }

            var isVertical = pos == Position.Right || pos == Position.Left;

            if (this._chart instanceof wijmo.chart.FlexChartCore) {
                this._sz = this._getDesiredSizeSeriesChart(engine, isVertical);
            } else if (this._chart instanceof wijmo.chart.FlexPie) {
                this._sz = this._getDesiredSizePieChart(engine, isVertical);
            } else {
                return null;
            }

            return this._sz;
        }

        private _getDesiredSizeSeriesChart(engine: IRenderEngine, isVertical:boolean): Size {
            // measure all series
            var sz = new Size();
            var arr = (<FlexChartCore>this._chart).series;
            var len = arr.length;
            for (var i = 0; i < len; i++) {

                // get the series
                var series = tryCast(arr[i], wijmo.chart.SeriesBase);

                // skip hidden series and series with no names
                var vis = series.visibility;
                if (!series.name || vis == SeriesVisibility.Hidden || vis == SeriesVisibility.Plot) {
                    continue;
                }

                // measure the legend
                var isz = series.measureLegendItem(engine);
                if (isVertical) {
                    sz.height += isz.height;
                    if (sz.width < isz.width) {
                        sz.width = isz.width;
                    }
                } else {
                    sz.width += isz.width;
                    if (sz.height < isz.height) {
                        sz.height = isz.height;
                    }
                }
            }

            return sz;
        }

        private _renderSeriesChart(engine: IRenderEngine, pos: Point, isVertical:boolean) {
            var arr = (<FlexChartCore>this._chart).series;
            var len = arr.length;

            // draw legend items
            for (var i = 0; i < len; i++) {

                // get the series
                var series = tryCast(arr[i], wijmo.chart.SeriesBase);
                if (!series) {
                    continue;
                }

                // skip hidden series and series with no names
                var vis = series.visibility;
                if (!series.name || vis == SeriesVisibility.Hidden || vis == SeriesVisibility.Plot) {
                    series._legendElement = null;
                    continue;
                }

                // create legend item
                var sz = series.measureLegendItem(engine);
                var rect = new Rect(pos.x, pos.y, sz.width, sz.height);
                var g = engine.startGroup(series.cssClass);
                if (vis == SeriesVisibility.Legend) {
                    g.setAttribute('opacity', '0.5');
                    series._legendElement = g;
                    series.drawLegendItem(engine, rect);
                } else if (vis == SeriesVisibility.Visible) {
                    series._legendElement = g;
                    series.drawLegendItem(engine, rect);
                } else {
                    series._legendElement = null;
                }

                // done, move on to next item
                engine.endGroup();
                this._areas.push(rect);
                if (isVertical) {
                    pos.y += sz.height;
                } else {
                    pos.x += sz.width;
                }
            }
        }

        private _getDesiredSizePieChart(engine: IRenderEngine, isVertical: boolean): Size {
            var sz = new Size();
            var pieChart = <FlexPie>this._chart; 
            var labels = pieChart._labels;
            var len = labels.length;
            for (var i = 0; i < len; i++) {
                // measure the legend
                var isz = pieChart._measureLegendItem(engine, labels[i]);
                if (isVertical) {
                    sz.height += isz.height;
                    if (sz.width < isz.width) {
                        sz.width = isz.width;
                    }
                } else {
                    sz.width += isz.width;
                    if (sz.height < isz.height) {
                        sz.height = isz.height;
                    }
                }
            }
            return sz;
        }

        private _renderPieChart(engine: IRenderEngine, pos: Point, isVertical: boolean) {
            var pieChart = <FlexPie>this._chart;
            var labels = pieChart._labels;
            var len = labels.length;

            // draw legend items
            for (var i = 0; i < len; i++) {

                var sz = pieChart._measureLegendItem(engine, labels[i]);
                var rect = new Rect(pos.x, pos.y, sz.width, sz.height);
                pieChart._drawLegendItem(engine, rect, i, labels[i]);

                this._areas.push(rect);
                if (isVertical) {
                    pos.y += sz.height;
                } else {
                    pos.x += sz.width;
                }
            }
        }


        _render(engine: IRenderEngine, pos: Point) {
            this._areas = [];
            var isVertical = this.position == Position.Right || this.position == Position.Left;

            // draw legend area
            engine.fill = 'transparent';
            engine.stroke = null;
            engine.drawRect(pos.x, pos.y, this._sz.width, this._sz.height);

            if (this._chart instanceof wijmo.chart.FlexChartCore) {
                this._renderSeriesChart(engine, pos, isVertical);
            } else if (this._chart instanceof wijmo.chart.FlexPie) {
                this._renderPieChart(engine, pos, isVertical);
            } else {
                return null;
            }
        }

        _hitTest(pt: Point): number {
            var areas = this._areas;
            for (var i = 0; i < areas.length; i++) {
                if (FlexChartCore._contains(areas[i], pt))
                    return i;
            }

            return null;
        }
    }
} 
module wijmo.chart
{
    'use strict';

    /**
     * The enumerated type of chart element that may be found by the hitTest method.
     */
    export enum ChartElement {
        /** The area within the axes. */
        PlotArea,
        /** X-axis. */
        AxisX,
        /** Y-axis. */
        AxisY,
        /** The area within the control but outside of the axes. */
        ChartArea,
        /** The chart legend. */
        Legend,
        /** The chart header. */
        Header,
        /** The chart footer. */
        Footer,
        /** A chart series. */
        Series,
        /** A chart series symbol. */
        SeriesSymbol,
        /** No chart element. */
        None
    };

    /**
     * Contains information about a part of a @see:FlexChart control at 
     * a specified page coordinate.
     */
    export class HitTestInfo
    {
        private _chart: FlexChartBase;
        private _pt: Point;

        _series: SeriesBase;
        _pointIndex: number = null;
        _chartElement: ChartElement = ChartElement.None;
        _dist: number;
        _item: any;
        private _x: any;
        private __xfmt: string;
        private _y: any;
        private __yfmt: string;

        /**
         * Initializes a new instance of a @see:HitTestInfo object.
         *
         * @param chart The chart control.
         * @param point The original point in window coordinates.
         */
        constructor(chart: FlexChartBase, point: Point)
        {
            this._chart = chart;
            this._pt = point;
        }

        /**
         * Gets the point in control coordinates that this HitTestInfo object refers to.
         */
        get point(): Point {
            return this._pt;
        }

        /**
         * Gets the chart series at the specified coordinates.
         */
        get series(): SeriesBase {
            return this._series;
        }

        /**
         * Gets the data point index at the specified coordinates.
         */
        get pointIndex(): number {
            return this._pointIndex;
        }

        /**
         * Gets the chart element at the specified coordinates.
         */
        get chartElement(): ChartElement {
            return this._chartElement;
        }

        /**
         * Gets the distance from the closest data point.
         */
        get distance(): number {
            return this._dist;
        }

        /**
         * Gets the data object that corresponds to the closest data point.
         */
        get item(): any {
            if (this._item == null)
            {
                //this._item = null;

                if (this.pointIndex !== null)
                {
                    if (this.series != null) {
                    this._item = this.series._getItem(this.pointIndex);
                    } else if (this._chart instanceof FlexPie) {
                        var pchart = <FlexPie>this._chart;
                        var items = null;
                        if (pchart._cv != null) {
                            items = pchart._cv.items;
                        } else {
                            items = pchart.itemsSource;
                        }
                        if (items && this.pointIndex < items.length) {
                            this._item = items[this.pointIndex];
                        }
                    }
                }
            }
            return this._item;
        }

        /**
         * Gets the x-value of the closest data point.
         */
        get x(): any {
            if (this._x === undefined) {
                this._x = this._getValue(1, false);
            }
            return this._x;
        }

        /**
         * Gets the y-value of the closest data point.
         */
        get y(): any {
            if (this._y === undefined) {
                this._y = this._getValue(0, false);
            }
            return this._y;
        }

        get value(): any {
            if (this._chart instanceof FlexPie) {
                var pchart = <FlexPie>this._chart;
                return pchart._values[this.pointIndex];
            } else {
                return this.y;
            }
        }

        get name(): any {
            if (this._chart instanceof FlexPie) {
                var pchart = <FlexPie>this._chart;
                return pchart._labels[this.pointIndex];
            } else {
                return this.series.name;
            }
        }

        // formatted x-value
        get _xfmt(): any {
            if (this.__xfmt === undefined) {
                this.__xfmt = this._getValue(1, true);
            }
            return this.__xfmt;
        }

        // formatted y-value
        get _yfmt(): any {
            if (this.__yfmt === undefined) {
                this.__yfmt = this._getValue(0, true);
            }
            return this.__yfmt;
        }

        // y: index=0
        // x: index=1
        private _getValue(index: number, formatted: boolean): any
        {
            if (this._chart instanceof FlexPie) {
                var pchart = <FlexPie>this._chart;
                return pchart._values[this.pointIndex];
            }

            // todo: rotated charts?

            var val = null,
                chart = <FlexChart>this._chart,
                pi = this.pointIndex;

            if (this.series !== null && pi !== null) {
                var vals = this.series.getValues(index); // xvalues
                var type = this.series.getDataType(index);

                // normal values
                if (vals && this.pointIndex < vals.length) {
                    val = vals[this.pointIndex];
                    if (type == DataType.Date && !formatted) {
                        val = FlexChart._fromOADate(val);
                    }
                } else if (index == 1) {
                    // category axis
                    if (chart._xlabels && chart._xlabels.length > 0 && pi < chart._xlabels.length) {
                        val = chart._xlabels[pi];
                        // automatic axis values
                    } else if (chart._xvals && pi < chart._xvals.length) {
                        val = chart._xvals[pi];
                        if (chart._xDataType == DataType.Date && !formatted) {
                            val = FlexChart._fromOADate(val);
                        }
                    }
                }
            }
            if (val !== null && formatted) {
                if (index == 0) {
                    val = chart.axisY._formatValue(val);
                } else if(index == 1) {
                    val = chart.axisX._formatValue(val);
                }
            }

            return val;
        }
    }

}


module wijmo.chart
{
    'use strict';

    /**
     * These are predefined color palettes for chart @see:Series objects.
     *
     * To create custom color palettes, supply an array of strings or rgba values.
     *
     * You can specify palettes for @see:FlexChart and @see:FlexPie controls.
     * For example:
     *
     * <pre>chart.palette = Palettes.light;</pre>
     *
     * The following palettes are pre-defined:
     * <ul>
     *   <li>standard (default)</li>
     *   <li>cocoa</li>
     *   <li>coral</li>
     *   <li>dark</li>
     *   <li>highcontrast</li>
     *   <li>light</li>
     *   <li>midnight</li>
     *   <li>minimal</li>
     *   <li>modern</li>
     *   <li>organic</li>
     *   <li>slate</li>
     * </ul>
     */
    export class Palettes {
        static standard = ['#88bde6', '#fbb258', '#90cd97', '#f6aac9', '#bfa554', '#bc99c7', '#eddd46', '#f07e6e', '#8c8c8c'];
        static cocoa = ['#466bb0', '#c8b422', '#14886e', '#b54836', '#6e5944', '#8b3872', '#73b22b', '#b87320', '#141414'];
        static coral = ['#84d0e0', '#f48256', '#95c78c', '#efa5d6', '#ba8452', '#ab95c2', '#ede9d0', '#e96b7d', '#888888'];
        static dark = ['#005fad', '#f06400', '#009330', '#e400b1', '#b65800', '#6a279c', '#d5a211', '#dc0127', '#000000'];
        static highcontrast = ['#ff82b0', '#0dda2c', '#0021ab', '#bcf28c', '#19c23b', '#890d3a', '#607efd', '#1b7700', '#000000'];
        static light = ['#ddca9a', '#778deb', '#778deb', '#b5eae2', '#7270be', '#a6c7a7', '#9e95c7', '#95b0c7', '#9b9b9b'];
        static midnight = ['#83aaca', '#e37849', '#14a46a', '#e097da', '#a26d54', '#a584b7', '#d89c54', '#e86996', '#2c343b'];
        static modern = ['#2d9fc7', '#ec993c', '#89c235', '#e377a4', '#a68931', '#a672a6', '#d0c041', '#e35855', '#68706a'];
        static organic = ['#9c88d9', '#a3d767', '#8ec3c0', '#e9c3a9', '#91ab36', '#d4ccc0', '#61bbd8', '#e2d76f', '#80715a'];
        static slate = ['#7493cd', '#f99820', '#71b486', '#e4a491', '#cb883b', '#ae83a4', '#bacc5c', '#e5746a', '#505d65'];
        static zen = ['#7bb5ae', '#e2d287', '#92b8da', '#eac4cb', '#7b8bbd', '#c7d189', '#b9a0c8', '#dfb397', '#a9a9a9'];
        static cyborg = ['#2a9fd6', '#77b300', '#9933cc', '#ff8800', '#cc0000', '#00cca3', '#3d6dcc', '#525252', '#000000'];
        static superhero = ['#5cb85c', '#f0ad4e', '#5bc0de', '#d9534f', '#9f5bde', '#46db8c', '#b6b86e', '#4e5d6c', '#2b3e4b'];
        static flatly = ['#18bc9c', '#3498db', '#f39c12', '#6cc1be', '#99a549', '#8f54b5', '#e74c3c', '#8a9899', '#2c3e50'];
        static darkly = ['#375a7f', '#00bc8c', '#3498db', '#f39c12', '#e74c3c', '#8f61b3', '#b08725', '#4a4949', '#000000'];
        static cerulan = ['#033e76', '#87c048', '#59822c', '#53b3eb', '#fc6506', '#d42323', '#e3bb00', '#cccccc', '#222222'];
    } 
}


module wijmo.chart {
    'use strict';

    /**
     * Calculates Spline curves.
     */
    export class _Spline {
        // 
        private k = 0.002;

        private _x;
        private _y;

        private _a = [];
        private _b = [];
        private _c = [];
        private _d = [];

        private _len: number;

        //  T^3     -1     +3    -3    +1     /
        //  T^2     +2     -5     4    -1    /
        //  T^1     -1      0     1     0   /  2
        //  T^0      0      2     0     0  /

        private m =
        [
            [-1 * 0.5, +3 * 0.5, -3 * 0.5, +1 * 0.5],
            [+2 * 0.5, -5 * 0.5, +4 * 0.5, -1 * 0.5],
            [-1 * 0.5, 0, +1 * 0.5, 0],
            [0, +2 * 0.5, 0, 0],
        ];

        //public Point[] Points
        //{
        //    get { return _pts; }
        //}

        constructor(x: number[], y: number[]) {
            this._x = x;
            this._y = y;

            var len = this._len = Math.min(x.length, y.length);

            if (len > 3) {
                for (var i = 0; i < len - 1; i++) {
                    var p1 = (i == 0) ? new Point(x[i], y[i]) : new Point(x[i - 1], y[i - 1]);
                    var p2 = new Point(x[i], y[i]);
                    var p3 = new Point(x[i + 1], y[i + 1]);
                    var p4 = (i == len - 2) ? new Point(x[i + 1], y[i + 1]) : new Point(x[i + 2], y[i + 2]);

                    var a = new Point();
                    var b = new Point();
                    var c = new Point();
                    var d = new Point();

                    a.x = p1.x * this.m[0][0] + p2.x * this.m[0][1] + p3.x * this.m[0][2] + p4.x * this.m[0][3];
                    b.x = p1.x * this.m[1][0] + p2.x * this.m[1][1] + p3.x * this.m[1][2] + p4.x * this.m[1][3];
                    c.x = p1.x * this.m[2][0] + p2.x * this.m[2][1] + p3.x * this.m[2][2] + p4.x * this.m[2][3];
                    d.x = p1.x * this.m[3][0] + p2.x * this.m[3][1] + p3.x * this.m[3][2] + p4.x * this.m[3][3];

                    a.y = p1.y * this.m[0][0] + p2.y * this.m[0][1] + p3.y * this.m[0][2] + p4.y * this.m[0][3];
                    b.y = p1.y * this.m[1][0] + p2.y * this.m[1][1] + p3.y * this.m[1][2] + p4.y * this.m[1][3];
                    c.y = p1.y * this.m[2][0] + p2.y * this.m[2][1] + p3.y * this.m[2][2] + p4.y * this.m[2][3];
                    d.y = p1.y * this.m[3][0] + p2.y * this.m[3][1] + p3.y * this.m[3][2] + p4.y * this.m[3][3];

                    this._a.push(a);
                    this._b.push(b);
                    this._c.push(c);
                    this._d.push(d);
                }
            }
        }

        private calculatePoint(val: number): any {
            var i = Math.floor(val);

            if (i < 0) {
                i = 0;
            }

            if (i > this._len - 2) {
                i = this._len - 2;
            }

            var d = val - i;

            var x = ((this._a[i].x * d + this._b[i].x) * d + this._c[i].x) * d + this._d[i].x;
            var y = ((this._a[i].y * d + this._b[i].y) * d + this._c[i].y) * d + this._d[i].y;

            return { x: x, y: y };
        }

        calculate() {
            if (this._len <= 3) {
                return { xs: this._x, ys: this._y };
            }

            var xs = [];
            var ys = [];

            var p0 = this.calculatePoint(0);
            xs.push(p0.x);
            ys.push(p0.y);

            var delta = this._len * this.k;
            var d = 3;

            for (var i = delta; ; i += delta) {
                var p = this.calculatePoint(i);

                if (Math.abs(p0.x - p.x) >= d || Math.abs(p0.y - p.y) >= d) {
                    xs.push(p.x);
                    ys.push(p.y)
                    p0 = p;
                }

                if (i > this._len - 1)
                    break;
            }

            return { xs: xs, ys: ys };
        }
    }
}
module wijmo.chart
{
    'use strict';

    /**
     * Specifies the position of data labels on the chart.
     */
    export enum LabelPosition {
        /** No data labels appear. */
        None = 0,
        /** The labels appear to the left of the data points. */
        Left = 1,
        /** The labels appear above the data points. */
        Top = 2,
        /** The labels appear to the right of the data points. */
        Right = 3,
        /** The labels appear below the data points. */
        Bottom = 4,
        /** The labels appear centered on the data points. */
        Center = 5
    };

    /**
     * Specifies the position of data labels on the pie chart.
     */
    export enum PieLabelPosition {
        /** No data labels. */
        None = 0,
        /** The label appears inside the pie slice. */
        Inside = 1,
        /** The item appears at the center of the pie slice. */
        Center = 2,
        /** The item appears outside the pie slice. */
        Outside = 3
    };

    /**
    * Base abstract class for the @see:DataLabel and @see:PieDataLabel classes.
    */
    export class DataLabelBase {
        private _content: any;
        _chart: FlexChartBase;
        private _bdr: boolean;

        /**
         * Gets or sets the content of data labels.
         * 
         * The content can be specified as a string or as a function that
         * takes a @see:HitTestInfo object as a parameter. 
         *
         * When the label content is a string, it can contain any of the following
         * parameters:
         *
         * <ul>
         *  <li><b>seriesName</b>: The name of the series that contains the data point (FlexChart only).</li>
         *  <li><b>pointIndex</b>: The index of the data point.</li>
         *  <li><b>value</b>: The <b>value</b> of the data point.</li>
         *  <li><b>x</b>: The <b>x</b>-value of the data point (FlexChart only).</li>
         *  <li><b>y</b>: The <b>y</b>-value of the data point (FlexChart only).</li>
         *  <li><b>name</b>: The <b>name</b> of the data point.</li>
         * </ul>
         * 
         * The parameter must be enclosed in curly brackets, for example 'x={x}, y={y}'.
         *
         * In the following example, we show the y value of the data point in the labels.
         *
         * <pre> 
         *  // Create a chart and show y data in labels positioned above the data point.
         *  var chart = new wijmo.chart.FlexChart('#theChart');          
         *  chart.initialize({
         *      itemsSource: data,
         *      bindingX: 'country',
         *      series: [
         *          { name: 'Sales', binding: 'sales' },
         *          { name: 'Expenses', binding: 'expenses' },
         *          { name: 'Downloads', binding: 'downloads' }],                            
         *  });
         *  chart.dataLabel.position = "Top";
         *  chart.dataLabel.content = "{y}";
         * </pre>
         *
         * The next example shows how to set data label content using a function.
         *
         * <pre> 
         *  // Set data label content 
         *  chart.dataLabel.content = function (ht) {
         *    return ht.name + ":" + ht.value.toFixed();
         *  }
         * </pre>
         *
         */
        get content(): any {
            return this._content;
        }
        set content(value: any) {
            if (value != this._content) {
                this._content = value;
                this._invalidate();
            }
        }
        /**
         * Gets or sets a value indicating whether data labels have borders.
         */
        get border(): boolean {
            return this._bdr;
        }
        set border(value: boolean) {
            if (value != this._bdr) {
                this._bdr = asBoolean(value, true);
                this._invalidate();
            }
        }

        _invalidate() {
            if (this._chart) {
                this._chart.invalidate();
            }
        }
    } 


    /**
     * The point data label for FlexChart.
     */
    export class DataLabel extends DataLabelBase {
        private _pos = LabelPosition.Top;

        /**
         * Gets or sets the position of data labels.
         */
        get position(): LabelPosition {
            return this._pos;
        }
        set position(value: LabelPosition) {
            if (value != this._pos) {
                this._pos = asEnum(value, LabelPosition);
                this._invalidate();
            }
        }
    } 

    /**
     * The point data label for FlexPie.
     */
    export class PieDataLabel extends DataLabelBase {
        private _pos = PieLabelPosition.None;

        /**
         * Gets or sets the position of the data labels.
         */
        get position(): PieLabelPosition {
            return this._pos;
        }
        set position(value: PieLabelPosition) {
            if (value != this._pos) {
                this._pos = asEnum(value, PieLabelPosition);
                this._invalidate();
            }
        }
    }
}


module wijmo.chart {
    'use strict';

    class LineMarkers {
        private _markers;
        private _bindMoveMarker;

        constructor() {
            this._markers = [];
            this._bindMoveMarker = this._moveMarker.bind(this);
        }

        attach(marker: LineMarker) {
            var hostEle = marker.chart.hostElement,
                markers = this._markers,
                len, arr;
            if ('markerIndex' in hostEle.dataset) {
                markers[hostEle.dataset['markerIndex']].push(marker);
            } else {
                len = markers.length,
                arr = [marker];
                markers.push(arr);
                hostEle.dataset['markerIndex'] = len;
                this._bindMoveEvent(hostEle);
            }
        }

        detach(marker: LineMarker) {
            var hostEle = marker.chart.hostElement,
                markers = this._markers,
                idx, arr: LineMarker[];

            if ('markerIndex' in hostEle.dataset) {
                arr = <LineMarker[]>markers[hostEle.dataset['markerIndex']];
                idx = arr.indexOf(marker);
                if (idx > -1) {
                    arr.splice(idx, 1);
                }
                if (arr.length === 0) {
                    idx = markers.indexOf(arr);
                    if (idx > -1) {
                        markers[idx] = undefined;
                    }
                    this._unbindMoveEvent(hostEle);
                }
            }
        }

        _moveMarker = function (e) {
            var dom = e.currentTarget,
                markers = this._markers,
                arr;

            if ('markerIndex' in dom.dataset) {
                arr = markers[dom.dataset['markerIndex']];
                arr.forEach(function (marker) {
                    marker._moveMarker(e);
                });
            }
        }

        _unbindMoveEvent(ele: Element) {
            var _moveMarker = this._bindMoveMarker;

            ele.removeEventListener('mousemove', _moveMarker);
            if ('ontouchstart' in window) {
                ele.removeEventListener('touchmove', _moveMarker);
            }
        }

        _bindMoveEvent(ele: Element) {
            var _moveMarker = this._bindMoveMarker;

            ele.addEventListener('mousemove', _moveMarker);
            if ('ontouchstart' in window) {
                ele.addEventListener('touchmove', _moveMarker);
            }
        }
    }

    var lineMarkers = new LineMarkers();

    /**
     * Specifies the line type for the LineMarker.
     */
    export enum LineMarkerLines {
        /** Show no lines. */
        None,
        /** Show a vertical line. */
        Vertical,
        /** Show a horizontal line. */
        Horizontal,
        /** Show both vertical and horizontal lines. */
        Both
    }

    // TODO: Implement drag interaction.
    // Drag 
    /**
     * Specifies how the LineMarker interacts with the user.
     */
    export enum LineMarkerInteraction {
        /** No interaction, the user specifies the position by clicking. */
        None,
        /** The LineMarker moves with the pointer. */
        Move,
        /** The LineMarker moves when the user drags the line. */
        Drag
    }

    //Binary
    //Right 0 -> 0, Left 1 -> 1, Bottom 4 -> 100, Top 6 -> 110
    /**
     * Specifies the alignment of the LineMarker.
     */
    export enum LineMarkerAlignment {
        /** 
         * The LineMarker alignment adjusts automatically so that it stays inside the 
         * boundaries of the plot area. */
        Auto = 2,
        /** The LineMarker aligns to the right of the pointer. */
        Right = 0,
        /** The LineMarker aligns to the left of the pointer. */
        Left = 1,
        /** The LineMarker aligns to the bottom of the pointer. */
        Bottom = 4,
        /** The LineMarker aligns to the top of the pointer. */
        Top = 6
    }

    /**
     * Represents an extension of the LineMarker for the FlexChart.
     *
     * The LineMarker consists of a text area with content reflecting data point 
     * values, and an optional vertical or horizontal line (or both for a cross-hair 
     * effect) positioned over the plot area. It can be static (interaction = None), 
     * follow the mouse or touch position (interaction = Move), or move when the user
     * drags the line (interaction = Drag).
     * For example:
     * <pre>
     *   // create an interactive marker with a horizontal line and y-value
     *   var lm = new wijmo.chart.LineMarker($scope.ctx.chart, {
     *       lines: wijmo.chart.LineMarkerLines.Horizontal,
     *       interaction: wijmo.chart.LineMarkerInteraction.Move,
     *       alignment : wijmo.chart.LineMarkerAlignment.Top
     *   });
     *   lm.content = function (ht) {
     *       // show y-value
     *       return lm.y.toFixed(2);
     *   }
     * </pre>
     */
    export class LineMarker {

        static _CSS_MARKER = 'wj-chart-linemarker';
        static _CSS_MARKER_HLINE = 'wj-chart-linemarker-hline';
        static _CSS_MARKER_VLINE = 'wj-chart-linemarker-vline';
        static _CSS_MARKER_CONTENT = 'wj-chart-linemarker-content';
        static _CSS_MARKER_CONTAINER = 'wj-chart-linemarker-container';
        static _CSS_LINE_DRAGGABLE = 'wj-chart-linemarker-draggable';

        private _chart: FlexChart;
        private _plot: SVGGElement;
        private _marker: HTMLElement;
        private _markerContainer: HTMLElement;
        private _markerContent: HTMLElement;
        private _dragEle: HTMLElement;
        private _hLine: HTMLElement;
        private _vLine: HTMLElement;
        private _plotRect: Rect;
        private _targetPoint: Point;
        private _wrapperMoveMarker;
        private _capturedEle: HTMLElement;
        private _wrapperMousedown = null;
        private _wrapperMouseup = null;
        private _contentDragStartPoint: Point;
        private _mouseDownCrossPoint: Point;

        // object model
        private _isVisible: boolean;
        private _horizontalPosition: number;
        private _verticalPosition: number;
        private _alignment: LineMarkerAlignment;
        private _content: Function;
        private _seriesIndex: number;
        private _lines: LineMarkerLines;
        private _interaction: LineMarkerInteraction;
        private _dragThreshold: number;
        private _dragContent: boolean;
        private _dragLines: boolean;

        /**
         * Initializes a new instance of a @see:LineMarker object.
         * 
         * @param chart The chart on which the LineMarker appears.
         * @param options A JavaScript object containing initialization data for the control.  
         */
        constructor(chart: FlexChart, options?) {
            var self = this;

            self._chart = chart;
            chart.rendered.addHandler(self._initialize, self);
            self._resetDefaultValue();
            wijmo.copy(this, options);
            self._initialize();
        }

        //--------------------------------------------------------------------------
        //** object model

        /**
         * Gets the @see:FlexChart object that owns the LineMarker.
         */
        get chart(): FlexChart {
            return this._chart;
        }

        /**
         * Gets or sets the visibility of the LineMarker.
         */
        get isVisible(): boolean {
            return this._isVisible;
        }
        set isVisible(value: boolean) {
            var self = this;

            if (value === self._isVisible) {
                return;
            }
            self._isVisible = asBoolean(value);
            if (!self._marker) {
                return;
            }
            self._toggleVisibility();
        }

        /**
         * Gets or sets the index of the series in the chart in which the LineMarker appears.
         * This takes effect when the @see:interaction property is set to 
         * wijmo.chart.LineMarkerInteraction.Move or wijmo.chart.LineMarkerInteraction.Drag.
         */
        get seriesIndex(): number {
            return this._seriesIndex;
        }
        set seriesIndex(value: number) {
            var self = this;

            if (value === self._seriesIndex) {
                return;
            }
            self._seriesIndex = asNumber(value, true);
        }

        /**
         * Gets or sets the horizontal position of the LineMarker relative to the plot area. 
         * 
         * Its value range is (0, 1).
         * If the value is null or undefined and @see:interaction is set to wijmo.chart.LineMarkerInteraction.Move  
         * or wijmo.chart.LineMarkerInteraction.Drag, the horizontal position of the marker is calculated automatically based on the pointer's position.
         */
        get horizontalPosition(): number {
            return this._horizontalPosition;
        }
        set horizontalPosition(value: number) {
            var self = this;

            if (value === self._horizontalPosition) {
                return;
            }
            self._horizontalPosition = asNumber(value, true);
            if (self._horizontalPosition < 0 || self._horizontalPosition > 1) {
                throw 'horizontalPosition\'s value should be in (0, 1).';
            }
            if (!self._marker) {
                return;
            }
            self._updateMarkerPosition();
        }

        /**
         * Gets the current x-value as chart data coordinates.
         */
        get x(): number {
            var self = this,
                len = self._targetPoint.x - self._plotRect.left,
                axis = self._chart.axisX;

            return axis.convertBack(len);
        }

        /**
         * Gets the current y-value as chart data coordinates.
         */
        get y(): number {
            var self = this,
                len = self._targetPoint.y - self._plotRect.top,
                axis = self._chart.axisY;

            return axis.convertBack(len);
        }

        /**
         * Gets or sets the content function that allows you to customize the text content of the LineMarker.
         */
        get content(): Function {
            return this._content;
        }
        set content(value: Function) {
            if (value === this._content) {
                return;
            }
            this._content = asFunction(value);
            this._updateMarkerPosition();
        }

        /**
         * Gets or sets the vertical position of the LineMarker relative to the plot area. 
         * 
         * Its value range is (0, 1).
         * If the value is null or undefined and @see:interaction is set to wijmo.chart.LineMarkerInteraction.Move 
         * or wijmo.chart.LineMarkerInteraction.Drag, the vertical position of the LineMarker is calculated automatically based on the pointer's position.
         */
        get verticalPosition(): number {
            return this._verticalPosition;
        }
        set verticalPosition(value: number) {
            var self = this;

            if (value === self._verticalPosition) {
                return;
            }
            self._verticalPosition = asNumber(value, true);
            if (self._verticalPosition < 0 || self._verticalPosition > 1) {
                throw 'verticalPosition\'s value should be in (0, 1).';
            }
            if (!self._marker) {
                return;
            }
            self._updateMarkerPosition();
        }

        /**
         * Gets or sets the alignment of the LineMarker content.
         * 
         * By default, the LineMarker shows to the right, at the bottom of the target point.
         * Use '|' to combine alignment values.
         * 
         * <pre>
         * // set the alignment to the left.
         * marker.alignment = wijmo.chart.LineMarkerAlignment.Left;
         * // set the alignment to the left top.
         * marker.alignment = wijmo.chart.LineMarkerAlignment.Left | wijmo.chart.LineMarkerAlignment.Top;
         * </pre>
         */
        get alignment(): LineMarkerAlignment {
            return this._alignment;
        }
        set alignment(value: LineMarkerAlignment) {
            var self = this;

            if (value === self._alignment) {
                return;
            }
            self._alignment = value;
            if (!self._marker) {
                return;
            }
            self._updatePositionByAlignment();
        }

        /**
         * Gets or sets the visibility of the LineMarker lines.
         */
        get lines(): LineMarkerLines {
            return this._lines;
        }
        set lines(value: LineMarkerLines) {
            var self = this;
            if (value === self._lines) {
                return;
            }
            self._lines = asEnum(value, LineMarkerLines);
            if (!self._marker) {
                return;
            }
            self._resetLinesVisibility();
        }

        /**
         * Gets or sets the interaction mode of the LineMarker.
         */
        get interaction(): LineMarkerInteraction {
            return this._interaction;
        }
        set interaction(value: LineMarkerInteraction) {
            var self = this;
            if (value === self._interaction) {
                return;
            }
            if (self._marker) {
                self._detach();
            }
            self._interaction = asEnum(value, LineMarkerInteraction);
            if (self._marker) {
                self._attach();
            }
            self._toggleElesDraggableClass(self._interaction === LineMarkerInteraction.Drag);
        }

        /**
            Gets or sets the maximum distance from the horizontal or vertical line that the marker can be dragged.
        */
        get dragThreshold(): number {
            return this._dragThreshold;
        }
        set dragThreshold(value: number) {
            if (value != this._dragThreshold) {
                this._dragThreshold = asNumber(value);
            }
        }

        /**
            Gets or sets a value indicating whether the content of the marker is draggable when the interaction mode is "Drag."
        */
        get dragContent(): boolean {
            return this._dragContent;
        }
        set dragContent(value: boolean) {
            var self = this;
            if (value !== self._dragContent) {
                self._dragContent = asBoolean(value);
            }
            toggleClass(self._dragEle, LineMarker._CSS_LINE_DRAGGABLE,
                self._interaction === LineMarkerInteraction.Drag &&
                self._dragContent &&
                self._lines !== LineMarkerLines.None);
        }

        /**
            Gets or sets a value indicating whether the lines are linked when the horizontal or vertical line is dragged when the interaction mode is "Drag."
        */
        get dragLines(): boolean {
            return this._dragLines;
        }
        set dragLines(value: boolean) {
            if (value != this._dragLines) {
                this._dragLines = asBoolean(value);
            }
        }

        /**
         * Occurs after the LineMarker's position changes.
         */
        positionChanged = new Event();

        /**
         * Raises the @see:positionChanged event.
         *
         * @param point The target point at which to show the LineMarker.
         */
        onPositionChanged(point: Point) {
            this.positionChanged.raise(this, point);
        }

        //--------------------------------------------------------------------------
        //** implementation

        /**
         * Removes the LineMarker from the chart.
         */
        remove() {
            var self = this,
                chart = self._chart;
            if (self._marker) {
                chart.rendered.removeHandler(self._initialize, self);
                self._detach();
                self._removeMarker();
                self._wrapperMoveMarker = null;
                self._wrapperMousedown = null;
                self._wrapperMouseup = null;
            }
        }

        private _attach() {
            var self = this;

            lineMarkers.attach(self);
            self._attachDrag();
        }

        private _attachDrag() {
            var self = this;

            if (self._interaction !== LineMarkerInteraction.Drag) {
                return;
            }

            if (!self._wrapperMousedown) {
                self._wrapperMousedown = self._onMousedown.bind(self);
            }
            if (!self._wrapperMouseup) {
                self._wrapperMouseup = self._onMouseup.bind(self);
            }
            // Drag mode
            self._toggleDragEventAttach(true);
        }

        private _detach() {
            var self = this;

            lineMarkers.detach(self);
            self._detachDrag();
        }

        private _detachDrag() {
            var self = this;

            if (self._interaction !== LineMarkerInteraction.Drag) {
                return;
            }

            // Drag mode
            self._toggleDragEventAttach(false);
        }

        private _toggleDragEventAttach(isAttach: boolean) {
            var self = this,
                chartHostEle = self._chart.hostElement,
                eventListener = isAttach ? 'addEventListener' : 'removeEventListener';

            chartHostEle[eventListener]('mousedown', self._wrapperMousedown);
            document[eventListener]('mouseup', self._wrapperMouseup);

            if ('ontouchstart' in window) {
                chartHostEle[eventListener]('touchstart', self._wrapperMousedown);
            }

            if ('ontouchend' in window) {
                document[eventListener]('touchend', self._wrapperMouseup);
            }
        }

        private _onMousedown(e) {
            var self = this, pt = self._getEventPoint(e),
                hRect, vRect, contentRect;

            if (self._interaction !== LineMarkerInteraction.Drag) {
                return;
            }

            hRect = getElementRect(self._hLine);
            vRect = getElementRect(self._vLine);
            contentRect = getElementRect(self._markerContent);

            if (self._dragContent &&
                self._pointInRect(pt, contentRect)) {
                self._capturedEle = self._markerContent;
                self._contentDragStartPoint = new Point(pt.x, pt.y);
                self._mouseDownCrossPoint = new Point(self._targetPoint.x, self._targetPoint.y);
            } else if ((Math.abs(hRect.top - pt.y) <= self._dragThreshold) ||
                (Math.abs(pt.y - hRect.top - hRect.height) <= self._dragThreshold) ||
                (pt.y >= hRect.top && pt.y <= hRect.top + hRect.height)) {
                self._capturedEle = self._hLine;
                self._contentDragStartPoint = undefined;
                addClass(self._chart.hostElement, LineMarker._CSS_LINE_DRAGGABLE);
            } else if (Math.abs(vRect.left - pt.x) <= self._dragThreshold ||
                (Math.abs(pt.x - vRect.left - vRect.width) <= self._dragThreshold) ||
                (pt.x >= vRect.left && pt.x <= vRect.left + vRect.width)) {
                self._capturedEle = self._vLine;
                self._contentDragStartPoint = undefined;
                addClass(self._chart.hostElement, LineMarker._CSS_LINE_DRAGGABLE);
            }

            e.preventDefault();
        }

        private _onMouseup(e) {
            var self = this,
                needReAlignment = self._alignment === LineMarkerAlignment.Auto
                && self._capturedEle === self._markerContent && self._lines !== LineMarkerLines.None;

            self._capturedEle = undefined;
            self._contentDragStartPoint = undefined;
            self._mouseDownCrossPoint = undefined;
            if (needReAlignment) {
                // because the size of content has changed, so need to adjust the position twice.
                self._updatePositionByAlignment();
                self._updatePositionByAlignment();
            }
            removeClass(self._chart.hostElement, LineMarker._CSS_LINE_DRAGGABLE);
            e.preventDefault();
        }

        _moveMarker(e) {
            var self = this,
                chart = self._chart,
                point = self._getEventPoint(e),
                plotRect = self._plotRect,
                isDragAction = self._interaction === LineMarkerInteraction.Drag,
                hLineVisible = self._lines === LineMarkerLines.Horizontal,
                vLineVisible = self._lines === LineMarkerLines.Vertical,
                seriesIndex = self._seriesIndex,
                series: wijmo.chart.Series,
                offset = getElementRect(chart.hostElement),
                hitTest, xAxis, yAxis, x, y;
            if (!self._isVisible || self._interaction === LineMarkerInteraction.None ||
                (self._interaction === LineMarkerInteraction.Drag &&
                (!self._capturedEle || self._lines === LineMarkerLines.None))) {
                return;
            }

            if (isDragAction) {
                if (self._contentDragStartPoint) {
                    point.x = hLineVisible ? self._targetPoint.x :
                            self._mouseDownCrossPoint.x + point.x - self._contentDragStartPoint.x;
                    point.y = vLineVisible ? self._targetPoint.y :
                            self._mouseDownCrossPoint.y + point.y - self._contentDragStartPoint.y;
                } else if (hLineVisible ||
                    (!self._dragLines && self._capturedEle === self._hLine)) {
                    // horizontal hine dragging
                    point.x = self._targetPoint.x;
                } else if (vLineVisible ||
                     (!self._dragLines && self._capturedEle === self._vLine)) {
                    // vertical hine dragging
                    point.y = self._targetPoint.y;
                }
            }

            if ((isDragAction && self._lines === LineMarkerLines.Horizontal) ||
                 (!self._dragLines && self._capturedEle === self._hLine)) {
                if (point.y <= plotRect.top || point.y >= plotRect.top + plotRect.height) {
                    return;
                }
            } else if ((isDragAction && self._lines === LineMarkerLines.Vertical) ||
                (!self._dragLines && self._capturedEle === self._vLine)) {
                if (point.x <= plotRect.left || point.x >= plotRect.left + plotRect.width) {
                    return;
                }
            } else {
                if (point.x <= plotRect.left || point.y <= plotRect.top
                    || point.x >= plotRect.left + plotRect.width
                    || point.y >= plotRect.top + plotRect.height) {
                    return;
                }
            }

            if (seriesIndex != null && seriesIndex >= 0 && seriesIndex < chart.series.length) {
                series = chart.series[seriesIndex];
                hitTest = series.hitTest(new Point(point.x, NaN));
                if (hitTest == null || hitTest.x == null || hitTest.y == null) {
                    return;
                }
                xAxis = series.axisX || chart.axisX;
                yAxis = series._getAxisY();
                x = isDate(hitTest.x) ? FlexChart._toOADate(hitTest.x) : hitTest.x;
                y = isDate(hitTest.y) ? FlexChart._toOADate(hitTest.y) : hitTest.y;
                point.x = xAxis.convert(x) + offset.left;
                point.y = yAxis.convert(y) + offset.top;
            }
            self._updateMarkerPosition(point);
            e.preventDefault();
        }

        private _show(ele?: HTMLElement) {
            var e = ele ? ele : this._marker;
            e.style.display = 'block';
        }

        private _hide(ele?: HTMLElement) {
            var e = ele ? ele : this._marker;
            e.style.display = 'none';
        }

        private _toggleVisibility() {
            this._isVisible ? this._show() : this._hide();
        }

        private _resetDefaultValue() {
            var self = this;

            self._isVisible = true;
            self._alignment = LineMarkerAlignment.Auto;
            self._lines = LineMarkerLines.None;
            self._interaction = LineMarkerInteraction.None;
            self._horizontalPosition = null;
            self._verticalPosition = null;
            self._content = null;
            self._seriesIndex = null;
            self._dragThreshold = 15;
            self._dragContent = false;
            self._dragLines = false;

            self._targetPoint = new Point();
        }

        private _initialize() {
            var self = this,
                plot = <SVGGElement>self._chart.hostElement.querySelector("." + FlexChart._CSS_PLOT_AREA),
                box;

            self._plot = plot;
            if (!self._marker) {
                self._createMarker();
            }
            if (plot) {
                self._plotRect = getElementRect(plot);

                box = plot.getBBox();
                self._plotRect.width = box.width;
                self._plotRect.height = box.height;
                self._updateMarkerSize();
                self._updateLinesSize();
            }
            self._updateMarkerPosition();
            self._wrapperMoveMarker = self._moveMarker.bind(self);
            self._attach();
        }

        private _createMarker() {
            var self = this,
                marker: HTMLElement,
                container: HTMLElement;

            marker = document.createElement('div');
            addClass(marker, LineMarker._CSS_MARKER);

            container = self._getContainer();
            container.appendChild(marker);

            self._markerContainer = container;
            self._marker = marker;

            self._createChildren();
        }

        private _removeMarker() {
            var self = this,
                mc = self._markerContainer;

            mc.removeChild(self._marker);
            self._content = null;
            self._hLine = null;
            self._vLine = null;

            if (!mc.hasChildNodes()) {
                self._chart.hostElement.removeChild(self._markerContainer);
                self._markerContainer = null;
            }
            self._marker = null;
        }

        private _getContainer(): HTMLElement {
            var container = <HTMLElement>this._chart.hostElement.querySelector(LineMarker._CSS_MARKER_CONTAINER);

            if (!container) {
                container = this._createContainer();
            }
            return container;
        }

        private _createContainer(): HTMLElement {
            var markerContainer = document.createElement('div'),
                hostEle = this._chart.hostElement;

            addClass(markerContainer, LineMarker._CSS_MARKER_CONTAINER);
            hostEle.insertBefore(markerContainer, hostEle.firstChild);

            return markerContainer;
        }

        private _createChildren() {
            var self = this,
                marker = self._marker,
                markerContent: HTMLElement, hline: HTMLElement, vline: HTMLElement, dragEle: HTMLElement;

            // work around for marker content touchmove: 
            // when the content is dynamic element, the touchmove fire only once.
            dragEle = document.createElement('div');
            dragEle.style.position = 'absolute';
            dragEle.style.height = '100%';
            dragEle.style.width = '100%';
            marker.appendChild(dragEle);
            self._dragEle = dragEle;
            //content
            markerContent = document.createElement('div');
            addClass(markerContent, LineMarker._CSS_MARKER_CONTENT);
            marker.appendChild(markerContent);
            self._markerContent = markerContent;
            // lines
            hline = document.createElement('div');
            addClass(hline, LineMarker._CSS_MARKER_HLINE);
            marker.appendChild(hline);
            self._hLine = hline;
            vline = document.createElement('div');
            addClass(vline, LineMarker._CSS_MARKER_VLINE);
            marker.appendChild(vline);
            self._vLine = vline;
            self._toggleElesDraggableClass(self._interaction === LineMarkerInteraction.Drag);
            self._resetLinesVisibility();
        }

        private _toggleElesDraggableClass(draggable: boolean) {
            var self = this;
            toggleClass(self._hLine, LineMarker._CSS_LINE_DRAGGABLE, draggable);
            toggleClass(self._vLine, LineMarker._CSS_LINE_DRAGGABLE, draggable);
            toggleClass(self._dragEle, LineMarker._CSS_LINE_DRAGGABLE, draggable &&
                self._dragContent && self._lines !== LineMarkerLines.None);
        }

        private _updateMarkerSize() {
            var self = this,
                plotRect = self._plotRect,
                chartEle = self._chart.hostElement,
                computedStyle = window.getComputedStyle(chartEle, null),
                chartRect = getElementRect(chartEle);

            if (!self._marker) {
                return;
            }
            self._marker.style.marginTop = (plotRect.top - chartRect.top - (parseFloat(computedStyle.getPropertyValue('padding-top')) || 0)) + 'px';
            self._marker.style.marginLeft = (plotRect.left - chartRect.left - (parseFloat(computedStyle.getPropertyValue('padding-left')) || 0)) + 'px';
        }

        private _updateLinesSize() {
            var self = this,
                plotRect = self._plotRect;

            if (!self._hLine || !self._vLine) {
                return;
            }

            self._hLine.style.width = plotRect.width + 'px';
            self._vLine.style.height = plotRect.height + 'px';
        }

        private _resetLinesVisibility() {
            var self = this;

            if (!self._hLine || !self._vLine) {
                return;
            }

            self._hide(self._hLine);
            self._hide(self._vLine);
            if (self._lines === LineMarkerLines.Horizontal || self._lines === LineMarkerLines.Both) {
                self._show(self._hLine);
            }
            if (self._lines === LineMarkerLines.Vertical || self._lines === LineMarkerLines.Both) {
                self._show(self._vLine);
            }
        }

        private _updateMarkerPosition(point?: Point) {
            var self = this,
                plotRect = self._plotRect,
                targetPoint = self._targetPoint,
                x, y, raiseEvent = false,
                isDragAction = self._interaction === LineMarkerInteraction.Drag;

            if (!self._plot) {
                return;
            }

            x = plotRect.left + plotRect.width * (self._horizontalPosition || 0);
            y = plotRect.top + plotRect.height * (self._verticalPosition || 0);

            if (self._horizontalPosition == null && point) {
                x = point.x;
            }
            if (self._verticalPosition == null && point) {
                y = point.y;
            }

            if (x !== targetPoint.x || y !== targetPoint.y) {
                raiseEvent = true;
            }

            targetPoint.x = x;
            targetPoint.y = y;
            self._toggleVisibility();
            if (self._content) {
                self._updateContent();
            }

            if (raiseEvent) {
                self._raisePositionChanged(x, y);
            }

            // after the content changed(size changed), then update the marker's position
            self._updatePositionByAlignment(point ? true : false);
        }

        private _updateContent() {
            var self = this,
                chart = self._chart,
                point = self._targetPoint,
                hitTestInfo = chart.hitTest(point),
                text;

            text = self._content.call(null, hitTestInfo, point);
            self._markerContent.innerHTML = text || '';
        }

        private _raisePositionChanged(x: number, y: number) {
            var plotRect = this._plotRect;

            this.onPositionChanged(new Point(x, y));
            //this.onPositionChanged(new Point(x - plotRect.left, y - plotRect.top));
        }

        private _updatePositionByAlignment(isMarkerMoved?: boolean) {
            var self = this,
                align = self._alignment,
                tp = self._targetPoint,
                marker = self._marker,
                topBottom = 0, leftRight = 0,
                width = marker.clientWidth,
                height = marker.clientHeight,
                plotRect = self._plotRect,
                //offset for right-bottom lnkemarker to avoid mouse overlapping.
                offset = 12;

            if (!self._plot) {
                return;
            }

            if (!self._capturedEle || (self._capturedEle && self._capturedEle !== self._markerContent)) {
                if (align === LineMarkerAlignment.Auto) {
                    if (tp.x + width + offset > plotRect.left + plotRect.width) {
                        leftRight = width;
                    }
                    //set default auto to right top.
                    topBottom = height;
                    if (tp.y - height < plotRect.top) {
                        topBottom = 0;
                    }
                } else {
                    if ((1 & align) === 1) {//left
                        leftRight = width;
                    }
                    if ((2 & align) === 2) {//Top
                        topBottom = height;
                    }
                }
                //only add offset when interaction is move and alignment is right bottom
                if (self._interaction === LineMarkerInteraction.Move && topBottom === 0 && leftRight === 0) {
                    leftRight = -offset;
                }
            } else {
                //content dragging: when the content is on top position
                if (parseInt(self._hLine.style.top) > 0) {
                    topBottom = height;
                }
                //content dragging: when the content is on left position
                if (parseInt(self._vLine.style.left) > 0) {
                    leftRight = width;
                }
            }

            marker.style.left = (tp.x - leftRight - plotRect.left) + 'px';
            marker.style.top = (tp.y - topBottom - plotRect.top) + 'px';
            self._hLine.style.top = topBottom + 'px';
            self._hLine.style.left = plotRect.left - tp.x + leftRight + 'px';
            self._vLine.style.top = plotRect.top - tp.y + topBottom + 'px';
            self._vLine.style.left = leftRight + 'px';
        }


        private _getEventPoint(e: any): Point {
            return e instanceof MouseEvent ?
                new Point(e.pageX, e.pageY) :
                new Point(e.changedTouches[0].pageX, e.changedTouches[0].pageY);
        }

        private _pointInRect(pt: Point, rect: Rect): boolean {
            if (!pt || !rect) {
                return false;
            }
            if (pt.x >= rect.left && pt.x <= rect.left + rect.width &&
                pt.y >= rect.top && pt.y <= rect.top + rect.height) {
                return true;
            }

            return false;
        }
    }
}
module wijmo.chart {
    'use strict';

    export class _DataPoint {
        private _seriesIndex: number;
        private _pointIndex: number;
        private _dataX: number;
        private _dataY: number;

        constructor(seriesIndex: number, pointIndex: number, dataX: number, dataY: number) {
            this._seriesIndex = seriesIndex;
            this._pointIndex = pointIndex;
            this._dataX = dataX;
            this._dataY = dataY;
        }

        get seriesIndex(): number {
            return this._seriesIndex;
        }

        get pointIndex(): number {
            return this._pointIndex;
        }

        get dataX(): number {
            return this._dataX;
        }

        get dataY(): number {
            return this._dataY;
        }
    }

    export enum _MeasureOption {
        X,
        Y,
        XY
    }

    export class _RectArea implements _IHitArea {
        private _rect: Rect;

        constructor(rect: Rect) {
            this._rect = rect;
        }

        get rect(): Rect {
            return this._rect;
        }

        tag: any;

        contains(pt: Point): boolean {
            var rect = this._rect;
            return pt.x >= rect.left && pt.x <= rect.right && pt.y >= rect.top && pt.y <= rect.bottom;
        }

        pointDistance(pt1: Point, pt2: Point, option: _MeasureOption): number {
            var dx = pt2.x - pt1.x;
            var dy = pt2.y - pt1.y;
            if (option == _MeasureOption.X) {
                return Math.abs(dx);
            } else if (option == _MeasureOption.Y) {
                return Math.abs(dy);
            }

            return Math.sqrt(dx * dx + dy * dy);
        }

        distance(pt: Point): number {
            var option = _MeasureOption.XY;
            if (pt.x === null) {
                option = _MeasureOption.Y;
            } else if (pt.y === null) {
                option = _MeasureOption.X;
            }

            var rect = this._rect;
            if (pt.x < rect.left) { // Region I, VIII, or VII
                if (pt.y < rect.top) { // I
                    return this.pointDistance(pt, new Point(rect.left, rect.top), option);
                }
                else if (pt.y > rect.bottom) { // VII
                    return this.pointDistance(pt, new Point(rect.left, rect.bottom), option);
                }
                else { // VIII

                    if (option == _MeasureOption.Y) {
                        return 0;
                    }
                    return rect.left - pt.x;
                }
            }
            else if (pt.x > rect.right) { // Region III, IV, or V
                if (pt.y < rect.top) { // III
                    return this.pointDistance(pt, new Point(rect.right, rect.top), option);
                }
                else if (pt.y > rect.bottom) { // V
                    return this.pointDistance(pt, new Point(rect.right, rect.bottom), option);
                }
                else { // IV
                    if (option == _MeasureOption.Y) {
                        return 0;
                    }

                    return pt.x - rect.right;
                }
            }
            else { // Region II, IX, or VI
                if (option == _MeasureOption.X) {
                    return 0;
                }

                if (pt.y < rect.top) { // II
                    return rect.top - pt.y;
                }
                else if (pt.y > rect.bottom) { // VI
                    return pt.y - rect.bottom;
                }
                else { // IX
                    return 0;
                }
            }
        }
    }

    export class _CircleArea implements _IHitArea {
        private _center: Point;
        private _rad: number;
        private _rad2: number;

        tag: any;

        constructor(center: Point, radius: number) {
            this._center = center;
            this._rad = radius;
            this._rad2 = radius * radius;
        }

        get center(): Point {
            return this._center;
        }

        contains(pt: Point): boolean {
            var dx = this._center.x - pt.x;
            var dy = this._center.y - pt.y;
            return dx * dx + dy * dy <= this._rad2;
        }

        distance(pt: Point): number {
            //var dx = pt.x !== null ? this._center.x - pt.x : 0;
            //var dy = pt.y !== null ? this._center.y - pt.y : 0;
            var dx = !isNaN(pt.x) ? this._center.x - pt.x : 0;
            var dy = !isNaN(pt.y) ? this._center.y - pt.y : 0;

            var d2 = dx * dx + dy * dy;

            if (d2 <= this._rad2)
                return 0;
            else
                return Math.sqrt(d2) - this._rad;
        }
    }

    export class _LinesArea implements _IHitArea {
        private _x = [];
        private _y = [];

        tag: any;

        constructor(x: any, y: any) {
            this._x = x;
            this._y = y;
        }

        contains(pt: Point): boolean {
            return false;
        }

        distance(pt: Point): number {
            var dmin = NaN;
            for (var i = 0; i < this._x.length - 1; i++) {
                var d = FlexChart._dist(pt, new Point(this._x[i], this._y[i]), new Point(this._x[i + 1], this._y[i + 1]));
                if (isNaN(dmin) || d < dmin) {
                    dmin = d;
                }
            }

            return dmin;
        }
    }

    export class _HitResult {
        area: _IHitArea;
        distance: number;
    }

    export class _HitTester {
        _chart: FlexChartCore;
        _map: { [key: number]: Array<_IHitArea> } = {};
        //private _areas = new Array<IHitArea>();

        constructor(chart: FlexChartCore) {
            this._chart = chart;
        }

        add(area: _IHitArea, seriesIndex: number) {
            if (this._map[seriesIndex]) {
                if (!area.tag) {
                    area.tag = new _DataPoint(seriesIndex, NaN, NaN, NaN);
                }
                this._map[seriesIndex].push(area);
            }
        }

        clear() {
            this._map = {};
            var series = this._chart.series;
            for (var i = 0; i < series.length; i++) {
                if (series[i].hitTest === Series.prototype.hitTest) {
                    this._map[i] = new Array<_IHitArea>();
                }
            }
        }

        hitTest(pt: Point, testLines= false): _HitResult {
            var closest = null;
            var dist = Number.MAX_VALUE;

            for (var key in this._map) {
                var areas = this._map[key];
                if (areas) {
                    var len = areas.length;

                    for (var i = len - 1; i >= 0; i--) {
                        var area = areas[i];
                        if (tryCast(area, _LinesArea) && !testLines) {
                            continue;
                        }

                        var d = area.distance(pt);
                        if (d < dist) {
                            dist = d;
                            closest = area;
                            if (dist == 0)
                                break;
                        }
                    }

                    if (dist == 0)
                        break;
                }
            }

            if (closest) {
                var hr = new _HitResult();
                hr.area = closest;
                hr.distance = dist;
                return hr;
            }

            return null;
        }

        hitTestSeries(pt: Point, seriesIndex): _HitResult {
            var closest = null;
            var dist = Number.MAX_VALUE;

            var areas = this._map[seriesIndex];
            if (areas) {
                var len = areas.length;

                for (var i = len - 1; i >= 0; i--) {
                    var area = areas[i];

                    var d = area.distance(pt);
                    if (d < dist) {
                        dist = d;
                        closest = area;
                        if (dist == 0)
                            break;
                    }
                }
            }

            if (closest) {
                var hr = new _HitResult();
                hr.area = closest;
                hr.distance = dist;
                return hr;
            }

            return null;
        }
    }
}
module wijmo.chart {
    'use strict';

    /**
     * Plots data series.
     */
    export interface _IPlotter {
        chart: FlexChartCore;
        dataInfo: _DataInfo;
        hitTester: _HitTester;
        seriesIndex: number;
        seriesCount: number;
        clipping: boolean;

        stacking: Stacking;
        rotated: boolean;
        adjustLimits(dataInfo: _DataInfo, plotRect: Rect): Rect;
        plotSeries(engine: IRenderEngine, ax: _IAxis, ay: _IAxis, series: _ISeries, palette: _IPalette, iser: number, nser: number);
    }

    /**
     * Base class for chart plotters of all types (bar, line, area).
     */
    export class _BasePlotter {
        _DEFAULT_WIDTH = 2;
        _DEFAULT_SYM_SIZE = 10;

        clipping = true;
        chart: FlexChart;
        hitTester: _HitTester;
        dataInfo: _DataInfo;
        seriesIndex: number;
        seriesCount: number;

        clear() {
            this.seriesCount = 0;
            this.seriesIndex = 0;
        }

        getNumOption(name: string, parent?: string): number {
            var options = this.chart.options;
            if (parent) {
                options = options ? options[parent] : null;
            }
            if (options && options[name]) {
                return asNumber(options[name], true);
            }
            return undefined;
        }

        cloneStyle(style: any, ignore: string[]): any {
            if (!style) {
                return style;
            }
            var newStyle = {};

            for (var key in style) {
                if (ignore && ignore.indexOf(key) >= 0) {
                    continue;
                }
                newStyle[key] = style[key];
            }

            return newStyle;
        }

        isValid(datax: number, datay: number, ax: _IAxis, ay: _IAxis): boolean {
            return _DataInfo.isValid(datax) && _DataInfo.isValid(datay) &&
                FlexChart._contains(this.chart._plotRect, new Point(datax, datay))
        }
    }
} 
module wijmo.chart {
    'use strict';

    /**
     * Bar/column chart plotter.
     */
    export class _BarPlotter extends _BasePlotter implements _IPlotter {
        origin = 0;
        width = 0.7;
        //isColumn = false;

        private stackPosMap = {}; //{ [key: number]: number } = {};
        private stackNegMap = {};// { [key: number]: number } = {};

        stacking = Stacking.None;
        rotated: boolean;

        clear() {
            super.clear();

            this.stackNegMap[this.chart.axisY._uniqueId] = {};
            this.stackPosMap[this.chart.axisY._uniqueId] = {};
        }

        adjustLimits(dataInfo: _DataInfo, plotRect: Rect): Rect {
            this.dataInfo = dataInfo;

            var xmin = dataInfo.getMinX();
            var ymin = dataInfo.getMinY();
            var xmax = dataInfo.getMaxX();
            var ymax = dataInfo.getMaxY();

            var dx = dataInfo.getDeltaX();
            if (dx <= 0) {
                dx = 1;
            }

            if (this.rotated) {
                if (!this.chart.axisY.logBase) {
                    if (this.origin > ymax) {
                        ymax = this.origin;
                    } else if (this.origin < ymin) {
                        ymin = this.origin;
                    }
                }
                return new Rect(ymin, xmin - 0.5 * dx, ymax - ymin, xmax - xmin + dx);
            } else {
                if (!this.chart.axisY.logBase) {
                    if (this.origin > ymax) {
                        ymax = this.origin;
                    } else if (this.origin < ymin) {
                        ymin = this.origin;
                    }
                }
                return new Rect(xmin - 0.5 * dx, ymin, xmax - xmin + dx, ymax - ymin);
            }
        }

        plotSeries(engine: IRenderEngine, ax: _IAxis, ay: _IAxis, series: _ISeries, palette: _IPalette, iser: number, nser: number) {
            var si = this.chart.series.indexOf(series);
            var ser = <SeriesBase>series;
            var options = this.chart.options;
            var cw = this.width;
            var wpx = 0;

            if (options && options.groupWidth) {
                var gw = options.groupWidth;
                if (isNumber(gw)) {
                    // px
                    var gwn = asNumber(gw);
                    if (isFinite(gwn) && gwn > 0) {
                        wpx = gwn; cw = 1;
                    }
                } else if (isString(gw)) {
                    var gws = asString(gw);

                    // %
                    if (gws && gws.indexOf('%') >= 0) {
                        gws = gws.replace('%', '');
                        var gwn = parseFloat(gws);
                        if (isFinite(gwn)) {
                            if (gwn < 0) {
                                gwn = 0;
                            } else if (gwn > 100) {
                                gwn = 100;
                            }
                            wpx = 0; cw = gwn / 100;
                        }
                    } else {
                        // px
                        var gwn = parseFloat(gws);
                        if (isFinite(gwn) && gwn > 0) {
                            wpx = gwn; cw = 1;
                        }
                    }
                }
            }

            //var cw = this.getNumOption("clusterWidth");
            //if (cw == undefined || cw <= 0 || cw > 1) {
            //    cw = this.width;
            //}

            var w = cw / nser;

            var axid = ser._getAxisY()._uniqueId;

            //if (iser == 0) {
            //    this.stackNegMap[axid] = {};
            //    this.stackPosMap[axid] = {};
            // }
            var stackNeg = this.stackNegMap[axid];
            var stackPos = this.stackPosMap[axid];

            var yvals = series.getValues(0);
            var xvals = series.getValues(1);

            if (!yvals) {
                return;
            }

            if (!xvals) {
                xvals = this.dataInfo.getXVals();
            }

            if (xvals) {
                // find mininmal distance between point and use it as column width
                var delta = this.dataInfo.getDeltaX();
                if (delta > 0) {
                    cw *= delta;
                    w *= delta;
                }
            }

            // set series fill and stroke from style
            var style = series.style,
                fill = null,
                stroke = null;
            if (style) {
                if (style.fill) {
                    fill = style.fill;
                }
                if (style.stroke) {
                    stroke = style.stroke;
                }
            }

            // get colors not provided from palette
            if (!fill) {
                fill = palette._getColorLight(si);
            }
            if (!stroke) {
                stroke = palette._getColor(si);
            }

            // apply fill and stroke
            engine.fill = fill;
            engine.stroke = stroke;

            var len = yvals.length;
            if (xvals != null) {
                len = Math.min(len, xvals.length);
            }
            var origin = this.origin;

            //var symClass = FlexChart._CSS_SERIES_ITEM;
            var itemIndex = 0;

            if (!this.rotated) {
                if (origin < ay.actualMin) {
                    origin = ay.actualMin;
                } else if (origin > ay.actualMax) {
                    origin = ay.actualMax;
                }

                var originScreen = ay.convert(origin);

                for (var i = 0; i < len; i++) {
                    var datax = xvals ? xvals[i] : i;
                    var datay = yvals[i];

                    if (_DataInfo.isValid(datax) && _DataInfo.isValid(datay)) {

                        if (this.stacking != Stacking.None && !ser._isCustomAxisY()) {
                            var x0 = ax.convert(datax - 0.5 * cw);
                            var x1 = ax.convert(datax + 0.5 * cw);
                            var y0: number, y1: number;

                            if (this.stacking == Stacking.Stacked100pc) {
                                var sumabs = this.dataInfo.getStackedAbsSum(datax);
                                datay = datay / sumabs;
                            }

                            var sum = 0;
                            if (datay > 0) {
                                sum = isNaN(stackPos[datax]) ? 0 : stackPos[datax];
                                y0 = ay.convert(sum);
                                y1 = ay.convert(sum + datay);
                                stackPos[datax] = sum + datay;
                            } else {
                                sum = isNaN(stackNeg[datax]) ? 0 : stackNeg[datax];
                                y0 = ay.convert(sum);
                                y1 = ay.convert(sum + datay);
                                stackNeg[datax] = sum + datay;
                            }

                            var rect = new Rect(Math.min(x0, x1), Math.min(y0, y1), Math.abs(x1 - x0), Math.abs(y1 - y0));
                            if (wpx > 0) {
                                var ratio = 1 - wpx / rect.width;
                                if (ratio < 0) {
                                    ratio = 0;
                                }
                                var xc = rect.left + 0.5 * rect.width;
                                rect.left += (xc - rect.left) * ratio;
                                rect.width = Math.min(wpx, rect.width);
                            }

                            var area = new _RectArea(rect);

                            //engine.drawRect(rect.left, rect.top, rect.width, rect.height, null, series.symbolStyle);
                            this.drawSymbol(engine, rect, series, i, new Point(rect.left + 0.5 * rect.width, y1));
                            series._setPointIndex(i, itemIndex);
                            itemIndex++;

                            area.tag = new _DataPoint(si, i, datax, sum + datay);
                            this.hitTester.add(area, si);
                        } else {
                            var x0 = ax.convert(datax - 0.5 * cw + iser * w),
                                x1 = ax.convert(datax - 0.5 * cw + (iser + 1) * w),
                                y = ay.convert(datay),
                                rect = new Rect(Math.min(x0, x1), Math.min(y, originScreen), Math.abs(x1 - x0), Math.abs(originScreen - y));

                            if (wpx > 0) {
                                var sw = wpx / nser;
                                var ratio = 1 - sw / rect.width;
                                if (ratio < 0) {
                                    ratio = 0;
                                }
                                var xc = ax.convert(datax);
                                rect.left += (xc - rect.left) * ratio;
                                rect.width = Math.min(sw, rect.width);
                            }

                            var area = new _RectArea(rect);

                            //engine.drawRect(rect.left, rect.top, rect.width, rect.height, null, series.symbolStyle);
                            this.drawSymbol(engine, rect, series, i, new Point(rect.left + 0.5 * rect.width, y));
                            series._setPointIndex(i, itemIndex);
                            itemIndex++;

                            area.tag = new _DataPoint(si, i, datax, datay);
                            this.hitTester.add(area, si);
                        }
                    }
                }
            } else {
                if (origin < ax.actualMin) {
                    origin = ax.actualMin;
                } else if (origin > ax.actualMax) {
                    origin = ax.actualMax;
                }

                var originScreen = ax.convert(origin);

                for (var i = 0; i < len; i++) {
                    var datax = xvals ? xvals[i] : i,
                        datay = yvals[i];

                    if (_DataInfo.isValid(datax) && _DataInfo.isValid(datay)) {
                        if (this.stacking != Stacking.None) {
                            var y0 = ay.convert(datax - 0.5 * cw);
                            var y1 = ay.convert(datax + 0.5 * cw);
                            var x0: number, x1: number;

                            if (this.stacking == Stacking.Stacked100pc) {
                                var sumabs = this.dataInfo.getStackedAbsSum(datax);
                                datay = datay / sumabs;
                            }

                            var sum = 0;
                            if (datay > 0) {
                                sum = isNaN(stackPos[datax]) ? 0 : stackPos[datax];
                                x0 = ax.convert(sum);
                                x1 = ax.convert(sum + datay);
                                stackPos[datax] = sum + datay;
                            } else {
                                sum = isNaN(stackNeg[datax]) ? 0 : stackNeg[datax];
                                x0 = ax.convert(sum);
                                x1 = ax.convert(sum + datay);
                                stackNeg[datax] = sum + datay;
                            }

                            var rect = new Rect(Math.min(x0, x1), Math.min(y0, y1), Math.abs(x1 - x0), Math.abs(y1 - y0));
                            if (wpx > 0) {
                                var ratio = 1 - wpx / rect.height;
                                if (ratio < 0) {
                                    ratio = 0;
                                }
                                var yc = rect.top + 0.5 * rect.height;
                                rect.top += (yc - rect.top) * ratio;
                                rect.height = Math.min(wpx, rect.height);
                            }

                            var area = new _RectArea(rect);
                            //engine.drawRect(rect.left, rect.top, rect.width, rect.height, null, series.symbolStyle);
                            this.drawSymbol(engine, rect, series, i, new Point(x1, rect.top + 0.5 * rect.height));
                            series._setPointIndex(i, itemIndex);
                            itemIndex++;

                            area.tag = new _DataPoint(si, i, sum + datay, datax);
                            this.hitTester.add(area, si);
                        }
                        else {
                            var y0 = ay.convert(datax - 0.5 * cw + iser * w),
                                y1 = ay.convert(datax - 0.5 * cw + (iser + 1) * w),
                                x = ax.convert(datay),
                                rect = new Rect(Math.min(x, originScreen), Math.min(y0, y1), Math.abs(originScreen - x), Math.abs(y1 - y0));

                            if (wpx > 0) {
                                var sw = wpx / nser;
                                var ratio = 1 - sw / rect.height;
                                if (ratio < 0) {
                                    ratio = 0;
                                }
                                var yc = ay.convert(datax);
                                rect.top += (yc - rect.top) * ratio;
                                rect.height = Math.min(sw, rect.height);
                            }

                            var area = new _RectArea(rect);
                            //engine.drawRect(rect.left, rect.top, rect.width, rect.height, null, series.symbolStyle);
                            this.drawSymbol(engine, rect, series, i, new Point(x, rect.top + 0.5 * rect.height));
                            series._setPointIndex(i, itemIndex);
                            itemIndex++;

                            area.tag = new _DataPoint(si, i, datay, datax);
                            this.hitTester.add(area, si);
                        }
                    }
                }
            }
        }

        private drawSymbol(engine: IRenderEngine, rect: Rect, series: _ISeries, pointIndex: number, point: Point) {
            if (this.chart.itemFormatter) {
                engine.startGroup();
                var hti = new HitTestInfo(this.chart, point);
                hti._chartElement = ChartElement.SeriesSymbol;
                hti._pointIndex = pointIndex;
                hti._series = <Series>series;

                this.chart.itemFormatter(engine, hti, () => {
                    this.drawDefaultSymbol(engine, rect, series);
                });
                engine.endGroup();
            }
            else {
                this.drawDefaultSymbol(engine, rect, series);
            }
        }

        private drawDefaultSymbol(engine: IRenderEngine, rect: Rect, series: _ISeries) {
            engine.drawRect(rect.left, rect.top, rect.width, rect.height, null, series.symbolStyle/* ,'plotRect'*/);
        }
    }
} 
module wijmo.chart {
    'use strict';

    /**
     * Line/scatter chart plotter.
     */
    export class _LinePlotter extends _BasePlotter implements _IPlotter {
        hasSymbols: boolean = false;
        hasLines: boolean = true;
        isSpline: boolean = false;
        rotated: boolean;
        stacking = Stacking.None;

        private stackPos: { [key: number]: number } = {};
        private stackNeg: { [key: number]: number } = {};

        constructor() {
            super();
            this.clipping = false;
        }

        clear() {
            super.clear();
            this.stackNeg = {};
            this.stackPos = {};
        }

        adjustLimits(dataInfo: _DataInfo, plotRect: Rect): Rect {
            this.dataInfo = dataInfo;
            var xmin = dataInfo.getMinX();
            var ymin = dataInfo.getMinY();
            var xmax = dataInfo.getMaxX();
            var ymax = dataInfo.getMaxY();

            if (this.isSpline && !this.chart.axisY.logBase) {
                var dy = 0.1 * (ymax - ymin);
                ymin -= dy;
                ymax += dy;
            }

            return this.rotated
                ? new Rect(ymin, xmin, ymax - ymin, xmax - xmin)
                : new Rect(xmin, ymin, xmax - xmin, ymax - ymin);
        }

        plotSeries(engine: IRenderEngine, ax: _IAxis, ay: _IAxis, series: _ISeries, palette: _IPalette, iser: number, nser: number) {
            var ser: SeriesBase = asType(series, SeriesBase);
            var si = this.chart.series.indexOf(series);
            //if (iser == 0) {
            //    this.stackNeg = {};
            //    this.stackPos = {};
            //}

            var ys = series.getValues(0);
            var xs = series.getValues(1);
            if (!ys) {
                return;
            }
            if (!xs) {
                xs = this.dataInfo.getXVals();
            }

            var style = this.cloneStyle(series.style, ['fill']);
            var len = ys.length;
            var hasXs = true;
            if (!xs) {
                hasXs = false;
                xs = new Array<number>(len);
            } else {
                len = Math.min(len, xs.length);
            }

            var swidth = this._DEFAULT_WIDTH;
            var fill = null;
            var stroke = null;
            var symSize = ser._getSymbolSize();

            if (fill === null) {
                fill = palette._getColorLight(si);
            }
            if (stroke === null) {
                stroke = palette._getColor(si);
            }

            engine.stroke = stroke;
            engine.strokeWidth = swidth;
            engine.fill = fill;

            var xvals = new Array<number>();
            var yvals = new Array<number>();

            var rotated = this.rotated;
            var stacked = this.stacking != Stacking.None && !ser._isCustomAxisY();
            var stacked100 = this.stacking == Stacking.Stacked100pc && !ser._isCustomAxisY();

            var interpolateNulls = this.chart.interpolateNulls;
            var hasNulls = false;

            //var symClass = FlexChart._CSS_SERIES_ITEM;

            for (var i = 0; i < len; i++) {
                var datax = hasXs ? xs[i] : i;
                var datay = ys[i];

                if (_DataInfo.isValid(datax) && _DataInfo.isValid(datay)) {

                    if (stacked) {
                        if (stacked100) {
                            var sumabs = this.dataInfo.getStackedAbsSum(datax);
                            datay = datay / sumabs;
                        }

                        if (datay >= 0) {
                            var sum = isNaN(this.stackPos[datax]) ? 0 : this.stackPos[datax];
                            datay = this.stackPos[datax] = sum + datay;
                        }
                        else {
                            var sum = isNaN(this.stackNeg[datax]) ? 0 : this.stackNeg[datax];
                            datay = this.stackNeg[datax] = sum + datay;
                        }
                    }

                    var dpt: _DataPoint;

                    if (rotated) {
                        dpt = new _DataPoint(si, i, datay, datax);
                        var x = ax.convert(datay);
                        datay = ay.convert(datax);
                        datax = x;
                    } else {
                        dpt = new _DataPoint(si, i, datax, datay);
                        datax = ax.convert(datax);
                        datay = ay.convert(datay);
                    }
                    if (!isNaN(datax) && !isNaN(datay)) {
                        xvals.push(datax);
                        yvals.push(datay);

                        //if (this.hasSymbols) {
                        //    this.drawSymbol(engine, datax, datay, symSize, symSize, symClass + i.toString());
                        //}

                        var area = new _CircleArea(new Point(datax, datay), 0.5 * symSize);
                        area.tag = dpt;
                        this.hitTester.add(area, si);
                    } else {
                        hasNulls = true;
                        if (interpolateNulls !== true) {
                            xvals.push(undefined);
                            yvals.push(undefined);
                        }
                    }
                } else {
                    hasNulls = true;
                    if (interpolateNulls !== true) {
                        xvals.push(undefined);
                        yvals.push(undefined);
                    }
                }
            }

            var itemIndex = 0;

            if (this.hasLines) {
                engine.fill = null;

                if (hasNulls && interpolateNulls !== true) {
                    var dx = [];
                    var dy = [];

                    for (var i = 0; i < len; i++) {
                        if (xvals[i] === undefined) {
                            if (dx.length > 1) {
                                this._drawLines(engine, dx, dy, null, style, this.chart._plotrectId);
                                this.hitTester.add(new _LinesArea(dx, dy), si);
                                itemIndex++;
                            }
                            dx = [];
                            dy = [];
                        }
                        else {
                            dx.push(xvals[i]);
                            dy.push(yvals[i]);
                        }
                    }
                    if (dx.length > 1) {
                        this._drawLines(engine, dx, dy, null, style, this.chart._plotrectId);
                        this.hitTester.add(new _LinesArea(dx, dy), si);
                        itemIndex++;
                    }
                } else {
                    this._drawLines(engine, xvals, yvals, null, style, this.chart._plotrectId);
                    this.hitTester.add(new _LinesArea(xvals, yvals), si);
                    itemIndex++;
                }
            }

            if ((this.hasSymbols || this.chart.itemFormatter) && symSize > 0) {
                engine.fill = fill;
                for (var i = 0; i < len; i++) {
                    var datax = xvals[i];
                    var datay = yvals[i];
                    //if (DataInfo.isValid(datax) && DataInfo.isValid(datay)) {
                    if (this.isValid(datax, datay, ax, ay)) {
                        this._drawSymbol(engine, datax, datay, symSize, ser, i);
                        series._setPointIndex(i, itemIndex);
                        itemIndex++;
                    }
                }
            }
        }

        _drawLines(engine: IRenderEngine, xs: number[], ys: number[], className?: string, style?: any, clipPath?: string) {
            if (this.isSpline) {
                engine.drawSplines(xs, ys, className, style, clipPath);
            } else {
                engine.drawLines(xs, ys, className, style, clipPath);
            }
        }

        _drawSymbol(engine: IRenderEngine, x: number, y: number, sz: number, series: SeriesBase, pointIndex: number) {
            if (this.chart.itemFormatter) {
                engine.startGroup();
                var hti = new HitTestInfo(this.chart, new Point(x, y));
                hti._chartElement = ChartElement.SeriesSymbol;
                hti._pointIndex = pointIndex;
                hti._series = series;

                this.chart.itemFormatter(engine, hti, () => {
                    if (this.hasSymbols) {
                        this._drawDefaultSymbol(engine, x, y, sz, series.symbolMarker, series.symbolStyle);
                    }
                });
                engine.endGroup();
            } else {
                this._drawDefaultSymbol(engine, x, y, sz, series.symbolMarker, series.symbolStyle);
            }
        }

        _drawDefaultSymbol(engine: IRenderEngine, x: number, y: number, sz: number, marker: Marker, style?: any) {
            if (marker == Marker.Dot) {
                engine.drawEllipse(x, y, 0.5 * sz, 0.5 * sz, null, style);
            } else if (marker == Marker.Box) {
                engine.drawRect(x - 0.5 * sz, y - 0.5 * sz, sz, sz, null, style);
            }
        }
    }
} 
module wijmo.chart {
    'use strict';

    /**
     * Area chart plotter.
     */
    export class _AreaPlotter extends _BasePlotter implements _IPlotter {
        stacking = Stacking.None;
        isSpline = false;
        rotated: boolean;

        private stackPos: { [key: number]: number } = {};
        private stackNeg: { [key: number]: number } = {};

        constructor() {
            super();
            //this.clipping = false;
        }

        adjustLimits(dataInfo: _DataInfo, plotRect: Rect): Rect {
            this.dataInfo = dataInfo;
            var xmin = dataInfo.getMinX();
            var ymin = dataInfo.getMinY();
            var xmax = dataInfo.getMaxX();
            var ymax = dataInfo.getMaxY();

            if (this.isSpline) {
                var dy = 0.1 * (ymax - ymin);
                if (!this.chart.axisY.logBase)
                    ymin -= dy;
                ymax += dy;
            }

            if (this.rotated) {
                return new Rect(ymin, xmin, ymax - ymin, xmax - xmin);
            }
            else {
                return new Rect(xmin, ymin, xmax - xmin, ymax - ymin);
            }
        }

        clear() {
            super.clear();
            this.stackNeg = {};
            this.stackPos = {};
        }

        plotSeries(engine: IRenderEngine, ax: _IAxis, ay: _IAxis, series: _ISeries, palette: _IPalette, iser: number, nser: number) {
            var si = this.chart.series.indexOf(series);
            var ser = <SeriesBase>series;
            //if (iser == 0) {
            //    this.stackNeg = {};
            //    this.stackPos = {};
            //}

            var ys = series.getValues(0);
            var xs = series.getValues(1);

            if (!ys) {
                return;
            }

            var len = ys.length;

            if (!xs)
                xs = this.dataInfo.getXVals();

            var hasXs = true;
            if (!xs) {
                hasXs = false
                xs = new Array<number>(len);
            }
            else if (xs.length < len) {
                len = xs.length;
            }

            var xvals = new Array<number>();
            var yvals = new Array<number>();

            var xvals0 = new Array<number>();
            var yvals0 = new Array<number>();

            var stacked = this.stacking != Stacking.None && !ser._isCustomAxisY();
            var stacked100 = this.stacking == Stacking.Stacked100pc && !ser._isCustomAxisY();
            var rotated = this.rotated;

            var hasNulls = false;
            var interpolateNulls = this.chart.interpolateNulls;

            var xmax: number = null;
            var xmin: number = null;

            var prect = this.chart._plotRect;

            for (var i = 0; i < len; i++) {
                var datax = hasXs ? xs[i] : i;
                var datay = ys[i];
                if (xmax === null || datax > xmax) {
                    xmax = datax;
                }
                if (xmin === null || datax < xmin) {
                    xmin = datax;
                }
                if (_DataInfo.isValid(datax) && _DataInfo.isValid(datay)) {
                    var x = rotated ? ay.convert(datax) : ax.convert(datax);
                    if (stacked) {
                        if (stacked100) {
                            var sumabs = this.dataInfo.getStackedAbsSum(datax);
                            datay = datay / sumabs;
                        }

                        var sum = 0;

                        if (datay >= 0) {
                            sum = isNaN(this.stackPos[datax]) ? 0 : this.stackPos[datax];
                            datay = this.stackPos[datax] = sum + datay;
                        }
                        else {
                            sum = isNaN(this.stackNeg[datax]) ? 0 : this.stackNeg[datax];
                            datay = this.stackNeg[datax] = sum + datay;
                        }

                        if (rotated) {
                            if (sum < ax.actualMin) {
                                sum = ax.actualMin;
                            }
                            xvals0.push(ax.convert(sum));
                            yvals0.push(x);
                        } else {
                            xvals0.push(x);
                            if (sum < ay.actualMin) {
                                sum = ay.actualMin;
                            }
                            yvals0.push(ay.convert(sum));
                        }
                    }
                    if (rotated) {
                        var y = ax.convert(datay);
                        if (!isNaN(x) && !isNaN(y)) {
                            xvals.push(y);
                            yvals.push(x);
                            if (FlexChart._contains(prect, new Point(y, x))) {
                                var area = new _CircleArea(new Point(y, x), this._DEFAULT_SYM_SIZE);
                                area.tag = new _DataPoint(si, i, datay, datax);
                                this.hitTester.add(area, si);
                            }
                        }
                        else {
                            hasNulls = true;
                            if (!stacked && interpolateNulls !== true) {
                                xvals.push(undefined);
                                yvals.push(undefined);
                            }
                        }
                    }
                    else {
                        var y = ay.convert(datay);

                        if (!isNaN(x) && !isNaN(y)) {
                            xvals.push(x);
                            yvals.push(y);
                            if (FlexChart._contains(prect, new Point(x, y))) {
                                var area = new _CircleArea(new Point(x, y), this._DEFAULT_SYM_SIZE);
                                area.tag = new _DataPoint(si, i, datax, datay);
                                this.hitTester.add(area, si);
                            }
                        }
                        else {
                            hasNulls = true;
                            if (!stacked && interpolateNulls !== true) {
                                xvals.push(undefined);
                                yvals.push(undefined);
                            }
                        }
                    }
                }
                else {
                    hasNulls = true;
                    if (!stacked && interpolateNulls !== true) {
                        xvals.push(undefined);
                        yvals.push(undefined);
                    }
                }
            }

            var swidth = this._DEFAULT_WIDTH;
            var fill = palette._getColorLight(si);
            var stroke = palette._getColor(si);

            var lstyle = this.cloneStyle(series.style, ['fill']);
            var pstyle = this.cloneStyle(series.style, ['stroke']);

            if (!stacked && interpolateNulls !== true && hasNulls) {
                var dx = [];
                var dy = [];

                for (var i = 0; i < len; i++) {
                    if (xvals[i] === undefined) {
                        if (dx.length > 1) {
                            if (this.isSpline) {
                                var s = this._convertToSpline(dx, dy);
                                dx = s.xs; dy = s.ys;
                            }

                            engine.stroke = stroke;
                            engine.strokeWidth = swidth;
                            engine.fill = 'none';
                            engine.drawLines(dx, dy, null, lstyle);
                            this.hitTester.add(new _LinesArea(dx, dy), si);

                            if (rotated) {
                                dx.push(ax.convert(ax.actualMin), ax.convert(ax.actualMin));
                                dy.push(ay.convert(ay.actualMax), ay.convert(ay.actualMin));
                            }
                            else {
                                dx.push(dx[dx.length - 1], dx[0]);
                                dy.push(ay.convert(ay.actualMin), ay.convert(ay.actualMin));
                            }
                            engine.fill = fill;
                            engine.stroke = 'none';
                            engine.drawPolygon(dx, dy, null, pstyle);
                        }
                        dx = [];
                        dy = [];
                    }
                    else {
                        dx.push(xvals[i]);
                        dy.push(yvals[i]);
                    }
                }
                if (dx.length > 1) {
                    if (this.isSpline) {
                        var s = this._convertToSpline(dx, dy);
                        dx = s.xs; dy = s.ys;
                    }

                    engine.stroke = stroke;
                    engine.strokeWidth = swidth;
                    engine.fill = 'none';
                    engine.drawLines(dx, dy, null, lstyle);
                    this.hitTester.add(new _LinesArea(dx, dy), si);

                    if (rotated) {
                        dx.push(ax.convert(ax.actualMin), ax.convert(ax.actualMin));
                        dy.push(ay.convert(ay.actualMax), ay.convert(ay.actualMin));
                    }
                    else {
                        dx.push(dx[dx.length - 1], dx[0]);
                        dy.push(ay.convert(ay.actualMin), ay.convert(ay.actualMin));
                    }
                    engine.fill = fill;
                    engine.stroke = 'none';
                    engine.drawPolygon(dx, dy, null, pstyle);
                }
            }
            else {
                //
                if (this.isSpline) {
                    var s = this._convertToSpline(xvals, yvals);
                    xvals = s.xs; yvals = s.ys;
                }
                //

                if (stacked) {
                    if (this.isSpline) {
                        var s0 = this._convertToSpline(xvals0, yvals0);
                        xvals0 = s0.xs; yvals0 = s0.ys;
                    }

                    xvals = xvals.concat(xvals0.reverse());
                    yvals = yvals.concat(yvals0.reverse());
                }
                else {
                    if (rotated) {
                        xvals.push(ax.convert(ax.actualMin), ax.convert(ax.actualMin));
                        yvals.push(ay.convert(xmax), ay.convert(xmin));
                    }
                    else {
                        xvals.push(ax.convert(xmax), ax.convert(xmin));
                        yvals.push(ay.convert(ay.actualMin), ay.convert(ay.actualMin));
                    }
                }

                engine.fill = fill;
                engine.stroke = 'none';
                engine.drawPolygon(xvals, yvals, null, pstyle);

                if (stacked) {
                    xvals = xvals.slice(0, xvals.length - xvals0.length);
                    yvals = yvals.slice(0, yvals.length - yvals0.length);
                } else {
                    xvals = xvals.slice(0, xvals.length - 2);
                    yvals = yvals.slice(0, yvals.length - 2);
                }

                engine.stroke = stroke;
                engine.strokeWidth = swidth;
                engine.fill = 'none';
                engine.drawLines(xvals, yvals, null, lstyle);
                this.hitTester.add(new _LinesArea(xvals, yvals), si);
            }

            this._drawSymbols(engine, series, si);
        }

        _convertToSpline(x: number[], y: number[]) {
            if (x && y) {
                var spline = new _Spline(x, y);
                var s = spline.calculate();
                return { xs: s.xs, ys: s.ys };
            } else {
                return { xs: x, ys: y };
            }
        }

        _drawSymbols(engine: IRenderEngine, series: _ISeries, seriesIndex: number) {
            if (this.chart.itemFormatter != null) {
                var areas = this.hitTester._map[seriesIndex];
                for (var i = 0; i < areas.length; i++) {
                    var area: _CircleArea = tryCast(areas[i], _CircleArea);
                    if (area) {
                        var dpt = <_DataPoint>area.tag;
                        engine.startGroup();
                        var hti = new HitTestInfo(this.chart, area.center);
                        hti._chartElement = ChartElement.SeriesSymbol;
                        hti._pointIndex = dpt.pointIndex;
                        hti._series = <Series>series;
                        this.chart.itemFormatter(engine, hti, () => {
                        });
                        engine.endGroup();
                    }
                }
            }
        }
    }
}
module wijmo.chart {
    'use strict';

    export class _BubblePlotter extends _LinePlotter {
        private _MIN_SIZE = 5;
        private _MAX_SIZE = 30;

        private _minSize: number;
        private _maxSize: number;
        private _minValue: number;
        private _maxValue: number;

        constructor() {
            super();
            this.hasLines = false;
            this.hasSymbols = true;
            this.clipping = true;
        }

        adjustLimits(dataInfo: _DataInfo, plotRect: Rect): Rect {
            var minSize = this.getNumOption('minSize', 'bubble');
            this._minSize = minSize ? minSize : this._MIN_SIZE;
            var maxSize = this.getNumOption('maxSize', 'bubble');
            this._maxSize = maxSize ? maxSize : this._MAX_SIZE;

            var series = this.chart.series;
            var len = series.length;

            var min: number = NaN;
            var max: number = NaN;
            for (var i = 0; i < len; i++) {
                var ser = <Series>series[i];
                var vals = ser._getBindingValues(1);
                if (vals) {
                    var vlen = vals.length;
                    for (var j = 0; j < vlen; j++) {
                        if (_DataInfo.isValid(vals[j])) {
                            if (isNaN(min) || vals[j] < min) {
                                min = vals[j];
                            }
                            if (isNaN(max) || vals[j] > max) {
                                max = vals[j];
                            }
                        }
                    }
                }
            }
            this._minValue = min;
            this._maxValue = max;

            var rect = super.adjustLimits(dataInfo, plotRect);

            var w = plotRect.width - this._maxSize;
            var kw = w / rect.width;
            rect.left -= this._maxSize * 0.5 / kw;
            rect.width += this._maxSize / kw;

            var h = plotRect.height - this._maxSize;
            var kh = h / rect.height;
            rect.top -= this._maxSize * 0.5 / kh;
            rect.height += this._maxSize / kh;

            return rect;
        }

        _drawSymbol(engine: IRenderEngine, x: number, y: number, sz: number, series: Series, pointIndex: number) {
            var item = series._getItem(pointIndex);
            if (item) {
                var szBinding = series._getBinding(1);
                if (szBinding) {
                    var sz = <number>item[szBinding];
                    if (_DataInfo.isValid(sz)) {
                        var k = this._minValue == this._maxValue ? 1 :
                            Math.sqrt((sz - this._minValue) / (this._maxValue - this._minValue));
                        sz = this._minSize + (this._maxSize - this._minSize) * k;

                        if (this.chart.itemFormatter) {
                            var hti = new HitTestInfo(this.chart, new Point(x, y));
                            hti._chartElement = ChartElement.SeriesSymbol;
                            hti._pointIndex = pointIndex;
                            hti._series = series;

                            engine.startGroup();
                            this.chart.itemFormatter(engine, hti, () => {
                                this._drawDefaultSymbol(engine, x, y, sz, series.symbolMarker, series.symbolStyle);
                            });
                            engine.endGroup();
                        } else {
                            this._drawDefaultSymbol(engine, x, y, sz, series.symbolMarker, series.symbolStyle);
                        }
                    }
                }
            }
        }
    }
} 
module wijmo.chart {
    'use strict';

    export class _FinancePlotter extends _BasePlotter {
        isCandle: boolean = true;

        adjustLimits(dataInfo: _DataInfo, plotRect: Rect): Rect {
            this.dataInfo = dataInfo;
            var xmin = dataInfo.getMinX();
            var ymin = dataInfo.getMinY();
            var xmax = dataInfo.getMaxX();
            var ymax = dataInfo.getMaxY();

            var series = this.chart.series;
            var len = series.length;

            var swmax = 0;

            for (var i = 0; i < len; i++) {
                var ser = <SeriesBase>series[i];
                if (ser._isCustomAxisY()) {
                    continue;
                }

                var bndLow = ser._getBinding(1),
                    bndOpen = ser._getBinding(2),
                    bndClose = ser._getBinding(3);

                var slen = ser._getLength();
                if (slen) {

                    var sw = ser._getSymbolSize();
                    if (sw > swmax) {
                        swmax = sw;
                    }

                    for (var j = 0; j < slen; j++) {
                        var item = ser._getItem(j);
                        if (item) {
                            var yvals = [bndLow ? item[bndLow] : null,
                                bndOpen ? item[bndOpen] : null,
                                bndClose ? item[bndClose] : null];

                            yvals.forEach((yval) => {
                                if (_DataInfo.isValid(yval) && yval !== null) {
                                    if (isNaN(ymin) || yval < ymin) {
                                        ymin = yval;
                                    }
                                    if (isNaN(ymax) || yval > ymax) {
                                        ymax = yval;
                                    }
                                }
                            });
                        }
                    }
                }
            }

            // adjust limits according to symbol size
            var xrng = xmax - xmin;
            var pr = this.chart._plotRect;
            if (pr && pr.width) {
                sw += 2;
                var xrng1 = pr.width / (pr.width - sw) * xrng;
                xmin = xmin - 0.5 * (xrng1 - xrng);
                xrng = xrng1;
            }

            return new Rect(xmin, ymin, xrng, ymax - ymin);
        }

        plotSeries(engine: IRenderEngine, ax: _IAxis, ay: _IAxis, series: _ISeries, palette: _IPalette, iser: number, nser: number) {
            var ser: SeriesBase = asType(series, SeriesBase);
            var si = this.chart.series.indexOf(series);

            var highs = series.getValues(0);
            var xs = series.getValues(1);

            if (!highs) {
                return;
            }

            if (!xs) {
                xs = this.dataInfo.getXVals();
            }

            //var style = this.cloneStyle(series.style, null);// ['fill']);
            var len = highs.length;
            var hasXs = true;
            if (!xs) {
                hasXs = false;
                xs = new Array<number>(len);
            } else {
                len = Math.min(len, xs.length);
            }

            var swidth = this._DEFAULT_WIDTH,
                fill = ser._getSymbolFill(si),
                stroke = ser._getSymbolStroke(si),
                symSize = ser._getSymbolSize(),
                symStyle = series.symbolStyle;

            /*if (symStyle) {
                fill = symStyle.fill;
                stroke = symStyle.stroke;
            }

            if (style) {
                if (fill === null) {
                    fill = style.fill;
                }
                if (stroke === null) {
                    stroke = style.stroke;
                }
            }

            if (fill === null) {
                fill = palette._getColorLight(si);
            }
            if (stroke === null) {
                stroke = palette._getColor(si);
            }*/

            engine.stroke = stroke;
            engine.strokeWidth = swidth;
            engine.fill = fill;

            var bndLow = ser._getBinding(1);
            var bndOpen = ser._getBinding(2);
            var bndClose = ser._getBinding(3);

            var xmin = ax.actualMin,
                xmax = ax.actualMax;

            var itemIndex = 0;
            for (var i = 0; i < len; i++) {
                var item = ser._getItem(i);
                if (item) {
                    var x = hasXs ? xs[i] : i;

                    if (_DataInfo.isValid(x) && xmin <= x && x <= xmax) {
                        var hi = highs[i];
                        var lo = bndLow ? item[bndLow] : null;
                        var open = bndOpen ? item[bndOpen] : null;
                        var close = bndClose ? item[bndClose] : null;

                        engine.startGroup();

                        var currentFill = open < close ? 'transparent' : fill;

                        if (this.chart.itemFormatter) {
                            var hti = new HitTestInfo(this.chart, new Point(ax.convert(x), ay.convert(hi)));
                            hti._chartElement = ChartElement.SeriesSymbol;
                            hti._pointIndex = i;
                            hti._series = ser;

                            this.chart.itemFormatter(engine, hti, () => {
                                this._drawSymbol(engine, ax, ay, si, i, currentFill, symSize, x, hi, lo, open, close);
                            });
                        } else {
                            this._drawSymbol(engine, ax, ay, si, i, currentFill, symSize, x, hi, lo, open, close);
                        }
                        engine.endGroup();

                        series._setPointIndex(i, itemIndex);
                        itemIndex++;
                    }
                }
            }
        }

        _drawSymbol(engine: IRenderEngine, ax: _IAxis, ay: _IAxis, si: number, pi: number, fill: any, w: number, x: number, hi: number, lo: number, open: number, close: number) {
            var dpt = new _DataPoint(si, pi, x, hi);
            var area: _RectArea;
            x = ax.convert(x);
            if (this.isCandle) {
                var y0 = null,
                    y1 = null;
                if (_DataInfo.isValid(open) && _DataInfo.isValid(close)) {
                    engine.fill = fill;
                    open = ay.convert(open);
                    close = ay.convert(close);
                    y0 = Math.min(open, close);
                    y1 = y0 + Math.abs(open - close);
                    engine.drawRect(x - 0.5 * w, y0, w, y1 - y0);
                    area = new _RectArea(new Rect(x - 0.5 * w, y0, w, y1 - y0));
                    area.tag = dpt;
                    this.hitTester.add(area, si);
                }
                if (_DataInfo.isValid(hi)) {
                    hi = ay.convert(hi);
                    if (y0 !== null) {
                        engine.drawLine(x, y0, x, hi);
                    }
                }
                if (_DataInfo.isValid(lo)) {
                    lo = ay.convert(lo);
                    if (y1 !== null) {
                        engine.drawLine(x, y1, x, lo);
                    }
                }
            } else {
                if (_DataInfo.isValid(hi) && _DataInfo.isValid(lo)) {
                    hi = ay.convert(hi);
                    lo = ay.convert(lo);
                    engine.drawLine(x, lo, x, hi);
                    area = new _RectArea(new Rect(x - 0.5 * w, Math.min(hi, lo), w, Math.abs(hi - lo)));
                    area.tag = dpt;
                    this.hitTester.add(area, si);
                }
                if (_DataInfo.isValid(open)) {
                    open = ay.convert(open);
                    engine.drawLine(x - 0.5 * w, open, x, open);
                }
                if (_DataInfo.isValid(close)) {
                    close = ay.convert(close);
                    engine.drawLine(x, close, x + 0.5 * w, close);
                }
            }
        }
    }
}
