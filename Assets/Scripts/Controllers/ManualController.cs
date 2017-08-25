using Assets.Scripts.Models;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
	public class ManualController
		: MonoBehaviour
	{
		[SerializeField]
		private Image manualImage;

		[SerializeField]
		private Text manualTitle;

		[SerializeField]
		private Text manualText;

		[SerializeField]
		private Text manualPage;

		public ManualPageEntry[] PageEntries;

		public ManualPageEntry[] LoadedPageEntries;

		private void Start()
		{
			UpdateActiveManualEntry();
		}

		private int _page;
		private int Page
		{
			get
			{
				return _page;
			}
			set
			{
				if (value >= LoadedPageEntries.Length)
				{
					_page = 0;
				}
				else if (value < 0)
				{
					_page = LoadedPageEntries.Length - 1;
				}
				else
				{
					_page = value;
				}

				UpdateActiveManualEntry();
			}
		}

		public void NextManualEntry()
		{
			Page++;
		}

		public void PreviousManualEntry()
		{
			Page--;
		}

		public void ResetPage()
		{
			Page = 0;
		}

		public void UpdateActiveManualEntry()
		{
			manualImage.sprite = LoadedPageEntries[Page].Sprite;
			manualTitle.text = LoadedPageEntries[Page].Title;
			manualText.text = LoadedPageEntries[Page].Text;
			manualPage.text = GetPageString();

		}

		private string GetPageString()
		{
			return String.Format("{0}/{1}", Page + 1, LoadedPageEntries.Length);
		}
	}
}
