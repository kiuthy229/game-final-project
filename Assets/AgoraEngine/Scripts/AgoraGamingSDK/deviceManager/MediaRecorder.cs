using System.Runtime.InteropServices;
using AOT;

namespace agora_gaming_rtc
{
    public abstract class IMediaRecorder : IRtcEngineNative
    {
        /**
        * Starts recording the local audio and video.
        *
        * @since v3.5.2
        *
        * After successfully getting the object, you can call this method to enable the recording of the local audio and video.
        *
        * This method can record the following content:
        * - The audio captured by the local microphone and encoded in AAC format.
        * - The video captured by the local camera and encoded by the SDK.
        *
        * The SDK can generate a recording file only when it detects the recordable audio and video streams; when there are
        * no audio and video streams to be recorded or the audio and video streams are interrupted for more than five
        * seconds, the SDK stops recording and triggers the
        * \ref IMediaRecorderObserver::onRecorderStateChanged "onRecorderStateChanged" (RECORDER_STATE_ERROR, RECORDER_ERROR_NO_STREAM)
        * callback.
        *
        * @note Call this method after joining the channel.
        *
        * @param config The recording configurations. See MediaRecorderConfiguration.
        *
        * @return
        * - 0(ERR_OK): Success.
        * - < 0: Failure:
        *    - `-2(ERR_INVALID_ARGUMENT)`: The parameter is invalid. Ensure the following:
        *      - The specified path of the recording file exists and is writable.
        *      - The specified format of the recording file is supported.
        *      - The maximum recording duration is correctly set.
        *    - `-4(ERR_NOT_SUPPORTED)`: IRtcEngine does not support the request due to one of the following reasons:
        *      - The recording is ongoing.
        *      - The recording stops because an error occurs.
        *    - `-7(ERR_NOT_INITIALIZED)`: This method is called before the initialization of IRtcEngine. Ensure that you have
        * called \ref IMediaRecorder::getMediaRecorder "getMediaRecorder" before calling `startRecording`.
        */
        public abstract int startRecording(MediaRecorderConfiguration config);
        /**
        * Stops recording the local audio and video.
        *
        * @since v3.5.2
        *
        * @note Call this method after calling \ref IMediaRecorder::startRecording "startRecording".
        *
        * @return
        * - 0(ERR_OK): Success.
        * - < 0: Failure:
        *  - `-7(ERR_NOT_INITIALIZED)`: This method is called before the initialization of IRtcEngine. Ensure that you have
        * called \ref IMediaRecorder::getMediaRecorder "getMediaRecorder" before calling `stopRecording`.
        */
        public abstract new int stopRecording();

        public abstract void initMediaRecorderObserver();
    }

    public sealed class MediaRecorder : IMediaRecorder
    {
        private static IRtcEngine mEngine = null;
        private static MediaRecorder _mediaRecorderInstance;

        public delegate void OnRecorderStateChangedHandler(RecorderState state, RecorderErrorCode error);
        public OnRecorderStateChangedHandler _OnRecorderStateChanged;
        public delegate void OnRecorderInfoUpdatedHandler(RecorderInfo info);
        public OnRecorderInfoUpdatedHandler _OnRecorderInfoUpdated;

        private MediaRecorder(IRtcEngine rtcEngine)
        {
            mEngine = rtcEngine;
            createMediaRecording();
        }

        ~MediaRecorder()
        {

        }

        public static MediaRecorder GetInstance(IRtcEngine rtcEngine)
        {
            if (_mediaRecorderInstance == null)
            {
                _mediaRecorderInstance = new MediaRecorder(rtcEngine);
            }
            return _mediaRecorderInstance;
        }

        public static void ReleaseInstance()
        {
            IRtcEngineNative.releaseMediaRecorder();
            _mediaRecorderInstance = null;
        }

        // used internally
        public void SetEngine(IRtcEngine engine)
        {
            mEngine = engine;
        }

        private int createMediaRecording()
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
            return IRtcEngineNative.createMediaRecorder();
        }

        public override int startRecording(MediaRecorderConfiguration config)
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
            return IRtcEngineNative.startRecording(config.storagePath, (int)config.containerFormat, (int)config.streamType, config.maxDurationMs, config.recorderInfoUpdateInterval);
        }

        public override int stopRecording()
        {
            if (mEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;
            return IRtcEngineNative.stopRecording();
        }

        public override void initMediaRecorderObserver()
        {
            if (mEngine == null)
                return;
            IRtcEngineNative.initEventOnMediaRecorderCallback(OnRecorderStateChangedCallback, OnRecorderInfoUpdatedCallback);
        }

        [MonoPInvokeCallback(typeof(EngineEventOnRecorderStateChanged))]
        private static void OnRecorderStateChangedCallback(int state, int error)
        {
            if (mEngine != null && _mediaRecorderInstance != null)
            {
                _mediaRecorderInstance._OnRecorderStateChanged((RecorderState)state, (RecorderErrorCode)error);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnRecorderInfoUpdated))]
        private static void OnRecorderInfoUpdatedCallback(string fileName, uint durationMs, uint fileSize)
        {
            if (mEngine != null && _mediaRecorderInstance != null)
            {
                RecorderInfo info;
                info.fileName = fileName;
                info.durationMs = durationMs;
                info.fileSize = fileSize;
                _mediaRecorderInstance._OnRecorderInfoUpdated(info);
            }
        }
    }
}