﻿using Es.Effective;
using Es.TexturePaint;
using Es.TexturePaint.Sample;
using System.Collections;
using UnityEngine;

public class ClipTest : MonoBehaviour
{
	[SerializeField]
	private bool grab = true;

	[SerializeField]
	private PaintBrush brush = null;

	private RenderTexture t = null;
	private RaycastHit hitInfo;

	private void OnGUI()
	{
		GUI.Box(new Rect(0, 0, 300, 320), "");
		GUI.Box(new Rect(0, 0, 300, 300), "Grab Texture");
		if(t != null)
			GUI.DrawTexture(new Rect(0, 0, 300, 300), t);
		grab = GUI.Toggle(new Rect(0, 300, 300, 20), grab, "Grab");
	}

	public void Awake()
	{
		t = new RenderTexture(brush.BrushTexture.width, brush.BrushTexture.height, 0);
	}

	private void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hitInfo))
			{
				var d = hitInfo.transform.GetComponent<DynamicCanvas>();
				if(d != null && !grab)
				{
					d.Paint(brush, hitInfo);
				}
				if(grab)
				{
					GrabArea.Clip(brush.BrushTexture, brush.Scale, hitInfo.transform.GetComponent<MeshRenderer>().sharedMaterial.mainTexture, hitInfo.textureCoord, t);
					brush.BrushTexture = t;
					brush.ColorBlending = PaintBrush.ColorBlendType.UseBrush;
					grab = false;
				}
			}
		}
	}
}