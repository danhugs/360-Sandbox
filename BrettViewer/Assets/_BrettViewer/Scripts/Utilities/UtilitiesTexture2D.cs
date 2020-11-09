using UnityEngine;

public static class UtilitiesTexture2D {

	public enum ImageFilterMode : int {
		Nearest = 0,
		Biliner = 1,
		Average = 2
	}

	/// <summary>
	/// Resize Texture2D source with filtering
	/// </summary>
	/// <param name="pSource"></param>
	/// <param name="width"></param>
	/// <param name="height"></param>
	/// <param name="pFilterMode"></param>
	/// <returns></returns>
	public static Texture2D Resize(Texture2D pSource, int width, int height, ImageFilterMode pFilterMode = ImageFilterMode.Average) { 
		//Based on http://blog.collectivemass.com/2014/03/resizing-textures-in-unity/


		//*** Variables
		int i;

		//*** Get All the source pixels
		Color[] sourceColors = pSource.GetPixels(0);
		Vector2 sourceSize = new Vector2(pSource.width, pSource.height);

		//*** Calculate New Size
		float xWidth = (float)width; //Mathf.RoundToInt((float)pSource.width * pScale);
		float xHeight = (float)height;// Mathf.RoundToInt((float)pSource.height * pScale);

		//*** Make New
		Texture2D textureNew = new Texture2D((int)xWidth, (int)xHeight, TextureFormat.RGBA32, false);

		//*** Make destination array
		int xLength = (int)xWidth * (int)xHeight;
		Color[] aColor = new Color[xLength];

		Vector2 vPixelSize = new Vector2(sourceSize.x / xWidth, sourceSize.y / xHeight);

		//*** Loop through destination pixels and process
		Vector2 vCenter = new Vector2();
		for (i = 0; i < xLength; i++) {

			//*** Figure out x&y
			float xX = (float)i % xWidth;
			float xY = Mathf.Floor((float)i / xWidth);

			//*** Calculate Center
			vCenter.x = (xX / xWidth) * sourceSize.x;
			vCenter.y = (xY / xHeight) * sourceSize.y;

			//*** Do Based on mode
			//*** Nearest neighbour (testing)
			if (pFilterMode == ImageFilterMode.Nearest) {

				//*** Nearest neighbour (testing)
				vCenter.x = Mathf.Round(vCenter.x);
				vCenter.y = Mathf.Round(vCenter.y);

				//*** Calculate source index
				int xSourceIndex = (int)((vCenter.y * sourceSize.x) + vCenter.x);

				//*** Copy Pixel
				aColor[i] = sourceColors[xSourceIndex];
			}

			//*** Bilinear
			else if (pFilterMode == ImageFilterMode.Biliner) {

				//*** Get Ratios
				float xRatioX = vCenter.x - Mathf.Floor(vCenter.x);
				float xRatioY = vCenter.y - Mathf.Floor(vCenter.y);

				//*** Get Pixel index's
				int xIndexTL = (int)((Mathf.Floor(vCenter.y) * sourceSize.x) + Mathf.Floor(vCenter.x));
				int xIndexTR = (int)((Mathf.Floor(vCenter.y) * sourceSize.x) + Mathf.Ceil(vCenter.x));
				int xIndexBL = (int)((Mathf.Ceil(vCenter.y) * sourceSize.x) + Mathf.Floor(vCenter.x));
				int xIndexBR = (int)((Mathf.Ceil(vCenter.y) * sourceSize.x) + Mathf.Ceil(vCenter.x));

				//*** Calculate Color
				aColor[i] = Color.Lerp(
					Color.Lerp(sourceColors[xIndexTL], sourceColors[xIndexTR], xRatioX),
					Color.Lerp(sourceColors[xIndexBL], sourceColors[xIndexBR], xRatioX),
					xRatioY
				);
			}

			//*** Average
			else if (pFilterMode == ImageFilterMode.Average) {

				//*** Calculate grid around point
				int xXFrom = (int)Mathf.Max(Mathf.Floor(vCenter.x - (vPixelSize.x * 0.5f)), 0);
				int xXTo = (int)Mathf.Min(Mathf.Ceil(vCenter.x + (vPixelSize.x * 0.5f)), sourceSize.x);
				int xYFrom = (int)Mathf.Max(Mathf.Floor(vCenter.y - (vPixelSize.y * 0.5f)), 0);
				int xYTo = (int)Mathf.Min(Mathf.Ceil(vCenter.y + (vPixelSize.y * 0.5f)), sourceSize.y);

				//*** Loop and accumulate
				//Vector4 oColorTotal = new Vector4();
				Color oColorTemp = new Color();
				float xGridCount = 0;
				for (int iy = xYFrom; iy < xYTo; iy++) {
					for (int ix = xXFrom; ix < xXTo; ix++) {

						//*** Get Color
						oColorTemp += sourceColors[(int)(((float)iy * sourceSize.x) + ix)];

						//*** Sum
						xGridCount++;
					}
				}

				//*** Average Color
				aColor[i] = oColorTemp / (float)xGridCount;
			}
		}

		//*** Set Pixels
		textureNew.SetPixels(aColor);
		textureNew.Apply();

		//*** Return
		return textureNew;
	}

}