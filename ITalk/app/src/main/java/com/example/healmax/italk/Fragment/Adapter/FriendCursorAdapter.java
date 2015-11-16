package com.example.healmax.italk.Fragment.Adapter;

import android.content.Context;
import android.database.Cursor;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.CursorAdapter;
import android.widget.CursorTreeAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import com.example.healmax.italk.DataBase.ITalkDB;
import com.example.healmax.italk.R;

/**
 * Created by healmax on 15/11/13.
 */
public class FriendCursorAdapter extends CursorAdapter {

    private Context context;

    public FriendCursorAdapter(Context context, Cursor c, boolean autoRequery) {
        super(context, c, autoRequery);
    }


    @Override
    public View newView(Context context, Cursor cursor, ViewGroup parent) {
        LayoutInflater inflater = LayoutInflater.from(context);
        return inflater.inflate(R.layout.friend_list_item, parent, false);
    }

    @Override
    public void bindView(View view, Context context, Cursor cursor) {
        final ImageView image = (ImageView) view.findViewById(R.id.pictureImage);
        final TextView text = (TextView) view.findViewById(R.id.friend_name);

        String id = cursor.getString(cursor.getColumnIndex(ITalkDB.FIELD_f_id));
        text.setText(id);
    }
}
