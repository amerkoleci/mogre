using System;
using System.Text;

namespace AutoWrap.Meta
{
    /// <summary>
    /// This string builder is used to create all generated source code files.
    /// </summary>
    public class SourceCodeStringBuilder
    {
        private readonly StringBuilder _builder = new StringBuilder();
        private readonly CodeStyleDefinition _codeStyleDef;
        private string _curIndention = "";
        private bool _isOnNewLine = true;

        public SourceCodeStringBuilder(CodeStyleDefinition codeStyleDef)
        {
            _codeStyleDef = codeStyleDef;
        }

        /// <summary>
        /// Clears this string builder.
        /// </summary>
        public void Clear()
        {
            _builder.Clear();
            _curIndention = "";
        }

        /// <summary>
        /// Increases the indention by one level.
        /// </summary>
        public void IncreaseIndent()
        {
            _curIndention += _codeStyleDef.IndentionLevelString;
        }

        /// <summary>
        /// Decreases the indention by one level.
        /// </summary>
        public void DecreaseIndent()
        {
            // Strip one indention level
            _curIndention = _curIndention.Substring(_codeStyleDef.IndentionLevelString.Length);
        }

        /// <summary>
        /// Appends the begining of a code block while automatically managing indentation.
        /// </summary>
        public void BeginBlock()
        {
            AppendLine("{");
            IncreaseIndent();
        }

        /// <summary>
        /// Appends the end of a code block while automatically managing indentation.
        /// </summary>
        public void EndBlock()
        {
            DecreaseIndent();
            AppendLine("}");
        }

        public void InsertAt(uint pos, string str, bool indent = true)
        {
            _builder.Insert((int)pos, CreateAppendableString(str, indent, indent));
        }

        /// <summary>
        /// Appends the specified string adhering to the current indentation level. If <paramref name="args"/> is specified the string will be formatted.
        /// </summary>
        /// <param name="str">A string or composite format string to append.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <see cref="AppendLine"/>
        public SourceCodeStringBuilder Append(string str, params object[] args)
        {
            if (_codeStyleDef.IndentionLevelString != "\t")
            {
                // Replace remaining tabs with the correct indention style
                str = str.Replace("\t", _codeStyleDef.IndentionLevelString);
            }

            if (args.Length > 0)
            {
                str = String.Format(str, args);
            }

            string[] lines = str.Replace("\r\n", "\n").Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                if (_isOnNewLine && lines[i].Length > 0)
                {
                    _builder.Append(_curIndention);
                }

                _builder.Append(lines[i]);

                if (i < lines.Length - 1)
                {
                    _builder.Append(_codeStyleDef.NewLineCharacters);
                    _isOnNewLine = true;
                }
                else
                {
                    _isOnNewLine = lines[i].Length == 0;
                }
            }

            return this;
        }

        /// <summary>
        /// Appends an empty line.
        /// </summary>
        public void AppendEmptyLine()
        {
            _builder.Append(_codeStyleDef.NewLineCharacters);
            _isOnNewLine = true;
        }

        /// <summary>
        /// Appends the specified string adhering to the current indentation level followed by a new line. If <paramref name="args"/> is specified the string will be formatted.
        /// </summary>
        /// <param name="str">A string or composite format string to append.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <see cref="Append"/>
        public SourceCodeStringBuilder AppendLine(string str, params object[] args)
        {
            Append(str, args);
            AppendEmptyLine();
            return this;
        }

        /// <summary>
        /// Converts the value of this instance to a <see cref="System.String"/>.
        /// </summary>
        public override string ToString()
        {
            return _builder.ToString();
        }

        private string CreateAppendableString(string str, bool firstLineIndent, bool otherLinesIndent)
        {
            if (_codeStyleDef.IndentionLevelString != "\t")
            {
                // Replace remaining tabs with the correct indention style
                str = str.Replace("\t", _codeStyleDef.IndentionLevelString);
            }

            string[] lines = str.Replace("\r\n", "\n").Split('\n');
            string result;

            if (firstLineIndent)
                result = _curIndention;
            else
                result = "";

            if (lines.Length == 1)
                return result + lines[0];

            if (otherLinesIndent)
            {
                result += String.Join(_codeStyleDef.NewLineCharacters + _curIndention, lines);

                // Remove indention at the end of the new string when the 
                // original string ended with an empty line.
                if (result.EndsWith(_curIndention))
                    result = result.Substring(0, result.Length - _curIndention.Length);
            }
            else
                result += String.Join(_codeStyleDef.NewLineCharacters, lines);

            return result;
        }
    }
}