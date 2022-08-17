using System;
using System.Runtime.InteropServices;
using AOT;

#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX 
namespace agora_gaming_rtc
{
	public abstract class IScreenCaptureManager : IRtcEngineNative
	{ 
		public abstract void CreateScreenCaptureManager(int thumbHeight, int thumbWidth, int iconHeight, int iconWidth, bool includeScreen);

	 	public abstract int GetScreenCaptureSourcesCount();

		public abstract ScreenCaptureSourceType GetScreenCaptureSourceType(uint index);

        public abstract string GetScreenCaptureSourceName(uint index);

        public abstract string GetScreenCaptureSourceProcessPath(uint index);

        public abstract string GetScreenCaptureSourceTitle(uint index);

        public abstract IntPtr GetScreenCaptureSourceId(uint index);

		public abstract bool GetScreenCaptureIsPrimaryMonitor(uint index);

		public abstract ThumbImageBuffer GetScreenCaptureThumbImage(uint index);

		public abstract ThumbImageBuffer GetScreenCaptureIconImage(uint index);
	}

    /** The definition of ScreenCaptureManager. */
	public sealed class ScreenCaptureManager : IScreenCaptureManager
    {
		
		private IRtcEngine mEngine = null;
		private static ScreenCaptureManager _screenCaptureManagerInstance;

		private ScreenCaptureManager(IRtcEngine rtcEngine)
		{
			mEngine = rtcEngine;
		}

		~ScreenCaptureManager() 
		{

		}

		public static ScreenCaptureManager GetInstance(IRtcEngine rtcEngine)
		{
			if (_screenCaptureManagerInstance == null)
			{
				_screenCaptureManagerInstance = new ScreenCaptureManager(rtcEngine);
			}
			return _screenCaptureManagerInstance;
		}

     	public static void ReleaseInstance()
		{
			_screenCaptureManagerInstance = null;
		}

		// used internally
		public void SetEngine (IRtcEngine engine)
		{
			mEngine = engine;
		}

		public override void CreateScreenCaptureManager(int thumbHeight, int thumbWidth, int iconHeight, int iconWidth, bool includeScreen)
		{
			if (mEngine == null) return;

			IRtcEngineNative.getScreenCaptureSources(thumbHeight, thumbWidth, iconHeight, iconWidth, includeScreen);
		}

        public override int GetScreenCaptureSourcesCount()
        {
            if (mEngine == null)
				return -1;

			return IRtcEngineNative.getScreenCaptureSourcesCount();
        }

		public override ScreenCaptureSourceType GetScreenCaptureSourceType(uint index)
		{
			if (mEngine == null)
				return ScreenCaptureSourceType.ScreenCaptureSourceType_Unknown;
			if (index >= 0 && index < GetScreenCaptureSourcesCount())
			{
				return (ScreenCaptureSourceType) IRtcEngineNative.getScreenCaptureSourceType(index);
			}
			return ScreenCaptureSourceType.ScreenCaptureSourceType_Unknown;
		}

        public override string GetScreenCaptureSourceName(uint index)
		{
			if (mEngine == null)
				return "Engine is null";
			if (index >= 0 && index < GetScreenCaptureSourcesCount())
			{
				return Marshal.PtrToStringAnsi(IRtcEngineNative.getScreenCaptureSourceName(index));
			}
			return "Invalid Argument";
		}

        public override string GetScreenCaptureSourceProcessPath(uint index)
		{
			if (mEngine == null)
				return "Engine is null";
			if (index >= 0 && index < GetScreenCaptureSourcesCount())
			{
				return Marshal.PtrToStringAnsi(IRtcEngineNative.getScreenCaptureSourceProcessPath(index));
			}
			return "Invalid Argument";
		}

        public override string GetScreenCaptureSourceTitle(uint index)
		{
			if (mEngine == null)
				return "Engine is null";
			if (index >= 0 && index < GetScreenCaptureSourcesCount())
			{
				return Marshal.PtrToStringAnsi(IRtcEngineNative.getScreenCaptureSourceTitle(index));
			}
			return "Invalid Argument";
		}

        public override IntPtr GetScreenCaptureSourceId(uint index)
		{
			if (mEngine == null)
				return IntPtr.Zero;
			if (index >= 0 && index < GetScreenCaptureSourcesCount())
			{
				return IRtcEngineNative.getScreenCaptureSourceId(index);
			}
			return IntPtr.Zero;
		}

		public override bool GetScreenCaptureIsPrimaryMonitor(uint index)
		{
			if (mEngine == null)
				return false;
			if (index >= 0 && index < GetScreenCaptureSourcesCount())
			{
				return IRtcEngineNative.getScreenCaptureIsPrimaryMonitor(index);
			}
			return false;
		}

		public override ThumbImageBuffer GetScreenCaptureThumbImage(uint index)
		{
			if (mEngine == null)
				return new ThumbImageBuffer();
			if (index >= 0 && index < GetScreenCaptureSourcesCount())
			{
				ThumbImageBuffer buffer = new ThumbImageBuffer();
				IRtcEngineNative.getScreenCaptureThumbImage(index, ref buffer);
				return buffer;
			}
			return new ThumbImageBuffer();
		}

		public override ThumbImageBuffer GetScreenCaptureIconImage(uint index)
		{
			if (mEngine == null)
				return new ThumbImageBuffer();
			if (index >= 0 && index < GetScreenCaptureSourcesCount())
			{
				ThumbImageBuffer buffer = new ThumbImageBuffer();
				IRtcEngineNative.getScreenCaptureIconImage(index, ref buffer);
				return buffer;
			}
			return new ThumbImageBuffer();
		}
	}
}
#endif