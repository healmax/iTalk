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
    public static final String  TABLE_NAME_User             = "User";
    public static final String  TABLE_NAME_Friend           = "Friend";
    public static final String  TABLE_NAME_ChatRecord       = "ChatRecord";

    public static final String	TABLE_NAME_SyncMeta			= "SyncMeta";


    /*
     * Field
     */

    public static final String  FIELD_u_sys_id              = "u_sys_id";
    public static final String  FIELD_u_id                  = "u_id";
    public static final String  FIELD_u_name                = "u_name";
    public static final String  isFriend                    = "u_isFriend";

    //Friend
    public static final String  FIELD_f_sys_id              = "f_sys_id";
    public static final String  FIELD_f_id                  = "f_id";
    public static final String  FIELD_f_name                = "f_name";

    //ChatRecord
    public static final String  FIELD_cr_sys_id             = "cr_sys_id";
    public static final String  FIELD_cr_friendID           = "cr_friendID";
    public static final String	FIELD_cr_content      	    = "cr_content";
    public static final String	FIELD_cr_sync_done		    = "cr_sync_done";
    public static final String 	FIELD_cr_sendTime           = "cr_sendTime";

    //SyncMeta info-----------------------------------------------------------// used for sync queue
    public static final String FIELD_SYNC_ID       				= "sync_id";
    public static final String FIELD_SYNC_ACTION 				= "sync_action";
    public static final String FIELD_SYNC_IS_SYNC 				= "sync_is_sync";

    public ITalkDB(Context context) {
        super(context, DATABASE_NAME, null, DATABASE_VERISION);
    }

    @Override
    public void onCreate(SQLiteDatabase db) {

        String sqlUser = "CREATE TABLE " + TABLE_NAME_User + " (" +
                FIELD_u_sys_id + " INTEGER PRIMARY KEY AUTOINCREMENT," +
                FIELD_u_id     + " TEXT NOT NULL,"+
                FIELD_u_name   + " TEXT NOT NULL," +
                isFriend       + " INTEGER NOT NULL" +
                ")";

        String sqlFriend = "CREATE TABLE " + TABLE_NAME_Friend + " (" +
                FIELD_f_sys_id + " INTEGER PRIMARY KEY AUTOINCREMENT," +
                FIELD_f_id     + " TEXT NOT NULL,"+
                FIELD_f_name   + " TEXT NOT NULL" +
                ")";

        String sqlChatRecord = "CREATE TABLE " + TABLE_NAME_ChatRecord + " (" +
                FIELD_cr_sys_id    + " INTEGER PRIMARY KEY AUTOINCREMENT," +
                FIELD_cr_friendID  + " TEXT NOT NULL,"+
                FIELD_cr_content   + " TEXT NOT NULL," +
                FIELD_cr_sync_done + " TEXT NOT NULL," +
                FIELD_cr_sendTime  + " TEXT NOT NULL"+
                ")";

        String sqlSyncMeta	= "CREATE TABLE " + TABLE_NAME_SyncMeta + "(" +
                FIELD_SYNC_ID      + " INTEGER PRIMARY KEY AUTOINCREMENT," +
                FIELD_SYNC_ACTION  +"  INTEGER NOT NULL,"+
                FIELD_SYNC_IS_SYNC + " INTEGER NOT NULL"+
                ")" ;

        db.execSQL(sqlUser);
        db.execSQL(sqlFriend);
        db.execSQL(sqlChatRecord);

        db.execSQL(sqlSyncMeta);
    }

    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {

    }
}
