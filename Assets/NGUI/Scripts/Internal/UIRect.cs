//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Abstract UI rectangle containing functionality common to both panels and widgets.
/// A UI rectangle contains 4 anchor points (one for each side), and it ensures that they are updated in the proper order.
/// </summary>

public abstract class UIRect : MonoBehaviour
{
	[System.Serializable]
	public class AnchorPoint
	{
		public Transform target;
		public float relative = 0f;
		public int absolute = 0;

		[System.NonSerialized]
		public UIRect rect;

		[System.NonSerialized]
		public Camera targetCam;

		public AnchorPoint () { }
		public AnchorPoint (float relative) { this.relative = relative; }

		/// <summary>
		/// Convenience function that sets the anchor's values.
		/// </summary>

		public void Set (float relative, float absolute)
		{
			this.relative = relative;
			this.absolute = Mathf.FloorToInt(absolute + 0.5f);
		}

		/// <summary>
		/// Set the anchor's value to the nearest of the 3 possible choices of (left, center, right) or (bottom, center, top).
		/// </summary>

		public void SetToNearest (float abs0, float abs1, float abs2) { SetToNearest(0f, 0.5f, 1f, abs0, abs1, abs2); }

		/// <summary>
		/// Set the anchor's value given the 3 possible anchor combinations. Chooses the one with the smallest absolute offset.
		/// </summary>

		public void SetToNearest (float rel0, float rel1, float rel2, float abs0, float abs1, float abs2)
		{
			float a0 = Mathf.Abs(abs0);
			float a1 = Mathf.Abs(abs1);
			float a2 = Mathf.Abs(abs2);

			if (a0 < a1 && a0 < a2) Set(rel0, abs0);
			else if (a1 < a0 && a1 < a2) Set(rel1, abs1);
			else Set(rel2, abs2);
		}

		/// <summary>
		/// Set the anchor's absolute coordinate relative to the specified parent, keeping the relative setting intact.
		/// </summary>

		public void SetHorizontal (Transform parent, float localPos)
		{
			if (rect)
			{
				Vector3[] sides = rect.GetSides(parent);
				float targetPos = Mathf.Lerp(sides[0].x, sides[2].x, relative);
				absolute = Mathf.FloorToInt(localPos - targetPos + 0.5f);
			}
			else
			{
				Vector3 targetPos = target.position;
				if (parent != null) targetPos = parent.InverseTransformPoint(targetPos);
				absolute = Mathf.FloorToInt(localPos - targetPos.x + 0.5f);
			}
		}

		/// <summary>
		/// Set the anchor's absolute coordinate relative to the specified parent, keeping the relative setting intact.
		/// </summary>

		public void SetVertical (Transform parent, float localPos)
		{
			if (rect)
			{
				Vector3[] sides = rect.GetSides(parent);
				float targetPos = Mathf.Lerp(sides[3].y, sides[1].y, relative);
				absolute = Mathf.FloorToInt(localPos - targetPos + 0.5f);
			}
			else
			{
				Vector3 targetPos = target.position;
				if (parent != null) targetPos = parent.InverseTransformPoint(targetPos);
				absolute = Mathf.FloorToInt(localPos - targetPos.y + 0.5f);
			}
		}

		/// <summary>
		/// Convenience function that returns the sides the anchored point is anchored to.
		/// </summary>

		public Vector3[] GetSides (Transform relativeTo)
		{
			if (target != null)
			{
				if (rect != null) return rect.GetSides(relativeTo);
				if (target.camera != null) return target.camera.GetSides(relativeTo);
			}
			return null;
		}
	}

	/// <summary>
	/// Left side anchor.
	/// </summary>

	public AnchorPoint leftAnchor = new AnchorPoint();

	/// <summary>
	/// Right side anchor.
	/// </summary>

	public AnchorPoint rightAnchor = new AnchorPoint(1f);

	/// <summary>
	/// Bottom side anchor.
	/// </summary>

	public AnchorPoint bottomAnchor = new AnchorPoint();

	/// <summary>
	/// Top side anchor.
	/// </summary>

	public AnchorPoint topAnchor = new AnchorPoint(1f);

	protected GameObject mGo;
	protected Transform mTrans;
	protected BetterList<UIRect> mChildren = new BetterList<UIRect>();
	protected bool mChanged = true;
	protected float mFinalAlpha = 0f;

	UIRoot mRoot;
	UIRect mParent;
	Camera mMyCam;
	int mUpdateFrame = -1;
	bool mAnchorsCached = false;
	bool mParentFound = false;
	bool mRootSet = false;

