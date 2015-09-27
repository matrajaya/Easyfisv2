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
 * Defines the @see:RadialGauge, @see:LinearGauge, and @see:BulletGraph
 * controls.
 *
 * Unlike many gauge controls, Wijmo gauges concentrate on the data being
 * displayed, with little extraneous color and markup elements. They were 
 * designed to be easy to use and to read, especially on small-screen devices.
 *
 * Wijmo gauges are composed of @see:Range objects. Every Wijmo gauge has 
 * at least two ranges: the "face" and the "pointer".
 *
 * <ul><li>
 * The "face" represents the gauge background. The "min" and "max"
 * properties of the face range correspond to the "min" and "max" properties 
 * of the gauge control, and limit the values that the gauge can display.
 * </li><li>
 * The "pointer" is the range that indicates the gauge's current value. The 
 * "max" property of the pointer range corresponds to the "value" property 
 * of the gauge.
 * </li></ul>
 *
 * In addition to these two special ranges, gauges may have any number of 
 * additional ranges added to their "ranges" collection. These additional 
 * ranges can be used for two things:
 *
 * <ul><li>
 * By default, the extra ranges appear as part of the gauge background. 
 * This way you can show 'zones' within the gauge, like 'good,' 'average,' 
 * and 'bad' for example.
 * </li><li>
 * If you set the gauge's "showRanges" property to false, the additional 
 * ranges are not shown. Instead, they are used to automatically set the 
 * color of the "pointer" based on the current value.
 * </li></ul>
 */
module wijmo.gauge {
    'use strict';

    /**
     * Specifies which values to display as text.
     */
    export enum ShowText {
        /** Do not show any text in the gauge. */
        None = 0,
        /** Show the gauge's @see:value as text. */
        Value = 1,
        /** Show the gauge's @see:min and @see:max values as text. */
        MinMax = 2,
        /** Show the gauge's @see:value, @see:min, and @see:max as text. */
        All = 3
    }

    /**
     * Base class for the Wijmo Gauge controls (abstract).
     */
    export class Gauge extends Control {
        static _SVGNS = 'http://www.w3.org/2000/svg';
        static _ctr = 0;

        // property storage
        private _ranges = new wijmo.collections.ObservableArray();
        private _rngElements = [];
        private _format = 'n0';
        private _showRanges = true;
        private _shadow = true;
        private _animated = true;
        private _animInterval: number;
        private _readOnly = true;
        private _step = 1;
        private _showText = ShowText.None;
        private _filterID: string;
        private _rangesDirty: boolean;
        private _origin: number;

        // protected
        _thickness = 0.8;
        _initialized = false;
        _animColor: string;

        // main ranges:
        // face is the background and defines the Gauge's range (min/max);
        // pointer is the indicator and defines the Gauge's current value.
        _face: Range;
        _pointer: Range;

        // template parts
        _dSvg: HTMLDivElement;
        _svg: SVGSVGElement;
        _gFace: SVGGElement;
        _gRanges: SVGGElement;
        _gPointer: SVGGElement;
        _gCover: SVGGElement;
        _pFace: SVGPathElement;
        _pPointer: SVGPathElement;
        _filter: SVGFilterElement;
        _cValue: SVGCircleElement;
        _tValue: SVGTextElement;
        _tMin: SVGTextElement;
        _tMax: SVGTextElement;

        /**
         * Gets or sets the template used to instantiate @see:Gauge controls.
         */
        static controlTemplate = '<div wj-part="dsvg" style="width:100%;height:100%">' +
            '<svg wj-part="svg" width="100%" height="100%" style="overflow:visible">' +
                '<defs>' +
                  '<filter wj-part="filter">' +
                    '<feOffset dx="3" dy="3"></feOffset>' +
                    '<feGaussianBlur result="offset-blur" stdDeviation="5"></feGaussianBlur>' +
                    '<feComposite operator="out" in="SourceGraphic" in2="offset-blur" result="inverse"></feComposite>' +
                    '<feFlood flood-color="black" flood-opacity="0.2" result="color"></feFlood>' +
                    '<feComposite operator="in" in="color" in2="inverse" result="shadow"></feComposite>' +
                    '<feComposite operator="over" in="shadow" in2="SourceGraphic"></feComposite>' +
                  '</filter>' +
                '</defs>' +
                '<g wj-part="gface" class="wj-face">' +
                    '<path wj-part="pface"/>' +
                '</g>' +
                '<g wj-part="granges"/>' +
                '<g wj-part="gpointer" class="wj-pointer">' +
                    '<path wj-part="ppointer"/>' +
                '</g>' +
                '<g wj-part="gcover">' +
                    '<circle wj-part="cvalue" class="wj-pointer"/>' +
                    '<text wj-part="value" class="wj-value"/>' +
                    '<text wj-part="min" class="wj-min"/>' +
                    '<text wj-part="max" class="wj-max"/>' +
                '</g>' +
            '</svg>' +
            '</div>';

