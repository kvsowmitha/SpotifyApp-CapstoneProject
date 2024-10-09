namespace AuthenticationService.Repository
{
    public class EmailBody
    {
        public static string EmailStringBody(string html, string otp)
        {
            return html.Replace("{OTP_PLACEHOLDER}", otp);
        }
    }
}
