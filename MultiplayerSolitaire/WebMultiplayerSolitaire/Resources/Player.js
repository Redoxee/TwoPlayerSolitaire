class Player {
    constructor(id, labelName) {
        this.Header = new PlayerHeader(id, labelName);
        this.Header.Face.SetInteractable(false);
        this.PlayerStats = new PlayerStats(id);
        this.Board = new CardDisplay("PlayerBoard", id, 3, false);
        this.Hand = new CardDisplay("PlayerHand", id, 4, true);
        this.HasHand = false;

        this.SelectedCardIndex = -1;
    }

    Attach(node, includeHand) {
        node.appendChild(this.Header.RootNode);
        node.appendChild(this.PlayerStats.RootNode);
        node.appendChild(this.Board.RootNode);
        if (includeHand) {
            node.appendChild(this.Hand.RootNode);
        }

        this.HasHand = includeHand;
    }

    Setup(playerObject) {
        this.PlayerStats.SetScore(playerObject.Score);
        this.PlayerStats.SetHealth(playerObject.Health);
        this.PlayerStats.SetPairCombo(playerObject.PairCombo);
        if (this.HasHand) {
            this.Hand.SetupFromArray(playerObject.Hand);
        }

        this.Board.SetupFromArray(playerObject.Board);
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

class PlayerStats {
    constructor(id) {
        this.RootNode = createElementWithClassAndId("div", "PlayerStats", id);
        var div = createElementWithClass("div");
        this.Score = document.createTextNode("Score : _");
        div.appendChild(this.Score);
        this.RootNode.appendChild(div);

        div = createElementWithClass("div");
        this.Health = document.createTextNode("Health : _");
        div.appendChild(this.Health);
        this.RootNode.appendChild(div);

        div = createElementWithClass("div");
        this.PairCombo = document.createTextNode("PairCombo : _");
        div.appendChild(this.PairCombo);
        // this.RootNode.appendChild(div);
    }

    SetPairComboSize(comboSize) {
        this.PairComboSize = comboSize;
    }

    SetScore(score) {
        this.Score.textContent = "Score : " + score;
    }

    SetHealth(health) {
        this.Health.textContent = "Health : " + health; 
    }

    SetPairCombo(pairCombo) {
        this.PairCombo.textContent = "Pair combo : " + pairCombo + "/" + this.PairComboSize;
    }
}