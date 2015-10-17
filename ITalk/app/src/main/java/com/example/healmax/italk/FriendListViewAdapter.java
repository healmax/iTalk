package com.example.healmax.italk;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;
import java.util.zip.Inflater;

/**
 * Created by healmax on 15/10/17.
 */
public class FriendListViewAdapter extends BaseAdapter {

    private List<String> friendList = new ArrayList<String>();
    private LayoutInflater layoutInflater;

    public FriendListViewAdapter(Context context, List<String> friendList) {
        // TODO Auto-generated constructor stub
        this.friendList=friendList;
        this.layoutInflater = LayoutInflater.from(context);
    }
    @Override
    public int getCount() {
        // TODO Auto-generated method stub
        return friendList.size();
    }

    @Override
    public Object getItem(int position) {
        // TODO Auto-generated method stub
        return this.friendList.get(position);
    }

    @Override
    public long getItemId(int position) {
        // TODO Auto-generated method stub
        return position;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        // TODO Auto-generated method stub
        Holder holder = new Holder();
        if (convertView == null) {
            convertView = layoutInflater.inflate(R.layout.friend_list_item, parent, false);
            holder.tv = (TextView)convertView.findViewById(R.id.friend_name);
            convertView.setTag(convertView);
        } else {
            holder = (Holder)convertView.getTag();
        }

        holder.tv.setText(this.friendList.get(position));

        return convertView;
    }

    public class Holder
    {
        TextView tv;
    }

}