        /**
         * Initializes a new instance of a @see:Gauge control.
         *
         * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
         * @param options The JavaScript object containing initialization data for the control.
         */
        constructor(element: any, options?) {
            super(element, null, true);
            Gauge._ctr++;

            // instantiate and apply template
            var tpl = this.getTemplate();
            this.applyTemplate('wj-control wj-gauge', tpl, {
                _dSvg: 'dsvg',
                _svg: 'svg',
                _filter: 'filter',
                _gFace: 'gface',
                _gRanges: 'granges',
                _gPointer: 'gpointer',
                _gCover: 'gcover',
                _pFace: 'pface',
                _pPointer: 'ppointer',
                _cValue: 'cvalue',
                _tValue: 'value',
                _tMin: 'min',
                _tMax: 'max'
            });

            // apply filter id to template
            this._filterID = 'wj-gauge-filter-' + Gauge._ctr.toString(36);
            this._filter.setAttribute('id', this._filterID);

            // initialize main ranges
            this._face = new Range();
            this._pointer = new Range();
            this._face.propertyChanged.addHandler(this._rangeChanged, this);
            this._pointer.propertyChanged.addHandler(this._rangeChanged, this);

            // invalidate control and re-create range elements when ranges change
            var self = this;
            this._ranges.collectionChanged.addHandler(function () {

                // check types
                var arr = self._ranges;
                for (var i = 0; i < arr.length; i++) {
                    var rng = tryCast(arr[i], Range);
                    if (!rng) {
                        throw 'ranges array must contain Range objects.';
                    }
                }

                // remember ranges are dirty and invalidate
                self._rangesDirty = true;
                self.invalidate();
            });

            // keyboard handling
            this.hostElement.addEventListener('keydown', this._keyDown.bind(this));

            // mouse handling: 
            // when the user presses the mouse on the control, hook up handlers to 
            // mouse move/up on the *document*, and unhook on mouse up.
            // this simulates a mouse capture (nice idea from ngGrid).
            // note: use 'document' and not 'window'; that doesn't work on Android.
            this.hostElement.addEventListener('mousedown', function (e: MouseEvent) {
                document.addEventListener('mousemove', winMouseMove);
                document.addEventListener('mouseup', winMouseUp);
                self._mouseDown(e);
            });
            var winMouseMove = function (e: MouseEvent) {
                self._mouseMove(e);
            };
            var winMouseUp = function (e: MouseEvent) {
                document.removeEventListener('mousemove', winMouseMove);
                document.removeEventListener('mouseup', winMouseUp);
                self._mouseUp(e);
            };

            // initialize control options
            this.initialize(options);

            // ensure face and text are updated
            this.invalidate();
        }

