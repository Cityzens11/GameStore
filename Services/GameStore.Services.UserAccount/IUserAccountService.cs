namespace GameStore.Services.UserAccount
{
    public interface IUserAccountService
    {
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
