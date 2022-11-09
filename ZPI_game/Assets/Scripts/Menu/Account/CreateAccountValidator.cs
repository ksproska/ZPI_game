﻿using DeveloperUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Webserver;

namespace Assets.Scripts.Menu.Account
{
    class CreateAccountValidator: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Text nameValidationText;

        [SerializeField] private TextMeshProUGUI loginText;
        [SerializeField] private Text loginValidationText;

        [SerializeField] private TextMeshProUGUI passwordText;
        [SerializeField] private Text passwordValidationText;

        [SerializeField] private TextMeshProUGUI confirmPasswordText;

        [SerializeField] private Text accountCreatedText;

        [SerializeField] private CryoUI cryo;

        private Regex emailRegex;


        private void Start()
        {
            emailRegex = new(@"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+.[A-Z|a-z]{2,}\b");
            nameValidationText.text = "";
            loginValidationText.text = "";
            passwordValidationText.text = "";
            accountCreatedText.text = "";
        }

        public async void SendData()
        {
            bool isNameOk = IsNameLocallyValid();
            bool isEmailOk = IsEmailLoccalyValid();
            bool isPasswordOk = IsPasswordLocallyValid();
            if (!isNameOk || !isEmailOk || !isPasswordOk)
            {
                cryo.SetBothEyesTypes(Cryo.Script.EyeType.EyeBig);
                cryo.SetMouthType(Cryo.Script.MouthType.Confused);
                StartCoroutine(SetCryoToNormal());
                return;
            }
            var menuActives = new List<IMenuActive>(GetComponentsInChildren<IMenuActive>());
            menuActives.ForEach(m => m.SetEnabled(false));

            var name = nameText.text;
            var login = loginText.text;
            var password = passwordText.text;
            User user = new(login, name, password);
            var (unityResponse, serverString) = await Auth.AuthenticateUser(user);
            switch (unityResponse)
            {
                case UnityEngine.Networking.UnityWebRequest.Result.Success:
                    OnSuccess();
                    break;
                case UnityEngine.Networking.UnityWebRequest.Result.ConnectionError:
                    OnConnectionError();
                    break;
                case UnityEngine.Networking.UnityWebRequest.Result.DataProcessingError:
                    OnDataProcessingError();
                    break;
                case UnityEngine.Networking.UnityWebRequest.Result.ProtocolError: // bad username and password
                    OnProtocolError();
                    break;
            }
            menuActives.ForEach(m => m.SetEnabled(true));
        }

        private void OnSuccess()
        {
            accountCreatedText.text = "Hooray! Your account has been successfully created!";
            cryo.SetBothEyesTypes(Cryo.Script.EyeType.Happy);
            cryo.SetMouthType(Cryo.Script.MouthType.Smile);
            StartCoroutine(SetCryoToNormal());
        }

        private void OnConnectionError()
        {
            accountCreatedText.text = "It seems you have no connection. Please try again later.";
            cryo.SetBothEyesTypes(Cryo.Script.EyeType.Sad);
            cryo.SetMouthType(Cryo.Script.MouthType.Confused);
            StartCoroutine(SetCryoToNormal());
        }

        private void OnDataProcessingError()
        {

        }

        private void OnProtocolError()
        {

        }

        public bool IsEmailLoccalyValid()
        {
            var email = loginText.text;
            loginValidationText.text = "";
            if (!emailRegex.IsMatch(email))
            {
                loginValidationText.text = "Incorrect email format.";
                return false;
            }
            return true;
        }

        public bool IsPasswordLocallyValid()
        {
            var password = passwordText.text;
            var confirmPassword = confirmPasswordText.text;
            passwordValidationText.text = "";
            if (password.Length < 12)
            {
                passwordValidationText.text = "Password should have at least 12 characters.";
                return false;
            }
            if(password != confirmPassword)
            {
                passwordValidationText.text = "Passwords do not match.";
                return false;
            }
            return true;
        }

        public bool IsNameLocallyValid()
        {
            var name = nameText.text;
            nameValidationText.text = "";
            if (name.Trim().Length < 4)
            {
                nameValidationText.text = "Name should have at least 3 characters.";
                return false;
            }
            return true;
        }

        IEnumerator SetCryoToNormal()
        {
            yield return new WaitForSeconds(3);
            cryo.SetBothEyesTypes(Cryo.Script.EyeType.Eye);
            cryo.SetMouthType(Cryo.Script.MouthType.Smile);
        }

        
    }
}
