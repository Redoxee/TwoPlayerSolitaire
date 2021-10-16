class CardDisplay {
    constructor(styleClass, id) {
        this.RootNode = createElementWithClassAndId("div", styleClass, id);
        this.Slots = [3]; // Array of card slots.
        for (var index = 0; index < 3; ++index) {
            var slot = new CardSlot(index);
            this.Slots[index] = slot;
            this.RootNode.appendChild(this.Slots[index].RootNode);
        }
    }

    SetupFromArray(cardArray) {
        for (var index = 0; index < 3; ++index) {
            if (cardArray[index].Value > -1) {
                var card = new Card(index);
                card.Setup(cardArray[index]);
                this.Slots[index].AttachCard(card);
            }
            else {
                this.Slots[index].DetatchCard();
            }
        }
    }
}