var wijmo;
(function (wijmo) {
    (function (_chart) {
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
        (function (interaction) {
            'use strict';

            /**
            * Specifies the orientation of the range selector.
            */
            (function (Orientation) {
                /** Horizontal, x-data range. */
                Orientation[Orientation["X"] = 0] = "X";

                /** Vertical, y-data range. */
                Orientation[Orientation["Y"] = 1] = "Y";
            })(interaction.Orientation || (interaction.Orientation = {}));
            var Orientation = interaction.Orientation;

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
            var RangeSelector = (function () {
                /**
                * Initializes a new instance of the @see:RangeSelector control.
                *
                * @param chart The FlexChart that displays the selected range.
                * @param options A JavaScript object containing initialization data for the control.
                */
                function RangeSelector(chart, options) {
                    // fields
                    this._isVisible = true;
                    this._min = null;
                    this._max = null;
                    this._orientation = Orientation.X;
                    this._chart = null;
                    this._rangeSelectorEle = null;
                    this._leftEle = null;
                    this._middleEle = null;
                    this._rightEle = null;
                    this._movingEle = null;
                    this._minPos = 0;
                    this._maxPos = 1;
                    this._startPt = null;
                    this._movingOffset = null;
                    this._wrapperMousedown = null;
                    this._wrapperMouseMove = null;
                    this._wrapperMouseup = null;
                    this._isTouch = false;
                    /**
                    * Occurs after the range changes.
                    */
                    this.rangeChanged = new wijmo.Event();
                    if (!chart) {
                        wijmo.assert(false, 'The FlexChart cannot be null.');
                    }

                    this._isTouch = 'ontouchstart' in window; //isTouchDevice();

                    this._chart = chart;
                    wijmo.copy(this, options);
                    this._createRangeSelector();
                }
                Object.defineProperty(RangeSelector.prototype, "isVisible", {
                    /**
                    * Gets or sets the visibility of the range selector.
                    */
                    get: function () {
                        return this._isVisible;
                    },
                    set: function (value) {
                        if (value != this._isVisible) {
                            this._isVisible = wijmo.asBoolean(value);
                            if (!this._rangeSelectorEle) {
                                return;
                            }
                            this._rangeSelectorEle.style.visibility = this._isVisible ? 'visible' : 'hidden';
                        }
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(RangeSelector.prototype, "min", {
                    /**
                    * Gets or sets the minimum value of the range.
                    * If not set, the minimum is calculated automatically.
                    */
                    get: function () {
                        return this._min;
                    },
                    set: function (value) {
                        if (value !== this._min) {
                            this._min = wijmo.asNumber(value, true, false);
                            this._changeRange();
                        }
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(RangeSelector.prototype, "max", {
                    /**
                    * Gets or sets the maximum value of the range.
                    * If not set, the maximum is calculated automatically.
                    */
                    get: function () {
                        return this._max;
                    },
                    set: function (value) {
                        if (value !== this._max) {
                            this._max = wijmo.asNumber(value, true, false);
                            this._changeRange();
                        }
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(RangeSelector.prototype, "orientation", {
                    /**
                    * Gets or sets the orientation of the range selector.
                    */
                    get: function () {
                        return this._orientation;
                    },
                    set: function (value) {
                        if (value !== this._orientation) {
                            this._orientation = wijmo.asEnum(value, Orientation);
                            this._invalidate();
                        }
                    },
                    enumerable: true,
                    configurable: true
                });

                /**
                * Raises the @see:rangeChanged event.
                */
                RangeSelector.prototype.onRangeChanged = function (e) {
                    this.rangeChanged.raise(this, e);
                };

                /**
                * Removes the range selector from the chart.
                */
                RangeSelector.prototype.remove = function () {
                    if (this._rangeSelectorEle) {
                        this._chart.hostElement.removeChild(this._rangeSelectorEle);
                        this._switchEvent(false);
                        this._rangeSelectorEle = null;
                    }
                };

                RangeSelector.prototype._createRangeSelector = function () {
                    var chart = this._chart, chartHostEle = chart.hostElement, pa, off, box, self = this, rclass = this._getScrollerClass();

                    this._rangeSelectorEle = wijmo.createElement('<div class="' + RangeSelector._CSS_RANGE_SELECTOR + '">' + '<div class="' + RangeSelector._CSS_SCROLLER_CENTER + ' middle"></div>' + '<div class="' + rclass + ' left"></div>' + '<div class="' + rclass + ' right"></div>');
                    this._rangeSelectorEle.style.visibility = this._isVisible ? 'visible' : 'hidden';

                    chartHostEle.appendChild(this._rangeSelectorEle);

                    this._leftEle = this._rangeSelectorEle.querySelector('.left');
                    this._middleEle = this._rangeSelectorEle.querySelector('.middle');
                    this._rightEle = this._rangeSelectorEle.querySelector('.right');

                    //bind event
                    this._wrapperMousedown = this._onMousedown.bind(this);
                    this._wrapperMouseMove = this._onMouseMove.bind(this);
                    this._wrapperMouseup = this._onMouseup.bind(this);
                    this._switchEvent(true);
                };

                RangeSelector.prototype._switchEvent = function (isOn) {
                    var eventListener = isOn ? 'addEventListener' : 'removeEventListener', eventHandler = isOn ? 'addHandler' : 'removeHandler';

                    if (this._rangeSelectorEle) {
                        this._rangeSelectorEle[eventListener]('mousedown', this._wrapperMousedown);
                        this._rangeSelectorEle[eventListener]('mousemove', this._wrapperMouseMove);
                        this._rangeSelectorEle[eventListener]('mouseup', this._wrapperMouseup);
                        if ('ontouchstart' in window) {
                            this._rangeSelectorEle[eventListener]('touchstart', this._wrapperMousedown);
                            this._rangeSelectorEle[eventListener]('touchmove', this._wrapperMouseMove);
                            this._rangeSelectorEle[eventListener]('touchend', this._wrapperMouseup);
                        }
                    }
                    if (this._chart.hostElement) {
                        //for quick moving on chart element
                        this._chart.hostElement[eventListener]('mousemove', this._wrapperMouseMove);
                        this._chart.hostElement[eventListener]('mouseup', this._wrapperMouseup);
                        if ('ontouchstart' in window) {
                            this._chart.hostElement[eventListener]('touchmove', this._wrapperMouseMove);
                            this._chart.hostElement[eventListener]('touchend', this._wrapperMouseup);
                        }
                        this._chart.rendered[eventHandler](this._refresh, this);
                    }
                };

                RangeSelector.prototype._refresh = function () {
                    var chartHostEle = this._chart.hostElement, pa, pOffset, plotBox, rOffset = wijmo.getElementRect(this._rangeSelectorEle);

                    pa = chartHostEle.querySelector('.' + _chart.FlexChart._CSS_PLOT_AREA);
                    pOffset = wijmo.getElementRect(pa);
                    plotBox = pa.getBBox();
                    this._plotBox = { x: pOffset.left, y: pOffset.top, width: plotBox.width, height: plotBox.height };
                    this._relaPlotBox = { x: plotBox.x, y: pOffset.top - rOffset.top, width: plotBox.width, height: plotBox.height };

                    this._adjustMinAndMax();
                    this._updateElesPosition();
                };

                RangeSelector.prototype._adjustMinAndMax = function () {
                    var chart = this._chart, self = this, min = self._min, max = self._max, axis = this._orientation === Orientation.X ? chart.axisX : chart.axisY, actualMin = axis.actualMin, actualMax = axis.actualMax, range = actualMax - actualMin;

                    self._min = (min === null || min === undefined || min < actualMin || min > actualMax) ? actualMin : min;
                    self._max = (max === null || max === undefined || max < actualMin || max > actualMax) ? actualMax : max;

                    self._minPos = (self._min - actualMin) / range;
                    self._maxPos = (self._max - actualMin) / range;
                };

                RangeSelector.prototype._updateElesPosition = function () {
                    var left = this._leftEle, middle = this._middleEle, right = this._rightEle, box = this._relaPlotBox, leftCss, middleCss, rightCss, isXAxis = this._orientation === Orientation.X;

                    if (box) {
                        leftCss = isXAxis ? { left: box.x + this._minPos * box.width - 0.5 * left.offsetWidth - 2, top: box.y - 2, height: box.height } : { left: box.x - 2, top: box.y + (1 - this._minPos) * box.height - 0.5 * right.offsetHeight - 2, width: box.width };

                        middleCss = isXAxis ? { left: box.x + this._minPos * box.width, top: box.y, width: (this._maxPos - this._minPos) * box.width, height: box.height } : { left: box.x, top: box.y + (1 - this._maxPos) * box.height, height: (this._maxPos - this._minPos) * box.height, width: box.width };

                        rightCss = isXAxis ? { left: box.x + this._maxPos * box.width - 0.5 * right.offsetWidth - 2, top: box.y - 2, height: box.height } : { left: box.x - 2, top: box.y + (1 - this._maxPos) * box.height - 0.5 * left.offsetHeight - 2, width: box.width };
                        wijmo.setCss(left, leftCss);
                        wijmo.setCss(middle, middleCss);
                        wijmo.setCss(right, rightCss);
                    }
                };

                RangeSelector.prototype._invalidate = function () {
                    var addClass, rmvClass;

                    if (!this._rangeSelectorEle) {
                        return;
                    }

                    //get needed adding and removing class
                    addClass = this._getScrollerClass();
                    rmvClass = this._orientation === Orientation.X ? RangeSelector.VSCROLLERCSS : RangeSelector.HSCROLLERCSS;

                    //change the selector's position and size.
                    [this._leftEle, this._rightEle].forEach(function (ele) {
                        ele.removeAttribute("style");
                        wijmo.removeClass(ele, rmvClass);
                        wijmo.addClass(ele, addClass);
                    });
                    this._middleEle.removeAttribute("style");
                    this._chart.refresh();
                };

                RangeSelector.prototype._changeRange = function () {
                    this._adjustMinAndMax();

                    if (!this._rangeSelectorEle) {
                        return;
                    }
                    this._updateElesPosition();
                    this.onRangeChanged();
                };

                RangeSelector.prototype._getScrollerClass = function () {
                    return this._orientation === Orientation.X ? RangeSelector.HSCROLLERCSS : RangeSelector.VSCROLLERCSS;
                };

                RangeSelector.prototype._onMousedown = function (e) {
                    if (!this._isVisible) {
                        return;
                    }

                    this._movingEle = e.srcElement || e.target;
                    this._startPt = e instanceof MouseEvent ? new wijmo.Point(e.pageX, e.pageY) : new wijmo.Point(e.changedTouches[0].pageX, e.changedTouches[0].pageY);

                    this._movingOffset = wijmo.getElementRect(this._movingEle);
                    if (this._movingEle != this._middleEle) {
                        if (this._orientation === Orientation.X) {
                            this._movingOffset.left += 0.5 * this._movingEle.offsetWidth;
                        } else {
                            this._movingOffset.top += 0.5 * this._movingEle.offsetHeight;
                        }
                    } else {
                        this._range = this._maxPos - this._minPos;
                    }

                    e.preventDefault();
                };

                RangeSelector.prototype._onMouseMove = function (e) {
                    if (!this._isVisible) {
                        return;
                    }

                    var movingPt = e instanceof MouseEvent ? new wijmo.Point(e.pageX, e.pageY) : new wijmo.Point(e.changedTouches[0].pageX, e.changedTouches[0].pageY);

                    this._onMove(movingPt);
                    e.preventDefault();
                };

                RangeSelector.prototype._onMove = function (mvPt) {
                    var strPt = this._startPt, movingOffset = this._movingOffset, plotBox = this._plotBox, range = this._range, moving = this._movingEle, left = this._leftEle, middle = this._middleEle, right = this._rightEle, x, y, pos;

                    if (strPt) {
                        if (this._orientation === Orientation.X) {
                            x = movingOffset.left + mvPt.x - strPt.x;
                            pos = (x - plotBox.x) / plotBox.width;
                        } else {
                            y = movingOffset.top + mvPt.y - strPt.y;
                            pos = 1 - (y - plotBox.y) / plotBox.height;
                        }

                        if (pos < 0) {
                            pos = 0;
                        } else if (pos > 1) {
                            pos = 1;
                        }

                        if (moving === left) {
                            if (pos > this._maxPos) {
                                pos = this._maxPos;
                            }
                            this._minPos = pos;
                        } else if (moving === right) {
                            if (pos < this._minPos) {
                                pos = this._minPos;
                            }
                            this._maxPos = pos;
                        } else if (moving === middle) {
                            if (this._orientation === Orientation.X) {
                                this._minPos = pos;
                                this._maxPos = this._minPos + range;
                                if (this._maxPos >= 1) {
                                    this._maxPos = 1;
                                    this._minPos = this._maxPos - range;
                                }
                            } else {
                                this._maxPos = pos;
                                this._minPos = this._maxPos - range;
                                if (this._minPos <= 0) {
                                    this._minPos = 0;
                                    this._maxPos = this._minPos + range;
                                }
                            }
                        }
                        this._updateElesPosition();
                    }
                };

                RangeSelector.prototype._onMouseup = function (e) {
                    var chart, axis, actualMin, actualMax, range;

                    if (!this._isVisible) {
                        return;
                    }

                    chart = this._chart;
                    axis = this._orientation === Orientation.X ? chart.axisX : chart.axisY, actualMin = axis.actualMin, actualMax = axis.actualMax;
                    range = actualMax - actualMin;

                    if (this._startPt) {
                        this._startPt = this._movingOffset = null;

                        //raise event
                        this._min = actualMin + this._minPos * range;
                        this._max = actualMin + this._maxPos * range;
                        this.onRangeChanged();
                        e.preventDefault();
                    }
                };
                RangeSelector._CSS_RANGE_SELECTOR = 'wj-rangeselector';
                RangeSelector._CSS_SCROLLER_CENTER = 'wj-scroller-center';
                RangeSelector._CSS_RANGE_SCROLLER = 'wj-scroller';
                RangeSelector._CSS_HORIZONTAL_SCROLLER = 'wj-hscroller';
                RangeSelector._CSS_VERTICAL_SCROLLER = 'wj-vscroller';

                RangeSelector.HSCROLLERCSS = RangeSelector._CSS_RANGE_SCROLLER + ' ' + RangeSelector._CSS_HORIZONTAL_SCROLLER;
                RangeSelector.VSCROLLERCSS = RangeSelector._CSS_RANGE_SCROLLER + ' ' + RangeSelector._CSS_VERTICAL_SCROLLER;
                return RangeSelector;
            })();
            interaction.RangeSelector = RangeSelector;
        })(_chart.interaction || (_chart.interaction = {}));
        var interaction = _chart.interaction;
    })(wijmo.chart || (wijmo.chart = {}));
    var chart = wijmo.chart;
})(wijmo || (wijmo = {}));
//# sourceMappingURL=wijmo.chart.interaction.js.map
