using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		[SerializeField]
		private ManualPageEntry[] PageEntries;

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
				if (value >= PageEntries.Length)
				{
					_page = 0;
				}
				else if (value < 0)
				{
					_page = PageEntries.Length - 1;
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

		private void UpdateActiveManualEntry()
		{
			manualImage = PageEntries[Page].Image;
			manualTitle.text = PageEntries[Page].Title;
			manualText.text = PageEntries[Page].Text;
			manualPage.text = GetPageString();

		}

		private string GetPageString()
		{
			return String.Format("{0}/{1}", Page + 1, PageEntries.Length);
		}
	}
}
