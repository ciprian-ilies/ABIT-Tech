var powerBiRenderer = (function() {

  function renderReport() { 
    var promise = new Promise(function(resolve, reject) {
      
        var embedContainer = $("#pbi-repiort")[0];
        var pbiConfig = getPbiConfig();
        var report = powerbi.embed(embedContainer, pbiConfig);

        report.off("rendered");
          report.on("rendered", function() {
            console.log("report was rendered");
            resolve(true);
          });

        report.on("error", function(event) {
            report.off("error");
            reject(new Error("The power BI component could not be loaded!"));
        });

     });
   return promise;
  }

  function getPbiConfig() { 
        var models = window["powerbi-client"].models;
        var permissions = models.Permissions.All;
        var config = {
            type: 'report',
            tokenType: models.TokenType.Embed,
            accessToken: "ACCESS_TOKEN",
            embedUrl: "EMBED_URL",
            id: "REPORT_ID",
            permissions: permissions,
            settings: {
              panes: {
                filters: {
                  visible: true
                },
                pageNavigation: {
                  visible: true
                }
              }
              }
         };
         return config;
  }


return {
  renderReport: renderReport
};
})();