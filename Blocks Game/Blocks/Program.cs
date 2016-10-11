using System;

/*
______ _            _        
| ___ \ |          | |       
| |_/ / | ___   ___| | _____ 
| ___ \ |/ _ \ / __| |/ / __|
| |_/ / | (_) | (__|   <\__ \
\____/|_|\___/ \___|_|\_\___/

*/
namespace TileGame
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
#endif
}
