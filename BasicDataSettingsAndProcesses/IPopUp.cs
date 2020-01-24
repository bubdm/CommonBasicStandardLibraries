using System;
using System.Threading.Tasks;
namespace CommonBasicStandardLibraries.BasicDataSettingsAndProcesses
{
	//well see if this new way works for tablets and phones or not (?)
	//if doing on phones and tablets don't work, then rethinking could be required.

	public interface IPopUp
	{
		Task LoadAsync(string title, string message);
		void PlaySound(int howOftenToRepeat);
		event Func<Task> ClosedAsync;
		event Func<TimeSpan, Task> SnoozedAsync;
	}
}