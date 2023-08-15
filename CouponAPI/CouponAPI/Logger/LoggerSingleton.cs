namespace ECOM.Services.CouponAPI.Logger; 
    public sealed class LoggerSingleton
    {
        private LoggerSingleton() { }

        private static LoggerSingleton? _instance;

        public static LoggerSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LoggerSingleton();
                }
                return _instance;
            }
        }

        public void Log(string message) { Console.WriteLine(message); }

    }
 
