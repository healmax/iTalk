package com.example.healmax.italk;

import android.content.Intent;
import android.os.AsyncTask;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.TextView;

import com.example.healmax.italk.API.ITalkAPI;
import com.example.healmax.italk.Interface.IView;
import com.example.healmax.italk.Model.Friend;
import com.example.healmax.italk.Model.ReturnMessage;
import com.example.healmax.italk.Model.User;
import com.example.healmax.italk.Util.FriendUtil;
import com.example.healmax.italk.Util.ITalkUtil;

import java.util.List;

public class AddFriendActivity extends AppCompatActivity implements IView {

    private TextView textViewid;

    private EditText editTextFriendID;
    private ImageButton imgBtnSearch;

    private TextView textSearchFriendMessage;
    private Button btnAddFriend;

    private User userInfo;


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

        this.textSearchFriendMessage = (TextView)findViewById(R.id.text_searchFriend_message);
        this.btnAddFriend = (Button)findViewById(R.id.btn_addFriend);
    }

    private void initListener() {
        this.imgBtnSearch.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String friendID = AddFriendActivity.this.editTextFriendID.getText().toString();
                FriendUtil.SearchFriendAsyncTask task = new FriendUtil(). new SearchFriendAsyncTask(AddFriendActivity.this);
                task.execute(friendID);
            }
        });

        this.btnAddFriend.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Friend friendInfo = new Friend();
                friendInfo.setId(AddFriendActivity.this.userInfo.getId());
                friendInfo.setName(AddFriendActivity.this.userInfo.getName());
                FriendUtil.AddFriendAsyncTask task = new FriendUtil(). new AddFriendAsyncTask(AddFriendActivity.this, friendInfo);
                task.execute();
            }
        });
    }


    @Override
    public void success(ReturnMessage result) {
//        ReturnMessage<User> d = new ReturnMessage();
        userInfo = (User)result.getDate();
        this.textSearchFriendMessage.setText("success");
        this.btnAddFriend.setVisibility(View.VISIBLE);
    }

    @Override
    public void fial(ReturnMessage result) {

    }
}
