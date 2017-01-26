// Amplify Occlusion - Robust Ambient Occlusion for Unity Pro
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[AddComponentMenu( "" )]
public class AmplifyOcclusionBase : MonoBehaviour
{
	public enum ApplicationMethod
	{
		PostEffect = 0,
		Deferred,
		Debug
	}

	public enum PerPixelNormalSource
	{
		None = 0,
		Camera,
		GBuffer,
		GBufferOctaEncoded,
	}

	public enum SampleCountLevel
	{
		Low = 0,
		Medium,
		High,
		VeryHigh
	}

	[Header( "Ambient Occlusion" )]
	public ApplicationMethod ApplyMethod = ApplicationMethod.PostEffect;
	public SampleCountLevel SampleCount = SampleCountLevel.Medium;
	public PerPixelNormalSource PerPixelNormals = PerPixelNormalSource.Camera;
	[Range( 0, 1 )] public float Intensity = 1.0f;
	public Color Tint = Color.black;
	[Range( 0, 16 )] public float Radius = 1.0f;
	[Range( 0, 16 )] public float PowerExponent = 1.8f;
	[Range( 0, 0.99f )]	public float Bias = 0.05f;
	public bool CacheAware = false;
	public bool Downsample = false;

	[Header( "Bilateral Blur" )]
	public bool BlurEnabled = true;
	[Range( 1, 4 )]	public int BlurRadius = 2;
	[Range( 1, 4 )]	public int BlurPasses = 1;
	[Range( 0, 20 )] public float BlurSharpness = 10.0f;

	private const int PerPixelNormalSourceCount = 4;

	private int prevScreenWidth = 0;
	private int prevScreenHeight = 0;
	private bool prevHDR = false;
	private ApplicationMethod prevApplyMethod;
	private SampleCountLevel prevSampleCount;
	private PerPixelNormalSource prevPerPixelNormals;
	private bool prevCacheAware;
	private bool prevDownscale;
	private bool prevBlurEnabled;
	private int prevBlurRadius;
	private int prevBlurPasses;

	private Camera m_camera;
	private Material m_occlusionMat;
	private Material m_blurMat;
	private Material m_copyMat;
	private Texture2D m_randomTex;

	private const int RandomSize = 4;
	private const int DirectionCount = 8;
	private Color[] m_randomData;

	private string[] m_layerOffsetNames = null;
	private string[] m_layerRandomNames = null;

	private string[] m_layerDepthNames = null;
	private string[] m_layerNormalNames = null;
	private string[] m_layerOcclusionNames = null;

	private RenderTexture m_occlusionRT = null;
	private int[] m_depthLayerRT = null;
	private int[] m_normalLayerRT = null;
	private int[] m_occlusionLayerRT = null;

	private int m_mrtCount = 0;
	private RenderTargetIdentifier[] m_depthTargets = null;
	private RenderTargetIdentifier[] m_normalTargets = null;
	private int m_deinterleaveDepthPass = 0;
	private int m_deinterleaveNormalPass = 0;

	private RenderTargetIdentifier[] m_applyDeferredTargets = null;

	private Mesh m_blitMesh = null;
	private TargetDesc m_target = new TargetDesc();
	private Dictionary<CameraEvent,CommandBuffer> m_registeredCommandBuffers = new Dictionary<CameraEvent,CommandBuffer>();

#if TRIAL
	private Texture2D watermark = null;
#endif

	private static class ShaderPass
	{
		public const int FullDepth = 0;
		public const int FullNormal_None = 1;
		public const int FullNormal_Camera = 2;
		public const int FullNormal_GBuffer = 3;
		public const int FullNormal_GBufferOctaEncoded = 4;

		public const int DeinterleaveDepth1 = 5;
		public const int DeinterleaveNormal1_None = 6;
		public const int DeinterleaveNormal1_Camera = 7;
		public const int DeinterleaveNormal1_GBuffer = 8;
		public const int DeinterleaveNormal1_GBufferOctaEncoded = 9;

		public const int DeinterleaveDepth4 = 10;
		public const int DeinterleaveNormal4_None = 11;
		public const int DeinterleaveNormal4_Camera = 12;
		public const int DeinterleaveNormal4_GBuffer = 13;
		public const int DeinterleaveNormal4_GBufferOctaEncoded = 14;

		public const int OcclusionCache_Low = 15;
		public const int OcclusionCache_Medium = 16;
		public const int OcclusionCache_High = 17;
		public const int OcclusionCache_VeryHigh = 18;

		public const int Reinterleave = 19;

		public const int OcclusionLow_None = 20;
		public const int OcclusionLow_Camera = 21;
		public const int OcclusionLow_GBuffer = 22;
		public const int OcclusionLow_GBufferOctaEncoded = 23;

		public const int OcclusionMedium_None = 24;
		public const int OcclusionMedium_Camera = 25;
		public const int OcclusionMedium_GBuffer = 26;
		public const int OcclusionMedium_GBufferOctaEncoded = 27;

		public const int OcclusionHigh_None = 28;
		public const int OcclusionHigh_Camera = 29;
		public const int OcclusionHigh_GBuffer = 30;
		public const int OcclusionHigh_GBufferOctaEncoded = 31;

		public const int OcclusionVeryHigh_None = 32;
		public const int OcclusionVeryHigh_Camera = 33;
		public const int OcclusionVeryHigh_GBuffer = 34;
		public const int OcclusionVeryHigh_GBufferNormalEncoded = 35;

		public const int ApplyDebug = 36;
		public const int ApplyDeferred = 37;
		public const int ApplyDeferredLog = 38;
		public const int ApplyPostEffect = 39;
		public const int ApplyPostEffectLog = 40;

		public const int CombineDownsampledOcclusionDepth = 41;

		public const int BlurHorizontal1 = 0;
		public const int BlurVertical1 = 1;
		public const int BlurHorizontal2 = 2;
		public const int BlurVertical2 = 3;
		public const int BlurHorizontal3 = 4;
		public const int BlurVertical3 = 5;
		public const int BlurHorizontal4 = 6;
		public const int BlurVertical4 = 7;

		public const int Copy = 0;
	}

	private struct TargetDesc
	{
		public int fullWidth;
		public int fullHeight;
		public RenderTextureFormat format;
		public int width;
		public int height;
		public int quarterWidth;
		public int quarterHeight;
		public float padRatioWidth;
		public float padRatioHeight;
	}

	bool CheckParamsChanged()
	{
		bool changed =
			( prevScreenWidth != m_camera.pixelWidth ) ||
			( prevScreenHeight != m_camera.pixelHeight ) ||
			( prevHDR != m_camera.hdr ) ||
			( prevApplyMethod != ApplyMethod ) ||
			( prevSampleCount != SampleCount ) ||
			( prevPerPixelNormals != PerPixelNormals ) ||
			( prevCacheAware != CacheAware ) ||
			( prevDownscale != Downsample ) ||
			( prevBlurEnabled != BlurEnabled ) ||
			( prevBlurRadius != BlurRadius ) ||
			( prevBlurPasses != BlurPasses );
		return changed;
	}

