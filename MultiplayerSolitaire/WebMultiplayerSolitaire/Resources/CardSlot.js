class CardSlot {
    constructor(index) {
        this.Index = index;
        this.RootNode = createElementWithClass("div", "CardSlot");
        this.CardReceptacle = createElementWithClass("div", "CardReceptacle");
        this.RootNode.appendChild(this.CardReceptacle);
        this.Card = null;

        this.Button = document.createElement("button");
        this.ButtonLabel = document.createTextNode("_");
        this.Button.appendChild(this.ButtonLabel);

        this.Action = function (index) { alert("unset"); };
        var instance = this;
        this.Button.addEventListener("click", function () {
            if (instance.Action != null) {
                instance.Action(instance.Index);
            }
        });
    }

    AttachCard(card) {
        clearChilds(this.RootNode);
        clearChilds(this.CardReceptacle);
        this.RootNode.appendChild(this.CardReceptacle);

        if (card == null || card.Value < 0) {
            this.Card = null;
            return;
        }

        this.Card = card;
        this.CardReceptacle.appendChild(card.RootNode);
    }

    DetatchCard() {
        clearChilds(this.CardReceptacle);
        this.Card = null;
    }

    SetInteractable(action, label) {
        clearChilds(this.RootNode);
        if (this.Card == null) {
            this.RootNode.appendChild(this.Button);
            this.ButtonLabel.textContent = label;
            this.Action = action;
        }
        else {
            this.AttachCard(this.Card);
            this.Card.SetInteractable(action, label);
        }
    }

    SetNotInteractable() {
        clearChilds(this.RootNode);
        if (this.Card != null) {
            this.AttachCard(this.Card);
            this.Card.SetNotInteractable();
        }
    }
}