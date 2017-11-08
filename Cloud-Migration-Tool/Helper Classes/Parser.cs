using FileHelpers;
using Cloud_Migration_Tool.Models;
using System.Windows.Forms;

namespace Cloud_Migration_Tool.Helper_Classes
{
    public class Parser
    {
        public Parser()
        {

        }

        public FileToBeMigrated[] Parse(string filePath)
        {
            var engine = new FileHelperEngine<FileToBeMigrated>();
            var records = engine.ReadFile(filePath);

            return records;
        }
    }
}
