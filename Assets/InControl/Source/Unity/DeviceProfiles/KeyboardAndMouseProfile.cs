using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InControl.UnityDeviceProfiles {
    // @cond nodoc
    [Preserve] [UnityInputDeviceProfile]
    public class KeyboardAndMouseProfile : InputDeviceProfile {
        public override void Define() {
            base.Define();

            DeviceName = "Keyboard and Mouse";
            DeviceNotes = "Keyboard and Mouse";

            DeviceClass = InputDeviceClass.Keyboard;
            DeviceStyle = InputDeviceStyle.Unknown;

            IncludePlatforms = new[]
            {
                "Amazon AFT"
            };

            Matchers = new[] { new InputDeviceMatcher { NameLiteral = "Keyboard" } };

            LastResortMatchers = new[] { new InputDeviceMatcher { NamePattern = "..." } };

            ButtonMappings = new[]
            {
                new InputControlMapping
                {
                    Name = "Fire - Mouse",
                    Target = InputControlType.Action1,
                    Source = Space
                },

            };

            AnalogMappings = new InputControlMapping[]
            {

            };
        }
    }
}
