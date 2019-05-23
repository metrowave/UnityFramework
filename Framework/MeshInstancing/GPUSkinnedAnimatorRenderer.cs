using UnityEngine;
using UnityEngine.Rendering;

namespace Framework
{
	namespace MeshInstancing
	{
        public class GPUSkinnedAnimatorRenderer : MonoBehaviour
		{
            #region Public Data
            public AnimationTextureRef _animationTexture;
			public Mesh _mesh;
			public Material[] _materials;
			public ShadowCastingMode _shadowCastingMode;
			public bool _recieveShadows;

			public GPUSkinnedAnimator[] _animators;
			#endregion

			#region Private Data
			private MaterialPropertyBlock _propertyBlock;
			private Matrix4x4[] _renderedInstanceTransforms;
			private float[] _currentFrames;
			#endregion

			#region MonoBehaviour
			private void Awake()
            {
				_propertyBlock = new MaterialPropertyBlock();

				for (int i=0; i<_materials.Length; i++)
				{
					_animationTexture.SetMaterialProperties(_materials[i]);
				}
			}
			
			private void Update()
			{
#if UNITY_EDITOR
				for (int i = 0; i < _materials.Length; i++)
				{
					_animationTexture.SetMaterialProperties(_materials[i]);
				}
#endif

				Render();
			}
			#endregion

			#region Public Functions
			
			#endregion

			#region Private Functions
			private void Render()
			{
				if (_animators.Length > 0)
				{
					//Loop through all meshes and render them
					_currentFrames = new float[_animators.Length];
					_renderedInstanceTransforms = new Matrix4x4[_animators.Length];

					for (int i = 0; i < _renderedInstanceTransforms.Length; i++)
					{
						_renderedInstanceTransforms[i] = _animators[i].transform.localToWorldMatrix;
						_currentFrames[i] = _animators[i].GetCurrentAnimationFrame();
					}

					_propertyBlock.SetFloatArray("frameIndex", _currentFrames);
					//_material.GetMaterial().SetFloat("preFrameIndex", -1.0f);
					//_material.GetMaterial().SetFloat("transitionProgress", -1.0f);

					for (int i = 0; i < _mesh.subMeshCount; i++)
					{
						Graphics.DrawMeshInstanced(_mesh, i, _materials[i], _renderedInstanceTransforms, _renderedInstanceTransforms.Length, _propertyBlock, _shadowCastingMode, _recieveShadows, this.gameObject.layer);
					}
				}
			}
			#endregion
        }
    }
}