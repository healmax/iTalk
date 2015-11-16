package com.example.healmax.italk.Util;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.AsyncTask;

import com.example.healmax.italk.API.ITalkAPI;
import com.example.healmax.italk.Interface.LoginInfoInterface;
import com.example.healmax.italk.Interface.RegisterInfoInterface;
import com.example.healmax.italk.Login.LoginActivity;

import org.json.JSONObject;

import java.util.HashMap;
import java.util.Map;

/**
 * Created by healmax on 15/10/17.
 */
public class ITalkUtil {

    //獲取login preference 字串
    private static final String LOGIN_PREFS = "LoginPrefsFile";

    public static final int NETWORK_TIMEOUT_CONNECTION = 5000;
    public static final int NETWORK_TIMEOUT_SOCKET = NETWORK_TIMEOUT_CONNECTION + 5000;

    public static final String USER_ID = "UserID";
    public static final String USER_PW = "UserPW";
    public static final String HAS_BEEN_LOGIN = "hasBeenLogin";

    private static String token;

    static public String getToken() {
        return token;
    }

    public class LoginAsyncTask extends AsyncTask<String, Void, Void> {

        String result;
        LoginInfoInterface loginInfoInterface;
        Context context;
        boolean isAutoLogin;

        public LoginAsyncTask(Context context, LoginInfoInterface loginInfoInterface, boolean isAutoLogin) {
            this.context = context;
            this.loginInfoInterface = loginInfoInterface;
            this.isAutoLogin = isAutoLogin;
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
        }

        @Override
        protected Void doInBackground(String... params) {

            String id;
            String pw;

            if (this.isAutoLogin) {
                Map<String, String> userInfo = ITalkUtil.getUserInfo(this.context);
                id = userInfo.get(ITalkUtil.USER_ID);
                pw = userInfo.get(ITalkUtil.USER_PW);
            } else {
                id = params[0];
                pw = params[1];
            }

            result = ITalkAPI.signIn(id, pw);
            return null;
        }

        @Override
        protected void onPostExecute(Void aVoid) {
            super.onPostExecute(aVoid);
            Intent intent = new Intent();
            intent.setClass(this.context, LoginActivity.class);

            Boolean isSuccess = false;

            try {
                JSONObject jsonObject = new JSONObject(this.result);
                if (jsonObject.has("access_token")) {
                    isSuccess = true;
                    ITalkUtil.token = jsonObject.getString("access_token");
                }
            } catch (Exception ex) {
                ex.printStackTrace();
            }

            if(result != null && !result.isEmpty()){
                if(isSuccess) {
                    this.loginInfoInterface.loginSuccess("test");
                } else {
                    loginInfoInterface.showLoginError(null);
                }
            } else {
                loginInfoInterface.showLoginError(null);
            }
        }
    }

    public class RegisterAsyncTask extends AsyncTask<String, Void, Void> {

        String result;
        RegisterInfoInterface registerInfoInterface;
        Context context;

        String id;
        String pw;

        public RegisterAsyncTask(Context context, RegisterInfoInterface registerInfoInterface, String id, String pw) {
            this.context = context;
            this.registerInfoInterface = registerInfoInterface;
            this.id = id;
            this.pw = pw;
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
        }

        @Override
        protected Void doInBackground(String... params) {

            result = ITalkAPI.register(this.id, this.pw);
            return null;
        }

        @Override
        protected void onPostExecute(Void aVoid) {
            super.onPostExecute(aVoid);
            Intent intent = new Intent();
            intent.setClass(this.context, LoginActivity.class);

            String success = null;
            String message = null;

            if(result != null && !result.isEmpty()){
                try {
                    JSONObject jsonObject = new JSONObject(result);
                    success = jsonObject.getString("Success");
                    message = jsonObject.getString("Message");
                } catch (Exception ex) {
                    ex.printStackTrace();
                }

                if(success.toLowerCase().equals("true")) {
                    ITalkUtil.saveUserInfo(this.context, id, pw);
                    this.registerInfoInterface.registerSuccess("test");
                } else {
                    registerInfoInterface.showRegisterError(message);
                }
            } else {
                registerInfoInterface.showRegisterError(message);
            }
        }
    }

    private static void saveUserInfo(Context context, String id, String pw) {
        SharedPreferences loginPrefs;
        loginPrefs = context.getSharedPreferences(ITalkUtil.LOGIN_PREFS, 0);
        SharedPreferences.Editor prefsWriter = loginPrefs.edit();
        prefsWriter.putBoolean(ITalkUtil.HAS_BEEN_LOGIN, true);
        prefsWriter.putString(ITalkUtil.USER_ID, id);
        prefsWriter.putString(ITalkUtil.USER_PW, pw);
        prefsWriter.commit();
        prefsWriter.clear();
    }

    private static Map<String, String> getUserInfo(Context context) {
        Map<String, String> userInfo = new HashMap<String, String>();
        SharedPreferences loginPrefs;
        loginPrefs = context.getSharedPreferences(ITalkUtil.LOGIN_PREFS, 0);

        boolean hasBeenLoing = loginPrefs.getBoolean(ITalkUtil.HAS_BEEN_LOGIN, false);
        if(hasBeenLoing) {
            userInfo.put(ITalkUtil.USER_ID, loginPrefs.getString(ITalkUtil.USER_ID, null));
            userInfo.put(ITalkUtil.USER_PW, loginPrefs.getString(ITalkUtil.USER_PW, null));
        }

        return userInfo;
    }

    public class CheckFriendAsyncTask extends AsyncTask<String, Void, Void> {

        String result;
        Context context;

        public CheckFriendAsyncTask(Context context) {
            this.context = context;
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
        }

        @Override
        protected Void doInBackground(String... params) {

            String id;
            id = params[0];

            result = ITalkAPI.checkFriend(id);
            return null;
        }

        @Override
        protected void onPostExecute(Void aVoid) {

        }
    }
}
