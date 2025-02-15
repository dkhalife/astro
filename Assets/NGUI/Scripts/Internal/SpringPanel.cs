//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Similar to SpringPosition, but also moves the panel's clipping. Works in local coordinates.
/// </summary>

[RequireComponent(typeof(UIPanel))]
[AddComponentMenu("NGUI/Internal/Spring Panel")]
public class SpringPanel : MonoBehaviour
{
	public Vector3 target = Vector3.zero;
	public float strength = 10f;

	public delegate void OnFinished ();
	public OnFinished onFinished;

	UIPanel mPanel;
	Transform mTrans;
	float mThreshold = 0f;
	UIScrollView mDrag;

	/// <summary>
	/// Cache the transform.
	/// </summary>

	void Start ()
	{
		mPanel = GetComponent<UIPanel>();
		mDrag = GetComponent<UIScrollView>();
		mTrans = transform;
	}

	/// <summary>
	/// Advance toward the target position.
	/// </summary>

	void Update ()
	{
	    AdvanceTowardsPosition();
	}

    /// <summary>
    /// Advance toward the target position.
    /// </summary>
    
    protected virtual void AdvanceTowardsPosition()
    {
        float delta = RealTime.deltaTime;

        if (mThreshold == 0f)
        {
            mThreshold = (target - mTrans.localPosition).magnitude * 0.005f;
            mThreshold = Mathf.Max(mThreshold, 0.00001f);
        }

        bool trigger = false;
        Vector3 before = mTrans.localPosition;
        Vector3 after = NGUIMath.SpringLerp(mTrans.localPosition, target, strength, delta);

        if (mThreshold >= Vector3.Magnitude(after - target))
        {
            after = target;
            enabled = false;
            trigger = true;
        }
        mTrans.localPosition = after;

        Vector3 offset = after - before;
        Vector2 cr = mPanel.clipOffset;
        cr.x -= offset.x;
        cr.y -= offset.y;
		mPanel.clipOffset = cr;

        if (mDrag != null) mDrag.UpdateScrollbars(false);
        if (trigger && onFinished != null) onFinished();
    }

	/// <summary>
	/// Start the tweening process.
	/// </summary>

	static public SpringPanel Begin (GameObject go, Vector3 pos, float strength)
	{
		SpringPanel sp = go.GetComponent<SpringPanel>();
		if (sp == null) sp = go.AddComponent<SpringPanel>();
		sp.target = pos;
		sp.strength = strength;
		sp.onFinished = null;
		sp.mThreshold = 0f;
		sp.enabled = true;
		return sp;
	}
}
