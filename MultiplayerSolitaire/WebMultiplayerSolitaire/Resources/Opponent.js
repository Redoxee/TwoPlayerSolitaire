    class Opponent {
        constructor() {
            this.RootNode = createElementWithClass("table", "otherPlayer");

            var row = document.createElement("tr");
            this.RootNode.appendChild(row);
            this.PlayerLabel = document.createTextNode("Player ?");
            row.appendChild(this.PlayerLabel);

            row = document.createElement("tr");
            this.RootNode.appendChild(row);
            this.ScoreLabel = document.createTextNode("Score : ?");
            row.appendChild(this.ScoreLabel);


            row = document.createElement("tr");
            this.RootNode.appendChild(row);
            this.Board = new CardDisplay();
            row.appendChild(this.Board.RootNode);
        }

        Setup(opponentData) {
            this.PlayerLabel.textContent = "Player " + (opponentData.Index + 1);
            this.ScoreLabel.textContent = "Health : " + opponentData.Health;
            this.Board.SetupFromArray(opponentData.Board);
        }
    }