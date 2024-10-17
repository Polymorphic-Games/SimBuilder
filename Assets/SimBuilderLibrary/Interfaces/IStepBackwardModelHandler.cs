using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimBuilderLibrary
{
    public interface IStepBackwardModelHandler
    {
        public void StepBackwardModelCallback(object sender, Dictionary<int, State> states);
    }
}
