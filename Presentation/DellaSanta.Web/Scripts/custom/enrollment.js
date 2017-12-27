//enrollment.js

$(function () {


    $("#courseenrollment").submit(function () {

        try {
            
            if ($("#courseenrollment").valid()) {
                
                    $.ajax({
                        url: this.action,
                        type: this.method,
                        data: $(this).serialize(),
                        success: function (result) {
                            console.log('result=' + result);
                            //alert(this.action + " ->success");
                            //alert(" ->success");
                            if (result) {
                //                $("#partsModal").modal("hide");
                //                location.reload();
                                $("#MessageToClient").text("The data was updated.")
                            } else {
                                $("#MessageToClient").text("The data was NOT updated.")
                            }
                        },
                        error: function () {
                            //alert(this.action + " ->failed");
                            alert("fail");
                            $("#MessageToClient").text("The web server had an error.")
                        }
                    });
               
            }
            return false;
            
        }
        catch (err) {
            adddlert(err.message);
            return false;
            
        }

        
    });

});