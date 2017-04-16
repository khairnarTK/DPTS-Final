function deleterecord(delid, cntrname) {

    var r = confirm("Do you want to Delete Permanently ?");
    if (r == true) {
        $.ajax({
            url: '/' + cntrname + '/DeleteConfirmed?id=' + delid,
            type: 'POST',

            success: function (data) {
                debugger;
                if (data == "Deleted") {
                    alert("Deleted Successfully !!");
                    window.setTimeout(function () { location.reload() }, 500)

                }
            }
        });
    } else {
        return false;
    }
    }



    $("#AddRoleForm").submit(function (event) {
        var role = $("#Name").val();
        if (!role) {
            //alert("Please enter role name.");
            event.preventDefault();
            alert("Please enter role name");
        }
    });

    $("#UserAddToRole").submit(function (event) {
        var role = $("#Username").val();
        if (!role) {
            event.preventDefault();
            alert("Please enter username.");
            return
        }

        var roleName = $("#RoleName").val();
        if (!roleName) {
            event.preventDefault();
            alert("Please select role.");
            return
        }

    });
    $("#DeleteRoleForUser").submit(function (event) {
        var role = $("#Username").val();
        if (!role) {
            event.preventDefault();
            alert("Please select username.");
            return
        }

        var roleName = $("#RoleName").val();
        if (!roleName) {
            event.preventDefault();
            alert("Please select role.");
            return
        }
    });
