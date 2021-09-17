class DiscardPile {
    constructor() {
        this.RootNode = createElementWithClass("table", "discardPile");
        this.Label = document.createTextNode("Cards in discard pile : ?");
        this.RootNode.appendChild(this.Label);
    }

    Setup(gameState) {
        this.Label.textContent = "Cards in discard Pile : " + gameState.CardsInDiscardPile;
    }
}