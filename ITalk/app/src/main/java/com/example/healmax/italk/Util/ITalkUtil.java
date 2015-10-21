package com.example.healmax.italk.Util;

import android.content.Context;
import android.content.Intent;
import android.os.AsyncTask;
import android.util.Log;

import com.example.healmax.italk.API.ITalkAPI;
import com.example.healmax.italk.Login.LoginActivity;

import org.json.JSONException;
import org.json.JSONObject;

/**
 * Created by healmax on 15/10/17.
 */
public class ITalkUtil {

    //獲取login preference 字串
    private static final String LOGIN_PREFS = "LoginPrefsFile";

    public static final int NETWORK_TIMEOUT_CONNECTION = 5000;
    public static final int NETWORK_TIMEOUT_SOCKET = NETWORK_TIMEOUT_CONNECTION + 5000;

    public class UserLoginAsyncTask extends AsyncTask<String, Void, Void> {

        String result;
        LoginInfoInterface loginInfoInterface;
        Context context;

        public UserLoginAsyncTask(Context context, LoginInfoInterface loginInfoInterface) {
            this.context = context;
            this.loginInfoInterface = loginInfoInterface;
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
        }

        @Override
        protected Void doInBackground(String... params) {

            String id = params[0];
            String pw = params[1];
            result = ITalkAPI.signIn(id, pw);
            return null;
        }

        @Override
        protected void onPostExecute(Void aVoid) {
            super.onPostExecute(aVoid);
            Intent intent = new Intent();
            intent.setClass(this.context, LoginActivity.class);

            if(result != null && !result.isEmpty()){
                if(result.equals("true")) {
                    this.loginInfoInterface.loginSuccess("test");
                } else {
                    loginInfoInterface.showLoginError();
                }
            } else {
                loginInfoInterface.showLoginError();
            }
        }
    }

    public class RegisterAsyncTask extends AsyncTask<String, Void, Void> {

        String result;
        LoginInfoInterface loginInfoInterface;
        Context context;

        String id;
        String pw;

        public RegisterAsyncTask(Context context, LoginInfoInterface loginInfoInterface, String id, String pw) {
            this.context = context;
            this.loginInfoInterface = loginInfoInterface;
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

            if(result != null && !result.isEmpty()){
                if(result.equals("true")) {
                    this.loginInfoInterface.loginSuccess("test");
                } else {
                    loginInfoInterface.showLoginError();
                }
            } else {
                loginInfoInterface.showLoginError();
            }
        }
    }
}
