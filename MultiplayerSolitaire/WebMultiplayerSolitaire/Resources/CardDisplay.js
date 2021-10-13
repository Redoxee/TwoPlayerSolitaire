class CardDisplay {
    constructor(styleClass, id) {
        this.RootNode = createElementWithClassAndId("table", styleClass, id);
        var row = document.createElement("tr");
        this.RootNode.appendChild(row);
        this.Slots = [3]; // Array of card slots.
        for (var index = 0; index < 3; ++index) {
            var slot = new CardSlot(index);
            this.Slots[index] = slot;
            var col = document.createElement("th");
            row.appendChild(col);
            col.appendChild(this.Slots[index].RootNode);
        }
    }

    SetupFromArray(cardArray) {
        for (var index = 0; index < 3; ++index) {
            if (cardArray[index].Value > -1) {
                var card = new Card(index);
                card.Setup(cardArray[index]);
                var slot = this.Slots[index].AttachCard(card);
            }
        }
    }
}