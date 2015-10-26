package com.example.healmax.italk;

import android.content.Intent;
import android.os.AsyncTask;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.TextView;

import com.example.healmax.italk.API.ITalkAPI;
import com.example.healmax.italk.Util.ITalkUtil;

public class AddFriendActivity extends AppCompatActivity {

    private TextView textViewid;
    private EditText editTextFriendID;
    private ImageButton imgBtnSearch;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_add_friend);
        initView();
        initListener();
    }

    private void initView() {
        this.textViewid = (TextView)findViewById(R.id.textView_id);
        this.editTextFriendID = (EditText)findViewById(R.id.editText_friendID);
        this.imgBtnSearch = (ImageButton)findViewById(R.id.imgBtn_search);
    }

    private void initListener() {
        this.imgBtnSearch.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String friendID = AddFriendActivity.this.editTextFriendID.getText().toString();
                ITalkUtil.CheckFriendAsyncTask task = new ITalkUtil(). new CheckFriendAsyncTask(getApplicationContext());
                task.execute(friendID);
//                String zz = ITalkUtil.ch(friendID);
            }
        });
    }
}
