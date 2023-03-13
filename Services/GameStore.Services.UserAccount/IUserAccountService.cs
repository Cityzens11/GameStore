namespace GameStore.Services.UserAccount
{
    public interface IUserAccountService
    {
        /// <summary>
        /// Gets the user by username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<UserAccountModel> GetUser(string userName);
        /// <summary>
        /// Create user account
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<UserAccountModel> Create(RegisterUserAccountModel model);

        Task SendVerifyEmail(UserAccountModel model);
        Task Verify(string userId, string code);
    }
}
