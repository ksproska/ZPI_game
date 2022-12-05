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
using UnityEngine.EventSystems;
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
        private EventSystem system;
        private GameObject currentSelection;

        private void Awake()
        {
            system = EventSystem.current;
        }

        private void Start()
        {
            loginValidationText.text = "";
            passwordValidationText.text = "";
            errorInfoFrame.gameObject.SetActive(false);
            system.SetSelectedGameObject(loginText.gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (currentSelection != loginText.gameObject && currentSelection != passwordText.gameObject)
                {
                    var obj = loginText.gameObject;
                    currentSelection = obj;
                    system.SetSelectedGameObject(obj);
                }
                SwapTextField();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendData();
            }
        }

        private void OnEnable()
        {
            loginValidationText.text = "";
            passwordValidationText.text = "";
            loginText.text = "";
            passwordText.text = "";
            system.SetSelectedGameObject(loginText.gameObject);
            currentSelection = loginText.gameObject;
        }

        private void OnDisable()
        {
            errorInfoFrame.gameObject.SetActive(false);
        }

        private void SwapTextField()
        {
            var obj = currentSelection == loginText.gameObject ? passwordText.gameObject : loginText.gameObject;
            currentSelection = obj;
            system.SetSelectedGameObject(obj);
        }

        public async void SendData()
        {
            var menuActives = new List<IMenuActive>(GetComponentsInChildren<IMenuActive>());
            menuActives.ForEach(m => m.SetEnabled(false));
            var login = loginText.text.Trim();
            var password = passwordText.text.Trim();
            User user = new(login, password);
            cryo.SetBothEyesTypes(Cryo.Script.EyeType.Loading);
            cryo.SetMouthType(Cryo.Script.MouthType.Line);
            var (unityResponse, serverString) = await Auth.AuthenticateUser(user);
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
            errorInfoFrame.IsLoginSuccessful = true;
            errorInfoFrame.gameObject.SetActive(true);
        }

        private void OnConnectionError()
        {
            errorInfoFrame.SetCryoEyeType(Cryo.Script.EyeType.Sad);
            errorInfoFrame.SetCryoMouthType(Cryo.Script.MouthType.Confused);
            errorInfoFrame.SetErrorText("It seems the server is not responding. Please check your internet connection or try again later.");
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

        public class UserResponse
        {
            public string nickname { get; set; }
            public int user_id { get; set; }
        }

    }


}
