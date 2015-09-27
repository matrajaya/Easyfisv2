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
declare module wijmo.chart {
    /**
    * Provides arguments for @see:Series events.
    */
    class RenderEventArgs extends EventArgs {
        public _engine: IRenderEngine;
        /**
        * Initializes a new instance of a @see:RenderEventArgs object.
        *
        * @param engine (@see:IRenderEngine) The rendering engine to use.
        */
        constructor(engine: IRenderEngine);
        /**
        * Gets the @see:IRenderEngine object to use for rendering the chart elements.
        */
        public engine : IRenderEngine;
    }
    /**
    * The @see:FlexChartBase control from which the FlexChart and FlexPie derive.
    */
    class FlexChartBase extends Control implements _IPalette {
        static _WIDTH: number;
        static _HEIGHT: number;
        static _SELECTION_THRESHOLD: number;
        public _items: any;
        public _cv: collections.ICollectionView;
        private _binding;
        private _palette;
        private _selectionMode;
        private _itemFormatter;
        public _selectionIndex: number;
        public _options: any;
        private _plotMargin;
        public _header: string;
        public _headerStyle: any;
        public _footer: string;
        public _footerStyle: any;
        public _legend: Legend;
        public _defPalette: string[];
        public _notifyCurrentChanged: boolean;
        public _rectFooter: Rect;
        public _rectHeader: Rect;
        public _rectChart: Rect;
        public _rectLegend: Rect;
        public _currentRenderEngine: IRenderEngine;
        public _legendHost: SVGGElement;
        private _needBind;
        private _toShow;
        private _toHide;
        public _tooltip: ChartTooltip;
        /**
        * Gets or sets the array or @see:ICollectionView object that contains the data used to create the chart.
        */
        public itemsSource : any;
        /**
        * Gets the @see:ICollectionView object that contains the chart data.
        */
        public collectionView : collections.ICollectionView;
        /**
        * Gets or sets the name of the property that contains the Y values.
        */
        public binding : string;
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
        public palette : string[];
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
        public plotMargin : any;
        /**
        * Gets the chart legend.
        */
        public legend : Legend;
        /**
        * Gets or sets the text displayed in the chart header.
        */
        public header : string;
        /**
        * Gets or sets the text displayed in the chart footer.
        */
        public footer : string;
        /**
        * Gets or sets the style of the chart header.
        */
        public headerStyle : any;
        /**
        * Gets or sets the style of the chart footer.
        */
        public footerStyle : any;
        /**
        * Gets or sets an enumerated value indicating whether or what is
        * selected when the user clicks the chart.
        */
        public selectionMode : SelectionMode;
        /**
        * Gets or sets the item formatter function that allows you to customize
        * the appearance of data points. See the Explorer sample's <a target="_blank"
        * href="http://demos.componentone.com/wijmo/5/Angular/Explorer/Explorer/#/chart/itemFormatter">
        * Item Formatter</a> for a demonstration.
        */
        public itemFormatter : Function;
        /**
        * Occurs before the chart starts rendering data.
        */
        public rendering: Event;
        /**
        * Occurs after the chart finishes rendering.
        */
        public rendered: Event;
        /**
        * Raises the @see:rendered event.
        *
        * @param e The @see:RenderEventArgs object used to render the chart.
        */
        public onRendered(e: RenderEventArgs): void;
        /**
        * Raises the @see:rendering event.
        *
        * @param e The @see:RenderEventArgs object used to render the chart.
        */
        public onRendering(e: RenderEventArgs): void;
        /**
        * Refreshes the chart.
        *
        * @param fullUpdate A value indicating whether to update the control layout as well as the content.
        */
        public refresh(fullUpdate?: boolean): void;
        private _cvCollectionChanged(sender, e);
        private _cvCurrentChanged(sender, e);
        /**
        * Gets a color from the palette by index.
        *
        * @param index The index of the color in the palette.
        */
        public _getColor(index: number): string;
        /**
        * Gets a lighter color from the palette by index.
        *
        * @param index The index of the color in the palette.
        */
        public _getColorLight(index: number): string;
        public _bindChart(): void;
        public _clearCachedValues(): void;
        public _render(engine: IRenderEngine): void;
        public _performBind(): void;
        public _refreshChart(): void;
        public _drawTitle(engine: IRenderEngine, rect: Rect, title: string, style: any, isFooter: boolean): Rect;
        public _toControl(pt: any, y?: number): Point;
        public _highlightItems(items: any, cls: any, selected: boolean): void;
        public _parseMargin(value: any): any;
        public _showToolTip(content: any, rect: any): void;
        public _hideToolTip(): void;
        private _clearTimeouts();
        public _getHostOffset(): Point;
        public _getHostSize(): Size;
        public _getHostComputedStyle(): CSSStyleDeclaration;
        public _find(elem: SVGElement, names: string[]): any[];
    }
    interface _IHitArea {
        contains(pt: Point): boolean;
        distance(pt: Point): number;
        tag: any;
    }
    class _KeyWords {
        private _keys;
        constructor();
        public replace(s: string, ht: HitTestInfo): string;
        public getValue(key: string, ht: HitTestInfo, fmt?: string): string;
    }
}

declare module wijmo.chart {
    /**
    * The @see:FlexPie control provides pie and doughnut charts with selectable
    * slices.
    *
    * To use the @see:FlexPie control, set the @see:itemsSource property to an
    * array containing the data and use the @see:binding and @see:bindingName
    * properties to set the properties that contain the item values and names.
    */
    class FlexPie extends FlexChartBase {
        private static _MARGIN;
        private _bindingName;
        private _areas;
        private _keywords;
        private _startAngle;
        private _innerRadius;
        private _offset;
        private _reversed;
        private _isAnimated;
        private _selectedItemPosition;
        private _selectedItemOffset;
        private _pieGroup;
        private _rotationAngle;
        private _center;
        private _radius;
        private _selectedOffset;
        private _selectedIndex;
        private _angles;
        private _selectionAnimationID;
        private _lbl;
        public _values: number[];
        public _labels: string[];
        public _pels: any[];
        public _sum: number;
        /**
        * Initializes a new instance of the @see:FlexPie control.
        *
        * @param element The DOM element that hosts the control, or a selector for the host element (e.g. '#theCtrl').
        * @param options A Javascript object containing initialization data for the control.
        */
        constructor(element: any, options?: any);
        /**
        * Gets or sets the name of the property that contains the name of the data item.
        */
        public bindingName : string;
        /**
        * Gets or sets the starting angle for the pie slices, in degrees.
        *
        * Angles are measured clockwise, starting at the 9 o'clock position.
        */
        public startAngle : number;
        /**
        * Gets or sets the offset of the slices from the pie center.
        *
        * The offset is measured as a fraction of the pie radius.
        */
        public offset : number;
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
        public innerRadius : number;
        /**
        * Gets or sets whether angles are reversed (counter-clockwise).
        *
        * The default value is false, which causes angles to be measured in
        * the clockwise direction.
        */
        public reversed : boolean;
        /**
        * Gets or sets the position of the selected slice.
        *
        * Setting this property to a value other than 'None' causes
        * the pie to rotate when an item is selected.
        *
        * Note that in order to select slices by clicking the chart,
        * you must set the @see:selectionMode property to "Point".
        */
        public selectedItemPosition : Position;
        /**
        * Gets or sets the offset of the selected slice from the pie center.
        *
        * Offsets are measured as a fraction of the pie radius.
        */
        public selectedItemOffset : number;
        /**
        * Gets or sets a value indicating whether to use animation when items are selected.
        *
        * See also the @see:selectedItemPosition and @see:selectionMode
        * properties.
        */
        public isAnimated : boolean;
        /**
        * Gets the chart's @see:Tooltip.
        */
        public tooltip : ChartTooltip;
        /**
        * Gets the point data label.
        */
        public dataLabel : PieDataLabel;
        /**
        * Gets a @see:HitTestInfo object with information about the specified point.
        *
        * @param pt The point to investigate, in window coordinates.
        * @param y The Y coordinate of the point (if the first parameter is a number).
        * @return A HitTestInfo object containing information about the point.
        */
        public hitTest(pt: any, y?: number): HitTestInfo;
        public _performBind(): void;
        public _render(engine: IRenderEngine): void;
        public _renderData(engine: IRenderEngine, rect: Rect, g: any): void;
        public _renderLabels(engine: IRenderEngine): void;
        public _drawSilce(engine: IRenderEngine, i: number, reversed: boolean, cx: number, cy: number, r: number, irad: number, angle: number, sweep: number): void;
        public _measureLegendItem(engine: IRenderEngine, name: string): Size;
        public _drawLegendItem(engine: IRenderEngine, rect: Rect, i: number, name: string): void;
        private _getLabelContent(ht, content);
        private _select(pointIndex, animate?);
        private _highlightCurrent();
        private _highlight(selected, pointIndex, animate?);
        public _animateSelectionAngle(target: number, duration: number): void;
    }
}