        /**
         * Gets or sets the value to display on the gauge.
         */
        get value(): number {
            return this._pointer.max;
        }
        set value(value: number) {
            if (value != this._pointer.max) {
                this._pointer.max = asNumber(value);
            }
        }
        /**
         * Gets or sets the minimum value that can be displayed on the gauge.
         */
        get min(): number {
            return this._face.min;
        }
        set min(value: number) {
            this._face.min = value;
        }
        /**
         * Gets or sets the maximum value that can be displayed on the gauge.
         */
        get max(): number {
            return this._face.max;
        }
        set max(value: number) {
            this._face.max = value;
        }
        /**
         * Gets or sets the starting point used for painting the range.
         *
         * By default, this property is set to null, which causes the value range
         * to start at the gauge's minimum value, or zero if the minimum is less
         * than zero.
         */
        get origin(): number {
            return this._origin;
        }
        set origin(value: number) {
            if (value != this._origin) {
                this._origin = asNumber(value, true);
                this.invalidate();
            }
        }
        /**
         * Gets or sets a value indicating whether the user can edit the value using the mouse and
         * the keyboard.
         */
        get isReadOnly(): boolean {
            return this._readOnly;
        }
        set isReadOnly(value: boolean) {
            this._readOnly = asBoolean(value);
            var cursor = this._readOnly ? null : 'pointer';
            this._setAttribute(this._gFace, 'cursor', cursor);
            this._setAttribute(this._gRanges, 'cursor', cursor);
            this._setAttribute(this._gPointer, 'cursor', cursor);
        }
        /**
         * Gets or sets the amount to add to or subtract from the @see:value property
         * when the user presses the arrow keys.
         */
        get step(): number {
            return this._step;
        }
        set step(value: number) {
            this._step = asNumber(value, true);
        }
        /**
         * Gets or sets the format string to use for displaying the gauge values
         * as text.
         */
        get format(): string {
            return this._format;
        }
        set format(value: string) {
            if (value != this._format) {
                this._format = asString(value);
                this.invalidate();
            }
        }
        /**
         * Gets or sets the thickness of the gauge, on a scale between zero and one.
         *
         * Setting the thickness to one causes the gauge to fill as much of the
         * control area as possible. Smaller values create thinner gauges.
         */
        get thickness(): number {
            return this._thickness;
        }
        set thickness(value: number) {
            if (value != this._thickness) {
                this._thickness = clamp(asNumber(value, false), 0, 1);
                this.invalidate();
            }
        }
        /**
         * Gets the @see:Range used to represent the gauge's overall geometry
         * and appearance.
         */
        get face(): Range {
            return this._face;
        }
        /**
         * Gets the @see:Range used to represent the gauge's current value.
         */
        get pointer(): Range {
            return this._pointer;
        }
        /**
         * Gets or sets the @see:ShowText values to display as text in the gauge.
         */
        get showText(): ShowText {
            return this._showText;
        }
        set showText(value: ShowText) {
            if (value != this._showText) {
                this._showText = asEnum(value, ShowText);
                this.invalidate();
            }
        }
        /**
         * Gets or sets a value indicating whether the gauge displays the ranges contained in the @see:ranges property.
         *
         * If this property is set to false, the ranges contained in the @see:ranges property are not
         * displayed in the gauge. Instead, they are used to interpolate the color of the @see:pointer
         * range while animating value changes.
         */
        get showRanges(): boolean {
            return this._showRanges;
        }
        set showRanges(value: boolean) {
            if (value != this._showRanges) {
                this._showRanges = asBoolean(value);
                this._animColor = null;
                this._rangesDirty = true;
                this.invalidate();
            }
        }
        /**
         * Gets or sets a value indicating whether the gauge displays a shadow effect.
         */
        get hasShadow(): boolean {
            return this._shadow;
        }
        set hasShadow(value: boolean) {
            if (value != this._shadow) {
                this._shadow = asBoolean(value);
                this.invalidate();
            }
        }
        /**
         * Gets or sets a value indicating whether the gauge animates value changes.
         */
        get isAnimated(): boolean {
            return this._animated;
        }
        set isAnimated(value: boolean) {
            if (value != this._animated) {
                this._animated = asBoolean(value);
            }
        }
        /**
         * Gets the collection of ranges in this gauge.
         */
        get ranges(): wijmo.collections.ObservableArray {
            return this._ranges;
        }
        /**
         * Occurs when the value shown on the gauge changes.
         */
        valueChanged = new Event();
        /**
         * Raises the @see:valueChanged event.
         */
        onValueChanged() {
            this.valueChanged.raise(this);
        }
        /**
         * Refreshes the control.
         *
         * @param fullUpdate Indicates whether to update the control layout as well as the content.
         */
        refresh(fullUpdate = true) {
            super.refresh(fullUpdate);

            // update ranges if they are dirty
            if (this._rangesDirty) { 
                this._rangesDirty = false;
                var gr = this._gRanges;

                // remove old elements and disconnect event handlers
                for (var i = 0; i < this._rngElements.length; i++) {
                    var e = this._rngElements[i];
                    e.rng.propertyChanged.removeHandler(this._rangeChanged);
                }
                while (gr.lastChild) {
                    gr.removeChild(gr.lastChild);
                }
                this._rngElements = [];

                // add elements for each range and listen to changes
                if (this._showRanges) {
                    for (var i = 0; i < this.ranges.length; i++) {
                        var rng = this.ranges[i];
                        rng.propertyChanged.addHandler(this._rangeChanged, this);
                        this._rngElements.push({
                            rng: rng,
                            el: this._createElement('path', gr)
                        });
                    }
                }
            }

            // update text elements
            var display = (this.showText & ShowText.Value) == 0 ? 'none' : null;
            this._setAttribute(this._tValue, 'display', display);
            this._setAttribute(this._cValue, 'display', display);
            display = (this.showText & ShowText.MinMax) == 0 ? 'none' : null;
            this._setAttribute(this._tMin, 'display', display);
            this._setAttribute(this._tMax, 'display', display);
            this._updateText();

            // update face and pointer
            var filterUrl = this._getFilterUrl();
            this._setAttribute(this._pFace, 'filter', filterUrl);
            this._setAttribute(this._pPointer, 'filter', filterUrl);
            this._updateRange(this._face);
            this._updateRange(this._pointer);

            // update ranges
            for (var i = 0; i < this.ranges.length; i++) {
                this._updateRange(this.ranges[i]);
            }

            // ready
            this._initialized = true;
        }
        /**
         * Gets a number that corresponds to the value of the gauge at a given point.
         *
         * For example:
         *
         * <pre>
         * // hit test a point when the user clicks on the gauge
         * gauge.hostElement.addEventListener('click', function (e) {
         *   var ht = gauge.hitTest(e.pageX, e.pageY);
         *   if (ht != null) {
         *     console.log('you clicked the gauge at value ' + ht.toString());
         *   }
         * });
         * </pre>
         *
         * @param pt The point to investigate, in window coordinates, or a MoueEvent object, 
         * or the x coordinate of the point.
         * @param y The Y coordinate of the point (if the first parameter is a number).
         * @return Value of the gauge at the point, or null if the point is not on the gauge's face.
         */
        hitTest(pt: any, y?: number): number {

            // get point in page coordinates
            if (isNumber(pt) && isNumber(y)) { // accept hitTest(x, y) as well
                pt = new Point(pt, y);
            } else if (pt instanceof MouseEvent) {
                pt = new Point(pt.pageX, pt.pageY);
            }
            asType(pt, Point);

            // convert point to gauge client coordinates
            var rc = Rect.fromBoundingRect(this._dSvg.getBoundingClientRect());
            pt.x -= rc.left + window.pageXOffset;
            pt.y -= rc.top + window.pageYOffset;

            // get gauge value from point
            return this._getValueFromPoint(pt);
        }

        // ** implementation

        // gets the unique filter ID used by this gauge
        _getFilterUrl() {
            return this.hasShadow ? 'url(#' + this._filterID + ')' : null;
        }

        // gets the path element that represents a Range
        _getRangeElement(rng: Range): SVGPathElement {
            if (rng == this._face) {
                return this._pFace;
            } else if (rng == this._pointer) {
                return this._pPointer;
            }
            for (var i = 0; i < this._rngElements.length; i++) {
                var rngEl = this._rngElements[i];
                if (rngEl.rng == rng) {
                    return rngEl.el;
                }
            }
            return null;
        }

