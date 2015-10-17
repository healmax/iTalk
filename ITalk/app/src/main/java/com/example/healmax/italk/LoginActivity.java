package com.example.healmax.italk;

import android.content.Intent;
import android.os.Message;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.os.Handler;

import com.example.healmax.italk.API.ITalkAPI;
import com.example.healmax.italk.R;
import com.example.healmax.italk.Util.ITalkUtil;
import com.example.healmax.italk.Util.LoginInfoInterface;


public class LoginActivity extends AppCompatActivity implements LoginInfoInterface {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        new Thread(new Runnable() {
            @Override
            public void run() {
                handler.sendEmptyMessage(0);
            }
        }).start();
    }

    Handler handler = new Handler() {
        @Override
        public void handleMessage(Message msg) {
            super.handleMessage(msg);
            autoLogin();
        }
    };

    private void autoLogin() {
        ITalkUtil.UserLoginAsyncTask loginAsyncTask = new ITalkUtil().new UserLoginAsyncTask(getApplicationContext(), this);
        String id = "jh193";
        String pw = "1234";
        loginAsyncTask.execute(id, pw);
    }


    @Override
    public void loginSuccess(String test) {
        Intent intent = new Intent();
        intent.setClass(LoginActivity.this, MainActivity.class);
        startActivity(intent);
    }

    @Override
    public void showLoginError() {

    }
}
