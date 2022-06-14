$(function () {

    jQuery.validator.addMethod('checksupportedfiletypes', function (value, element, params) {
        if (value.length > 0) {
            var supportedTypes = params.supportedTypes.toString();
            var extention = value.substr(value.lastIndexOf('.')).toLowerCase();
            if (supportedTypes.indexOf(extention) >= 0)
                return true;
            else {
                //$(element).val("");              
                if (params.displayImageId != null)
                    $(element).parent().find('#' + params.displayImageId.toString()).empty();
                if (params.displayImageDivID != null)
                    $("#" + params.displayImageDivID.toString() + "").removeClass("show imagepreview").addClass("hide imagepreview");
                return false;
            }
        }
        return true;
    }, '');

    jQuery.validator.addMethod('requiredfile', function (value, element, params) {
        if (params.fileName.length <= 0 && value.length <= 0) {
            return false;
        }
        return true;
    }, '');

}(jQuery));