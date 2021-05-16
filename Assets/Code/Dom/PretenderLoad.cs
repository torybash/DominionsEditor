using System.IO;

public static class PretenderLoad
{


	public static Pretender LoadFile (string pretenderFilePath)
	{
		var fileBytes = File.ReadAllBytes(pretenderFilePath);
		// Debug.Log($"file: {pretenderFilePath}, fileBytes: {fileBytes.Length}");

		int counter = 0;
		
		//bytes
		//10:		??? 
		counter += 10;
		
		//2:	version?
		int version = fileBytes[counter];
		int version2 = fileBytes[counter+1];
		counter += 2;

		//10:	00?
		//4:	ff?
		counter += 14;
		
		//1:		nation
		int nation = fileBytes[counter];
		counter += 1;
		
		//11:		00?
		//9:		user id?
		counter += 20;
		
		
		byte lockCheck = fileBytes[counter];

		bool isLocked = lockCheck != 0x78;
		if (!isLocked)
		{
			//if no lock:
			//1:		78?
			byte asd = 0x78;
			counter += 1;
		} else
		{
			// Debug.LogError("locked pretender files not handled");
			// return new Pretender(pretenderFilePath);
			//if lock:
			//1-N		password?
			//TODO Check for 78? to see when password ends
		}
		
		//15:		other id?
		counter += 15;
		
		//26:		00?
		counter += 26;
		
		//4:		ff?
		counter += 4;
		
		// //1-N:		pretender name?
		// const int MAX_PRETENDER_NAME_LENGTH = 32;
		// string pretenderName = "";
		// for (int i = 0; i < MAX_PRETENDER_NAME_LENGTH; i++)
		// {
		// 	var charByte = fileBytes[counter + i];
		// 	if (charByte == 0xFF) //Check for 4 FFs to see when name ends?
		// 	{
		// 		break;
		// 	}
		//
		// 	pretenderName += ByteToChar(charByte);
		// 	// pretenderName += charByte;
		// }
		//
		// counter += pretenderName.Length;
		
		//4:		ff?
		counter += 4;
		//
		// Debug.Log($"" +
		// 	$"nation: {nation} " +
		// 	// $"pretenderName: {pretenderName} " +
		// 	$"version: {version}.{version2} " +
		// 	$"");
		
		return new Pretender(pretenderFilePath);
	}

	public static char ByteToChar (byte b)
	{
		switch (b)
		{
			case 0x0e: return 'A';	
			case 0x0d: return 'B';	
			case 0x0c: return 'C';	
			case 0x0b: return 'D';	
			case 0x0a: return 'E';	
			case 0x09: return 'F';	
			case 0x08: return 'G';	
			case 0x07: return 'H';	
			case 0x06: return 'I';	
			case 0x05: return 'J';	
			case 0x04: return 'K';	
			case 0x03: return 'L';	
			case 0x02: return 'M';	
			case 0x01: return 'N';	
			case 0x00: return 'O';	
			case 0x1f: return 'P';	
			case 0x1e: return 'Q';	
			case 0x1d: return 'R';	
			case 0x1c: return 'S';	
			case 0x1b: return 'T';	
			case 0x1a: return 'U';	
			case 0x19: return 'V';	
			case 0x18: return 'W';	
			case 0x17: return 'X';	
			case 0x16: return 'Y';	
			case 0x15: return 'Z';	
			
			case 0x2e: return 'a';	
			case 0x2d: return 'b';	
			case 0x2c: return 'c';	
			case 0x2b: return 'd';	
			case 0x2a: return 'e';	
			case 0x29: return 'f';	
			case 0x28: return 'g';	
			case 0x27: return 'h';	
			case 0x26: return 'i';	
			case 0x25: return 'j';	
			case 0x24: return 'k';	
			case 0x23: return 'l';	
			case 0x22: return 'm';	
			case 0x21: return 'n';	
			case 0x20: return 'o';	
			case 0x3f: return 'p';	
			case 0x3e: return 'q';	
			case 0x3d: return 'r';	
			case 0x3c: return 's';	
			case 0x3b: return 't';	
			case 0x3a: return 'u';	
			case 0x39: return 'v';	
			case 0x38: return 'w';	
			case 0x37: return 'x';	
			case 0x36: return 'y';	
			case 0x35: return 'z';	
		}

		return 'Z';
	}
}