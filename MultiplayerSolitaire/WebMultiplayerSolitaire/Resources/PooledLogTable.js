class PooledLogTable {
    constructor() {
        this.RootNode = createElementWithClass("table", "LogTable");
        this.PoolSize = 10;
        this.Pool = [];
        for (var index = 0; index < this.PoolSize; ++index)
        {
            this.Pool[index] = document.createElement("tr");
            this.Pool[index].Label = document.createTextNode("");
            this.Pool[index].appendChild(this.Pool[index].Label);
        }

        this.Count = 0;
    }

    Clear() {
        clearChilds(this.RootNode);
        this.Count = 0;
    }

    Log(message) {
        if (this.Count == this.PoolSize) {
            this.PoolSize = this.PoolSize + 20;
            for (var index = this.Count; index < this.PoolSize; ++index) {
                this.Pool[index] = document.createElement("tr");
                this.Pool[index].Label = document.createTextNode("");
                this.Pool[index].appendChild(this.Pool[index].Label);
            }
        }

        var poolElement = this.Pool[this.Count];
        var label = poolElement.Label;
        label.textContent = message;
        this.RootNode.appendChild(this.Pool[this.Count]);
        this.Count++;
    }
}