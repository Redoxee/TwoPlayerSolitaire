class Deck {
    constructor() {
        this.RootNode = createElementWithClass("table", "deck");
        this.NumberOfCards = 0;
        var row = document.createElement("tr");
        this.RootNode.appendChild(row);
        this.CardCountLabel = document.createTextNode("");
        row.appendChild(this.CardCountLabel);
    }

    SetCardCount(cardCount) {
        this.CardCountLabel.textContent = cardCount;
    }
}