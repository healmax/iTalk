package com.example.healmax.italk.Util;

import android.content.Context;
import android.os.AsyncTask;
import android.util.Log;

import com.example.healmax.italk.API.ITalkAPI;

import org.json.JSONException;
import org.json.JSONObject;

/**
 * Created by healmax on 15/10/17.
 */
public class ITalkUtil {

    public static final int NETWORK_TIMEOUT_CONNECTION = 5000;
    public static final int NETWORK_TIMEOUT_SOCKET = NETWORK_TIMEOUT_CONNECTION + 5000;

    public class UserLoginAsyncTask extends AsyncTask<String, Void, Void> {

        String result;
        LoginInfoInterface loginInfoInterface;

        public UserLoginAsyncTask(Context context, LoginInfoInterface loginInfoInterface) {
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
            result = ITalkAPI.register(id, pw);
            return null;
        }

        @Override
        protected void onPostExecute(Void aVoid) {
            super.onPostExecute(aVoid);

            if(result != null && !result.isEmpty()){
                if(result.equals("true")) {
                    this.loginInfoInterface.loginSuccess("test");
                } else {
                    this.loginInfoInterface.showLoginError();
                }
            } else {
                this.loginInfoInterface.showLoginError();
            }
        }
    }
}
