var frames : Texture2D[]:
var framesPorSegundo = 10.0;
function Update() {
    var index : int = Time.time * framesPorSegundo;
    index = index % frames.length;
    GetComponent.<Renderer>().material.mainTexture = frames[index];
}