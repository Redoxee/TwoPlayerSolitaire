class GameInfo {
    constructor() {
        this.RootNode = document.createElement("table");

        var roundParagraph = document.createElement("p");
        this.RootNode.appendChild(roundParagraph);
        this.RoundMessage = "Round : ";
        this.RoundLabel = document.createTextNode(this.RoundMessage);
        roundParagraph.appendChild(this.RoundLabel);

        var playerParagraph = document.createElement("p");
        this.RootNode.appendChild(playerParagraph);
        this.CurrentPlayerMessage = "Current player : ";
        this.CurrentPlayerLabel = document.createTextNode(this.CurrentPlayerMessage);
        playerParagraph.appendChild(this.CurrentPlayerLabel);

        this.Deck = new Deck();
        this.RootNode.appendChild(this.Deck.RootNode);
    }

    Setup(playerIndex, gameState) {
        this.CurrentPlayerLabel.textContent = this.CurrentPlayerMessage + (playerIndex + 1);
        this.RoundMessage = "Round " + gameState.Round + 1;
        this.RoundLabel.textContent = this.RoundMessage;
        this.Deck.SetCardCount(gameState.CardsInDeck);
    }

    SetCurrentPlayer(currentPlayer) {
        this.CurrentPlayerLabel.textContent = this.CurrentPlayerMessage + (currentPlayer + 1);
    }
}