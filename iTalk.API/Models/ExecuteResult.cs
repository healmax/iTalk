namespace iTalk.API.Models {
    /// <summary>
    /// 執行API結果
    /// </summary>
    public class ExecuteResult {
        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="statusCode">Http 狀態碼</param>
        /// <param name="message">訊息</param>
        public ExecuteResult(bool success = true, int statusCode = 200, string message = null) {
            this.Success = success;
            this.StatusCode = statusCode;
            this.Message = message;
        }

        /// <summary>
        /// 取得 執行 Http 狀態碼
        /// </summary>
        public int StatusCode { get; private set; }

        /// <summary>
        /// 取得 是否成功
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        ///  取得 訊息
        /// </summary>
        public string Message { get; private set; }
    }

    /// <summary>
    /// 執行API結果，含回傳資料
    /// </summary>
    public class ExecuteResult<T> : ExecuteResult {
        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="result">結果</param>
        /// <param name="success">是否成功</param>
        /// <param name="statusCode">Http 狀態碼</param>
        /// <param name="message">訊息</param>
        public ExecuteResult(T result, bool success = true, int statusCode = 200, string message = null)
            : base(success, statusCode, message) {
            this.Result = result;
        }

        /// <summary>
        /// 要回傳的結果
        /// </summary>
        public T Result { get; private set; }
    }
}