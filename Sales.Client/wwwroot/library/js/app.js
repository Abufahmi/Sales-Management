// Function to open a modal dialog with a given ID
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

function isOnline() {
    return window.navigator.onLine;
}

// Function to set the file source for an image element by ID
function setFileSource(eleId, bytes) {
    var id = String(eleId).valueOf();
    var ele = document.getElementById(id);
    if (ele) {
        var blob = new Blob([bytes], { type: "image/png", width: 500, height: 500 });
        var imageUrl = URL.createObjectURL(blob);
        ele.src = imageUrl;
    }
}

// Function to set the image source using a streaming approach
async function setImageUsingStreaming(imageElementId, imageStream) {
    const arrayBuffer = await imageStream.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    document.getElementById(imageElementId).src = url;
    document.getElementById(imageElementId).scrollIntoView();
}
