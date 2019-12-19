using CommonBasicStandardLibraries.MVVMHelpers;
using CommonBasicStandardLibraries.MVVMHelpers.Interfaces;
using System;
using System.Threading.Tasks; //most of the time, i will be using asyncs.
namespace CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions
{
    public static class ViewModelHelpers
    {
        public static async Task ProcessCommandAsync(this IToggleVM viewModel, IAsyncCommand? command)
        {
            if (command == null)
                return;
            if (command.CanExecute() == false)
                return;
            viewModel.Visible = false;
            await command.ExecuteAsync();
        }
        public static async Task ProcessCommandAsync<T>(this IToggleVM viewModel, IAsyncCommand<T>? command, T args)
        {
            if (command == null)
                return;
            if (command.CanExecute(args) == false)
                return;
            viewModel.Visible = false;
            await command.ExecuteAsync(args);
        }
        public static async Task ProcessCommandAsync(this IToggleVM viewModel, Func<Task>? action)
        {
            if (action == null)
                return;
            viewModel.Visible = false;
            await action.Invoke();
        }
        public static async Task ProcessCommandAsync<T>(this IToggleVM viewModel, Func<T, Task>? action, T args)
        {
            if (action == null)
                return;
            viewModel.Visible = false;
            await action.Invoke(args);
        }
        public static void ToggleVisible(this IToggleVM disable, IToggleVM enable)
        {
            disable.Visible = false;
            enable.Visible = true;
        }
        public static void CreateBackCommand(this INavigateVM model, IErrorHandler error)
        {
            model.BackCommand = new Command(async item => await model.ProcessCommandAsync(model.BackAction), item => true, error);
        }
        //public static void CreateBackCommand<T>(this INavigateVM<T> model, IErrorHandler error)
        //{
        //    model.BackCommand = new Command<T>(async item => await model.ProcessCommandAsync(model.BackAction), item => true, error);
        //}
        public static void CreateTwoCommands(this INavigateVM model, Command? command, Func<Task>? action, IErrorHandler error)
        {
            model.CreateBackCommand(error);
            command = new Command(async item => await model.ProcessCommandAsync(action), item => true, error);
        }
        public static void CreateTwoCommands<T>(this INavigateVM model, Command<T>? command, Func<T, Task>? action, IErrorHandler error)
        {
            model.CreateBackCommand(error);
            command = new Command<T>(async item => await model.ProcessCommandAsync(action, item), item => true, error);
        }
    }
}