var c = document.getElementById("WebFitnessCanvas");
var ctx = c.getContext("2d");

ctx.font = "45px Verdana";
// Create gradient
var gradient = ctx.createLinearGradient(0, 0, c.width, 0);
gradient.addColorStop("0", " black");
gradient.addColorStop("0.5", "blue");
gradient.addColorStop("1", "teal");
// Fill with gradient
ctx.fillStyle = gradient;
ctx.fillText("Web Fitness", 10, 90);