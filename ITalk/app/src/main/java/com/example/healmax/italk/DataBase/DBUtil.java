package com.example.healmax.italk.DataBase;

import android.content.ContentProvider;
import android.content.ContentUris;
import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.net.Uri;

import com.example.healmax.italk.Model.Friend;
import com.example.healmax.italk.Model.User;

/**
 * Created by healmax on 15/11/5.
 */
public class DBUtil {

    public static long insertSyncTable(Context context, ContentValues cv) {
        Uri uri = context.getContentResolver().insert(ITalkProvider.uriSyncMeta, cv);
        long _Id = ContentUris.parseId(uri);
        return _Id;

    }

    public static Cursor queryFriendById (Context context, int primaryKey) {
        String sql = "SELECT * from " + ITalkDB.TABLE_NAME_Friend + " where " + ITalkDB.FIELD_f_sys_id + "=" + primaryKey;
        Uri url = Uri.parse(ITalkProvider.uriRawQuery + sql);
        Cursor cur = context.getContentResolver().query(url, null, null, null, null);
        return cur;
    }

    public static void deleteFriendById (Context context, String id) {

        String whereCon = ITalkDB.FIELD_f_id + "=" + id;
        context.getContentResolver().delete(ITalkProvider.uriFriend, whereCon, null);
    }

    public static long insertFriend (Context context, Friend friendInfo, boolean isNotify) {

        Uri url = isNotify ? ITalkProvider.uriFriend : ITalkProvider.uriFriend_NoNotify;
        long id = -1;

        ContentValues cv = new ContentValues();

        cv.put(ITalkDB.FIELD_f_id, friendInfo.getId());
        cv.put(ITalkDB.FIELD_f_name, friendInfo.getName());

        Uri uri = context.getContentResolver().insert(url, cv);
        id = ContentUris.parseId(uri);

        return id;
    }

    public static Cursor queryALlFriends(Context context) {

        String sql = "SELECT *, " + ITalkDB.FIELD_f_sys_id + " AS _id  from " + ITalkDB.TABLE_NAME_Friend;
        Uri url = Uri.parse(ITalkProvider.uriRawQuery + sql);
        return context.getContentResolver().query(url, null, null, null, null);
//        return context.getContentResolver().query(ITalkProvider.uriFriend, null, null, null, null);
    }
}
