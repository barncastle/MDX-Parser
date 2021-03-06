﻿using MDXLib.Structs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MDXLib.MDX
{
	public class EVTS : BaseChunk, IReadOnlyCollection<Event>
	{
		Event[] Events;

		public EVTS(BinaryReader br, uint version) : base(br)
		{
			Events = new Event[br.ReadInt32()];
			for (int i = 0; i < Events.Length; i++)
				Events[i] = new Event(br);
		}

		public int Count => Events.Length;
		public IEnumerator<Event> GetEnumerator() => Events.AsEnumerable().GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => Events.AsEnumerable().GetEnumerator();
	}

	public class Event : GenObject
	{
		public uint TotalSize;
		public SimpleTrack EventKeys;

		public Event(BinaryReader br)
		{
			TotalSize = br.ReadUInt32();
			long end = br.BaseStream.Position + TotalSize;

			ObjSize = br.ReadUInt32();
			Name = br.ReadCString(Constants.SizeName);
			ObjectId = br.ReadInt32();
			ParentId = br.ReadInt32();
			Flags = (GENOBJECTFLAGS)br.ReadUInt32();
			
			LoadTracks(br);

			while (br.BaseStream.Position < end && !br.AtEnd())
			{
				string tagname = br.ReadString(4);
				switch (tagname)
				{
					case "KEVT": EventKeys = new SimpleTrack(br, false); break;
					default:
						br.BaseStream.Position -= 4;
						return;
				}
			}
		}
	}


}
