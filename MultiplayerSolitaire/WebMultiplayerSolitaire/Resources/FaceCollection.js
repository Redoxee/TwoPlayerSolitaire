class FaceCollection {
    constructor() {
        this.RootNode = createElementWithClass("div", "FaceCollection");

        const xmlhttp = new XMLHttpRequest();
        let instance = this;
        xmlhttp.onload = function () {
            const myObj = JSON.parse(xmlhttp.responseText);
            instance.FacesData = myObj.FacesData;
            instance.OnFaceLoaded();
        }

        xmlhttp.open("GET", "Config.json");
        xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
        xmlhttp.send();
    }

    OnFaceLoaded() {
        this.Faces = [];

        for (var index = 0; index < this.FacesData.length; ++index) {
            var f = new Face();
            f.Setup(this.FacesData,index , 0);
            this.RootNode.appendChild(f.RootNode);
            this.Faces[index] = f;


            (function (capturedButton, capturedIndex) {
                capturedButton.addEventListener("click", function () {
                    RequestPlayerFace(capturedIndex);
                })
            })(this.Faces[index].Button, index);
        }
    }

    SetFaceVisibility(index, visible) {
        this.Faces[index].SetVisible(visible);
    }
}