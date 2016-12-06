using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBM.Watson.DeveloperCloud.Services.AlchemyAPI.v1;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Utilities;

namespace TestDebug
{
  class Program
  {
    
    static void Main(string[] args)
    {
      LogSystem.InstallDefaultReactors();

      string path = Constants.Path.APP_DATA + Constants.Path.CONFIG_FILE;
      Log.Debug("Debug Program", "path: {0}", path);
      Config.Instance.LoadConfig();

      AlchemyAPI alchemyDataNews = new AlchemyAPI();

      if (!alchemyDataNews.GetNews((NewsResponse newsData, string data) =>
      {
        Log.Debug("Debug Program", "received! {0}", newsData);
      }))
        Log.Debug("Debug Program", "Failed!");
    }
  }
}
