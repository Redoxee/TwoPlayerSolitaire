const GameStateID = [
    "Initialize",
    "Playing",
    "Transitioning",
    "EndGame",
    "Unkown"];

const SigilLabel = [
    "Spade",
    "Club",
    "Heart",
    "Diamond"];

const SigilSymbol = [
    '\u2660',
    '\u2663',
    '\u2661',
    '\u2662'];

const ValueLabel = [
    "Ace",
    "2",
    "3",
    "4",
    "5",
    "6",
    "7",
    "8",
    "9",
    "10",
];

var websocket = null;
var isConnected = false;
var gameWebSocketUrl = document.URL.replace("http://", "ws://");

var output = document.querySelector("#debugOutput");
var playArea = document.querySelector("#playArea");

var player = new Player();
var playerSlots = new PlayerSlots();
var gameInfo = new GameInfo();
var opponent = new Opponent();
var endGame = new EndGame();
var faceCollection = new FaceCollection();

var clientState = "None";
var localPlayerIndex = -1;
var availableSlots = [];
var nextOrderID = 0;

var pendingOrders = [];
var gameState = null;
var nextRoundState = null;

var messageHandles = [];
messageHandles["AvailablePlayerSlots"] = HandleAvailablePlayerSlots;
messageHandles["OrderAcknowledgement"] = HandleOrderAcknowledgement;
messageHandles["PlayerViewUpdate"] = HandlePlayerViewUpdate;
messageHandles["SandboxChanges"] = HandleSandboxUpdate;

writeToScreen("WS URI " + gameWebSocketUrl);

CreateWebSocket();
writeToScreen('\u261a');

function writeToScreen(message) {
    output.insertAdjacentHTML("afterbegin", "<p>" + message + "</p>");
}

function toggleDebug() {
    if (output.style.display == "none" || output.style.display == "") {
        output.style.display = "block";
    }
    else {
        output.style.display = "none";
    }
}

function HandleOrderAcknowledgement(messageData) {
    if (pendingOrders[messageData.OrderID] != null) {
        pendingOrders[messageData.OrderID](messageData);
        pendingOrders[messageData.OrderID] = null;
    } else {
        writeToScreen("No acknowledgment found for order " + messageData.OrderID + ".");

        var pendingOrderKeys = pendingOrders.keys();
        var keyMessage = "PendingOrder keys :";
        for (const key of pendingOrderKeys) {
            keyMessage += " " + key;
        }

        writeToScreen(keyMessage);
    }
}

function HandleAvailablePlayerSlots(messageData) {
    availableSlots = messageData.AvaialablePlayerSlots;

    // are we searching for a player slot ?
    if (localPlayerIndex < 0) {
        clearPlayArea();

        playArea.appendChild(faceCollection.RootNode);
        playerSlots.Setup(availableSlots);
        playArea.appendChild(playerSlots.RootNode);
    }
    else {
        // if not, how many are still open ?
        var numberOfAvailableSlots = 0;
        for (var index = 0; index < availableSlots.length; ++index) {
            if (availableSlots[index]) {
                numberOfAvailableSlots++;
            }
        }

        // we are waiting for the slots to be taken
        if (numberOfAvailableSlots > 0) {
            clearPlayArea();

            var p = document.createElement("p").appendChild(document.createTextNode("Waiting for " + numberOfAvailableSlots + " players."));
            playArea.appendChild(p);
        }
        else {
            // we are ready to play, are we already playing ?
            if (clientState == "None") {
                if (gameState == null) {
                    clientState = "WaitingForGameState";
                }
                else {
                    SetupFromGameState();
                }
            }
        }
    }
}

function HandlePlayerViewUpdate(messageData) {
    if (messageData.IsNextRoundState) {
        nextRoundState = messageData;
        return;
    }

    gameState = messageData;

    if (GameStateID[gameState.GameStateID] != null) {
        gameState.GameStateID = GameStateID[gameState.GameStateID];
    }

    if (clientState == "WaitingForGameState") {
        SetupFromGameState();
    }

    writeToScreen("Game state = " + gameState.GameStateID);
}

