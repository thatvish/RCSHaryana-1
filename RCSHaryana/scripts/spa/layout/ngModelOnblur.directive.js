(function (app) {
    'use strict';

    app.directive('ngModelOnblur', ngModelOnblur);

    function ngModelOnblur() {
        return {
            restrict: 'A',
            require: 'ngModel',
            priority: 1, // needed for angular 1.2.x
            link: function (scope, elm, attr, ngModelCtrl) {
                if (attr.type === 'radio' || attr.type === 'checkbox') return;

                elm.unbind('input').unbind('keydown').unbind('change');
                elm.bind('blur', function () {
                    scope.$apply(function () {
                        ngModelCtrl.$setViewValue(elm.val());
                    });
                });
            }
        };
    }

})(angular.module('common.ui'));