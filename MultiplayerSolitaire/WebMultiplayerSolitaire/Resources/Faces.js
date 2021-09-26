
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
        var content = document.createElement("button");
        this.RootNode.appendChild(content);
        this.Rows = [];
        for (var index = 0; index < 8; ++index) {
            var pre = document.createElement("pre");
            content.appendChild(pre);

            this.Rows[index] = document.createTextNode("");
            pre.appendChild(this.Rows[index]);
        }

        this.Setup(1, 0);
    }

    Setup(faceIndex, expressionIndex) {
        for (var index = 0; index < 8; ++index) {
            this.Rows[index].textContent = FacesData[faceIndex][expressionIndex][index];
        }
    }
}