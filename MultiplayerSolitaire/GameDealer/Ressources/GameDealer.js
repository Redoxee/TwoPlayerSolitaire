var ResponseNode = document.querySelector(".Response");
var serverStatusLabel = document.createTextNode("");
var content = document.querySelector(".Content");

ResponseNode.appendChild(serverStatusLabel);

var refreshButton = document.createElement("button");
refreshButton.onclick = RefreshServerState;
refreshButton.appendChild(document.createTextNode("Refresh"));
content.appendChild(refreshButton);

var newGameButton = document.createElement("button");
newGameButton.onclick = RequestNewGame;
newGameButton.appendChild(document.createTextNode("Create new game"));

var gameLink = document.createElement("a");
gameLink.setAttribute("rel", "noopener noreferrer");
gameLink.setAttribute("target", "_blank");

var refreshRate = 3000;
var status = "Unkown";
var gameAdress = "";

function RefreshServerState() {
    var xhttp = new XMLHttpRequest();

    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            HandleUpdateFromServer(this.responseText);
        }
    };

    xhttp.open("POST", "ServerState", true);
    xhttp.send();
}

function RefreshServerStatsPeriodically() {
    RefreshServerState();
    setTimeout(RefreshServerStatsPeriodically, refreshRate);
}

function RequestNewGame() {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            HandleUpdateFromServer(this.responseText);
        }
    };

    xhttp.open("POST", "RequestNewGame", true);
    xhttp.send();
}

function HandleUpdateFromServer(textContent) {
    try {
        console.log(textContent);
        messageData = JSON.parse(textContent);
        status = messageData.Status;
        if (messageData.GameAdress != null) {
            gameAdress = messageData.GameAdress;
        }
    }
    catch (ex) {
        (console.error || console.log).call(console, textContent);
        (console.error || console.log).call(console, ex.stack || ex);
    }

    serverStatusLabel.textContent = status;
    ClearChilds(content);
    content.append(refreshButton);
    if (status == "Idle" || status == "Exited") {
        content.append(newGameButton);
    }

    if (status == "Running") {
        gameLink.textContent = gameAdress;
        gameLink.setAttribute("href", "http://" + gameAdress);
        content.append(gameLink);
    }
}

function ClearChilds(node) {
    node.innerHTML = "";
}
 
RefreshServerStatsPeriodically();