	void UpdateParams()
	{
		prevScreenWidth = m_camera.pixelWidth;
		prevScreenHeight = m_camera.pixelHeight;
		prevHDR = m_camera.hdr;
		prevApplyMethod = ApplyMethod;
		prevSampleCount = SampleCount;
		prevPerPixelNormals = PerPixelNormals;
		prevCacheAware = CacheAware;
		prevDownscale = Downsample;
		prevBlurEnabled = BlurEnabled;
		prevBlurRadius = BlurRadius;
		prevBlurPasses = BlurPasses;
	}

	void Warmup()
	{
		CheckMaterial();
		CheckRandomData();

		m_depthLayerRT = new int[ 16 ];
		m_normalLayerRT = new int[ 16 ];
		m_occlusionLayerRT = new int[ 16 ];

		m_mrtCount = Mathf.Min( SystemInfo.supportedRenderTargetCount, 4 );

		m_layerOffsetNames = new string[ m_mrtCount ];
		m_layerRandomNames = new string[ m_mrtCount ];

		for ( int i = 0; i < m_mrtCount; i++ )
		{
			m_layerOffsetNames[ i ] = "_AO_LayerOffset" + i;
			m_layerRandomNames[ i ] = "_AO_LayerRandom" + i;
		}

		m_layerDepthNames = new string[ 16 ];
		m_layerNormalNames = new string[ 16 ];
		m_layerOcclusionNames = new string[ 16 ];

		for ( int i = 0; i < 16; i++ )
		{
			m_layerDepthNames[ i ] = "_AO_DepthLayer" + i;
			m_layerNormalNames[ i ] = "_AO_NormalLayer" + i;
			m_layerOcclusionNames[ i ] = "_AO_OcclusionLayer" + i;
		}

		m_depthTargets = new RenderTargetIdentifier[ m_mrtCount ];
		m_normalTargets = new RenderTargetIdentifier[ m_mrtCount ];

		switch ( m_mrtCount )
		{
		case 4:
			m_deinterleaveDepthPass = ShaderPass.DeinterleaveDepth4;
			m_deinterleaveNormalPass = ShaderPass.DeinterleaveNormal4_None;
			break;
		default:
			m_deinterleaveDepthPass = ShaderPass.DeinterleaveDepth1;
			m_deinterleaveNormalPass = ShaderPass.DeinterleaveNormal1_None;
			break;
		}

		m_applyDeferredTargets = new RenderTargetIdentifier[ 2 ];

		if ( m_blitMesh != null )
			DestroyImmediate( m_blitMesh );

		m_blitMesh = new Mesh();
		m_blitMesh.vertices = new Vector3[ 4 ] { new Vector3( 0, 0, 0 ), new Vector3( 0, 1, 0 ), new Vector3( 1, 1, 0 ), new Vector3( 1, 0, 0 ) };
		m_blitMesh.uv = new Vector2[ 4 ] { new Vector2( 0, 0 ), new Vector2( 0, 1 ), new Vector2( 1, 1 ), new Vector2( 1, 0 ) };
		m_blitMesh.triangles = new int[ 6 ] { 0, 1, 2, 0, 2, 3 };
	}

	void Shutdown()
	{
		CommandBuffer_UnregisterAll();
		SafeReleaseRT( ref m_occlusionRT );

		if ( m_occlusionMat != null )
			DestroyImmediate( m_occlusionMat );
		if ( m_blurMat != null )
			DestroyImmediate( m_blurMat );
		if ( m_copyMat != null )
			DestroyImmediate( m_copyMat );
		if ( m_randomTex != null )
			DestroyImmediate( m_randomTex );
		if ( m_blitMesh != null )
			DestroyImmediate( m_blitMesh );
	}

	void OnEnable()
	{
		m_camera = GetComponent<Camera>();

		Warmup();
		CommandBuffer_UnregisterAll();

	#if TRIAL
		watermark = new Texture2D( 4, 4 ) { hideFlags = HideFlags.HideAndDontSave };
		watermark.LoadImage( AmplifyOcclusion.Watermark.ImageData );
	#endif
	}

	void OnDisable()
	{
		Shutdown();

	#if TRIAL
		if ( watermark != null )
		{
			DestroyImmediate( watermark );
			watermark = null;
		}
	#endif
	}

	void OnDestroy()
	{
		Shutdown();
	}

	void Update()
	{
		if ( m_camera.actualRenderingPath != RenderingPath.DeferredShading )
		{
			if ( PerPixelNormals != PerPixelNormalSource.None && PerPixelNormals != PerPixelNormalSource.Camera )
			{
				// NOTE: use inspector warning box instead?
				PerPixelNormals = PerPixelNormalSource.Camera;
				UnityEngine.Debug.LogWarning( "[AmplifyOcclusion] GBuffer Normals only available in Camera Deferred Shading mode. Switched to Camera source." );
			}

			if ( ApplyMethod == ApplicationMethod.Deferred )
			{
				// NOTE: use inspector warning box instead?
				ApplyMethod = ApplicationMethod.PostEffect;
				UnityEngine.Debug.LogWarning( "[AmplifyOcclusion] Deferred Method requires a Deferred Shading path. Switching to Post Effect Method." );
			}
		}

		if ( ApplyMethod == ApplicationMethod.Deferred && PerPixelNormals == PerPixelNormalSource.Camera )
		{
			// NOTE: use inspector warning box instead?
			PerPixelNormals = PerPixelNormalSource.GBuffer;
			UnityEngine.Debug.LogWarning( "[AmplifyOcclusion] Camera Normals not supported for Deferred Method. Switching to GBuffer Normals." );
		}

		if ( ( m_camera.depthTextureMode & DepthTextureMode.Depth ) == 0 )
			m_camera.depthTextureMode |= DepthTextureMode.Depth;

		if ( ( PerPixelNormals == PerPixelNormalSource.Camera ) && ( m_camera.depthTextureMode & DepthTextureMode.DepthNormals ) == 0 )
			m_camera.depthTextureMode |= DepthTextureMode.DepthNormals;

		CheckMaterial();
		CheckRandomData();
	}

	void CheckMaterial()
	{
		if ( m_occlusionMat == null )
			m_occlusionMat = new Material( Shader.Find( "Hidden/Amplify Occlusion/Occlusion" ) ) { hideFlags = HideFlags.DontSave };
		if ( m_blurMat == null )
			m_blurMat = new Material( Shader.Find( "Hidden/Amplify Occlusion/Blur" ) ) { hideFlags = HideFlags.DontSave };
		if ( m_copyMat == null )
			m_copyMat = new Material( Shader.Find( "Hidden/Amplify Occlusion/Copy" ) ) { hideFlags = HideFlags.DontSave };
	}

