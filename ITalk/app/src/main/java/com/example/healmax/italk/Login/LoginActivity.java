package com.example.healmax.italk.Login;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;

import com.example.healmax.italk.MainActivity;
import com.example.healmax.italk.R;
import com.example.healmax.italk.Util.ITalkUtil;
import com.example.healmax.italk.Util.LoginInfoInterface;

public class LoginActivity extends AppCompatActivity implements LoginInfoInterface{

    EditText editId;
    EditText editName;

    Button btnLogin;
    Button btnRegister;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);
        initView();
        initListener();
    }

    private void initView () {
        this.editId = (EditText)findViewById(R.id.edit_id);
        this.editName = (EditText)findViewById(R.id.edit_name);
        this.btnLogin = (Button)findViewById(R.id.btn_login);
        this.btnRegister = (Button)findViewById(R.id.btn_register);
    }

    private void initListener() {

        this.btnLogin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String id = LoginActivity.this.editId.getText().toString();
                String pw = LoginActivity.this.editName.getText().toString();
                ITalkUtil.LoginAsyncTask loginAsyncTask = new ITalkUtil(). new LoginAsyncTask(getApplicationContext(), LoginActivity.this, false);
                loginAsyncTask.execute(id, pw);
            }
        });

        this.btnRegister.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String id = LoginActivity.this.editId.getText().toString();
                String pw = LoginActivity.this.editName.getText().toString();
                ITalkUtil.RegisterAsyncTask registerAsyncTask = new ITalkUtil(). new RegisterAsyncTask(getApplicationContext(), LoginActivity.this, id, pw);
                registerAsyncTask.execute(id, pw);
            }
        });
    }

    @Override
    public void loginSuccess(String test) {
        Intent intent = new Intent();
        intent.setClass(this, MainActivity.class);
        startActivity(intent);
    }

    @Override
    public void showLoginError(String message) {

    }
}
