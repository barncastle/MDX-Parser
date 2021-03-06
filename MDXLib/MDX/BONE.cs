﻿using MDXLib.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Collections;
using System.Linq;

namespace MDXLib.MDX
{
    public class BONE : BaseChunk, IReadOnlyCollection<Bone>
    {
        private Bone[] Bones;

        public BONE(BinaryReader br, uint version) : base(br)
		{
            Bones = new Bone[br.ReadInt32()];
            for (int i = 0; i < Bones.Length; i++)
                Bones[i] = new Bone(br);
        }

        public int Count => Bones.Length;
        public IEnumerator<Bone> GetEnumerator() => Bones.AsEnumerable().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Bones.AsEnumerable().GetEnumerator();
    }

    public class Bone : GenObject
    {
        public int GeosetId;
        public int GeosetAnimId;

        public Bone(BinaryReader br)
        {
            ObjSize = br.ReadUInt32();
            Name = br.ReadCString(Constants.SizeName);
            ObjectId = br.ReadInt32();
            ParentId = br.ReadInt32();
            Flags = (GENOBJECTFLAGS)br.ReadUInt32();

			LoadTracks(br);

            GeosetId = br.ReadInt32();
            GeosetAnimId = br.ReadInt32();
        }
    }
}
