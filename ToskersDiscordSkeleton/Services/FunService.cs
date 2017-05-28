using System;

namespace ToskersDiscordSkeleton.Services
{
    public class FunService
    {
        private static Random random = new Random();

        public string RateMe()
        {
            var rating = random.Next(1, 11);

            if (rating < 5)
                return $"If we are being honest... {rating}";
            else if (rating < 7)
                return $"Meh, I'd say you're a {rating}";
            else if (rating < 9)
                return $"You're a solid {rating}";
            else
                return $"Is that even a question? You're a smoking {rating}";
        }

        public string MyPenis()
        {
            var length = random.Next(1, 16);
            var shaft = string.Empty;
            for (int i = 0; i < length; i++)
            {
                shaft += "=";
            }
            return $"8{shaft}D";
        }
    }
}