using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class GameUI
    {
        public static void UpdateGuiText(string name ,string text) {
            var outputText = GameObject.Find(name);
            if (outputText.GetComponent<Text>() == null)
            {
                outputText.GetComponent<InputField>().text = text;

            }
            else
            {
                outputText.GetComponent<Text>().text = text;
            }
        }
        public static void ActiveButton(string name , bool actived)
        {
            var button = GameObject.Find(name).GetComponent<Button>();
            if (actived)
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }
        }
    }
}
