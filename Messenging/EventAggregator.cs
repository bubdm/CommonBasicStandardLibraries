using CommonBasicStandardLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks; //most of the time, i will be using asyncs.
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
        public bool HandlerExistsFor(Type messageType, string arguments = "", EnumActionCategory action = EnumActionCategory.Regular)
        {
            lock (_handlers)
            {
                return _handlers.Any(handler => handler.Handles(messageType, action) & !handler.IsDead && handler.Tag == arguments);
            }
        }
        public void Publish(object message, string arguments = "", bool allowDuplicates = true)
        {
            CustomBasicList<Handler> toNotify;
            var messageType = message.GetType();
            lock (_handlers)
            {
                toNotify = _handlers.Where(Items => Items.Tag == arguments).ToCustomBasicList();
            }
            CustomBasicList<CustomMethod> results = new CustomBasicList<CustomMethod>();
            toNotify.ForEach(items =>
            {
                results.AddRange(items.GetItemsToInvoke(messageType, EnumActionCategory.Regular));
            });
            if (allowDuplicates == false && results.Count > 1)
                throw new BasicBlankException($"Duplicates not allowed For PublishAsync.  Message Was {messageType.ToString()} and tag was {message}");

            results.ForEach(items =>
            {
                items.Method.Invoke(items.Object, new[] { message });
            });
            ProcessDeadOnes(toNotify);
        }
        public async Task PublishAsync(object message, string arguments = "", bool allowDuplicates = true)
        {
            CustomBasicList<Handler> toNotify;
            var messageType = message.GetType();
            lock (_handlers)
            {
                toNotify = _handlers.Where(Items => Items.Tag == arguments).ToCustomBasicList();
            }
            CustomBasicList<CustomMethod> results = new CustomBasicList<CustomMethod>();
            toNotify.ForEach(items =>
            {
                results.AddRange(items.GetItemsToInvoke(messageType, EnumActionCategory.Async));
            });
            if (allowDuplicates == false && results.Count > 1)
                throw new BasicBlankException($"Duplicates not allowed For PublishAsync.  Message Was {messageType.ToString()} and tag was {message}");

            CustomBasicList<Task> taskList = new CustomBasicList<Task>();
            results.ForEach(items =>
            {
                taskList.Add((Task)items.Method.Invoke(items.Object, new[] { message }));
            });
            await Task.WhenAll(taskList);
            ProcessDeadOnes(toNotify);
        }
        private void ProcessDeadOnes(CustomBasicList<Handler> tempHandler)
        {
            CustomBasicList<Handler> deadList = tempHandler.Where(Items => Items.IsDead == true).ToCustomBasicList();
            if (deadList.Count == 0)
                return;
            lock (_handlers)
            {
                deadList.ForEach(items =>
                {
                    _handlers.RemoveSpecificItem(items); //because its dead
                });
            }
        }
        public void Subscribe(object subscriber, string tags = "") //done i think
        {
            if (subscriber == null)
                throw new ArgumentNullException(nameof(subscriber));
            lock (_handlers)
            {
                if (_handlers.Any(x => x.Matches(subscriber, tags)))
                    return;
                _handlers.Add(new Handler(subscriber, tags));
            }
        }
        public void Unsubscribe(object subscriber, string tags = "")
        {
            if (subscriber == null)
                throw new ArgumentNullException(nameof(subscriber));

            lock (_handlers)
            {
                var found = _handlers.FirstOrDefault(x => x.Matches(subscriber, tags));
                if (found != null)
                    _handlers.RemoveSpecificItem(found);
            }
        }

        public void ClearSubscriptions(object parent)
        {
            lock(_handlers)
            {
                _handlers.Clear();
                _handlers.Add(new Handler(parent, ""));
                //problem.
                //the bootstrapper needs to handle clearing out to resubscribe again.
                //probably do via delegates.
            }
        }

        private class CustomType
        {
            public Type? Type;
            public EnumActionCategory ThisAction;
        }
        private class CustomMethod
        {
            public MethodInfo Method;
            public object Object;
            public CustomMethod(MethodInfo thisMethod, object thisObject)
            {
                Method = thisMethod;
                Object = thisObject;
            }
        }

        private class Handler
        {
            private readonly WeakReference _reference;
            public string Tag;
            private readonly Dictionary<CustomType, MethodInfo> _supportedHandlers = new Dictionary<CustomType, MethodInfo>();
            public Handler(object handler, string tag)
            {
                _reference = new WeakReference(handler);
                Tag = tag;
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
                    {
                        throw new BasicBlankException("No Handle Found");
                    }
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
                    {
                        throw new BasicBlankException("No Handle Found");
                    }
                }
            }
            public bool IsDead => _reference.Target == null;
            public bool Matches(object instance, string tag)
            {
                return _reference.Target == instance && this.Tag == tag;
            }
            public CustomBasicList<CustomMethod> GetItemsToInvoke(Type messageType, EnumActionCategory thisAction)
            {
                var target = _reference.Target;
                if (target == null)
                    return new CustomBasicList<CustomMethod>();
                return _supportedHandlers.Where(handler => handler.Key.ThisAction == thisAction &&
                handler.Key.Type!.IsAssignableFrom(messageType))
                .Select(Temps => new CustomMethod(Temps.Value, target)).ToCustomBasicList();
            }
            public bool Handles(Type messageType, EnumActionCategory thisAction)
            {
                return _supportedHandlers.Any(pair =>
                pair.Key.ThisAction == thisAction && pair.Key.Type
                .GetTypeInfo().IsAssignableFrom(messageType.GetTypeInfo()));
            }
        }
    }
}