        // handle changes to range objects
        _rangeChanged(rng: Range, e: PropertyChangedEventArgs) {

            // when pointer.max changes, raise valueChanged
            if (rng == this._pointer && e.propertyName == 'max') {
                this.onValueChanged();
                this._updateText();
            }

            // when face changes, invalidate the whole gauge
            if (rng == this._face) {
                this.invalidate();
                return;
            }

            // update pointer with animation
            if (rng == this._pointer && e.propertyName == 'max' && this.isAnimated && !this.isUpdating && this._initialized) {

                // clear pending animations if any
                if (this._animInterval) {
                    clearInterval(this._animInterval);
                }

                // animate
                var self = this,
                    s1 = this._getPointerColor(e.oldValue),
                    s2 = this._getPointerColor(e.newValue),
                    c1 = s1 ? new Color(s1) : null,
                    c2 = s2 ? new Color(s2) : null;
                this._animInterval = animate(function (pct) {
                    self._animColor = (c1 && c2)
                        ? Color.interpolate(c1, c2, pct).toString()
                        : null;
                    self._updateRange(rng, e.oldValue + pct * (e.newValue - e.oldValue));
                    if (pct >= 1) {
                        self._animColor = null;
                        self._updateRange(rng);
                        self._updateText();
                        self._animInterval = null;
                    }
                });
                return;
            }

            // update range without animation
            this._updateRange(rng);
        }

        // creates an SVG element with the given tag and appends it to a given element
        _createElement(tag: string, parent: SVGElement, cls?: string) {
            var e = document.createElementNS(Gauge._SVGNS, tag);
            if (cls) {
                e.setAttribute('class', cls);
            }
            parent.appendChild(e);
            return e;
        }

        // centers an SVG text element at a given point
        _centerText(e: SVGTextElement, value: number, center: Point) {
            if (e.getAttribute('display') != 'none') {
                e.textContent = Globalize.format(value, this.format);
                var box = Rect.fromBoundingRect(e.getBBox()),
                    x = (center.x - box.width / 2),
                    y = (center.y + box.height / 4);
                e.setAttribute('x', this._fix(x));
                e.setAttribute('y', this._fix(y));
            }
        }

        // method used in JSON-style initialization
        _copy(key: string, value: any): boolean {
            if (key == 'ranges') {
                var arr = asArray(value);
                for (var i = 0; i < arr.length; i++) {
                    var r = new Range();
                    wijmo.copy(r, arr[i]);
                    this.ranges.push(r);
                }
                return true;
            } else if (key == 'pointer') {
                wijmo.copy(this.pointer, value);
                return true;
            }
            return false;
        }

        // scales a value to a percentage based on the gauge's min and max properties
        _getPercent = function (value) {
            var pct = (this.max > this.min) ? (value - this.min) / (this.max - this.min) : 0;
            return Math.max(0, Math.min(1, pct));
        };

        // sets or clears an attribute
        _setAttribute(e: SVGElement, att: string, value: string) {
            if (value) {
                e.setAttribute(att, value);
            } else {
                e.removeAttribute(att);
            }
        }

        // updates the element for a given range
        _updateRange(rng: Range, value = rng.max) {

            // update pointer's min value
            if (rng == this._pointer) {
                rng.min = this.origin != null
                    ? this.origin
                    : (this.min < 0 && this.max > 0) ? 0 : this.min;
            }

            // update the range's element
            var e = this._getRangeElement(rng);
            if (e) {
                this._updateRangeElement(e, rng, value);
                var color = rng.color;
                if (rng == this._pointer) {
                    color = this._animColor ? this._animColor : this._getPointerColor(rng.max);
                }
                this._setAttribute(e, 'style', color ? 'fill:' + color : null);
            }
        }

        // gets the color for the pointer range based on the gauge ranges
        _getPointerColor(value: number): string {
            var rng: Range;
            if (!this._showRanges) {
                for (var i = 0; i < this._ranges.length; i++) {
                    var r = this._ranges[i];
                    if (value >= r.min && value <= r.max) {
                        if (rng == null || rng.max - rng.min > r.max - r.min) {
                            rng = r;
                        }
                    }
                }
                if (rng) {
                    return rng.color;
                }
            }
            return this._pointer.color;
        }

        // keyboard handling
        _keyDown(e: KeyboardEvent) {
            if (!this._readOnly && this._step) {
                var handled = true;
                switch (e.keyCode) {
                    case Key.Left:
                    case Key.Down:
                        this.value = clamp(this.value - this.step, this.min, this.max);
                        break;
                    case Key.Right:
                    case Key.Up:
                        this.value = clamp(this.value + this.step, this.min, this.max);
                        break;
                    case Key.Home:
                        this.value = this.min;
                        break;
                    case Key.End:
                        this.value = this.max;
                        break;
                    default:
                        handled = false;
                        break;
                }
                if (handled) {
                    e.preventDefault();
                }
            }
        }

        // mouse handling
        _htDown: number;
        _mouseDown(e: MouseEvent) {
            if (!this.isReadOnly) {
                this._htDown = this.hitTest(e);
                if (this._htDown != null) {
                    this.focus();
                    e.preventDefault();
                    this._applyValue(this._htDown);
                }
            }
        }
        _mouseMove(e: MouseEvent) {
            if (this._htDown != null) {
                this._applyValue(this.hitTest(e));
            }
        }
        _mouseUp(e: MouseEvent) {
            this._htDown = null;
        }
        _applyValue(value: number) {
            if (value != null) {
                if (this._step != null) {
                    value = Math.round(value / this._step) * this._step;
                }
                this.value = clamp(value, this.min, this.max);
            }
        }

