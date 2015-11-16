package com.example.healmax.italk.sync;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.os.AsyncTask;
import android.os.Handler;

import com.example.healmax.italk.API.FriendApi;
import com.example.healmax.italk.DataBase.DBUtil;
import com.example.healmax.italk.DataBase.ITalkDB;
import com.example.healmax.italk.DataBase.ITalkProvider;
import com.example.healmax.italk.Model.Friend;
import com.example.healmax.italk.Model.ReturnMessage;
import com.example.healmax.italk.Service.LoginService;
import com.example.healmax.italk.Util.FriendUtil;

import java.lang.ref.WeakReference;
import java.util.List;
import java.util.Objects;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

/**
 * Created by healmax on 15/11/5.
 */
public class SyncManager {

    private static ExecutorService SYNC_TASK_EXECUTOR;
    static {
        SYNC_TASK_EXECUTOR = (ExecutorService) Executors.newFixedThreadPool(1);
    };

    public static void syncActionForService (Context context, int actionCode, Object meta, Handler syncHandler, int syncId) {
        SyncManager.syncAction(context, actionCode, meta, syncHandler, syncId);
    }

    public static void syncAction(Context context, int actionCode, Object meta, Handler syncHandler, int syncId) {
        SyncActionAsyncTask task = new SyncActionAsyncTask(context, actionCode, meta, syncHandler, syncId);
        task.executeOnExecutor(SYNC_TASK_EXECUTOR);
    }

    private static int getFriends(Context context, boolean isNotify) {
        FriendApi api = new FriendApi();
        ReturnMessage<List<Friend>> returnMessage = api.getList();
        if (returnMessage.getStatus() == 0) {
            List<Friend> friendList = returnMessage.getDate();
            syncFriendListToDB(context, friendList);
        }

        return returnMessage.getStatus();
    }

    private static void syncFriendListToDB(Context context, List<Friend> friendList ) {
        Cursor cursor = context.getContentResolver().query(ITalkProvider.uriFriend, null, null, null, null);

        while (cursor.moveToNext()) {
            boolean hasDelete = true;
            String id = cursor.getString(cursor.getColumnIndex(ITalkDB.FIELD_f_id));
            for (Friend friend: friendList) {
                if (id.equals(friend.getId())) {
                    hasDelete = false;
                    break;
                }
            }

            if (hasDelete) {
                DBUtil.deleteFriendById(context, id);
            }
        }

        for (Friend friend: friendList) {
            boolean hasAdd = true;
            cursor.moveToFirst();
            while (cursor.moveToNext()) {
                String id = cursor.getString(cursor.getColumnIndex(ITalkDB.FIELD_f_id));
                if (id.equals(friend.getId())) {
                    hasAdd = false;
                    break;
                }
            }

            if (hasAdd) {
                DBUtil.insertFriend(context, friend, true);
            }
        }
    }

    private static void addFriend(Context context, int friendSysId, boolean isNotify) {
        FriendApi api = new FriendApi();
        Cursor cursor = DBUtil.queryFriendById(context, friendSysId);

        //表示資料庫有此人同步到Server上
        if (cursor.getCount() > 0) {
            Friend friend = new Friend();
            friend.setId(cursor.getString(cursor.getColumnIndex(ITalkDB.FIELD_f_id)));
            friend.setName(cursor.getString(cursor.getColumnIndex(ITalkDB.FIELD_f_name)));
            api.addFriend(friend, LoginService.getInstance().getLoginUser().getToken());
        }
    }



    private static class SyncActionAsyncTask extends AsyncTask<Objects, Void, Void> {

        Context m_context = null;
        int actionCode = -1;
        Object meta = null;
        int retVal = -1;
        Handler handler = null;
        int syncId = -1;

        public SyncActionAsyncTask(Context context, int actionCode, Object meta, Handler syncHandler, int syncId) {
            m_context = context;
            this.actionCode = actionCode;
            this.meta = meta;
            handler = syncHandler;
            this.syncId = syncId;
        }

        @Override
        protected Void doInBackground(Objects... params) {

            switch (this.actionCode) {
                case SyncParameter.SYNC_ACTION_GET_FRIEND_LIST:
                    getFriends(m_context, true);
                    break;
                case SyncParameter.SYNC_ACTION_ADD_FRIEND:
                    addFriend(m_context, syncId, true);
                    break;
            }
            return null;
        }

        @Override
        protected void onPreExecute() {
        }

        @Override
        protected void onPostExecute(Void aVoid) {

        }
    }

    public static long addSyncAction(Context context, String cid, int action) {
        long retVal = -1;

        ContentValues cv = new ContentValues();
        cv.put(ITalkDB.FIELD_SYNC_ACTION, action);
        cv.put(ITalkDB.FIELD_SYNC_IS_SYNC, 0);
        retVal = DBUtil.insertSyncTable(context, cv);

        return retVal;
    }
}
