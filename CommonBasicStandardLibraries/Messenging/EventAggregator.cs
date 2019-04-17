using System;
using System.Text;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using System.Linq;
using CommonBasicStandardLibraries.BasicDataSettingsAndProcesses;
using static CommonBasicStandardLibraries.BasicDataSettingsAndProcesses.BasicDataFunctions;
using CommonBasicStandardLibraries.CollectionClasses;
using System.Threading.Tasks; //most of the time, i will be using asyncs.
using fs = CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.FileHelpers;
using js = CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.JsonSerializers.NewtonJsonStrings; //just in case i need those 2.
using System.Collections.Generic;
using System.Reflection;
//i think this is the most common things i like to do
namespace CommonBasicStandardLibraries.Messenging
{
    public enum EnumActionCategory
    {
        None, Async, Regular
    }
    public class EventAggregator : IEventAggregator
    {

        private readonly CustomBasicList<Handler> _handlers = new CustomBasicList<Handler>();
        //done.
        public bool HandlerExistsFor(Type messageType, string arguments = "", EnumActionCategory Action = EnumActionCategory.Regular)
        {
            lock (_handlers)
            {
                return _handlers.Any(handler => handler.Handles(messageType, Action) & !handler.IsDead && handler.Tag == arguments);
            }
        }

        public void Publish(object message, string arguments = "", bool AllowDuplicates = true)
        {
            CustomBasicList<Handler> toNotify;
            var messageType = message.GetType();
            lock (_handlers)
            {
                toNotify = _handlers.Where(Items => Items.Tag == arguments).ToCustomBasicList();
            }
            CustomBasicList<CustomMethod> Results = new CustomBasicList<CustomMethod>();
            toNotify.ForEach(Items =>
            {
                Results.AddRange(Items.GetItemsToInvoke(messageType, EnumActionCategory.Regular));
            });
            if (AllowDuplicates == false && Results.Count > 1)
                throw new BasicBlankException($"Duplicates not allowed For PublishAsync.  Message Was {messageType.ToString()} and tag was {message}");

            Results.ForEach(Items =>
            {
                Items.Method.Invoke(Items.Object, new[] { message });
            });
            ProcessDeadOnes(toNotify);
        }

        public async Task PublishAsync(object message, string arguments = "", bool AllowDuplicates = true)
        {
            CustomBasicList<Handler> toNotify;
            var messageType = message.GetType();
            lock (_handlers)
            {
                toNotify = _handlers.Where(Items => Items.Tag == arguments).ToCustomBasicList();
            }
            CustomBasicList<CustomMethod> Results = new CustomBasicList<CustomMethod>();
            toNotify.ForEach(Items =>
            {
                Results.AddRange(Items.GetItemsToInvoke(messageType, EnumActionCategory.Async));
            });
            if (AllowDuplicates == false && Results.Count > 1)
                throw new BasicBlankException($"Duplicates not allowed For PublishAsync.  Message Was {messageType.ToString()} and tag was {message}");

            CustomBasicList<Task> TaskList = new CustomBasicList<Task>();
            Results.ForEach(Items =>
            {
                TaskList.Add((Task)Items.Method.Invoke(Items.Object, new[] { message }));
            });
            await Task.WhenAll(TaskList);
            ProcessDeadOnes(toNotify);
        }

        private void ProcessDeadOnes(CustomBasicList<Handler> TempHandler)
        {
            CustomBasicList<Handler> DeadList = TempHandler.Where(Items => Items.IsDead == true).ToCustomBasicList();
            if (DeadList.Count == 0)
                return;
            lock (_handlers)
            {
                DeadList.ForEach(Items =>
                {
                    _handlers.RemoveSpecificItem(Items); //because its dead
                });
            }

        }

