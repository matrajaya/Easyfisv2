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
* Wijmo culture file: ko (Korean)
*/
var wijmo;
(function (wijmo) {
    wijmo.culture = {
        Globalize: {
            numberFormat: {
                '.': '.',
                ',': ',',
                percent: { pattern: ['-n %', 'n %'] },
                currency: { decimals: 0, symbol: '₩', pattern: ['-$n', '$n'] }
            },
            calendar: {
                '/': '-',
                ':': ':',
                firstDay: 0,
                days: ['일요일', '월요일', '화요일', '수요일', '목요일', '금요일', '토요일'],
                daysAbbr: ['일', '월', '화', '수', '목', '금', '토'],
                months: ['1월', '2월', '3월', '4월', '5월', '6월', '7월', '8월', '9월', '10월', '11월', '12월'],
                monthsAbbr: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12'],
                am: ['오전', '오'],
                pm: ['오후', '오'],
                eras: ['서기'],
                patterns: {
                    d: 'yyyy-MM-dd', D: 'yyyy"년" M"월" d"일" dddd',
                    f: 'yyyy"년" M"월" d"일" dddd tt h:mm', F: 'yyyy"년" M"월" d"일" dddd tt h:mm:ss',
                    t: 'tt h:mm', T: 'tt h:mm:ss',
                    m: 'M"월" d"일"', M: 'M"월" d"일"',
                    y: 'yyyy"년" M"월"', Y: 'yyyy"년" M"월"',
                    g: 'yyyy-MM-dd tt h:mm', G: 'yyyy-MM-dd tt h:mm:ss',
                    s: 'yyyy"-"MM"-"dd"T"HH":"mm":"ss'
                }
            }
        },
        FlexGrid: {
            groupHeaderFormat: '{name}: <b>{value} </b>({count:n0} 항목)'
        },
        FlexGridFilter: {
            // filter
            ascending: '\u2191 오름차순',
            descending: '\u2193 내림차순',
            apply: '적용',
            clear: '확인',
            conditions: '조건',
            values: '값',
            // value filter
            search: '검색',
            selectAll: '모든 선택',
            null: '(제로)',
            // condition filter
            header: '자료 행 보기',
            and: '그리고',
            or: '또는',
            stringOperators: [
                { name: '(설정되지 않음)', op: null },
                { name: '같음', op: 0 },
                { name: '같지 않음', op: 1 },
                { name: '로 시작', op: 6 },
                { name: '로 엔드', op: 7 },
                { name: '포함', op: 8 },
                { name: '포함하지 않음', op: 9 }
            ],
            numberOperators: [
                { name: '(설정되지 않음)', op: null },
                { name: '같음', op: 0 },
                { name: '같지 않음', op: 1 },
                { name: '보다 큼', op: 2 },
                { name: '크거나 같음', op: 3 },
                { name: '보다 작음', op: 4 },
                { name: '작거나 같음', op: 5 }
            ],
            dateOperators: [
                { name: '(설정되지 않음)', op: null },
                { name: '같음', op: 0 },
                { name: '전에', op: 4 },
                { name: '후', op: 3 }
            ],
            booleanOperators: [
                { name: '(설정되지 않음)', op: null },
                { name: '같음', op: 0 },
                { name: '같지 않음', op: 1 }
            ]
        }
    };
})(wijmo || (wijmo = {}));
;
//# sourceMappingURL=wijmo.culture.ko.js.map
