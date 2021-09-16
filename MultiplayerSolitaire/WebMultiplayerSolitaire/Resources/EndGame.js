class EndGame {
    constructor() {
        this.RootNode = createElementWithClass("table","EndGame");

        this.EndGameLabel = document.createTextNode("Game Ended");
        var col = document.createElement("th");
        var row = document.createElement("tr");
        col.appendChild(row);
        row.appendChild(this.EndGameLabel);
        this.RootNode.appendChild(col);
        this.WinningPlayerLabel = document.createTextNode("Player ? Win");
        row = document.createElement("tr");
        row.appendChild(this.WinningPlayerLabel);
        col.appendChild(row);
    }

    Setup(winningPlayer) {
        this.WinningPlayerLabel.textContent = "Player " + (winningPlayer + 1) + " Win!";
    }
}