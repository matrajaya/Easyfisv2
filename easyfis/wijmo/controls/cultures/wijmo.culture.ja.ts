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
/*
 * Wijmo culture file: ja (Japanese)
 */
module wijmo {
    wijmo.culture = {
        Globalize: {
            numberFormat: {
                '.': '.',
                ',': ',',
                percent: { pattern: ['-n%', 'n%'] },
                currency: { decimals: 0, symbol: '¥', pattern: ['-$n', '$n'] }
            },
            calendar: {
                '/': '/',
                ':': ':',
                firstDay: 0,
                days: ['日曜日', '月曜日', '火曜日', '水曜日', '木曜日', '金曜日', '土曜日'],
                daysAbbr: ['日', '月', '火', '水', '木', '金', '土'],
                months: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],
                monthsAbbr: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12'],
                am: ['午前', '午'],
                pm: ['午後', '午'],
                //eras: ['西暦'],
                eras: [
                    { name: '平成', symbol: 'H', start: new Date(1989, 0, 8) },
                    { name: '昭和', symbol: 'S', start: new Date(1926, 11, 25) },
                    { name: '大正', symbol: 'T', start: new Date(1912, 6, 30) },
                    { name: '明治', symbol: 'M', start: new Date(1868, 8, 8) }
                ],
                patterns: {
                    d: 'yyyy/M/d', D: 'yyyy"年"M"月"d"日"',
                    f: 'yyyy"年"M"月"d"日" H:mm', F: 'yyyy"年"M"月"d"日" H:mm:ss',
                    t: 'H:mm', T: 'H:mm:ss',
                    m: 'M"月"d"日"', M: 'M"月"d"日"',
                    y: 'yyyy"年"M"月"', Y: 'yyyy"年"M"月"',
                    g: 'yyyy/M/d H:mm', G: 'yyyy/M/d H:mm:ss',
                    s: 'yyyy"-"M"-"d"T"HH":"mm":"ss'
                }
            }
        },
        FlexGrid: {
            groupHeaderFormat: '{name}: <b>{value} </b>({count:n0} 項目)'
        },
        FlexGridFilter: {

            // filter
            ascending: '\u2191 昇順',
            descending: '\u2193 降順',
            apply: '適用',
            clear: 'リセット',
            conditions: '条件',
            values: '値',

            // value filter
            search: 'フィルタ',
            selectAll: '全て選択',
            null: '(ゼロ)',

            // condition filter
            header: '抽出条件の指定',
            and: 'AND',
            or: 'OR',
            stringOperators: [
                { name: '(設定しない)', op: null },
                { name: '指定の値に等しい', op: 0 },
                { name: '指定の値に等しくない', op: 1 },
                { name: '指定の値で始まる', op: 6 },
                { name: '指定の値で終わる', op: 7 },
                { name: '指定の値を含む', op: 8 },
                { name: '指定の値を含まない', op: 9 }
            ],
            numberOperators: [
                { name: '(設定しない)', op: null },
                { name: '指定の値に等しい', op: 0 },
                { name: '指定の値に等しくない', op: 1 },
                { name: '指定の値より大きい', op: 2 },
                { name: '指定の値以上', op: 3 },
                { name: '指定の値より小さい', op: 4 },
                { name: '指定の値以下', op: 5 }
            ],
            dateOperators: [
                { name: '(設定しない)', op: null },
                { name: '指定の値に等しい', op: 0 },
                { name: '前である', op: 4 },
                { name: 'の後である', op: 3 }
            ],
            booleanOperators: [
                { name: '(設定しない)', op: null },
                { name: '指定の値に等しい', op: 0 },
                { name: '指定の値に等しくない', op: 1 }
            ]
        }
    };
};

