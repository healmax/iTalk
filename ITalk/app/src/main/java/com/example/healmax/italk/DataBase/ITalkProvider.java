package com.example.healmax.italk.DataBase;

import android.content.ContentProvider;
import android.content.ContentUris;
import android.content.ContentValues;
import android.content.UriMatcher;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;
import android.database.sqlite.SQLiteQueryBuilder;
import android.net.Uri;

/**
 * Created by healmax on 15/10/22.
 */
public class ITalkProvider extends ContentProvider{

    public static final String AUTHORITY = "com.example.healmax.italk";

    private static final int TYPE_RawQuery = 0;
    private static final int TYPE_Friend = 1;
    private static final int TYPE_ChatRecord = 2;
    private static final int TYPE_SyncMeta = 3;

    // no notifyChange
    private static final String NoNotify = "_NoNotify";
    private static final int TYPE_Friend_NoNotify = 101;
    private static final int TYPE_ChatRecord_NoNotify = 102;
    private static final int TYPE_SyncMeta_NoNotify = 103;


    public static Uri uriFriend_NoNotify = Uri.parse("content://" + AUTHORITY + "/" + ITalkDB.TABLE_NAME_Friend + NoNotify);

    public static Uri uriChatRecord_NoNotify = Uri.parse("content://" + AUTHORITY + "/" + ITalkDB.TABLE_NAME_ChatRecord + NoNotify);

    public static Uri uriSyncMeta_NoNotify = Uri.parse("content://" + AUTHORITY + "/" + ITalkDB.TABLE_NAME_SyncMeta + NoNotify);

    // notifyChange

    public static String uriRawQuery = "content://" + AUTHORITY + "/rowQuery/";

    public static Uri uriFriend = Uri.parse("content://" + AUTHORITY + "/" + ITalkDB.TABLE_NAME_Friend);

    public static Uri uriChatRecord = Uri.parse("content://" + AUTHORITY + "/" + ITalkDB.TABLE_NAME_ChatRecord);

    public static Uri uriSyncMeta = Uri.parse("content://" + AUTHORITY + "/" + ITalkDB.TABLE_NAME_SyncMeta);

    /*
     * end 20150225
     */
    // parameters
    private SQLiteOpenHelper m_OpenHelper;
    private static final UriMatcher m_urlMatcher = new UriMatcher(UriMatcher.NO_MATCH);

    static {
        m_urlMatcher.addURI(AUTHORITY, "rowQuery/*", TYPE_RawQuery);
        m_urlMatcher.addURI(AUTHORITY, ITalkDB.TABLE_NAME_Friend, TYPE_Friend);
        m_urlMatcher.addURI(AUTHORITY, ITalkDB.TABLE_NAME_ChatRecord, TYPE_ChatRecord);
        m_urlMatcher.addURI(AUTHORITY, ITalkDB.TABLE_NAME_SyncMeta, TYPE_SyncMeta);

        // no notifyChange
        m_urlMatcher.addURI(AUTHORITY, ITalkDB.TABLE_NAME_Friend + NoNotify, TYPE_Friend_NoNotify);
        m_urlMatcher.addURI(AUTHORITY, ITalkDB.TABLE_NAME_ChatRecord + NoNotify, TYPE_ChatRecord_NoNotify);
        m_urlMatcher.addURI(AUTHORITY, ITalkDB.TABLE_NAME_SyncMeta + NoNotify, TYPE_SyncMeta_NoNotify);

    }

    @Override
    public boolean onCreate() {
        m_OpenHelper = new ITalkDB(this.getContext());
        return true;
    }

    @Override
    public Cursor query(Uri uri, String[] projection, String selection, String[] selectionArgs, String sortOrder) {
        Cursor cursor = null;
        SQLiteQueryBuilder qb = new SQLiteQueryBuilder();
        SQLiteDatabase db = null;
        int match = m_urlMatcher.match(uri);

        switch (match) {
            case TYPE_RawQuery:
                db = m_OpenHelper.getReadableDatabase();
                String sql = uri.getLastPathSegment();
                cursor = db.rawQuery(sql,selectionArgs);
                break;
            case TYPE_Friend:
                qb.setTables(ITalkDB.TABLE_NAME_Friend);
                db = m_OpenHelper.getReadableDatabase();
                cursor = qb.query(db, projection, selection, selectionArgs, null, null, sortOrder);
                break;

            case  TYPE_ChatRecord:
                qb.setTables(ITalkDB.TABLE_NAME_ChatRecord);
                db = m_OpenHelper.getReadableDatabase();
                cursor = qb.query(db, projection, selection, selectionArgs, null, null, sortOrder);
                break;

            case  TYPE_SyncMeta:
                qb.setTables(ITalkDB.TABLE_NAME_SyncMeta);
                db = m_OpenHelper.getReadableDatabase();
                cursor = qb.query(db, projection, selection, selectionArgs, null, null, sortOrder);
                break;
            default:
                throw new IllegalArgumentException("Unknown URI " + uri);
        }

        cursor.setNotificationUri(getContext().getContentResolver(), uri);
        return cursor;
    }

