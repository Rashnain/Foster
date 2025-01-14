﻿using System.Collections;

namespace Foster.Framework.Storage;

/// <summary>
/// Default Content implementation.
/// Should work well for PC, etc, but can be overridden for custom handling.
/// </summary>
public class Content
{
	public string CurrentDirectory { get; set; } = "";

	private class ContentEnumerator : IEnumerator<string>
	{
		public string[] Locations;
		public int Index = -1;

		public string Current
		{
			get
			{
				try
				{
					return Locations[Index];
				}
				catch (IndexOutOfRangeException)
				{
					throw new InvalidOperationException();
				}
			}
		}

		object IEnumerator.Current => Current;

		public ContentEnumerator(string[] locations)
		{
			Locations = locations;
		}

		public bool MoveNext()
		{
			Index++;
			if (Index >= Locations.Length)
				return false;
			return true;
		}

		public void Reset() => Index = -1;

		public void Dispose() { }
	}

	public Content() { }
	public Content(string content) : this()
	{
		CurrentDirectory = content;
	}

	#region Directory
	public virtual bool FileExists(string relativePath)
	{
		return File.Exists(Path.Combine(CurrentDirectory, relativePath));
	}
	public virtual bool DirectoryExists(string relativePath)
	{
		return Directory.Exists(Path.Combine(CurrentDirectory, relativePath));
	}
	public virtual bool Exists(string name)
	{
		return FileExists(name) || DirectoryExists(name);
	}

	public virtual IEnumerator<string> EnumerateFiles(string path, string searchPattern, bool recursive)
	{
		return new ContentEnumerator(
				Directory.GetFiles(
					Path.Combine(CurrentDirectory, path),
					searchPattern,
					recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly
					));
	}

	public virtual IEnumerator<string> EnumerateDirectories(string path, string searchPattern, bool recursive)
	{
		return new ContentEnumerator(
			Directory.GetDirectories(
				Path.Combine(CurrentDirectory, path),
				searchPattern,
				recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly
				));
	}

	#endregion

	#region File

	public virtual Stream OpenRead(string relativePath)
	{
		return File.OpenRead(Path.Combine(CurrentDirectory, relativePath));
	}

	public virtual byte[] ReadAllBytes(string relativePath)
	{
		return File.ReadAllBytes(Path.Combine(CurrentDirectory, relativePath));
	}

	public virtual string ReadAllText(string relativePath)
	{
		return File.ReadAllText(Path.Combine(CurrentDirectory, relativePath));
	}

	#endregion
}
