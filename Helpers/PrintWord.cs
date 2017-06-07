using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

namespace TestCreator.Helpers
{
    public class PrintWord
    {

        public static void Print(string path)
        {
            object wordOMissing = Missing.Value;

            Application wordApp = new Application { Visible = false };
            Microsoft.Office.Interop.Word.Document doc = wordApp.Documents.Open(path, ReadOnly: true, Visible: true);
            
            doc.PrintOut();
            doc.Close(WdSaveOptions.wdDoNotSaveChanges, wordOMissing, wordOMissing);
            wordApp.Quit(ref wordOMissing, ref wordOMissing, ref wordOMissing);
        }
    }
}
