function CreateWebSocket() {
    console.log(gameWebSocketUrl);
    websocket = new WebSocket(gameWebSocketUrl);

    websocket.onopen = function (e) {
        isConnected = true;
        writeToScreen("CONNECTED");
        var requestFaces = '{ "OrderType": "RequestPlayerFaces" }';
        DoSend(requestFaces);
    };

    websocket.onclose = function (e) {
        writeToScreen("DISCONNECTED");
    };

    websocket.onmessage = RecieveWebSocketMessage;

    websocket.onerror = function (e) {
        writeToScreen("<span class=error>ERROR:</span> " + e.data);
    };
}

function RecieveWebSocketMessage(e) {
    var messageData = null;
    writeToScreen("Recieving message " + e.data);
    try {
        messageData = JSON.parse(e.data);
    }
    catch (error) {
        writeToScreen("error while parsing json" + error.message);
    }

    if (messageData.MessageType in messageHandles) {
        messageHandles[messageData.MessageType](messageData);
    }
    else {
        writeToScreen("Unkown message type.");
    }
}

function DoSend(message) {
    writeToScreen("SENT: " + message);
    websocket.send(message);
}