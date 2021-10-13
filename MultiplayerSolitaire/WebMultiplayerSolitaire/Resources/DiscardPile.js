class DiscardPile {
    constructor() {
        this.RootNode = createElementWithClass("div", "Deck");
        var div = createElementWithClass("div");
        var label = document.createTextNode("Discard");
        div.appendChild(label);
        this.RootNode.appendChild(div);

        div = createElementWithClass("div")
        this.CardCountLabel = document.createTextNode("");
        div.appendChild(this.CardCountLabel);
        this.RootNode.appendChild(div);
    }

    Setup(gameState) {
        this.CardCountLabel.textContent = gameState.CardsInDiscardPile;
    }
}