    @Override
    public String getType(Uri uri) {
        return null;
    }

    @Override
    public Uri insert(Uri uri, ContentValues values) {
        long row = 0;
        SQLiteDatabase db = null;

        int match = m_urlMatcher.match(uri);

        if (match == TYPE_Friend || match == TYPE_Friend_NoNotify) {
            db = m_OpenHelper.getWritableDatabase();
            db.insert(ITalkDB.TABLE_NAME_Friend, null, values);
        } else  if (match == TYPE_ChatRecord || match == TYPE_ChatRecord_NoNotify) {
            db = m_OpenHelper.getWritableDatabase();
            db.insert(ITalkDB.TABLE_NAME_ChatRecord, null, values);
        } else if (match == TYPE_SyncMeta || match == TYPE_SyncMeta_NoNotify) {
            db = m_OpenHelper.getWritableDatabase();
            db.insert(ITalkDB.TABLE_NAME_SyncMeta, null, values);
        } else {
            throw new IllegalArgumentException("Unknown URI " + uri);
        }

        Uri _uri = ContentUris.withAppendedId(uri, row);

        //这样就通知那些监测databases变化的observer，而你的observer可以在一个service里面注册。
        if (match < 100) {
            getContext().getContentResolver().notifyChange(uri, null);
        }

        return _uri;
    }

    @Override
    public int delete(Uri uri, String selection, String[] selectionArgs) {

        int row = 0;
        int match = m_urlMatcher.match(uri);
        SQLiteDatabase db = null;

        switch (match) {
            case TYPE_Friend:
            case TYPE_Friend_NoNotify:
                db = m_OpenHelper.getWritableDatabase();
                row = db.delete(ITalkDB.TABLE_NAME_Friend, selection, selectionArgs);
                break;
            case TYPE_ChatRecord:
            case TYPE_ChatRecord_NoNotify:
                db = m_OpenHelper.getWritableDatabase();
                row = db.delete(ITalkDB.TABLE_NAME_ChatRecord, selection, selectionArgs);
                break;
            case TYPE_SyncMeta:
            case TYPE_SyncMeta_NoNotify:
                db = m_OpenHelper.getWritableDatabase();
                row = db.delete(ITalkDB.TABLE_NAME_SyncMeta, selection, selectionArgs);
                break;
            default:
                throw new IllegalArgumentException("Unknown URI " + uri);
        }

        if(match < 100) {
            getContext().getContentResolver().notifyChange(uri, null);
        }

        return row;
    }

    @Override
    public int update(Uri uri, ContentValues values, String selection, String[] selectionArgs) {

        int row = 0;
        int match = m_urlMatcher.match(uri);
        SQLiteDatabase db;

        switch (match) {
            case TYPE_Friend:
            case TYPE_Friend_NoNotify:
                db = m_OpenHelper.getWritableDatabase();
                row = db.update(ITalkDB.TABLE_NAME_Friend, values, selection, selectionArgs);
                break;
            case TYPE_ChatRecord:
            case TYPE_ChatRecord_NoNotify:
                db = m_OpenHelper.getWritableDatabase();
                row = db.update(ITalkDB.TABLE_NAME_ChatRecord, values, selection, selectionArgs);
                break;
            case TYPE_SyncMeta:
            case TYPE_SyncMeta_NoNotify:
                db = m_OpenHelper.getWritableDatabase();
                row = db.update(ITalkDB.TABLE_NAME_SyncMeta, values, selection, selectionArgs);
                break;
            default:
                throw new IllegalArgumentException("Unknown URI " + uri);
        }

        if (match < 100) {
            getContext().getContentResolver().notifyChange(uri, null);
        }

        return row;
    }
}
