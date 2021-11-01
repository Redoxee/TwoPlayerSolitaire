var ResponseNode = document.querySelector(".Response");

function requestNewGame() {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var responseMessage = document.createTextNode(this.responseText);
            ResponseNode.appendChild(responseMessage);
        }
    };
    xhttp.open("POST", "RequestNewGame", true);
    xhttp.send();
}