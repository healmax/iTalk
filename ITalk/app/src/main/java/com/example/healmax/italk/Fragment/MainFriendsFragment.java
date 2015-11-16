package com.example.healmax.italk.Fragment;

import android.app.Activity;
import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ListView;
import android.widget.TextView;

import com.example.healmax.italk.DataBase.DBUtil;
import com.example.healmax.italk.DataBase.ITalkDB;
import com.example.healmax.italk.DataBase.ITalkProvider;
import com.example.healmax.italk.Fragment.Adapter.FriendCursorAdapter;
import com.example.healmax.italk.R;

/**
 * Created by healmax on 15/10/19.
 */
public class MainFriendsFragment extends Fragment {
    //顯示文字內容
    private String text = "test";

    private ListView friendListView = null;


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        //導入Tab分頁的Fragment Layout
        View view = inflater.inflate(R.layout.layout_main_fragment, container, false);
        this.friendListView = (ListView)view.findViewById(R.id.friendListView);
        return inflater.inflate(R.layout.layout_main_fragment, container, false);


    }

    @Override
    public void onResume() {
        super.onResume();
//        SyncManager.addSyncAction(getActivity().getApplicationContext(), "-1", SyncParameter.SYNC_ACTION_GET_FRIEND_LIST);
    }

    @Override
    public void onAttach(Activity activity)
    {
        super.onAttach(activity);
    }

    @Override
    public void onActivityCreated(Bundle savedInstanceState)
    {
        super.onActivityCreated(savedInstanceState);

        try {
            ContentValues test = new ContentValues();
            test.put(ITalkDB.FIELD_f_id, "zzzzz");
            test.put(ITalkDB.FIELD_f_name, "fffff");
            this.getActivity().getContentResolver().insert(ITalkProvider.uriFriend, test);

            test = new ContentValues();
            test.put(ITalkDB.FIELD_f_id, "iiiii");
            test.put(ITalkDB.FIELD_f_name, "iiiii");
            this.getActivity().getContentResolver().insert(ITalkProvider.uriFriend, test);
        } catch (Exception ex) {
            ex.printStackTrace();
        }

        Cursor cursor = DBUtil.queryALlFriends(this.getActivity().getApplicationContext());
//        FriendCursorAdapter adapter = new FriendCursorAdapter(this.getActivity().getApplicationContext(), cursor, true);
        testAdapter adapter = new testAdapter(this.getActivity());
        this.friendListView.setAdapter(adapter);
    }

    private class testAdapter extends BaseAdapter {

        Context context;

        public testAdapter(Context context) {
            this.context = context;
        }

        @Override
        public int getCount() {
            return 1;
        }

        @Override
        public Object getItem(int position) {
            return null;
        }

        @Override
        public long getItemId(int position) {
            return 0;
        }

        @Override
        public View getView(int position, View convertView, ViewGroup parent) {
            View view  = LayoutInflater.from(this.context).inflate(R.layout.layout_main_fragment, parent, false);
            TextView textView = (TextView)view.findViewById(R.id.friend_name);
            textView.setText("test");

            return view;
        }
    }
}
