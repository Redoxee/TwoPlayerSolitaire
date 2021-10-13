class GameInfo {
    constructor() {
        this.RootNode = createElementWithClass("div", "GeneralInfoContainer");

        this.Deck = new Deck();
        this.RootNode.appendChild(this.Deck.RootNode);

        this.DiscardPile = new DiscardPile();
        this.RootNode.appendChild(this.DiscardPile.RootNode);
    }

    Setup(playerIndex, gameState) {
        this.Deck.Setup(gameState);
        this.DiscardPile.Setup(gameState);
    }
}