	void CheckRandomData()
	{
		if ( m_randomData == null )
			m_randomData = GenerateRandomizationData();

		if ( m_randomTex == null )
			m_randomTex = GenerateRandomizationTexture( m_randomData );
	}

	static public Color[] GenerateRandomizationData()
	{
		Color[] randomPixels = new Color[ RandomSize * RandomSize ];

		for ( int i = 0, j = 0; i < RandomSize * RandomSize; i++ )
		{
			float r1 = RandomTable.Values[ j++ ];
			float r2 = RandomTable.Values[ j++ ];
			float angle = 2.0f * Mathf.PI * r1 / DirectionCount;


			randomPixels[ i ].r = Mathf.Cos( angle );
			randomPixels[ i ].g = Mathf.Sin( angle );
			randomPixels[ i ].b = r2;
			randomPixels[ i ].a = 0;
		}
		return randomPixels;
	}

	static public Texture2D GenerateRandomizationTexture( Color[] randomPixels )
	{
		Texture2D tex = new Texture2D( RandomSize, RandomSize, TextureFormat.ARGB32, false, true ) { hideFlags = HideFlags.DontSave };
		tex.name = "RandomTexture";
		tex.filterMode = FilterMode.Point;
		tex.wrapMode = TextureWrapMode.Repeat;
		tex.SetPixels( randomPixels );
		tex.Apply();
		return tex;
	}

	RenderTexture SafeAllocateRT( string name, int width, int height, RenderTextureFormat format, RenderTextureReadWrite readWrite )
	{
        width = Mathf.Max( width, 1 );
		height = Mathf.Max( height, 1 );

        RenderTexture rt = new RenderTexture( width, height, 0, format, readWrite ) { hideFlags = HideFlags.DontSave };
		rt.name = name;
		rt.filterMode = FilterMode.Point;
		rt.wrapMode = TextureWrapMode.Clamp;
		rt.Create();
		return rt;
	}

	void SafeReleaseRT( ref RenderTexture rt )
	{
		if ( rt != null )
		{
			RenderTexture.active = null;
			rt.Release();
			RenderTexture.DestroyImmediate( rt );
			rt = null;
		}
	}

	int SafeAllocateTemporaryRT( CommandBuffer cb, string propertyName, int width, int height, RenderTextureFormat format = RenderTextureFormat.Default,
		RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default, FilterMode filterMode = FilterMode.Point )
	{
		int id = Shader.PropertyToID( propertyName );
		cb.GetTemporaryRT( id, width, height, 0, filterMode, format, readWrite );
		return id;
	}

	void SafeReleaseTemporaryRT( CommandBuffer cb, int id )
	{cb.ReleaseTemporaryRT( id );
	}

	void SetBlitTarget( CommandBuffer cb, RenderTargetIdentifier[] targets, int targetWidth, int targetHeight )
	{
		cb.SetGlobalVector( "_AO_Target_TexelSize", new Vector4( 1.0f / targetWidth, 1.0f / targetHeight, targetWidth, targetHeight ) );
		cb.SetGlobalVector( "_AO_Target_Position", Vector2.zero );
		cb.SetRenderTarget( targets, targets[ 0 ] );
	}

	void SetBlitTarget( CommandBuffer cb, RenderTargetIdentifier target, int targetWidth, int targetHeight )
	{
		cb.SetGlobalVector( "_AO_Target_TexelSize", new Vector4( 1.0f / targetWidth, 1.0f / targetHeight, targetWidth, targetHeight ) );
		cb.SetRenderTarget( target );
	}

	void PerformBlit( CommandBuffer cb, Material mat, int pass )
	{
		cb.DrawMesh( m_blitMesh, Matrix4x4.identity, mat, 0, pass );
	}

	void PerformBlit( CommandBuffer cb, Material mat, int pass, int x, int y )
	{
		cb.SetGlobalVector( "_AO_Target_Position", new Vector2( x, y ) );
		PerformBlit( cb, mat, pass );
	}

	void PerformBlit( CommandBuffer cb, RenderTargetIdentifier source, int sourceWidth, int sourceHeight, Material mat, int pass )
	{
		cb.SetGlobalTexture( "_AO_Source", source );
		cb.SetGlobalVector( "_AO_Source_TexelSize", new Vector4( 1.0f / sourceWidth, 1.0f / sourceHeight, sourceWidth, sourceHeight ) );
		PerformBlit( cb, mat, pass );
	}

	void PerformBlit( CommandBuffer cb, RenderTargetIdentifier source, int sourceWidth, int sourceHeight, Material mat, int pass, int x, int y )
	{
		cb.SetGlobalVector( "_AO_Target_Position", new Vector2( x, y ) );
		PerformBlit( cb, source, sourceWidth, sourceHeight, mat, pass );
	}

	CommandBuffer CommandBuffer_Allocate( string name )
	{
		CommandBuffer cb = new CommandBuffer();
		cb.name = name;
		return cb;
	}

	void CommandBuffer_Register( CameraEvent cameraEvent, CommandBuffer commandBuffer )
	{
		m_camera.AddCommandBuffer( cameraEvent, commandBuffer );
		m_registeredCommandBuffers.Add( cameraEvent, commandBuffer );
	}

	void CommandBuffer_Unregister( CameraEvent cameraEvent, CommandBuffer commandBuffer )
	{
		if ( m_camera != null )
		{
			CommandBuffer[] cbs = m_camera.GetCommandBuffers( cameraEvent );
			foreach ( CommandBuffer cb in cbs )
			{
				if ( cb.name == commandBuffer.name )
					m_camera.RemoveCommandBuffer( cameraEvent, cb );
			}
		}
	}

	CommandBuffer CommandBuffer_AllocateRegister( CameraEvent cameraEvent )
	{
		string name = "";
		if ( cameraEvent == CameraEvent.BeforeReflections )
			name = "AO-BeforeRefl";
		else if ( cameraEvent == CameraEvent.AfterLighting )
			name = "AO-AfterLighting";
		else if ( cameraEvent == CameraEvent.BeforeImageEffectsOpaque )
			name = "AO-BeforePostOpaque";
		else
			Debug.LogError( "[AmplifyOcclusion] Unsupported CameraEvent. Please contact support." );

		CommandBuffer cb = CommandBuffer_Allocate( name );
		CommandBuffer_Register( cameraEvent, cb );
		return cb;
	}

	void CommandBuffer_UnregisterAll()
	{
		foreach ( KeyValuePair<CameraEvent,CommandBuffer> pair in m_registeredCommandBuffers )
			CommandBuffer_Unregister( pair.Key, pair.Value );
		m_registeredCommandBuffers.Clear();
	}

