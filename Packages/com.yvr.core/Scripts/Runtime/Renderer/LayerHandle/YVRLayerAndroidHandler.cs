using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using Object = System.Object;

namespace YVR.Core
{
    public class YVRLayerAndroidHandler : IYVRLayerHandle
    {
        private enum CallbackType
        {
            PrepareCreate,
            PrepareDestroy,
            PrepareAdd,
            PrepareRemove,
            Created,
            Destroyed,
            Added,
            Removed,
            PreSubmit,
            DepthSet,
        }

        [DllImport("yvrplugin")]
        private static extern void yvrPrepareHandleLayerAsync(IntPtr containerPtr, Action<IntPtr> onPrepareHandleLayerCallback);

        [DllImport("yvrplugin")]
        private static extern void yvrAddActiveLayer(int layerId);

        [DllImport("yvrplugin")]
        private static extern void yvrAddActiveLayerAsync(int layerId, IntPtr containerPtr,
            Action<IntPtr> onLayerAddedCallback);

        [DllImport("yvrplugin")]
        private static extern void yvrRemoveActiveLayer(int layerId);

        [DllImport("yvrplugin")]
        private static extern void yvrRemoveActiveLayerAsync(int layerId, IntPtr containerPtr,
            Action<IntPtr> onLayerRemovedCallback);

        [DllImport("yvrplugin")]
        private static extern int yvrCreateRenderLayer(YVRLayerCreateInfo createInfo);

        [DllImport("yvrplugin")]
        private static extern void yvrCreateRenderLayerAsync(YVRLayerCreateInfo createInfo, IntPtr containerPtr,
            Action<IntPtr, int> onLayerCreatedCallback);

        [DllImport("yvrplugin")]
        private static extern void yvrDestroyRenderLayer(int layerId, bool destroyImmediate);

        [DllImport("yvrplugin")]
        private static extern void yvrDestroyRenderLayerAsync(int layerId, bool destroyImmediate, IntPtr containerPtr,
            Action<IntPtr> onLayerCreatedCallback);

        [DllImport("yvrplugin")]
        private static extern void yvrSetLayerPose(int layerId, in XRPose layerPose);

        [DllImport("yvrplugin")]
        private static extern void yvrSetLayerSize(int layerId, in XRSize size);

        [DllImport("yvrplugin")]
        private static extern void yvrSetEquirectParam(int layerId, float radius);

        [DllImport("yvrplugin")]
        private static extern void yvrSetEquirect2Param(int layerId, float radius, float centralHorizontalAngle, float upperVerticalAngle, float lowerVerticalAngle);

        [DllImport("yvrplugin")]
        private static extern void yvrSetCylinderParam(int layerId, float radius, float centralAngle, float aspectRatio);

        [DllImport("yvrplugin")]
        private static extern void yvrSetLayerSettings(int layerId, bool enableSuperSample, bool expensiveSuperSample, bool enableSharpen, bool expensiveSharpen);

        [DllImport("yvrplugin")]
        private static extern void yvrSwapLayerBuffer(int layerId);

        [DllImport("yvrplugin")]
        private static extern void yvrSetLayerPreSubmitCallback(IntPtr containerPtr,
            Action<IntPtr> onLayerPreSubmitCallback);

        [DllImport("yvrplugin")]
        private static extern int yvrGetLayerColorHandle(int layerId, int index);

        [DllImport("yvrplugin")]
        private static extern int yvrGetEyeBufferLayerId();

        [DllImport("yvrplugin")]
        private static extern int yvrGetLayersCount();

        [DllImport("yvrplugin")]
        private static extern void yvrGetAllLayersColorHandle(int[] layersHandle);

        [DllImport("yvrplugin")]
        private static extern int yvrSetLayerDepthAsync(int id, int depth, IntPtr containerPtr,
            Action<IntPtr, bool> onLayerDepthSettedCallback);

        private GCHandle m_ContainerGCHandle = default;

        private GCHandle containerGCHandle
        {
            get
            {
                if (!m_ContainerGCHandle.IsAllocated) m_ContainerGCHandle = GCHandle.Alloc(this);
                return m_ContainerGCHandle;
            }
        }

        private readonly object m_LockObject = new object();

        private Queue<Action> m_OnPrepareCreateLayerCallbackQueue = new Queue<Action>();
        private Queue<Action> m_OnPrepareDestroyLayerCallbackQueue = new Queue<Action>();
        private Queue<Action> m_OnPrepareAddCallbackQueue = new Queue<Action>();
        private Queue<Action> m_OnPrepareRemoveCallbackQueue = new Queue<Action>();
        private Queue<Action> m_OnLayerPreSubmitCallbackQueue = new Queue<Action>();
        private Queue<Action<int>> m_OnLayerCreatedCallbackQueue = new Queue<Action<int>>();
        private Queue<Action> m_OnLayerDestroyedCallbackQueue = new Queue<Action>();
        private Queue<Action> m_OnLayerAddedCallbackQueue = new Queue<Action>();
        private Queue<Action> m_OnLayerRemovedCallbackQueue = new Queue<Action>();
        private Queue<Action<bool>> m_OnLayerDepthSettedCallbackQueue = new Queue<Action<bool>>();


