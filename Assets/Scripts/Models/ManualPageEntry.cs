using System;
using UnityEngine;

namespace Assets.Scripts.Models
{
	[Serializable]
	public class ManualPageEntry
	{
		[SerializeField]
		private Sprite _image;
		public Sprite Sprite
		{
			get
			{
				return _image;
			}
		}

		[SerializeField]
		private string _title;
		public string Title
		{
			get
			{
				return _title;
			}
		}

		[SerializeField]
		private string _text;
		public string Text
		{
			get
			{
				return _text;
			}
		}

		[SerializeField]
		private string _category;
		public string Category
		{
			get
			{
				return _category;
			}
		}
	}
}
