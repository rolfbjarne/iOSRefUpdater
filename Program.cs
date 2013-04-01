using System;
using System.IO;
using Mono.Cecil;

class AssemblyReferenceUpdater {
	
	static byte[] new_pk_token = { 0x84, 0xe0, 0x4f, 0xf9, 0xcf, 0xb7, 0x90, 0x65 };
	
	static int Usage (string message)
	{
		Console.WriteLine ("Assembly Reference Update for Xamarin.iOS");
		Console.WriteLine ("Usage: arefupdate assembly.dll");
		Console.WriteLine ();
		Console.WriteLine ("Error: {0}", message);
		return 1;
	}
	
	public static int Main (string[] args)
	{
		if (args.Length == 0)
			return Usage ("No assembly was specified.");
		string filename = args [0];
		if (!File.Exists (filename))
			return Usage (String.Format ("Could not find `{0}`.", filename));
		
		string backup = args [0] + ".bak";
		if (File.Exists (backup))
			return Usage (String.Format ("Backup file `{0}` is already present.", backup));
		
		try {
			File.Copy (args [0], backup);
			
			bool action = false;
			var ad = AssemblyDefinition.ReadAssembly (args [0]);
			foreach (var reference in ad.MainModule.AssemblyReferences) {
				switch (reference.FullName) {
				case "monotouch, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null":
				case "MonoTouch.Dialog-1, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null":
				case "OpenTK, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null":
					action = true;
					Console.WriteLine ("Updating reference for {0}", reference.Name);
					reference.PublicKeyToken = new_pk_token; 
					break;
				}
			}
			if (action) {
				if (ad.Name.HasPublicKey)
					Console.WriteLine ("warning: any existing assembly signature will be invalid.");
				ad.Write (args [0]);
			} else {
				Console.WriteLine ("No reference needed to be modified. Original file is unchanged.");
			}
			return 0;
		}
		catch (Exception e) {
			return Usage (e.ToString ());
		}
	}
}