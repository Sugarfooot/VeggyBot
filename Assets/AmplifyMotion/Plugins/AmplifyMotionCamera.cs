// Amplify Motion - Full-scene Motion Blur for Unity Pro
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4  || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
#define UNITY_4
#endif
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4  || UNITY_5_5 || UNITY_5_6 || UNITY_5_7 || UNITY_5_8 || UNITY_5_9
#define UNITY_5
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if !UNITY_4
using UnityEngine.Rendering;
#endif

[AddComponentMenu( "" )]
[RequireComponent( typeof( Camera ) )]
public class AmplifyMotionCamera : MonoBehaviour
{
	internal AmplifyMotionEffectBase Instance = null;

	internal Matrix4x4 PrevViewProjMatrix;
	internal Matrix4x4 ViewProjMatrix;
	internal Matrix4x4 InvViewProjMatrix;

	internal Matrix4x4 PrevViewProjMatrixRT;
	internal Matrix4x4 ViewProjMatrixRT;

	internal Transform Transform;

	private bool m_linked = false;
	private bool m_initialized = false;
	private bool m_starting = true;

	private bool m_autoStep = true;
	private bool m_step = false;
	private bool m_overlay = false;
	private Camera m_camera;

	public bool Initialized { get { return m_initialized; } }
	public bool AutoStep { get { return m_autoStep; } }
	public bool Overlay { get { return m_overlay; } }
	public Camera Camera { get { return m_camera; } }

	private int m_prevFrameCount = 0;

	private HashSet<AmplifyMotionObjectBase> m_affectedObjectsTable = new HashSet<AmplifyMotionObjectBase>();
	private AmplifyMotionObjectBase[] m_affectedObjects = null;
	private bool m_affectedObjectsChanged = true;

#if !UNITY_4
	private const CameraEvent m_renderCBEvent = CameraEvent.BeforeImageEffects;
	private CommandBuffer m_renderCB = null;
#endif

	public void RegisterObject( AmplifyMotionObjectBase obj )
	{
		m_affectedObjectsTable.Add( obj );
		m_affectedObjectsChanged = true;
	}

	public void UnregisterObject( AmplifyMotionObjectBase obj )
	{
		m_affectedObjectsTable.Remove( obj );
		m_affectedObjectsChanged = true;
	}

	void UpdateAffectedObjects()
	{
		if ( m_affectedObjects == null || m_affectedObjectsTable.Count != m_affectedObjects.Length )
			m_affectedObjects = new AmplifyMotionObjectBase[ m_affectedObjectsTable.Count ];

		m_affectedObjectsTable.CopyTo( m_affectedObjects );

		m_affectedObjectsChanged = false;
	}

	public void LinkTo( AmplifyMotionEffectBase instance, bool overlay )
	{
		Instance = instance;

		m_camera = GetComponent<Camera>();
		m_camera.depthTextureMode |= DepthTextureMode.Depth;
	#if !UNITY_4
		InitializeCommandBuffers();
	#endif

		m_overlay = overlay;
		m_linked = true;
	}

	public void Initialize()
	{
		m_step = false;
		UpdateMatrices();
		m_initialized = true;
	}

#if !UNITY_4
	void InitializeCommandBuffers()
	{
		ShutdownCommandBuffers();

		m_renderCB = new CommandBuffer();
		m_renderCB.name = "AmplifyMotion.Render";
		m_camera.AddCommandBuffer( m_renderCBEvent, m_renderCB );
	}

	void ShutdownCommandBuffers()
	{
		if ( m_renderCB != null )
		{
			m_camera.RemoveCommandBuffer( m_renderCBEvent, m_renderCB );
			m_renderCB.Release();
			m_renderCB = null;
		}
	}
#endif

	void Awake()
	{
		Transform = transform;
	}

	void OnEnable()
	{
		AmplifyMotionEffectBase.RegisterCamera( this );
	}

	void OnDisable()
	{
		m_initialized = false;
	#if !UNITY_4
		ShutdownCommandBuffers();
	#endif
		AmplifyMotionEffectBase.UnregisterCamera( this );
	}

