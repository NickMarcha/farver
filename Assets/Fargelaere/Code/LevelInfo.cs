using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct LevelInfo : IEquatable<LevelInfo>
{

	public TileBase[,,] tiles;

	public BlobInfo[] blobs;

	public Vector3Int startPos;

	public LevelInfo(TileBase[,,] tiles, BlobInfo[] blobs, Vector3Int startPos)
	{
		this.tiles = tiles;
		this.blobs = blobs;
		this.startPos = startPos;
	}

	public bool Equals(LevelInfo other)
	{
		return tiles.Equals(other.tiles) && blobs.Equals(other.blobs) && startPos.Equals(other.startPos);
	}

	public struct BlobInfo
	{
		public Vector3 position;
		public Color color;
	}
}
