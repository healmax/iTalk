package com.example.healmax.italk.Service;

import android.content.Context;
import android.os.AsyncTask;

import com.example.healmax.italk.API.UserApi;
import com.example.healmax.italk.Model.ReturnMessage;
import com.example.healmax.italk.Model.User;
import com.example.healmax.italk.Interface.LoginInfoInterface;
import com.example.healmax.italk.Interface.RegisterInfoInterface;

/**
 * Created by healmax on 15/10/27.
 */
public class LoginService {

    private static LoginService loginService;

    private User loginUser;

    private LoginService() {
        super();
    }

    public User getLoginUser() {
        return loginUser;
    }

    public static LoginService getInstance() {
        if (loginService == null) {
            loginService = new LoginService();
        }

        return loginService;
    }

    public class LoginAsyncTask extends AsyncTask<String, Void, Void> {

        ReturnMessage<User> result;
        LoginInfoInterface loginInfoInterface;


        public LoginAsyncTask(LoginInfoInterface loginInfoInterface) {

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
            UserApi api = new UserApi();
            result = api.login(id, pw);

            return null;
        }

        @Override
        protected void onPostExecute(Void aVoid) {
            super.onPostExecute(aVoid);

            if(result.getStatus() == 0) {
                this.loginInfoInterface.loginSuccess("true");
                LoginService.loginService.loginUser = result.getDate();
            } else {
                this.loginInfoInterface.showLoginError(result.getMessage());
            }
        }
    }

    public class AutoLoginAsyncTask extends AsyncTask<String, Void, Void> {

        ReturnMessage<User> result;
        LoginInfoInterface loginInfoInterface;
        Context context;

        public AutoLoginAsyncTask(Context context, LoginInfoInterface loginInfoInterface) {
            this.loginInfoInterface = loginInfoInterface;
            this.context = context;
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
        }

        @Override
        protected Void doInBackground(String... params) {
            UserApi api = new UserApi();
            result = api.autoLogin(this.context);
            return null;
        }

        @Override
        protected void onPostExecute(Void aVoid) {
            super.onPostExecute(aVoid);

            if(result.getStatus() == 0) {
                this.loginInfoInterface.loginSuccess("true");
                LoginService.loginService.loginUser = result.getDate();
            } else {
                this.loginInfoInterface.showLoginError(result.getMessage());
            }
        }
    }

    public class RegisterAsyncTask extends AsyncTask<String, Void, Void> {

        ReturnMessage result;
        RegisterInfoInterface registerInfoInterface;
        Context context;

        public RegisterAsyncTask(Context context, RegisterInfoInterface registerInfoInterface) {
            this.context = context;
            this.registerInfoInterface = registerInfoInterface;

        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
        }

        @Override
        protected Void doInBackground(String... params) {

            String id = params[0];
            String pw = params[1];
            UserApi api = new UserApi();
            result = api.register(this.context, id, pw);
            return null;
        }

        @Override
        protected void onPostExecute(Void aVoid) {
            super.onPostExecute(aVoid);

            if(result.getStatus() == 0) {
                this.registerInfoInterface.registerSuccess("success");
            } else {
                this.registerInfoInterface.showRegisterError(result.getMessage());
            }
        }
    }
}
