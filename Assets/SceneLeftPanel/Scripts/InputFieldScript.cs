//using System;
//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//namespace SceneLeftPanel
//{
//    public class InputFieldScript : MediatorComponent
//    {
//        public string prevValue = "";
//        public string value = "";

//        public void Awake()
//        {
//            GetComponent<TMP_InputField>().onTextSelection.AddListener(delegate { onTextSelection(); });
//            GetComponent<TMP_InputField>().onEndEdit.AddListener(delegate { onEndEdit(); });
//        }

//        public void onTextSelection()
//        {
//            prevValue = GetComponent<TMP_InputField>().text;
//        }
//        public virtual void onEndEdit()
//        {
//            setValue(GetComponent<TMP_InputField>().text);
//            mediator.Notify(this, "onEndEdit");
//        }

//        //model listener function
//        public void setValue(string value)
//        {
//            this.value = value;
//            GetComponent<TMP_InputField>().text = value;
//        }

//    }
//}
