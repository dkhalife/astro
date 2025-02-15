//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// All children added to the game object with this script will be arranged into a table
/// with rows and columns automatically adjusting their size to fit their content
/// (think "table" tag in HTML).
/// </summary>

[AddComponentMenu("NGUI/Interaction/Table")]
public class UITable : UIWidgetContainer
{
	public delegate void OnReposition ();

	public enum Direction
	{
		Down,
		Up,
	}

	/// <summary>
	/// How many columns there will be before a new line is started. 0 means unlimited.
	/// </summary>

	public int columns = 0;

	/// <summary>
	/// Which way the new lines will be added.
	/// </summary>

	public Direction direction = Direction.Down;

	/// <summary>
	/// Whether the table's contents will be sorted alphabetically.
	/// </summary>

	public bool sorted = false;

	/// <summary>
	/// Whether inactive children will be discarded from the table's calculations.
	/// </summary>

	public bool hideInactive = true;

	/// <summary>
	/// Whether the parent container will be notified of the table's changes.
	/// </summary>

	public bool keepWithinPanel = false;

	/// <summary>
	/// Padding around each entry, in pixels.
	/// </summary>

	public Vector2 padding = Vector2.zero;

	/// <summary>
	/// Delegate function that will be called when the table repositions its content.
	/// </summary>

	public OnReposition onReposition;

	UIPanel mPanel;
	UIScrollView mDrag;
	bool mInitDone = false;
	bool mReposition = false;
	List<Transform> mChildren = new List<Transform>();

	/// <summary>
	/// Reposition the children on the next Update().
	/// </summary>

	public bool repositionNow { set { if (value) { mReposition = true; enabled = true; } } }

	/// <summary>
	/// Function that sorts items by name.
	/// </summary>

	static public int SortByName (Transform a, Transform b) { return string.Compare(a.name, b.name); }

	/// <summary>
	/// Returns the list of table's children, sorted alphabetically if necessary.
	/// </summary>

	public List<Transform> children
	{
		get
		{
			if (mChildren.Count == 0)
			{
				Transform myTrans = transform;
				mChildren.Clear();

				for (int i = 0; i < myTrans.childCount; ++i)
				{
					Transform child = myTrans.GetChild(i);

					if (child && child.gameObject && (!hideInactive || NGUITools.GetActive(child.gameObject))) mChildren.Add(child);
				}
				if (sorted) mChildren.Sort(SortByName);
			}
			return mChildren;
		}
	}

	/// <summary>
	/// Positions the grid items, taking their own size into consideration.
	/// </summary>

	void RepositionVariableSize (List<Transform> children)
	{
		float xOffset = 0;
		float yOffset = 0;

		int cols = columns > 0 ? children.Count / columns + 1 : 1;
		int rows = columns > 0 ? columns : children.Count;

		Bounds[,] bounds = new Bounds[cols, rows];
		Bounds[] boundsRows = new Bounds[rows];
		Bounds[] boundsCols = new Bounds[cols];

		int x = 0;
		int y = 0;

		for (int i = 0, imax = children.Count; i < imax; ++i)
		{
			Transform t = children[i];
			Bounds b = NGUIMath.CalculateRelativeWidgetBounds(t);

			Vector3 scale = t.localScale;
			b.min = Vector3.Scale(b.min, scale);
			b.max = Vector3.Scale(b.max, scale);
			bounds[y, x] = b;

			boundsRows[x].Encapsulate(b);
			boundsCols[y].Encapsulate(b);

			if (++x >= columns && columns > 0)
			{
				x = 0;
				++y;
			}
		}

		x = 0;
		y = 0;

		for (int i = 0, imax = children.Count; i < imax; ++i)
		{
			Transform t = children[i];
			Bounds b = bounds[y, x];
			Bounds br = boundsRows[x];
			Bounds bc = boundsCols[y];

			Vector3 pos = t.localPosition;
			pos.x = xOffset + b.extents.x - b.center.x;
			pos.x += b.min.x - br.min.x + padding.x;

			if (direction == Direction.Down)
			{
				pos.y = -yOffset - b.extents.y - b.center.y;
				pos.y += (b.max.y - b.min.y - bc.max.y + bc.min.y) * 0.5f - padding.y;
			}
			else
			{
				pos.y = yOffset + (b.extents.y - b.center.y);
				pos.y -= (b.max.y - b.min.y - bc.max.y + bc.min.y) * 0.5f - padding.y;
			}

			xOffset += br.max.x - br.min.x + padding.x * 2f;

			t.localPosition = pos;

			if (++x >= columns && columns > 0)
			{
				x = 0;
				++y;

				xOffset = 0f;
				yOffset += bc.size.y + padding.y * 2f;
			}
		}
	}

	/// <summary>
	/// Recalculate the position of all elements within the table, sorting them alphabetically if necessary.
	/// </summary>

	[ContextMenu("Execute")]
	public void Reposition ()
	{
		if (Application.isPlaying && !mInitDone)
		{
			mReposition = true;
			return;
		}

		if (!mInitDone) Init();

		mReposition = false;
		Transform myTrans = transform;
		mChildren.Clear();
		List<Transform> ch = children;
		if (ch.Count > 0) RepositionVariableSize(ch);

		if (mDrag != null)
		{
			mDrag.UpdateScrollbars(true);
			mDrag.RestrictWithinBounds(true);
		}
		else if (mPanel != null)
		{
			mPanel.ConstrainTargetToBounds(myTrans, true);
		}

		if (onReposition != null)
			onReposition();
	}

	/// <summary>
	/// Position the grid's contents when the script starts.
	/// </summary>

	void Start ()
	{
		Init();
		Reposition();
		enabled = false;
	}

	/// <summary>
	/// Find the necessary components.
	/// </summary>

	void Init ()
	{
		mInitDone = true;

		if (keepWithinPanel)
		{
			mPanel = NGUITools.FindInParents<UIPanel>(gameObject);
			mDrag = NGUITools.FindInParents<UIScrollView>(gameObject);
		}
	}

	/// <summary>
	/// Is it time to reposition? Do so now.
	/// </summary>

	void LateUpdate ()
	{
		if (mReposition) Reposition();
		enabled = false;
	}
}
