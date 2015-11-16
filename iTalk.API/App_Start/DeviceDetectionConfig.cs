using System.Web.Hosting;
using System.Web.WebPages;
using WURFL;
using WURFL.Config;
using System.Linq;

namespace iTalk.API {
    public static class DeviceDetectionConfig {
        public static void Config() {
            var wurflDataFile = HostingEnvironment.MapPath("~/App_Data/wurfl-latest.zip");
            var configurer = new InMemoryConfigurer()
                .MainFile(wurflDataFile)
                .SetMatchMode(MatchMode.Accuracy);
            WURFLManagerBuilder.Build(configurer);

            var mobileMode = DisplayModeProvider.Instance.Modes
                .FirstOrDefault(m => m.DisplayModeId == "Mobile") as DefaultDisplayMode;

            if (mobileMode != null) {
                mobileMode.ContextCondition = ctx => {
                    string userAgent = ctx.GetOverriddenUserAgent();
                    IDevice device = WURFLManagerBuilder.Instance.GetDeviceForRequest(userAgent);
                    bool isMobile;
                    bool.TryParse(device.GetCapability("is_smartphone"), out isMobile);

                    return isMobile;
                };
            }
        }
    }
}