	void UpdateGlobalShaderConstants( TargetDesc target )
	{
		float fovRad = m_camera.fieldOfView * Mathf.Deg2Rad;
		Vector2 focalLen = new Vector2( 1.0f / Mathf.Tan( fovRad * 0.5f ) * ( target.height / ( float ) target.width ), 1.0f / Mathf.Tan( fovRad * 0.5f ) );
		Vector2 invFocalLen = new Vector2( 1.0f / focalLen.x, 1.0f / focalLen.y );

		float projScale;
		if ( m_camera.orthographic )
			projScale = ( ( float ) target.height ) / m_camera.orthographicSize;
		else
			projScale = ( ( float ) target.height ) / ( Mathf.Tan( fovRad * 0.5f ) * 2.0f );

		float bias = Mathf.Clamp( Bias, 0.0f, 1.0f );

		Shader.SetGlobalMatrix( "_AO_CameraProj", GL.GetGPUProjectionMatrix( Matrix4x4.Ortho( 0, 1, 0, 1, -1, 100 ), false ) );
		Shader.SetGlobalMatrix( "_AO_CameraView", m_camera.worldToCameraMatrix );

		Shader.SetGlobalVector( "_AO_UVToView", new Vector4( 2.0f * invFocalLen.x, -2.0f * invFocalLen.y, -1.0f * invFocalLen.x, 1.0f * invFocalLen.y ) );

		Shader.SetGlobalFloat( "_AO_NegRcpR2", -1.0f / ( Radius * Radius ) );
		Shader.SetGlobalFloat( "_AO_RadiusToScreen", Radius * 0.5f * projScale );

		Shader.SetGlobalFloat( "_AO_PowExponent", PowerExponent );
		Shader.SetGlobalFloat( "_AO_Bias", bias );
		Shader.SetGlobalFloat( "_AO_Multiplier", 1.0f / ( 1.0f - bias ) );
		Shader.SetGlobalFloat( "_AO_BlurSharpness", BlurSharpness );

		Shader.SetGlobalColor( "_AO_Levels", new Color( Tint.r, Tint.g, Tint.b, Intensity ) );
	}

	void CommandBuffer_FillComputeOcclusion( CommandBuffer cb, TargetDesc target )
	{
		CheckMaterial();
		CheckRandomData();

		cb.SetGlobalVector( "_AO_Buffer_PadScale", new Vector4( target.padRatioWidth, target.padRatioHeight, 1.0f / target.padRatioWidth, 1.0f / target.padRatioHeight ) );
		cb.SetGlobalVector( "_AO_Buffer_TexelSize", new Vector4( 1.0f / target.width, 1.0f / target.height, target.width, target.height ) );
		cb.SetGlobalVector( "_AO_QuarterBuffer_TexelSize", new Vector4( 1.0f / target.quarterWidth, 1.0f / target.quarterHeight, target.quarterWidth, target.quarterHeight ) );

		cb.SetGlobalFloat( "_AO_MaxRadiusPixels", Mathf.Min( target.width, target.height ) );

		if ( m_occlusionRT == null || m_occlusionRT.width != target.width || m_occlusionRT.height != target.height || !m_occlusionRT.IsCreated() )
		{
			SafeReleaseRT( ref m_occlusionRT );
			m_occlusionRT = SafeAllocateRT( "_AO_OcclusionTexture", target.width, target.height, RenderTextureFormat.RGHalf, RenderTextureReadWrite.Linear );
		}

		int smallOcclusionRT = -1;
		if ( Downsample )
			smallOcclusionRT = SafeAllocateTemporaryRT( cb, "_AO_SmallOcclusionTexture", target.width / 2, target.height / 2, RenderTextureFormat.RGHalf, RenderTextureReadWrite.Linear, FilterMode.Bilinear );

		// Ambient Occlusion
		if ( CacheAware && !Downsample )
		{
			int occlusionAtlasRT = SafeAllocateTemporaryRT( cb, "_AO_OcclusionAtlas", target.width, target.height, RenderTextureFormat.RGHalf, RenderTextureReadWrite.Linear );
			for ( int i = 0; i < 16; i++ )
			{
				m_depthLayerRT[ i ] = SafeAllocateTemporaryRT( cb, m_layerDepthNames[ i ], target.quarterWidth, target.quarterHeight, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear );
				m_normalLayerRT[ i ] = SafeAllocateTemporaryRT( cb, m_layerNormalNames[ i ], target.quarterWidth, target.quarterHeight, RenderTextureFormat.ARGB2101010, RenderTextureReadWrite.Linear );
				m_occlusionLayerRT[ i ] = SafeAllocateTemporaryRT( cb, m_layerOcclusionNames[ i ], target.quarterWidth, target.quarterHeight, RenderTextureFormat.RGHalf, RenderTextureReadWrite.Linear );
			}

			// Deinterleaved Normal + Depth
			for ( int scan = 0; scan < 16; scan += m_mrtCount )
			{
				for ( int i = 0; i < m_mrtCount; i++ )
				{
					int layer = i + scan;
					int x = layer & 3;
					int y = layer >> 2;

					cb.SetGlobalVector( m_layerOffsetNames[ i ], new Vector2( x + 0.5f, y + 0.5f ) );

					m_depthTargets[ i ] = m_depthLayerRT[ layer ];
					m_normalTargets[ i ] = m_normalLayerRT[ layer ];
				}

				SetBlitTarget( cb, m_depthTargets, target.quarterWidth, target.quarterHeight );
				PerformBlit( cb, m_occlusionMat, m_deinterleaveDepthPass );

				SetBlitTarget( cb, m_normalTargets, target.quarterWidth, target.quarterHeight );
				PerformBlit( cb, m_occlusionMat, m_deinterleaveNormalPass + ( int ) PerPixelNormals );
			}

			// Deinterleaved Occlusion
			for ( int i = 0; i < 16; i++ )
			{
				cb.SetGlobalVector( "_AO_LayerOffset", new Vector2( ( i & 3 ) + 0.5f, ( i >> 2 ) + 0.5f ) );
				cb.SetGlobalVector( "_AO_LayerRandom", m_randomData[ i ] );

				cb.SetGlobalTexture( "_AO_NormalTexture", m_normalLayerRT[ i ] );
				cb.SetGlobalTexture( "_AO_DepthTexture", m_depthLayerRT[ i ] );

				SetBlitTarget( cb, m_occlusionLayerRT[ i ], target.quarterWidth, target.quarterHeight );
				PerformBlit( cb, m_occlusionMat, ShaderPass.OcclusionCache_Low + ( int ) SampleCount );
			}

			// Reinterleave
			SetBlitTarget( cb, occlusionAtlasRT, target.width, target.height );

			for ( int i = 0; i < 16; i++ )
			{
				int dst_x = ( i & 3 ) * target.quarterWidth;
				int dst_y = ( i >> 2 ) * target.quarterHeight;

				PerformBlit( cb, m_occlusionLayerRT[ i ], target.quarterWidth, target.quarterHeight, m_copyMat, ShaderPass.Copy, dst_x, dst_y );
			}

			cb.SetGlobalTexture( "_AO_OcclusionAtlas", occlusionAtlasRT );
			SetBlitTarget( cb, m_occlusionRT, target.width, target.height );
			PerformBlit( cb, m_occlusionMat, ShaderPass.Reinterleave );

			for ( int i = 0; i < 16; i++ )
			{
				SafeReleaseTemporaryRT( cb, m_occlusionLayerRT[ i ] );
				SafeReleaseTemporaryRT( cb, m_normalLayerRT[ i ] );
				SafeReleaseTemporaryRT( cb, m_depthLayerRT[ i ] );
			}

			SafeReleaseTemporaryRT( cb, occlusionAtlasRT );
		}
		else
		{
			m_occlusionMat.SetTexture( "_AO_RandomTexture", m_randomTex );

			int occlusionPass = ( ShaderPass.OcclusionLow_None + ( ( int ) SampleCount ) * PerPixelNormalSourceCount + ( ( int ) PerPixelNormals ) );
			if ( Downsample )
			{
				cb.Blit( (Texture) null, new RenderTargetIdentifier( smallOcclusionRT ), m_occlusionMat, occlusionPass );

				SetBlitTarget( cb, m_occlusionRT, target.width, target.height );
				PerformBlit( cb, smallOcclusionRT, target.width / 2, target.height / 2, m_occlusionMat, ShaderPass.CombineDownsampledOcclusionDepth );
			}
			else
			{
				cb.Blit( (Texture) null, m_occlusionRT, m_occlusionMat, occlusionPass );
			}
		}

		if ( BlurEnabled )
		{
			int tempRT = SafeAllocateTemporaryRT( cb, "_AO_TEMP", target.width, target.height, RenderTextureFormat.RGHalf, RenderTextureReadWrite.Linear );

			// Apply Cross Bilateral Blur
			for ( int i = 0; i < BlurPasses; i++ )
			{
				SetBlitTarget( cb, tempRT, target.width, target.height );
				PerformBlit( cb, m_occlusionRT, target.width, target.height, m_blurMat, ShaderPass.BlurHorizontal1 + ( BlurRadius - 1 ) * 2 );

				SetBlitTarget( cb, m_occlusionRT, target.width, target.height );
				PerformBlit( cb, tempRT, target.width, target.height, m_blurMat, ShaderPass.BlurVertical1 + ( BlurRadius - 1 ) * 2 );
			}

			SafeReleaseTemporaryRT( cb, tempRT );
		}

		if ( Downsample && smallOcclusionRT >= 0 )
			SafeReleaseTemporaryRT( cb, smallOcclusionRT );

		cb.SetRenderTarget( default( RenderTexture ) );
	}

