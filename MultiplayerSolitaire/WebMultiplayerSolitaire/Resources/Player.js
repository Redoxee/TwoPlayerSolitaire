class Player {
    constructor() {
        this.RootNode = createElementWithClass("table", "player");
        this.PlayerLabel = document.createTextNode("Player " + localPlayerIndex);
        var paragraph = document.createElement("p");
        paragraph.appendChild(this.PlayerLabel);
        this.RootNode.appendChild(paragraph);

        this.Board = new CardDisplay();
        this.RootNode.appendChild(this.Board.RootNode);

        this.Hand = new CardDisplay();
        this.RootNode.appendChild(this.Hand.RootNode);

        this.SelectedCardIndex = -1;
    }

    Setup(gameData) {
        this.PlayerLabel.textContent = "Player " + (gameData.PlayerIndex + 1);
        this.Hand.SetupFromArray(gameData.Hand);
        this.Board.SetupFromArray(gameData.Board);
    }
}