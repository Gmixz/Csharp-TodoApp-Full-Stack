function deleteTodo(i)
{
    $.ajax({
        url: 'Home/Delete',
        type: 'POST',
        data: {id: i},
        success: function() 
        {
            window.location.reload();
        }
    });
}

function populateForm(i) 
{
    $.ajax({
        url: 'Home/PopulateForm',
        type: 'GET',
        data: {id: i},
        dataType: 'json',
        success: function(response) 
        {
            $("#TodoApp_Name").val(response.name);
            $("#TodoApp_Id").val(response.id);
            $("#form-button").val("Update TodoApp");
            $("#form-action").attr("action", "/Home/Update");
        }  
    });
} 