        public void CreateLayerAsync(YVRLayerCreateInfo layerCreateInfo, Action<int> onLayerCreated = null)
        {
            RegisterCallback(CallbackType.Created, onLayerCreated);

            yvrCreateRenderLayerAsync(layerCreateInfo, GCHandle.ToIntPtr(containerGCHandle), OnLayerCreated);
        }

        public int CreateLayer(YVRLayerCreateInfo layerCreateInfo)
        {
            return yvrCreateRenderLayer(layerCreateInfo);
        }

        public void PrepareCreateLayerAsync(Action onPrepareCreateLayer = null)
        {
            RegisterCallback(CallbackType.PrepareCreate, onPrepareCreateLayer);

            yvrPrepareHandleLayerAsync(GCHandle.ToIntPtr(containerGCHandle), OnPrepareLayerCreate);
        }

        public void DestroyLayerAsync(int layerId, bool destroyImmediate, Action onLayerDestroyed = null)
        {
            RegisterCallback(CallbackType.Destroyed, onLayerDestroyed);
            yvrDestroyRenderLayerAsync(layerId, destroyImmediate, GCHandle.ToIntPtr(containerGCHandle), OnLayerDestroyed);
        }

        public void DestroyLayer(int layer, bool destroyImmediate)
        {
            yvrDestroyRenderLayer(layer, destroyImmediate);
        }

        public void PrepareDestroyLayerAsync(Action onPrepareDestroyLayer = null)
        {
            RegisterCallback(CallbackType.PrepareDestroy, onPrepareDestroyLayer);
            yvrPrepareHandleLayerAsync(GCHandle.ToIntPtr(containerGCHandle), OnPrepareLayerDestroy);
        }

        public void AddLayerAsync(int layerId, Action onLayerAdded = null)
        {
            RegisterCallback(CallbackType.Added, onLayerAdded);
            yvrAddActiveLayerAsync(layerId, GCHandle.ToIntPtr(containerGCHandle), OnLayerAdded);
        }

        public void AddLayer(int layerId)
        {
            yvrAddActiveLayer(layerId);
        }

        public void PrepareAddLayer(Action onPrepareAddLayer = null)
        {
            RegisterCallback(CallbackType.PrepareAdd, onPrepareAddLayer);
            yvrPrepareHandleLayerAsync(GCHandle.ToIntPtr(containerGCHandle), OnPrepareLayerAdd);
        }

        public void RemoveLayerAsync(int layerId, Action onLayerRemoved = null)
        {
            RegisterCallback(CallbackType.Removed, onLayerRemoved);
            yvrRemoveActiveLayerAsync(layerId, GCHandle.ToIntPtr(containerGCHandle), OnLayerRemoved);
        }

        public void RemoveLayer(int layerId)
        {
            yvrRemoveActiveLayer(layerId);
        }

        public void PrepareRemoveLayer(Action onPrepareRemoveLayer = null)
        {
            RegisterCallback(CallbackType.PrepareRemove, onPrepareRemoveLayer);
            yvrPrepareHandleLayerAsync(GCHandle.ToIntPtr(containerGCHandle), OnPrepareLayerRemove);
        }

        public void SetLayerPreSubmitCallback(Action onLayerPreSubmit)
        {
            RegisterCallback(CallbackType.PreSubmit, onLayerPreSubmit);
            yvrSetLayerPreSubmitCallback(GCHandle.ToIntPtr(containerGCHandle), OnLayerPreSubmit);
        }

        public void SetLayerPose(in int layerId, in XRPose pose) { yvrSetLayerPose(layerId, pose); }
        public void SetLayerSize(in int layerId, in XRSize size) { yvrSetLayerSize(layerId, size); }

        public void SetLayerCylinderParam(in int layerId, in float radius, in float centralAngle, in float aspectRatio) { yvrSetCylinderParam(layerId, radius, centralAngle, aspectRatio); }

        public void SetLayerEquirectParam(in int layerId, in float radius)
        {
            yvrSetEquirectParam(layerId, radius);
        }

        public void SetLayerEquirect2Param(in int layerId, in float radius, in float centralHorizontalAngle, in float upperVerticalAngle, in float lowerVerticalAngle)
        {
            yvrSetEquirect2Param(layerId, radius, centralHorizontalAngle, upperVerticalAngle, lowerVerticalAngle);
        }

