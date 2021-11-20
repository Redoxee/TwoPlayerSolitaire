class CardPile {
    constructor(title, countDelta) {
        this.RootNode = createElementWithClass("div", "CardPile");

        this.CountDelta = countDelta;

        var div = createElementWithClass("div");
        var label = document.createTextNode(title);
        div.appendChild(label);
        this.RootNode.appendChild(div);

        div = createElementWithClass("div")
        this.CardCountLabel = document.createTextNode("");
        div.appendChild(this.CardCountLabel);
        this.RootNode.appendChild(div);

        this.MiniCardDisplay = createElementWithClass("div", "MiniCardDisplay");
        this.RootNode.appendChild(this.MiniCardDisplay);

        this.CardColumns = [];
        for (var sigilIndex = 0; sigilIndex < SigilLabel.length; ++sigilIndex) {
            this.CardColumns[sigilIndex] = createElementWithClass("div", "MiniCardColumn");
            this.MiniCardDisplay.appendChild(this.CardColumns[sigilIndex]);
        }
    }

    SetupDeck(gameState) {
        this.NumberOfCards = this.CountDelta;
        this.CardCountLabel.textContent = gameState.CardsInDeck;
        this.AllCards = [];
        for (var sigilIndex = 0; sigilIndex < SigilLabel.length; ++sigilIndex) {
            this.AllCards[sigilIndex] = [];
            clearChilds(this.CardColumns[sigilIndex]);
            for (var valueIndex = 0; valueIndex < ValueLabel.length; ++valueIndex) {
                this.AddCard({ Value: valueIndex, Sigil: sigilIndex });
            }
        }

        for (var index = 0; index < gameState.CurrentPlayer.Hand.length; ++index) {
            if (gameState.CurrentPlayer.Hand[index].Value > -1) {
                this.RemoveCard(gameState.CurrentPlayer.Hand[index]);
            }
        }

        for (var index = 0; index < gameState.CurrentPlayer.Board.length; ++index) {
            if (gameState.CurrentPlayer.Board[index].Value > -1) {
                this.RemoveCard(gameState.CurrentPlayer.Board[index]);
            }
        }
    }

    SetupDiscard(gameState) {
        this.NumberOfCards = this.CountDelta;
        this.CardCountLabel.textContent = gameState.CardsInDiscardPile;
        this.AllCards = [];
        for (var sigilIndex = 0; sigilIndex < SigilLabel.length; ++sigilIndex) {
            this.AllCards[sigilIndex] = [];
            clearChilds(this.CardColumns[sigilIndex]);
        }

        this.RefreshCountLabel();
    }

    AddCard(card) {
        this.AllCards[card.Sigil][card.Value] = new CardMini(card);
        this.CardColumns[card.Sigil].appendChild(this.AllCards[card.Sigil][card.Value].RootNode);
        this.NumberOfCards++;
        this.RefreshCountLabel();
    }

    RemoveCard(card) {
        this.CardColumns[card.Sigil].removeChild(this.AllCards[card.Sigil][card.Value].RootNode);
        this.NumberOfCards--;
        this.RefreshCountLabel();
    }

    RefreshCountLabel() {
        this.CardCountLabel.textContent = this.NumberOfCards;
    }
}