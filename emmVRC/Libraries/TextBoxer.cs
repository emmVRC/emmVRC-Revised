using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Libraries
{
    public class TextBoxer
    {
        public enum FormatMode
        {
            hard = 0,
            soft = 1
        }
        private string originalText;
        private int xLimit = 80;
        private int yLimit = 25;
        private FormatMode currentMode;
        public TextBoxer(string originalText = "", int xlimit = 80, int ylimit = 25, FormatMode mode = FormatMode.hard)
        {
            this.originalText = originalText;
            this.xLimit = xlimit;
            this.yLimit = ylimit;

        }
        /// <summary>
        ///  Changes the text of the TextBoxer instance
        /// </summary>
        public void SetText(string newText)
        {
            originalText = newText;
        }
        /// <summary>
        /// Changes the text formatting mode of the TextBoxer instance
        /// </summary>
        public void SetMode(FormatMode newMode)
        {
            currentMode = newMode;
        }
    }
}
