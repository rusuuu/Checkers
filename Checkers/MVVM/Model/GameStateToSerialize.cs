using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace Checkers.MVVM
{
    [Serializable]
    public class GameStateToSerialize
    {
        public string gameName { get; set; }
        public List<int> Pieces { get; set; }
        public int playerToMove {  get; set; }
        public int multipleJump {  get; set; }

        public GameStateToSerialize()
        {
        }

        public GameStateToSerialize(string gameName, List<int> pieces, int playerToMove, int multipleJump)
        {
            this.gameName = gameName;
            this.Pieces = pieces;
            this.playerToMove = playerToMove;
            this.multipleJump = multipleJump;   
        }

        public void SerializeGame(GameStateToSerialize game, List<GameStateToSerialize> games)
        {
            string projectDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;
            string directoryPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDirectory, "MVVM\\View\\Resources"));

            games.Add(game);

            string filePath = Path.Combine(directoryPath, "games.json");
            string jsonString = JsonSerializer.Serialize<List<GameStateToSerialize>>(games);
            File.WriteAllText(filePath, jsonString);
        }

        public List<GameStateToSerialize> DeserializeGames()
        {
            string projectDirectory = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectDirectory, "MVVM\\View\\Resources\\games.json"));

            if (!File.Exists(filePath))
            {
                return new List<GameStateToSerialize>();
            }

            string jsonContent = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<GameStateToSerialize>>(jsonContent);
        }
    }
}