	void OnDestroy()
	{
		if ( Instance != null )
			Instance.RemoveCamera( m_camera );
	}

	public void StopAutoStep()
	{
		if ( m_autoStep )
		{
			m_autoStep = false;
			m_step = true;
		}
	}

	public void StartAutoStep()
	{
		m_autoStep = true;
	}

	public void Step()
	{
		m_step = true;
	}

	void Update()
	{
		if ( !m_linked || !Instance.isActiveAndEnabled )
			return;

		if ( !m_initialized )
			Initialize();

		if ( ( m_camera.depthTextureMode & DepthTextureMode.Depth ) == 0 )
			m_camera.depthTextureMode |= DepthTextureMode.Depth;
	}

	void UpdateMatrices()
	{
		if ( !m_starting )
		{
			PrevViewProjMatrix = ViewProjMatrix;
			PrevViewProjMatrixRT = ViewProjMatrixRT;
		}

		Matrix4x4 view = m_camera.worldToCameraMatrix;
		Matrix4x4 proj = GL.GetGPUProjectionMatrix( m_camera.projectionMatrix, false );
		ViewProjMatrix = proj * view;
		InvViewProjMatrix = Matrix4x4.Inverse( ViewProjMatrix );

		Matrix4x4 projRT = GL.GetGPUProjectionMatrix( m_camera.projectionMatrix, true );
		ViewProjMatrixRT = projRT * view;

		if ( m_starting )
		{
			PrevViewProjMatrix = ViewProjMatrix;
			PrevViewProjMatrixRT = ViewProjMatrixRT;
		}
	}

#if UNITY_4
	public void FixedUpdateTransform( AmplifyMotionEffectBase inst )
#else
	public void FixedUpdateTransform( AmplifyMotionEffectBase inst, CommandBuffer updateCB )
#endif
	{
		if ( !m_initialized )
			Initialize();

		if ( m_affectedObjectsChanged )
			UpdateAffectedObjects();

		for ( int i = 0; i < m_affectedObjects.Length; i++ )
		{
			if ( m_affectedObjects[ i ].FixedStep )
			{
			#if UNITY_4
				m_affectedObjects[ i ].OnUpdateTransform( inst, m_camera, m_starting );
			#else
				m_affectedObjects[ i ].OnUpdateTransform( inst, m_camera, updateCB, m_starting );
			#endif
			}
		}
	}

#if UNITY_4
	public void UpdateTransform( AmplifyMotionEffectBase inst )
#else
	public void UpdateTransform( AmplifyMotionEffectBase inst, CommandBuffer updateCB )
#endif
	{
		if ( !m_initialized )
			Initialize();

		if ( Time.frameCount > m_prevFrameCount && ( m_autoStep || m_step ) )
		{
			UpdateMatrices();

			if ( m_affectedObjectsChanged )
				UpdateAffectedObjects();

			for ( int i = 0; i < m_affectedObjects.Length; i++ )
			{
				if ( !m_affectedObjects[ i ].FixedStep )
				{
				#if UNITY_4
					m_affectedObjects[ i ].OnUpdateTransform( inst, m_camera, m_starting );
				#else
					m_affectedObjects[ i ].OnUpdateTransform( inst, m_camera, updateCB, m_starting );
				#endif
				}
			}

			m_starting = false;
			m_step = false;

			m_prevFrameCount = Time.frameCount;
		}
	}

	public void RenderReprojectionVectors( RenderTexture destination, float scale )
	{
	#if UNITY_4
		Shader.SetGlobalMatrix( "_AM_MATRIX_CURR_REPROJ", PrevViewProjMatrix * InvViewProjMatrix );
		Shader.SetGlobalFloat( "_AM_MOTION_SCALE", scale );

		Graphics.Blit( null, destination, Instance.ReprojectionMaterial );
	#else
		m_renderCB.SetGlobalMatrix( "_AM_MATRIX_CURR_REPROJ", PrevViewProjMatrix * InvViewProjMatrix );
		m_renderCB.SetGlobalFloat( "_AM_MOTION_SCALE", scale );

		RenderTexture dummy = null;
		m_renderCB.Blit( new RenderTargetIdentifier( dummy ), destination, Instance.ReprojectionMaterial );
	#endif
	}

