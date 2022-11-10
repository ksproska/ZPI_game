using DeveloperUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        [SerializeField] private TMP_InputField loginText;
        [SerializeField] private Text loginValidationText;

        [SerializeField] private TMP_InputField passwordText;
        [SerializeField] private Text passwordValidationText;

        [SerializeField] private ConnectionErrorBox errorInfoFrame;

        [SerializeField] private CryoUI cryo;


        private void Start()
        {
            loginValidationText.text = "";
            passwordValidationText.text = "";
            errorInfoFrame.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            errorInfoFrame.gameObject.SetActive(false);
        }

        public async void SendData()
        {
            var menuActives = new List<IMenuActive>(GetComponentsInChildren<IMenuActive>());
            menuActives.ForEach(m => m.SetEnabled(false));
            var login = loginText.text.Trim();
            var password = passwordText.text.Trim();
            User user = new(login, password);
            cryo.SetBothEyesTypes(Cryo.Script.EyeType.Wink);
            cryo.SetMouthType(Cryo.Script.MouthType.Confused);
            var (unityResponse, serverString) = await Auth.AuthenticateUser(user);
            serverString.Debug();
            switch(unityResponse)
            {
                case UnityEngine.Networking.UnityWebRequest.Result.Success:
                    OnSuccess(serverString);
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
            cryo.SetBothEyesTypes(Cryo.Script.EyeType.Eye);
            cryo.SetMouthType(Cryo.Script.MouthType.Smile);
            menuActives.ForEach(m => m.SetEnabled(true));
        }

        private void OnSuccess(string serverString)
        {
            errorInfoFrame.SetCryoEyeType(Cryo.Script.EyeType.Happy);
            errorInfoFrame.SetCryoMouthType(Cryo.Script.MouthType.Smile);
            var userResponse = JsonSerializer.Deserialize<UserResponse>(serverString);
            errorInfoFrame.SetErrorText("You have been logged in successfully!");
            CurrentState.CurrentGameState.Instance.CurrentUserId = userResponse.user_id;
            CurrentState.CurrentGameState.Instance.CurrentUserNickname = userResponse.nickname;
            errorInfoFrame.gameObject.SetActive(true);
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

        class UserResponse
        {
            public string nickname { get; set; }
            public int user_id { get; set; }
        }

    }


}
