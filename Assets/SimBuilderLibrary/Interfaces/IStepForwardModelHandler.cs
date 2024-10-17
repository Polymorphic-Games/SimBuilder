using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimBuilderLibrary
{
    public interface IStepForwardModelHandler
    {
        public void StepForwardModelCallback(object sender, Dictionary<int, State> states);
    }
}