	void CommandBuffer_FillApplyDeferred( CommandBuffer cb, TargetDesc target, bool logTarget )
	{
		cb.SetGlobalTexture( "_AO_OcclusionTexture", m_occlusionRT );

		m_applyDeferredTargets[ 0 ] = BuiltinRenderTextureType.GBuffer0;
		m_applyDeferredTargets[ 1 ] = logTarget ? BuiltinRenderTextureType.GBuffer3 : BuiltinRenderTextureType.CameraTarget;

		if ( !logTarget )
		{
			SetBlitTarget( cb, m_applyDeferredTargets, target.fullWidth, target.fullHeight );
			PerformBlit( cb, m_occlusionMat, ShaderPass.ApplyDeferred );
		}
		else
		{
			int gbufferAlbedoRT = SafeAllocateTemporaryRT( cb, "_AO_GBufferAlbedo", target.fullWidth, target.fullHeight, RenderTextureFormat.ARGB32 );
			int gbufferEmissionRT = SafeAllocateTemporaryRT( cb, "_AO_GBufferEmission", target.fullWidth, target.fullHeight, RenderTextureFormat.ARGB32 );

			cb.Blit( m_applyDeferredTargets[ 0 ], gbufferAlbedoRT );
			cb.Blit( m_applyDeferredTargets[ 1 ], gbufferEmissionRT );

			cb.SetGlobalTexture( "_AO_GBufferAlbedo", gbufferAlbedoRT );
			cb.SetGlobalTexture( "_AO_GBufferEmission", gbufferEmissionRT );

			SetBlitTarget( cb, m_applyDeferredTargets, target.fullWidth, target.fullHeight );
			PerformBlit( cb, m_occlusionMat, ShaderPass.ApplyDeferredLog );

			SafeReleaseTemporaryRT( cb, gbufferAlbedoRT );
			SafeReleaseTemporaryRT( cb, gbufferEmissionRT );
		}

		cb.SetRenderTarget( default( RenderTexture ) );
	}

	void CommandBuffer_FillApplyPostEffect( CommandBuffer cb, TargetDesc target, bool logTarget )
	{
		cb.SetGlobalTexture( "_AO_OcclusionTexture", m_occlusionRT );

		if ( !logTarget )
		{
			SetBlitTarget( cb, BuiltinRenderTextureType.CameraTarget, target.fullWidth, target.fullHeight );
			PerformBlit( cb, m_occlusionMat, ShaderPass.ApplyPostEffect );
		}
		else
		{
			int gbufferEmissionRT = SafeAllocateTemporaryRT( cb, "_AO_GBufferEmission", target.fullWidth, target.fullHeight, RenderTextureFormat.ARGB32 );

			cb.Blit( BuiltinRenderTextureType.GBuffer3, gbufferEmissionRT );

			cb.SetGlobalTexture( "_AO_GBufferEmission", gbufferEmissionRT );

			SetBlitTarget( cb, BuiltinRenderTextureType.GBuffer3, target.fullWidth, target.fullHeight );
			PerformBlit( cb, m_occlusionMat, ShaderPass.ApplyPostEffectLog );

			SafeReleaseTemporaryRT( cb, gbufferEmissionRT );
		}

		cb.SetRenderTarget( default( RenderTexture ) );
	}

	void CommandBuffer_FillApplyDebug( CommandBuffer cb, TargetDesc target )
	{
		cb.SetGlobalTexture( "_AO_OcclusionTexture", m_occlusionRT );

		SetBlitTarget( cb, BuiltinRenderTextureType.CameraTarget, target.fullWidth, target.fullHeight );
		PerformBlit( cb, m_occlusionMat, ShaderPass.ApplyDebug );

		cb.SetRenderTarget( default( RenderTexture ) );
	}

