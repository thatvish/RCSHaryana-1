/*!
 * jQuery Math Captcha Plugin v1.0.0
 * https://github.com/matfiz/jquery-math-captcha
 *
 * Copyright 2014 Grzegorz Brzezinka, Mariusz Henn
 * Released under the MIT license
 */
jQuery(document).ready(function ($) {
    // browser window scroll (in pixels) after which the "back to top" link is shown
    var offset = 300,
		//browser window scroll (in pixels) after which the "back to top" link opacity is reduced
		offset_opacity = 1200,
		//duration of the top scrolling animation (in ms)
		scroll_top_duration = 700,
		//grab the "back to top" link
		$back_to_top = $('.cd-top');

    //hide or show the "back to top" link
    $(window).scroll(function () {
        ($(this).scrollTop() > offset) ? $back_to_top.addClass('cd-is-visible') : $back_to_top.removeClass('cd-is-visible cd-fade-out');
        if ($(this).scrollTop() > offset_opacity) {
            $back_to_top.addClass('cd-fade-out');
        }
    });

    //smooth scroll to top
    $back_to_top.on('click', function (event) {
        event.preventDefault();
        $('body,html').animate({
            scrollTop: 0,
        }, scroll_top_duration
		);
    });

});

(function ($) {
    $.fn.mathCaptcha = function (options) {
        var settings = $.extend({
            operation: "random",
            imgPath: '../images/',
            introText: 'Please enter the result of equation:'
        }, options);
        var numbers = {
            1: settings.imgPath + 'jeden.png',
            2: settings.imgPath + 'dwa.png',
            3: settings.imgPath + 'trzy.png',
            4: settings.imgPath + 'cztery.png',
            5: settings.imgPath + 'piec.png',
            6: settings.imgPath + 'szesc.png',
            7: settings.imgPath + 'siedem.png',
            8: settings.imgPath + 'osiem.png',
            9: settings.imgPath + 'dziewiec.png',
            0: settings.imgPath + 'zero.png'
        }
        var operations = {
            plus: settings.imgPath + 'dodaj.png',
            times: settings.imgPath + 'pomnoz.png'
        }
        var equal = settings.imgPath + 'rownosc.png';
        var displayImage = function (src) {
            return "<img src='" + src + "'>";
        }
        var displayNumber = function (num) {
            return displayImage(numbers[num]);
        }
        //generate random numbers 0-9
        var number1 = Math.floor(Math.random() * 10);
        var number2 = Math.floor(Math.random() * 10);
        //select operation
        if (settings.operation == 'random') {
            if (Math.random() > 0.5) {
                var operation = 'plus';
            }
            else {
                var operation = 'times';
            }
        }
        else {
            var operation = settings.operation;
        }
        //generate input
        var input = "<input type='intNumber' id='math-captcha-result'>";
        var intro = "<p>" + settings.introText + "</p>";
        if (operation == 'plus') {
            var operationImg = displayImage(operations.plus);
        }
        else {
            var operationImg = displayImage(operations.times);
        }
        this.html(intro + displayNumber(number1) + operationImg + displayNumber(number2) + displayImage(equal) + input);
        this.addClass('math-captcha');

        $('body').on('keyup', '#math-captcha-result', function () {
            if (operation == 'plus') {
                var condition = parseInt($(this).val()) === number1 + number2;
            }
            else {
                var condition = parseInt($(this).val()) === number1 * number2;
            }
            if (condition) {
                settings.successFunction.call()
            }
            else {
                settings.failFunction.call()
            }
        });

        return this;

    };

}(jQuery));

// div-replacement-script
//on load
responsive_change_box_order();
//on resize
window.addEventListener('resize', responsive_change_box_order);

function responsive_change_box_order() {
    if (window.matchMedia("(max-width: 760px)").matches) {
        jQuery("#tabs-div").remove().insertBefore(jQuery("#news-section"));
    } else {
        jQuery("#tabs-div").remove().insertAfter(jQuery("#news-section"));
    }
}
// //div-replacement-script

// captcha-script
$("#math-captcha").mathCaptcha({
    imgPath: 'images/',
    operation: "random",
    introText: 'Enter the result of equation:',
    successFunction: function () {
        form = $('form');
        //form.append('<p><input type="submit" name="login" class="login loginmodal-submit login-btn" value="Login"></p><p><input type="submit" name="login" class="login loginmodal-submit" value="Reset"></p>');
        $('.math-captcha-error').remove();
    },
    failFunction: function () {
        form = $('form');
        //form.find('input[type="submit"]').remove();
        if ($('.math-captcha-error').length < 1) {
            $('#math-captcha').append("<p class='math-captcha-error'>Wrong result!</p>");
        }
    }
});
// //captcha-script

// scroll-to
jQuery("#skip-text").click(function () {
    jQuery('html, body').animate({
        scrollTop: jQuery("#first-div").offset().top
    }, 1000);
});
// //scroll-to
//   jQuery(function() {
//   jQuery(".datepicker").datepicker();
//});