        // ** virtual methods (must be overridden in derived classes)

        // updates the range element
        _updateRangeElement(e: SVGPathElement, rng: Range, value: number) {
            assert(false, 'Gauge is an abstract class.');
        }

        // updates the text elements
        _updateText() {
            assert(false, 'Gauge is an abstract class.');
        }

        // gets the value at a given point (in gauge client coordinates)
        _getValueFromPoint(pt: Point) {
            return null;
        }

        // formats numbers or points with up to 4 decimals
        _fix(n: any): string {
            return isNumber(n)
                ? parseFloat(n.toFixed(4)).toString()
                : this._fix(n.x) + ' ' + this._fix(n.y);
        }
    }
}


module wijmo.gauge {
    'use strict';

    /**
     * Represents the direction in which the pointer of a @see:LinearGauge
     * increases.
     */
    export enum GaugeDirection {
        /** Gauge value increases from left to right. */
        Right,
        /** Gauge value increases from right to left. */
        Left,
        /** Gauge value increases from bottom to top. */
        Up,
        /** Gauge value increases from top to bottom. */
        Down
    }

    /**
     * The @see:LinearGauge displays a linear scale with an indicator
     * that represents a single value and optional ranges to represent
     * reference values.
     *
     * If you set the gauge's @see:isReadOnly property to false, then the
     * user will be able to edit the value by clicking on the gauge.
     *
     * @fiddle:t842jozb
     */
    export class LinearGauge extends Gauge {

        // property storage
        private _direction = GaugeDirection.Right;

        /**
         * Initializes a new instance of a @see:LinearGauge control.
         *
         * @param element The DOM element that will host the control, or a selector for the host element (e.g. '#theCtrl').
         * @param options JavaScript object containing initialization data for the control.
         */
        constructor(element: any, options?) {
            super(element, null);

            // customize
            addClass(this.hostElement, 'wj-lineargauge');

            // initialize control options
            this.initialize(options);
        }

        /**
         * Gets or sets the direction in which the gauge is filled.
         */
        get direction(): GaugeDirection {
            return this._direction;
        }
        set direction(value: GaugeDirection) {
            if (value != this._direction) {
                this._direction = asEnum(value, GaugeDirection);
                this.invalidate();
            }
        }

        // virtual methods

        // updates the element for a given range
        _updateRangeElement(e: SVGPathElement, rng: Range, value: number) {

            // update the path
            var rc = this._getRangeRect(rng, value);
            this._updateSegment(e, rc);

            // update text display of current value
            if (rng == this._pointer && (this.showText & ShowText.Value) != 0) {

                // update text element
                var x = rc.left + rc.width / 2,
                    y = rc.top + rc.height / 2;
                switch (this.direction) {
                    case GaugeDirection.Right:
                        x = rc.right;
                        break;
                    case GaugeDirection.Left:
                        x = rc.left;
                        break;
                    case GaugeDirection.Up:
                        y = rc.top;
                        break;
                    case GaugeDirection.Down:
                        y = rc.bottom;
                        break;
                }
                this._centerText(this._tValue, value, new Point(x, y));

                // update circle around the text
                rc = Rect.fromBoundingRect(this._tValue.getBBox());
                var color = this._animColor ? this._animColor : this._getPointerColor(rng.max),
                    ce = this._cValue;
                this._setAttribute(ce, 'cx', this._fix(x));
                this._setAttribute(ce, 'cy', this._fix(y));
                this._setAttribute(ce, 'r', this._fix(Math.max(rc.width, rc.height) * .8));
                this._setAttribute(ce, 'style', color ? 'fill:' + color : null);
            }
        }

        // updates the text elements
        _updateText() {
            var rc = this._getRangeRect(this._face);
            switch (this.direction) {
                case GaugeDirection.Right:
                    this._setText(this._tMin, this.min, rc, 'left');
                    this._setText(this._tMax, this.max, rc, 'right');
                    break;
                case GaugeDirection.Left:
                    this._setText(this._tMin, this.min, rc, 'right');
                    this._setText(this._tMax, this.max, rc, 'left');
                    break;
                case GaugeDirection.Up:
                    this._setText(this._tMin, this.min, rc, 'bottom');
                    this._setText(this._tMax, this.max, rc, 'top');
                    break;
                case GaugeDirection.Down:
                    this._setText(this._tMin, this.min, rc, 'top');
                    this._setText(this._tMax, this.max, rc, 'bottom');
                    break;
            }
        }

        // ** private stuff

        // draws a rectangular segment at the specified position
        _updateSegment(path: SVGPathElement, rc: Rect) {
            var data = {
                p1: this._fix(new Point(rc.left, rc.top)),
                p2: this._fix(new Point(rc.right, rc.top)),
                p3: this._fix(new Point(rc.right, rc.bottom)),
                p4: this._fix(new Point(rc.left, rc.bottom))
            };
            var content = wijmo.format('M {p1} L {p2} L {p3} L {p4} Z', data);
            path.setAttribute('d', content);
        }

        // positions a text element
        _setText(e: SVGTextElement, value: number, rc: Rect, pos: string) {
            if (e.getAttribute('display') != 'none') {
                e.textContent = Globalize.format(value, this.format);
                var box = Rect.fromBoundingRect(e.getBBox()),
                    pt = new Point(rc.left + rc.width / 2 - box.width / 2,
                        rc.top + rc.height / 2 + box.height / 2);
                switch (pos) {
                    case 'top':
                        pt.y = rc.top - 4;
                        break;
                    case 'left':
                        pt.x = rc.left - 4 - box.width;
                        break;
                    case 'right':
                        pt.x = rc.right + 4;
                        break;
                    case 'bottom':
                        pt.y = rc.bottom + 4 + box.height;
                        break;
                }
                e.setAttribute('x', this._fix(pt.x));
                e.setAttribute('y', this._fix(pt.y));
            }
        }