        public void SetLayerSettings(int layerId, bool enableSuperSample, bool expensiveSuperSample, bool enableSharpen, bool expensiveSharpen) { yvrSetLayerSettings(layerId, enableSuperSample, expensiveSuperSample, enableSharpen, expensiveSharpen); }

        public void SetLayerDepthAsync(int layerId, int depth, Action<bool> onLayerDepthSet = null)
        {
            RegisterCallback(CallbackType.DepthSet, onLayerDepthSet);
            yvrSetLayerDepthAsync(layerId, depth, GCHandle.ToIntPtr(containerGCHandle), OnLayerDepthSetted);
        }

        public void SwapBufferLayer(int layerId) { yvrSwapLayerBuffer(layerId); }

        public int GetEyeBufferLayerId() { return yvrGetEyeBufferLayerId(); }

        public int GetLayerColorHandle(int layerId, int index) { return yvrGetLayerColorHandle(layerId, index); }
        public int GetLayerColorHandle(int layerId) { return yvrGetLayerColorHandle(layerId, -1); }

        public int GetLayersCount() { return yvrGetLayersCount(); }

        public int[] GetAlLayersColorHandle()
        {
            int count = yvrGetLayersCount();
            int[] layersColorHandle = new int[count];
            yvrGetAllLayersColorHandle(layersColorHandle);

            return layersColorHandle;
        }

        [MonoPInvokeCallback(typeof(Action<IntPtr>))]
        private static void OnPrepareLayerCreate(IntPtr containerPtr)
        {
            YVRLayerAndroidHandler handler = GCHandle.FromIntPtr(containerPtr).Target as YVRLayerAndroidHandler;
            handler?.TriggerAndUnRegisterCallback(CallbackType.PrepareCreate);
        }

        [MonoPInvokeCallback(typeof(Action<IntPtr>))]
        private static void OnPrepareLayerDestroy(IntPtr containerPtr)
        {
            YVRLayerAndroidHandler handler = GCHandle.FromIntPtr(containerPtr).Target as YVRLayerAndroidHandler;
            handler?.TriggerAndUnRegisterCallback(CallbackType.PrepareDestroy);
        }

        [MonoPInvokeCallback(typeof(Action<IntPtr>))]
        private static void OnPrepareLayerAdd(IntPtr containerPtr)
        {
            YVRLayerAndroidHandler handler = GCHandle.FromIntPtr(containerPtr).Target as YVRLayerAndroidHandler;
            handler?.TriggerAndUnRegisterCallback(CallbackType.PrepareAdd);
        }

        [MonoPInvokeCallback(typeof(Action<IntPtr>))]
        private static void OnPrepareLayerRemove(IntPtr containerPtr)
        {
            YVRLayerAndroidHandler handler = GCHandle.FromIntPtr(containerPtr).Target as YVRLayerAndroidHandler;
            handler?.TriggerAndUnRegisterCallback(CallbackType.PrepareRemove);
        }

        [MonoPInvokeCallback(typeof(Action<IntPtr, int>))]
        private static void OnLayerCreated(IntPtr containerPtr, int layerId)
        {
            YVRLayerAndroidHandler handler = GCHandle.FromIntPtr(containerPtr).Target as YVRLayerAndroidHandler;
            handler?.TriggerAndUnRegisterCallback(CallbackType.Created, layerId);
        }

        [MonoPInvokeCallback(typeof(Action<IntPtr>))]
        private static void OnLayerDestroyed(IntPtr containerPtr)
        {
            YVRLayerAndroidHandler handler = GCHandle.FromIntPtr(containerPtr).Target as YVRLayerAndroidHandler;
            handler?.TriggerAndUnRegisterCallback(CallbackType.Destroyed);
        }

        [MonoPInvokeCallback(typeof(Action<IntPtr>))]
        private static void OnLayerPreSubmit(IntPtr containerPtr)
        {
            YVRLayerAndroidHandler handler = GCHandle.FromIntPtr(containerPtr).Target as YVRLayerAndroidHandler;
            handler?.TriggerAndUnRegisterCallback(CallbackType.PreSubmit);
        }

        [MonoPInvokeCallback(typeof(Action<IntPtr>))]
        private static void OnLayerAdded(IntPtr containerPtr)
        {
            YVRLayerAndroidHandler handler = GCHandle.FromIntPtr(containerPtr).Target as YVRLayerAndroidHandler;
            handler?.TriggerAndUnRegisterCallback(CallbackType.Added);
        }

        [MonoPInvokeCallback(typeof(Action<IntPtr>))]
        private static void OnLayerRemoved(IntPtr containerPtr)
        {
            YVRLayerAndroidHandler handler = GCHandle.FromIntPtr(containerPtr).Target as YVRLayerAndroidHandler;
            handler?.TriggerAndUnRegisterCallback(CallbackType.Removed);
        }

