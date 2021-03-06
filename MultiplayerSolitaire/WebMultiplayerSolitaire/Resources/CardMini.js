class CardMini {
    constructor(card) {
        this.RootNode = createElementWithClass("div", "CardMini");
        this.Label = document.createTextNode("");
        this.RootNode.appendChild(this.Label);
        this.Setup(card);
    }

    Setup(card) {
        this.Label.textContent = MiniValueLabel[card.Value] + SigilSymbol[card.Sigil];
        this.RootNode.id = SigilLabel[card.Sigil];
    }
}