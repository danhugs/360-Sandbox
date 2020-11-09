using UnityEditor;
using UnityEngine;

public class PreprocessorSprites : AssetPostprocessor {
	void OnPreprocessTexture() {


		Debug.Log("Import: " + TextureImporter.assetPath);

		if (TextureImporter.assetPath.Contains ("Resources/share_")) {
			TextureImporter.isReadable = true;
			TextureImporter.textureCompression = TextureImporterCompression.Uncompressed;
			TextureImporter.mipmapEnabled = false;
			TextureImporter.npotScale = TextureImporterNPOTScale.None;

		}else if (TextureImporter.assetPath.Contains("/Assets/Sprites/")) {
			Debug.Log("Import sprite...");
			TextureImporter.textureType = TextureImporterType.Sprite;
			TextureImporter.spriteImportMode = SpriteImportMode.Single;
			TextureImporter.spritePixelsPerUnit = 1;
			TextureImporter.alphaIsTransparency = TextureImporter.DoesSourceTextureHaveAlpha();
			TextureImporter.spritePackingTag = "ui";
			//TextureImporter.spriteBorder = 4;
		}

		if (TextureImporter.assetPath.Contains("/Assets/Icons/")) {
			Debug.Log("Import Icon...");
			TextureImporter.textureType = TextureImporterType.Default;
			TextureImporter.textureCompression = TextureImporterCompression.Uncompressed;
			TextureImporter.npotScale = TextureImporterNPOTScale.None;
			//TextureImporter.spriteBorder = 4;
		}

		if (TextureImporter.assetPath.Contains("/Editor/Vuforia/")) {
			Debug.Log("Import Tracking image");
			TextureImporter.textureShape = TextureImporterShape.Texture2D;
		}

	}

	protected TextureImporter TextureImporter {
		get {
			return (TextureImporter)assetImporter;
		}
	}
}