        [MonoPInvokeCallback(typeof(Action<IntPtr, int>))]
        private static void OnLayerDepthSetted(IntPtr containerPtr, bool result)
        {
            YVRLayerAndroidHandler handler = GCHandle.FromIntPtr(containerPtr).Target as YVRLayerAndroidHandler;
            handler?.TriggerAndUnRegisterCallback(CallbackType.DepthSet, result);
        }

        private void RegisterCallback(CallbackType callbackType, MulticastDelegate callback)
        {
            lock (m_LockObject)
            {
                switch (callbackType)
                {
                    case CallbackType.PrepareCreate:
                        m_OnPrepareCreateLayerCallbackQueue.Enqueue((Action)callback);
                        break;
                    case CallbackType.PrepareDestroy:
                        m_OnPrepareDestroyLayerCallbackQueue.Enqueue((Action)callback);
                        break;
                    case CallbackType.PrepareAdd:
                        m_OnPrepareAddCallbackQueue.Enqueue((Action)callback);
                        break;
                    case CallbackType.PrepareRemove:
                        m_OnPrepareRemoveCallbackQueue.Enqueue((Action)callback);
                        break;
                    case CallbackType.Created:
                        m_OnLayerCreatedCallbackQueue.Enqueue((Action<int>) callback);
                        break;
                    case CallbackType.Destroyed:
                        m_OnLayerDestroyedCallbackQueue.Enqueue((Action) callback);
                        break;
                    case CallbackType.PreSubmit:
                        m_OnLayerPreSubmitCallbackQueue.Enqueue((Action) callback);
                        break;
                    case CallbackType.Added:
                        m_OnLayerAddedCallbackQueue.Enqueue((Action) callback);
                        break;
                    case CallbackType.Removed:
                        m_OnLayerRemovedCallbackQueue.Enqueue((Action) callback);
                        break;
                    case CallbackType.DepthSet:
                        m_OnLayerDepthSettedCallbackQueue.Enqueue((Action<bool>) callback);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(callbackType), callbackType, null);
                }
            }
        }

        private void TriggerAndUnRegisterCallback(CallbackType callbackType, Object data = null)
        {
            lock (m_LockObject)
            {
                try
                {
                    switch (callbackType)
                    {
                        case CallbackType.PrepareCreate when m_OnPrepareCreateLayerCallbackQueue.Count != 0:
                            m_OnPrepareCreateLayerCallbackQueue.Dequeue()?.Invoke();
                            break;
                        case CallbackType.PrepareDestroy when m_OnPrepareDestroyLayerCallbackQueue.Count != 0:
                            m_OnPrepareDestroyLayerCallbackQueue.Dequeue()?.Invoke();
                            break;
                        case CallbackType.PrepareAdd when m_OnPrepareAddCallbackQueue.Count != 0:
                            m_OnPrepareAddCallbackQueue.Dequeue()?.Invoke();
                            break;
                        case CallbackType.PrepareRemove when m_OnPrepareRemoveCallbackQueue.Count != 0:
                            m_OnPrepareRemoveCallbackQueue.Dequeue()?.Invoke();
                            break;
                        case CallbackType.Created when m_OnLayerCreatedCallbackQueue.Count != 0:
                            m_OnLayerCreatedCallbackQueue.Dequeue()?.Invoke((int) data);
                            break;
                        case CallbackType.Destroyed when m_OnLayerDestroyedCallbackQueue.Count != 0:
                            m_OnLayerDestroyedCallbackQueue.Dequeue()?.Invoke();
                            break;
                        case CallbackType.PreSubmit when m_OnLayerPreSubmitCallbackQueue.Count != 0:
                            m_OnLayerPreSubmitCallbackQueue.Dequeue()?.Invoke();
                            break;
                        case CallbackType.Added when m_OnLayerAddedCallbackQueue.Count != 0:
                            m_OnLayerAddedCallbackQueue.Dequeue()?.Invoke();
                            break;
                        case CallbackType.Removed when m_OnLayerRemovedCallbackQueue.Count != 0:
                            m_OnLayerRemovedCallbackQueue.Dequeue()?.Invoke();
                            break;
                        case CallbackType.DepthSet when m_OnLayerDepthSettedCallbackQueue.Count != 0:
                            m_OnLayerDepthSettedCallbackQueue.Dequeue()?.Invoke((bool) data);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(callbackType), callbackType, null);
                    }
                } catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        ~YVRLayerAndroidHandler()
        {
            if (m_ContainerGCHandle.IsAllocated)
                m_ContainerGCHandle.Free();
        }
    }
}