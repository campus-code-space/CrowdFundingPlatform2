using Supabase;

namespace EndeKisse2.Services
{
    public class SupabaseService
    {
         private Supabase.Client _client;
       
        public Supabase.Client GetClient()
        {
            var key = Environment.GetEnvironmentVariable("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImJlZnJhcmRpZG9idnF4c2txcHBnIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzU2Njg3MDAsImV4cCI6MjA1MTI0NDcwMH0.bkBOMmX8sNSkB6-uOGO54H6HQuO0irfGnw-2r4sy9zk");

            if (_client == null)
            {
                _client = new Supabase.Client("https://befrardidobvqxskqppg.supabase.co", key);
                _client.InitializeAsync().Wait();
            }
            return _client;
        }
    }

}