        public void Subscribe(object subscriber, string Tags = "") //done i think
        {
            if (subscriber == null)
                throw new ArgumentNullException(nameof(subscriber));


            lock (_handlers)
            {
                if (_handlers.Any(x => x.Matches(subscriber, Tags)))
                    return;

                _handlers.Add(new Handler(subscriber, Tags));
            }
        }
        //done
        public void Unsubscribe(object subscriber, string Tags = "")
        {
            if (subscriber == null)
                throw new ArgumentNullException(nameof(subscriber));

            lock (_handlers)
            {
                var found = _handlers.FirstOrDefault(x => x.Matches(subscriber, Tags));

                if (found != null)
                    _handlers.RemoveSpecificItem(found);
            }
        }


        private class CustomType
        {
            public Type Type;
            //public string Tag;
            public EnumActionCategory ThisAction;
        }
        private class CustomMethod
        {
            public MethodInfo Method;
            public object Object;
            public CustomMethod(MethodInfo ThisMethod, object ThisObject)
            {
                Method = ThisMethod;
                Object = ThisObject;
            }
        }

        private class Handler
        {
            private readonly WeakReference _reference;
            public string Tag;
            private readonly Dictionary<CustomType, MethodInfo> _supportedHandlers = new Dictionary<CustomType, MethodInfo>();

            public Handler(object handler, string Tag)
            {
                _reference = new WeakReference(handler);
                this.Tag = Tag;
                var interfaces = handler.GetType().GetTypeInfo().ImplementedInterfaces
                    .Where(x => x.GetTypeInfo().IsGenericType && x.GetGenericTypeDefinition() == typeof(IHandle<>));
                foreach (var @interface in interfaces)
                {
                    var type = @interface.GetTypeInfo().GenericTypeArguments[0];
                    var method = @interface.GetRuntimeMethod("Handle", new[] { type });
                    if (method != null)
                    {
                        CustomType ThisCustom = new CustomType();
                        ThisCustom.Type = type;
                        ThisCustom.ThisAction = EnumActionCategory.Regular;
                        _supportedHandlers[ThisCustom] = method;
                    }
                    else
                        throw new BasicBlankException("No Handle Found");
                }
                interfaces = handler.GetType().GetTypeInfo().ImplementedInterfaces
                    .Where(x => x.GetTypeInfo().IsGenericType && x.GetGenericTypeDefinition() == typeof(IHandleAsync<>));
                foreach (var @interface in interfaces)
                {
                    var type = @interface.GetTypeInfo().GenericTypeArguments[0];
                    var method = @interface.GetRuntimeMethod("HandleAsync", new[] { type });

                    if (method != null)
                    {
                        CustomType ThisCustom = new CustomType();
                        ThisCustom.Type = type;
                        ThisCustom.ThisAction = EnumActionCategory.Async;
                        _supportedHandlers[ThisCustom] = method;
                    }
                    else
                        throw new BasicBlankException("No Handle Found");
                }
            }

            public bool IsDead => _reference.Target == null;

            public bool Matches(object instance, string Tag)
            {

                return _reference.Target == instance && this.Tag == Tag;
            }

            public CustomBasicList<CustomMethod> GetItemsToInvoke(Type messageType, EnumActionCategory ThisAction)
            {
                var target = _reference.Target;
                if (target == null)
                    return new CustomBasicList<CustomMethod>();

                //if (_supportedHandlers.Count > 1)
                //{
                //    var FirstItem = _supportedHandlers.First();

                //}

                return _supportedHandlers.Where(handler => handler.Key.ThisAction == ThisAction &&
                handler.Key.Type.IsAssignableFrom(messageType))
                .Select(Temps => new CustomMethod(Temps.Value, target)).ToCustomBasicList();
            }



            public bool Handles(Type messageType, EnumActionCategory ThisAction)
            {
                return _supportedHandlers.Any(pair =>
                pair.Key.ThisAction == ThisAction && pair.Key.Type
                .GetTypeInfo().IsAssignableFrom(messageType.GetTypeInfo()));
            }
        }

    }
}
