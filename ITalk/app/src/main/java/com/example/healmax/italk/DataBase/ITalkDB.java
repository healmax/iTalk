package com.example.healmax.italk.DataBase;

import android.content.Context;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;

import com.example.healmax.italk.Util.ITalkUtil;

/**
 * Created by healmax on 15/10/22.
 */
public class ITalkDB extends SQLiteOpenHelper{

    public static final int DATABASE_VERISION                = 1;
    public static final String DATABASE_NAME                 = "iTalkDB";

    /*
     * Table
     */
    public static final String  TABLE_NAME_Friend           = "Friend";
    public static final String  TABLE_NAME_ChatRecord       = "ChatRecord";


    /*
     * Field
     */
    //Friend
    public static final String  FIELD_f_id                  = "f_id";
    public static final String  FIELD_f_name                = "f_name";

    //ChatRecord
    public static final String  FIELD_cr_friendID           = "cr_friendID";
    public static final String	FIELD_cr_content      	    = "cr_content";
    public static final String	FIELD_cr_sync_done		    = "cr_sync_done";
    public static final String 	FIELD_cr_sendTime           = "cr_sendTime";

    public ITalkDB(Context context) {
        super(context, DATABASE_NAME, null, DATABASE_VERISION);
    }

    @Override
    public void onCreate(SQLiteDatabase db) {

        String sqlFriend = "CREATE TABLE " + TABLE_NAME_Friend + " (" +
                FIELD_f_id + " TEXT PRIMARY KEY,"+
                FIELD_f_name + " TEXT NOT NULL" +
                ")";

        String sqlChatRecord = "CREATE TABLE " + TABLE_NAME_ChatRecord + " (" +
                FIELD_cr_friendID + " TEXT PRIMARY KEY,"+
                FIELD_cr_content + " TEXT NOT NULL," +
                FIELD_cr_sync_done + " TEXT NOT NULL," +
                FIELD_cr_sendTime + " TEXT NOT NULL"+
                ")";

        db.execSQL(sqlFriend);
        db.execSQL(sqlChatRecord);
    }

    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {

    }
}
