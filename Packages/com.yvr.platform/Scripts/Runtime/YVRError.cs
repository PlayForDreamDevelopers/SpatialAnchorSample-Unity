namespace YVR.Platform
{
    /// <summary>
    /// Encapsulate properties of message error 
    /// </summary>
    public class YVRError
    {
        /// <summary>
        /// The error code of message error
        /// </summary>
        public readonly ErrorCode errorCode;

        /// <summary>
        /// The error message of message error
        /// </summary>
        public readonly string errorMsg;

        internal YVRError(int eCode, string eMsg)
        {
            errorCode = (ErrorCode)eCode;
            errorMsg = eMsg;
        }

        /// <summary>
        /// Enum of error code
        /// </summary>
        public enum ErrorCode
        {
            UnknownError = -9999,
            ServiceRunError = -5,
            NoUserInfo = -4,
            NeedOfflineEntitle = -3,
            NoRequestMethod = -2,
            NoLoggedUser = -1,
            Success = 0,
            NetworkError = 404,
        }

        public override string ToString()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            
            str.Append($"errorCode:[{errorCode}],\n\r");
            str.Append($"errorMsg:[{ errorMsg ?? "null"}],\n\r");
            
            return str.ToString();
        }
    }
}