        // gets a rectangle that represents a Range
        _getRangeRect(rng: Range, value = rng.max): Rect {

            // get gauge direction
            var rc = Rect.fromBoundingRect(this._dSvg.getBoundingClientRect());
            rc.left = rc.top = 0;

            // get face rect (account for thickness, text at edges)
            var fs = (this.showText != ShowText.None) ? parseInt(getComputedStyle(this.hostElement).fontSize) : 0;
            switch (this.direction) {
                case GaugeDirection.Right:
                case GaugeDirection.Left:
                    rc = rc.inflate(-fs * 3, -rc.height * (1 - this.thickness * rng.thickness) / 2);
                    break;
                case GaugeDirection.Up:
                case GaugeDirection.Down:
                    rc = rc.inflate(-rc.width * (1 - this.thickness * rng.thickness) / 2, -fs);
                    break;
            }

            // get range rect
            var pctMin = this._getPercent(rng.min),
                pctMax = this._getPercent(value);
            switch (this.direction) {
                case GaugeDirection.Right:
                    rc.left += rc.width * pctMin;
                    rc.width *= (pctMax - pctMin);
                    break;
                case GaugeDirection.Left:
                    rc.left = rc.right - rc.width * pctMax;
                    rc.width = rc.width * (pctMax - pctMin);
                    break;
                case GaugeDirection.Down:
                    rc.top += rc.height * pctMin;
                    rc.height *= (pctMax - pctMin);
                    break;
                case GaugeDirection.Up:
                    rc.top = rc.bottom - rc.height * pctMax;
                    rc.height = rc.height * (pctMax - pctMin);
                    break;
            }

            // done
            return rc;
        }

        // gets the gauge value at a given point (in gauge client coordinates)
        _getValueFromPoint(pt: Point) {

            // check that the point is within the face
            var rc = this._getRangeRect(this._face);
            if (!rc.contains(pt)) {
                return null;
            }

            // get position
            var pct = 0;
            switch (this.direction) {
                case GaugeDirection.Right:
                    pct = (pt.x - rc.left) / rc.width;
                    break;
                case GaugeDirection.Left:
                    pct = (rc.right - pt.x) / rc.width;
                    break;
                case GaugeDirection.Up:
                    pct = (rc.bottom - pt.y) / rc.height;
                    break;
                case GaugeDirection.Down:
                    pct = (pt.y - rc.top) / rc.height;
                    break;
            }

            // done
            return this.min + pct * (this.max - this.min);
        }
    }
}

module wijmo.gauge {
    'use strict';

    /**
     * The @see:RadialGauge displays a circular scale with an indicator
     * that represents a single value and optional ranges to represent
     * reference values.
     *
     * If you set the gauge's @see:isReadOnly property to false, then the
     * user can edit the value by clicking on the gauge.
     *
     * @fiddle:7ec2144u
     */
    export class RadialGauge extends Gauge {

        // property storage
        private _startAngle = 0;
        private _sweepAngle = 180;
        private _autoScale = true;

        /**
         * Initializes a new instance of a @see:RadialGauge control.
         *
         * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
         * @param options The JavaScript object containing initialization data for the control.
         */
        constructor(element: any, options?) {
            super(element, null);

            // customize
            addClass(this.hostElement, 'wj-radialgauge');
            this._thickness = .4;
            this.showText = ShowText.All;

            // initialize control options
            this.initialize(options);
        }

        /**
         * Gets or sets the starting angle for the gauge, in degrees.
         *
         * Angles are measured clockwise, starting at the 9 o'clock position.
         */
        get startAngle(): number {
            return this._startAngle;
        }
        set startAngle(value: number) {
            if (value != this._startAngle) {
                this._startAngle = clamp(asNumber(value, false), -360, 360);
                this.invalidate();
            }
        }
        /**
         * Gets or sets the sweeping angle for the gauge, in degrees.
         *
         * Angles are measured clockwise, starting at the 9 o'clock position.
         */
        get sweepAngle(): number {
            return this._sweepAngle;
        }
        set sweepAngle(value: number) {
            if (value != this._sweepAngle) {
                this._sweepAngle = clamp(asNumber(value, false), -360, 360);
                this.invalidate();
            }
        }
        /**
         * Gets or sets a value indicating whether the gauge automatically scales to fill the host element.
         */
        get autoScale(): boolean {
            return this._autoScale;
        }
        set autoScale(value: boolean) {
            if (value != this._autoScale) {
                this._autoScale = asBoolean(value);
                this.invalidate();
            }
        }

        // virtual methods