	void CommandBuffer_Rebuild( TargetDesc target )
	{
		bool gbufferSource = ( PerPixelNormals == PerPixelNormalSource.GBuffer || PerPixelNormals == PerPixelNormalSource.GBufferOctaEncoded );

		CommandBuffer cb = null;
		CameraEvent stage = gbufferSource ? CameraEvent.AfterLighting : CameraEvent.BeforeImageEffectsOpaque;

		if ( ApplyMethod == ApplicationMethod.Debug )
		{
			cb = CommandBuffer_AllocateRegister( stage );
			CommandBuffer_FillComputeOcclusion( cb, target );
			CommandBuffer_FillApplyDebug( cb, target );
		}
		else
		{
			bool logTarget = ( !m_camera.hdr && gbufferSource );
			stage = ( ApplyMethod == ApplicationMethod.Deferred ) ? CameraEvent.BeforeReflections : stage;

			cb = CommandBuffer_AllocateRegister( stage );
			CommandBuffer_FillComputeOcclusion( cb, target );

			if ( ApplyMethod == ApplicationMethod.PostEffect )
				CommandBuffer_FillApplyPostEffect( cb, target, logTarget );
			else if ( ApplyMethod == ApplicationMethod.Deferred )
				CommandBuffer_FillApplyDeferred( cb, target, logTarget );
		}
	}

	void OnPreRender()
	{
		m_target.fullWidth = m_camera.pixelWidth;
		m_target.fullHeight = m_camera.pixelHeight;
		m_target.format = m_camera.hdr ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.ARGB32;

		m_target.width = CacheAware ? ( m_target.fullWidth + 3 ) & ~3 : m_target.fullWidth;
		m_target.height = CacheAware ? ( m_target.fullHeight + 3 ) & ~3 : m_target.fullHeight;
		m_target.quarterWidth = m_target.width / 4;
		m_target.quarterHeight = m_target.height / 4;
		m_target.padRatioWidth = m_target.width / ( float ) m_target.fullWidth;
		m_target.padRatioHeight = m_target.height / ( float ) m_target.fullHeight;

		UpdateGlobalShaderConstants( m_target );

		if ( CheckParamsChanged() || m_registeredCommandBuffers.Count == 0 )
		{
			CommandBuffer_UnregisterAll();
			CommandBuffer_Rebuild( m_target );
			UpdateParams();
		}
	}

#if TRIAL
	void OnGUI()
	{
		if ( watermark != null )
			GUI.DrawTexture( new Rect( Screen.width - watermark.width - 15, Screen.height - watermark.height - 12, watermark.width, watermark.height ), watermark );
	}
#endif
}

