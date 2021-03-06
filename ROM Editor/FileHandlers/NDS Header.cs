﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ROM_Editor..NDS_Handlers
{
	public class NDSHeader : HASE.BaseClass
	{
		/// <summary>
		/// This is the header data for NDS ROMs, excluding DSi enhanced ROMs.
		/// This file points to the information in the rest of the game, and
		/// gives the NDS a title, icon, code, et cetera to read before booting
		/// the ROM.
		/// </summary>

		public NDSHeader(Stream stream)
		{
			if(stream.Length != 16384)
			{
				return;
			}

			using (BinaryReader reader = new BinaryReader(stream))
			{
				GameTitle = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(10));
				reader.BaseStream.Position = 12;
				GameCode = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(4));
				MakerCode = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(2));
				UnitCode = reader.ReadByte();
				EncryptionSeed = reader.ReadByte();
				DeviceCapaciy = reader.ReadByte();
				reader.BaseStream.Position = 29;
				RegionCode = reader.ReadByte();
				Version = reader.ReadByte();
				InternalFlags = reader.ReadByte();
				if (debug)
				{
					System.Console.WriteLine("Header");

					System.Console.WriteLine("	GameTitle: " + GameTitle);
					System.Console.WriteLine("	GameCode: " + GameCode);
					System.Console.WriteLine("	MakerCode: " + MakerCode);
					System.Console.WriteLine("	UnitCode: " + UnitCode);
					System.Console.WriteLine("	EncryptionSeed: " + EncryptionSeed);
					System.Console.WriteLine("	DeviceCapaciy: " + DeviceCapaciy);
					System.Console.WriteLine("	RegionCode: " + RegionCode);
					System.Console.WriteLine("	Version: " + Version);
					System.Console.WriteLine("	InternalFlags: " + InternalFlags);
				}


				ARM9Offset = reader.ReadUInt32();
				ARM9Entry = reader.ReadUInt32();
				ARM9Load = reader.ReadUInt32();
				ARM9Length = reader.ReadUInt32();
				if (debug)
				{
					System.Console.WriteLine("	ARM9Offset: " + ARM9Offset);
					System.Console.WriteLine("	ARM9Entry: " + ARM9Entry);
					System.Console.WriteLine("	ARM9Load: " + ARM9Load);
					System.Console.WriteLine("	ARM9Length: " + ARM9Length);
				}

				ARM7Offset = reader.ReadUInt32();
				ARM7Entry = reader.ReadUInt32();
				ARM7Load = reader.ReadUInt32();
				ARM7Length = reader.ReadUInt32();
				if (debug)
				{
					System.Console.WriteLine("	ARM7Offset: " + ARM7Offset);
					System.Console.WriteLine("	ARM7Entry: " + ARM7Entry);
					System.Console.WriteLine("	ARM7Load: " + ARM7Load);
					System.Console.WriteLine("	ARM7Length: " + ARM7Length);
				}


				FNTOffset = reader.ReadUInt32();
				FNTLength = reader.ReadUInt32();
				if (debug)
				{
					System.Console.WriteLine("	FNTOffset: " + FNTOffset);
					System.Console.WriteLine("	FNTLength: " + FNTLength);
				}


				FATOffset = reader.ReadUInt32();
				FATLength = reader.ReadUInt32();
				if (debug)
				{
					System.Console.WriteLine("	FATOffset: " + FATOffset);
					System.Console.WriteLine("	FATLength: " + FATLength);
				}


				ARM9OverlayOffset = reader.ReadUInt32();
				ARM9OverlayLength = reader.ReadUInt32();
				if (debug)
				{
					System.Console.WriteLine("	ARM9OverlayOffset: " + ARM9OverlayOffset);
					System.Console.WriteLine("	ARM9OverlayLength: " + ARM9OverlayLength);
				}


				ARM7OverlayOffset = reader.ReadUInt32();
				ARM7OverlayLength = reader.ReadUInt32();
				if (debug)
				{
					System.Console.WriteLine("	ARM7OverlayOffset: " + ARM7OverlayOffset);
					System.Console.WriteLine("	ARM7OverlayLength: " + ARM7OverlayLength);
				}


				PortNormal = reader.ReadUInt32();
				PortKEY1 = reader.ReadUInt32();
				if (debug)
				{
					System.Console.WriteLine("	PortNormal: " + PortNormal);
					System.Console.WriteLine("	PortKEY1: " + PortKEY1);
				}


				IconOffset = reader.ReadUInt32();
				if (debug)
				{
					System.Console.WriteLine("	IconOffset: " + IconOffset);
				}


				SecureChecksum = reader.ReadUInt16();
				PortKEY1 = reader.ReadUInt16();
				if (debug)
				{
					System.Console.WriteLine("	SecureChecksum: " + SecureChecksum);
					System.Console.WriteLine("	SecureDelay: " + SecureDelay);
				}


				ARM9AutoLoad = reader.ReadUInt32();
				ARM7AutoLoad = reader.ReadUInt32();
				if (debug)
				{
					System.Console.WriteLine("	ARM9AutoLoad: " + ARM9AutoLoad);
					System.Console.WriteLine("	ARM7AutoLoad: " + ARM7AutoLoad);
				}


				SecureDisable = reader.ReadUInt64();
				if (debug)
				{
					System.Console.WriteLine("	SecureDisable: " + SecureDisable);
				}


				TotalSize = reader.ReadUInt32();
				HeaderSize = reader.ReadUInt32();
				if (debug)
				{
					System.Console.WriteLine("	TotalSize: " + TotalSize);
					System.Console.WriteLine("	HeaderSize: " + HeaderSize);
					System.Console.WriteLine();
				}

			}
		}


		// Header Byte Array
		public byte[] Data;					// This'll hold the header itself.

		// Begin Header Data
		public string GameTitle;				// 0x00		12 Characters; ASCII Codes 0x20-0x5F; 0x20 For Spaces; 0x00 For Padding
		public string GameCode;				// 0x0C		Game-Specific 4-Digit Code
		public string MakerCode;				// 0x10		2-Digit License Code Assigned By Nintendo

		// Hardware/Software Information
		public byte UnitCode;				// 0x12		Intended Hardware: 0x0 for NDS; 0x2 For NDS+DSi; 0x3 For DSi
		public byte DeviceType;				// 0x13		Type Of Device Mounted Inside Cartridge
		public byte DeviceCapaciy;			// 0x14		0x0-0x9: Megabits [1|2|4|8|16|32|64|128|256|512]; 0xA-0xF: Gigabits [1|2|4|8|16|32]
		// Reserved [A]					// 0x15		Reserved [0x8]
		public byte RegionCode;				// 0x1D		0x80 For China; 0x40 For Korea; 0x00 For Others
		public byte Version;				// 0x1E		Also Referred To As "RemasterVersion"
		public byte InternalFlags;			// 0x1F		Auto-Load (?)

		// ARM9
		public uint ARM9Offset;		          // 0x20		ARM9 Source Address
		public uint ARM9Entry;		          // 0x24		ARM9 Execution Start Address
		public uint ARM9Load;				// 0x28		ARM9 RAM Transfer Destination Address
		public uint ARM9Length;				// 0x2C		ARM9 Size

		// ARM7
		public uint ARM7Offset;             // 0x30		ARM7 Source Address
		public uint ARM7Entry;              // 0x34		ARM7 Execution Start Address
		public uint ARM7Load;               // 0x38		ARM7 RAM Transfer Destination Address
		public uint ARM7Length;             // 0x3C		ARM7 Size

		// File System
		public uint FNTOffset;				// 0x40		File Name Table Address
		public uint FNTLength;				// 0x44		File Name Table Size
		public uint FATOffset;				// 0x48		File Allocation Table Address
		public uint FATLength;				// 0x4C		File Allocation Table Size

		// Overlay Tables
		public uint ARM9OverlayOffset;		// 0x50		ARM9 Overlay Table Address
		public uint ARM9OverlayLength;		// 0x54		ARM9 Overlay Table Size
		public uint ARM7OverlayOffset;		// 0x58		ARM7 Overlay Table Address
		public uint ARM7OverlayLength;		// 0x5C		ARM7 Overlay Table Size

		// ROM Control Parameters [A]
		public uint PortNormal;             // 0x60		Port 40001A4h For Normal Commands
		public uint PortKEY1;               // 0x64		Port 40001A4h For KEY1 Commands

		// Icon/Title Offset
		public uint BannerOffset;			// 0x68		Banner File Address

		// ROM Control Parameters [B]
		public ushort SecureCRC;			// 0x6C		Secure Region CRC
		public ushort SecureTimeout;        // 0x6E		Secure Transfer Timeout

		// ARM Auto-Load List
		public uint ARM9AutoLoad;           // 0x70		ARM9 Auto-Load Hook Address
		public uint ARM7AutoLoad;			// 0x74		ARM7 Auto-Load Hook Address

		// ROM Control Parameters [C]
		public ulong SecureDisable;			// 0x78		Secure Region Disable

		// ROM Size
		public uint TotalSize;				// 0x80		Offset At ROM's End
		public uint HeaderSize;             // 0x84		Offset At Header's End

		// ARM Auto-Load Parameters
		public uint ARM9AutoParam;			// 0x88		ARM9 Auto-Load Parameters Address
		public uint ARM7AutoParam;          // 0x8C		ARM7 Auto-Load Parameters Address

		// Nintendo Logo					// 0xC0		Nintendo Logo Image [0x9C]
		public ushort NintendoLogoCRC;		// 0x15C	Nintendo Logo CRC
		public ushort HeaderCRC;            // 0x15E	Header CRC
		// Reserved [C]						// 0x160	Reserved For Debugging [0x20]
	}
}
