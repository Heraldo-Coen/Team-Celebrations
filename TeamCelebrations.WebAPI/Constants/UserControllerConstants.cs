namespace TeamCelebrations.WebAPI.Constants
{
    public static class UserControllerConstants
    {
        // Constants for locking user account
        public const int LOG_IN_UNLOCK_MINUTES = 10;
        public const int MAX_LOGIN_ATTEMPTS = 3;

        // Constants for verification code
        public const int VERIFICATION_CODE_EXPIRED_MINUTES = 5;
        public const int VERIFICATION_MIN_RANGE_VALUE = 50000000;
        public const int VERIFICATION_MAX_RANGE_VALUE = 99999999;
        public const int VERIFICATION_CODE_NULL_VALUE = 10000000;
        public const int RESET_PASSWORD_UNLOCK_MINUTES = 60;
        public const int MAX_RESET_PASSWORD_ATTEMPTS = 3;

        public const string EMAIL_ALREADY_USED_EXCEPTION_MESSAGE_1 = "23505: duplicate key value violates unique constraint";
        public const string EMAIL_ALREADY_USED_EXCEPTION_MESSAGE_2 = "IX_Employees_Email";
    }
}