(function ($) {
    $.fn.validate = function (options) {
        return this.each(function () {

            function IsNumberEvent(e) {
                //alert("IsNumberEvent->keyCode=" + e.keyCode + " e.which=" + e.which);

                var k = e.keyCode;
                var w = e.which;

                // firefox DEL
                if (k == 46 && w == 0)
                    return true;
                // firefox BackSpace
                if (k == 8 && w == 8)
                    return true;
                // firefox TAB
                if (k == 9 && w == 0)
                    return true;
                // firefox setas
                if ((k >= 37 || k <= 40) && w == 0)
                    return true;
                else if (!String.fromCharCode(w).match(/[0-9]/))
                    return false;
                return true;
            };
            function IsNumericField(f, e) {
                //alert("IsNumericField->keyCode=" + e.keyCode + " e.which=" + e.which);

                var k = e.keyCode;
                var w = e.which;

                // firefox DEL
                if (k == 46 && w == 0)
                    return true;
                // firefox BackSpace
                if (k == 8 && w == 8)
                    return true;
                // firefox TAB
                if (k == 9 && w == 0)
                    return true;
                // firefox setas
                if ((k >= 37 || k <= 40) && w == 0)
                    return true;
                if (!String.fromCharCode(w).match(/[0-9,]/))
                    return false;
                var ck = String.fromCharCode(w);
                var cf = f.value;
                if (ck == "." && (cf.indexOf(".") > 0 || cf.indexOf(",") > 0)) return false;
                if (ck == "," && (cf.indexOf(",") > 0 || cf.indexOf(".") > 0)) return false;
                return true;
            };
            $('.numbersAndCommasOnly').keypress(function (e) {
                return IsNumericField(this, e);
            });
            $('.numbersOnly').keypress(function (e) {
                return IsNumberEvent(e);
            });

        });
    };
})(jQuery);
