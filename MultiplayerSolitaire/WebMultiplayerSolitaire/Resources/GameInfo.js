class GameInfo {
    constructor() {
        this.RootNode = document.createElement("table");

        var roundParagraph = document.createElement("p");
        this.RootNode.appendChild(roundParagraph);
        this.RoundLabel = document.createTextNode("");
        roundParagraph.appendChild(this.RoundLabel);

        var playerParagraph = document.createElement("p");
        this.RootNode.appendChild(playerParagraph);
        this.CurrentPlayerMessage = "Current player : ";
        this.CurrentPlayerLabel = document.createTextNode(this.CurrentPlayerMessage);
        playerParagraph.appendChild(this.CurrentPlayerLabel);

        this.Deck = new Deck();
        this.RootNode.appendChild(this.Deck.RootNode);

        this.DiscardPile = new DiscardPile();
        this.RootNode.appendChild(this.DiscardPile.RootNode);
    }

    Setup(playerIndex, gameState) {
        this.CurrentPlayerLabel.textContent = this.CurrentPlayerMessage + (playerIndex + 1);
        this.RoundLabel.textContent = "Round " + gameState.RoundIndex + 1;
        this.Deck.Setup(gameState);
        this.DiscardPile.Setup(gameState);
    }

    SetCurrentPlayer(currentPlayer) {
        this.CurrentPlayerLabel.textContent = this.CurrentPlayerMessage + (currentPlayer + 1);
    }
}