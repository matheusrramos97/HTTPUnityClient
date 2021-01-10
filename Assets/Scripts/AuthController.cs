using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using RestClient.Core;
using RestClient.Core.Models;
using System;

public class AuthController : MonoBehaviour
{

    public InputField InputBaseURL;

    [SerializeField]
    private string Token;

    [SerializeField]
    private string ID;

    [SerializeField]
    private string Name;

    [SerializeField]
    private string Email;

    [Header("FormControl")]

    public GameObject RegisterMenu;
    public GameObject LoginMenu;
    public GameObject ForgotPasswordMenu;
    public GameObject ResetPasswordMenu;

    [SerializeField]
    private string baseUrl = "localhost:3000";

    [Header("CREATE ACCOUNT")]
    public InputField InputEmail;

    public InputField InputName;

    public InputField InputPassword;

    public Button Create;
    public Text InputResponse;

    [Header("LOGIN")]
    public InputField InputLoginEmail;
    public InputField InputLoginPassword;
    public Text InputLoginResponse;

    [Header("FORGOT PASSWORD")]
    public InputField InputForgotPassEmail;
    public Text InputForgotPassResponse;

    [Header("RESET PASSWORD")]
    public InputField InputResetPassEmail;
    public InputField InputResetPassNewPass;
    public InputField InputResetPassToken;
    public Text InputResetPassResponse;

    private void Start() {
        DontDestroyOnLoad(this.transform);
        
    }

    private void Update() {
        if(InputBaseURL.text != ""){
            baseUrl = InputBaseURL.text;
        }else{
            baseUrl = "localhost:3000";
        }
    }

    public void CreateAccount(){

        string Email = InputEmail.text;
        string Name = InputName.text;
        string Password = InputPassword.text;

        if (Email == "" || Name == "" || Password == ""){
            InputResponse.text = "FILL ALL THE FIELDS";
            InputResponse.color = Color.red;
            return;
        }

        //Debug.Log("Email: " + Email + " Name: " + Name + " Password: " + Password);
        InputResponse.text = "WAIT...";
        InputResponse.color = Color.yellow;
        Create.enabled = false;

        RegisterAccount(Email, Name, Password);
    }

    void RegisterAccount(string Email, string Name, string Password){

        RequestHeader header = new RequestHeader {
            Key = "Content-Type",
            Value = "application/json"
        };

        StartCoroutine(RestWebClient.Instance.HttpPost(
            baseUrl + "/auth/register",  
            JsonUtility.ToJson(new Account { email = Email, name = Name, password = Password}), 
            (r) => OnRequestComplete(r), new List<RequestHeader> { header } 
        ));
    }

    private void OnRequestComplete(Response response)
    {
        Debug.Log($"Status Code: {response.StatusCode}");
        //Debug.Log($"Data: {response.Data}");

        var ServerResp = JsonConvert.DeserializeObject<ServerResponse>(response.Data);

        if(ServerResp.error == null){
            InputResponse.text = ServerResp.user.name + " it was registered!";
            InputResponse.color = Color.green;
            Token = ServerResp.token;
            Name = ServerResp.user.name;
            Email = ServerResp.user.email;
            ID = ServerResp.user._id;

        }else {
            InputResponse.text = ServerResp.error;
            InputResponse.color = Color.red;
        }

    }

    public void Login(){

        string Email = InputLoginEmail.text;
        string Password = InputLoginPassword.text;

        if (Email == "" || Password == ""){
            InputLoginResponse.text = "FILL ALL THE FIELDS";
            InputLoginResponse.color = Color.red;
            return;
        }

        //Debug.Log("Email: " + Email + " Name: " + Name + " Password: " + Password);
        InputLoginResponse.text = "WAIT...";
        InputLoginResponse.color = Color.yellow;
        Create.enabled = false;

        Authenticate(Email, Password);
    }

    private void Authenticate(string Email, string Password)
    {
        RequestHeader header = new RequestHeader {
            Key = "Content-Type",
            Value = "application/json"
        };

        StartCoroutine(RestWebClient.Instance.HttpPost(
            baseUrl + "/auth/authenticate",  
            JsonUtility.ToJson(new Account { email = Email, password = Password}), 
            (r) => OnLoginRequestComplete(r), new List<RequestHeader> { header } 
        ));
    }

    private void OnLoginRequestComplete(Response response)
    {
        Debug.Log($"Status Code: {response.StatusCode}");
        Debug.Log($"Data: {response.Data}");

        var ServerResp = JsonConvert.DeserializeObject<ServerResponse>(response.Data);

        if(ServerResp.error == null){
            InputLoginResponse.text = "logged in as " + ServerResp.user.name;
            InputLoginResponse.color = Color.green;
            Token = ServerResp.token;
            Name = ServerResp.user.name;
            Email = ServerResp.user.email;
            ID = ServerResp.user._id;

        }else {
            InputLoginResponse.text = ServerResp.error;
            InputLoginResponse.color = Color.red;
        }
    }


