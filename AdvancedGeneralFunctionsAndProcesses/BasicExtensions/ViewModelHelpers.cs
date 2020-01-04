using CommonBasicStandardLibraries.Exceptions;
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
        //public static Command CreateTwoCommands(this INavigateVM model, Func<Task>? action, IErrorHandler error)
        //{
        //    model.CreateBackCommand(error);
        //    return new Command(async item => await model.ProcessCommandAsync(action), item => false, error);
        //}
        //public static void CreateTwoCommands<T>(this IActionVM model, Func<T, Task>? action, IErrorHandler error)
        //{
        //    model.CreateBackCommand(error);
        //    model.ActionCommand = new Command<T>(async item => await model.ProcessCommandAsync(action, item), item => true, error);
        //}
        //public static Command<T> CreateTwoCommands<T>(this INavigateVM model, ActionContainer<T> action, IErrorHandler error)
        //{
        //    model.CreateBackCommand(error);
        //    return new Command<T>(async item =>
        //    {
        //        await model.ProcessCommandAsync(action.Action, item);
        //        //if (action.Action == null)
        //        //    throw new BasicBlankException("Help");
        //        //throw new BasicBlankException("Still working partially");
        //    }, item => true, error);

        //    //return new Command(async item => await model.ProcessCommandAsync(
        //    //    {
        //    //}action: action!.Action, (T) item), item => true, error);
        //    //return new Command(async item =>
        //    //{
        //    //    if (action == null)
        //    //        throw new BasicBlankException("Help");
        //    //}, item => true, error);
        //    //return  new Command(async item => await model.ProcessCommandAsync(action, (T) item), item => true, error);
        //}
        //for now, we won't have both the generic and regular.  if i change my mind, has to think about naming.
        public static Command CreateTwoCommands<T>(this INavigateVM model, ActionContainer<T> action, IErrorHandler error)
        {
            model.CreateBackCommand(error);
            return model.CreateExtraCommand(action, error);
            

            //return new Command(async item => await model.ProcessCommandAsync(
            //    {
            //}action: action!.Action, (T) item), item => true, error);
            //return new Command(async item =>
            //{
            //    if (action == null)
            //        throw new BasicBlankException("Help");
            //}, item => true, error);
            //return  new Command(async item => await model.ProcessCommandAsync(action, (T) item), item => true, error);
        }
        public static Command CreateExtraCommand<T>(this INavigateVM model, ActionContainer<T> action, IErrorHandler error)
        {
            return new Command(async item =>
            {
                await model.ProcessCommandAsync(action.Action, (T)item);
                //if (action.Action == null)
                //    throw new BasicBlankException("Help");
                //throw new BasicBlankException("Still working partially");
            }, item => true, error);
        }
        public static Command CreateExtraCommand(this INavigateVM model, ActionContainer action, IErrorHandler error)
        {
            //i may have extra things that has to happen in order to be valid.  if that is so, then rethinking will be required.
            return new Command(async item =>
            {
                await model.ProcessCommandAsync(action.Action);
            }, item => true, error);
        }
        //if one has more than one extra command, then rethinking could be required.
        //including an extra actioncontainer.  in that case, you still call the extra command


        //for now, not working out as expected unfortunately.
        //if i named it the same way, then bad news is harder to maintain because the name would have to be the same.
        //there could be 2 actions.  that would still not be a good solution.
        //if i really wanted to do this, i would have to have another class.
        //should try this on the hotel first.

    }
}