        /**
         * Refreshes the control.
         *
         * @param fullUpdate Indicates whether to update the control layout as well as the content.
         */
        refresh(fullUpdate = true) {

            // clear viewbox
            this._setAttribute(this._svg, 'viewBox', null);

            // update gauge
            super.refresh(fullUpdate);

            // set viewbox to auto-scale
            if (this._autoScale) {
                var rc = Rect.fromBoundingRect(this._pFace.getBBox());
                if ((this.showText & ShowText.Value) != 0) {
                    rc = Rect.union(rc, Rect.fromBoundingRect(this._tValue.getBBox()));
                }
                if ((this.showText & ShowText.MinMax) != 0) {
                    rc = Rect.union(rc, Rect.fromBoundingRect(this._tMin.getBBox()));
                    rc = Rect.union(rc, Rect.fromBoundingRect(this._tMax.getBBox()));
                }
                var viewBox = [this._fix(rc.left), this._fix(rc.top), this._fix(rc.width), this._fix(rc.height)].join(' ');
                this._setAttribute(this._svg, 'viewBox', viewBox);
            }
        }

        // updates the element for a given range
        _updateRangeElement(e: SVGPathElement, rng: Range, value: number) {
            var rc = Rect.fromBoundingRect(this._dSvg.getBoundingClientRect()),
                center = new wijmo.Point(rc.width / 2, rc.height / 2),
                radius = Math.min(rc.width, rc.height) / 2,
                faceThickness = radius * this.thickness,
                rngThickness = faceThickness * rng.thickness,
                outer = radius - (faceThickness - rngThickness) / 2,
                inner = outer - rngThickness,
                start = this.startAngle + 180,
                sweep = this.sweepAngle,
                ps = this._getPercent(rng.min),
                pe = this._getPercent(value),
                rngStart = start + sweep * ps,
                rngSweep = sweep * (pe - ps);

            // update path
            this._updateSegment(e, center, outer, inner, rngStart, rngSweep);
        }

        // updates the content and position of the text elements
        _updateText() {
            var rc = Rect.fromBoundingRect(this._dSvg.getBoundingClientRect()),
                center = new wijmo.Point(rc.width / 2, rc.height / 2),
                outer = Math.min(rc.width, rc.height) / 2,
                inner = Math.max(0, outer * (1 - this.thickness)),
                start = this.startAngle + 180,
                sweep = this.sweepAngle;

            // hide min/max if sweep angle > 300 degrees
            var display = (this.showText & ShowText.MinMax) == 0 || Math.abs(sweep) > 300 ? 'none' : null;
            this._setAttribute(this._tMin, 'display', display);
            this._setAttribute(this._tMax, 'display', display);

            // update text/position
            this._centerText(this._tValue, this.value, center);
            this._centerText(this._tMin, this.min, this._getPoint(center, start - 10, (outer + inner) / 2));
            this._centerText(this._tMax, this.max, this._getPoint(center, start + sweep + 10, (outer + inner) / 2));
        }

        // draws a radial segment at the specified position
        _updateSegment(path: SVGPathElement, ctr: Point, rOut: number, rIn: number, start: number, sweep: number) {
            sweep = Math.min(Math.max(sweep, -359.99), 359.99);
            var p1 = this._getPoint(ctr, start, rIn),
                p2 = this._getPoint(ctr, start, rOut),
                p3 = this._getPoint(ctr, start + sweep, rOut),
                p4 = this._getPoint(ctr, start + sweep, rIn);
            var data = {
                large: Math.abs(sweep) > 180 ? 1 : 0,
                cw: sweep > 0 ? 1 : 0,
                ccw: sweep > 0 ? 0 : 1,
                or: this._fix(rOut),
                ir: this._fix(rIn),
                p1: this._fix(p1),
                p2: this._fix(p2),
                p3: this._fix(p3),
                p4: this._fix(p4)
            };
            var content = wijmo.format('M {p1} ' +
                'L {p2} A {or} {or} 0 {large} {cw} {p3} ' +
                'L {p4} A {ir} {ir} 0 {large} {ccw} {p1} Z',
                data);
            path.setAttribute('d', content);
        }

        // converts polar to cartesian coordinates
        _getPoint(ctr: Point, angle: number, radius: number): Point {
            angle = angle * Math.PI / 180;
            return new Point(
                ctr.x + radius * Math.cos(angle),
                ctr.y + radius * Math.sin(angle));
        }

        // gets the gauge value at a given point (in gauge client coordinates)
        _getValueFromPoint(pt: Point) {

            // account for viewBox
            // http://stackoverflow.com/questions/11715966/change-in-mouse-position-when-viewbox-is-added
            if (this.autoScale) {
                var m = this._pFace.getCTM(),
                    p = this._svg.createSVGPoint();
                p.x = pt.x;
                p.y = pt.y;
                p = p.matrixTransform(m.inverse());
                pt.x = p.x;
                pt.y = p.y;
            }

            // calculate geometry
            var rc = Rect.fromBoundingRect(this._dSvg.getBoundingClientRect()),
                center = new wijmo.Point(rc.width / 2, rc.height / 2),
                outer = Math.min(rc.width, rc.height) / 2,
                inner = outer * (1 - this.thickness),
                dx = pt.x - center.x,
                dy = pt.y - center.y;

            // check that the point is within the face
            var r2 = dy * dy + dx * dx;
            if (r2 > outer * outer || r2 < inner * inner) {
                return null;
            }

            // calculate angle, percentage
            var ang = (Math.PI - Math.atan2(-dy, dx)) * 180 / Math.PI;
            while (ang < this.startAngle) ang += 360;
            while (ang > this.startAngle + this.sweepAngle) ang -= 360;
            var pct = (ang - this.startAngle) / this.sweepAngle;
            return this.min + pct * (this.max - this.min);
        }
    }
}

module wijmo.gauge {
    'use strict';

