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
    "♠",
    "♣",
    "♥",
    "♦"];

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

const MiniValueLabel = [
    "1",
    "2",
    "3",
    "4",
    "5",
    "6",
    "7",
    "8",
    "9",
    "10",
]

var websocket = null;
var isConnected = false;
var gameWebSocketUrl = document.URL.replace("http://", "ws://");

var output = document.querySelector("#debugOutput");
var playArea = document.querySelector("#playArea");

var playerSlots = new PlayerSlots();
var gameInfo = new GameInfo();
var player = new Player("LocalPlayer", "You");
var opponent = new Player("Opponent","Opponent");
var endGame = new EndGame();
var faceCollection = new FaceCollection();
var gameLog = new PooledLogTable();

var clientState = "None";
var localPlayerIndex = -1;
var availableSlots = [];
var nextOrderID = 0;

var pendingOrders = [];
var gameState = null;
var nextRoundState = null;

var messageHandles = [];
messageHandles["AvailableFaces"] = HandleAvailableFaces;
messageHandles["OrderAcknowledgement"] = HandleOrderAcknowledgement;
messageHandles["PlayerViewUpdate"] = HandlePlayerViewUpdate;
messageHandles["SandboxChanges"] = HandleSandboxUpdate;

writeToScreen("WS URI " + gameWebSocketUrl);

CreateWebSocket();

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

