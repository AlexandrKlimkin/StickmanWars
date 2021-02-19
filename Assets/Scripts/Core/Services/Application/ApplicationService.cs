using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Services.Application {
    public class ApplicationService : ILoadableService, IUnloadableService {

        public void Load() {
            Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
        }

        public void Unload() {
            
        }
    }
}