package com.example.healmax.italk.Login;

import android.content.Intent;
import android.os.Message;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.os.Handler;

import com.example.healmax.italk.MainActivity;
import com.example.healmax.italk.R;
import com.example.healmax.italk.Service.LoginService;
import com.example.healmax.italk.Interface.LoginInfoInterface;
import com.example.healmax.italk.sync.SyncService;


public class AutoLoginActivity extends AppCompatActivity implements LoginInfoInterface {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_autologin);

        //start SYNC service
        Intent srevice_intent = new Intent(this.getApplicationContext(), SyncService.class);
        startService(srevice_intent);

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
//        ITalkUtil.LoginAsyncTask loginAsyncTask = new ITalkUtil().new LoginAsyncTask(getApplicationContext(), this, true);
//        loginAsyncTask.execute();
        LoginService.AutoLoginAsyncTask autoLoginAsyncTask = LoginService.getInstance(). new AutoLoginAsyncTask(getApplicationContext(), this);
        autoLoginAsyncTask.execute();
    }


    @Override
    public void loginSuccess(String test) {
        Intent intent = new Intent();
        intent.setClass(AutoLoginActivity.this, MainActivity.class);
        startActivity(intent);
    }

    @Override
    public void showLoginError(String Message) {
        Intent intent = new Intent();
        intent.setClass(AutoLoginActivity.this, LoginActivity.class);
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        startActivity(intent);
    }
}
