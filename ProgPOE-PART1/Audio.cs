using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace ProgPOE_PART1
{
    class Audio
    {
        private readonly string _filePath;

        public Audio(string filePath)
        {
            _filePath = filePath;
        }

        public void Play()
        {
            try
            {
                // Use 'using' so SoundPlayer disposes automatically
                using (SoundPlayer player = new SoundPlayer(_filePath))
                {
                    player.PlaySync(); // Blocks until file finishes playing
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error playing WAV file: {ex.Message}");
            }
        }
    }
}
