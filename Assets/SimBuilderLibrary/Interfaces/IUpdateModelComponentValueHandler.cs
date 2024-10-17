using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimBuilderLibrary
{
    public interface IUpdateModelComponentValueHandler
    {
        public void UpdateModelComponentValueCallback(object sender, float value);
    }
}