	public void PreRenderVectors( RenderTexture motionRT, bool clearColor, float rcpDepthThreshold )
	{
	#if UNITY_4
		Graphics.SetRenderTarget( motionRT );
		GL.Clear( true, clearColor, Color.black );
	#else
		m_renderCB.Clear();

		m_renderCB.SetGlobalFloat( "_AM_MIN_VELOCITY", Instance.MinVelocity );
		m_renderCB.SetGlobalFloat( "_AM_MAX_VELOCITY", Instance.MaxVelocity );
		m_renderCB.SetGlobalFloat( "_AM_RCP_TOTAL_VELOCITY", 1.0f / ( Instance.MaxVelocity - Instance.MinVelocity ) );
		m_renderCB.SetGlobalVector( "_AM_DEPTH_THRESHOLD", new Vector2( Instance.DepthThreshold, rcpDepthThreshold ) );

		m_renderCB.SetRenderTarget( motionRT );
		m_renderCB.ClearRenderTarget( true, clearColor, Color.black );
	#endif
	}

	public void RenderVectors( float scale, float fixedScale, AmplifyMotion.Quality quality )
	{
		if ( !m_initialized )
			Initialize();

		// For some reason Unity's own values weren't working correctly on Windows/OpenGL
		float near = m_camera.nearClipPlane;
		float far = m_camera.farClipPlane;
		Vector4 zparam;

		if ( AmplifyMotionEffectBase.IsD3D )
		{
			zparam.x = 1.0f - far / near;
			zparam.y = far / near;
		}
		else
		{
			// OpenGL
			zparam.x = ( 1.0f - far / near ) / 2.0f;
			zparam.y = ( 1.0f + far / near ) / 2.0f;
		}

		zparam.z = zparam.x / far;
		zparam.w = zparam.y / far;

	#if UNITY_4
		Shader.SetGlobalVector( "_AM_ZBUFFER_PARAMS", zparam );
	#else
		m_renderCB.SetGlobalVector( "_AM_ZBUFFER_PARAMS", zparam );
	#endif

		if ( m_affectedObjectsChanged )
			UpdateAffectedObjects();

		for ( int i = 0; i < m_affectedObjects.Length; i++ )
		{
			// don't render objects excluded via camera culling mask
			if ( ( m_camera.cullingMask & ( 1 << m_affectedObjects[ i ].gameObject.layer ) ) != 0 )
			{
			#if UNITY_4
				m_affectedObjects[ i ].OnRenderVectors( m_camera, m_affectedObjects[ i ].FixedStep ? fixedScale : scale, quality );
			#else
				m_affectedObjects[ i ].OnRenderVectors( m_camera, m_renderCB, m_affectedObjects[ i ].FixedStep ? fixedScale : scale, quality );
			#endif
			}
		}
	}

#if UNITY_4
	void OnPostRender()
	{
		if ( !m_linked || !Instance.isActiveAndEnabled )
			return;

		if ( !m_initialized )
			Initialize();

		if ( m_overlay )
		{

			RenderTexture prevRT = RenderTexture.active;

			Graphics.SetRenderTarget( Instance.MotionRenderTexture );
			RenderVectors( Instance.MotionScaleNorm, Instance.FixedMotionScaleNorm, Instance.QualityLevel );

			RenderTexture.active = prevRT;
		}
	}
#endif

#if UNITY_EDITOR
	void OnGUI()
	{
		if ( !Application.isEditor )
			return;

		if ( !m_linked || !Instance.isActiveAndEnabled )
			return;

		if ( !m_initialized )
			Initialize();

		if ( m_affectedObjectsChanged )
			UpdateAffectedObjects();

		for ( int i = 0; i < m_affectedObjects.Length; i++ )
			m_affectedObjects[ i ].OnRenderDebugHUD( m_camera );
	}
#endif
}
