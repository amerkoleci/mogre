using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using AutoWrap.Meta;
using AutoWrap.Mogre;

namespace AutoWrap
{
    static class Program
    {
        private const string BASE_DIR = @"..\..\..\..\";
        
        private const string META_XML_FILE = BASE_DIR + @"Codegen\cpp2java\build\meta.xml";
        private const string ATTRIBUTES_FILE = BASE_DIR + @"Codegen\Attributes.xml";
        
        private const string INCLUDES_DEST_DIR = BASE_DIR + @"Main\auto\include";
        private const string SRC_DEST_DIR = BASE_DIR + @"Main\auto\src";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (!File.Exists(META_XML_FILE))
            {
                MessageBox.Show("The file \"Meta.xml\" doesn't exist. You need to run \"cpp2java\" to create it first.", "Error");
                return;
            }

            MetaDefinition meta = new MetaDefinition(META_XML_FILE, "Ogre", "Mogre",
                                                new MogreConstructFactory(), new MogreCodeStyleDef());
            
            try
            {
                meta.AddAttributes(ATTRIBUTES_FILE);
            }
            catch (InvalidAttributeException e)
            {
                MessageBox.Show("Invalid attribute found: " + e.Message, "Error");
                return;
            }

            //check if auto directories exists, and create it if needed
            if (!Directory.Exists(INCLUDES_DEST_DIR))
                Directory.CreateDirectory(INCLUDES_DEST_DIR);

            if (!Directory.Exists(SRC_DEST_DIR))
                Directory.CreateDirectory(SRC_DEST_DIR);


            Wrapper wrapper = new Wrapper(meta, INCLUDES_DEST_DIR, SRC_DEST_DIR);

            if (args.Length > 0)
            {
                // Command line mode - parse arguments.
                if (args.Length == 1 && args[0] == "produce")
                {
                    wrapper.IncludeFileWrapped += delegate(object sender, IncludeFileWrapEventArgs e)
                    {
                        Console.WriteLine(e.IncludeFile);
                    };
                    wrapper.GenerateCodeFiles();
                }
                else
                {
                    Console.Write(
                        "Invalid command\n\n" +
                        "Usage: AutoWrap.exe <command>\n\n" +
                        "Supported Commands\n" +
                        "==================\n\n" +
                        "    produce    Produces Mogre auto-generated files (equivalent to pressing the \"Produce\" button in the GUI).\n" +
                        "\n"
                    );
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new AutoWrap(wrapper));
            }
        }
    }
}