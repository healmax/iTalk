package com.example.healmax.italk;

import android.app.Activity;
import android.content.ContentValues;
import android.content.Intent;
import android.database.Cursor;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTabHost;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;

import com.example.healmax.italk.API.ITalkAPI;
import com.example.healmax.italk.DataBase.ITalkDB;
import com.example.healmax.italk.DataBase.ITalkProvider;
import com.example.healmax.italk.Fragment.MainFriendsFragment;
import com.example.healmax.italk.Fragment.MainTalkFragment;

import java.util.ArrayList;
import java.util.List;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        //獲取TabHost控制元件
        FragmentTabHost mTabHost = (FragmentTabHost) findViewById(android.R.id.tabhost);
        //設定Tab頁面的顯示區域，帶入Context、FragmentManager、Container ID
        mTabHost.setup(this, getSupportFragmentManager(), R.id.container);

        /**
         新增Tab結構說明 :
         首先帶入Tab分頁標籤的Tag資訊並可設定Tab標籤上顯示的文字與圖片，
         再來帶入Tab頁面要顯示連結的Fragment Class，最後可帶入Bundle資訊。
         **/

        //建立一個Tab，這個Tab的Tag設定為one，
        //並設定Tab上顯示的文字為第一堂課與icon圖片，Tab連結切換至
        //LessonOneFragment class，無夾帶Bundle資訊。
        mTabHost.addTab(mTabHost.newTabSpec("one")
                .setIndicator(getResources().getString(R.string.main_friendsfragment_friend),null)
                ,MainFriendsFragment.class,null);

        mTabHost.addTab(mTabHost.newTabSpec("two")
                .setIndicator(getResources().getString(R.string.main_talkfragment_talk), null)
                , MainTalkFragment.class, null);

//        ContentValues values = new ContentValues();
//        values.put(ITalkDB.FIELD_f_id, "zzzzz");
//        values.put(ITalkDB.FIELD_f_name, "fffff");
//
//        getContentResolver().insert(ITalkProvider.uriFriend, values);
//        Cursor cursor = getContentResolver().query(ITalkProvider.uriFriend, null, null, null, null);
//        if (cursor.getCount() > 0) {
//        }
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