declare module wijmo.chart {
    /**
    * Specifies whether and how to stack the chart's data values.
    */
    enum Stacking {
        /** No stacking. Each series object is plotted independently. */
        None = 0,
        /** Stacked charts show how each value contributes to the total. */
        Stacked = 1,
        /** 100% stacked charts show how each value contributes to the total with the relative size of
        * each series representing its contribution to the total. */
        Stacked100pc = 2,
    }
    /**
    * Specifies what is selected when the user clicks the chart.
    */
    enum SelectionMode {
        /** Select neither series nor data points when the user clicks the chart. */
        None = 0,
        /** Select the whole @see:Series when the user clicks it on the chart. */
        Series = 1,
        /** Select the data point when the user clicks it on the chart. Since Line, Area, Spline,
        * and SplineArea charts do not render individual data points, nothing is selected with this
        * setting on those chart types. */
        Point = 2,
    }
    /**
    * The core charting control for @see:FlexChart.
    *
    */
    class FlexChartCore extends FlexChartBase {
        static _CSS_AXIS_X: string;
        static _CSS_AXIS_Y: string;
        static _CSS_LINE: string;
        static _CSS_GRIDLINE: string;
        static _CSS_TICK: string;
        static _CSS_GRIDLINE_MINOR: string;
        static _CSS_TICK_MINOR: string;
        static _CSS_LABEL: string;
        static _CSS_LEGEND: string;
        static _CSS_HEADER: string;
        static _CSS_FOOTER: string;
        static _CSS_TITLE: string;
        static _CSS_SELECTION: string;
        static _CSS_PLOT_AREA: string;
        static _FG: string;
        private _series;
        private _axes;
        private _axisX;
        private _axisY;
        private _selection;
        private _interpolateNulls;
        private _legendToggle;
        private _symbolSize;
        private _dataInfo;
        public _plotRect: Rect;
        private __barPlotter;
        private __linePlotter;
        private __areaPlotter;
        private __bubblePlotter;
        private __financePlotter;
        private _plotters;
        private _bindingX;
        private _rotated;
        public _stacking: Stacking;
        private _lbl;
        public _xlabels: string[];
        public _xvals: number[];
        public _xDataType: DataType;
        private _hitTester;
        private _keywords;
        /**
        * Initializes a new instance of the @see:FlexChart control.
        *
        * @param element The DOM element that will host the control, or a selector for the host element (e.g. '#theCtrl').
        * @param options A JavaScript object containing initialization data for the control.
        */
        constructor(element: any, options?: any);
        /**
        * Gets the collection of @see:Series objects.
        */
        public series : collections.ObservableArray;
        /**
        * Gets the collection of @see:Axis objects.
        */
        public axes : collections.ObservableArray;
        /**
        * Gets the main X axis.
        */
        public axisX : Axis;
        /**
        * Gets the main Y axis.
        */
        public axisY : Axis;
        /**
        * Gets or sets the name of the property that contains the X data values.
        */
        public bindingX : string;
        /**
        * Gets or sets the size of the symbols used for all Series objects in this @see:FlexChart.
        *
        * This property may be overridden by the symbolSize property on each @see:Series object.
        */
        public symbolSize : number;
        /**
        * Gets or sets whether to interpolate null values in the data.
        *
        * If true, the chart interpolates the value of any missing data
        * based on neighboring points. If false, it leaves a break in
        * lines and areas at the points with null values.
        */
        public interpolateNulls : boolean;
        /**
        * Gets or sets a value indicating whether clicking legend items toggles the
        * series visibility in the chart.
        */
        public legendToggle : boolean;
        /**
        * Gets or sets a value indicating whether to flip the axes so X is vertical and Y horizontal.
        */
        public rotated : boolean;
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
        public options : any;
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
        public tooltip : ChartTooltip;
        /**
        * Gets the point data label.
        */
        public dataLabel : DataLabel;
        /**
        * Gets or sets the selected chart series.
        */
        public selection : SeriesBase;
        /**
        * Occurs after the selection changes, whether programmatically
        * or when the user clicks the chart. This is useful, for example,
        * when you want to update details in a textbox showing the current
        * selection.
        */
        public selectionChanged: Event;
        /**
        * Raises the @see:selectionChanged event.
        */
        public onSelectionChanged(e?: EventArgs): void;
        /**
        * Occurs when the series visibility changes, for example when the legendToggle
        * property is set to true and the user clicks the legend.
        */
        public seriesVisibilityChanged: Event;
        /**
        * Raises the @see:seriesVisibilityChanged event.
        *
        * @param e The @see:SeriesEventArgs object that contains the event data.
        */
        public onSeriesVisibilityChanged(e: SeriesEventArgs): void;
        /**
        * Gets a @see:HitTestInfo object with information about the specified point.
        *
        * @param pt The point to investigate, in window coordinates.
        * @param y The Y coordinate of the point (if the first parameter is a number).
        * @return A HitTestInfo object with information about the point.
        */
        public hitTest(pt: any, y?: number): HitTestInfo;
        /**
        * Converts a @see:Point from control coordinates to chart data coordinates.
        *
        * @param pt The point to convert, in control coordinates.
        * @param y The Y coordinate of the point (if the first parameter is a number).
        * @return The point in chart data coordinates.
        */
        public pointToData(pt: any, y?: number): Point;
        /**
        * Converts a @see:Point from data coordinates to control coordinates.
        *
        * @param pt @see:Point in data coordinates, or X coordinate of a point in data coordinates.
        * @param y Y coordinate of the point (if the first parameter is a number).
        * @return The @see:Point in control coordinates.
        */
        public dataToPoint(pt: any, y?: number): Point;
        public _copy(key: string, value: any): boolean;
        public _createSeries(): SeriesBase;
        public _clearCachedValues(): void;
        public _performBind(): void;
        public _hitTestSeries(pt: Point, seriesIndex: number): HitTestInfo;
        public _hitTestData(pt: any): HitTestInfo;
        private static _dist2(p1, p2);
        static _dist(p0: Point, p1: Point, p2: Point): number;
        static _distToSegmentSquared(p: Point, v: Point, w: Point): number;
        private _isRotated();
        public _plotrectId: string;
        public _getChartType(): ChartType;
        public _render(engine: IRenderEngine): void;
        private _renderLabels(engine);
        private _getAxes();
        private _clearPlotters();
        public _initPlotter(plotter: _IPlotter): void;
        private _barPlotter;
        private _linePlotter;
        private _areaPlotter;
        private _bubblePlotter;
        private _financePlotter;
        public _getPlotter(series: SeriesBase): _IPlotter;
        private _layout(rect, size, engine);
        private _convertX(x, left, right);
        private _convertY(y, top, bottom);
        private _getLabelContent(ht, content);
        private _select(newSelection, pointIndex);
        private _highlightCurrent();
        private _highlight(series, selected, pointIndex);
        public _updateAuxAxes(axes: Axis[], isRotated: boolean): void;
        static _contains(rect: Rect, pt: Point): boolean;
        static _intersects(rect1: Rect, rect2: Rect): boolean;
        static _epoch: number;
        static _msPerDay: number;
        static _toOADate(date: Date): number;
        static _fromOADate(val: number): Date;
        static _renderText(engine: IRenderEngine, text: string, pos: Point, halign: any, valign: any, className?: string, groupName?: string, style?: any, test?: any): Rect;
        static _renderRotatedText(engine: IRenderEngine, text: string, pos: Point, halign: any, valign: any, center: Point, angle: number, className: string, style?: any): void;
    }
    /**
    * Analyzes chart data.
    */
    class _DataInfo {
        private minY;
        private maxY;
        private minX;
        private maxX;
        private minXp;
        private minYp;
        private dataTypeX;
        private dataTypeY;
        private stackAbs;
        private _xvals;
        private dx;
        constructor();
        public analyse(seriesList: any, isRotated: boolean, stacking: Stacking, xvals: number[], logx: boolean, logy: boolean): void;
        public getMinY(): number;
        public getMaxY(): number;
        public getMinX(): number;
        public getMaxX(): number;
        public getMinXp(): number;
        public getMinYp(): number;
        public getDeltaX(): number;
        public getDataTypeX(): DataType;
        public getDataTypeY(): DataType;
        public getStackedAbsSum(key: number): number;
        public getXVals(): number[];
        static isValid(value: number): boolean;
    }
    /**
    * Represents the chart palette.
    */
    interface _IPalette {
        _getColor(i: number): string;
        _getColorLight(i: number): string;
    }
    /**
    * Extends the @see:Tooltip class to provide chart tooltips.
    */
    class ChartTooltip extends Tooltip {
        private _content;
        private _threshold;
        /**
        * Initializes a new instance of a @see:ChartTooltip.
        */
        constructor();
        /**
        * Gets or sets the tooltip content.
        */
        public content : any;
        /**
        * Gets or sets the maximum distance from the element to display the tooltip.
        */
        public threshold : number;
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
declare module wijmo.chart {
    /**
    * Specifies the chart type.
    */
    enum ChartType {
        /** Column charts show vertical bars and allow you to compare values of items across categories. */
        Column = 0,
        /** Bar charts show horizontal bars. */
        Bar = 1,
        /** Scatter charts use X and Y coordinates to show patterns within the data. */
        Scatter = 2,
        /** Line charts show trends over a period of time or across categories. */
        Line = 3,
        /** Line and symbol charts are line charts with a symbol on each data point. */
        LineSymbols = 4,
        /** Area charts are line charts with the area below the line filled with color. */
        Area = 5,
        /** Bubble charts are Scatter charts with a
        * third data value that determines the size of the symbol. */
        Bubble = 6,
        /** Candlestick charts present items with high, low, open, and close values.
        * The size of the wick line is determined by the High and Low values, while the size of the bar is
        * determined by the Open and Close values. The bar is displayed using different colors, depending on
        * whether the close value is higher or lower than the open value. */
        Candlestick = 7,
        /** High-low-open-close charts display the same information as a candlestick chart, except that opening
        * values are displayed using lines to the left, while lines to the right indicate closing values.  */
        HighLowOpenClose = 8,
        /** Spline charts are line charts that plot curves rather than angled lines through the data points. */
        Spline = 9,
        /** Spline and symbol charts are spline charts with symbols on each data point. */
        SplineSymbols = 10,
        /** Spline area charts are spline charts with the area below the line filled with color. */
        SplineArea = 11,
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
    class FlexChart extends FlexChartCore {
        private _chartType;
        /**
        * Initializes a new instance of the @see:FlexChart control.
        *
        * @param element The DOM element that will host the control, or a selector for the host element (e.g. '#theCtrl').
        * @param options A JavaScript object containing initialization data for the control.
        */
        constructor(element: any, options?: any);
        public _getChartType(): ChartType;
        /**
        * Gets or sets the type of chart to create.
        */
        public chartType : ChartType;
        /**
        * Gets or sets whether and how series objects are stacked.
        */
        public stacking : Stacking;
    }
}

declare module wijmo.chart {
    /**
    * Specifies the position of an axis or legend on the chart.
    */
    enum Position {
        /** The item is not visible. */
        None = 0,
        /** The item appears to the left of the chart. */
        Left = 1,
        /** The item appears above the chart. */
        Top = 2,
        /** The item appears to the right of the chart. */
        Right = 3,
        /** The item appears below the chart. */
        Bottom = 4,
    }
    /**
    * Specifies the axis type.
    */
    enum AxisType {
        /** Category axis (normally horizontal). */
        X = 0,
        /** Value axis (normally vertical). */
        Y = 1,
    }
    /**
    * Specifies how to handle overlapping labels.
    */
    enum OverlappingLabels {
        /**
        * Hide overlapping labels.
        */
        Auto = 0,
        /**
        * Show all labels, including overlapping ones.
        */
        Show = 1,
    }
    /**
    * Axis interface.
    */
    interface _IAxis {
        actualMin: number;
        actualMax: number;
        convert(val: number): number;
    }
    /**
    * Specifies whether and where axis tick marks appear.
    */
    enum TickMark {
        /** No tick marks appear. */
        None = 0,
        /** Tick marks appear outside the plot area. */
        Outside = 1,
        /** Tick marks appear inside the plot area. */
        Inside = 2,
        /** Tick marks cross the axis. */
        Cross = 3,
    }
    /**
    * Represents an axis in the chart.
    */
    class Axis implements _IAxis {
        private _GRIDLINE_WIDTH;
        private _LINE_WIDTH;
        private _TICK_WIDTH;
        private _TICK_HEIGHT;
        private _TICK_OVERLAP;
        private _TICK_LABEL_DISTANCE;
        public _chart: FlexChartCore;
        private _type;
        private _min;
        private _max;
        private _position;
        private _majorUnit;
        private _minorUnit;
        private _majorGrid;
        private _minorGrid;
        private _title;
        private _labelStyle;
        private _reversed;
        private _format;
        private _actualMin;
        private _actualMax;
        private _axisType;
        private _majorTickMarks;
        private _minorTickMarks;
        private _logBase;
        private _labels;
        private _labelAngle;
        private _axisLine;
        private _plotrect;
        private _szTitle;
        private _isTimeAxis;
        private _lbls;
        private _values;
        private _rects;
        private _name;
        private _origin;
        private _overlap;
        private _items;
        private _cv;
        private _binding;
        private _ifmt;
        private static _id;
        private __uniqueId;
        public _axrect: Rect;
        public _desiredSize: Size;
        public _annoSize: Size;
        public _hasOrigin: boolean;
        /**
        * Initializes a new instance of an @see:Axis object.
        *
        * @param position The position of the axis on the chart.
        */
        constructor(position: Position);
        /**
        * Gets the actual axis minimum.
        */
        public actualMin : number;
        /**
        * Gets the actual axis maximum.
        */
        public actualMax : number;
        /**
        * Gets or sets the minimum value shown on the axis.
        * If not set, the minimum is calculated automatically.
        */
        public min : number;
        /**
        * Gets or sets the maximum value shown on the axis.
        * If not set, the maximum is calculated automatically.
        */
        public max : number;
        /**
        * Gets or sets a value indicating whether the axis is
        * reversed (top to bottom or right to left).
        */
        public reversed : boolean;
        /**
        * Gets or sets the enumerated axis position.
        */
        public position : Position;
        /**
        * Gets or sets the number of units between axis labels.
        *
        * If the axis contains date values, then the units are
        * expressed in days.
        */
        public majorUnit : number;
        /**
        * Gets or sets the number of units between minor axis ticks.
        *
        * If the axis contains date values, then the units are
        * expressed in days.
        */
        public minorUnit : number;
        /**
        * Gets or sets the axis name.
        */
        public name : string;
        /**
        * Gets or sets the title text shown next to the axis.
        */
        public title : string;
        /**
        * Gets or sets the format string used for axis labels
        * (see @see:wijmo.Globalize).
        */
        public format : string;
        /**
        * Gets or sets a value indicating whether the axis includes grid lines.
        */
        public majorGrid : boolean;
        /**
        * Gets or sets the location of axis tick marks.
        */
        public majorTickMarks : TickMark;
        /**
        * Gets or sets a value indicating whether the axis includes minor grid lines.
        */
        public minorGrid : boolean;
        /**
        * Gets or sets the location of minor axis tick marks.
        */
        public minorTickMarks : TickMark;
        /**
        * Gets or sets a value indicating whether the axis line is visible.
        */
        public axisLine : boolean;
        /**
        * Gets or sets a value indicating whether axis labels are visible.
        */
        public labels : boolean;
        /**
        * Gets or sets the rotation angle of axis labels.
        *
        * The angle is measured in degrees with valid values
        * ranging from -90 to 90.
        */
        public labelAngle : number;
        /**
        * Gets or sets the value at which the axis crosses the perpendicular axis.
        **/
        public origin : number;
        /**
        * Gets or sets a value indicating how to handle overlapping axis labels.
        */
        public overlappingLabels : OverlappingLabels;
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
        public itemsSource : any;
        /**
        * Gets or sets the comma-separated property names for the @see:wijmo.chart.Axis.itemsSource
        * property to use in axis labels.
        *
        * The first name specifies the value on the axis, the second represents the corresponding
        * axis label. The default value is 'value,text.'
        */
        public binding : string;
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
        public itemFormatter : Function;
        /**
        * Gets or sets the logarithmic base of the axis.
        *
        * If the base is not specified the axis uses the normal scale.
        */
        public logBase : number;
        /**
        * Occurs when the axis range changes.
        */
        public rangeChanged: Event;
        /**
        * Raises the @see:rangeChanged event.
        */
        public onRangeChanged(): void;
        /**
        * Calculates the axis height.
        *
        * @param engine Rendering engine.
        */
        public _getHeight(engine: IRenderEngine): number;
        /**
        * Update actual axis limits based on specified data range.
        *
        * @param dataType Data type.
        * @param dataMin Data minimum.
        * @param dataMax Data maximum.
        * @param labels Category labels(category axis).
        * @param values Values(value axis).
        */
        public _updateActualLimits(dataType: DataType, dataMin: number, dataMax: number, labels?: string[], values?: number[]): void;
        /**
        * Set axis position.
        *
        * @param axisRect Axis rectangle.
        * @param plotRect Plot area rectangle.
        */
        public _layout(axisRect: Rect, plotRect: Rect): void;
        /**
        * Render the axis.
        *
        * @param engine Rendering engine.
        */
        public _render(engine: IRenderEngine): void;
        public _createMinors(engine: IRenderEngine, vals: number[], isVert: boolean, isNear: boolean): void;
        private _renderMinors(engine, ticks, isVert, isNear);
        private _renderLabel(engine, val, text, pos, ha, va, className?);
        /**
        * Converts the specified value from data to pixel coordinates.
        *
        * @param val The data value to convert.
        */
        public convert(val: number): number;
        /**
        * Converts the specified value from pixel to data coordinates.
        *
        * @param val The pixel coordinates to convert back.
        */
        public convertBack(val: number): number;
        /**
        * Gets the axis type.
        */
        public axisType : AxisType;
        private _invalidate();
        private _cvCollectionChanged(sender, e);
        private _createLabels(start, len, delta, vals, lbls);
        private _createLogarithmicLabels(min, max, unit, vals, lbls, isLabels);
        private _createTimeLabels(start, len, vals, lbls);
        public _formatValue(val: number): string;
        private _calcMajorUnit();
        private _getAnnoNumber(isVert);
        private _nicePrecision(range);
        private _niceTickNumber(x);
        private _niceNumber(x, exp, round);
        public _uniqueId : number;
    }
    /**
    * Represents a collection of @see:Axis objects in a @see:FlexChart control.
    */
    class AxisCollection extends collections.ObservableArray {
        /**
        * Gets an axis by name.
        *
        * @param name The name of the axis to look for.
        * @return The axis object with the specified name, or null if not found.
        */
        public getAxis(name: string): Axis;
        /**
        * Gets the index of an axis by name.
        *
        * @param name The name of the axis to look for.
        * @return The index of the axis with the specified name, or -1 if not found.
        */
        public indexOf(name: string): number;
    }
}

declare module wijmo.chart {
    /**
    * Specifies whether and where the Series is visible.
    */
    enum SeriesVisibility {
        /** The series is visible on the plot and in the legend. */
        Visible = 0,
        /** The series is visible only on the plot. */
        Plot = 1,
        /** The series is visible only in the legend. */
        Legend = 2,
        /** The series is hidden. */
        Hidden = 3,
    }
    /**
    * Specifies the type of marker to use for the @see:symbolMarker property.
    * Applies to Scatter, LineSymbols, and SplineSymbols chart types.
    */
    enum Marker {
        /**
        * Uses a circle to mark each data point.
        */
        Dot = 0,
        /**
        * Uses a square to mark each data point.
        */
        Box = 1,
    }
    /**
    * Data series interface
    */
    interface _ISeries {
        style: any;
        symbolStyle: any;
        getValues: (dim: number) => number[];
        getDataType: (dim: number) => DataType;
        drawLegendItem(engine: IRenderEngine, rect: Rect): any;
        measureLegendItem(engine: IRenderEngine): Size;
        _setPointIndex(pointIndex: number, elementIndex: number): any;
    }
    /**
    * Provides arguments for @see:Series events.
    */
    class SeriesEventArgs extends EventArgs {
        public _series: Series;
        /**
        * Initializes a new instance of a @see:SeriesEventArgs object.
        *
        * @param series Specifies the @see:Series object affected by this event.
        */
        constructor(series: SeriesBase);
        /**
        * Gets the @see:Series object affected by this event.
        */
        public series : SeriesBase;
    }
    /**
    * Represents a series of data points to display in the chart.
    *
    */
    class SeriesBase implements _ISeries {
        static _LEGEND_ITEM_WIDTH: number;
        static _LEGEND_ITEM_HEIGHT: number;
        static _LEGEND_ITEM_MARGIN: number;
        private static _DEFAULT_SYM_SIZE;
        public _chart: FlexChartCore;
        private _name;
        private _binding;
        private _showValues;
        private _symbolStyle;
        private _symbolSize;
        private _style;
        private _cv;
        private _itemsSource;
        private _values;
        private _valueDataType;
        public _chartType: ChartType;
        private _symbolMarker;
        private _bindingX;
        private _xvalues;
        private _xvalueDataType;
        private _cssClass;
        private _visibility;
        private _axisX;
        private _axisY;
        public _legendElement: SVGAElement;
        public _hostElement: SVGGElement;
        public _pointIndexes: number[];
        constructor();
        /**
        * Gets or sets the series style.
        */
        public style : any;
        /**
        * Gets or sets the series symbol style.
        * Applies to Scatter, LineSymbols, and SplineSymbols chart types.
        */
        public symbolStyle : any;
        /**
        * Gets or sets the size in pixels of the symbols used to render this @see:Series.
        * Applies to Scatter, LineSymbols, and SplineSymbols chart types.
        */
        public symbolSize : number;
        public _getSymbolSize(): number;
        /**
        * Gets or sets the shape of marker to use for each data point in the series.
        * Applies to Scatter, LineSymbols, and SplineSymbols chart types.
        */
        public symbolMarker : Marker;
        /**
        * Gets or sets the name of the property that contains Y values for the series.
        */
        public binding : string;
        /**
        * Gets or sets the name of the property that contains X values for the series.
        */
        public bindingX : string;
        /**
        * Gets or sets the series name.
        *
        * The series name is displayed in the chart legend. Any series without a name
        * does not appear in the legend.
        */
        public name : string;
        /**
        * Gets or sets the array or @see:ICollectionView object that contains the series data.
        */
        public itemsSource : any;
        /**
        * Gets the @see:ICollectionView object that contains the data for this series.
        */
        public collectionView : collections.ICollectionView;
        /**
        * Gets the @see:FlexChart object that owns this series.
        */
        public chart : FlexChartCore;
        /**
        * Gets the series host element.
        */
        public hostElement : SVGGElement;
        /**
        * Gets the series element in the legend.
        */
        public legendElement : SVGGElement;
        /**
        * Gets or sets the series CSS class.
        */
        public cssClass : string;
        /**
        * Gets or sets an enumerated value indicating whether and where the series appears.
        */
        public visibility : SeriesVisibility;
        /**
        * Occurs when series is rendering.
        */
        public rendering: Event;
        /**
        * Raises the @see:rendering event.
        *
        * @param engine The @see:IRenderEngine object used to render the series.
        */
        public onRendering(engine: IRenderEngine): void;
        /**
        * Gets a @see:HitTestInfo object with information about the specified point.
        *
        * @param pt The point to investigate, in window coordinates.
        * @param y The Y coordinate of the point (if the first parameter is a number).
        */
        public hitTest(pt: any, y?: number): HitTestInfo;
        /**
        * Gets the plot element that corresponds to the specified point index.
        *
        * @param pointIndex The index of the data point.
        */
        public getPlotElement(pointIndex: number): any;
        /**
        * Gets or sets the x-axis for the series.
        */
        public axisX : Axis;
        /**
        * Gets or sets the y-axis for the series.
        */
        public axisY : Axis;
        public getDataType(dim: number): DataType;
        public getValues(dim: number): number[];
        /**
        * Draw a legend item at the specified position.
        *
        * @param engine The rendering engine to use.
        * @param rect The position of the legend item.
        */
        public drawLegendItem(engine: IRenderEngine, rect: Rect): void;
        /**
        * Measures the height and width of the legend item.
        *
        * @param engine The rendering engine to use.
        */
        public measureLegendItem(engine: IRenderEngine): Size;
        /**
        * Returns series bounding rectangle in data coordinates.
        *
        * If getDataRect() returns null the limits are calculated automatically based on data values.
        */
        public getDataRect(): Rect;
        public _getChartType(): ChartType;
        /**
        * Clears any cached data values.
        */
        public _clearValues(): void;
        public _getBinding(index: number): string;
        public _getBindingValues(index: number): number[];
        public _getItem(pointIndex: number): any;
        public _getLength(): number;
        public _setPointIndex(pointIndex: number, elementIndex: number): void;
        private _getDataRect();
        public _isCustomAxisX(): boolean;
        public _isCustomAxisY(): boolean;
        public _getAxisX(): Axis;
        public _getAxisY(): Axis;
        private _cvCollectionChanged(sender, e);
        private _cvCurrentChanged(sender, e);
        private _bindValues(items, binding, isX?);
        public _invalidate(): void;
        public _indexToPoint(pointIndex: number): Point;
        public _getSymbolFill(seriesIndex?: number): string;
        public _getSymbolStroke(seriesIndex?: number): string;
    }
}

declare module wijmo.chart {
    /**
    * Represents a series of data points to display in the chart.
    *
    * The @see:Series class supports all basic chart types. You may define
    * a different chart type on each @see:Series object that you add to the
    * @see:FlexChart series collection, and it overrides the @see:chartType
    * property set on the chart that is the default for all @see:Series objects
    * in its collection.
    */
    class Series extends SeriesBase {
        /**
        * Gets or sets the chart type for a specific series, overriding the chart type
        * set on the overall chart.
        */
        public chartType : ChartType;
    }
}

declare module wijmo.chart {
    /**
    * Represents a rendering engine that performs basic drawing routines.
    */
    interface IRenderEngine {
        /**
        * Clear the viewport and start the rendering cycle.
        */
        beginRender(): any;
        /**
        * Finish the rendering cycle.
        */
        endRender(): any;
        /**
        * Set the size of the viewport.
        *
        * @param w Viewport width.
        * @param h Viewport height.
        */
        setViewportSize(w: number, h: number): any;
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
        drawEllipse(cx: number, cy: number, rx: number, ry: number, className?: string, style?: any): any;
        drawRect(x: number, y: number, w: number, h: number, className?: string, style?: any, clipPath?: string): any;
        drawLine(x1: number, y1: number, x2: number, y2: number, className?: string, style?: any): any;
        drawLines(xs: number[], ys: number[], className?: string, style?: any, clipPath?: string): any;
        drawSplines(xs: number[], ys: number[], className?: string, style?: any, clipPath?: string): any;
        drawPolygon(xs: number[], ys: number[], className?: string, style?: any, clipPath?: string): any;
        drawPieSegment(cx: number, cy: number, radius: number, startAngle: number, sweepAngle: number, className?: string, style?: any, clipPath?: string): any;
        drawDonutSegment(cx: number, cy: number, radius: number, innerRadius: number, startAngle: number, sweepAngle: number, className?: string, style?: any, clipPath?: string): any;
        drawString(s: string, pt: Point, className?: string, style?: any): any;
        drawStringRotated(label: string, pt: Point, center: Point, angle: number, className?: string, style?: any): any;
        drawImage(imageHref: string, x: number, y: number, w: number, h: number): any;
        measureString(s: string, className?: string, groupName?: string, style?: any): Size;
        startGroup(className?: string, clipPath?: string, createTransform?: boolean): any;
        endGroup(): any;
        addClipRect(clipRect: Rect, id: string): any;
    }
}

declare module wijmo.chart {
    /**
    * Render to svg.
    */
    class _SvgRenderEngine implements IRenderEngine {
        private static svgNS;
        private static xlinkNS;
        private _element;
        private _svg;
        private _text;
        private _textGroup;
        private _defs;
        private _fill;
        private _stroke;
        private _textFill;
        private _strokeWidth;
        private _fontSize;
        private _fontFamily;
        private _group;
        private _clipRect;
        constructor(element: HTMLElement);
        public beginRender(): void;
        public endRender(): void;
        public setViewportSize(w: number, h: number): void;
        public fill : string;
        public fontSize : string;
        public fontFamily : string;
        public stroke : string;
        public strokeWidth : number;
        public textFill : string;
        public addClipRect(clipRect: Rect, id: string): void;
        public drawEllipse(cx: number, cy: number, rx: number, ry: number, className?: string, style?: any): void;
        public drawRect(x: number, y: number, w: number, h: number, className?: string, style?: any, clipPath?: string): void;
        public drawLine(x1: number, y1: number, x2: number, y2: number, className?: string, style?: any): void;
        public drawLines(xs: number[], ys: number[], className?: string, style?: any, clipPath?: string): void;
        public drawSplines(xs: number[], ys: number[], className?: string, style?: any, clipPath?: string): void;
        public drawPolygon(xs: number[], ys: number[], className?: string, style?: any, clipPath?: string): void;
        public drawPieSegment(cx: number, cy: number, r: number, startAngle: number, sweepAngle: number, className?: string, style?: any, clipPath?: string): void;
        public drawDonutSegment(cx: number, cy: number, radius: number, innerRadius: number, startAngle: number, sweepAngle: number, className?: string, style?: any, clipPath?: string): void;
        public drawString(s: string, pt: Point, className?: string, style?: any): void;
        public drawStringRotated(s: string, pt: Point, center: Point, angle: number, className?: string, style?: any): void;
        public measureString(s: string, className?: string, groupName?: string, style?: any): Size;
        public startGroup(className?: string, clipPath?: string, createTransform?: boolean): SVGGElement;
        public endGroup(): void;
        public drawImage(imageHref: string, x: number, y: number, w: number, h: number): void;
        private _appendChild(element);
        private _create();
        private _setText(element, s);
        private _createText(pos, text);
        private _applyStyle(el, style);
        private _deCase(s);
    }
}

declare module wijmo.chart {
    /**
    * Represents the chart legend.
    */
    class Legend {
        public _chart: FlexChartBase;
        public _position: Position;
        private _areas;
        private _sz;
        constructor(chart: FlexChartBase);
        /**
        * Gets or sets the enumerated value that determines whether and where the
        * legend appears in relation to the chart.
        */
        public position : Position;
        public _getDesiredSize(engine: IRenderEngine): Size;
        private _getDesiredSizeSeriesChart(engine, isVertical);
        private _renderSeriesChart(engine, pos, isVertical);
        private _getDesiredSizePieChart(engine, isVertical);
        private _renderPieChart(engine, pos, isVertical);
        public _render(engine: IRenderEngine, pos: Point): any;
        public _hitTest(pt: Point): number;
    }
}

declare module wijmo.chart {
    /**
    * The enumerated type of chart element that may be found by the hitTest method.
    */
    enum ChartElement {
        /** The area within the axes. */
        PlotArea = 0,
        /** X-axis. */
        AxisX = 1,
        /** Y-axis. */
        AxisY = 2,
        /** The area within the control but outside of the axes. */
        ChartArea = 3,
        /** The chart legend. */
        Legend = 4,
        /** The chart header. */
        Header = 5,
        /** The chart footer. */
        Footer = 6,
        /** A chart series. */
        Series = 7,
        /** A chart series symbol. */
        SeriesSymbol = 8,
        /** No chart element. */
        None = 9,
    }
    /**
    * Contains information about a part of a @see:FlexChart control at
    * a specified page coordinate.
    */
    class HitTestInfo {
        private _chart;
        private _pt;
        public _series: SeriesBase;
        public _pointIndex: number;
        public _chartElement: ChartElement;
        public _dist: number;
        public _item: any;
        private _x;
        private __xfmt;
        private _y;
        private __yfmt;
        /**
        * Initializes a new instance of a @see:HitTestInfo object.
        *
        * @param chart The chart control.
        * @param point The original point in window coordinates.
        */
        constructor(chart: FlexChartBase, point: Point);
        /**
        * Gets the point in control coordinates that this HitTestInfo object refers to.
        */
        public point : Point;
        /**
        * Gets the chart series at the specified coordinates.
        */
        public series : SeriesBase;
        /**
        * Gets the data point index at the specified coordinates.
        */
        public pointIndex : number;
        /**
        * Gets the chart element at the specified coordinates.
        */
        public chartElement : ChartElement;
        /**
        * Gets the distance from the closest data point.
        */
        public distance : number;
        /**
        * Gets the data object that corresponds to the closest data point.
        */
        public item : any;
        /**
        * Gets the x-value of the closest data point.
        */
        public x : any;
        /**
        * Gets the y-value of the closest data point.
        */
        public y : any;
        public value : any;
        public name : any;
        public _xfmt : any;
        public _yfmt : any;
        private _getValue(index, formatted);
    }
}

declare module wijmo.chart {
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
    class Palettes {
        static standard: string[];
        static cocoa: string[];
        static coral: string[];
        static dark: string[];
        static highcontrast: string[];
        static light: string[];
        static midnight: string[];
        static modern: string[];
        static organic: string[];
        static slate: string[];
        static zen: string[];
        static cyborg: string[];
        static superhero: string[];
        static flatly: string[];
        static darkly: string[];
        static cerulan: string[];
    }
}

declare module wijmo.chart {
    /**
    * Calculates Spline curves.
    */
    class _Spline {
        private k;
        private _x;
        private _y;
        private _a;
        private _b;
        private _c;
        private _d;
        private _len;
        private m;
        constructor(x: number[], y: number[]);
        private calculatePoint(val);
        public calculate(): {
            xs: any;
            ys: any;
        };
    }
}

declare module wijmo.chart {
    /**
    * Specifies the position of data labels on the chart.
    */
    enum LabelPosition {
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
        Center = 5,
    }
    /**
    * Specifies the position of data labels on the pie chart.
    */
    enum PieLabelPosition {
        /** No data labels. */
        None = 0,
        /** The label appears inside the pie slice. */
        Inside = 1,
        /** The item appears at the center of the pie slice. */
        Center = 2,
        /** The item appears outside the pie slice. */
        Outside = 3,
    }
    /**
    * Base abstract class for the @see:DataLabel and @see:PieDataLabel classes.
    */
    class DataLabelBase {
        private _content;
        public _chart: FlexChartBase;
        private _bdr;
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
        public content : any;
        /**
        * Gets or sets a value indicating whether data labels have borders.
        */
        public border : boolean;
        public _invalidate(): void;
    }
    /**
    * The point data label for FlexChart.
    */
    class DataLabel extends DataLabelBase {
        private _pos;
        /**
        * Gets or sets the position of data labels.
        */
        public position : LabelPosition;
    }
    /**
    * The point data label for FlexPie.
    */
    class PieDataLabel extends DataLabelBase {
        private _pos;
        /**
        * Gets or sets the position of the data labels.
        */
        public position : PieLabelPosition;
    }
}

declare module wijmo.chart {
    /**
    * Specifies the line type for the LineMarker.
    */
    enum LineMarkerLines {
        /** Show no lines. */
        None = 0,
        /** Show a vertical line. */
        Vertical = 1,
        /** Show a horizontal line. */
        Horizontal = 2,
        /** Show both vertical and horizontal lines. */
        Both = 3,
    }
    /**
    * Specifies how the LineMarker interacts with the user.
    */
    enum LineMarkerInteraction {
        /** No interaction, the user specifies the position by clicking. */
        None = 0,
        /** The LineMarker moves with the pointer. */
        Move = 1,
        /** The LineMarker moves when the user drags the line. */
        Drag = 2,
    }
    /**
    * Specifies the alignment of the LineMarker.
    */
    enum LineMarkerAlignment {
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
        Top = 6,
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
    class LineMarker {
        static _CSS_MARKER: string;
        static _CSS_MARKER_HLINE: string;
        static _CSS_MARKER_VLINE: string;
        static _CSS_MARKER_CONTENT: string;
        static _CSS_MARKER_CONTAINER: string;
        static _CSS_LINE_DRAGGABLE: string;
        private _chart;
        private _plot;
        private _marker;
        private _markerContainer;
        private _markerContent;
        private _dragEle;
        private _hLine;
        private _vLine;
        private _plotRect;
        private _targetPoint;
        private _wrapperMoveMarker;
        private _capturedEle;
        private _wrapperMousedown;
        private _wrapperMouseup;
        private _contentDragStartPoint;
        private _mouseDownCrossPoint;
        private _isVisible;
        private _horizontalPosition;
        private _verticalPosition;
        private _alignment;
        private _content;
        private _seriesIndex;
        private _lines;
        private _interaction;
        private _dragThreshold;
        private _dragContent;
        private _dragLines;
        /**
        * Initializes a new instance of a @see:LineMarker object.
        *
        * @param chart The chart on which the LineMarker appears.
        * @param options A JavaScript object containing initialization data for the control.
        */
        constructor(chart: FlexChart, options?: any);
        /**
        * Gets the @see:FlexChart object that owns the LineMarker.
        */
        public chart : FlexChart;
        /**
        * Gets or sets the visibility of the LineMarker.
        */
        public isVisible : boolean;
        /**
        * Gets or sets the index of the series in the chart in which the LineMarker appears.
        * This takes effect when the @see:interaction property is set to
        * wijmo.chart.LineMarkerInteraction.Move or wijmo.chart.LineMarkerInteraction.Drag.
        */
        public seriesIndex : number;
        /**
        * Gets or sets the horizontal position of the LineMarker relative to the plot area.
        *
        * Its value range is (0, 1).
        * If the value is null or undefined and @see:interaction is set to wijmo.chart.LineMarkerInteraction.Move
        * or wijmo.chart.LineMarkerInteraction.Drag, the horizontal position of the marker is calculated automatically based on the pointer's position.
        */
        public horizontalPosition : number;
        /**
        * Gets the current x-value as chart data coordinates.
        */
        public x : number;
        /**
        * Gets the current y-value as chart data coordinates.
        */
        public y : number;
        /**
        * Gets or sets the content function that allows you to customize the text content of the LineMarker.
        */
        public content : Function;
        /**
        * Gets or sets the vertical position of the LineMarker relative to the plot area.
        *
        * Its value range is (0, 1).
        * If the value is null or undefined and @see:interaction is set to wijmo.chart.LineMarkerInteraction.Move
        * or wijmo.chart.LineMarkerInteraction.Drag, the vertical position of the LineMarker is calculated automatically based on the pointer's position.
        */
        public verticalPosition : number;
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
        public alignment : LineMarkerAlignment;
        /**
        * Gets or sets the visibility of the LineMarker lines.
        */
        public lines : LineMarkerLines;
        /**
        * Gets or sets the interaction mode of the LineMarker.
        */
        public interaction : LineMarkerInteraction;
        /**
        Gets or sets the maximum distance from the horizontal or vertical line that the marker can be dragged.
        */
        public dragThreshold : number;
        /**
        Gets or sets a value indicating whether the content of the marker is draggable when the interaction mode is "Drag."
        */
        public dragContent : boolean;
        /**
        Gets or sets a value indicating whether the lines are linked when the horizontal or vertical line is dragged when the interaction mode is "Drag."
        */
        public dragLines : boolean;
        /**
        * Occurs after the LineMarker's position changes.
        */
        public positionChanged: Event;
        /**
        * Raises the @see:positionChanged event.
        *
        * @param point The target point at which to show the LineMarker.
        */
        public onPositionChanged(point: Point): void;
        /**
        * Removes the LineMarker from the chart.
        */
        public remove(): void;
        private _attach();
        private _attachDrag();
        private _detach();
        private _detachDrag();
        private _toggleDragEventAttach(isAttach);
        private _onMousedown(e);
        private _onMouseup(e);
        public _moveMarker(e: any): void;
        private _show(ele?);
        private _hide(ele?);
        private _toggleVisibility();
        private _resetDefaultValue();
        private _initialize();
        private _createMarker();
        private _removeMarker();
        private _getContainer();
        private _createContainer();
        private _createChildren();
        private _toggleElesDraggableClass(draggable);
        private _updateMarkerSize();
        private _updateLinesSize();
        private _resetLinesVisibility();
        private _updateMarkerPosition(point?);
        private _updateContent();
        private _raisePositionChanged(x, y);
        private _updatePositionByAlignment(isMarkerMoved?);
        private _getEventPoint(e);
        private _pointInRect(pt, rect);
    }
}

declare module wijmo.chart {
    class _DataPoint {
        private _seriesIndex;
        private _pointIndex;
        private _dataX;
        private _dataY;
        constructor(seriesIndex: number, pointIndex: number, dataX: number, dataY: number);
        public seriesIndex : number;
        public pointIndex : number;
        public dataX : number;
        public dataY : number;
    }
    enum _MeasureOption {
        X = 0,
        Y = 1,
        XY = 2,
    }
    class _RectArea implements _IHitArea {
        private _rect;
        constructor(rect: Rect);
        public rect : Rect;
        public tag: any;
        public contains(pt: Point): boolean;
        public pointDistance(pt1: Point, pt2: Point, option: _MeasureOption): number;
        public distance(pt: Point): number;
    }
    class _CircleArea implements _IHitArea {
        private _center;
        private _rad;
        private _rad2;
        public tag: any;
        constructor(center: Point, radius: number);
        public center : Point;
        public contains(pt: Point): boolean;
        public distance(pt: Point): number;
    }
    class _LinesArea implements _IHitArea {
        private _x;
        private _y;
        public tag: any;
        constructor(x: any, y: any);
        public contains(pt: Point): boolean;
        public distance(pt: Point): number;
    }
    class _HitResult {
        public area: _IHitArea;
        public distance: number;
    }
    class _HitTester {
        public _chart: FlexChartCore;
        public _map: {
            [key: number]: _IHitArea[];
        };
        constructor(chart: FlexChartCore);
        public add(area: _IHitArea, seriesIndex: number): void;
        public clear(): void;
        public hitTest(pt: Point, testLines?: boolean): _HitResult;
        public hitTestSeries(pt: Point, seriesIndex: any): _HitResult;
    }
}

declare module wijmo.chart {
    /**
    * Plots data series.
    */
    interface _IPlotter {
        chart: FlexChartCore;
        dataInfo: _DataInfo;
        hitTester: _HitTester;
        seriesIndex: number;
        seriesCount: number;
        clipping: boolean;
        stacking: Stacking;
        rotated: boolean;
        adjustLimits(dataInfo: _DataInfo, plotRect: Rect): Rect;
        plotSeries(engine: IRenderEngine, ax: _IAxis, ay: _IAxis, series: _ISeries, palette: _IPalette, iser: number, nser: number): any;
    }
    /**
    * Base class for chart plotters of all types (bar, line, area).
    */
    class _BasePlotter {
        public _DEFAULT_WIDTH: number;
        public _DEFAULT_SYM_SIZE: number;
        public clipping: boolean;
        public chart: FlexChart;
        public hitTester: _HitTester;
        public dataInfo: _DataInfo;
        public seriesIndex: number;
        public seriesCount: number;
        public clear(): void;
        public getNumOption(name: string, parent?: string): number;
        public cloneStyle(style: any, ignore: string[]): any;
        public isValid(datax: number, datay: number, ax: _IAxis, ay: _IAxis): boolean;
    }
}

declare module wijmo.chart {
    /**
    * Bar/column chart plotter.
    */
    class _BarPlotter extends _BasePlotter implements _IPlotter {
        public origin: number;
        public width: number;
        private stackPosMap;
        private stackNegMap;
        public stacking: Stacking;
        public rotated: boolean;
        public clear(): void;
        public adjustLimits(dataInfo: _DataInfo, plotRect: Rect): Rect;
        public plotSeries(engine: IRenderEngine, ax: _IAxis, ay: _IAxis, series: _ISeries, palette: _IPalette, iser: number, nser: number): void;
        private drawSymbol(engine, rect, series, pointIndex, point);
        private drawDefaultSymbol(engine, rect, series);
    }
}

declare module wijmo.chart {
    /**
    * Line/scatter chart plotter.
    */
    class _LinePlotter extends _BasePlotter implements _IPlotter {
        public hasSymbols: boolean;
        public hasLines: boolean;
        public isSpline: boolean;
        public rotated: boolean;
        public stacking: Stacking;
        private stackPos;
        private stackNeg;
        constructor();
        public clear(): void;
        public adjustLimits(dataInfo: _DataInfo, plotRect: Rect): Rect;
        public plotSeries(engine: IRenderEngine, ax: _IAxis, ay: _IAxis, series: _ISeries, palette: _IPalette, iser: number, nser: number): void;
        public _drawLines(engine: IRenderEngine, xs: number[], ys: number[], className?: string, style?: any, clipPath?: string): void;
        public _drawSymbol(engine: IRenderEngine, x: number, y: number, sz: number, series: SeriesBase, pointIndex: number): void;
        public _drawDefaultSymbol(engine: IRenderEngine, x: number, y: number, sz: number, marker: Marker, style?: any): void;
    }
}

declare module wijmo.chart {
    /**
    * Area chart plotter.
    */
    class _AreaPlotter extends _BasePlotter implements _IPlotter {
        public stacking: Stacking;
        public isSpline: boolean;
        public rotated: boolean;
        private stackPos;
        private stackNeg;
        constructor();
        public adjustLimits(dataInfo: _DataInfo, plotRect: Rect): Rect;
        public clear(): void;
        public plotSeries(engine: IRenderEngine, ax: _IAxis, ay: _IAxis, series: _ISeries, palette: _IPalette, iser: number, nser: number): void;
        public _convertToSpline(x: number[], y: number[]): {
            xs: any;
            ys: any;
        };
        public _drawSymbols(engine: IRenderEngine, series: _ISeries, seriesIndex: number): void;
    }
}

declare module wijmo.chart {
    class _BubblePlotter extends _LinePlotter {
        private _MIN_SIZE;
        private _MAX_SIZE;
        private _minSize;
        private _maxSize;
        private _minValue;
        private _maxValue;
        constructor();
        public adjustLimits(dataInfo: _DataInfo, plotRect: Rect): Rect;
        public _drawSymbol(engine: IRenderEngine, x: number, y: number, sz: number, series: Series, pointIndex: number): void;
    }
}

declare module wijmo.chart {
    class _FinancePlotter extends _BasePlotter {
        public isCandle: boolean;
        public adjustLimits(dataInfo: _DataInfo, plotRect: Rect): Rect;
        public plotSeries(engine: IRenderEngine, ax: _IAxis, ay: _IAxis, series: _ISeries, palette: _IPalette, iser: number, nser: number): void;
        public _drawSymbol(engine: IRenderEngine, ax: _IAxis, ay: _IAxis, si: number, pi: number, fill: any, w: number, x: number, hi: number, lo: number, open: number, close: number): void;
    }
}

