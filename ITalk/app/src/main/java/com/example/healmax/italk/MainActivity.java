package com.example.healmax.italk;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.ListView;

import com.example.healmax.italk.API.ITalkAPI;

import java.util.ArrayList;
import java.util.List;

public class MainActivity extends AppCompatActivity {

    private ListView friendList;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        this.friendList = (ListView) findViewById(R.id.friend_list);
        FriendListViewAdapter adapter = new FriendListViewAdapter(this, ITalkAPI.getFriends());
        this.friendList.setAdapter(adapter);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();

        if (id == R.id.action_settings) {
            return true;
        } else if (id == R.id.add_friend) {
            Intent intent = new Intent();
            intent.setClass(MainActivity.this, AddFriendActivity.class);
            startActivity(intent);
        }

        return super.onOptionsItemSelected(item);
    }
}