    public void ForgotPassword(){

        string Email = InputForgotPassEmail.text;

        if (Email == ""){
            InputForgotPassResponse.text = "FILL ALL THE FIELDS";
            InputForgotPassResponse.color = Color.red;
            return;
        }

        Debug.Log("Email: " + Email);

        InputForgotPassResponse.text = "WAIT...";
        InputForgotPassResponse.color = Color.yellow;
        Create.enabled = false;

        RequestHeader header = new RequestHeader {
            Key = "Content-Type",
            Value = "application/json"
        };

        StartCoroutine(RestWebClient.Instance.HttpPost(
            baseUrl + "/auth/forgot_password",  
            JsonUtility.ToJson(new Account { email = Email}), 
            (r) => OnForgotPasswordRequestComplete(r), new List<RequestHeader> { header } 
        ));
    }

    private void OnForgotPasswordRequestComplete(Response response)
    {
        Debug.Log($"Status Code: {response.StatusCode}");
        Debug.Log($"Data: {response.Data}");

        var ServerResp = JsonConvert.DeserializeObject<ServerResponse>(response.Data);

        if(ServerResp.error == null){
            InputResetPassResponse.text =  ServerResp.msg;
            InputResetPassResponse.color = Color.green;

            RegisterMenu.SetActive(false);
            LoginMenu.SetActive(false);
            ForgotPasswordMenu.SetActive(false);
            ResetPasswordMenu.SetActive(true);

        }else {
            InputForgotPassResponse.text = ServerResp.error;
            InputForgotPassResponse.color = Color.red;
        }
    }

public void ResetPassword(){

        string Email = InputResetPassEmail.text;
        string Token = InputResetPassToken.text;
        string Password = InputResetPassNewPass.text;

        if (Email == "" || Password == "" || Token == ""){
            InputForgotPassResponse.text = "FILL ALL THE FIELDS";
            InputForgotPassResponse.color = Color.red;
            return;
        }

        InputForgotPassResponse.text = "WAIT...";
        InputForgotPassResponse.color = Color.yellow;
        Create.enabled = false;

        RequestHeader header = new RequestHeader {
            Key = "Content-Type",
            Value = "application/json"
        };

        StartCoroutine(RestWebClient.Instance.HttpPost(
            baseUrl + "/auth/reset_password",  
            JsonUtility.ToJson(new Account { email = Email, token = Token, password = Password}), 
            (r) => OnResetPasswordRequestComplete(r), new List<RequestHeader> { header } 
        ));
    }

    private void OnResetPasswordRequestComplete(Response response)
    {
        Debug.Log($"Status Code: {response.StatusCode}");
        Debug.Log($"Data: {response.Data}");

        var ServerResp = JsonConvert.DeserializeObject<ServerResponse>(response.Data);

        if(ServerResp.error == null){
            InputResetPassResponse.text =  ServerResp.msg;
            InputResetPassResponse.color = Color.green;

            RegisterMenu.SetActive(false);
            LoginMenu.SetActive(false);
            ForgotPasswordMenu.SetActive(false);
            ResetPasswordMenu.SetActive(true);

        }else {
            InputForgotPassResponse.text = ServerResp.error;
            InputForgotPassResponse.color = Color.red;
        }
    }

    public void GotoLogin(){
        LoginMenu.SetActive(true);
        RegisterMenu.SetActive(false);
        ForgotPasswordMenu.SetActive(false);
        ResetPasswordMenu.SetActive(false);
    }

    public void GotoRegister(){
        RegisterMenu.SetActive(true);
        LoginMenu.SetActive(false);
        ForgotPasswordMenu.SetActive(false);
        ResetPasswordMenu.SetActive(false);
    }

    public void GotoForgotPassword(){
        ForgotPasswordMenu.SetActive(true);
        RegisterMenu.SetActive(false);
        LoginMenu.SetActive(false);
        ResetPasswordMenu.SetActive(false);
    }

    public class Account {
        public string email;
        public string token;
        public string name;
        public string password;
    }

    public class User    {
        public string _id { get; set; } 
        public string name { get; set; } 
        public string email { get; set; } 
        public DateTime createAt { get; set; } 
        public int __v { get; set; } 
    }

    public class ServerResponse   {
        public User user { get; set; } 
        public string token { get; set; } 
        public string error { get; set; } 
        public string msg { get; set; }
    }
}
