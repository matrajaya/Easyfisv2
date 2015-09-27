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
* Defines classes that add interactive features to charts.
*/
declare module wijmo.chart.interaction {
    /**
    * Specifies the orientation of the range selector.
    */
    enum Orientation {
        /** Horizontal, x-data range. */
        X = 0,
        /** Vertical, y-data range. */
        Y = 1,
    }
    /**
    * The @see:RangeSelector control displays a range selector that allows the user to
    * choose the range of data to display on the specified @see:FlexChart.
    *
    * To use the @see:RangeSelector control, specify the @see:FlexChart
    * control on which to display the selected range of data.
    *
    * The @see:rangeChanged event fires when the min or max value changes.
    * For example:
    * <pre>
    *  var rangeSelector = new wijmo.chart.interaction.RangeSelector(chart);
    *  rangeSelector.rangeChanged.addHandler(function () {
    *     // perform related updates
    *     // e.g. modify displayed range of another chart
    *     update(rangeSelector.min, rangeSelector.max);
    *   });
    * </pre>
    */
    class RangeSelector {
        static _CSS_RANGE_SELECTOR: string;
        static _CSS_SCROLLER_CENTER: string;
        static _CSS_RANGE_SCROLLER: string;
        static _CSS_HORIZONTAL_SCROLLER: string;
        static _CSS_VERTICAL_SCROLLER: string;
        static HSCROLLERCSS: string;
        static VSCROLLERCSS: string;
        private _isVisible;
        private _min;
        private _max;
        private _orientation;
        private _chart;
        private _rangeSelectorEle;
        private _leftEle;
        private _middleEle;
        private _rightEle;
        private _movingEle;
        private _minPos;
        private _maxPos;
        private _startPt;
        private _movingOffset;
        private _range;
        private _plotBox;
        private _relaPlotBox;
        private _wrapperMousedown;
        private _wrapperMouseMove;
        private _wrapperMouseup;
        private _isTouch;
        /**
        * Initializes a new instance of the @see:RangeSelector control.
        *
        * @param chart The FlexChart that displays the selected range.
        * @param options A JavaScript object containing initialization data for the control.
        */
        constructor(chart: FlexChart, options?: any);
        /**
        * Gets or sets the visibility of the range selector.
        */
        public isVisible : boolean;
        /**
        * Gets or sets the minimum value of the range.
        * If not set, the minimum is calculated automatically.
        */
        public min : number;
        /**
        * Gets or sets the maximum value of the range.
        * If not set, the maximum is calculated automatically.
        */
        public max : number;
        /**
        * Gets or sets the orientation of the range selector.
        */
        public orientation : Orientation;
        /**
        * Occurs after the range changes.
        */
        public rangeChanged: Event;
        /**
        * Raises the @see:rangeChanged event.
        */
        public onRangeChanged(e?: EventArgs): void;
        /**
        * Removes the range selector from the chart.
        */
        public remove(): void;
        public _createRangeSelector(): void;
        public _switchEvent(isOn: boolean): void;
        public _refresh(): void;
        public _adjustMinAndMax(): void;
        public _updateElesPosition(): void;
        public _invalidate(): void;
        public _changeRange(): void;
        public _getScrollerClass(): any;
        public _onMousedown(e: any): void;
        public _onMouseMove(e: any): void;
        public _onMove(mvPt: Point): void;
        public _onMouseup(e: any): void;
    }
}