function HandleSandboxUpdate(messageData) {
    for (var changeIndex = 0; changeIndex < messageData.GameChanges.length; ++changeIndex) {
        var gameChange = messageData.GameChanges[changeIndex];
        var changeType = gameChange.ChangeType;
        if (changeType == "PlayedCard") {
            if (gameState.PlayerTurn != gameChange.PlayerIndex) {
                alert("Desync between local player and playing player.");
                return;
            }

            var card = new Card(gameChange.IndexOnBoard);
            card.Setup(gameChange.Card);
            var cardMini = new CardMini(gameChange.Card);

            if (gameChange.PlayerIndex == localPlayerIndex) {
                if (!AssertClientState("PlayedCard")) {
                    return;
                }

                player.Log("played " + cardMini.Label.textContent);

                PlayerhandModeUninteractable();
                PlayerBoardModeUninteractable();

                if (player.Board.Slots[gameChange.IndexOnBoard].Card == null) {
                    gameState.CardsInDeck--;
                    gameInfo.Deck.Setup(gameState);
                }

                player.Hand.Slots[gameChange.IndexInHand].DetatchCard();
                player.Board.Slots[gameChange.IndexOnBoard].AttachCard(card);
            }
            else {
                if (!AssertClientState("OtherPlayerTurn")) {
                    return;
                }

                opponent.Log("played " + cardMini.Label.textContent);

                if (opponent.Board.Slots[gameChange.IndexOnBoard].Card == null) {
                    gameState.CardsInDeck--;
                    gameInfo.Deck.Setup(gameState);
                }

                opponent.Board.Slots[gameChange.IndexOnBoard].AttachCard(card);
            }
        }
        else if (changeType == "PickedCard") {
            if (gameChange.PlayerIndex == localPlayerIndex) {
                var newCard = new Card(gameChange.IndexInHand);
                newCard.Setup(gameChange.Card);
                player.Log("picked " + newCard.CardLabel.textContent);
                player.Hand.Slots[gameChange.IndexInHand].AttachCard(newCard);
            }
        }

        else if (changeType == "PlayerCombo") {
            if (gameChange.PlayerIndex == localPlayerIndex) {
                player.Log("Combo " + gameChange.CardCombo);

                for (var index = 0; index < 3; ++index) {
                    if ((gameChange.UsedCards & 1 << index) != 0) {
                        gameState.CardsInDiscardPile++;
                        gameInfo.DiscardPile.Setup(gameState);
                        player.Board.Slots[index].DetatchCard();
                    }
                }
            }
            else {
                player.Log("Combo " + gameChange.CardCombo);
                for (var index = 0; index < 3; ++index) {
                    if ((gameChange.UsedCards & 1 << index) != 0) {
                        opponent.Board.Slots[index].DetatchCard();
                    }
                }
            }
        }
        else if (changeType == "PlayerPropertyChanged") {
            var target = player;
            if (gameChange.PlayerIndex != localPlayerIndex) {
                target = opponent;
            }

            if (gameChange.PlayerProperty == "Score") {
                target.ScoreLabel.textContent = "Score " + gameChange.NewValue;
            }

            if (gameChange.PlayerProperty == "Health") {
                target.HealthLabel.textContent = "Health " + gameChange.NewValue;
            }

            if (gameChange.PlayerProperty == "Shield") {
                target.ShieldLabel.textContent = "Shield " + gameChange.NewValue;
            }

            if (gameChange.PlayerProperty == "PairBullets") {
                target.PairBulletLabel.textContent = "Pair Combo " + gameChange.NewValue + "/" + gameState.PairComboSize;
            }
        }

        else if (changeType == "GameStateChange") {
            gameState.GameStateID = gameChange.GameStateID;
            if (gameState.GameStateID == "Transitioning") {
                gameState.PlayerTurn = gameChange.PlayerIndex;
                if (gameState.PlayerTurn == localPlayerIndex) {
                    PlayerHandModeChooseCard();
                    clientState = "SelectHandCard";
                }
                else {
                    clientState = "OtherPlayerTurn";
                }
            }
            else if (gameState.GameStateID == "Initialize") {
                gameState = nextRoundState;
                nextRoundState = null;
                clientState = "None";
            }
            else if (gameState.GameStateID == "Playing") {
                SetupFromGameState();
            }
            else if (gameState.GameStateID == "EndGame") {
                clearPlayArea();
                playArea.appendChild(endGame.RootNode);
                endGame.Setup(gameState.PlayerTurn);
            }
        }
    }
}

