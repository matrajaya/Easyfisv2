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
* Wijmo culture file: it (Italian)
*/
var wijmo;
(function (wijmo) {
    wijmo.culture = {
        Globalize: {
            numberFormat: {
                '.': ',',
                ',': '.',
                percent: { pattern: ['-n%', 'n%'] },
                currency: { decimals: 2, symbol: '€', pattern: ['-$ n', '$ n'] }
            },
            calendar: {
                '/': '/',
                ':': ':',
                firstDay: 1,
                days: ['domenica', 'lunedì', 'martedì', 'mercoledì', 'giovedì', 'venerdì', 'sabato'],
                daysAbbr: ['dom', 'lun', 'mar', 'mer', 'gio', 'ven', 'sab'],
                months: ['gennaio', 'febbraio', 'marzo', 'aprile', 'maggio', 'giugno', 'luglio', 'agosto', 'settembre', 'ottobre', 'novembre', 'dicembre'],
                monthsAbbr: ['gen', 'feb', 'mar', 'apr', 'mag', 'giu', 'lug', 'ago', 'set', 'ott', 'nov', 'dic'],
                am: ['', ''],
                pm: ['', ''],
                eras: ['d.C.'],
                patterns: {
                    d: 'dd/MM/yyyy', D: 'dddd d MMMM yyyy',
                    f: 'dddd d MMMM yyyy HH:mm', F: 'dddd d MMMM yyyy HH:mm:ss',
                    t: 'HH:mm', T: 'HH:mm:ss',
                    m: 'd MMMM', M: 'd MMMM',
                    y: 'MMMM yyyy', Y: 'MMMM yyyy',
                    g: 'dd/MM/yyyy HH:mm', G: 'dd/MM/yyyy HH:mm:ss',
                    s: 'yyyy"-"MM"-"dd"T"HH":"mm":"ss'
                }
            }
        },
        FlexGrid: {
            groupHeaderFormat: '{name}: <b>{value} </b>({count:n0} articoli)'
        },
        FlexGridFilter: {
            // filter
            ascending: '\u2191 Crescente',
            descending: '\u2193 Decrescente',
            apply: 'Applica',
            clear: 'Rimuovi',
            conditions: 'Condizioni',
            values: 'Valori',
            // value filter
            search: 'Filtro',
            selectAll: 'Selezionare tutte',
            null: '(nulla)',
            // condition filter
            header: 'Mostra elementi in cui il valore',
            and: 'E',
            or: 'O',
            stringOperators: [
                { name: '(Non impostato)', op: null },
                { name: 'È uguale a', op: 0 },
                { name: 'Non è uguale a', op: 1 },
                { name: 'Inizia con', op: 6 },
                { name: 'Finisce per', op: 7 },
                { name: 'Contiene', op: 8 },
                { name: 'Non contiene', op: 9 }
            ],
            numberOperators: [
                { name: '(Non impostato)', op: null },
                { name: 'È uguale a', op: 0 },
                { name: 'Non è uguale a', op: 1 },
                { name: 'È maggiore di', op: 2 },
                { name: 'È maggiore di o uguale a', op: 3 },
                { name: 'È minore di', op: 4 },
                { name: 'È minore di o uguale a', op: 5 }
            ],
            dateOperators: [
                { name: '(Non impostato)', op: null },
                { name: 'È uguale a', op: 0 },
                { name: 'Prima di', op: 4 },
                { name: 'Dopo', op: 3 }
            ],
            booleanOperators: [
                { name: '(Non impostato)', op: null },
                { name: 'È uguale a', op: 0 },
                { name: 'Non è uguale a', op: 1 }
            ]
        }
    };
})(wijmo || (wijmo = {}));
;
//# sourceMappingURL=wijmo.culture.it.js.map
