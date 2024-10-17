using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimBuilderLibrary
{
    public interface IUpdateModelStatesHandler
    {
        public void UpdateModelStatesCallback(object sender, Dictionary<int, State> updatedStates);
    }
}

