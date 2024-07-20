function openMessageComponent() {
    var ele = document.getElementById("messageModal");
    if (ele) {
        var model = new bootstrap.Modal(ele);
        if (model) {
            model.show();
            return true;
        }
    }
    return false;
}