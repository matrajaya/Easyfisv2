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
* Wijmo culture file: pt (Portuguese)
*/
var wijmo;
(function (wijmo) {
    wijmo.culture = {
        Globalize: {
            numberFormat: {
                '.': ',',
                ',': '.',
                percent: { pattern: ['-n%', 'n%'] },
                currency: { decimals: 2, symbol: 'R$', pattern: ['-$ n', '$ n'] }
            },
            calendar: {
                '/': '/',
                ':': ':',
                firstDay: 0,
                days: ['domingo', 'segunda-feira', 'terça-feira', 'quarta-feira', 'quinta-feira', 'sexta-feira', 'sábado'],
                daysAbbr: ['dom', 'seg', 'ter', 'qua', 'qui', 'sex', 'sáb'],
                months: ['janeiro', 'fevereiro', 'março', 'abril', 'maio', 'junho', 'julho', 'agosto', 'setembro', 'outubro', 'novembro', 'dezembro'],
                monthsAbbr: ['jan', 'fev', 'mar', 'abr', 'mai', 'jun', 'jul', 'ago', 'set', 'out', 'nov', 'dez'],
                am: ['', ''],
                pm: ['', ''],
                eras: ['d.C.'],
                patterns: {
                    d: 'dd/MM/yyyy', D: 'dddd, d" de "MMMM" de "yyyy',
                    f: 'dddd, d" de "MMMM" de "yyyy HH:mm', F: 'dddd, d" de "MMMM" de "yyyy HH:mm:ss',
                    t: 'HH:mm', T: 'HH:mm:ss',
                    m: 'd" de "MMMM', M: 'd" de "MMMM',
                    y: 'MMMM" de "yyyy', Y: 'MMMM" de "yyyy',
                    g: 'dd/MM/yyyy HH:mm', G: 'dd/MM/yyyy HH:mm:ss',
                    s: 'yyyy"-"MM"-"dd"T"HH":"mm":"ss'
                }
            }
        },
        FlexGrid: {
            groupHeaderFormat: '{name}: <b>{value} </b>({count:n0} itens)'
        },
        FlexGridFilter: {
            // filter
            ascending: '\u2191 Crescente',
            descending: '\u2193 Decrescente',
            apply: 'Aplicar',
            clear: 'Remover',
            conditions: 'Condições',
            values: 'Valores',
            // value filter
            search: 'Filtro',
            selectAll: 'Selecionar todos',
            null: '(nulo)',
            // condition filter
            header: 'Mostrar items com valor',
            and: 'E',
            or: 'Ou',
            stringOperators: [
                { name: '(nenhum)', op: null },
                { name: 'Igual a', op: 0 },
                { name: 'Diferente de', op: 1 },
                { name: 'Que inicia com', op: 6 },
                { name: 'Que termina em', op: 7 },
                { name: 'Que contém', op: 8 },
                { name: 'Que não contém', op: 9 }
            ],
            numberOperators: [
                { name: '(nenhum)', op: null },
                { name: 'Igual a', op: 0 },
                { name: 'Diferente de', op: 1 },
                { name: 'Maior que', op: 2 },
                { name: 'Maior ou igual a', op: 3 },
                { name: 'Menor que', op: 4 },
                { name: 'Menor ou igual a', op: 5 }
            ],
            dateOperators: [
                { name: '(nenhum)', op: null },
                { name: 'Igual a', op: 0 },
                { name: 'Antes de', op: 4 },
                { name: 'Depois de', op: 3 }
            ],
            booleanOperators: [
                { name: '(nenhum)', op: null },
                { name: 'Igual a', op: 0 },
                { name: 'Diferente de', op: 1 }
            ]
        }
    };
})(wijmo || (wijmo = {}));
;
//# sourceMappingURL=wijmo.culture.pt.js.map