function SetupFromGameState() {
    if (gameState == null) {
        alert("GameState is null | clientState : " + clientState);
        return;
    }

    if (gameState.GameStateID == "Playing") {
        if (clientState == "None") {
            clearPlayArea();

            gameInfo.Setup(gameState.PlayerTurn, gameState);

            // Settuping the board.
            gameInfo.Setup(localPlayerIndex, gameState);
            playArea.appendChild(gameInfo.RootNode);

            opponent.Setup(gameState.OtherPlayer);
            playArea.appendChild(opponent.RootNode);

            player.Setup(gameState.CurrentPlayer);
            playArea.appendChild(player.RootNode);

            if (gameState.PlayerTurn == localPlayerIndex) {
                PlayerHandModeChooseCard();
                clientState = "SelectHandCard";
            }
            else {
                clientState = "OtherPlayerTurn";
            }
        }
    }
}

function SelectCardInHand(cardIndex) {
    if (!AssertClientState("SelectHandCard")) {
        return;
    }

    player.SelectedCardIndex = cardIndex;

    for (var index = 0; index < 3; ++index) {
        var card = player.Hand.Slots[index].Card;
        if (index == cardIndex) {
            card.SetInteractable(UnselectCard, "X");
        }
        else {
            card.SetNotInteractable();
        }
    }

    PlayerBoardModeChooseSlot();

    clientState = "SelectBoardSlot";
}

function UnselectCard(cardIndex) {
    if (!AssertClientState("SelectBoardSlot")) {
        return;
    }

    PlayerHandModeChooseCard();
    PlayerBoardModeUninteractable();
    clientState = "SelectHandCard";
}

function SelectBoardSlot(slotIndex) {
    if (!AssertClientState("SelectBoardSlot")) {
        return;
    }

    PlayerhandModeUninteractable();
    PlayerBoardModeUninteractable();
    clientState = "PlayedCard";
    RequestPlayCard(player.SelectedCardIndex, slotIndex);
}

function PlayerHandModeChooseCard() {
    for (var index = 0; index < 3; ++index) {
        var card = player.Hand.Slots[index].Card;
        card.SetInteractable(SelectCardInHand, "Play");
    }
}

function PlayerhandModeUninteractable() {
    for (var index = 0; index < 3; ++index) {
        var card = player.Hand.Slots[index].Card;
        card.SetNotInteractable();
    }
}

function PlayerBoardModeChooseSlot() {
    for (var index = 0; index < 3; ++index) {
        var slot = player.Board.Slots[index];
        slot.SetInteractable(SelectBoardSlot, "Play");
    }
}

function PlayerBoardModeUninteractable() {
    for (var index = 0; index < 3; ++index) {
        var slot = player.Board.Slots[index];
        slot.SetNotInteractable();
    }
}
function RequestPlayerFace(requestedIndex) {
    if (localPlayerIndex >= 0) {
        return;
    }

    var orderID = nextOrderID++;
    var requestPlayerIndex = '{"OrderType":"SelectPlayerFace", "FaceIndex": ' + requestedIndex + ', "OrderID" : ' + orderID + '}';
    pendingOrders[orderID] = function (responseData) {
        if (responseData.FailureFlags == "None") {
            localPlayerIndex = responseData.PlayerIndex;
            player.faceIndex = responseData.FaceIndex;
        }
    };

    DoSend(requestPlayerIndex);
}

function RequestPlayerSlots(requestedIndex) {
    if (localPlayerIndex >= 0) {
        return;
    }

    if (!availableSlots[requestedIndex]) {
        return;
    }

    var orderID = nextOrderID++;
    var requestPlayerIndex = '{"OrderType":"SelectPlayerSlot", "PlayerIndex": ' + requestedIndex + ', "OrderID" : ' + orderID + '}';
    pendingOrders[orderID] = function (responseData) {
        if (responseData.FailureFlags == "None") {
            localPlayerIndex = requestedIndex;
        }
    };

    DoSend(requestPlayerIndex);
}

function RequestPlayCard(cardIndex, boardIndex) {
    var orderID = nextOrderID++;
    var playOrder = '{"OrderType":"PlayCard", "CardIndex": ' + cardIndex + ', "BoardIndex": ' + boardIndex + ', "OrderID": ' + orderID + '}';
    pendingOrders[orderID] = function (responseData) { };

    DoSend(playOrder);
}

function clearChilds(node) {
    while (node.firstChild) {
        node.removeChild(node.firstChild);
    }
}

function clearPlayArea() {
    while (playArea.firstChild) {
        playArea.removeChild(playArea.firstChild)
    }
}

function createElementWithClass(element, className) {
    var el = document.createElement(element);
    el.className = className;
    return el;
}

function AssertClientState(expectedState) {
    if (clientState != expectedState) {
        alert("Unexpected state " + clientState + " expecting " + expectedState);
        return false;
    }

    return true;
}