	/// <summary>
	/// Game object gets cached for speed. Can't simply return 'mGo' set in Awake because this function may be called on a prefab.
	/// </summary>

	public GameObject cachedGameObject { get { if (mGo == null) mGo = gameObject; return mGo; } }

	/// <summary>
	/// Transform gets cached for speed. Can't simply return 'mTrans' set in Awake because this function may be called on a prefab.
	/// </summary>

	public Transform cachedTransform { get { if (mTrans == null) mTrans = transform; return mTrans; } }

	/// <summary>
	/// Camera used by anchors.
	/// </summary>

	public Camera anchorCamera { get { if (!mAnchorsCached) ResetAnchors(); return mMyCam; } }

	/// <summary>
	/// Get the rectangle's parent, if any.
	/// </summary>

	public UIRect parent
	{
		get
		{
			if (!mParentFound)
			{
				mParentFound = true;
				mParent = NGUITools.FindInParents<UIRect>(cachedTransform.parent);
			}
			return mParent;
		}
	}

	/// <summary>
	/// Get the root object, if any.
	/// </summary>

	public UIRoot root
	{
		get
		{
			if (parent != null) return mParent.root;

			if (!mRootSet)
			{
				mRootSet = true;
				mRoot = NGUITools.FindInParents<UIRoot>(cachedTransform);
			}
			return mRoot;
		}
	}

	/// <summary>
	/// Returns 'true' if the widget is currently anchored on any side.
	/// </summary>

	public bool isAnchored
	{
		get
		{
			return leftAnchor.target || rightAnchor.target || topAnchor.target || bottomAnchor.target;
		}
	}

	/// <summary>
	/// Local alpha, not relative to anything.
	/// </summary>

	public abstract float alpha { get; set; }

	/// <summary>
	/// Alpha property is exposed so that it's possible to make it cumulative.
	/// </summary>

	public abstract float finalAlpha { get; }

	/// <summary>
	/// Local-space corners of the UI rectangle. The order is bottom-left, top-left, top-right, bottom-right.
	/// </summary>

	public abstract Vector3[] localCorners { get; }

	/// <summary>
	/// World-space corners of the UI rectangle. The order is bottom-left, top-left, top-right, bottom-right.
	/// </summary>

	public abstract Vector3[] worldCorners { get; }

	/// <summary>
	/// Sets the local 'changed' flag, indicating that some parent value(s) are now be different, such as alpha for example.
	/// </summary>

	public void Invalidate (bool includeChildren)
	{
		mChanged = true;
		if (includeChildren)
			for (int i = 0; i < mChildren.size; ++i)
				mChildren.buffer[i].Invalidate(true);
	}

	// Temporary variable to avoid GC allocation
	static Vector3[] mSides = new Vector3[4];

	/// <summary>
	/// Get the sides of the rectangle relative to the specified transform.
	/// The order is left, top, right, bottom.
	/// </summary>

	public virtual Vector3[] GetSides (Transform relativeTo)
	{
		if (anchorCamera != null)
		{
			return anchorCamera.GetSides(relativeTo);
		}
		else
		{
			Vector3 pos = cachedTransform.position;
			for (int i = 0; i < 4; ++i)
				mSides[i] = pos;

			if (relativeTo != null)
			{
				for (int i = 0; i < 4; ++i)
					mSides[i] = relativeTo.InverseTransformPoint(mSides[i]);
			}
			return mSides;
		}
	}

	/// <summary>
	/// Helper function that gets the specified anchor's position relative to the chosen transform.
	/// </summary>

	protected Vector3 GetLocalPos (AnchorPoint ac, Transform trans)
	{
		if (anchorCamera == null || ac.targetCam == null)
			return cachedTransform.localPosition;

		Vector3 pos = mMyCam.ViewportToWorldPoint(ac.targetCam.WorldToViewportPoint(ac.target.position));
		if (trans != null) pos = trans.InverseTransformPoint(pos);
		pos.x = Mathf.Floor(pos.x + 0.5f);
		pos.y = Mathf.Floor(pos.y + 0.5f);
		return pos;
	}

	/// <summary>
	/// Automatically find the parent rectangle.
	/// </summary>

	protected virtual void OnEnable ()
	{
		mChanged = true;
		mRootSet = false;
		mParentFound = false;
		if (parent != null) mParent.mChildren.Add(this);
	}

	/// <summary>
	/// Clear the parent rectangle reference.
	/// </summary>

