class Player {
    constructor(id, labelName) {
        this.Header = new PlayerHeader(id, labelName);
        this.Header.Face.SetInteractable(false);

        this.RootNode = createElementWithClass("table", "player");
        this.PlayerLabel = document.createTextNode("Player " + localPlayerIndex);
        var paragraph = document.createElement("p");
        paragraph.appendChild(this.PlayerLabel);
        this.RootNode.appendChild(paragraph);

        this.Board = new CardDisplay();
        this.RootNode.appendChild(this.Board.RootNode);

        this.Hand = new CardDisplay();
        this.RootNode.appendChild(this.Hand.RootNode);

        this.ScoreLabel = document.createTextNode("Score : ");
        this.HealthLabel = document.createTextNode("Health : ");
        this.PairBulletLabel = document.createTextNode("Pair Combo : ");

        var statsParagraph = createElementWithClass("tr", "Stats");
        this.RootNode.appendChild(statsParagraph);
        var statsTable = document.createElement("table");
        statsParagraph.appendChild(statsTable);

        var statsHeader = document.createElement("th");
        statsTable.appendChild(statsHeader);
        statsHeader.appendChild(this.ScoreLabel);
        statsHeader = document.createElement("th");
        statsTable.appendChild(statsHeader);
        statsHeader.appendChild(this.HealthLabel);
        statsHeader = document.createElement("th");
        statsTable.appendChild(statsHeader);
        statsHeader.appendChild(this.PairBulletLabel);

        this.SelectedCardIndex = -1;
    }

    Attach(node) {
        node.appendChild(this.Header.RootNode);
    }

    Setup(playerObject) {
        this.PlayerLabel.textContent = "Player " + (playerObject.Index + 1);
        this.Hand.SetupFromArray(playerObject.Hand);
        this.Board.SetupFromArray(playerObject.Board);

        this.ScoreLabel.textContent = "Score : " + playerObject.Score;
        this.HealthLabel.textContent = "Health : " + playerObject.Health;
        this.PairBulletLabel.textContent = "Pair Combo : " + playerObject.PairCombo;
    }

    Log(message) {
        writeToScreen(message);
    }
}

class PlayerHeader {
    constructor(id, labelName) {
        this.RootNode = createElementWithClassAndId("div", "PlayerHeader", id);
        this.Label = document.createTextNode(labelName);
        this.RootNode.appendChild(this.Label);
        this.Face = new Face();
        this.RootNode.appendChild(this.Face.RootNode);
    }
}