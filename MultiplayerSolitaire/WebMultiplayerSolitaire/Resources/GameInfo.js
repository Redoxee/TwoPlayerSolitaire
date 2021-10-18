class GameInfo {
    constructor() {
        this.RootNode = createElementWithClass("div", "GeneralInfoContainer");

        this.Deck = new CardPile("Deck", -3);
        this.RootNode.appendChild(this.Deck.RootNode);

        this.DiscardPile = new CardPile("DiscardPile", 0);
        this.RootNode.appendChild(this.DiscardPile.RootNode);
    }

    Setup(gameState) {
        this.Deck.SetupDeck(gameState);
        this.DiscardPile.SetupDiscard(gameState);
    }
}