using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Models
{
	[Serializable]
	public class ManualPageEntry
	{
		[SerializeField]
		private Image _image;
		public Image Image
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
	}
}
