using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using TMPro;
using Cysharp.Threading.Tasks;

public class FB001_FirebaseAuthEmail_Password : MonoBehaviour
{
    // Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    // Login Variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;

    // Registration Variables
    [Header("Registration")]
    public TMP_InputField nameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField confirmPasswordRegisterField;

    /// <summary>
    /// Kiểm tra và khởi tạo Firebase khi Awake.
    /// </summary>
    private void Awake()
    {
        CheckFirebaseDependencies().Forget();
    }

    /// <summary>
    /// Kiểm tra các dependencies của Firebase.
    /// </summary>
    private async UniTaskVoid CheckFirebaseDependencies()
    {
        dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {
            InitializeFirebase();
        }
        else
        {
            Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
        }
    }

    /// <summary>
    /// Khởi tạo Firebase Auth.
    /// </summary>
    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    /// <summary>
    /// Lắng nghe sự thay đổi trạng thái đăng nhập.
    /// </summary>
    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    /// <summary>
    /// Gọi đăng nhập.
    /// </summary>
    public void Login()
    {
        LoginAsync(emailLoginField.text, passwordLoginField.text).Forget();
    }

    /// <summary>
    /// Đăng nhập với Firebase Authentication.
    /// </summary>
    private async UniTask LoginAsync(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        await UniTask.WaitUntil(() => loginTask.IsCompleted);
        if (loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);
        }
        else
        {
            user = loginTask.Result.User;
            Debug.LogFormat("{0} You Are Successfully Logged In", user.DisplayName);
        }
        AutoLogin();
    }

    /// <summary>
    /// Kiểm tra xác thực email tự động sau khi đăng nhập.
    /// </summary>
    private void AutoLogin()
    {
        if (user.IsEmailVerified)
        {
            Debug.Log("Email is verified");
        }
        else
        {
            Debug.Log("Email is not verified");
            SendEmailForVerification().Forget();
        }
    }

    /// <summary>
    /// Gửi email xác thực tài khoản.
    /// </summary>
    private async UniTask SendEmailForVerification()
    {
        var sendEmailTask = user.SendEmailVerificationAsync();
        await UniTask.WaitUntil(() => sendEmailTask.IsCompleted);
        if (sendEmailTask.Exception != null)
        {
            Debug.LogError(sendEmailTask.Exception);
        }
        else
        {
            Debug.Log("Email Verification Sent");
        }
    }

    /// <summary>
    /// Gọi đăng ký tài khoản.
    /// </summary>
    public void Register()
    {
        RegisterAsync(nameRegisterField.text, emailRegisterField.text, passwordRegisterField.text, confirmPasswordRegisterField.text).Forget();
    }

    /// <summary>
    /// Đăng ký tài khoản với Firebase Authentication.
    /// </summary>
    private async UniTask RegisterAsync(string name, string email, string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            Debug.LogError("Password does not match");
            return;
        }

        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        await UniTask.WaitUntil(() => registerTask.IsCompleted);
        if (registerTask.Exception != null)
        {
            Debug.LogError(registerTask.Exception);
        }
        else
        {
            user = registerTask.Result.User;
            UserProfile userProfile = new UserProfile { DisplayName = name };
            var updateProfileTask = user.UpdateUserProfileAsync(userProfile);
            await UniTask.WaitUntil(() => updateProfileTask.IsCompleted);
            if (updateProfileTask.Exception != null)
            {
                await user.DeleteAsync();
                Debug.LogError(updateProfileTask.Exception);
            }
            else
            {
                Debug.Log("Registration Successful! Welcome " + user.DisplayName);
            }
        }
    }
}
