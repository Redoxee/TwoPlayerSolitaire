class CardSlot {
    constructor(index) {
        this.Index = index;
        this.RootNode = createElementWithClass("table", "cardSlot");
        var row = document.createElement("tr");
        this.RootNode.appendChild(row);
        this.CardReceptacle = row;
        this.Card = null;

        row = document.createElement("tr");
        this.RootNode.appendChild(row);
        this.Button = document.createElement("button");
        row.appendChild(this.Button);
        this.ButtonLabel = document.createTextNode("_");
        this.Button.appendChild(this.ButtonLabel);

        this.Button.hidden = true;
        this.Action = function (index) { alert("unset"); };
        var instance = this;
        this.Button.addEventListener("click", function () {
            if (instance.Action != null) {
                instance.Action(instance.Index);
            }
        });
    }

    AttachCard(card) {
        clearChilds(this.CardReceptacle);
        if (card == null || card.Value < 0) {
            this.Card = null;
            return;
        }

        this.Card = card;
        this.CardReceptacle.appendChild(card.RootNode);
        this.SetNotInteractable();
    }

    DetatchCard() {
        clearChilds(this.CardReceptacle);
        this.Card = null;
    }

    SetInteractable(action, label) {
        this.Button.hidden = false;
        this.ButtonLabel.textContent = label;
        this.Action = action;
    }

    SetNotInteractable() {
        this.Action = null;
        this.Button.hidden = true;
    }
}