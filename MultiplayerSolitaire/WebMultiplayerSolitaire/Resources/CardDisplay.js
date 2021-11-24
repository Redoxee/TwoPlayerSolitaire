class CardDisplay {
    constructor(styleClass, id, maxNumberOfCards, hideOnEmptySlot) {
        this.RootNode = createElementWithClassAndId("div", styleClass, id);
        this.Slots = [maxNumberOfCards]; // Array of card slots.
        for (var index = 0; index < maxNumberOfCards; ++index) {
            var slot = new CardSlot(index, hideOnEmptySlot);
            this.Slots[index] = slot;
            this.RootNode.appendChild(this.Slots[index].RootNode);
        }
    }

    SetupFromArray(cardArray) {
        for (var index = 0; index < cardArray.length; ++index) {
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