﻿    class Opponent {
        constructor() {
            this.RootNode = createElementWithClass("table", "otherPlayer");

            var row = document.createElement("tr");
            this.RootNode.appendChild(row);
            this.PlayerLabel = document.createTextNode("Player ?");
            row.appendChild(this.PlayerLabel);

            this.ScoreLabel = document.createTextNode("Score : ");
            this.HealthLabel = document.createTextNode("Health : ");
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
            statsHeader.appendChild(this.PairBulletLabel);

            var row = document.createElement("tr");
            this.RootNode.appendChild(row);
            this.Board = new CardDisplay();
            row.appendChild(this.Board.RootNode);
        }

        Setup(opponentData) {
            this.PlayerLabel.textContent = "Player " + (opponentData.Index + 1);
            this.Board.SetupFromArray(opponentData.Board);

            this.ScoreLabel.textContent = "Score : " + opponentData.Score;
            this.HealthLabel.textContent = "Health : " + opponentData.Health;
            this.PairBulletLabel.textContent = "Pair Combo : " + opponentData.PairCombo;
        }

        Log(message) {
            writeToScreen(message);
        }
    }