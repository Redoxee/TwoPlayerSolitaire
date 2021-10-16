class PooledLogTable {
    constructor() {
        this.RootNode = createElementWithClass("Div", "GameLog");
        this.PoolSize = 15;
        this.Pool = [];
        for (var index = 0; index < this.PoolSize; ++index)
        {
            this.Pool[index] = createElementWithClass("div", "LogEntry");
            this.Pool[index].Label = document.createTextNode("");
        }

        this.Cursor = 0;
    }

    Clear() {
        clearChilds(this.RootNode);
        this.Cursor = 0;
    }

    Log(message, additionalCards) {
        var poolElement = this.Pool[this.Cursor];
        clearChilds(poolElement);
        poolElement.appendChild(poolElement.Label);

        var label = poolElement.Label;
        label.textContent = message;
        if (additionalCards != null) {
            for (var index = 0; index < additionalCards.length; ++index) {
                poolElement.appendChild(additionalCards[index].RootNode);
            }
        }

        this.RootNode.appendChild(poolElement);
        this.Cursor++;
        if (this.Cursor >= this.PoolSize) {
            this.Cursor = 0;
        }
    }
}