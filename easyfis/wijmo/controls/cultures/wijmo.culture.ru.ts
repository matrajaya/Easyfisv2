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
 * Wijmo culture file: ru (Russian)
 */
module wijmo {
    wijmo.culture = {
        Globalize: {
            numberFormat: {
                '.': ',',
                ',': ' ',
                percent: { pattern: ['-n%', 'n%'] },
                currency: { decimals: 2, symbol: 'р.', pattern: ['-n $', 'n $'] }
            },
            calendar: {
                '/': '.',
                ':': ':',
                firstDay: 1,
                days: ['воскресенье', 'понедельник', 'вторник', 'среда', 'четверг', 'пятница', 'суббота'],
                daysAbbr: ['Вс', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'],
                months: ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'],
                monthsAbbr: ['янв', 'фев', 'мар', 'апр', 'май', 'июн', 'июл', 'авг', 'сен', 'окт', 'ноя', 'дек'],
                am: ['', ''],
                pm: ['', ''],
                eras: ['наша эра'],
                patterns: {
                    d: 'dd.MM.yyyy', D: 'd MMMM yyyy "г."',
                    f: 'd MMMM yyyy "г." H:mm', F: 'd MMMM yyyy "г." H:mm:ss',
                    t: 'H:mm', T: 'H:mm:ss',
                    m: 'd MMMM', M: 'd MMMM',
                    y: 'MMMM yyyy', Y: 'MMMM yyyy',
                    g: 'dd.MM.yyyy H:mm', G: 'dd.MM.yyyy H:mm:ss',
                    s: 'yyyy"-"MM"-"dd"T"HH":"mm":"ss'
                }
            }
        },
        FlexGrid: {
            groupHeaderFormat: '{name}: <b>{value} </b>({count:n0} наименований)'
        },
        FlexGridFilter: {

            // filter
            ascending: '\u2191 По возрастанию',
            descending: '\u2193 По убыванию',
            apply: 'Применить',
            clear: 'Очистить',
            conditions: 'Фильтр по условию',
            values: 'Фильтр по значению',

            // value filter
            search: 'Поиск',
            selectAll: 'Выбрать все',
            null: '(пустой)',

            // condition filter
            header: 'Показать строки, где значение',
            and: 'И',
            or: 'Или',
            stringOperators: [
                { name: '(Не задано)', op: null },
                { name: 'равно', op: 0 },
                { name: 'не равно', op: 1 },
                { name: 'начинается с', op: 6 },
                { name: 'заканчивается на', op: 7 },
                { name: 'содержит', op: 8 },
                { name: 'не содержит', op: 9 }
            ],
            numberOperators: [
                { name: '(Не задано)', op: null },
                { name: 'равно', op: 0 },
                { name: 'не равно', op: 1 },
                { name: 'больше чем', op: 2 },
                { name: 'больше или равно', op: 3 },
                { name: 'меньше чем', op: 4 },
                { name: 'меньше или равно', op: 5 }
            ],
            dateOperators: [
                { name: '(Не задано)', op: null },
                { name: 'равно', op: 0 },
                { name: 'до', op: 4 },
                { name: 'после', op: 3 }
            ],
            booleanOperators: [
                { name: '(Не задано)', op: null },
                { name: 'равно', op: 0 },
                { name: 'не равно', op: 1 }
            ]
        }
    };
};
