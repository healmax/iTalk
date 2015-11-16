package com.example.healmax.italk.sync;

import android.app.Service;
import android.content.Intent;
import android.database.ContentObserver;
import android.database.Cursor;
import android.os.Handler;
import android.os.IBinder;
import android.support.annotation.Nullable;

import com.example.healmax.italk.DataBase.ITalkDB;
import com.example.healmax.italk.DataBase.ITalkProvider;

import java.util.LinkedList;
import java.util.Queue;

/**
 * Created by healmax on 15/11/5.
 */
public class SyncService extends Service {

    private SyncDBobserver m_syncDBobserver;

    private static Queue<SyncData> uploadQ ;
    private static Queue<SyncData> downloadQ;

    @Nullable
    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    @Override
    public void onCreate() {
        super.onCreate();
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        uploadQ = new LinkedList<SyncData>();
        downloadQ = new LinkedList<SyncData>();
        registerContentObserver();
        return super.onStartCommand(intent, flags, startId);
    }

    @Override
    public void onDestroy() {
        super.onDestroy();
        unregisterContentObserver();
        uploadQ.clear();
        downloadQ.clear();;
    }

    private void registerContentObserver() {
        m_syncDBobserver = new SyncDBobserver();
        getContentResolver().registerContentObserver(ITalkProvider.uriSyncMeta, true, m_syncDBobserver);
    }

    private void unregisterContentObserver() {
        getContentResolver().unregisterContentObserver(m_syncDBobserver);
    }

    public class SyncDBobserver extends ContentObserver {


        public SyncDBobserver() {
            super(new Handler());
        }

        @Override
        public void onChange(boolean selfChange) {
            super.onChange(selfChange);
            refreshSync();
        }
    }

    private synchronized void refreshSync() {

        Cursor cursor = this.getContentResolver().query(ITalkProvider.uriSyncMeta, null, null, null, null);

        if(cursor != null && cursor.getCount() > 0) {
            while (cursor.moveToNext()) {
                int action = cursor.getInt(cursor.getColumnIndex(ITalkDB.FIELD_SYNC_ACTION));
                int id = cursor.getInt(cursor.getColumnIndex(ITalkDB.FIELD_SYNC_ID));

                SyncData syncData;

                switch (action) {
                    case SyncParameter.SYNC_ACTION_ADD_FRIEND:
                        syncData = new SyncData(SyncParameter.SYNC_ACTION_ADD_FRIEND, null, null, id);
                        uploadQ.offer(syncData);
                        break;

                    case SyncParameter.SYNC_ACTION_GET_FRIEND_LIST:
                        syncData = new SyncData(SyncParameter.SYNC_ACTION_GET_FRIEND_LIST, null, null, id);
                        downloadQ.offer(syncData);
                        break;
                }

                processNextSyncDate();
            }
        }
    }

    private synchronized void processNextSyncDate () {
        SyncData nextSyncData = null;

        if (! uploadQ.isEmpty()) {
            nextSyncData = uploadQ.poll();
        } else if (! downloadQ.isEmpty()) {
            nextSyncData = downloadQ.poll();
        } else {
            return;
        }

        if (nextSyncData != null) {
            SyncManager.syncActionForService(getApplicationContext(), nextSyncData.actionCode, nextSyncData.meta, nextSyncData.syncHandler, nextSyncData.syncId);
        }
    }

    private class SyncData {
        int actionCode;
        Object meta;
        Handler syncHandler;
        int syncId;

        public SyncData(int action, Object meta, Handler handler, int id) {
            actionCode = action;
            this.meta = meta;
            syncHandler = handler;
            syncId = id;
        }
    }
}
