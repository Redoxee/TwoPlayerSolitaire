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

        this.ScoreLabel = document.createTextNode("Score : ");
        this.HealthLabel = document.createTextNode("Health : ");
        this.ShieldLabel = document.createTextNode("Shield : ");
        this.PairBulletLabel = document.createTextNode("Pair Bullet : ");

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
        statsHeader.appendChild(this.ShieldLabel);
        statsHeader = document.createElement("th");
        statsTable.appendChild(statsHeader);
        statsHeader.appendChild(this.PairBulletLabel);

        this.SelectedCardIndex = -1;
    }

    Setup(gameData) {
        this.PlayerLabel.textContent = "Player " + (gameData.PlayerIndex + 1);
        this.Hand.SetupFromArray(gameData.Hand);
        this.Board.SetupFromArray(gameData.Board);

        this.ScoreLabel.textContent = "Score : " + gameData.Score;
        this.HealthLabel.textContent = "Health : " + gameData.Health;
        this.ShieldLabel.textContent = "Shield : " + gameData.Shield;
        this.PairBulletLabel.textContent = "Pair Bullet : " + gameData.PairBullets;
    }
}