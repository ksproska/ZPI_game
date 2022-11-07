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
    class CreateAccountValidator: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI loginText;
        [SerializeField] private Text loginValidationText;

        [SerializeField] private TextMeshProUGUI passwordText;
        [SerializeField] private Text passwordValidationText;

        private Regex emailRegex;


        private void Start()
        {
            emailRegex = new(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*
                             @((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
        }

        public void ValidateEmail()
        {
            var email = loginText.text;
            if (!emailRegex.IsMatch(email))
            {
                loginValidationText.text = "Incorrect email format";
            }
            return;
        }
    }
}
