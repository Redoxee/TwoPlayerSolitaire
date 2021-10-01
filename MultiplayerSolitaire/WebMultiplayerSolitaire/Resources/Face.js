
FacesData = [
    [
        [
            " ~~~~~~~~~ ",
            "/ ~~~~~~~ \\",
            "|          |",
            "|   O   O  |",
            "|     U    |",
            "|   \\___/  |",
            "\\         /",
            " ¯¯¯¯¯¯¯¯¯ ",
        ],
    ],
    [
        [
            " ~~~~~~~~~ ",
            "/~~~      \\",
            "|~~        |",
            "|~  O   O  |",
            "|   ¯ V ¯  |",
            "|# |____| #|",
            "\\#########/",
            "  ¯¯¯¯¯¯¯¯  ",
        ],
    ],
]

class Face {
    constructor() {
        this.RootNode = createElementWithClass("div", "Face");
        this.Content = document.createElement("button");
        this.RootNode.appendChild(this.Content);
        this.Rows = [];
        for (var index = 0; index < 8; ++index) {
            var pre = document.createElement("pre");
            this.Content.appendChild(pre);

            this.Rows[index] = document.createTextNode("");
            pre.appendChild(this.Rows[index]);
        }
    }

    Setup(faceData, faceIndex, expressionIndex) {
        this.Index = faceIndex;
        for (var index = 0; index < 8; ++index) {
            this.Rows[index].textContent = faceData[expressionIndex][index];
        }
    }
}