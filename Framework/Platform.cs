﻿using System.Runtime.InteropServices;

namespace Foster.Framework;

internal static class Platform
{
	public const string DLL = "FosterPlatform";

	[Flags]
	public enum FosterFlags
	{
		FULLSCREEN    = 1 << 0,
		VSYNC         = 1 << 1,
		RESIZABLE     = 1 << 2,
		MOUSE_VISIBLE = 1 << 3,
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void FosterLogFn(IntPtr msg);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void FosterExitRequestFn();

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void FosterOnTextFn(IntPtr cstr);
	
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void FosterOnKeyFn(int key, bool pressed);
	
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void FosterOnMouseButtonFn(int button, bool pressed);
	
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void FosterOnMouseWheelFn(float offsetX, float offsetY);
	
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void FosterOnControllerConnectFn(int index, IntPtr name, int buttonCount, int axisCount, bool isGamepad, ushort vendor, ushort product, ushort version);
	
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void FosterOnControllerDisconnectFn(int index);
	
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void FosterOnControllerButtonFn(int index, int button, bool pressed);
	
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void FosterOnControllerAxisFn(int index, int axis, float value);
	
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void FosterWriteFn(IntPtr context, IntPtr data, int size);

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct FosterDesc
	{
		public string windowTitle;
		public string applicationName;
		public int width;
		public int height;
		public Renderers renderer;
		public FosterFlags flags;
		public FosterLogFn onLogInfo;
		public FosterLogFn onLogWarn;
		public FosterLogFn onLogError;
		public FosterExitRequestFn onExitRequest;
		public FosterOnTextFn onText;
		public FosterOnKeyFn onKey;
		public FosterOnMouseButtonFn onMouseButton;
		public FosterOnMouseWheelFn onMouseWheel;
		public FosterOnControllerConnectFn onControllerConnect;
		public FosterOnControllerDisconnectFn onControllerDisconnect;
		public FosterOnControllerButtonFn onControllerButton;
		public FosterOnControllerAxisFn onControllerAxis;
		public int logging;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct FosterVertexElement
	{
		public int index;
		public VertexType type;
		public int normalized;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct FosterVertexFormat
	{
		public IntPtr elements;
		public int elementCount;
		public int stride;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct FosterUniformInfo
	{
		public int index;
		public IntPtr name;
		public UniformType type;
		public int arrayElements;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct FosterShaderData
	{
		public string vertex;
		public string fragment;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct FosterRect
	{
		public int x, y, w, h;

		public FosterRect(int x, int y, int w, int h) { this.x = x; this.y = y; this.w = w; this.h = h; }
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct FosterDrawCommand
	{
		public IntPtr target;
		public IntPtr mesh;
		public IntPtr shader;
		public int hasViewport;
		public int hasScissor;
		public FosterRect viewport;
		public FosterRect scissor;
		public int indexStart;
		public int indexCount;
		public int instanceCount;
		public DepthCompare compare;
		public CullMode cull;
		public BlendMode blend;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
	public struct FosterClearCommand
	{
		public IntPtr target;
		public FosterRect clip;
		public Color color;
		public int depth;
		public int stencil;
		public ClearMask mask;
	}

	public static unsafe string ParseUTF8(IntPtr s)
	{
		byte* ptr = (byte*) s;
		while (*ptr != 0)
			ptr++;
		return System.Text.Encoding.UTF8.GetString((byte*)s, (int)(ptr - (byte*)s));
	}

	[DllImport(DLL)]
	public static extern void FosterStartup(FosterDesc desc);
	[DllImport(DLL)]
	public static extern void FosterBeginFrame();
	[DllImport(DLL)]
	public static extern void FosterPollEvents();
	[DllImport(DLL)]
	public static extern void FosterEndFrame();
	[DllImport(DLL)]
	public static extern void FosterShutdown();
	[DllImport(DLL)]
	public static extern bool FosterIsRunning();
	[DllImport(DLL)]
	public static extern void FosterSetTitle(string title);
	[DllImport(DLL)]
	public static extern void FosterSetSize(int width, int height);
	[DllImport(DLL)]
	public static extern void FosterGetSize(out int width, out int height);
	[DllImport(DLL)]
	public static extern void FosterGetSizeInPixels(out int width, out int height);
	[DllImport(DLL)]
	public static extern void FosterSetFlags(FosterFlags flags);
	[DllImport(DLL)]
	public static extern IntPtr FosterGetUserPath();
	[DllImport(DLL)]
	public static extern void FosterSetClipboard(string ptr);
	[DllImport(DLL)]
	public static extern IntPtr FosterGetClipboard();
	[DllImport(DLL)]
	public static extern IntPtr FosterImageLoad(IntPtr memory, int length, out int w, out int h);
	[DllImport(DLL)]
	public static extern void FosterImageFree(IntPtr data);
	[DllImport(DLL)]
	public static extern bool FosterImageWrite(FosterWriteFn func, IntPtr context, int w, int h, IntPtr data);
	[DllImport(DLL)]
	public static extern unsafe Renderers FosterGetRenderer();
	[DllImport(DLL)]
	public static extern IntPtr FosterTextureCreate(int width, int height, TextureFormat format);
	[DllImport(DLL)]
	public static extern void FosterTextureSetData(IntPtr texture, IntPtr data, int length);
	[DllImport(DLL)]
	public static extern void FosterTextureGetData(IntPtr texture, IntPtr data, int length);
	[DllImport(DLL)]
	public static extern void FosterTextureDestroy(IntPtr texture);
	[DllImport(DLL)]
	public static extern IntPtr FosterTargetCreate(int width, int height, TextureFormat[] formats, int formatCount);
	[DllImport(DLL)]
	public static extern IntPtr FosterTargetGetAttachment(IntPtr target, int index);
	[DllImport(DLL)]
	public static extern void FosterTargetDestroy(IntPtr target);
	[DllImport(DLL)]
	public static extern IntPtr FosterShaderCreate(ref FosterShaderData data);
	[DllImport(DLL)]
	public static extern unsafe void FosterShaderGetUniforms(IntPtr shader, FosterUniformInfo* output, out int count, int max);
	[DllImport(DLL)]
	public static extern unsafe void FosterShaderSetUniform(IntPtr shader, int index, float* values);
	[DllImport(DLL)]
	public static extern unsafe void FosterShaderSetTexture(IntPtr shader, int index, IntPtr* values);
	[DllImport(DLL)]
	public static extern unsafe void FosterShaderSetSampler(IntPtr shader, int index, TextureSampler* values);
	[DllImport(DLL)]
	public static extern void FosterShaderDestroy(IntPtr shader);
	[DllImport(DLL)]
	public static extern IntPtr FosterMeshCreate();
	[DllImport(DLL)]
	public static extern void FosterMeshSetVertexFormat(IntPtr mesh, ref FosterVertexFormat format);
	[DllImport(DLL)]
	public static extern void FosterMeshSetVertexData(IntPtr mesh, IntPtr data, int dataSize);
	[DllImport(DLL)]
	public static extern void FosterMeshSetIndexFormat(IntPtr mesh, IndexFormat format);
	[DllImport(DLL)]
	public static extern void FosterMeshSetIndexData(IntPtr mesh, IntPtr data, int dataSize);
	[DllImport(DLL)]
	public static extern void FosterMeshDestroy(IntPtr mesh);
	[DllImport(DLL)]
	public static extern void FosterDraw(ref FosterDrawCommand command);
	[DllImport(DLL)]
	public static extern void FosterClear(ref FosterClearCommand command);
}