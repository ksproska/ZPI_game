using System;
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
    class LoginValidator: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI loginText;
        [SerializeField] private Text loginValidationText;

        [SerializeField] private TextMeshProUGUI passwordText;
        [SerializeField] private Text passwordValidationText;

        [SerializeField] private ConnectionErrorBox errorInfoFrame;


        private void Start()
        {
            loginValidationText.text = "";
            passwordValidationText.text = "";
        }

        public async void SendData()
        {
            var login = loginText.text;
            var password = passwordText.text;
            User user = new(login, password);
            var (unityResponse, serverString) = await Auth.AuthenticateUser(user);
            switch(unityResponse)
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
        }

        private void OnSuccess()
        {
            errorInfoFrame.SetCryoEyeType(Cryo.Script.EyeType.Happy);
            errorInfoFrame.SetCryoMouthType(Cryo.Script.MouthType.Smile);
            errorInfoFrame.SetErrorText("You have been logged in successfully!");

        }

        private void OnConnectionError()
        {
            errorInfoFrame.SetCryoEyeType(Cryo.Script.EyeType.Sad);
            errorInfoFrame.SetCryoMouthType(Cryo.Script.MouthType.Confused);
            errorInfoFrame.SetErrorText("No nternet connection. Please check your internet connection or try again later.");
            errorInfoFrame.gameObject.SetActive(true);
        }

        private void OnProtocolError()
        {
            loginValidationText.text = "Username or password is not correct.";
            passwordValidationText.text = "Username or password is not correct.";
        }

        private void OnDataProcessingError()
        {

        }

    }


}