function HandleAvailableFaces(messageData) {
    availableFaces = messageData.Faces;

    // are we searching for a player slot ?
    if (localPlayerIndex < 0) {
        clearPlayArea();

        for (var index = 0; index < availableFaces.length; ++index) {
            faceCollection.SetFaceVisibility(index, availableFaces[index]);
        }

        playArea.appendChild(faceCollection.RootNode);
    } else if (!messageData.ReadyToPlay) {
        clearPlayArea();
        var p = document.createElement("p").appendChild(document.createTextNode("Waiting for opponent."));
        playArea.appendChild(p);
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

                gameLog.Log("You played ", [cardMini]);

                PlayerhandModeUninteractable();
                PlayerBoardModeUninteractable();

                if (player.Board.Slots[gameChange.IndexOnBoard].Card == null) {
                    gameState.CardsInDeck--;
                }
                else {
                    gameInfo.Deck.AddCard(player.Board.Slots[gameChange.IndexOnBoard].Card.CardData);
                }

                player.Hand.Slots[gameChange.IndexInHand].DetatchCard();
                player.Board.Slots[gameChange.IndexOnBoard].AttachCard(card);
            }
            else {
                if (!AssertClientState("OtherPlayerTurn")) {
                    return;
                }

                gameLog.Log("Opponent played ", [cardMini]);

                if (opponent.Board.Slots[gameChange.IndexOnBoard].Card == null) {
                    gameState.CardsInDeck--;
                }
                else {
                    gameInfo.Deck.AddCard(opponent.Board.Slots[gameChange.IndexOnBoard].Card.CardData);
                }

                opponent.Board.Slots[gameChange.IndexOnBoard].AttachCard(card);
                gameInfo.Deck.RemoveCard(gameChange.Card);
            }
        }
        else if (changeType == "PickedCard") {
            if (gameChange.PlayerIndex == localPlayerIndex) {
                gameState.CurrentPlayer.Hand[gameChange.IndexInHand] = gameChange.Card;
                var newCard = new Card(gameChange.IndexInHand);
                newCard.Setup(gameChange.Card);

                var newCardMini = new CardMini(gameChange.Card);
                gameLog.Log("You picked ", [newCardMini]);
                player.Hand.Slots[gameChange.IndexInHand].AttachCard(newCard);
                gameInfo.Deck.RemoveCard(gameChange.Card);
            }
        }

        else if (changeType == "PlayerCombo") {
            var cardInCombo = [];
            var numberOfCombo = 0;

            if (gameChange.PlayerIndex == localPlayerIndex) {
                for (var index = 0; index < 3; ++index) {
                    if ((gameChange.UsedCards & 1 << index) != 0) {
                        gameState.CardsInDiscardPile++;
                        gameInfo.DiscardPile.AddCard(player.Board.Slots[index].Card.CardData);
                        cardInCombo[numberOfCombo++] = new CardMini(player.Board.Slots[index].Card.CardData);

                        player.Board.Slots[index].DetatchCard();
                    }
                }
            }
            else {
                for (var index = 0; index < 3; ++index) {
                    if ((gameChange.UsedCards & 1 << index) != 0) {
                        gameState.CardsInDiscardPile++;
                        gameInfo.DiscardPile.AddCard(opponent.Board.Slots[index].Card.CardData);
                        cardInCombo[numberOfCombo++] = new CardMini(opponent.Board.Slots[index].Card.CardData);

                        opponent.Board.Slots[index].DetatchCard();
                    }
                }
            }

            gameLog.Log("Combo " + gameChange.CardCombo, cardInCombo);
        }
        else if (changeType == "PlayerPropertyChanged") {
            var target = player;
            if (gameChange.PlayerIndex != localPlayerIndex) {
                target = opponent;
            }

            if (gameChange.PlayerProperty == "Score") {
                target.PlayerStats.SetScore(gameChange.NewValue);

                if (gameChange.PlayerIndex == localPlayerIndex) {
                    gameLog.Log("You won the round!");
                }
                else {
                    gameLog.Log("Opponent win the round!");
                }
            }

            if (gameChange.PlayerProperty == "Health") {
                target.PlayerStats.SetHealth(gameChange.NewValue);
            }

            if (gameChange.PlayerProperty == "PairCombo") {
                target.PlayerStats.SetPairCombo(gameChange.NewValue);
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
                clientState = "WaitForPlay";
            }
            else if (gameState.GameStateID == "Playing") {
                if (clientState == "WaitForPlay") {
                    InitializeRound();
                }
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

    clearPlayArea();

    player.PlayerStats.SetPairComboSize(gameState.PairComboSize);
    opponent.PlayerStats.SetPairComboSize(gameState.PairComboSize);

    player.Attach(playArea, true);
    player.Header.Face.Setup(faceCollection.FacesData, gameState.CurrentPlayer.FaceIndex, 0);
    player.Setup(gameState.CurrentPlayer);

    opponent.Attach(playArea, false);
    opponent.Header.Face.Setup(faceCollection.FacesData, gameState.OtherPlayer.FaceIndex, 0);
    opponent.Setup(gameState.OtherPlayer);

    // Settuping the board.
    gameInfo.Setup(gameState);
    playArea.appendChild(gameInfo.RootNode);

    playArea.appendChild(gameLog.RootNode);

    if (gameState.PlayerTurn == localPlayerIndex) {
        PlayerHandModeChooseCard();
        clientState = "SelectHandCard";
    }
    else {
        clientState = "OtherPlayerTurn";
    }

    var playerSeparator = createElementWithClass("div", "PlayerSeparator");
    playArea.appendChild(playerSeparator);
}

function InitializeRound() {
    opponent.Setup(gameState.OtherPlayer);
    player.Setup(gameState.CurrentPlayer);
    gameInfo.Setup(gameState);
    if (gameState.PlayerTurn == localPlayerIndex) {
        PlayerHandModeChooseCard();
        clientState = "SelectHandCard";
    }
    else {
        clientState = "OtherPlayerTurn";
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

            clientState = "WaitingForGameState";
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
    node.innerHTML = "";
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

function createElementWithClassAndId(element, className, id) {
    var el = document.createElement(element);
    el.className = className;
    el.id = id;
    return el;
}

function AssertClientState(expectedState) {
    if (clientState != expectedState) {
        alert("Unexpected state " + clientState + " expecting " + expectedState);
        return false;
    }

    return true;
}