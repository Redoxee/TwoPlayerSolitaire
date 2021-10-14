class Card {
    constructor(cardIndex) {
        this.RootNode = createElementWithClass("div", "Card");
        this.CardIndex = cardIndex;

        this.CardLabel = document.createTextNode("? of ?");
        this.Button = document.createElement("button");

        this.Action = function (index) { alert("unset"); };
        var instance = this;
        this.Button.addEventListener("click", function () {
            if (instance.Action != null) {
                instance.Action(instance.CardIndex);
            }
        });

        this.SetNotInteractable();
    }

    Setup(cardData) {
        this.CardData = cardData;
        if (cardData.Value < 0) {
            clearChilds(this.RootNode);
        }
        else {
            this.CardLabel.textContent = ValueLabel[cardData.Value] + " of " + SigilSymbol[cardData.Sigil];
        }
    }

    SetInteractable(action, label) {
        clearChilds(this.RootNode);
        clearChilds(this.Button);
        this.RootNode.appendChild(this.Button);
        this.Button.appendChild(this.CardLabel);
        this.Action = action;
    }

    SetNotInteractable() {
        clearChilds(this.RootNode);
        clearChilds(this.Button);
        this.RootNode.appendChild(this.CardLabel);
        this.Action = null;
    }
}