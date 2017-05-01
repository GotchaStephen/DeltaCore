using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DateSelector : MonoBehaviour {

	public Dropdown daysDropdown;
	public Dropdown monthsDropdown;
	public Dropdown yearsDropdown;

	//[HideInInspector]
	public int day;
	//[HideInInspector]
	public int month;
	//[HideInInspector]
	public int year;

	//Used for manipulating the drop down menu options for date
	private static List<string> monthsList = new List<string>();
	private static List<int> yearsList = new List<int>();

	// Use this for initialization
	void Start ()
	{
		Reset();
	}

	public void Reset()
	{
		// Add values to dropdown menus

		// Clear, then add years
		yearsDropdown.options.Clear();
		AddYears();

		// Clear, then add months
		monthsDropdown.options.Clear();
		AddMonths();

		//Adjust days relative to month
		AdjustDayDropdown(31);

		UpdateDays();

		//Set day
		day = daysDropdown.value + 1;
		//Set month
		month = monthsDropdown.value  + 1;
		// Set year
		List<Dropdown.OptionData> menuOptions = yearsDropdown.GetComponent<Dropdown>().options;
		year = Convert.ToInt32(menuOptions [0].text);
	}

	public void UpdateDays()
	{
		if ((month == 2 ) && !(year % 4 == 0))
		{
			AdjustDayDropdown(28);
		}
		//leap year
		else if(month == 2 && (year % 4 == 0))
		{
			AdjustDayDropdown(29);
		}
		else if(month == 4 || month == 6 || month == 9 || month == 11)
		{
			AdjustDayDropdown(30);
		}
		else if(month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
		{
			AdjustDayDropdown(31);
		}
	}

	public void AddMonths()
	{
		monthsDropdown.options.Add(new Dropdown.OptionData(){text = "January"});
		monthsDropdown.options.Add(new Dropdown.OptionData(){text = "February"});
		monthsDropdown.options.Add(new Dropdown.OptionData(){text = "March"});
		monthsDropdown.options.Add(new Dropdown.OptionData(){text = "April"});
		monthsDropdown.options.Add(new Dropdown.OptionData(){text = "May"});
		monthsDropdown.options.Add(new Dropdown.OptionData(){text = "June"});
		monthsDropdown.options.Add(new Dropdown.OptionData(){text = "July"});
		monthsDropdown.options.Add(new Dropdown.OptionData(){text = "August"});
		monthsDropdown.options.Add(new Dropdown.OptionData(){text = "September"});
		monthsDropdown.options.Add(new Dropdown.OptionData(){text = "October"});
		monthsDropdown.options.Add(new Dropdown.OptionData(){text = "November"});
		monthsDropdown.options.Add(new Dropdown.OptionData(){text = "December"});
	}

	public void AddYears()
	{
		int startYear = 2017;

		yearsDropdown.options.Add(new Dropdown.OptionData(){text = 2017.ToString()});
		yearsDropdown.options.Add(new Dropdown.OptionData(){text = 2018.ToString()});
	}

	public void AdjustDayDropdown(int maxDays)
	{
		int counter = 1;

		// Clear the options
		daysDropdown.options.Clear();

		while(counter <= maxDays)
		{
			daysDropdown.options.Add(new Dropdown.OptionData(){text = counter.ToString()});
			counter++;
		}

		if (day <= maxDays)
		{
			daysDropdown.value = day - 1;
		}
		else
		{
			day = 1;
			daysDropdown.value = 0;
		}
	}

	public void SetDay(int index)
	{
		List<Dropdown.OptionData> menuOptions = daysDropdown.GetComponent<Dropdown>().options;
		day = Convert.ToInt32(menuOptions [index].text);
	}
	public void SetMonth()
	{
		month = monthsDropdown.value + 1;
		UpdateDays(); 
	}
	public void SetYear(int index)
	{
		List<Dropdown.OptionData> menuOptions = yearsDropdown.GetComponent<Dropdown>().options;
		year = Convert.ToInt32(menuOptions [index].text);
	}
}
