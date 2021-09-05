class CardDisplay {
    constructor() {
        this.RootNode = createElementWithClass("table", "cardDisplay");
        var row = document.createElement("tr");
        this.RootNode.appendChild(row);
        this.Slots = [3]; // Array of card slots.
        for (var index = 0; index < 3; ++index) {
            var slot = new CardSlot(index);
            this.Slots[index] = slot;
            var col = document.createElement("th");
            row.appendChild(col);
            col.appendChild(this.Slots[index].RootNode);

            var card = new Card(index);
            slot.AttachCard(card);
        }
    }

    SetupFromArray(cardArray) {
        for (var index = 0; index < 3; ++index) {
            var slot = this.Slots[index].Card.Setup(cardArray[index]);
        }
    }
}