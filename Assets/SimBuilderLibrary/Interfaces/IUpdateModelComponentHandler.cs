using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimBuilderLibrary
{
    public interface IUpdateModelComponentHandler
    {
        public void UpdateModelComponentCallback(object sender, ModelComponent value);
    }
}
