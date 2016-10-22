using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Facebook.Unity;

public class FacebookLogin : MonoBehaviour {
    public UIButton loginButton;
    public UITexture profilePicture;

    public void Awake()
    {
        FB.Init(() =>
        {
            loginButton.SetState(UIButtonColor.State.Normal, true);
        });
    }

    public void OnFacebookLogin()
    {
        var permissions = new string[] {
            "public_profile"
        };

        FB.LogInWithReadPermissions(
            permissions,
            LoginCallback);
    }

    private void LoginCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            var token = AccessToken.CurrentAccessToken;

            string userId = token.UserId;
            string accessToken = token.TokenString;

            Debug.Log("user id : " + userId);
            Debug.Log("token : " + accessToken);

            FB.API("/" + userId + "/picture?type=large", HttpMethod.GET, OnPicture);
        }
        else
        {
            // login error
        }
    }

    private void OnPicture(IGraphResult result)
    {
        if (result.Error == null)
        {
            profilePicture.mainTexture = result.Texture;
        }
    }
}