public class RandomTable
{
	public static float[] Values = new float[]
	{
		0.4639f, 0.3400f, 0.2230f, 0.4684f, 0.3222f, 0.9792f, 0.0317f, 0.9733f, 0.7783f, 0.4561f, 0.2585f, 0.3300f, 0.3873f, 0.3801f, 0.1798f, 0.9107f,
		0.5116f, 0.0929f, 0.1807f, 0.6201f, 0.1013f, 0.5563f, 0.6424f, 0.4420f, 0.2151f, 0.4752f, 0.1573f, 0.5688f, 0.5012f, 0.6292f, 0.6992f, 0.7077f,
		0.5567f, 0.0055f, 0.7083f, 0.5831f, 0.2366f, 0.9923f, 0.9810f, 0.1198f, 0.5108f, 0.5604f, 0.9614f, 0.5578f, 0.5399f, 0.3328f, 0.4178f, 0.9207f,
		0.7307f, 0.0766f, 0.0085f, 0.6601f, 0.4289f, 0.5113f, 0.5878f, 0.9064f, 0.4379f, 0.6203f, 0.0621f, 0.1194f, 0.2356f, 0.7958f, 0.0444f, 0.6173f,
		0.8911f, 0.2631f, 0.2452f, 0.2765f, 0.7869f, 0.0597f, 0.4243f, 0.4333f, 0.0521f, 0.6999f, 0.1394f, 0.4028f, 0.7419f, 0.5579f, 0.1270f, 0.9463f,
		0.2055f, 0.0928f, 0.4229f, 0.7151f, 0.7119f, 0.9260f, 0.3686f, 0.2865f, 0.2414f, 0.8316f, 0.2322f, 0.4786f, 0.3669f, 0.4320f, 0.2684f, 0.6191f,
		0.3917f, 0.0566f, 0.0677f, 0.5090f, 0.9208f, 0.2983f, 0.7010f, 0.0443f, 0.9367f, 0.4859f, 0.2712f, 0.1087f, 0.3258f, 0.6823f, 0.9550f, 0.6581f,
		0.2958f, 0.5625f, 0.8671f, 0.8105f, 0.4879f, 0.8695f, 0.2247f, 0.9626f, 0.6465f, 0.0037f, 0.2288f, 0.2636f, 0.3651f, 0.9583f, 0.6066f, 0.9018f,
		0.7572f, 0.3060f, 0.6331f, 0.4076f, 0.4436f, 0.9799f, 0.9229f, 0.9464f, 0.5940f, 0.6043f, 0.8642f, 0.1875f, 0.8771f, 0.7920f, 0.9548f, 0.9767f,
		0.3505f, 0.8347f, 0.9451f, 0.1558f, 0.4118f, 0.5523f, 0.8554f, 0.7413f, 0.7612f, 0.8962f, 0.7820f, 0.2662f, 0.1288f, 0.6457f, 0.5915f, 0.2473f,
		0.2608f, 0.8119f, 0.6533f, 0.9767f, 0.2215f, 0.9574f, 0.2940f, 0.1590f, 0.8205f, 0.5696f, 0.9343f, 0.4671f, 0.7631f, 0.8357f, 0.2400f, 0.3898f,
		0.9987f, 0.7837f, 0.7580f, 0.6143f, 0.2211f, 0.5024f, 0.9780f, 0.2477f, 0.6195f, 0.6583f, 0.7696f, 0.7684f, 0.3371f, 0.3706f, 0.0847f, 0.5105f,
		0.5949f, 0.9946f, 0.1812f, 0.8681f, 0.3120f, 0.4804f, 0.1773f, 0.3673f, 0.7416f, 0.2029f, 0.2294f, 0.1081f, 0.0986f, 0.0104f, 0.7273f, 0.9422f,
		0.0238f, 0.1106f, 0.9582f, 0.2089f, 0.5846f, 0.4918f, 0.2382f, 0.5915f, 0.2974f, 0.6814f, 0.2150f, 0.5877f, 0.7044f, 0.9789f, 0.9116f, 0.6926f,
		0.4629f, 0.2732f, 0.8028f, 0.6516f, 0.7367f, 0.9862f, 0.4023f, 0.5240f, 0.7404f, 0.7990f, 0.9182f, 0.7053f, 0.4774f, 0.1022f, 0.8099f, 0.8606f,
		0.1182f, 0.0095f, 0.2801f, 0.9484f, 0.0254f, 0.4581f, 0.5126f, 0.0820f, 0.5369f, 0.4725f, 0.8357f, 0.0785f, 0.3579f, 0.7975f, 0.5705f, 0.1627f,
		0.8159f, 0.8741f, 0.9153f, 0.3920f, 0.3663f, 0.7662f, 0.4627f, 0.0876f, 0.4023f, 0.2776f, 0.2941f, 0.3927f, 0.5048f, 0.2634f, 0.5091f, 0.5189f,
		0.7388f, 0.9658f, 0.0038f, 0.9768f, 0.2922f, 0.8371f, 0.5254f, 0.7437f, 0.3590f, 0.0606f, 0.5954f, 0.4831f, 0.9001f, 0.4232f, 0.9819f, 0.1549f,
		0.0855f, 0.6815f, 0.8144f, 0.1059f, 0.9722f, 0.2070f, 0.9946f, 0.9892f, 0.6462f, 0.3302f, 0.4320f, 0.1399f, 0.9086f, 0.2715f, 0.5393f, 0.8451f,
		0.1400f, 0.0014f, 0.3401f, 0.5822f, 0.6935f, 0.2931f, 0.7334f, 0.3755f, 0.6760f, 0.1306f, 0.6065f, 0.4410f, 0.1135f, 0.8444f, 0.3999f, 0.5510f,
		0.4827f, 0.8948f, 0.1889f, 0.4310f, 0.0436f, 0.3946f, 0.5443f, 0.7987f, 0.0404f, 0.0222f, 0.6812f, 0.5983f, 0.0699f, 0.2556f, 0.1747f, 0.8808f,
		0.4120f, 0.3979f, 0.9328f, 0.9794f, 0.2442f, 0.4880f, 0.3137f, 0.8581f, 0.3909f, 0.4261f, 0.7548f, 0.3607f, 0.8628f, 0.5264f, 0.0900f, 0.6739f,
		0.7150f, 0.2374f, 0.2102f, 0.9528f, 0.4484f, 0.7380f, 0.0773f, 0.2606f, 0.5904f, 0.1275f, 0.6289f, 0.1362f, 0.8601f, 0.5967f, 0.5240f, 0.8971f,
		0.6488f, 0.1167f, 0.6668f, 0.5369f, 0.8117f, 0.8549f, 0.8572f, 0.9450f, 0.4341f, 0.6023f, 0.8237f, 0.1094f, 0.6846f, 0.1955f, 0.2136f, 0.2835f,
		0.3870f, 0.1820f, 0.8346f, 0.9489f, 0.3731f, 0.2497f, 0.1625f, 0.5878f, 0.1926f, 0.7378f, 0.7774f, 0.6514f, 0.5625f, 0.9183f, 0.0948f, 0.2606f,
		0.6294f, 0.7513f, 0.3622f, 0.6496f, 0.3973f, 0.6706f, 0.2156f, 0.9254f, 0.9083f, 0.4868f, 0.1410f, 0.2361f, 0.9263f, 0.4160f, 0.7814f, 0.5385f,
		0.1195f, 0.0041f, 0.8475f, 0.8767f, 0.9455f, 0.9350f, 0.4220f, 0.5028f, 0.9325f, 0.1166f, 0.7008f, 0.9955f, 0.3349f, 0.1746f, 0.9828f, 0.1741f,
		0.7342f, 0.7693f, 0.9175f, 0.3826f, 0.7958f, 0.0518f, 0.5281f, 0.6919f, 0.3379f, 0.6756f, 0.9694f, 0.3549f, 0.0545f, 0.2542f, 0.9788f, 0.6112f,
		0.8900f, 0.7126f, 0.2196f, 0.8264f, 0.3511f, 0.0873f, 0.8625f, 0.8054f, 0.4993f, 0.4821f, 0.0364f, 0.8156f, 0.0165f, 0.8759f, 0.3083f, 0.6500f,
		0.4941f, 0.6159f, 0.3967f, 0.9216f, 0.1646f, 0.4727f, 0.5598f, 0.6756f, 0.0598f, 0.2957f, 0.8180f, 0.7693f, 0.1586f, 0.6481f, 0.2287f, 0.6274f,
		0.1385f, 0.6394f, 0.2003f, 0.3523f, 0.4707f, 0.8886f, 0.3117f, 0.5711f, 0.9793f, 0.4572f, 0.1151f, 0.7256f, 0.6205f, 0.6293f, 0.8502f, 0.9499f,
		0.2546f, 0.1423f, 0.6888f, 0.3072f, 0.2848f, 0.8476f, 0.6170f, 0.2074f, 0.5505f, 0.5418f, 0.1738f, 0.4748f, 0.6783f, 0.2891f, 0.5281f, 0.3065f,
		0.8693f, 0.0402f, 0.4173f, 0.4725f, 0.8576f, 0.9174f, 0.8423f, 0.9868f, 0.6045f, 0.7311f, 0.6078f, 0.9046f, 0.3979f, 0.6278f, 0.5333f, 0.6567f,
		0.6272f, 0.2235f, 0.2684f, 0.2548f, 0.8343f, 0.1310f, 0.8380f, 0.6135f, 0.8216f, 0.8597f, 0.4052f, 0.9099f, 0.0361f, 0.6430f, 0.1870f, 0.9457f,
		0.3190f, 0.7090f, 0.8522f, 0.5595f, 0.8657f, 0.3688f, 0.8404f, 0.9505f, 0.3151f, 0.3317f, 0.5092f, 0.4686f, 0.1190f, 0.5418f, 0.9834f, 0.1155f,
		0.2998f, 0.8403f, 0.4452f, 0.9007f, 0.6336f, 0.3041f, 0.9961f, 0.8440f, 0.4623f, 0.3144f, 0.8500f, 0.7736f, 0.9583f, 0.7653f, 0.5675f, 0.7226f,
		0.0012f, 0.1896f, 0.3646f, 0.1923f, 0.8368f, 0.7836f, 0.0267f, 0.0652f, 0.5887f, 0.9377f, 0.9936f, 0.5974f, 0.8519f, 0.6703f, 0.3609f, 0.7556f,
		0.5715f, 0.2319f, 0.4250f, 0.1164f, 0.3218f, 0.6296f, 0.7012f, 0.7169f, 0.1463f, 0.3605f, 0.4984f, 0.8460f, 0.3079f, 0.3234f, 0.2888f, 0.4779f,
		0.2364f, 0.8765f, 0.6674f, 0.9771f, 0.1793f, 0.4794f, 0.6332f, 0.9576f, 0.3436f, 0.8718f, 0.4528f, 0.8954f, 0.3276f, 0.8677f, 0.5968f, 0.9070f,
		0.4174f, 0.5307f, 0.5474f, 0.1410f, 0.7210f, 0.5876f, 0.8300f, 0.4608f, 0.5638f, 0.6737f, 0.0358f, 0.7558f, 0.3318f, 0.6534f, 0.9263f, 0.7245f,
		0.9785f, 0.4952f, 0.0981f, 0.9367f, 0.1399f, 0.8513f, 0.8898f, 0.3765f, 0.6614f, 0.1564f, 0.6718f, 0.4878f, 0.0465f, 0.4419f, 0.0140f, 0.4404f,
		0.2359f, 0.1637f, 0.0753f, 0.2547f, 0.2140f, 0.5548f, 0.7128f, 0.7957f, 0.4716f, 0.1050f, 0.3559f, 0.8344f, 0.4980f, 0.0183f, 0.3647f, 0.9188f,
		0.9092f, 0.8585f, 0.9282f, 0.9463f, 0.7553f, 0.4087f, 0.1378f, 0.2478f, 0.3006f, 0.4700f, 0.2487f, 0.5216f, 0.0098f, 0.8915f, 0.9089f, 0.2275f,
		0.7029f, 0.5967f, 0.5815f, 0.0999f, 0.8048f, 0.9474f, 0.0806f, 0.3757f, 0.8904f, 0.6891f, 0.6009f, 0.3822f, 0.8140f, 0.2583f, 0.2780f, 0.9073f,
		0.6250f, 0.0166f, 0.5028f, 0.7430f, 0.2478f, 0.8462f, 0.6478f, 0.3798f, 0.5173f, 0.9214f, 0.9048f, 0.8056f, 0.6719f, 0.4872f, 0.6780f, 0.5756f,
		0.9107f, 0.9476f, 0.5247f, 0.2312f, 0.2990f, 0.0681f, 0.5696f, 0.1210f, 0.7016f, 0.3119f, 0.4473f, 0.0140f, 0.0133f, 0.2578f, 0.4818f, 0.8088f,
		0.6282f, 0.7802f, 0.2027f, 0.0249f, 0.7743f, 0.7830f, 0.3300f, 0.7888f, 0.3468f, 0.7787f, 0.2619f, 0.6966f, 0.2128f, 0.7138f, 0.8718f, 0.6397f,
		0.7110f, 0.6512f, 0.0423f, 0.2369f, 0.7462f, 0.2350f, 0.4427f, 0.1954f, 0.1759f, 0.9879f, 0.0312f, 0.9754f, 0.2770f, 0.7526f, 0.6397f, 0.5078f,
		0.8735f, 0.7753f, 0.3900f, 0.4159f, 0.2878f, 0.1893f, 0.8379f, 0.1862f, 0.3556f, 0.8037f, 0.0291f, 0.8020f, 0.2480f, 0.3540f, 0.4205f, 0.1095f,
		0.7312f, 0.7006f, 0.7160f, 0.6515f, 0.2500f, 0.8842f, 0.3642f, 0.2449f, 0.4722f, 0.0806f, 0.3093f, 0.2506f, 0.5190f, 0.0661f, 0.0378f, 0.8657f,
		0.7677f, 0.6173f, 0.5370f, 0.7439f, 0.4012f, 0.5954f, 0.8698f, 0.1939f, 0.6703f, 0.0184f, 0.7431f, 0.9795f, 0.3823f, 0.1910f, 0.9922f, 0.9461f,
		0.3064f, 0.7937f, 0.6873f, 0.5562f, 0.9583f, 0.3909f, 0.3578f, 0.1102f, 0.9775f, 0.8314f, 0.4858f, 0.1486f, 0.8473f, 0.7331f, 0.3973f, 0.3763f,
		0.3987f, 0.4638f, 0.9769f, 0.8447f, 0.0756f, 0.4738f, 0.4709f, 0.5481f, 0.3501f, 0.7274f, 0.1231f, 0.3477f, 0.8395f, 0.5627f, 0.0368f, 0.5647f,
		0.9603f, 0.2205f, 0.9069f, 0.6776f, 0.8410f, 0.1115f, 0.0323f, 0.0277f, 0.4682f, 0.2291f, 0.5087f, 0.1996f, 0.2981f, 0.6772f, 0.5260f, 0.8282f,
		0.4133f, 0.3051f, 0.2233f, 0.7780f, 0.1980f, 0.4149f, 0.0074f, 0.4642f, 0.7852f, 0.5344f, 0.0605f, 0.5724f, 0.6933f, 0.8658f, 0.0349f, 0.5868f,
		0.1617f, 0.2037f, 0.6565f, 0.6043f, 0.6883f, 0.2572f, 0.2464f, 0.3382f, 0.8399f, 0.2684f, 0.9132f, 0.7595f, 0.2892f, 0.3472f, 0.5089f, 0.3615f,
		0.5546f, 0.0864f, 0.0243f, 0.6616f, 0.9888f, 0.1106f, 0.1294f, 0.4059f, 0.7817f, 0.3039f, 0.5218f, 0.2362f, 0.2779f, 0.6992f, 0.7338f, 0.7720f,
		0.6584f, 0.0563f, 0.1530f, 0.5368f, 0.7922f, 0.1652f, 0.5922f, 0.2283f, 0.1470f, 0.1160f, 0.3192f, 0.2934f, 0.8726f, 0.8422f, 0.3062f, 0.2287f,
		0.7457f, 0.8213f, 0.7782f, 0.6113f, 0.9691f, 0.2976f, 0.3673f, 0.8150f, 0.9858f, 0.6932f, 0.4117f, 0.3666f, 0.3454f, 0.6090f, 0.7789f, 0.6408f,
		0.3409f, 0.3284f, 0.8986f, 0.9523f, 0.2725f, 0.7589f, 0.1112f, 0.6134f, 0.8643f, 0.6076f, 0.3573f, 0.2276f, 0.1770f, 0.7738f, 0.3182f, 0.2983f,
		0.6793f, 0.4546f, 0.9767f, 0.2445f, 0.8801f, 0.0462f, 0.4513f, 0.7092f, 0.7841f, 0.4883f, 0.2287f, 0.0412f, 0.0774f, 0.7188f, 0.4542f, 0.0391f,
		0.6147f, 0.5386f, 0.8566f, 0.8889f, 0.1840f, 0.4879f, 0.8803f, 0.7268f, 0.1129f, 0.8357f, 0.9433f, 0.3400f, 0.1679f, 0.2412f, 0.1259f, 0.4601f,
		0.7899f, 0.3138f, 0.6407f, 0.7959f, 0.1980f, 0.4073f, 0.6738f, 0.4143f, 0.1859f, 0.3534f, 0.7867f, 0.4221f, 0.1339f, 0.3632f, 0.3938f, 0.7487f,
		0.3281f, 0.1156f, 0.2538f, 0.5269f, 0.6727f, 0.5174f, 0.6864f, 0.5328f, 0.5511f, 0.6674f, 0.3826f, 0.4087f, 0.6494f, 0.6139f, 0.6004f, 0.4854f,
	};
}
