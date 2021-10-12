class Face {
    constructor() {
        this.RootNode = createElementWithClass("div", "Face");
        this.Button = document.createElement("button");
        this.Div = document.createElement("div");

        this.Rows = [];
        this.Pres = [];
        for (var index = 0; index < 8; ++index) {
            this.Pres[index] = document.createElement("pre");
            this.Rows[index] = document.createTextNode("");
            this.Pres[index].appendChild(this.Rows[index]);
        }

        this.SetInteractable(true);
    }

    SetVisible(isVisible) {
        if (!isVisible) {
            clearChilds(this.RootNode);
        }
        else {
            this.SetInteractable(this.IsInteractable);
        }
    }

    SetInteractable(isInteractable) {
        var content;
        clearChilds(this.RootNode);
        if (isInteractable) {
            this.RootNode.append(this.Button);
            content = this.Button;
        }
        else {
            this.RootNode.appendChild(this.Div);
            content = this.Div;
        }

        clearChilds(content);
        for (var index = 0; index < this.Pres.length; ++index) {
            content.appendChild(this.Pres[index]);
            clearChilds(this.Pres[index]);
            this.Pres[index].appendChild(this.Rows[index]);
        }

        this.IsInteractable = isInteractable;
    }

    Setup(faceData, faceIndex, expressionIndex) {
        this.Index = faceIndex;
        for (var index = 0; index < this.Rows.length; ++index) {
            this.Rows[index].textContent = faceData[faceIndex][expressionIndex][index];
        }
    }
}