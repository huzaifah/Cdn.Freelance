namespace Cdn.Freelance.Domain.SeedWork
{
    /// <summary>
    /// The user composer
    /// </summary>
    public interface IUserComposer
    {
        /// <summary>
        /// Format the clientId and userName
        /// </summary>
        /// <param name="clientId">The client id</param>
        /// <param name="subject">The subject</param>
        /// <param name="actClientId">The act </param>
        /// <returns></returns>
        string Compose(string clientId, string subject, string actClientId);
    }

    /// <summary>
    /// The default user composer
    /// </summary>
    public class DefaultUserComposer : IUserComposer
    {
        /// <summary>
        /// Format the user data to the format "clientId/userName"
        /// </summary>
        /// <param name="clientId">The client id</param>
        /// <param name="subject">The subject</param>
        /// <param name="actClientId">The act client id</param>
        /// <returns></returns>
        public string Compose(string clientId, string subject, string actClientId)
        {
            string user = $"{Uri.EscapeDataString(clientId ?? string.Empty)}";

            if (!string.IsNullOrEmpty(subject))
                user += $"/{Uri.EscapeDataString(subject)}";

            if (!string.IsNullOrEmpty(actClientId))
                user += $"/{Uri.EscapeDataString(actClientId)}";

            return user;
        }
    }
}
