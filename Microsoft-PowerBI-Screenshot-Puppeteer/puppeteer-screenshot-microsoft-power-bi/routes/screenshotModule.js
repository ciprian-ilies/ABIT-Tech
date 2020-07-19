module.exports = {
  takeScreenShot: async function () {

      const puppeteer = require('puppeteer');
      
          var getBrowserOptions = function () {
            var browserOptions = {
              headless: true,
              args: [
                '--disable-gpu',
                '--disable-dev-shm-usage',
                '--no-sandbox',
                '--disable-setuid-sandbox',
                '--ignore-certificate-errors'
              ],
              ignoreHTTPSErrors: true
            };
            return browserOptions;
          }

          const browserOptions = getBrowserOptions();
          const browser = await puppeteer.launch(browserOptions);

          const page = await browser.newPage();
          await page.goto(POWERBI_URL);

          const result = await page.evaluate(() => {
            return powerBiRenderer.renderReport();
          });

          if (result) {
           await page.screenshot({
              clip: {
                x: 0,
                y: 0,
                width: 550,
                height: 550
              },
               path: 'screenshot.jpg' 
            });
            await browser.close();
          }
  }
};