class PlayerSlots {
    constructor() {
        this.RootNode = document.createElement("table");
        this.Buttons = [];
        var row = document.createElement("tr");
        this.RootNode.appendChild(row);

        this.PoolSize = 10;
        for (var index = 0; index < this.PoolSize; ++index) {
            this.Buttons[index] = document.createElement("button");
            var label = document.createTextNode("Player " + (index + 1));
            this.Buttons[index].appendChild(label);
            row.appendChild(this.Buttons[index]);

            (function (capturedButton, capturedIndex) {
                capturedButton.addEventListener("click", function () {
                    RequestPlayerSlots(capturedIndex);
                })
            })(this.Buttons[index], index);
        }
    }

    Setup(availableSlots) {
        for (var index = 0; index < this.PoolSize; ++index) {
            if (index >= availableSlots.length) {
                this.Buttons[index].hidden = true;
                continue;
            }

            if (!availableSlots[index]) {
                this.Buttons[index].hidden = true;
                continue;
            }

            this.Buttons[index].hidden = false;
        }
    }
}