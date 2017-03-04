module.exports = function () {
  var instanceRoot = "C:\\inetpub\\wwwroot\\hackathon2017";
  var config = {
    websiteRoot: instanceRoot + "\\Website",
    sitecoreLibraries: instanceRoot + "\\Website\\bin",
    licensePath: instanceRoot + "\\Data\\license.xml",
    solutionName: "Habitat",
    buildConfiguration: "Debug",
    runCleanBuilds: false
  };
  return config;
}