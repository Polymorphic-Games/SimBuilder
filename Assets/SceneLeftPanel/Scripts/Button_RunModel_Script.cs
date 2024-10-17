using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SceneLeftPanel
{
    public class Button_RunModel_Script : MediatorComponent
    {
        private const int STOPPED = 0;
        private const int STARTED = 1;
        private int state = STOPPED;

        Model Model;

        void Start()
        {
            GetComponent<Button>().onClick.AddListener(onClickRunModel);
        }

        public void InitializeComponent(MediatorComponent mediator, Model model)
        {
            base.InitializeComponent(mediator);
            Model = model;
        }

        void onClickRunModel()
        {
            switch (state)
            {
                case STOPPED:
                    StartCoroutine("RunModel");
                    state = STARTED;
                    break;
                case STARTED:
                    StopCoroutine("RunModel");
                    state = STOPPED;
                    break;
            }
        }

        IEnumerator RunModel()
        {
            WaitForSeconds delay = new WaitForSeconds(1);

            while (true)
            {
                Model.run_model_tau_leap(0.1f, 1);
                yield return delay;
            }

        }
    }
}
