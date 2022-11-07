using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu.Account
{
    class LoginValidator: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI loginText;
        [SerializeField] private Text loginValidationText;

        [SerializeField] private TextMeshProUGUI passwordText;
        [SerializeField] private Text passwordValidationText;

        [SerializeField] private GameObject connectionInfoFrame;

        private Regex emailRegex;


        private void Start()
        {
            emailRegex = new(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*
                             @((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
        }

        public void SendData()
        {
            var login = loginText.text;
            var password = passwordText.text;
            // SomeController.SendData(login, password);
            bool hasInternetConnection = false;
            if(!hasInternetConnection)
            {
                connectionInfoFrame.SetActive(true);
            }
        }

    }


}