	protected virtual void OnDisable ()
	{
		if (mParent) mParent.mChildren.Remove(this);
		mParent = null;
		mRoot = null;
		mRootSet = false;
		mParentFound = false;
	}

	/// <summary>
	/// Set anchor rect references on start.
	/// </summary>

	protected void Start () { OnStart(); }

	/// <summary>
	/// Rectangles need to update in a specific order -- parents before children.
	/// When deriving from this class, override its OnUpdate() function instead.
	/// </summary>

	public void Update ()
	{
		if (!mAnchorsCached) ResetAnchors();

		int frame = Time.frameCount;

		if (mUpdateFrame != frame)
		{
			mUpdateFrame = frame;
			bool anchored = false;

			if (leftAnchor.target)
			{
				anchored = true;
				if (leftAnchor.rect != null && leftAnchor.rect.mUpdateFrame != frame)
					leftAnchor.rect.Update();
			}

			if (bottomAnchor.target)
			{
				anchored = true;
				if (bottomAnchor.rect != null && bottomAnchor.rect.mUpdateFrame != frame)
					bottomAnchor.rect.Update();
			}

			if (rightAnchor.target)
			{
				anchored = true;
				if (rightAnchor.rect != null && rightAnchor.rect.mUpdateFrame != frame)
					rightAnchor.rect.Update();
			}

			if (topAnchor.target)
			{
				anchored = true;
				if (topAnchor.rect != null && topAnchor.rect.mUpdateFrame != frame)
					topAnchor.rect.Update();
			}

			// Update the dimensions using anchors
			if (anchored) OnAnchor();

			// Continue with the update
			OnUpdate();
		}
	}

	/// <summary>
	/// Manually update anchored sides.
	/// </summary>

	public void UpdateAnchors () { if (isAnchored) OnAnchor(); }

	/// <summary>
	/// Update the dimensions of the rectangle using anchor points.
	/// </summary>

	protected abstract void OnAnchor ();

	/// <summary>
	/// Ensure that all rect references are set correctly on the anchors.
	/// </summary>

	public void ResetAnchors ()
	{
		mAnchorsCached = true;

		leftAnchor.rect		= (leftAnchor.target)	? leftAnchor.target.GetComponent<UIRect>()	 : null;
		bottomAnchor.rect	= (bottomAnchor.target) ? bottomAnchor.target.GetComponent<UIRect>() : null;
		rightAnchor.rect	= (rightAnchor.target)	? rightAnchor.target.GetComponent<UIRect>()	 : null;
		topAnchor.rect		= (topAnchor.target)	? topAnchor.target.GetComponent<UIRect>()	 : null;

		mMyCam = NGUITools.FindCameraForLayer(cachedGameObject.layer);

		FindCameraFor(leftAnchor);
		FindCameraFor(bottomAnchor);
		FindCameraFor(rightAnchor);
		FindCameraFor(topAnchor);
	}

	/// <summary>
	/// Helper function -- attempt to find the camera responsible for the specified anchor.
	/// </summary>

	void FindCameraFor (AnchorPoint ap)
	{
		// If we don't have a target or have a rectangle to work with, camera isn't needed
		if (ap.target == null || ap.rect != null)
		{
			ap.targetCam = null;
		}
		else
		{
			// Find the camera responsible for the target object
			ap.targetCam = NGUITools.FindCameraForLayer(ap.target.gameObject.layer);

			// No camera found? Clear the references
			if (ap.targetCam == null)
			{
				ap.target = null;
				return;
			}
		}
	}

	/// <summary>
	/// Call this function when the rectangle's parent has changed.
	/// </summary>

	public virtual void ParentHasChanged ()
	{
		UIRect pt = NGUITools.FindInParents<UIRect>(cachedTransform.parent);

		if (mParent != pt)
		{
			if (mParent) mParent.mChildren.Remove(this);
			mParent = pt;
			if (mParent) mParent.mChildren.Add(this);
			mRootSet = false;
		}
	}

	/// <summary>
	/// Abstract start functionality, ensured to happen after the anchor rect references have been set.
	/// </summary>

	protected abstract void OnStart ();

	/// <summary>
	/// Abstract update functionality, ensured to happen after the targeting anchors have been updated.
	/// </summary>

	protected virtual void OnUpdate () { }

#if UNITY_EDITOR
	/// <summary>
	/// This callback is sent inside the editor notifying us that some property has changed.
	/// </summary>

	protected virtual void OnValidate()
	{
		ResetAnchors();
		Invalidate(true);
	}
#endif
}
