class Card {
    constructor(cardIndex) {
        this.RootNode = createElementWithClass("table", "card");
        this.CardIndex = cardIndex;
        var row = document.createElement("tr");
        this.ContentHolder = document.createElement("div");
        row.appendChild(this.ContentHolder);
        this.CardLabel = document.createTextNode("? of ?");
        this.ContentHolder.appendChild(this.CardLabel);
        this.RootNode.appendChild(row);
        row = document.createElement("tr");
        this.Button = document.createElement("button");
        this.ButtonLabel = document.createTextNode("Play");
        row.appendChild(this.Button);
        this.Button.appendChild(this.ButtonLabel);
        this.RootNode.appendChild(row);
        this.Button.hidden = true;

        this.Action = function (index) { alert("unset"); };
        var instance = this;
        this.Button.addEventListener("click", function () {
            if (instance.Action != null) {
                instance.Action(instance.CardIndex);
            }
        });
    }

    Setup(cardData) {
        this.CardData = cardData;
        if (cardData.Value < 0) {
            this.ContentHolder.style.display = "none";
        }
        else {
            this.ContentHolder.style.display = "block";
            this.CardLabel.textContent = ValueLabel[cardData.Value] + " of " + SigilSymbol[cardData.Sigil];
        }
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