    /**
     * The @see:BulletGraph is a type of linear gauge designed specifically for use
     * in dashboards. It displays a single key measure along with a comparative
     * measure and qualitative ranges to instantly signal whether the measure is
     * good, bad, or in some other state.
     *
     * Bullet Graphs were created and popularized by dashboard design expert 
     * Stephen Few. You can find more details and examples on 
     * <a href="http://en.wikipedia.org/wiki/Bullet_graph">Wikipedia</a>.
     *
     * @fiddle:8uxb1vwf
     */
    export class BulletGraph extends LinearGauge {

        // child ranges
        _rngTarget: Range;
        _rngGood: Range;
        _rngBad: Range;

        /**
         * Initializes a new instance of a @see:BulletGraph control.
         *
         * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
         * @param options The JavaScript object containing initialization data for the control.
         */
        constructor(element: any, options?) {
            super(element, null);

            // customize
            addClass(this.hostElement, 'wj-bulletgraph');
            this._pointer.thickness = .35;

            // add reference ranges
            this._rngTarget = new Range('target');
            this._rngTarget.thickness = .8;
            this._rngTarget.color = 'black';
            this._rngGood = new Range('good');
            this._rngGood.color = 'rgba(0,0,0,.15)';
            this._rngBad = new Range('bad');
            this._rngBad.color = 'rgba(0,0,0,.3)';
            this.ranges.push(this._rngBad);
            this.ranges.push(this._rngGood);
            this.ranges.push(this._rngTarget);

            // initialize control options
            this.initialize(options);
        }

        /**
         * Gets or sets the target value for the measure.
         */
        get target(): number {
            return this._rngTarget.max;
        }
        set target(value: number) {
            this._rngTarget.max = value;
        }
        /**
         * Gets or sets a reference value considered good for the measure.
         */
        get good(): number {
            return this._rngGood.max;
        }
        set good(value: number) {
            this._rngGood.max = value;
        }
        /**
         * Gets or sets a reference value considered bad for the measure.
         */
        get bad(): number {
            return this._rngBad.max;
        }
        set bad(value: number) {
            this._rngBad.max = value;
        }

        // ** implementation

        // gets a rectangle that represents a Range
        _getRangeRect(rng: Range, value = rng.max): Rect {

            // let base class calculate the rectangle
            var rc = super._getRangeRect(rng, value);

            // make target range rect look like a bullet
            if (rng == this._rngTarget) {
                switch (this.direction) {
                    case GaugeDirection.Right:
                        rc.left = rc.right - 1;
                        rc.width = 3;
                        break;
                    case GaugeDirection.Left:
                        rc.width = 3;
                        break;
                    case GaugeDirection.Up:
                        rc.height = 3;
                        break;
                    case GaugeDirection.Down:
                        rc.top = rc.bottom - 1;
                        rc.height = 3;
                        break;
                }
            }

            // done
            return rc;
        }
    }
}

module wijmo.gauge {
    'use strict';

    /**
     * Defines ranges to be used with @see:Gauge controls.
     *
     * @see:Range objects have @see:min and @see:max properties that
     * define the range's domain, as well as @see:color and @see:thickness
     * properties that define the range's appearance.
     *
     * Every @see:Gauge control has at least two ranges: 
     * the 'face' defines the minimum and maximum values for the gauge, and
     * the 'pointer' displays the gauge's current value.
     *
     * In addition to the built-in ranges, gauges may have additional
     * ranges used to display regions of significance (for example, 
     * low, medium, and high values).
     */
    export class Range {
        static _ctr = 0;
        private _min = 0;
        private _max = 100;
        private _thickness = 1;
        private _color: string;
        private _name: string;

        /**
         * Initializes a new instance of a @see:Range.
         *
         * @param name The name of the range.
         */
        constructor(name?: string) {
            this._name = name;
        }

        /**
         * Gets or sets the minimum value for this range.
         */
        get min(): number {
            return this._min;
        }
        set min(value: number) {
            this._setProp('_min', asNumber(value));
        }
        /**
         * Gets or sets the maximum value for this range.
         */
        get max(): number {
            return this._max;
        }
        set max(value: number) {
            this._setProp('_max', asNumber(value));
        }
        /**
         * Gets or sets the color used to display this range.
         */
        get color(): string {
            return this._color;
        }
        set color(value: string) {
            this._setProp('_color', asString(value));
        }
        /**
         * Gets or sets the thickness of this range as a percentage of 
         * the parent gauge's thickness.
         */
        get thickness(): number {
            return this._thickness;
        }
        set thickness(value: number) {
            this._setProp('_thickness', clamp(asNumber(value), 0, 1));
        }
        /**
         * Gets or sets the name of this @see:Range.
         */
        get name(): string {
            return this._name;
        }
        set name(value: string) {
            this._setProp('_name', asString(value));
        }

        /**
         * Occurs when the value of a property changes.
         */
        propertyChanged = new Event();
        /**
         * Raises the @see:propertyChanged event.
         *
         * @param e @see:PropertyChangedEventArgs that contains the property
         * name, old, and new values.
         */
        onPropertyChanged(e: PropertyChangedEventArgs) {
            this.propertyChanged.raise(this, e);
        }

        // ** implementation

        // sets property value and notifies about the change
        _setProp(name: string, value: any) {
            var oldValue = this[name];
            if (value != oldValue) {
                this[name] = value;
                var e = new PropertyChangedEventArgs(name.substr(1), oldValue, value);
                this.onPropertyChanged(e);
            }
        }
    }
}


