using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services {
    public class CharacterSpawnedSignal {
        public CharacterUnit Unit;

        public CharacterSpawnedSignal(CharacterUnit unit) {
            this.Unit = unit;
        }
    }
}
