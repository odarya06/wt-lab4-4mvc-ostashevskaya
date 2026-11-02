// Нормализация десятичного разделителя в полях ввода цены.
// Сервер принимает и точку, и запятую, но jQuery Validation ожидает число
// в формате с точкой, поэтому перед проверкой заменяем запятую на точку.
document.addEventListener("DOMContentLoaded", function () {
    var decimalInputs = document.querySelectorAll('input[inputmode="decimal"]');

    function normalize(input) {
        if (!input.value.includes(",")) {
            return;
        }

        var caret = input.selectionStart;
        input.value = input.value.replace(",", ".");

        if (caret !== null && typeof input.setSelectionRange === "function") {
            input.setSelectionRange(caret, caret);
        }
    }

    decimalInputs.forEach(function (input) {
        normalize(input);
        input.addEventListener("input", function () {
            normalize(input);
        });
    });
});
